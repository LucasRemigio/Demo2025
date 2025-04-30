// // Copyright (c) 2024 Engibots. All rights reserved.

using engimatrix.ModelObjs;
using engimatrix.Models;
using engimatrix.ResponseMessages;

namespace engimatrix.Views
{
    public class ProductDiscountListResponse
    {
        public List<ProductDiscountDTO> product_discounts { get; set; }
        public string result { get; set; }
        public int result_code { get; set; }

        public ProductDiscountListResponse(List<ProductDiscountDTO> productDiscounts, int result_code, string language)
        {
            this.product_discounts = productDiscounts;
            this.result_code = result_code;
            this.result = ResponseMessage.GetResponseMessage(result_code, language);
        }

        public ProductDiscountListResponse(int result_code, string language)
        {
            this.product_discounts = [];
            this.result_code = result_code;
            this.result = ResponseMessage.GetResponseMessage(result_code, language);
        }
    }

    public class ProductDiscountItemResponse
    {
        public ProductDiscountDTO product_discount { get; set; } 
        public string result { get; set; }
        public int result_code { get; set; }

        public ProductDiscountItemResponse(ProductDiscountDTO productDiscount, int result_code, string language)
        {
            this.product_discount = productDiscount;
            this.result_code = result_code;
            this.result = ResponseMessage.GetResponseMessage(result_code, language);
        }

        public ProductDiscountItemResponse(int result_code, string language)
        {
            this.product_discount = new();
            this.result_code = result_code;
            this.result = ResponseMessage.GetResponseMessage(result_code, language);
        }
    }
}
