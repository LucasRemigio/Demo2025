// // Copyright (c) 2024 Engibots. All rights reserved.

namespace engimatrix.ModelObjs
{
    public class PricingStrategyItem
    {
        public int id { get; set; }
        public string name { get; set; }
        public string slug { get; set; }

        public PricingStrategyItem(int id, string name, string slug)
        {
            this.id = id;
            this.name = name;
            this.slug = slug;
        }

        public PricingStrategyItem()
        { }

    }
}
