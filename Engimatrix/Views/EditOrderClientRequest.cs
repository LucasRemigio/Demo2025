// // Copyright (c) 2024 Engibots. All rights reserved.

using engimatrix.Utils;

namespace engimatrix.Views
{
    public class EditOrderClientRequest
    {
        public string client_code { get; set; }
        public string client_nif { get; set; }

        public bool IsValid()
        {
            if (!Util.IsValidInputString(this.client_code) || this.client_code.Length > 10 || this.client_code.Length <= 3)
            {
                return false;
            }

            // validate portuguese nif
            if (!Util.IsValidNif(this.client_nif))
            {
                return false;
            }

            return true;
        }
    }
}
