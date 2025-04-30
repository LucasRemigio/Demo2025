// // Copyright (c) 2024 Engibots. All rights reserved.

using engimatrix.Models;

namespace engimatrix.ModelObjs;

public class OrderProductDTO : OrderProductBase
{
    public ProductCatalogDTO product_catalog { get; set; }
    public ProductUnitItem product_unit { get; set; }
    public decimal rate_meters { get; set; }
    public bool is_price_locked { get; set; }

    // Override ToString for debugging or logging (can customize further if needed)
    public override string ToString()
    {
        return base.ToString() + $"\n" +
               $"Product Catalog: {product_catalog}\n" +
               $"Product Size: {product_unit}";
    }

}

public class OrderProductDTOBuilder
{
    private readonly OrderProductDTO orderProductDTO = new();

    public OrderProductDTOBuilder SetId(int id)
    {
        orderProductDTO.id = id;
        return this;
    }

    public OrderProductDTOBuilder SetOrderToken(string order_token)
    {
        orderProductDTO.order_token = order_token;
        return this;
    }

    public OrderProductDTOBuilder SetProductCatalog(ProductCatalogDTO product_catalog)
    {
        orderProductDTO.product_catalog = product_catalog;
        return this;
    }

    public OrderProductDTOBuilder SetProductUnit(ProductUnitItem product_unit)
    {
        orderProductDTO.product_unit = product_unit;
        return this;
    }

    public OrderProductDTOBuilder SetQuantity(decimal quantity)
    {
        orderProductDTO.quantity = quantity;
        return this;
    }

    public OrderProductDTOBuilder SetCalculatedPrice(decimal calculated_price)
    {
        orderProductDTO.calculated_price = calculated_price;
        return this;
    }

    public OrderProductDTOBuilder SetPriceDiscount(decimal price_discount)
    {
        orderProductDTO.price_discount = price_discount;
        return this;
    }

    public OrderProductDTOBuilder SetConfidence(int confidence)
    {
        orderProductDTO.confidence = confidence;
        return this;
    }

    public OrderProductDTOBuilder SetIsInstantMatch(bool is_instant_match)
    {
        orderProductDTO.is_instant_match = is_instant_match;
        return this;
    }

    public OrderProductDTOBuilder SetIsManualInsert(bool is_manual_insert)
    {
        orderProductDTO.is_manual_insert = is_manual_insert;
        return this;
    }

    public OrderProductDTOBuilder SetRateMeters(decimal rate_meters)
    {
        orderProductDTO.rate_meters = rate_meters;
        return this;
    }

    public OrderProductDTOBuilder SetConfidenceLevel(int confidence)
    {
        orderProductDTO.confidence = confidence;
        return this;
    }

    public OrderProductDTOBuilder SetPriceLockedAt(DateTime? price_locked_at)
    {
        orderProductDTO.price_locked_at = price_locked_at;
        orderProductDTO.is_price_locked = OrderProductModel.IsPriceLocked(price_locked_at);
        return this;
    }

    public OrderProductDTO Build()
    {
        return orderProductDTO;
    }
}

public class OrderProductDTONoAuth
{
    public int id { get; set; }
    public string order_token { get; set; }
    public decimal quantity { get; set; }
    public int confidence { get; set; }
    public bool is_instant_match { get; set; }
    public bool is_manual_insert { get; set; }
    public ProductCatalogDTONoAuth product_catalog { get; set; }
    public ProductUnitItem product_unit { get; set; }
    public decimal rate_meters { get; set; }
    public decimal price_discount { get; set; }
    public decimal calculated_price { get; set; }
    public DateTime? price_locked_at { get; set; }
    public bool is_price_locked { get; set; }

    // Override ToString for debugging or logging (can customize further if needed)
    public override string ToString()
    {
        return base.ToString() + $"\n" +
               $"Product Catalog: {product_catalog}\n" +
               $"Product Size: {product_unit}";
    }

}