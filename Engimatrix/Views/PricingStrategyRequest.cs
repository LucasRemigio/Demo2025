// // Copyright (c) 2024 Engibots. All rights reserved.

using engimatrix.Utils;

namespace engimatrix.Views
{
    public class PatchPricingStrategyRequest
    {
        public int pricing_strategy_id { get; set; }
        public string? family_id { get; set; }
        public int? product_id { get; set; }

        public bool IsValid()
        {
            if (pricing_strategy_id < 0 || pricing_strategy_id > 10)
            {
                return false;
            }

            return true;
        }
    }
}
