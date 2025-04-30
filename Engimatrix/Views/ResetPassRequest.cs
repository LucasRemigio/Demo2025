// // Copyright (c) 2024 Engibots. All rights reserved.

using engimatrix.Utils;

namespace engimatrix.Views
{
    public class ResetPassRequest
    {
        public string email { get; set; }
        public string token { get; set; }
        public string password { get; set; }

        public bool Validate()
        {
            if (!Util.IsValidInputEmail(this.email) && !string.IsNullOrEmpty(token) && !string.IsNullOrEmpty(password))
            {
                return false;
            }

            return true;
        }
    }
}
