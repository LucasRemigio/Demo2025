// // Copyright (c) 2024 Engibots. All rights reserved.

using engimatrix.ModelObjs;
using engimatrix.Models;
using engimatrix.ResponseMessages;

namespace engimatrix.Views
{
    public class SimpleMessageResponse
    {
        public string message { get; set; }
        public string result { get; set; }
        public int result_code { get; set; }

        public SimpleMessageResponse(string message, int result_code, string language)
        {
            this.message = message;
            this.result_code = result_code;
            this.result = ResponseMessage.GetResponseMessage(result_code, language);
        }

        public SimpleMessageResponse(int result_code, string language)
        {
            this.result_code = result_code;
            this.result = ResponseMessage.GetResponseMessage(result_code, language);
        }
    }
}
