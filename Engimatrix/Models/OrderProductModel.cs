// // Copyright (c) 2024 Engibots. All rights reserved.

using System.Globalization;
using engimatrix.Config;
using engimatrix.Connector;
using engimatrix.Exceptions;
using engimatrix.ModelObjs;
using engimatrix.PricingAlgorithm;
using engimatrix.Utils;
using Engimatrix.Models;
using Smartsheet.Api.OAuth;

namespace engimatrix.Models;

public static class OrderProductModel
{
    public static List<OrderProductItem> GetOrderProducts(string order_token, string execute_user)
    {
        Dictionary<string, string> dic = [];
        dic.Add("orderToken", order_token);

        string query = "SELECT id, order_token, product_catalog_id, quantity, product_unit_id, " +
            "calculated_price, confidence, is_instant_match, is_manual_insert, price_discount, price_locked_at " +
            "FROM `order_product` " +
            "WHERE `order_token` = @orderToken";

        List<OrderProductItem> orderProducts = SqlExecuter.ExecuteFunction<OrderProductItem>(query, dic, execute_user, false, "GetOrderProductRecord");

        return orderProducts;
    }

    public static List<OrderProductDTO> GetOrderProductsDTO(string order_token, string execute_user)
    {
        Dictionary<string, string> dic = [];
        dic.Add("orderToken", order_token);

        // TODO: RETRIEVE THE PRODUCT CATALOG HERE IN THE QUERY 
        string query = "SELECT op.id, op.order_token, op.product_catalog_id, op.quantity, " +
            "op.product_unit_id, ps.abbreviation, ps.name, ps.slug, " +
            "op.calculated_price, op.confidence, op.is_instant_match, op.is_manual_insert, " +
            "op.price_discount, op.price_locked_at " +
            "FROM `order_product` op " +
            "JOIN product_unit ps ON ps.id = op.product_unit_id " +
            "WHERE `order_token` = @orderToken";

        SqlExecuterItem response = SqlExecuter.ExecuteFunction(query, dic, execute_user, false, "GetOrderProductRecord");

        if (!response.operationResult)
        {
            throw new Exception("Something went wrong getting the order product from the database");
        }

        if (response.out_data.Count == 0)
        {
            return [];
        }

        List<OrderProductDTO> orderProducts = [];

        // TODO:  INSTEAD OF HERE! 
        // get all the product_catalog_id retrieved
        List<int> productCatalogIds = response.out_data.Select(x => Convert.ToInt32(x["product_catalog_id"])).ToList();
        List<ProductCatalogDTO> catalogProductsDTO = ProductCatalogModel.GetProductCatalogsByIdsDTO(productCatalogIds, execute_user);
        Dictionary<int, ProductCatalogDTO> catalogProductsMap = catalogProductsDTO.ToDictionary(x => x.id);

        foreach (Dictionary<string, string> item in response.out_data)
        {
            ProductUnitItem productUnit = new ProductUnitItemBuilder()
                .SetId(Convert.ToInt32(item["product_unit_id"]))
                .SetAbbreviation(item["abbreviation"])
                .SetName(item["name"])
                .SetSlug(item["slug"])
                .Build();

            decimal quantity = Convert.ToDecimal(item["quantity"]);
            DateTime? priceLockedAt = string.IsNullOrEmpty(item["price_locked_at"]) ? null : DateTime.Parse(item["price_locked_at"]);

            OrderProductDTOBuilder orderProduct = new OrderProductDTOBuilder()
                .SetId(Convert.ToInt32(item["id"]))
                .SetOrderToken(item["order_token"])
                .SetQuantity(quantity)
                .SetProductUnit(productUnit)
                .SetCalculatedPrice(Convert.ToDecimal(item["calculated_price"]))
                .SetConfidence(Convert.ToInt16(item["confidence"]))
                .SetIsInstantMatch(Convert.ToBoolean(item["is_instant_match"]))
                .SetIsManualInsert(Convert.ToBoolean(item["is_manual_insert"]))
                .SetPriceDiscount(Convert.ToDecimal(item["price_discount"]))
                .SetPriceLockedAt(priceLockedAt);

            int productCatalogId = Convert.ToInt32(item["product_catalog_id"]);

            if (catalogProductsMap.TryGetValue(productCatalogId, out ProductCatalogDTO? productCatalog))
            {
                orderProduct.SetProductCatalog(productCatalog);
            }
            else
            {
                Log.Error($"GetOrderProductsDTO: Product catalog with id {item["product_catalog_id"]} not found");
            }

            orderProducts.Add(orderProduct.Build());
        }

        return orderProducts;
    }

    // Put
    public static List<OrderProductItem> UpdateOrderProducts(string orderToken, List<OrderProductItem> orderProducts, string execute_user)
    {
        if (orderProducts == null || orderProducts.Count == 0)
        {
            return [];
        }

        // check if any of the products does not have the correct order token
        if (orderProducts.Any(p => p.order_token != orderToken))
        {
            throw new Exception("One or more products do not have the correct order token");
        }

        // Get the original order
        bool orderExists = OrderModel.OrderExists(orderToken, execute_user);
        if (!orderExists)
        {
            throw new NotFoundException($"Order with token {orderToken} not found");
        }

        // first check what products are already there
        List<OrderProductItem> currentOrderProducts = GetOrderProducts(orderToken, execute_user);
        List<OrderProductItem> updatedProducts = [];

        /*
         * Use cases to validate while comparing item for item:
         * If id is the same: check if values have changed
         *      If values have changed, update that item
         * If original had x id but now it doesnt: the item was eliminated
         * If products list has an id that didnt have before: the item is new
         */

        foreach (OrderProductItem product in orderProducts)
        {
            // Check if the product already exists in the current list (by ID)
            OrderProductItem? currentProduct = currentOrderProducts.FirstOrDefault(p => p.id == product.id);

            if (currentProduct != null)
            {
                // If the product exists, check if values have changed
                if (HasOrderProductChanged(currentProduct, product))
                {
                    // Update the product if values have changed
                    OrderProductItem updatedOrder = UpdateOrderProduct(product, execute_user);
                    updatedProducts.Add(updatedOrder);
                }

                // Remove the product from the current list since it is handled
                currentOrderProducts.Remove(currentProduct);
            }
            else
            {
                // Product is new (does not exist in the current list)
                OrderProductItem createdOrder = CalculatePriceAndCreateOrderProduct(product, execute_user);
                updatedProducts.Add(createdOrder);
            }
        }

        // Remaining items in currentOrderProducts are the ones that were eliminated
        if (currentOrderProducts.Count > 0)
        {
            DeleteOrderProductList(currentOrderProducts, execute_user);
        }

        // Patch the order rating
        OrderRatingModel.PatchOperationalCostRating(orderToken, execute_user);

        List<OrderProductItem> orderProductsAfterUpdates = OrderModel.RecalculateOrderProductsPrice(orderToken, execute_user);

        return orderProductsAfterUpdates;
    }

    private static bool HasOrderProductChanged(OrderProductItem currentProduct, OrderProductItem newProduct)
    {
        return currentProduct.product_catalog_id != newProduct.product_catalog_id ||
               currentProduct.quantity != newProduct.quantity ||
               currentProduct.product_unit_id != newProduct.product_unit_id;
    }

    public static OrderProductItem UpdateOrderProduct(OrderProductItem orderProduct, string execute_user)
    {
        // we do not check the stock because we can insert any stock both in quotation (where stock does not matter currently) and in order
        // as there are warning on the frontend and they can buy the material after accepting the order
        Dictionary<string, string> dic = new()
        {
            { "id", orderProduct.id.ToString() },
            { "productCatalogId", orderProduct.product_catalog_id.ToString() },
            { "quantity", orderProduct.quantity.ToString() },
            { "productUnitId", orderProduct.product_unit_id.ToString() }
        };

        string query = "UPDATE `order_product` " +
                       "SET product_catalog_id = @productCatalogId, quantity = @quantity, product_unit_id = @productUnitId ";

        // we need to check if the price is locked. If not locked, calculate the order product price as is. If locked, just get the current price from the database
        // The prices remain locked for 5 days
        if (!IsPriceLocked(orderProduct.price_locked_at))
        {
            ProductPricesItem? productPrices = PricingCalculator.CalculateOrderProductPrice(orderProduct) ?? throw new Exception("Could not calculate the price for the order product");

            orderProduct.calculated_price = productPrices.price_with_margin;
            orderProduct.price_discount = productPrices.price_with_margin_with_discount;

            dic.Add("calculatedPrice", orderProduct.calculated_price.ToString(CultureInfo.InvariantCulture));
            dic.Add("priceDiscount", orderProduct.price_discount.ToString(CultureInfo.InvariantCulture));

            query += ", calculated_price = @calculatedPrice, price_discount = @priceDiscount ";
        }

        query += " WHERE id = @id";

        SqlExecuterItem response = SqlExecuter.ExecFunction(query, dic, execute_user, true, "UpdateOrderProductRecord");

        if (!response.operationResult)
        {
            throw new DatabaseException("Something went wrong updating the order product in the database");
        }

        return orderProduct;
    }

    public static bool IsPriceLocked(DateTime? priceLockedAt)
    {
        if (!priceLockedAt.HasValue)
        {
            return false;
        }

        // Price locks expire after 5 days
        int expirationDays = GetQuotationExpirationDays();
        DateTime lockExpirationDate = DateTime.UtcNow.AddDays(-expirationDays);

        // Price is locked if lock date is more recent than expiration date
        return priceLockedAt.Value > lockExpirationDate;
    }

    private static int GetQuotationExpirationDays()
    {
        string settingKey = PlatformSettingId.QuotationExpirationTime.GetDescription();

        if (!ConfigManager.platformSettings.TryGetValue(settingKey, out string? settingValue))
        {
            throw new Exception($"Setting {settingKey} not found in app settings");
        }

        return int.Parse(settingValue, CultureInfo.InvariantCulture);
    }

    private static bool IsStockValid(OrderProductItem orderProduct, string executeUser)
    {
        if (orderProduct.product_catalog_id == 0 || orderProduct.product_unit_id == 0)
        {
            Log.Debug($"Product catalog or product unit not valid for order product with id: {orderProduct.id}, catalog_id: {orderProduct.product_catalog_id}, unit_id: {orderProduct.product_unit_id}");
            return false;
        }

        if (orderProduct.quantity <= 0)
        {
            Log.Debug($"Product quantity inserted is negative for order product with id: {orderProduct.id}, quantity: {orderProduct.quantity}");
            return false;
        }

        // check if the unit of the product is the same as the one in the produt catalog
        // if it is, the comparison is direct and it must be less than the available stock

        // if not, get the conversion rate for the unit on the product catalog and check if the
        // quantity times the conversion rate is less than the stock current

        // Get the maximum allowed quantity (in the order product's unit).
        decimal maxAllowed = GetMaxAllowedQuantity(orderProduct, executeUser);
        if (maxAllowed < 0)
        {
            // GetMaxAllowedQuantity already logged the error.
            return false;
        }

        if (orderProduct.quantity <= maxAllowed)
        {
            return true;
        }

        Log.Debug($"Order quantity ({orderProduct.quantity}) exceeds allowed maximum ({maxAllowed}) for order product id: {orderProduct.id}");
        return false;
    }

    private static decimal GetMaxAllowedQuantity(OrderProductItem orderProduct, string executeUser)
    {
        // Retrieve product catalog and product unit information.
        ProductCatalogItem? productCatalog = ProductCatalogModel.GetProductCatalogById(orderProduct.product_catalog_id, false, executeUser);
        if (productCatalog == null)
        {
            Log.Debug($"Product catalog not found for order product with id: {orderProduct.id}, catalog_id: {orderProduct.product_catalog_id}");
            return -1;
        }

        ProductUnitItem? productUnit = ProductUnitModel.GetProductUnitById(orderProduct.product_unit_id, executeUser);
        if (productUnit == null)
        {
            Log.Debug($"Product unit not found for order product with id: {orderProduct.id}, unit_id: {orderProduct.product_unit_id}");
            return -1;
        }

        // If the product unit matches the catalog unit, the maximum allowed quantity is the current stock.
        if (productUnit.abbreviation.Equals(productCatalog.unit, StringComparison.OrdinalIgnoreCase))
        {
            return productCatalog.stock_current;
        }

        // If the units differ, attempt to get the conversion rate.
        decimal? conversionRate = UnitConverter.CalculateConversionRate(productUnit.abbreviation, productCatalog);
        if (!conversionRate.HasValue)
        {
            Log.Debug($"Conversion rate not found for order product with id: {orderProduct.id}, catalog_id: {orderProduct.product_catalog_id}, unit_id: {orderProduct.product_unit_id}");
            return -1;
        }

        // Convert the catalog stock to the order product's unit.
        decimal maxQuantity = productCatalog.stock_current / conversionRate.Value;
        return maxQuantity;
    }

    private static void DeleteOrderProductList(List<OrderProductItem> orderProducts, string execute_user)
    {
        if (orderProducts == null || orderProducts.Count == 0)
        {
            return;
        }

        List<int> productIds = orderProducts.Select(p => p.id).ToList();
        string idsToDelete = string.Join(", ", productIds);

        string query = $"DELETE FROM `order_product` WHERE id IN ({idsToDelete})";

        SqlExecuterItem response = SqlExecuter.ExecFunction(query, [], execute_user, true, "DeleteOrderProductRecords");

        if (!response.operationResult)
        {
            throw new Exception("Something went wrong deleting the order products from the database");
        }
    }

    public static OrderProductItem CalculatePriceAndCreateOrderProduct(OrderProductItem orderProduct, string execute_user)
    {
        ProductPricesItem? productPrices = PricingCalculator.CalculateOrderProductPrice(orderProduct);
        if (productPrices == null)
        {
            throw new Exception("Could not calculate the price for the order product");
        }

        orderProduct.calculated_price = productPrices.price_with_margin;
        orderProduct.price_discount = productPrices.price_with_margin_with_discount;

        return CreateOrderProduct(orderProduct, execute_user, false);
    }

    public static decimal GetMaxProductAllowedStock(OrderProductItem orderProduct, string execute_user)
    {
        decimal maxAllowedQuantity = GetMaxAllowedQuantity(orderProduct, execute_user);
        if (maxAllowedQuantity <= 0)
        {
            throw new ProductStockNotValidException($"Product stock not valid for order product with id: {orderProduct.id}, catalog_id: {orderProduct.product_catalog_id}, {orderProduct.quantity}");
        }

        if (orderProduct.quantity <= 0)
        {
            if (maxAllowedQuantity > 1)
            {
                return 1;
            }

            return maxAllowedQuantity;
        }

        if (orderProduct.quantity <= maxAllowedQuantity)
        {
            return orderProduct.quantity;
        }

        decimal previousQuantity = orderProduct.quantity;
        // the price will be 2 digits after the decimal point, so if the value is 20.16666, we cannot save 20.17 as that is higher than the allowed.
        // So we must truncate the value on the second digit
        decimal truncatedAllowedQuantity = Math.Truncate(maxAllowedQuantity * 100) / 100;

        // if the product unit is a Unit, the truncation must be to the unit. we cannot sell 1.5 varoes
        if (orderProduct.product_unit_id == (int)ProductUnitConstants.Unit.UN)
        {
            truncatedAllowedQuantity = Math.Truncate(maxAllowedQuantity);

            Log.Debug($"Order quantity ({orderProduct.quantity}) exceeds allowed maximum ({maxAllowedQuantity}), so changed the quantity from ({previousQuantity}) to ({truncatedAllowedQuantity}) for order product id: {orderProduct.id}");

            return truncatedAllowedQuantity;
        }

        // else, we can round up to 2 decimal places
        Log.Debug($"Order quantity ({orderProduct.quantity}) exceeds allowed maximum ({maxAllowedQuantity}), so changed the quantity from ({previousQuantity}) to ({truncatedAllowedQuantity}) for order product id: {orderProduct.id}");

        return truncatedAllowedQuantity;

    }

    public static OrderProductItem CreateOrderProduct(OrderProductItem orderProduct, string execute_user, bool isValidateStock)
    {
        if (isValidateStock)
        {
            orderProduct.quantity = GetMaxProductAllowedStock(orderProduct, execute_user);
        }

        Dictionary<string, string> dic = [];
        dic.Add("@orderToken", orderProduct.order_token);
        dic.Add("@productCatalogId", orderProduct.product_catalog_id.ToString());
        dic.Add("@quantity", orderProduct.quantity.ToString());
        dic.Add("@productUnitId", orderProduct.product_unit_id.ToString());
        dic.Add("@confidence", orderProduct.confidence.ToString());
        dic.Add("@isInstantMatch", orderProduct.is_instant_match ? "1" : "0");
        dic.Add("@isManualInsert", orderProduct.is_manual_insert ? "1" : "0");
        dic.Add("@calculatedPrice", orderProduct.calculated_price.ToString(CultureInfo.InvariantCulture));
        dic.Add("@priceDiscount", orderProduct.price_discount.ToString(CultureInfo.InvariantCulture));

        string query = "INSERT INTO `order_product` " +
            "(order_token, product_catalog_id, quantity, product_unit_id, calculated_price, price_discount, confidence, is_instant_match, is_manual_insert) " +
            "VALUES (@orderToken, @productCatalogId, @quantity, @productUnitId, @calculatedPrice, @priceDiscount, @confidence, @isInstantMatch, @isManualInsert)";

        SqlExecuterItem response = SqlExecuter.ExecuteFunction(query, dic, execute_user, true, "CreateOrderProductRecord");

        if (!response.operationResult)
        {
            throw new Exception("Something went wrong creating the order product in the database");
        }

        // get the last id
        query = "SELECT MAX(id) as id FROM `order_product`";
        SqlExecuterItem responseId = SqlExecuter.ExecuteFunction(query, [], execute_user, false, "GetLastInsertId");
        int id = Convert.ToInt32(responseId.out_data[0]["id"]);
        orderProduct.id = id;

        return orderProduct;
    }

    public static void LockOrderProductPrices(string orderToken, string execute_user)
    {
        Dictionary<string, string> dic = [];
        dic.Add("@OrderToken", orderToken);
        dic.Add("@PriceLockedAt", DateTime.UtcNow.ToString("yyyy-MM-dd HH:mm:ss", CultureInfo.InvariantCulture));

        string query =
            @"UPDATE `order_product` 
            SET price_locked_at = @PriceLockedAt 
            WHERE order_token = @OrderToken ";

        SqlExecuterItem response = SqlExecuter.ExecuteFunction(query, dic, execute_user, true, "LockOrderProductPrices");

        if (!response.operationResult)
        {
            throw new Exception("Something went wrong locking the order product prices in the database");
        }
    }

    // New helper method that accepts the dictionaries and does the lookups.
    public static (decimal quantityInUnits, decimal quantityInBaseUnit) GetConvertedQuantities(
        OrderProductItem product,
        Dictionary<int, ProductCatalogItem> productCatalogsMap,
        Dictionary<string, List<ProductConversionItem>> productConversionsMap,
        List<ProductUnitItem> units)
    {
        if (!productCatalogsMap.TryGetValue(product.product_catalog_id, out ProductCatalogItem? catalog))
        {
            throw new NotFoundException("Product catalog not found with id " + product.product_catalog_id);
        }

        if (!productConversionsMap.TryGetValue(catalog.product_code, out List<ProductConversionItem>? productConversions))
        {
            Log.Warning("Product conversion not found with product code " + catalog.product_code);
        }

        // Delegate to the internal conversion logic
        return GetConvertedQuantitiesInternal(product, catalog, productConversions, units);
    }

    // Original conversion logic now extracted into an internal method.
    private static (decimal quantityInUnits, decimal quantityInBaseUnit) GetConvertedQuantitiesInternal(
        OrderProductItem product,
        ProductCatalogItem catalog,
        // There are some products that do not have product converisons, like the Vigas
        List<ProductConversionItem>? productConversions,
        List<ProductUnitItem> units
        )
    {
        decimal quantityInUnits = product.quantity;
        decimal quantityInBaseUnit = product.quantity;

        // If product unit is not UN, convert quantityInUnits using conversion rate to UN.
        if (product.product_unit_id != (int)ProductUnitConstants.Unit.UN)
        {
            if (productConversions is not null)
            {
                ProductConversionItem? conversionToUN = productConversions.FirstOrDefault(
                    x => x.origin_unit_id == product.product_unit_id &&
                         x.end_unit_id == (int)ProductUnitConstants.Unit.UN)
                    ?? throw new NotFoundException("Conversion to UN not found");
                quantityInUnits *= conversionToUN.rate;
            }
        }

        // Get the catalog's base unit based on its abbreviation.
        ProductUnitItem catalogBaseUnit = units.FirstOrDefault(x => x.abbreviation == catalog.unit)
            ?? throw new NotFoundException("Catalog base unit not found");

        // Retrieve product's unit details.
        ProductUnitItem? productUnit = units.FirstOrDefault(x => x.id == product.product_unit_id)
            ?? throw new NotFoundException("Product unit not found");

        // If product unit differs from catalog base unit, convert quantityInBaseUnit.
        if (productUnit.abbreviation != catalog.unit)
        {
            if (productConversions is not null)
            {
                ProductConversionItem? conversionToBaseUnit = productConversions.FirstOrDefault(
                    x => x.origin_unit_id == product.product_unit_id &&
                         x.end_unit_id == catalogBaseUnit.id)
                    ?? throw new NotFoundException("Conversion to catalog base unit not found");
                quantityInBaseUnit *= conversionToBaseUnit.rate;
            }
        }

        quantityInUnits = Math.Round(quantityInUnits, 2, MidpointRounding.AwayFromZero);
        quantityInBaseUnit = Math.Round(quantityInBaseUnit, 2, MidpointRounding.AwayFromZero);

        return (quantityInUnits, quantityInBaseUnit);
    }

}
