// // Copyright (c) 2024 Engibots. All rights reserved.

using engimatrix.ModelObjs;
using engimatrix.Models;
using engimatrix.ResponseMessages;

namespace engimatrix.Views
{
    public class ReplyGeneratedByAIResponse
    {
        public string response { get; set; }
        public string result { get; set; }
        public int result_code { get; set; }

        public ReplyGeneratedByAIResponse(string response, int result_code, string language)
        {
            this.response = response;
            this.result_code = result_code;
            this.result = ResponseMessage.GetResponseMessage(result_code, language);
        }

        public ReplyGeneratedByAIResponse(int result_code, string language)
        {
            this.result_code = result_code;
            this.result = ResponseMessage.GetResponseMessage(result_code, language);
        }
    }
}
