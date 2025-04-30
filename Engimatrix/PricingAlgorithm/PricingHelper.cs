// // Copyright (c) 2024 Engibots. All rights reserved.

using engimatrix.ModelObjs;
using engimatrix.Config;

namespace engimatrix.PricingAlgorithm;

public static class PricingHelper
{
    public static decimal GetPriceFromPricingStrategy(ProductCatalogItem product)
    {
        decimal price = product.pricing_strategy_id switch
        {
            (int)PricingStrategyId.LAST_PRICE => product.price_last,
            (int)PricingStrategyId.AVERAGE_PRICE => product.price_avg,
            _ => product.price_pvp
        };

        return price;
    }

}
