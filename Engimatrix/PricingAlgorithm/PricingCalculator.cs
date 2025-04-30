// // Copyright (c) 2024 Engibots. All rights reserved.

using engimatrix.ModelObjs;
using Engimatrix.ModelObjs;
using engimatrix.Models;
using engimatrix.Utils;
using engimatrix.Config;
using engimatrix.Exceptions;

namespace engimatrix.PricingAlgorithm;

public static class PricingCalculator
{
    private const int IVA_23 = 23;
    public static decimal CalculateQuotePriceAndSaveProducts(List<ProdutoComparado> products, OrderItem order)
    {
        // Step 1: Fetch Product Catalogs and Discounts
        List<string> productIds = [.. products
            .Where(p => !string.IsNullOrWhiteSpace(p.IdProdutoCatalogo))
            .Select(p => p.IdProdutoCatalogo.ToString())];

        if (productIds.Count == 0)
        {
            Log.Error("No product ids found");
            return 0;
        }

        List<ProductCatalogItem> productCatalogs = ProductCatalogModel.GetProductCatalogsByIds(productIds, false, "system");

        // Step 2: Get Order and Client Rating
        // Minimum client weighted rating possible
        // The segment with least discounts
        int clientSegmentId = 5;
        decimal orderWeightedRating = OrderRatingModel.GetOrderWeightedRating(order, "system");

        // Step 3: Fetch discounts
        List<string> productFamilyIds = [.. productCatalogs
            .Select(p => p.family_id)
            .Distinct()];

        // Get the productDiscounts by the distinct family ids existent on the product catalogs
        // The segment id only comes from the client rating
        List<ProductDiscountItem> productDiscounts = ProductDiscountModel
            .GetProductDiscountsByProductFamilyIdListBySegmentId(productFamilyIds, clientSegmentId, "system");

        // Step 3: Process Each Product, Get their price and save them to the order
        decimal total = 0;
        foreach (ProdutoComparado product in products)
        {
            decimal? currentPrice = ProcessProduct(product, productCatalogs, productDiscounts, orderWeightedRating, order.token);
            if (!currentPrice.HasValue)
            {
                Log.Error($"Could not calculate price for product {product.NomeProdutoSolicitado}");
                continue;
            }
            total += currentPrice.Value;
        }

        return Math.Round(total, 2, MidpointRounding.AwayFromZero);
    }

    private static decimal? ProcessProduct(
        ProdutoComparado product,
        List<ProductCatalogItem> productCatalogs,
        List<ProductDiscountItem> productDiscounts,
        decimal orderWeightedRating,
        string orderToken
    )
    {
        if (string.IsNullOrEmpty(product.IdProdutoCatalogo))
        {
            Log.Debug($"Product {product.NomeProdutoSolicitado} has no product catalog id");
            return null;
        }

        product.Quantidade = string.IsNullOrEmpty(product.Quantidade) ? "1" : product.Quantidade;
        int productCatalogId = int.Parse(product.IdProdutoCatalogo);

        // NOTE: If slow, change to dictionaries. Might not be needed as lists are small
        ProductCatalogItem? productCatalog = productCatalogs.FirstOrDefault(p => p.id == productCatalogId);
        if (productCatalog == null)
        {
            Log.Debug($"Could not find corresponding product catalog for product {product.NomeProdutoSolicitado} and id {productCatalogId}");
            return null;
        }

        ProductDiscountItem? productDiscount = productDiscounts.FirstOrDefault(p => p.product_family_id == productCatalog.family_id);
        if (productDiscount == null)
        {
            Log.Debug($"Could not find corresponding product discount for product {product.NomeProdutoSolicitado} and family id {productCatalog.family_id}");
            return null;
        }

        string unit = product.UnidadeQuantidade;
        ProductPricesItem finalProductPrice = CalculateProductPrice(ref unit, productCatalog, productDiscount, orderWeightedRating);
        product.UnidadeQuantidade = unit;

        SaveProductToDatabase(product, orderToken, productCatalogId, finalProductPrice);

        decimal quantity = decimal.Parse(product.Quantidade);
        decimal priceWithQuantity = finalProductPrice.price_with_margin * quantity;

        return priceWithQuantity;
    }

    private static ProductPricesItem CalculateProductPrice(
        ref string unit,
        ProductCatalogItem productCatalog,
        ProductDiscountItem productDiscount,
        decimal orderWeightedRating
    )
    {
        /*
        * FIRST PART: Calculate the price given the unit saved on the database, and the unit asked by the client
        */

        // First we check the unit the client is asking for and then the unit the product is sold
        // If not the same, we need to call the primavera endpoint to get the conversion for the
        // unit available at the product catalog so that we get the price
        // Ex: product catalog has KG, the client asks for UNIT, so we need the convert the
        // 2 units to 12 kgs and then get the price
        // Unit can either be KG, UN, M2, MT, RL, ML, M
        decimal productPrice = PricingHelper.GetPriceFromPricingStrategy(productCatalog);

        decimal? conversionRate = UnitConverter.CalculateConversionRate(unit, productCatalog);
        if (!conversionRate.HasValue)
        {
            Log.Error($"Error calculating conversion rate for product {productCatalog.product_code}");

            // Given no conversion rate found, try with UN
            if (unit != ProductUnitConstants.Unit.UN.ToString())
            {
                Log.Info($"Trying conversion rate with unit '{ProductUnitConstants.Unit.UN}' for product {productCatalog.product_code}");
                unit = ProductUnitConstants.Unit.UN.ToString();
                conversionRate = UnitConverter.CalculateConversionRate(unit, productCatalog);
            }

            // Last chance of getting a conversion rate, is by going with the unit explicit on the product catalog
            if (!conversionRate.HasValue)
            {
                Log.Error($"Error calculating conversion rate for product {productCatalog.product_code} even after converting to UN. Falling back to default.");
                unit = productCatalog.unit;
                conversionRate = 1;
            }
        }

        /*
        * SECOND PART: Calculate the price for the client given the segment and the product family. Apply all discounts and margins
        */

        // To calculate the pricing, we need to know 2 things - the product family and the segment id
        // For the product family, we get it by getting the product on the product catalog and checking the id
        // For the segment id, we need to get the client information that reveals which segment he is in
        // With that, we know the mb_min (minimum margin) and desc_max (maximum discount)
        decimal? mbMin100 = productDiscount?.mb_min;
        if (!mbMin100.HasValue)
        {
            Log.Error($"Could not find mb_min for product {productCatalog.product_code}, setting it to default value of 10");
            mbMin100 = 10;
        }
        decimal? descMax100 = productDiscount?.desc_max;
        if (!descMax100.HasValue)
        {
            Log.Error($"Could not find desc_max for product {productCatalog.product_code}, setting it to default value of 10");
            descMax100 = 10;
        }
        decimal mbMin = mbMin100.Value / 100;
        decimal descMax = descMax100.Value / 100;

        // The fixed margin is the mbMin that we always add to the product - the minimum margin needed for the enterprise activity
        // To that, we also add the descMax, which will be dynamic depending on the behaviour of the client
        // So if mbMin is 10% and descMax is 10%, we now have a product with 20% margin which will be the best case price (the client has poor ratings)
        decimal mbMinPriceMargin = productPrice * mbMin;
        decimal descMaxPriceDiscount = productPrice * descMax;
        decimal productPriceWithMargin = productPrice + mbMinPriceMargin + descMaxPriceDiscount;
        // Now we need to apply the discounts for the client depending on his ratings
        // If the client is top rated, weighted rating = 1, will subtract the total of descMax.
        // If the client is the worst, weighted rating = 0, will keep the current price with max margin
        decimal clientWeightedRatingBy100 = orderWeightedRating / 100;
        decimal clientDiscount = clientWeightedRatingBy100 * descMaxPriceDiscount;
        decimal productPriceWithDiscount = productPriceWithMargin - clientDiscount;

        decimal productTax = IVA_23 / 100 * productPriceWithDiscount;
        decimal productPriceWithDiscountPlusTax = productTax + productPriceWithDiscount;
        productPriceWithDiscountPlusTax *= conversionRate.Value;

        string debugMessage = $"Product: {productCatalog.product_code} - {productCatalog.description_full}\n" +
            $"Product price: {productPrice}\n" +
            $"Product price with margin: {productPriceWithMargin}\n" +
            $"Total discount: {clientDiscount} - For weight of {orderWeightedRating}\n" +
            $"Product price with discount: {productPriceWithDiscount}\n" +
            $"Product price with discount and tax, on the req.unit: {productPriceWithDiscountPlusTax}\n";

        ProductPricesItem productPrices = new(productPriceWithMargin, productPriceWithDiscount, productPriceWithDiscountPlusTax);
        productPrices = RoundPrices(productPrices, 4);

        Log.Debug(debugMessage);

        return productPrices;
    }

    private static ProductPricesItem RoundPrices(ProductPricesItem prices, int decimals)
    {
        return new ProductPricesItem(
            Math.Round(prices.price_with_margin, decimals, MidpointRounding.AwayFromZero),
            Math.Round(prices.price_with_margin_with_discount, decimals, MidpointRounding.AwayFromZero),
            Math.Round(prices.final_price_with_iva_in_requested_unit, decimals, MidpointRounding.AwayFromZero)
        );
    }

    private static void SaveProductToDatabase(
        ProdutoComparado product,
        string orderToken,
        int productCatalogId,
        ProductPricesItem productPrice
    )
    {
        decimal quantity = decimal.Parse(product.Quantidade);
        ProductUnitItem productUnit = UnitConverter.GetValidUnit(product.UnidadeQuantidade);
        int confidence = 0;
        if (!string.IsNullOrEmpty(product.Confianca))
        {
            if (int.TryParse(product.Confianca, out int parsedConfidence))
            {
                confidence = parsedConfidence;
            }
            else
            {
                Log.Error($"Could not parse confidence {product.Confianca}");
            }
        }

        OrderProductItem orderProduct = new OrderProductItemBuilder()
            .SetOrderToken(orderToken)
            .SetProductCatalogId(productCatalogId)
            .SetProductUnitId(productUnit.id)
            .SetQuantity(quantity)
            .SetCalculatedPrice(productPrice.price_with_margin)
            .SetPriceDiscount(productPrice.price_with_margin_with_discount)
            .SetIsInstantMatch(product.IsMatchInstantaneo)
            .SetConfidence(confidence)
            .SetIsManualInsert(false)
            .Build();

        OrderProductModel.CreateOrderProduct(orderProduct, "System", true);

        Log.Debug($"Saved product {product.NomeProdutoSolicitado} to database, associated with {product.NomeProdutoCatalogo} with price {productPrice.price_with_margin} and a discount of {productPrice.price_with_margin_with_discount}");
    }

    public static ProductPricesItem? CalculateOrderProductPrice(
        OrderProductItem product
    )
    {
        string orderToken = product.order_token;
        if (string.IsNullOrEmpty(orderToken))
        {
            throw new ArgumentNullException(nameof(orderToken), "Order token cannot be null or empty");
        }
        OrderItem? order = OrderModel.GetOrderByToken(orderToken, "system");
        if (order == null)
        {
            throw new NotFoundException($"Could not find order with token {orderToken}");
        }

        ProductCatalogItem? productCatalog = ProductCatalogModel.GetProductCatalogById(product.product_catalog_id, false, "system");
        if (productCatalog == null)
        {
            Log.Error($"Could not find corresponding product catalog for product {product.product_catalog_id}");
            return null;
        }

        int clientSegmentId = 5;
        // get the client
        if (!string.IsNullOrEmpty(order.client_code))
        {
            ClientItem? client = ClientModel.GetClientByCode(order.client_code, "system");
            if (client != null)
            {
                clientSegmentId = client.segment_id;
            }
        }

        decimal orderWeightedRating = OrderRatingModel.GetOrderWeightedRating(order, "system");

        // Get the productDiscounts by the distinct family ids existent on the product catalogs
        // The segment id only comes from the client rating
        ProductDiscountItem? productDiscount = ProductDiscountModel
            .GetProductDiscountByProductFamilyIdBySegmentId(productCatalog.family_id, clientSegmentId, "system");

        if (productDiscount == null)
        {
            Log.Error($"Could not find corresponding product catalog or discount for product {product.product_catalog_id}");
            return null;
        }

        ProductUnitItem? productUnit = ProductUnitModel.GetProductUnitById(product.product_unit_id, "system");
        productUnit ??= new()
        {
            abbreviation = ProductUnitConstants.Unit.UN.ToString()
        };

        string unit = productUnit.abbreviation;
        ProductPricesItem calculatedPrice = CalculateProductPrice(ref unit, productCatalog, productDiscount, orderWeightedRating);

        return calculatedPrice;
    }


}
