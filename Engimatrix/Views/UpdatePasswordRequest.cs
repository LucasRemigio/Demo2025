// // Copyright (c) 2024 Engibots. All rights reserved.

using engimatrix.Utils;

namespace engimatrix.Views
{
    public class UpdatePasswordRequest
    {
        public string OldPass { get; set; }
        public string NewPass { get; set; }
        public string RepeatNewPass { get; set; }
        public string email { get; set; }

        public bool Validate()
        {
            if (string.IsNullOrEmpty(this.OldPass) || !Util.IsValidInputEmail(this.email) || string.IsNullOrEmpty(this.NewPass) || string.IsNullOrEmpty(this.RepeatNewPass))
            {
                return false;
            }

            return true;
        }
    }
}
