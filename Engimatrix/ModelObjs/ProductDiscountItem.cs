// // Copyright (c) 2024 Engibots. All rights reserved.

namespace engimatrix.ModelObjs
{
    public class ProductDiscountItem
    {
        public string product_family_id { get; set; }
        public int segment_id { get; set; } 
        public decimal mb_min { get; set; } 
        public decimal desc_max { get; set; } 

        public ProductDiscountItem(string productFamilyId, int segmentId, decimal mbMin, decimal descMax)
        {
            this.product_family_id = productFamilyId;
            this.segment_id = segmentId;
            this.mb_min = mbMin;
            this.desc_max = descMax;
        }

        public ProductDiscountItem()
        { }

        public ProductDiscountItem ToItem()
        {
            return new ProductDiscountItem(this.product_family_id, this.segment_id, this.mb_min, this.desc_max);
        }

        public override string ToString()
        {
            return $"ProductDiscountItem:\n" +
                $"product_family_id: {product_family_id}\n" +
                $"segment_id: {segment_id}\n" +
                $"mb_min: {mb_min}\n" +
                $"desc_max: {desc_max}\n";
        }

        public bool IsEmpty()
        {
            return string.IsNullOrEmpty(product_family_id) &&
                   segment_id == 0 &&
                   mb_min == 0 &&
                   desc_max == 0;
        }
    }
}
