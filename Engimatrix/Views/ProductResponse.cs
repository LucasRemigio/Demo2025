// // Copyright (c) 2024 Engibots. All rights reserved.

using engimatrix.ModelObjs;
using engimatrix.ResponseMessages;
using Engimatrix.ModelObjs;

namespace engimatrix.Views
{
    public class ProductResponse
    {
        public List<ProductItem> products { get; set; }
        public string result { get; set; }
        public int result_code { get; set; }

        public ProductResponse(List<ProductItem> products, int result_code, string language)
        {
            this.products = products;
            this.result = ResponseMessage.GetResponseMessage(result_code, language);
            this.result_code = result_code;
        }

        public ProductResponse(int result_code, string language)
        {
            this.result = ResponseMessage.GetResponseMessage(result_code, language);
            this.result_code = result_code;
        }
    }
}
