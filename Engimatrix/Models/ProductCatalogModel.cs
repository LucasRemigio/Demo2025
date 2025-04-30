// // Copyright (c) 2024 Engibots. All rights reserved.

using System.Diagnostics;
using System.Text.RegularExpressions;
using engimatrix.Config;
using engimatrix.Exceptions;
using engimatrix.ModelObjs;
using engimatrix.ModelObjs.Primavera;
using engimatrix.Utils;
using engimatrix.Views;
using Quartz.Util;

namespace engimatrix.Models;

public static class ProductCatalogModel
{
    private static string GetProductCatalogDtoBaseQuery()
    {
        string query = "SELECT p.id, p.product_code, p.description_full AS description, p.unit, p.stock_current, p.currency, p.price_pvp, p.price_avg, p.price_last, " +
            "DATE_FORMAT(p.date_last_entry, '%d-%m-%Y') AS date_last_entry, DATE_FORMAT(p.date_last_exit, '%d-%m-%Y') AS date_last_exit, " +
            "f.id AS family_id, f.name AS family_name, p.price_ref_market, t.id AS type_id, t.name AS type_name, m.id AS material_id, " +
            "m.name AS material_name, s.id AS shape_id, s.name AS shape_name, fi.id AS finishing_id, fi.name AS finishing_name, " +
            "p.length, p.width, p.height, su.id AS surface_id, su.name AS surface_name, p.thickness, p.diameter, p.description_full as description_full, " +
            "ps.id AS strategy_id, ps.name AS strategy_name, ps.slug AS strategy_slug, p.is_pricing_ready " +
            "FROM t_product_catalog p " +
            "LEFT JOIN mf_product_family f ON p.family_id = f.id " +
            "LEFT JOIN mf_product_type t ON p.type_id = t.id " +
            "LEFT JOIN mf_product_material m ON p.material_id = m.id " +
            "LEFT JOIN mf_product_shape s ON p.shape_id = s.id " +
            "LEFT JOIN mf_product_finishing fi ON p.finishing_id = fi.id " +
            "LEFT JOIN mf_product_surface su ON p.surface_id = su.id " +
            "LEFT JOIN mf_pricing_strategy ps ON p.pricing_strategy_id = ps.id " +
            "WHERE 1=1 ";

        return query;
    }

    private static string GetProductCatalogItemBaseQuery()
    {
        string query = "SELECT p.id, p.product_code, p.description_full, p.unit, p.stock_current, p.currency, p.price_pvp, p.price_avg, p.price_last, " +
            "DATE_FORMAT(p.date_last_entry, '%d-%m-%Y') AS date_last_entry, DATE_FORMAT(p.date_last_exit, '%d-%m-%Y') AS date_last_exit, " +
            "p.family_id, p.price_ref_market, p.type_id, p.material_id, p.shape_id, " +
            "p.finishing_id, p.length, p.width, p.height, p.surface_id, p.thickness, p.diameter, " +
            "p.created_at, p.created_by, p.updated_at, p.updated_by, description, " +
            "p.nominal_dimension, p.pricing_strategy_id, p.is_pricing_ready " +
            "FROM t_product_catalog p " +
            "WHERE 1 = 1 ";

        return query;
    }

    public static ProductCatalogItem FabricateProductCatalogItem(Dictionary<string, string> product)
    {
        int id = int.Parse(product["id"]);
        string productCode = product["product_code"];
        string descriptionFull = product["description_full"];
        string unit = product["unit"];
        decimal stockCurr = ParseHelper.ParseDecimal(product["stock_current"]);
        string currency = product["currency"];
        decimal pricePvp = ParseHelper.ParseDecimal(product["price_pvp"]);
        decimal priceAvg = ParseHelper.ParseDecimal(product["price_avg"]);
        decimal priceLast = ParseHelper.ParseDecimal(product["price_last"]);
        DateOnly dateLastEntry = ParseHelper.ParseDateOnly(product["date_last_entry"], "dd-MM-yyyy");
        DateOnly dateLastExit = ParseHelper.ParseDateOnly(product["date_last_exit"], "dd-MM-yyyy");
        string familyId = product["family_id"];
        decimal priceRefMarket = ParseHelper.ParseDecimal(product["price_ref_market"]);
        int typeId = ParseHelper.ParseInt(product["type_id"]);
        int materialId = ParseHelper.ParseInt(product["material_id"]);
        int shapeId = ParseHelper.ParseInt(product["shape_id"]);
        int finishingId = ParseHelper.ParseInt(product["finishing_id"]);
        int surfaceId = ParseHelper.ParseInt(product["surface_id"]);
        decimal length = ParseHelper.ParseDecimal(product["length"]);
        decimal width = ParseHelper.ParseDecimal(product["width"]);
        decimal height = ParseHelper.ParseDecimal(product["height"]);
        decimal thickness = ParseHelper.ParseDecimal(product["thickness"]);
        decimal diameter = ParseHelper.ParseDecimal(product["diameter"]);
        DateTime createdAt = !string.IsNullOrEmpty(product["created_at"]) ? DateTime.Parse(product["created_at"]) : default;
        string createdBy = product["created_by"];
        DateTime? updatedAt = !string.IsNullOrEmpty(product["updated_at"]) ? DateTime.Parse(product["updated_at"]) : null;
        string updatedBy = product["updated_by"];
        string description = product["description"];
        string nominalDimension = product["nominal_dimension"];
        int pricingStrategyId = ParseHelper.ParseInt(product["pricing_strategy_id"]);
        bool isPricingReady = ParseHelper.ParseBool(product["is_pricing_ready"]);

        ProductCatalogItem productCatalog = new ProductCatalogItemBuilder()
            .SetId(id)
            .SetProductCode(productCode)
            .SetDescriptionFull(descriptionFull)
            .SetUnit(unit)
            .SetStockCurrent(stockCurr)
            .SetCurrency(currency)
            .SetPricePvp(pricePvp)
            .SetPriceAvg(priceAvg)
            .SetPriceLast(priceLast)
            .SetDateLastEntry(dateLastEntry)
            .SetDateLastExit(dateLastExit)
            .SetFamilyId(familyId)
            .SetPriceRefMarket(priceRefMarket)
            .SetTypeId(typeId)
            .SetMaterialId(materialId)
            .SetShapeId(shapeId)
            .SetFinishingId(finishingId)
            .SetSurfaceId(surfaceId)
            .SetLength(length)
            .SetWidth(width)
            .SetHeight(height)
            .SetThickness(thickness)
            .SetDiameter(diameter)
            .SetCreatedAt(createdAt)
            .SetCreatedBy(createdBy)
            .SetUpdatedAt(updatedAt)
            .SetUpdatedBy(updatedBy)
            .SetDescription(description)
            .SetNominalDimension(nominalDimension)
            .SetPricingStrategyId(pricingStrategyId)
            .SetIsPricingReady(isPricingReady)
            .Build();

        return productCatalog;
    }

    public static List<ProductCatalogDTO> GetProductCatalogsDTOForPricing(string execute_user, bool? hasStock)
    {
        // TODO:
        // Possible Filters: stockCurr, pricepvp, priceavg, pricelast, datelastentry, datelastexit, pricerefmarket
        // product_code

        Dictionary<string, string> dic = [];
        string query = GetProductCatalogDtoBaseQuery();

        if (hasStock.HasValue && hasStock.Value)
        {
            query += "AND p.stock_current > 0 ";
        }

        query += "AND p.is_pricing_ready = 1 " +
            "ORDER BY p.product_code";

        SqlExecuterItem response = SqlExecuter.ExecuteFunction(query, dic, execute_user, false, "GetProductCatalogsDTO");

        if (!response.operationResult) { throw new Exception("An error occurred fetching data from the product catalogs table."); }

        List<ProductCatalogDTO> productCatalogs = [];
        foreach (Dictionary<string, string> item in response.out_data)
        {
            ProductCatalogDTO product = FabricateProductCatalogDTO(item);
            productCatalogs.Add(product);
        }
        return productCatalogs;
    }

    public static List<ProductCatalogDTO> GetProductCatalogsDTO(string execute_user, string? family_id, bool? hasStock, bool isToFilterDiscontinued)
    {
        // TODO:
        // Possible Filters: stockCurr, pricepvp, priceavg, pricelast, datelastentry, datelastexit, pricerefmarket
        // product_code

        Dictionary<string, string> dic = [];
        string query = GetProductCatalogDtoBaseQuery();

        if (!string.IsNullOrEmpty(family_id))
        {
            dic.Add("@FamilyId", family_id);
            query += "AND p.family_id = @FamilyId ";
        }

        if (isToFilterDiscontinued)
        {
            dic.Add("@FamilyIdDiscontinued", ProductFamilyConstants.ProductFamilyCode.DESCONTINUADOS.ToString());
            query += "AND p.family_id != @FamilyIdDiscontinued ";
        }

        if (hasStock.HasValue && hasStock.Value)
        {
            query += "AND p.stock_current > 0 ";
        }

        query += "ORDER BY p.product_code";

        SqlExecuterItem response = SqlExecuter.ExecuteFunction(query, dic, execute_user, false, "GetProductCatalogsDTO");

        if (!response.operationResult)
        {
            throw new Exception("An error occurred fetching data from the product catalogs table.");
        }

        List<ProductCatalogDTO> productCatalogs = [];

        if (response.out_data.Count <= 0)
        {
            return productCatalogs;
        }

        foreach (Dictionary<string, string> item in response.out_data)
        {
            ProductCatalogDTO product = FabricateProductCatalogDTO(item);
            productCatalogs.Add(product);
        }
        return productCatalogs;
    }

    public static List<ProductCatalogDTO> SearchProductCatalogs(string? query, bool? hasStock, string executeUser)
    {
        Dictionary<string, string> dic = [];
        string sqlQuery = GetProductCatalogDtoBaseQuery();

        if (hasStock.HasValue && hasStock.Value)
        {
            sqlQuery += "AND p.stock_current > 0 ";
        }

        if (string.IsNullOrEmpty(query))
        {
            // if no query, return the most famous products
            List<int> famousIds = GetMostRequestedProductCatalogIds(executeUser, 20);
            List<ProductCatalogDTO> famousProducts = GetProductCatalogsByIdsDTO(famousIds, executeUser);
            return famousProducts;
        }

        string[] words = query.Split(' ', StringSplitOptions.RemoveEmptyEntries);
        List<string> conditions = [];

        for (int i = 0; i < words.Length; i++)
        {
            string paramName = $"@Query{i}";
            dic.Add(paramName, "%" + words[i] + "%");
            conditions.Add($"(p.product_code LIKE {paramName} OR p.description_full LIKE {paramName} OR t.name LIKE {paramName} OR m.name LIKE {paramName} OR s.name LIKE {paramName} OR fi.name LIKE {paramName} OR su.name LIKE {paramName})");
        }

        sqlQuery += " AND " + string.Join(" AND ", conditions);

        sqlQuery += "ORDER BY p.product_code LIMIT 20";

        SqlExecuterItem response = SqlExecuter.ExecuteFunction(sqlQuery, dic, executeUser, false, "SearchProductCatalogs");

        if (!response.operationResult)
        {
            throw new Exception("An error occurred fetching data from the product catalogs table.");
        }

        List<ProductCatalogDTO> productCatalogs = [];
        List<string> productCodes = [.. response.out_data.Select(item => item["product_code"])];
        List<ProductConversionDTO> productConversions = ProductConversionModel.GetProductConversionsDTOByCodes(productCodes, executeUser);
        Dictionary<string, List<ProductConversionDTO>> conversionsByCode = productConversions
            .GroupBy(pc => pc.product_code)
            .ToDictionary(group => group.Key, group => group.ToList());

        foreach (Dictionary<string, string> item in response.out_data)
        {
            ProductCatalogDTO product = FabricateProductCatalogDTO(item);
            if (conversionsByCode.TryGetValue(product.product_code, out List<ProductConversionDTO>? conversions))
            {
                product.product_conversions = conversions;
            }
            productCatalogs.Add(product);
        }

        return productCatalogs;
    }

    public static List<int> GetMostRequestedProductCatalogIds(string executeUser, int limit)
    {
        string query = $@"SELECT product_catalog_id, SUM(quantity) AS total_quantity
            FROM order_product
            GROUP BY product_catalog_id
            ORDER BY total_quantity DESC
            LIMIT {limit};";

        SqlExecuterItem response = SqlExecuter.ExecuteFunction(query, [], executeUser, false, "GetMostRequestedProductCatalogIds");

        if (!response.operationResult)
        {
            throw new Exception("An error occurred fetching data from the product catalogs table.");
        }

        if (response.out_data.Count <= 0)
        {
            throw new Exception("No product codes found in the product catalogs table.");
        }

        List<int> productIds = [];

        foreach (Dictionary<string, string> item in response.out_data)
        {
            productIds.Add(int.Parse(item["product_catalog_id"]));
        }

        return productIds;
    }


    public static List<ProductCatalogDTONoAuth> GetProductCatalogsDTONoAuth(string? familyId, bool? hasStock, bool isToFilterDiscontinued, string executeUser)
    {
        List<ProductCatalogDTO> productCatalogs = GetProductCatalogsDTO(executeUser, familyId, hasStock, isToFilterDiscontinued);

        List<ProductCatalogDTONoAuth> productCatalogsNoAuth = [];
        foreach (ProductCatalogDTO product in productCatalogs)
        {
            ProductCatalogDTONoAuth productNoAuth = new()
            {
                id = product.id,
                product_code = product.product_code,
                description = product.description,
                description_full = product.description_full,
                unit = product.unit,
                family = product.family,
                type = product.type,
                shape = product.shape,
                material = product.material,
                finishing = product.finishing,
                surface = product.surface,
                length = product.length,
                width = product.width,
                height = product.height,
                thickness = product.thickness,
                diameter = product.diameter,
            };

            productCatalogsNoAuth.Add(productNoAuth);
        }

        return productCatalogsNoAuth;
    }

    public static List<ProductCatalogDTONoAuth> SearchProductCatalogsNoAuth(string? query, bool? hasStock, string executeUser)
    {
        List<ProductCatalogDTO> productCatalogs = SearchProductCatalogs(query, hasStock, executeUser);

        List<ProductCatalogDTONoAuth> productCatalogsNoAuth = [];
        foreach (ProductCatalogDTO product in productCatalogs)
        {
            ProductCatalogDTONoAuth productNoAuth = new()
            {
                id = product.id,
                product_code = product.product_code,
                description = product.description,
                description_full = product.description_full,
                unit = product.unit,
                family = product.family,
                type = product.type,
                shape = product.shape,
                material = product.material,
                finishing = product.finishing,
                surface = product.surface,
                length = product.length,
                width = product.width,
                height = product.height,
                thickness = product.thickness,
                diameter = product.diameter,
                product_conversions = product.product_conversions
            };

            productCatalogsNoAuth.Add(productNoAuth);
        }

        return productCatalogsNoAuth;
    }

    public static List<ProductCatalogItem> GetProductCatalogsByStock(bool hasStock, string execute_user)
    {
        string query = GetProductCatalogItemBaseQuery() +
            "AND p.price_pvp > 0 ";

        if (hasStock)
        {
            query += " AND p.stock_current > 0 ";
        }

        query += " ORDER BY p.product_code";

        SqlExecuterItem response = SqlExecuter.ExecuteFunction(query, [], execute_user, false, "GetProductCatalogsDTO");

        if (!response.operationResult)
        {
            throw new Exception("An error occurred fetching data from the product catalogs table.");
        }

        List<ProductCatalogItem> productCatalogs = [];

        if (response.out_data.Count <= 0)
        {
            return productCatalogs;
        }

        foreach (Dictionary<string, string> item in response.out_data)
        {
            ProductCatalogItem product = FabricateProductCatalogItem(item);

            productCatalogs.Add(product);
        }
        return productCatalogs;
    }

    public static List<ProductCatalogItem> GetProductCatalogs(string execute_user)
    {
        string query = GetProductCatalogItemBaseQuery() + "ORDER BY p.product_code";

        SqlExecuterItem response = SqlExecuter.ExecuteFunction(query, [], execute_user, false, "GetProductCatalogItems");

        if (!response.operationResult)
        {
            throw new DatabaseException("An error occurred fetching data from the product catalogs table.");
        }

        List<ProductCatalogItem> productCatalogs = [];

        if (response.out_data.Count <= 0)
        {
            return productCatalogs;
        }

        foreach (Dictionary<string, string> item in response.out_data)
        {
            ProductCatalogItem product = FabricateProductCatalogItem(item);
            productCatalogs.Add(product);
        }

        return productCatalogs;
    }

    public static List<ProductCatalogItem> GetProductCatalogsByCode(string productCode, string execute_user)
    {
        Dictionary<string, string> dic = new() { { "@ProductCode", productCode } };
        string query = GetProductCatalogItemBaseQuery();
        query += "AND p.product_code = @ProductCode " +
            "ORDER BY id";

        SqlExecuterItem response = SqlExecuter.ExecuteFunction(query, dic, execute_user, false, "GetProductCatalogsDTO");

        if (!response.operationResult)
        {
            throw new Exception("An error occurred fetching data from the product catalogs table.");
        }

        List<ProductCatalogItem> productCatalogs = [];

        if (response.out_data.Count <= 0)
        {
            return productCatalogs;
        }

        foreach (Dictionary<string, string> item in response.out_data)
        {
            ProductCatalogItem product = FabricateProductCatalogItem(item);

            productCatalogs.Add(product);
        }
        return productCatalogs;
    }

    public static List<ProductCatalogItem> GetProductCatalogsByIds(List<string> ids, bool hasStock, string execute_user)
    {
        if (ids.Count == 0)
        {
            return [];
        }

        string query = GetProductCatalogItemBaseQuery();

        if (hasStock)
        {
            query += "AND p.stock_current > 0 ";
        }

        query += $"AND p.id IN ({string.Join(", ", ids)}) ORDER BY id";

        SqlExecuterItem response = SqlExecuter.ExecuteFunction(query, [], execute_user, false, "GetProductCatalogsByID");

        if (!response.operationResult)
        {
            throw new Exception("An error occurred fetching data from the product catalogs table.");
        }

        List<ProductCatalogItem> productCatalogs = [];

        if (response.out_data.Count <= 0)
        {
            return productCatalogs;
        }

        foreach (Dictionary<string, string> item in response.out_data)
        {
            ProductCatalogItem productCatalog = FabricateProductCatalogItem(item);

            productCatalogs.Add(productCatalog);
        }
        return productCatalogs;
    }

    public static ProductCatalogItem? GetProductCatalogById(int productCatalogId, bool hasStock, string execute_user)
    {
        string query = GetProductCatalogItemBaseQuery();

        if (hasStock)
        {
            query += "AND p.stock_current > 0 ";
        }

        Dictionary<string, string> dic = [];
        dic.Add("ProductCatalogId", productCatalogId.ToString());

        query += $"AND p.id = @ProductCatalogId";

        SqlExecuterItem response = SqlExecuter.ExecuteFunction(query, dic, execute_user, false, "GetProductCatalogsByID");

        if (!response.operationResult)
        {
            throw new Exception("An error occurred fetching data from the product catalogs table.");
        }

        if (response.out_data.Count <= 0)
        {
            return null;
        }

        Dictionary<string, string> item = response.out_data[0];

        ProductCatalogItem productCatalog = FabricateProductCatalogItem(item);

        return productCatalog;
    }

    public static ProductCatalogDTO GetProductCatalogsByIdDTO(string productCatalogId, string execute_user)
    {
        Dictionary<string, string> dic = new()
        {
            { "productCatalogId", productCatalogId }
        };
        string query = GetProductCatalogDtoBaseQuery() +
            "AND p.id = @productCatalogId";

        SqlExecuterItem response = SqlExecuter.ExecuteFunction(query, dic, execute_user, false, "GetProductCatalogsByIdDTO");

        if (!response.operationResult)
        {
            throw new Exception("An error occurred fetching data from the product catalogs table.");
        }

        if (response.out_data.Count <= 0)
        {
            throw new InputNotValidException($"No product catalog DTO found with following id {productCatalogId}");
        }

        Dictionary<string, string> item = response.out_data[0];
        ProductCatalogDTO productCatalogDTO = FabricateProductCatalogDTO(item);

        return productCatalogDTO;
    }

    public static List<ProductCatalogDTO> GetProductCatalogsByIdsDTO(List<int> productCatalogIds, string execute_user)
    {
        string ids = string.Join(", ", productCatalogIds);

        string query = GetProductCatalogDtoBaseQuery() +
            $"AND p.id IN ({ids})";

        SqlExecuterItem response = SqlExecuter.ExecuteFunction(query, [], execute_user, false, "GetProductCatalogsByIdDTO");

        if (!response.operationResult)
        {
            throw new Exception("An error occurred fetching data from the product catalogs table.");
        }

        if (response.out_data.Count <= 0)
        {
            throw new InputNotValidException($"No product catalog DTO found with following ids {ids}");
        }

        List<string> productCodes = [.. response.out_data.Select(item => item["product_code"])];
        List<ProductConversionDTO> productConversions = ProductConversionModel.GetProductConversionsDTOByCodes(productCodes, execute_user);
        Dictionary<string, List<ProductConversionDTO>> conversionsByCode = productConversions
            .GroupBy(pc => pc.product_code)
            .ToDictionary(group => group.Key, group => group.ToList());

        List<ProductCatalogDTO> products = [];
        foreach (Dictionary<string, string> item in response.out_data)
        {
            ProductFamilyItem productFamily = new(item["family_id"], item["family_name"]);

            int id = int.Parse(item["id"]);
            decimal stockCurr = string.IsNullOrEmpty(item["stock_current"]) ? default : decimal.Parse(item["stock_current"]);
            decimal pricePvp = string.IsNullOrEmpty(item["price_pvp"]) ? default : decimal.Parse(item["price_pvp"]);
            decimal priceAvg = string.IsNullOrEmpty(item["price_avg"]) ? default : decimal.Parse(item["price_avg"]);
            decimal priceLast = string.IsNullOrEmpty(item["price_last"]) ? default : decimal.Parse(item["price_last"]);
            DateOnly dateLastEntry = string.IsNullOrEmpty(item["date_last_entry"]) ? default : DateOnly.ParseExact(item["date_last_entry"], "dd-MM-yyyy");
            DateOnly dateLastExit = string.IsNullOrEmpty(item["date_last_exit"]) ? default : DateOnly.ParseExact(item["date_last_exit"], "dd-MM-yyyy");
            decimal priceRefMarket = string.IsNullOrEmpty(item["price_ref_market"]) ? default : decimal.Parse(item["price_ref_market"]);

            int typeId = string.IsNullOrEmpty(item["type_id"]) ? default : int.Parse(item["type_id"]);
            int materialId = string.IsNullOrEmpty(item["material_id"]) ? default : int.Parse(item["material_id"]);
            int shapeId = string.IsNullOrEmpty(item["shape_id"]) ? default : int.Parse(item["shape_id"]);
            int finishingId = string.IsNullOrEmpty(item["finishing_id"]) ? default : int.Parse(item["finishing_id"]);
            int surfaceId = string.IsNullOrEmpty(item["surface_id"]) ? default : int.Parse(item["surface_id"]);

            ProductTypeItem productType = new(typeId, item["type_name"]);
            ProductMaterialItem productMaterial = new(materialId, item["material_name"]);
            ProductShapeItem productShape = new(shapeId, item["shape_name"]);
            ProductFinishingItem productFinishing = new(finishingId, item["finishing_name"]);
            ProductSurfaceItem productSurface = new(surfaceId, item["surface_name"]);

            decimal length = string.IsNullOrEmpty(item["length"]) ? default : decimal.Parse(item["length"]);
            decimal width = string.IsNullOrEmpty(item["width"]) ? default : decimal.Parse(item["width"]);
            decimal height = string.IsNullOrEmpty(item["height"]) ? default : decimal.Parse(item["height"]);
            decimal thickness = string.IsNullOrEmpty(item["thickness"]) ? default : decimal.Parse(item["thickness"]);
            decimal diameter = string.IsNullOrEmpty(item["diameter"]) ? default : decimal.Parse(item["diameter"]);

            string descriptionFull = item["description_full"];
            PricingStrategyItem pricingStrategy = new(int.Parse(item["strategy_id"]), item["strategy_name"], item["strategy_slug"]);

            ProductCatalogDTO productCatalogDTO = new(
                id, item["product_code"], item["description"], descriptionFull, productType, productShape, productMaterial, productFinishing, productSurface,
                length, width, height, thickness, diameter, item["unit"], stockCurr, item["currency"], pricePvp, priceAvg, priceLast, dateLastEntry,
                dateLastExit, productFamily, priceRefMarket
            );

            productCatalogDTO.pricing_strategy = pricingStrategy;

            if (conversionsByCode.TryGetValue(productCatalogDTO.product_code, out List<ProductConversionDTO>? conversions))
            {
                productCatalogDTO.product_conversions = conversions;
            }

            products.Add(productCatalogDTO);
        }

        return products;
    }

    public static ProductCatalogDTO FabricateProductCatalogDTO(Dictionary<string, string> item)
    {
        int id = int.Parse(item["id"]);

        decimal stockCurr = ParseHelper.ParseDecimal(item["stock_current"]);
        decimal pricePvp = ParseHelper.ParseDecimal(item["price_pvp"]);
        decimal priceAvg = ParseHelper.ParseDecimal(item["price_avg"]);
        decimal priceLast = ParseHelper.ParseDecimal(item["price_last"]);

        DateOnly dateLastEntry = ParseHelper.ParseDateOnly(item["date_last_entry"], "dd-MM-yyyy");
        DateOnly dateLastExit = ParseHelper.ParseDateOnly(item["date_last_exit"], "dd-MM-yyyy");
        decimal priceRefMarket = ParseHelper.ParseDecimal(item["price_ref_market"]);

        decimal length = ParseHelper.ParseDecimal(item["length"]);
        decimal width = ParseHelper.ParseDecimal(item["width"]);
        decimal height = ParseHelper.ParseDecimal(item["height"]);
        decimal thickness = ParseHelper.ParseDecimal(item["thickness"]);
        decimal diameter = ParseHelper.ParseDecimal(item["diameter"]);

        string descriptionFull = item["description_full"];

        int pricingStrategyId = ParseHelper.ParseInt(item["strategy_id"]);
        PricingStrategyItem pricing = new(pricingStrategyId, item["strategy_name"], item["strategy_slug"]);

        bool isPricingReady = ParseHelper.ParseBool(item["is_pricing_ready"]);

        ProductCatalogDTOBuilder productCatalogDTOBuilder = new ProductCatalogDTOBuilder()
            .SetId(id)
            .SetProductCode(item["product_code"])
            .SetDescription(item["description"])
            .SetDescriptionFull(descriptionFull)
            .SetUnit(item["unit"])
            .SetStockCurrent(stockCurr)
            .SetCurrency(item["currency"])
            .SetPricePvp(pricePvp)
            .SetPriceAvg(priceAvg)
            .SetPriceLast(priceLast)
            .SetDateLastEntry(dateLastEntry)
            .SetDateLastExit(dateLastExit)
            .SetPriceRefMarket(priceRefMarket)
            .SetLength(length)
            .SetWidth(width)
            .SetHeight(height)
            .SetThickness(thickness)
            .SetDiameter(diameter)
            .SetPricingStrategy(pricing)
            .SetIsPricingReady(isPricingReady);

        string familyId = item["family_id"];
        if (!string.IsNullOrEmpty(familyId))
        {
            ProductFamilyItem productFamily = new(item["family_id"], item["family_name"]);
            productCatalogDTOBuilder.SetFamily(productFamily);
        }

        int typeId = ParseHelper.ParseInt(item["type_id"]);
        int materialId = ParseHelper.ParseInt(item["material_id"]);
        int shapeId = ParseHelper.ParseInt(item["shape_id"]);
        int finishingId = ParseHelper.ParseInt(item["finishing_id"]);
        int surfaceId = ParseHelper.ParseInt(item["surface_id"]);

        if (typeId != 0)
        {
            ProductTypeItem productType = new(typeId, item["type_name"]);
            productCatalogDTOBuilder.SetType(productType);
        }
        if (materialId != 0)
        {
            ProductMaterialItem productMaterial = new(materialId, item["material_name"]);
            productCatalogDTOBuilder.SetMaterial(productMaterial);
        }
        if (shapeId != 0)
        {
            ProductShapeItem productShape = new(shapeId, item["shape_name"]);
            productCatalogDTOBuilder.SetShape(productShape);
        }
        if (finishingId != 0)
        {
            ProductFinishingItem productFinishing = new(finishingId, item["finishing_name"]);
            productCatalogDTOBuilder.SetFinishing(productFinishing);
        }
        if (surfaceId != 0)
        {
            ProductSurfaceItem productSurface = new(surfaceId, item["surface_name"]);
            productCatalogDTOBuilder.SetSurface(productSurface);
        }

        return productCatalogDTOBuilder.Build();
    }

    public static void CreateProductCatalog(ProductCatalogItem productCatalogItem, string execute_user)
    {
        Dictionary<string, string> dic = new()
        {
            { "product_code", productCatalogItem.product_code },
            { "description", productCatalogItem.description },
            { "unit", productCatalogItem.unit },
            { "stock_current", productCatalogItem.stock_current.ToString() },
            { "currency", productCatalogItem.currency },
            { "price_pvp", productCatalogItem.price_pvp.ToString() },
            { "price_avg", productCatalogItem.price_avg.ToString() },
            { "price_last", productCatalogItem.price_last.ToString() },
            { "date_last_entry", productCatalogItem.date_last_entry.ToString("yyyy-MM-dd") },
            { "date_last_exit", productCatalogItem.date_last_exit.ToString("yyyy-MM-dd") },
            { "family_id", productCatalogItem.family_id },
            { "price_ref_market", productCatalogItem.price_ref_market.ToString() },
            { "created_at", DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss") },
            { "created_by", execute_user }
        };

        string query = "INSERT INTO t_product_catalog (product_code, description, unit, stock_current, currency, price_pvp, price_avg, price_last, date_last_entry, date_last_exit, family_id, price_ref_market, created_at, created_by) " +
            "VALUES (@product_code, @description, @unit, @stock_current, @currency, @price_pvp, @price_avg, @price_last, @date_last_entry, @date_last_exit, @family_id, @price_ref_market, @created_at, @created_by)";

        SqlExecuterItem response = SqlExecuter.ExecFunction(query, dic, execute_user, true, "CreateProductCatalog");

        if (!response.operationResult)
        {
            throw new Exception("An error occurred creating a new product catalog.");
        }
    }

    public static void UpdateProductCatalog(ProductCatalogItem productCatalogItem, string execute_user)
    {
        Dictionary<string, string> dic = new()
        {
            { "product_code", productCatalogItem.product_code },
            { "description", productCatalogItem.description },
            { "unit", productCatalogItem.unit },
            { "stock_current", productCatalogItem.stock_current.ToString() },
            { "currency", productCatalogItem.currency },
            { "price_pvp", productCatalogItem.price_pvp.ToString() },
            { "price_avg", productCatalogItem.price_avg.ToString() },
            { "price_last", productCatalogItem.price_last.ToString() },
            { "date_last_entry", productCatalogItem.date_last_entry.ToString("yyyy-MM-dd") },
            { "date_last_exit", productCatalogItem.date_last_exit.ToString("yyyy-MM-dd") },
            { "family_id", productCatalogItem.family_id },
            { "price_ref_market", productCatalogItem.price_ref_market.ToString() },
            { "productCatalogId", productCatalogItem.id.ToString() },
            { "updated_at", DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss") },
            { "updated_by", execute_user }
        };

        string query = "UPDATE t_product_catalog SET product_code = @product_code, description = @description, " +
            "unit = @unit, stock_current = @stock_current, currency = @currency, price_pvp = @price_pvp, price_avg = @price_avg, " +
            "price_last = @price_last, date_last_entry = @date_last_entry, date_last_exit = @date_last_exit, family_id = @family_id, " +
            "price_ref_market = @price_ref_market, updated_at = @updated_at, updated_by = @updated_by " +
            "WHERE id = @productCatalogId";

        SqlExecuterItem response = SqlExecuter.ExecFunction(query, dic, execute_user, true, "UpdateProductCatalog");

        if (!response.operationResult)
        {
            throw new Exception("An error occurred updating the product catalog.");
        }
    }

    public static void DeleteProductCatalog(int productCatalogId, string execute_user)
    {
        Dictionary<string, string> dic = new()
        {
            { "productCatalogId", productCatalogId.ToString() }
        };

        string query = "DELETE FROM t_product_catalog WHERE id = @productCatalogId";

        SqlExecuterItem response = SqlExecuter.ExecFunction(query, dic, execute_user, true, "DeleteProductCatalog");

        if (!response.operationResult)
        {
            throw new Exception("An error occurred deleting the product catalog.");
        }
    }

    public static void PatchProductPricingStrategy(PatchPricingStrategyRequest pricingReq, string execute_user)
    {
        Dictionary<string, string> dic = new()
        {
            { "pricingStrategyId", pricingReq.pricing_strategy_id.ToString() }
        };

        string query = "UPDATE t_product_catalog " +
             "SET pricing_strategy_id = @pricingStrategyId " +
             "WHERE 1=1 ";

        if (!string.IsNullOrEmpty(pricingReq.family_id))
        {
            dic.Add("@FamilyId", pricingReq.family_id);
            query += "AND family_id = @FamilyId";
        }

        string? productId = pricingReq.product_id.ToString();
        if (!string.IsNullOrEmpty(productId))
        {
            dic.Add("@ProductId", productId);
            query += "AND id = @ProductId";
        }

        SqlExecuterItem response = SqlExecuter.ExecuteFunction(query, dic, execute_user, true, "PatchPricingStrategy");

        if (!response.operationResult)
        {
            throw new Exception("An error occurred updating the pricing strategy.");
        }
    }

    public static List<string> GetPossibleUnits(string executeUser)
    {
        string query = "SELECT DISTINCT unit FROM t_product_catalog";

        SqlExecuterItem response = SqlExecuter.ExecuteFunction(query, [], executeUser, false, "GetPossibleUnits");

        if (!response.operationResult)
        {
            throw new Exception("An error occurred fetching data from the product catalogs table.");
        }

        if (response.out_data.Count <= 0)
        {
            throw new Exception("No units found in the product catalogs table.");
        }

        List<string> units = response.out_data.Select(item => item["unit"].ToUpperInvariant()).ToList();

        return units;
    }

    public static List<string> GetProductCodes(string execute_user)
    {
        string query = "SELECT DISTINCT(product_code) FROM t_product_catalog";

        SqlExecuterItem response = SqlExecuter.ExecuteFunction(query, [], execute_user, false, "GetProductCodes");

        if (!response.operationResult)
        {
            throw new Exception("An error occurred fetching data from the product catalogs table.");
        }

        if (response.out_data.Count <= 0)
        {
            throw new Exception("No product codes found in the product catalogs table.");
        }

        List<string> productCodes = [];

        foreach (Dictionary<string, string> item in response.out_data)
        {
            productCodes.Add(item["product_code"]);
        }

        return productCodes;
    }

    public static Dictionary<string, string> GetProductCodesDictionary(string execute_user)
    {
        List<string> productCodes = GetProductCodes(execute_user);

        Dictionary<string, string> hashedProductCodes = [];

        foreach (string productCode in productCodes)
        {
            hashedProductCodes.Add(productCode, productCode);
        }

        return hashedProductCodes;
    }

    public static bool ProductCodeExists(string productCode, string executeUser)
    {
        Dictionary<string, string> dic = new()
        {
            { "productCode", productCode }
        };

        string query = "SELECT COUNT(*) AS count FROM t_product_catalog WHERE product_code = @productCode";

        SqlExecuterItem response = SqlExecuter.ExecuteFunction(query, dic, executeUser, false, "ProductCodeExists");

        if (!response.operationResult)
        {
            throw new Exception("An error occurred fetching data from the product catalogs table.");
        }

        if (response.out_data.Count <= 0)
        {
            throw new Exception("No data found in the product catalogs table.");
        }

        return int.Parse(response.out_data[0]["count"]) > 0;
    }

    public static async Task<SyncPrimaveraStats> SyncPrimavera(string executer_user)
    {
        Stopwatch stopwatch = new();
        stopwatch.Start();
        int totalSyncs = 0;

        // get our products
        List<ProductCatalogItem> productCatalogs = GetProductCatalogs(executer_user);

        // get primavera products
        Dictionary<string, List<MFPrimaveraProductItem>> primaveraProductsByCode = await PrimaveraProductCatalogModel.GetPrimaveraProductsByProductCodeHashed();

        // get the stocks for the products
        Dictionary<string, PrimaveraProductStockItem> primaveraStocksByCodeByUnit = await PrimaveraProductCatalogModel.GetProductStockByProductCodeByUnitHashed();

        // for each of our products =>
        // find the match having in mind the unit
        // check if any of the price changed
        // if yes, update it
        foreach (ProductCatalogItem product in productCatalogs)
        {
            // get the corresponding primavera products
            if (!primaveraProductsByCode.TryGetValue(product.product_code, out List<MFPrimaveraProductItem>? primaveraProducts))
            {
                // no product found, for some reason, so skip it
                // This should not happen as every product must be a mirror of the primavera product
                Log.Error($"SyncPrimaveraCatalogProduct - No product found for product code {product.product_code}");
                continue;
            }

            // Get the first product. it exists because we checked above and we only need the first, we dont need more as
            // we already do the conversions for the product units
            MFPrimaveraProductItem primaveraProduct = primaveraProducts.First();

            // get the stock for the product. The key of code+unit ensures we only get 1 stock for the product
            string key = PrimaveraProductCatalogModel.GetProductStockByProductCodeByUnitHashedKey(product.product_code, product.unit);
            if (!primaveraStocksByCodeByUnit.TryGetValue(key, out PrimaveraProductStockItem? primaveraStock))
            {
                Log.Debug($"SyncPrimaveraCatalogProduct - No stock found for key {key}");
                // proceed, no stock found means our product does not have the correct unit, so we will update it next
            }

            if (primaveraStock != null)
            {
                // check if any price, stock or unit has changed
                if (!HasProductChanged(product, primaveraProduct, primaveraStock))
                {
                    // everything is the same, no update needed. Proceed to next product
                    continue;
                }
            }

            string productDebugMessage = $"Price has changed: \n " +
            $"ProductCatalog {product.product_code} {product.description_full}, stock: {product.stock_current}, pvp: {product.price_pvp}, average: {product.price_avg}, last: {product.price_last} \n " +
            $"PrimaveraProduct {primaveraProduct.Artigo} {primaveraProduct.Descricao}, stock {primaveraStock?.StkDisponivel}, pvp: {primaveraProduct.PVP1}, average: {primaveraProduct.PrecoCustoMedio}, last: {primaveraProduct.UltimoPrecoCompra}";
            Log.Info(productDebugMessage);

            // update the product if any of the parametrs has updated
            product.unit = primaveraProduct.UnidadeBase;
            product.stock_current = primaveraStock != null ? (decimal)primaveraStock.StkDisponivel : 0;
            product.price_pvp = (decimal)primaveraProduct.PVP1;
            product.price_avg = primaveraProduct.PrecoCustoMedio.HasValue ? (decimal)primaveraProduct.PrecoCustoMedio.Value! : 0;
            product.price_last = (decimal)primaveraProduct.UltimoPrecoCompra;
            UpdateProductCatalog(product, executer_user);
            totalSyncs++;
        }

        stopwatch.Stop();
        Log.Debug($"SyncProductCatalogPrimavera finished in {stopwatch.ElapsedMilliseconds} ms with {totalSyncs} syncs");
        SyncPrimaveraStats primaveraStats = new(stopwatch.ElapsedMilliseconds, totalSyncs);
        return primaveraStats;
    }

    public static async Task<SyncPrimaveraStats> CompareProductStockLists(string executer_user)
    {
        Stopwatch stopwatch = new();
        stopwatch.Start();
        int totalSyncs = 0;
        int totalNotFound = 0;

        List<ProductCatalogItem> productCatalogs = GetProductCatalogs(executer_user);

        // get the stocks for the products
        Dictionary<string, PrimaveraProductStockItem> primaveraStocksByCodeByUnit = await PrimaveraProductCatalogModel.GetProductStockByProductCodeByUnitHashed();

        // get the stocks for the products by warehouse
        Dictionary<string, PrimaveraProductStockWarehouseItem> primaveraStocksByCodeByUnitByWarehouse = await PrimaveraProductCatalogModel.GetProductStockByProductCodeWarehouseHashed();

        // We want to go product by product to see if their corresponding match from the warehouse has a different stock
        foreach (ProductCatalogItem product in productCatalogs)
        {
            if (!primaveraStocksByCodeByUnit.TryGetValue(product.product_code, out PrimaveraProductStockItem? productStock))
            {
                continue;
            }

            // get the corresponding primavera products
            if (!primaveraStocksByCodeByUnitByWarehouse.TryGetValue(product.product_code, out PrimaveraProductStockWarehouseItem? warehouseProduct))
            {
                totalNotFound++;
                continue;
            }

            // round both values
            decimal stock = Math.Round((decimal)productStock.StkDisponivel, 2);
            decimal stockWarehouse = Math.Round((decimal)warehouseProduct.StkDisponivel, 2);

            if (stock == stockWarehouse)
            {
                continue;
            }

            string productDebugMessage = $"Stock has changed: \n " +
            $"ProductCatalog {product.product_code} {product.description_full}, stock: {product.stock_current} \n " +
            $"Product {productStock.Artigo}, stock: {stock} \n " +
            $"Warehouse {warehouseProduct.Artigo}, stock {stockWarehouse}";
            Log.Info(productDebugMessage);

            totalSyncs++;
        }

        stopwatch.Stop();
        Log.Debug($"SyncPrimaveraCompareStocks finished in {stopwatch.ElapsedMilliseconds} ms with {totalSyncs} syncs and {totalNotFound} not found");
        SyncPrimaveraStats primaveraStats = new(stopwatch.ElapsedMilliseconds, totalSyncs);
        return primaveraStats;
    }

    public static bool HasProductChanged(
        ProductCatalogItem productCatalog,
        MFPrimaveraProductItem primaveraProduct,
        PrimaveraProductStockItem primaveraProductStock
    )
    {
        // Stock
        if (productCatalog.stock_current != (decimal)primaveraProductStock.StkDisponivel)
        {
            return true;
        }

        // Unit
        if (!string.Equals(productCatalog.unit, primaveraProduct.UnidadeBase, StringComparison.OrdinalIgnoreCase))
        {
            return true;
        }

        // Prices
        if (productCatalog.price_pvp != (decimal)primaveraProduct.PVP1)
        {
            return true;
        }

        if (primaveraProduct.PrecoCustoMedio.HasValue)
        {
            if (productCatalog.price_avg != (decimal)primaveraProduct.PrecoCustoMedio!)
            {
                return true;
            }
        }

        if (productCatalog.price_last != (decimal)primaveraProduct.UltimoPrecoCompra)
        {
            return true;
        }

        return false;
    }
}