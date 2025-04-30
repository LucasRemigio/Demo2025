// // Copyright (c) 2024 Engibots. All rights reserved.

using engimatrix.Utils;

namespace engimatrix.Views;

public class EmailRequest
{
    public string mailbox { get; set; }
    public string to { get; set; }
    public string subject { get; set; }
    public string body { get; set; }
    public string? cc { get; set; }
    public string? bcc { get; set; }
    public List<IFormFile>? attachments { get; set; }

    public bool Validate()
    {
        if (!Util.IsValidInputEmail(this.to) || String.IsNullOrWhiteSpace(this.subject) || String.IsNullOrWhiteSpace(this.body) || String.IsNullOrEmpty(mailbox))
        {
            return false;
        }

        return true;
    }
}
