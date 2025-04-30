// // Copyright (c) 2024 Engibots. All rights reserved.

using engimatrix.Utils;

namespace engimatrix.Views
{
    public class NotificationsRequest
    {
        public string email { get; set; }
        public string subject { get; set; }
        public string body { get; set; }
        public string source_email { get; set; }

        public bool Validate()
        {
            if (!Util.IsValidInputEmail(this.email) || !Util.IsValidInputString(this.subject) || string.IsNullOrEmpty(this.body) || !Util.IsValidInputEmail(this.source_email))
            {
                return false;
            }

            return true;
        }
    }
}
