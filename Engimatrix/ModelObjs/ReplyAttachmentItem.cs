// // Copyright (c) 2024 Engibots. All rights reserved.

namespace engimatrix.ModelObjs
{
    public class ReplyAttachmentItem
    {
        public string id { get; set; } = "";
        public string reply_token { get; set; } = "";
        public string name { get; set; } = "";
        public string size { get; set; } = "";
        public string file { get; set; } = "";

        public ReplyAttachmentItem() { }

        public ReplyAttachmentItem(string id, string reply_token, string name, string size, string file)
        {
            this.id = id;
            this.reply_token = reply_token;
            this.name = name;
            this.size = size;
            this.file = file;
        }
           
    }
}
