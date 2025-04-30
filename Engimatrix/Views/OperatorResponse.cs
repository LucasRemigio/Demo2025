// // Copyright (c) 2024 Engibots. All rights reserved.

using engimatrix.ModelObjs;
using engimatrix.ResponseMessages;

namespace engimatrix.Views
{
    public class OperatorResponse
    {
        public class GetAllOperators
        {
            public List<OperatorItem> operators { get; set; }
            public string result { get; set; }
            public int result_code { get; set; }

            public GetAllOperators(List<OperatorItem> operators, int result_code, string language)
            {
                this.operators = operators;
                this.result_code = result_code;
                this.result = ResponseMessage.GetResponseMessage(result_code, language);
            }

            public GetAllOperators(int result_code, string language)
            {
                this.result_code = result_code;
                this.result = ResponseMessage.GetResponseMessage(result_code, language);
            }
        }
    }
}
