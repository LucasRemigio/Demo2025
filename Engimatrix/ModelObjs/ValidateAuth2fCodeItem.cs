// // Copyright (c) 2024 Engibots. All rights reserved.

namespace engimatrix.ModelObjs
{
    public class ValidateAuth2fCodeItem
    {
        public string user_email { get; set; }
        public string token { get; set; }
        public string created_at { get; set; }

        public ValidateAuth2fCodeItem(string user_email, string token, string created_at)
        {
            this.user_email = user_email;
            this.token = token;
            this.created_at = created_at;
        }
    }
}
