// // Copyright (c) 2024 Engibots. All rights reserved.

using engimatrix.Utils;

namespace engimatrix.Views
{
    public class ValidateAuth2FCodeRequest
    {
        public string auth2f_code { get; set; }
        public string email { get; set; }

        public bool Validate()
        {
            if (!Util.IsValidInputEmail(this.email) || !Util.IsValidInputString(this.auth2f_code))
            {
                return false;
            }

            return true;
        }
    }
}
