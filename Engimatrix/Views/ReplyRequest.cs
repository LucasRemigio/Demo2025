// // Copyright (c) 2024 Engibots. All rights reserved.

using engimatrix.ModelObjs;
using engimatrix.Models;
using engimatrix.ResponseMessages;

namespace engimatrix.Views
{
    public class ReplyRequest
    {
        public string response { get; set; }
        public List<IFormFile>? attachments { get; set; }
        public bool isReplyToOriginalEmail { get; set; }
        public string? cc { get; set; }
        public string? bcc { get; set; }
        public string? destinatary { get; set; }

        public bool IsValid()
        {
            if (string.IsNullOrEmpty(this.response))
            {
                return false;
            }
            return true;
        }
    }
}
