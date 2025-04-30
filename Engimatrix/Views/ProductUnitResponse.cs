// // Copyright (c) 2024 Engibots. All rights reserved.

using engimatrix.ModelObjs;
using engimatrix.Models;
using engimatrix.ResponseMessages;

namespace engimatrix.Views
{
    public class ProductUnitResponse
    {
        public List<ProductUnitItem> product_units { get; set; }
        public string result { get; set; }
        public int result_code { get; set; }

        public ProductUnitResponse(List<ProductUnitItem> product_units, int result_code, string language)
        {
            this.product_units = product_units;
            this.result_code = result_code;
            this.result = ResponseMessage.GetResponseMessage(result_code, language);
        }

        public ProductUnitResponse(int result_code, string language)
        {
            this.result_code = result_code;
            this.result = ResponseMessage.GetResponseMessage(result_code, language);
        }
    }
}
