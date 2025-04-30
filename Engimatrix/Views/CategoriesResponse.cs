// // Copyright (c) 2024 Engibots. All rights reserved.

using engimatrix.ModelObjs;
using engimatrix.ResponseMessages;

namespace engimatrix.Views
{
    public class CategoriesResponse
    {
        public class GetAllCategoriesResponse
        {
            public List<CategoriesItem> categories { get; set; }
            public string result { get; set; }
            public int result_code { get; set; }

            public GetAllCategoriesResponse(List<CategoriesItem> categories, int result_code, string language)
            {
                this.categories = categories;
                this.result_code = result_code;
                this.result = ResponseMessage.GetResponseMessage(result_code, language);
            }

            public GetAllCategoriesResponse(int result_code, string language)
            {
                this.result_code = result_code;
                this.result = ResponseMessage.GetResponseMessage(result_code, language);
            }
        }
    }
}
