// // Copyright (c) 2024 Engibots. All rights reserved.

namespace engimatrix.ModelObjs
{
    public class ProductDiscountDTO
    {
        public ProductFamilyItem product_family { get; set; }
        public SegmentItem segment { get; set; }
        public decimal mb_min { get; set; } 
        public decimal desc_max { get; set; } 

        public ProductDiscountDTO(ProductFamilyItem productFamily, SegmentItem segment, decimal mbMin, decimal descMax)
        {
            this.product_family = productFamily;
            this.segment = segment;
            this.mb_min = mbMin;
            this.desc_max = descMax;
        }

        public ProductDiscountDTO()
        { }

        public ProductDiscountDTO ToItem()
        {
            return new ProductDiscountDTO(this.product_family, this.segment, this.mb_min, this.desc_max);
        }

        public override string ToString()
        {
            return $"ProductDiscountDTO:\n" +
                $"product_family_id: {product_family.id}\n" +
                     $"segment_id: {segment.id}\n" +
                     $"mb_min: {mb_min}\n" +
                     $"desc_max: {desc_max}\n";
        }

        public bool IsEmpty()
        {
            return string.IsNullOrEmpty(product_family.id) &&
                   segment.id == 0 &&
                   mb_min == 0 &&
                   desc_max == 0;
        }
    }
}
