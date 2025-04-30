// // Copyright (c) 2024 Engibots. All rights reserved.

namespace engimatrix.ModelObjs;

public class OrderProductItem : OrderProductBase
{
    public int product_catalog_id { get; set; }
    public int product_unit_id { get; set; }

    // Override ToString for debugging or logging
    public override string ToString()
    {
        return base.ToString() + $"\n" +
               $"Product Catalog ID: {product_catalog_id}\n" +
               $"Product Size ID: {product_unit_id}";
    }
}

public class OrderProductItemBuilder
{
    private readonly OrderProductItem orderProductItem = new();

    public OrderProductItemBuilder SetId(int id)
    {
        orderProductItem.id = id;
        return this;
    }

    public OrderProductItemBuilder SetOrderToken(string order_token)
    {
        orderProductItem.order_token = order_token;
        return this;
    }

    public OrderProductItemBuilder SetProductCatalogId(int product_catalog_id)
    {
        orderProductItem.product_catalog_id = product_catalog_id;
        return this;
    }

    public OrderProductItemBuilder SetProductUnitId(int product_unit_id)
    {
        orderProductItem.product_unit_id = product_unit_id;
        return this;
    }

    public OrderProductItemBuilder SetQuantity(decimal quantity)
    {
        orderProductItem.quantity = quantity;
        return this;
    }

    public OrderProductItemBuilder SetCalculatedPrice(decimal calculated_price)
    {
        orderProductItem.calculated_price = calculated_price;
        return this;
    }

    public OrderProductItemBuilder SetPriceDiscount(decimal price_discount)
    {
        orderProductItem.price_discount = price_discount;
        return this;
    }

    public OrderProductItemBuilder SetConfidence(int confidence)
    {
        orderProductItem.confidence = confidence;
        return this;
    }

    public OrderProductItemBuilder SetIsInstantMatch(bool is_instant_match)
    {
        orderProductItem.is_instant_match = is_instant_match;
        return this;
    }

    public OrderProductItemBuilder SetIsManualInsert(bool is_manual_insert)
    {
        orderProductItem.is_manual_insert = is_manual_insert;
        return this;
    }

    public OrderProductItemBuilder SetPriceLockedAt(DateTime price_locked_at)
    {
        orderProductItem.price_locked_at = price_locked_at;
        return this;
    }

    public OrderProductItem Build()
    {
        return orderProductItem;
    }
}
