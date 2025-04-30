// // Copyright (c) 2024 Engibots. All rights reserved.

using engimatrix.ResponseMessages;

namespace engimatrix.Views
{
    public class ValidateAuth2FCodeResponse
    {
        public string auth2f_code { get; set; }
        public string result { get; set; }
        public int result_code { get; set; }

        public ValidateAuth2FCodeResponse(string auth2f_code, int result_code, string language)
        {
            this.auth2f_code = auth2f_code;
            this.result_code = result_code;
            this.result = ResponseMessage.GetResponseMessage(result_code, language);
        }
    }
}
