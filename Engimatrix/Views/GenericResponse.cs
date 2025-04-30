// // Copyright (c) 2024 Engibots. All rights reserved.

using engimatrix.ResponseMessages;

namespace engimatrix.Views
{
    public class GenericResponse
    {
        public string result { get; set; }
        public int result_code { get; set; }

        public GenericResponse(int result_code, string language)
        {
            this.result_code = result_code;
            this.result = ResponseMessage.GetResponseMessage(result_code, language);
        }
    }
}
