// // Copyright (c) 2024 Engibots. All rights reserved.

namespace engimatrix.ModelObjs
{
    public class ProductUnitItem
    {
        public int id { get; set; }
        public string abbreviation { get; set; }
        public string name { get; set; }
        public string slug { get; set; }
    }

    public class ProductUnitItemBuilder
    {
        private readonly ProductUnitItem productUnit = new();

        public ProductUnitItemBuilder SetId(int id)
        {
            productUnit.id = id;
            return this;
        }

        public ProductUnitItemBuilder SetAbbreviation(string abbreviation)
        {
            productUnit.abbreviation = abbreviation;
            return this;
        }

        public ProductUnitItemBuilder SetName(string name)
        {
            productUnit.name = name;
            return this;
        }

        public ProductUnitItemBuilder SetSlug(string slug)
        {
            productUnit.slug = slug;
            return this;
        }

        public ProductUnitItem Build()
        {
            return productUnit;
        }
    }
}
