// // Copyright (c) 2024 Engibots. All rights reserved.

using engimatrix.ModelObjs;
using engimatrix.Models;
using engimatrix.ResponseMessages;

namespace engimatrix.Views
{
    public class ProductFamilyListResponse
    {
        public List<ProductFamilyItem> product_families { get; set; } 
        public string result { get; set; }
        public int result_code { get; set; }

        public ProductFamilyListResponse(List<ProductFamilyItem> productFamilies, int result_code, string language)
        {
            this.product_families = productFamilies;
            this.result_code = result_code;
            this.result = ResponseMessage.GetResponseMessage(result_code, language);
        }

        public ProductFamilyListResponse(int result_code, string language)
        {
            this.product_families = [];
            this.result_code = result_code;
            this.result = ResponseMessage.GetResponseMessage(result_code, language);
        }
    }

    public class ProductFamilyItemResponse
    {
        public ProductFamilyItem product_family { get; set; } 
        public string result { get; set; }
        public int result_code { get; set; }

        public ProductFamilyItemResponse(ProductFamilyItem productFamily, int result_code, string language)
        {
            this.product_family = productFamily;
            this.result_code = result_code;
            this.result = ResponseMessage.GetResponseMessage(result_code, language);
        }

        public ProductFamilyItemResponse(int result_code, string language)
        {
            this.product_family = new();
            this.result_code = result_code;
            this.result = ResponseMessage.GetResponseMessage(result_code, language);
        }
    }
}
