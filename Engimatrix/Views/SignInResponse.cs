// // Copyright (c) 2024 Engibots. All rights reserved.

using engimatrix.ResponseMessages;
using Engimatrix.ModelObjs;

namespace engimatrix.Views
{
    public class SignInResponse
    {
        public string user { get; set; }
        public string email { get; set; }
        public string token { get; set; }
        public string role { get; set; }
        public string result { get; set; }
        public int result_code { get; set; }
        public bool useAuth2f { get; set; }
        public List<DepartmentItem> departments { get; set; }

        public SignInResponse(string user, string email, string token, string role, string result)
        {
            this.user = user;
            this.email = email;
            this.token = token;
            this.role = role;
            this.result = result;
            this.departments = new List<DepartmentItem>();
        }

        public SignInResponse(string user, string email, string token, string role, int result_code, string language, bool useAuth2f)
        {
            this.user = user;
            this.email = email;
            this.token = token;
            this.role = role;
            this.result_code = result_code;
            this.result = ResponseMessage.GetResponseMessage(result_code, language);
            this.useAuth2f = useAuth2f;
            this.departments = new List<DepartmentItem>();
        }

        public SignInResponse(string user, string email, string token, string role, List<DepartmentItem> departments, int result_code, string language, bool useAuth2f)
        {
            this.user = user;
            this.email = email;
            this.token = token;
            this.role = role;
            this.result_code = result_code;
            this.result = ResponseMessage.GetResponseMessage(result_code, language);
            this.useAuth2f = useAuth2f;
            this.departments = departments;
        }
    }
}
