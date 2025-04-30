// // Copyright (c) 2024 Engibots. All rights reserved.

namespace engimatrix.ModelObjs;
public class ProductPricesItem
{
    public decimal price_with_margin { get; set; }
    public decimal price_with_margin_with_discount { get; set; }
    public decimal final_price_with_iva_in_requested_unit { get; set; }

    public ProductPricesItem(decimal priceWithMargin, decimal priceWithMarginWithDiscount, decimal finalPriceWithIvaInRequestedUnit)
    {
        this.price_with_margin = priceWithMargin;
        this.price_with_margin_with_discount = priceWithMarginWithDiscount;
        this.final_price_with_iva_in_requested_unit = finalPriceWithIvaInRequestedUnit;
    }

}