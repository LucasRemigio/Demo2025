// // Copyright (c) 2024 Engibots. All rights reserved.

using engimatrix.Utils;

namespace engimatrix.Views
{
    public class SendNewAuth2FCodeRequest
    {
        public string email { get; set; }

        public bool Validate()
        {
            if (!Util.IsValidInputEmail(this.email))
            {
                return false;
            }

            return true;
        }
    }
}
