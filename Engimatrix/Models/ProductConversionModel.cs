// // Copyright (c) 2024 Engibots. All rights reserved.

using System.Diagnostics;
using engimatrix.Exceptions;
using engimatrix.ModelObjs;
using engimatrix.ModelObjs.Primavera;
using engimatrix.PricingAlgorithm;
using engimatrix.Utils;
using engimatrix.Views;
using Microsoft.Extensions.Azure;

namespace engimatrix.Models;

public static class ProductConversionModel
{
    public static string GetBaseQueryProductConversionDto()
    {
        string query = $"SELECT pc.id, pc.product_code, pc.origin_unit_id, pc.end_unit_id, pc.rate, " +
                "ou.abbreviation AS origin_unit_abbreviation, ou.name AS origin_unit_name, ou.slug AS origin_unit_slug, " +
                "eu.abbreviation AS end_unit_abbreviation, eu.name AS end_unit_name, eu.slug AS end_unit_slug " +
            "FROM product_conversion pc " +
                "INNER JOIN product_unit ou ON pc.origin_unit_id = ou.id " +
                "INNER JOIN product_unit eu ON pc.end_unit_id = eu.id " +
            "WHERE 1=1 ";

        return query;
    }
    public static List<ProductConversionItem> GetProductConversions(string execute_user)
    {
        string query = $"SELECT id, product_code, origin_unit_id, end_unit_id, rate FROM product_conversion";

        SqlExecuterItem response = SqlExecuter.ExecuteFunction(query, [], execute_user, false, "getProductConversions");

        if (!response.operationResult)
        {
            throw new DatabaseException("Error geting the product conversions");
        }

        if (response.out_data.Count <= 0)
        {
            return [];
        }

        List<ProductConversionItem> productConversions = [];

        foreach (Dictionary<string, string> item in response.out_data)
        {
            int originUnitId = Convert.ToInt32(item["origin_unit_id"]);
            int endUnitId = Convert.ToInt32(item["end_unit_id"]);
            ProductConversionItem productConversion = new ProductConversionItemBuilder()
                .SetId(Convert.ToInt32(item["id"]))
                .SetProductCode(item["product_code"])
                .SetOriginUnitId(originUnitId)
                .SetEndUnitId(endUnitId)
                .SetRate(Convert.ToDecimal(item["rate"]))
                .Build();

            productConversions.Add(productConversion);
        }

        return productConversions;
    }

    public static Dictionary<string, List<ProductConversionDTO>> GetProductConversionsDtoDicByProductCode(string execute_user)
    {
        List<ProductConversionDTO> productConversions = GetProductConversionsDTO(execute_user);

        Dictionary<string, List<ProductConversionDTO>> productConversionsDic = [];

        foreach (ProductConversionDTO productConversion in productConversions)
        {
            // If the product_code is already in the dictionary, add the conversion to the existing list
            if (productConversionsDic.TryGetValue(productConversion.product_code, out List<ProductConversionDTO>? value))
            {
                value.Add(productConversion);
            }
            else
            {
                // If the product_code is not in the dictionary, create a new list and add the conversion
                productConversionsDic[productConversion.product_code] = [productConversion];
            }
        }

        return productConversionsDic;
    }

    public static List<ProductConversionDTO> GetProductConversionsDTO(string execute_user)
    {
        string query = $"SELECT pc.id, pc.product_code, pc.origin_unit_id, pc.end_unit_id, pc.rate, " +
                "ou.abbreviation AS origin_unit_abbreviation, ou.name AS origin_unit_name, ou.slug AS origin_unit_slug, " +
                "eu.abbreviation AS end_unit_abbreviation, eu.name AS end_unit_name, eu.slug AS end_unit_slug, " +
                "pcg.description_full AS description_full " +
            "FROM product_conversion pc " +
                "INNER JOIN product_unit ou ON pc.origin_unit_id = ou.id " +
                "INNER JOIN product_unit eu ON pc.end_unit_id = eu.id " +
                "JOIN t_product_catalog pcg ON pc.product_code = pcg.product_code " +
            "ORDER BY pc.product_code";

        SqlExecuterItem response = SqlExecuter.ExecuteFunction(query, [], execute_user, false, "getProductConversionsDTO");

        if (!response.operationResult)
        {
            throw new DatabaseException("Error getting the product conversions");
        }

        if (response.out_data.Count <= 0)
        {
            return [];
        }

        List<ProductConversionDTO> productConversions = [];
        foreach (Dictionary<string, string> item in response.out_data)
        {
            int originUnitId = Convert.ToInt32(item["origin_unit_id"]);
            int endUnitId = Convert.ToInt32(item["end_unit_id"]);

            ProductUnitItem originUnit = new ProductUnitItemBuilder()
                .SetId(originUnitId)
                .SetAbbreviation(item["origin_unit_abbreviation"])
                .SetName(item["origin_unit_name"])
                .SetSlug(item["origin_unit_slug"])
                .Build();

            ProductUnitItem endUnit = new ProductUnitItemBuilder()
                .SetId(endUnitId)
                .SetAbbreviation(item["end_unit_abbreviation"])
                .SetName(item["end_unit_name"])
                .SetSlug(item["end_unit_slug"])
                .Build();

            ProductConversionDTO productConversion = new ProductConversionDTOBuilder()
                .SetId(Convert.ToInt32(item["id"]))
                .SetProductCode(item["product_code"])
                .SetOriginUnit(originUnit)
                .SetEndUnit(endUnit)
                .SetRate(Convert.ToDecimal(item["rate"]))
                .SetProductCatalogDescription(item["description_full"])
                .Build();

            productConversions.Add(productConversion);
        }

        return productConversions;
    }

    public static List<ProductConversionDTO> GetProductConversionsDTOByCodes(List<string> productCodes, string execute_user)
    {
        if (productCodes.Count <= 0)
        {
            return [];
        }

        Dictionary<string, string> parameters = [];

        List<string> codePlaceholders = [];
        for (int i = 0; i < productCodes.Count; i++)
        {
            string paramName = $"@code{i}";
            codePlaceholders.Add(paramName);
            parameters.Add(paramName, productCodes[i]);
        }

        // Join the placeholders with commas to form the IN clause.
        string inClause = string.Join(",", codePlaceholders);

        string query = $"SELECT pc.id, pc.product_code, pc.origin_unit_id, pc.end_unit_id, pc.rate, " +
                "ou.abbreviation AS origin_unit_abbreviation, ou.name AS origin_unit_name, ou.slug AS origin_unit_slug, " +
                "eu.abbreviation AS end_unit_abbreviation, eu.name AS end_unit_name, eu.slug AS end_unit_slug, " +
                "pcg.description_full AS description_full " +
            "FROM product_conversion pc " +
                "INNER JOIN product_unit ou ON pc.origin_unit_id = ou.id " +
                "INNER JOIN product_unit eu ON pc.end_unit_id = eu.id " +
                "JOIN t_product_catalog pcg ON pc.product_code = pcg.product_code " +
            $"WHERE pc.product_code IN ({inClause}) " +
            "ORDER BY pc.product_code";

        SqlExecuterItem response = SqlExecuter.ExecuteFunction(query, parameters, execute_user, false, "getProductConversionsDTO");

        if (!response.operationResult)
        {
            throw new DatabaseException("Error getting the product conversions");
        }

        List<ProductConversionDTO> productConversions = [];
        foreach (Dictionary<string, string> item in response.out_data)
        {
            int originUnitId = Convert.ToInt32(item["origin_unit_id"]);
            int endUnitId = Convert.ToInt32(item["end_unit_id"]);

            ProductUnitItem originUnit = new ProductUnitItemBuilder()
                .SetId(originUnitId)
                .SetAbbreviation(item["origin_unit_abbreviation"])
                .SetName(item["origin_unit_name"])
                .SetSlug(item["origin_unit_slug"])
                .Build();

            ProductUnitItem endUnit = new ProductUnitItemBuilder()
                .SetId(endUnitId)
                .SetAbbreviation(item["end_unit_abbreviation"])
                .SetName(item["end_unit_name"])
                .SetSlug(item["end_unit_slug"])
                .Build();

            ProductConversionDTO productConversion = new ProductConversionDTOBuilder()
                .SetId(Convert.ToInt32(item["id"]))
                .SetProductCode(item["product_code"])
                .SetOriginUnit(originUnit)
                .SetEndUnit(endUnit)
                .SetRate(Convert.ToDecimal(item["rate"]))
                .SetProductCatalogDescription(item["description_full"])
                .Build();

            productConversions.Add(productConversion);
        }

        return productConversions;
    }



    public static ProductConversionItem? GetProductConversionByAbbreviations(string productCode, string originUnit, string endUnit, string execute_user)
    {
        Dictionary<string, string> dic = new()
        {
            { "@ProductCode", productCode },
            { "@OriginUnit", originUnit },
            { "@EndUnit", endUnit }
        };

        string query = $"SELECT id, product_code, origin_unit_id, end_unit_id, rate FROM product_conversion " +
            "WHERE origin_unit_id = (SELECT id FROM product_unit WHERE abbreviation = @OriginUnit) " +
            "AND end_unit_id = (SELECT id FROM product_unit WHERE abbreviation = @EndUnit) " +
            "AND product_code = @ProductCode";

        SqlExecuterItem response = SqlExecuter.ExecuteFunction(query, dic, execute_user, false, "getProductConversionByAbbreviations");

        if (!response.operationResult)
        {
            throw new DatabaseException("Error getting the product conversion");
        }

        if (response.out_data.Count <= 0)
        {
            return null;
        }

        Dictionary<string, string> item = response.out_data[0];

        int originUnitId = Convert.ToInt32(item["origin_unit_id"]);
        int endUnitId = Convert.ToInt32(item["end_unit_id"]);

        return new ProductConversionItemBuilder()
            .SetId(Convert.ToInt32(item["id"]))
            .SetProductCode(item["product_code"])
            .SetOriginUnitId(originUnitId)
            .SetEndUnitId(endUnitId)
            .SetRate(Convert.ToDecimal(item["rate"]))
            .Build();
    }

    public static ProductConversionItem? GetProductConversionById(int id, string execute_user)
    {
        Dictionary<string, string> dic = new() { { "@Id", id.ToString() } };

        string query = $"SELECT id, product_code, origin_unit_id, end_unit_id, rate FROM product_conversion WHERE id = @Id";

        SqlExecuterItem response = SqlExecuter.ExecuteFunction(query, dic, execute_user, false, "getProductConversionById");

        if (!response.operationResult)
        {
            throw new DatabaseException("Error getting the product conversion");
        }

        if (response.out_data.Count <= 0)
        {
            return null;
        }

        Dictionary<string, string> item = response.out_data[0];

        int originUnitId = Convert.ToInt32(item["origin_unit_id"]);
        int endUnitId = Convert.ToInt32(item["end_unit_id"]);

        return new ProductConversionItemBuilder()
            .SetId(Convert.ToInt32(item["id"]))
            .SetProductCode(item["product_code"])
            .SetOriginUnitId(originUnitId)
            .SetEndUnitId(endUnitId)
            .SetRate(Convert.ToDecimal(item["rate"]))
            .Build();
    }

    public static List<ProductConversionItem> GetProductConversionsByCode(string productCode, string execute_user)
    {
        Dictionary<string, string> dic = new() { { "@ProductCode", productCode } };

        string query = $"SELECT id, product_code, origin_unit_id, end_unit_id, rate FROM product_conversion WHERE product_code = @ProductCode";

        SqlExecuterItem response = SqlExecuter.ExecuteFunction(query, dic, execute_user, false, "getProductConversionById");

        if (!response.operationResult)
        {
            throw new DatabaseException("Error getting the product conversion");
        }

        if (response.out_data.Count <= 0)
        {
            return [];
        }

        List<ProductConversionItem> productConversions = [];

        foreach (Dictionary<string, string> item in response.out_data)
        {
            int originUnitId = Convert.ToInt32(item["origin_unit_id"]);
            int endUnitId = Convert.ToInt32(item["end_unit_id"]);

            ProductConversionItem product = new ProductConversionItemBuilder()
                .SetId(Convert.ToInt32(item["id"]))
                .SetProductCode(item["product_code"])
                .SetOriginUnitId(originUnitId)
                .SetEndUnitId(endUnitId)
                .SetRate(Convert.ToDecimal(item["rate"]))
                .Build();

            productConversions.Add(product);
        }

        return productConversions;
    }

    public static Dictionary<string, List<ProductConversionDTO>> HashProductConversionByCode(List<ProductConversionDTO> productConversions)
    {
        Dictionary<string, List<ProductConversionDTO>> productConversionsHash = [];
        foreach (ProductConversionDTO productConversion in productConversions)
        {
            if (!productConversionsHash.TryGetValue(productConversion.product_code, out List<ProductConversionDTO>? value))
            {
                value = [];
                productConversionsHash[productConversion.product_code] = value;
            }

            value.Add(productConversion);
        }
        return productConversionsHash;
    }

    public static void CreateProductConversion(ProductConversionItem productConversion, string execute_user)
    {
        Dictionary<string, string> dic = new()
        {
            { "@ProductCode", productConversion.product_code },
            { "@OriginUnit", productConversion.origin_unit_id.ToString() },
            { "@EndUnit", productConversion.end_unit_id.ToString() },
            { "@Rate", productConversion.rate.ToString() }
        };

        string query = "INSERT INTO product_conversion (product_code, origin_unit_id, end_unit_id, rate) VALUES (@ProductCode, @OriginUnit, @EndUnit, @Rate)";

        SqlExecuterItem response = SqlExecuter.ExecuteFunction(query, dic, execute_user, true, "createProductConversion");

        if (!response.operationResult)
        {
            throw new DatabaseException("Error creating the product conversion");
        }
    }

    public static void UpdateProductConversion(ProductConversionItem productConversion, string execute_user)
    {
        Dictionary<string, string> dic = new()
        {
            { "@Id", productConversion.id.ToString() },
            { "@ProductCode", productConversion.product_code },
            { "@OriginUnit", productConversion.origin_unit_id.ToString() },
            { "@EndUnit", productConversion.end_unit_id.ToString() },
            { "@Rate", productConversion.rate.ToString() }
        };

        string query = "UPDATE product_conversion SET product_code = @ProductCode, origin_unit_id = @OriginUnit, end_unit_id = @EndUnit, rate = @Rate WHERE id = @Id";

        SqlExecuterItem response = SqlExecuter.ExecuteFunction(query, dic, execute_user, true, "updateProductConversion");

        if (!response.operationResult)
        {
            throw new DatabaseException("Error updating the product conversion");
        }
    }

    public static void PatchRate(int id, double rate, string execute_user)
    {
        Dictionary<string, string> dic = new()
        {
            { "@Id", id.ToString() },
            { "@Rate", rate.ToString() }
        };

        string query = "UPDATE product_conversion SET rate = @Rate WHERE id = @Id";

        SqlExecuterItem response = SqlExecuter.ExecuteFunction(query, dic, execute_user, true, "patchRate");

        if (!response.operationResult)
        {
            throw new DatabaseException("Error updating the product conversion rate");
        }
    }
}
