// // Copyright (c) 2024 Engibots. All rights reserved.

namespace engimatrix.ModelObjs
{
    public class ProductItem
    {
        public int id { get; set; }
        public string email_token { get; set; }
        public string name { get; set; }
        public string size { get; set; }
        public decimal quantity { get; set; }
        public string quantity_unit { get; set; }
        public int confidence { get; set; }
        public int? product_catalog_id { get; set; }
        public int? match_confidence { get; set; }
        public decimal? calculated_price { get; set; }

        public ProductItem()
        { }

        public ProductItem(int id, string email_token, string name, string size, decimal quantity, string quantity_unit, int confidence, int? product_catalog_id, int? match_confidence, decimal? calculated_price)
        {
            this.id = id;
            this.email_token = email_token;
            this.name = name;
            this.size = size;
            this.quantity = quantity;
            this.quantity_unit = quantity_unit;
            this.confidence = confidence;
            this.product_catalog_id = product_catalog_id;
            this.match_confidence = match_confidence;
            this.calculated_price = calculated_price;
        }

        public ProductItem(string email_token, string name, string size, decimal quantity, string quantity_unit, int confidence, int? product_catalog_id, int? match_confidence, decimal? calculated_price)
        {
            this.id = -1;
            this.email_token = email_token;
            this.name = name;
            this.size = size;
            this.quantity = quantity;
            this.quantity_unit = quantity_unit;
            this.confidence = confidence;
            this.product_catalog_id = product_catalog_id;
            this.match_confidence = match_confidence;
            this.calculated_price = calculated_price;
        }

        // Method to create a copy of the current object
        public ProductItem ToItem()
        {
            return new ProductItem(this.id, this.email_token, this.name, this.size, this.quantity, this.quantity_unit, this.confidence, this.product_catalog_id, this.match_confidence, this.calculated_price);
        }
    }
}
