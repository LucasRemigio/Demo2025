// // Copyright (c) 2024 Engibots. All rights reserved.

namespace Engimatrix.ModelObjs
{
    public class EmailAttachmentItem
    {
        public string id { get; set; } = "";
        public string email { get; set; } = "";
        public string name { get; set; } = "";
        public string size { get; set; } = "";
        public string file { get; set; } = "";

        public EmailAttachmentItem() { }

        public EmailAttachmentItem(string id, string email, string name, string size, string file)
        {
            this.id = id;
            this.email = email;
            this.name = name;
            this.size = size;
            this.file = file;
        }

        public override string ToString()
        {
            return $"EmailAttachmentItem:\n" +
                   $"ID: {id}\n" +
                   $"Email: {email}\n" +
                   $"Name: {name}\n" +
                   $"Size: {size}\n" +
                   $"File: {file}";
        }
    }
}
