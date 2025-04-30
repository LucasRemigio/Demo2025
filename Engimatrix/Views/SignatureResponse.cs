// // Copyright (c) 2024 Engibots. All rights reserved.

using engimatrix.ModelObjs;
using engimatrix.Models;
using engimatrix.ResponseMessages;

namespace engimatrix.Views
{
    public class SignatureResponse
    {
        public SignatureItem signature { get; set; }
        public string result { get; set; }
        public int result_code { get; set; }

        public SignatureResponse(SignatureItem signature, int result_code, string language)
        {
            this.signature = signature;
            this.result_code = result_code;
            this.result = ResponseMessage.GetResponseMessage(result_code, language);
        }

        public SignatureResponse(int result_code, string language)
        {
            this.result_code = result_code;
            this.result = ResponseMessage.GetResponseMessage(result_code, language);
        }
    }
}
