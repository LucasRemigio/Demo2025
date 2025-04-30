// // Copyright (c) 2024 Engibots. All rights reserved.

using engimatrix.ModelObjs;
using Engimatrix.ModelObjs;
using engimatrix.Models;
using engimatrix.PricingAlgorithm;
using engimatrix.Utils;

namespace engimatrix.PricingAlgorithm;

public static class ProductMatcher
{
    public static (List<ProdutoComparado>, List<ProductCatalogDTO>) MatchProductsWithCatalog(List<Produto> produtos)
    {
        List<ProductCatalogDTO> productCatalogList = ProductCatalogModel.GetProductCatalogsDTOForPricing("System", false);

        Log.Debug($"ProductCatalogModel.GetProductCatalogsDTOForPricing returned {productCatalogList.Count} products.");

        List<ProductCatalogDTO> allMostSimilarProductCatalogList = [];
        List<ProdutoComparado> matchedProducts = [];
        HashSet<int> productsIdToRemove = [];

        // Nuno refered that the match should have in mind the stock current
        productCatalogList.RemoveAll(x => x.stock_current == 0);

        foreach (Produto produto in produtos)
        {
            List<ProductCatalogDTO> currentProductMostSimilarProductCatalogList = GetMostSimilarProducts(productCatalogList, produto);

            // Instant product match
            if (currentProductMostSimilarProductCatalogList.Count == 1)
            {
                ProdutoComparado produtoComparado = new()
                {
                    IdProdutoCatalogo = currentProductMostSimilarProductCatalogList[0].id.ToString(),
                    NomeProdutoCatalogo = currentProductMostSimilarProductCatalogList[0].description_full,
                    NomeProdutoSolicitado = produto.produtoSolicitado ?? string.Empty,
                    IdProdutoSolicitado = produto.Id.ToString(),
                    MedidasProdutoSolicitado = produto.MedidasProduto ?? string.Empty,
                    MedidasProdutoCatalogo = currentProductMostSimilarProductCatalogList[0].description_full,
                    Quantidade = produto.Quantidade ?? "1",
                    UnidadeQuantidade = produto.UnidadeQuantidade ?? "un",
                    Confianca = "100",
                    Justificacao = "Produto encontrado na base de dados",
                    IsMatchInstantaneo = true
                };

                matchedProducts.Add(produtoComparado);
                productsIdToRemove.Add(produto.Id);
                continue;
            }

            // concatenate all lists of potential products together
            foreach (ProductCatalogDTO product in currentProductMostSimilarProductCatalogList)
            {
                if (!allMostSimilarProductCatalogList.Any(x => x.id == product.id))
                {
                    allMostSimilarProductCatalogList.Add(product);
                }
            }
        }
        // Remove instantly matched products
        produtos.RemoveAll(x => productsIdToRemove.Contains(x.Id));

        return (matchedProducts, allMostSimilarProductCatalogList);
    }

    public static List<ProductCatalogDTO> GetMostSimilarProducts(List<ProductCatalogDTO> productCatalogList, Produto produto)
    {
        // On this function, we will have a different approach. Instead of checking for the product description, we will receive the Type, Material, Shape and Finishing
        // Now the database has those columns, so instead of matching with the description, we will match field by field.
        List<ProductCatalogDTO> currentProductMostSimilarProductCatalogList = productCatalogList;

        /*
         *  SECTION 1 - FILTER BY PROPERTIES
         */
        List<ProductCatalogDTO> productsWithCharacteristicsFiltered = FilterProductsByCharacteristics(currentProductMostSimilarProductCatalogList, produto);

        /*
        *  SECTION 2 - FILTER BY DIMENSIONS
        */
        List<ProductCatalogDTO> productsWithDimensionsFiltered = FilterProductsByDimensions(productsWithCharacteristicsFiltered, produto);

        return productsWithDimensionsFiltered;
    }

    private static List<ProductCatalogDTO> FilterProductsByCharacteristics(List<ProductCatalogDTO> productCatalogList, Produto produto)
    {
        productCatalogList = ProductHelper.FilterProductCatalogList(productCatalogList, produto.CaracteristicasProduto?.TipoDeProduto, x => x.type?.name);
        string material = produto.CaracteristicasProduto?.TipoMaterial ?? "";
        if (!material.Equals("ferro", StringComparison.OrdinalIgnoreCase) && !material.Equals("aço", StringComparison.OrdinalIgnoreCase))
        {
            productCatalogList = ProductHelper.FilterProductCatalogList(productCatalogList, produto.CaracteristicasProduto?.TipoMaterial, x => x.material?.name);
        }
        productCatalogList = ProductHelper.FilterProductCatalogList(productCatalogList, produto.CaracteristicasProduto?.FormaProduto, x => x.shape?.name);
        productCatalogList = ProductHelper.FilterProductCatalogList(productCatalogList, produto.CaracteristicasProduto?.FinalizacaoProduto, x => x.finishing?.name);
        productCatalogList = ProductHelper.FilterProductCatalogList(productCatalogList, produto.CaracteristicasProduto?.SuperficieProduto, x => x.surface?.name);

        return productCatalogList;
    }


    private static List<ProductCatalogDTO> FilterProductsByDimensions(List<ProductCatalogDTO> productCatalogList, Produto produto)
    {
        // clean up the dimensions (length, width, diameter) in case they have mm or m or any measurment. only numbers allowed
        produto.CaracteristicasProduto.Dimensoes.Altura = ProductHelper.CleanDimension(produto.CaracteristicasProduto.Dimensoes.Altura);
        produto.CaracteristicasProduto.Dimensoes.Comprimento = ProductHelper.CleanDimension(produto.CaracteristicasProduto.Dimensoes.Comprimento);
        produto.CaracteristicasProduto.Dimensoes.Largura = ProductHelper.CleanDimension(produto.CaracteristicasProduto.Dimensoes.Largura);
        produto.CaracteristicasProduto.Dimensoes.Espessura = ProductHelper.CleanDimension(produto.CaracteristicasProduto.Dimensoes.Espessura);
        produto.CaracteristicasProduto.Dimensoes.Diametro = ProductHelper.CleanDimension(produto.CaracteristicasProduto.Dimensoes.Diametro);

        // Filter by dimensions. Use the original dimensions first.
        List<ProductCatalogDTO> productsWithWidthFiltered = ProductHelper.FilterProductCatalogListByDimensions(productCatalogList, produto.CaracteristicasProduto.Dimensoes.Largura, x => x.width);
        List<ProductCatalogDTO> productsWithHeightFiltered = ProductHelper.FilterProductCatalogListByDimensions(productCatalogList, produto.CaracteristicasProduto.Dimensoes.Altura, x => x.height);
        List<ProductCatalogDTO> productsWithLengthFiltered = ProductHelper.FilterProductCatalogListByDimensions(productCatalogList, produto.CaracteristicasProduto.Dimensoes.Comprimento, x => x.length);
        List<ProductCatalogDTO> productsWithThicknessFiltered = ProductHelper.FilterProductCatalogListByDimensions(productCatalogList, produto.CaracteristicasProduto.Dimensoes.Espessura, x => x.thickness);
        List<ProductCatalogDTO> productsWithDiameterFiltered = ProductHelper.FilterProductCatalogListByDimensions(productCatalogList, produto.CaracteristicasProduto.Dimensoes.Diametro, x => x.diameter);

        List<ProductCatalogDTO> productsWithDimensionsFiltered = ProductHelper.GetCommonProductsByFilteringSequencially(
            productsWithWidthFiltered,
            productsWithHeightFiltered,
            productsWithLengthFiltered,
            productsWithThicknessFiltered,
            productsWithDiameterFiltered);

        // Also consider alternate dimension mappings.
        List<ProductCatalogDTO> productsWithWidthInvertedFiltered = ProductHelper.FilterProductCatalogListByDimensions(productCatalogList, produto.CaracteristicasProduto.Dimensoes.Largura, x => x.height);
        List<ProductCatalogDTO> productsWithHeightInvertedFiltered = ProductHelper.FilterProductCatalogListByDimensions(productCatalogList, produto.CaracteristicasProduto.Dimensoes.Altura, x => x.width);
        List<ProductCatalogDTO> productsWithDiameterCheckedFiltered = [];
        if (produto.CaracteristicasProduto.Dimensoes.Largura.Equals(produto.CaracteristicasProduto.Dimensoes.Altura, StringComparison.OrdinalIgnoreCase))
        {
            productsWithDiameterCheckedFiltered = ProductHelper.FilterProductCatalogListByDimensions(productCatalogList, produto.CaracteristicasProduto.Dimensoes.Largura, x => x.diameter);
        }

        List<ProductCatalogDTO> productsWithChangedDimensionsFiltered = ProductHelper.GetCommonProductsByFilteringSequencially(
            productsWithWidthInvertedFiltered,
            productsWithHeightInvertedFiltered,
            productsWithLengthFiltered,
            productsWithThicknessFiltered,
            productsWithDiameterFiltered);

        // return the original product list if both lists are empty
        if (productsWithDimensionsFiltered.Count == 0 && productsWithChangedDimensionsFiltered.Count == 0)
        {
            return productCatalogList;
        }

        // Decide which list to return
        if (productsWithDimensionsFiltered.Count == 1 && productsWithChangedDimensionsFiltered.Count == 1)
        {
            // Combine both lists if both have exactly one item
            return productsWithDimensionsFiltered.Concat(productsWithChangedDimensionsFiltered).Distinct().ToList();
        }

        if (productsWithDimensionsFiltered.Count == 1)
        {
            return productsWithDimensionsFiltered;
        }

        // return combination of both lists if both have more than 1
        if (productsWithDimensionsFiltered.Count > 1 && productsWithChangedDimensionsFiltered.Count >= 1)
        {
            return productsWithDimensionsFiltered.Concat(productsWithChangedDimensionsFiltered).Distinct().ToList();
        }

        // Fallback: Return the smallest original dimension-filtered list
        return new[] { productsWithWidthFiltered, productsWithHeightFiltered, productsWithLengthFiltered }
            .OrderBy(list => list.Count).Distinct().ToList()
            .FirstOrDefault() ?? [];
    }

}
