// // Copyright (c) 2024 Engibots. All rights reserved.

using engimatrix.Utils;

namespace engimatrix.Views
{
    public class SignInRequest
    {
        public string email { get; set; }
        public string password { get; set; }

        public bool Validate()
        {
            if (!Util.IsValidInputEmail(this.email) || !Util.IsValidInputString(this.password))
            {
                return false;
            }

            return true;
        }
    }
}
