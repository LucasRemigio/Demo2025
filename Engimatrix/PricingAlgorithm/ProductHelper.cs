// // Copyright (c) 2024 Engibots. All rights reserved.

using engimatrix.ModelObjs;
using System.Text.RegularExpressions;
using engimatrix.Connector;
using Engimatrix.ModelObjs;
using engimatrix.Utils;
using System.Text;

namespace engimatrix.PricingAlgorithm;

public static class ProductHelper
{
    public static string CleanDimension(string? dimension)
    {
        if (string.IsNullOrEmpty(dimension) || dimension == null)
        {
            return string.Empty;
        }
        dimension = dimension.Replace(',', '.');
        return Regex.Replace(dimension, "[^0-9.]", "");
    }


    public static List<ProductCatalogDTO> GetCommonProductsByFilteringSequencially(
        List<ProductCatalogDTO> productsWithWidthFiltered,
        List<ProductCatalogDTO> productsWithHeightFiltered,
        List<ProductCatalogDTO> productsWithLengthFiltered,
        List<ProductCatalogDTO> productsWithThicknessFiltered,
        List<ProductCatalogDTO> productsWithDiameterFiltered
    )
    {
        // Start with all products
        List<ProductCatalogDTO>? currentList = null;

        // List of filters to apply, skipping empty lists
        List<List<ProductCatalogDTO>> filters =
        [
            productsWithWidthFiltered,
            productsWithHeightFiltered,
            productsWithLengthFiltered,
            productsWithThicknessFiltered,
            productsWithDiameterFiltered
        ];

        foreach (List<ProductCatalogDTO> filter in filters)
        {
            if (filter.Count == 0) { continue; } // Skip empty lists

            // If no current list, initialize it
            currentList ??= [.. filter];

            // Intersect current list with the next filter
            currentList = currentList.Intersect(filter).Distinct().ToList();

            // If the filtered list is not empty, return it
            if (currentList.Count == 0 || currentList.Count == 1)
            {
                break;
            }
        }

        if (currentList?.Count >= 1)
        {
            return currentList;
        }

        // If no intersection was found, return the the concatenation of all lists
        return filters
            .Where(list => list.Count > 0) // Consider only non-empty lists
            .Aggregate(new List<ProductCatalogDTO>(), (acc, list) => [.. acc, .. list])
            .Distinct()
            .ToList();
    }

    public static List<ProductCatalogDTO> FilterProductCatalogList(List<ProductCatalogDTO> currentProductsList, string? filter, Func<ProductCatalogDTO, string?>? propertySelector)
    {
        if (string.IsNullOrEmpty(filter) || filter == null)
        {
            return currentProductsList;
        }

        if (propertySelector == null)
        {
            return currentProductsList;
        }

        List<ProductCatalogDTO> previousList = currentProductsList;

        currentProductsList = currentProductsList
                .Where(x => string.Equals(propertySelector(x), filter, StringComparison.OrdinalIgnoreCase))
                .ToList();

        // Fall back if nothing is found
        if (currentProductsList.Count == 0)
        {
            currentProductsList = previousList;
        }

        return currentProductsList;
    }

    public static List<ProductCatalogDTO> FilterProductCatalogListByDimensions(
         List<ProductCatalogDTO> currentProductsList,
         string? filter,
         Func<ProductCatalogDTO, decimal?> propertySelector
    )
    {
        if (string.IsNullOrEmpty(filter) || filter == null)
        {
            return [];
        }

        List<ProductCatalogDTO> previousList = currentProductsList;

        // First convert filter string to decimal with try parse for error handling
        if (!decimal.TryParse(filter, out decimal filterDecimal))
        {
            return [];
        }

        currentProductsList = currentProductsList
            .Where(x => propertySelector(x).HasValue && propertySelector(x).Value == filterDecimal)
            .ToList();

        // Fall back if nothing is found
        if (currentProductsList.Count != 0)
        {
            return currentProductsList;
        }

        // If no exact match is found, try to find a close match
        decimal tolerance = filterDecimal * 0.1m;

        currentProductsList = previousList
            .Where(x => propertySelector(x).HasValue && Math.Abs(propertySelector(x).Value - filterDecimal) <= tolerance)
            .ToList();

        return currentProductsList;
    }

    public static void DebugLoggingOriginalProductsAndFinalMatches(
        List<ProdutoRaw> produtosRaw,
        List<ProdutoComparado> finalMatchedProducts,
        List<ProdutoComparado> instantMatchedProducts
    )
    {
        // Initialize StringBuilder for log accumulation
        StringBuilder logBuilder = new();
        logBuilder.AppendLine("\n");

        if (Util.HasValidData(instantMatchedProducts, "No instantly matched products"))
        {
            logBuilder.AppendLine($"# Instantly matched products: {instantMatchedProducts.Count}");
        }

        // Sort product raw by id
        produtosRaw.Sort((a, b) => Convert.ToInt16(a.Id).CompareTo(Convert.ToInt16(b.Id)));

        logBuilder.AppendLine("\n# Original products:");
        foreach (ProdutoRaw produto in produtosRaw)
        {
            logBuilder.AppendLine($"#{produto.Id}. {produto.ProdutoSolicitado} - {produto.Medidas}");
        }

        // finalMatchedProducts are already sorted by IdProdutoSolicitado
        logBuilder.AppendLine("# Final matched products:");
        foreach (ProdutoComparado product in finalMatchedProducts)
        {
            logBuilder.AppendLine($"#{product.IdProdutoSolicitado}. ({product.IsMatchInstantaneo}) {product.NomeProdutoCatalogo} - {product.IdProdutoCatalogo}");
        }

        // Write logs to a file
        string directoryPath = Path.Combine("Tests", "Results");
        if (!Directory.Exists(directoryPath))
        {
            Directory.CreateDirectory(directoryPath);
        }
        string filePath = Path.Combine(directoryPath, "debug_results.txt");

        File.WriteAllText(filePath, logBuilder.ToString());

        Log.Debug(logBuilder.ToString());
    }

    private static List<ProductCatalogDTO> GetCommonProductsInMostLists(
        List<ProductCatalogDTO> productsWithWidthFiltered,
        List<ProductCatalogDTO> productsWithHeightFiltered,
        List<ProductCatalogDTO> productsWithLengthFiltered,
        List<ProductCatalogDTO> productsWithThicknessFiltered,
        List<ProductCatalogDTO> productsWithDiameterFiltered
    )
    {
        // Combine all lists into a single collection with counts
        IEnumerable<ProductCatalogDTO> allProducts = productsWithWidthFiltered
            .Concat(productsWithHeightFiltered)
            .Concat(productsWithLengthFiltered)
            .Concat(productsWithThicknessFiltered)
            .Concat(productsWithDiameterFiltered);

        // Count occurrences of each product
        Dictionary<ProductCatalogDTO, int> productScores = allProducts
            .GroupBy(product => product)
            .ToDictionary(group => group.Key, group => group.Count());

        // Find the maximum score
        int maxScore = productScores.Values.Max();

        // Return products that have the highest score (appear in the most lists)
        return productScores
            .Where(pair => pair.Value == maxScore)
            .Select(pair => pair.Key)
            .ToList();
    }
}