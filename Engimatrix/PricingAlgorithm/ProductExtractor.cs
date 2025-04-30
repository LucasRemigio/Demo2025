// // Copyright (c) 2024 Engibots. All rights reserved.

using engimatrix.ModelObjs;
using Engimatrix.ModelObjs;
using engimatrix.Utils;
using engimatrix.Connector;

namespace engimatrix.PricingAlgorithm;

public static class ProductExtractor
{
    public static async Task<List<ProdutoComparado>> ExtractProductsFromEmailOpenAI(string emailBody)
    {
        // Step 1: Extract entities from the email
        List<ProdutoRaw> produtosRaw = await OpenAI.ExtractEntitiesFromEmail(emailBody);
        if (!Util.HasValidData(produtosRaw, "No extracted products from email")) { return []; }

        // Step 2: Get the product types (Barra, Chapa, Viga, etc...)
        List<ProdutoComTipo> productsWithType = await OpenAI.GetProductsTypes(produtosRaw);
        if (!Util.HasValidData(productsWithType, "No product types found")) { return []; }

        // Add product types to raw products and clean up the list
        AddProductTypeToRawProductsAndCleanUp(produtosRaw, productsWithType);

        // Step 3: Classify the products
        List<Produto> produtosClassified = await OpenAI.ClassifyProducts(produtosRaw);
        if (!Util.HasValidData(produtosClassified, "Couldn't classify the products")) { return []; }

        // Replace Produto dimensions with produtosRaw dimensions
        foreach (Produto produto in produtosClassified)
        {
            ProdutoRaw? produtoRaw = produtosRaw.FirstOrDefault(p => p.Id == produto.Id);
            if (produtoRaw != null)
            {
                produto.MedidasProduto = produtoRaw.Medidas;
                produto.CaracteristicasProduto.Dimensoes.Altura = produtoRaw.Altura;
                produto.CaracteristicasProduto.Dimensoes.Comprimento = produtoRaw.Comprimento;
                produto.CaracteristicasProduto.Dimensoes.Largura = produtoRaw.Largura;
            }
        }

        // Step 4: Match products with the catalog
        (List<ProdutoComparado> instantMatchedProducts, List<ProductCatalogDTO> similarProducts) = ProductMatcher.MatchProductsWithCatalog(produtosClassified);
        // Check for valid similar products
        if (!Util.HasValidData(similarProducts, "No similar products found in the catalog")
            && !Util.HasValidData(instantMatchedProducts, "No instant matched products")) { return []; }

        if (similarProducts.Count > 2500)
        {
            Log.Debug($"Invalid similar product list size: Too many products - {similarProducts.Count}");
            return [];
        }

        // Step 5: Validate matches with OpenAI
        List<ProdutoComparado> finalMatchedProducts = [];
        if (Util.HasValidData(similarProducts, "No similar products, every singled product was instantly matched"))
        {
            finalMatchedProducts = await ValidateProductMatchesInChunks(produtosClassified, similarProducts);
        }

        // add the instantly matched products to the final list
        finalMatchedProducts.AddRange(instantMatchedProducts);

        finalMatchedProducts.Sort((a, b) => Convert.ToInt16(a.IdProdutoSolicitado).CompareTo(Convert.ToInt16(b.IdProdutoSolicitado)));

        // DEBUG: Log raw products and matched catalog entries
        ProductHelper.DebugLoggingOriginalProductsAndFinalMatches(produtosRaw, finalMatchedProducts, instantMatchedProducts);

        return finalMatchedProducts;
    }


    public static void AddProductTypeToRawProductsAndCleanUp(List<ProdutoRaw> produtosRaw, List<ProdutoComTipo> productsWithType)
    {
        // dictionary that associated the product id with the tipo produto and tipo produto id
        Dictionary<int, (string TipoProduto, int TipoProdutoId)> productsWithTypeDict =
                productsWithType.ToDictionary(
                    product => product.Id,
                    product => (product.TipoProduto, product.TipoProdutoId)
                );

        // Match and update TipoProduto
        foreach (ProdutoRaw produto in produtosRaw)
        {
            if (!productsWithTypeDict.TryGetValue(produto.Id, out var tipoProduto))
            {
                // product is not found in the dictionary
                continue; // Skip to the next iteration
            }

            // If the product is found, update its properties
            produto.TipoProduto = tipoProduto.TipoProduto;
            produto.TipoProdutoId = tipoProduto.TipoProdutoId;
        }

        // Remove items classified as "Diversos"
        produtosRaw.RemoveAll(produto => produto.TipoProduto.Equals("Diversos", StringComparison.OrdinalIgnoreCase));
    }

    public static async Task<List<ProdutoComparado>> ValidateProductMatchesInChunks(
        List<Produto> produtos,
        List<ProductCatalogDTO> allMostSimilarProductCatalogList
    ) => await ValidateProductMatchesInChunks(produtos, allMostSimilarProductCatalogList, 300);

    public static async Task<List<ProdutoComparado>> ValidateProductMatchesInChunks(
        List<Produto> produtos,
        List<ProductCatalogDTO> allMostSimilarProductCatalogList,
        int maxChunkSize
    )
    {
        List<ProdutoComparado> allMatchedProducts = [];
        int totalProducts = allMostSimilarProductCatalogList.Count;

        // In this function we will limit the amount of items sending to the OpenAI as when we send too many items (400-500+) the api never
        // answers back, so we need to split the items in chunks and send them in parts
        // For this, on the first callback, we need to add the response to a list even if it has no matches, so that on the subsequent calls,
        // if any product has an higher confidence match, we replace the current with the highest confidence

        Log.Info($"ValidateProductMatchesInChunks: Number of most similar products for matching: {totalProducts}");
        int chunkSize = Util.GetOptimalChunkSize(maxChunkSize, totalProducts);

        // Create tasks for each chunk
        List<Task<List<ProdutoComparado>>> tasks = [];

        for (int i = 0; i < totalProducts; i += chunkSize)
        {
            List<ProductCatalogDTO> currentMostSimilarProductsChunk = allMostSimilarProductCatalogList.Skip(i).Take(chunkSize).ToList();
            Log.Info($"ValidateProductMatchesInChunks: Number of products sent in current chunk: {currentMostSimilarProductsChunk.Count}");

            // Create a task for each chunk
            tasks.Add(OpenAI.ValidateProductMatches(produtos, currentMostSimilarProductsChunk));
        }

        // Wait for all tasks to complete
        List<ProdutoComparado>[] allResults;
        try
        {
            allResults = await Task.WhenAll(tasks);
        }
        catch (Exception ex)
        {
            Log.Error($"Error occurred during chunk processing: {ex.Message}");
            allResults = tasks.Where(t => t.IsCompletedSuccessfully)
                              .Select(t => t.Result)
                              .ToArray();
        }

        // Merge results from all tasks
        for (int i = 0; i < allResults.Length; i++)
        {
            List<ProdutoComparado> currentMatches = allResults[i];

            // If it's the first batch, add all matches; otherwise, merge based on confidence
            MergeMatches(allMatchedProducts, currentMatches, i == 0);
        }

        return allMatchedProducts;
    }

    /// <summary>
    /// Merges the current batch of matches into the existing match list.
    /// Ensures higher confidence matches replace lower confidence ones.
    /// </summary>
    private static void MergeMatches(
        List<ProdutoComparado> existingMatches,
        List<ProdutoComparado> currentMatches,
        bool isFirstBatch)
    {
        foreach (ProdutoComparado currentProduct in currentMatches)
        {
            ProdutoComparado? existingProduct = existingMatches
                .FirstOrDefault(p => p.IdProdutoSolicitado == currentProduct.IdProdutoSolicitado);

            if (isFirstBatch || existingProduct == null)
            {
                // Add new match during the first batch or if no existing match is found
                existingMatches.Add(currentProduct);
                continue;
            }

            // Compare confidence and update only if the new match has higher confidence
            int currentConfidence = int.Parse(currentProduct.Confianca);
            int existingConfidence = int.Parse(existingProduct.Confianca);

            if (currentConfidence == 100 || currentConfidence > existingConfidence)
            {
                existingMatches.Remove(existingProduct);
                existingMatches.Add(currentProduct);
            }
        }
    }
}