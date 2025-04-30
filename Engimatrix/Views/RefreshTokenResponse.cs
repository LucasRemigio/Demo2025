// // Copyright (c) 2024 Engibots. All rights reserved.

using engimatrix.ResponseMessages;
using Engimatrix.ModelObjs;

namespace engimatrix.Views
{
    public class RefreshTokenResponse
    {
        public string user { get; set; }
        public string email { get; set; }
        public string token { get; set; }
        public string role { get; set; }
        public string result { get; set; }
        public int result_code { get; set; }
        public List<DepartmentItem> departments { get; set; }

        public RefreshTokenResponse(string user, string email, string token, string role, string result)
        {
            this.user = user;
            this.email = email;
            this.token = token;
            this.role = role;
            this.result = result;
        }

        public RefreshTokenResponse(string user, string email, string token, string role, int result_code, string language)
        {
            this.user = user;
            this.email = email;
            this.token = token;
            this.role = role;
            this.result_code = result_code;
            this.result = ResponseMessage.GetResponseMessage(result_code, language);
            this.departments = new List<DepartmentItem>();
        }

        public RefreshTokenResponse(string user, string email, string token, string role, List<DepartmentItem> departments, int result_code, string language)
        {
            this.user = user;
            this.email = email;
            this.token = token;
            this.role = role;
            this.result_code = result_code;
            this.result = ResponseMessage.GetResponseMessage(result_code, language);
            this.departments = departments;
        }
    }
}
