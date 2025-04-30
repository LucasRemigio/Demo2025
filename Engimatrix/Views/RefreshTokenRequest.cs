// // Copyright (c) 2024 Engibots. All rights reserved.

namespace engimatrix.Views
{
    public class RefreshTokenRequest
    {
        public string access_token { get; set; }
        public string a2f_code { get; set; }

        public bool Validate()
        {
            if (string.IsNullOrEmpty(this.access_token))
            {
                return false;
            }

            return true;
        }
    }
}
