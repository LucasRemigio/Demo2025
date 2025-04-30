// // Copyright (c) 2024 Engibots. All rights reserved.

using Engimatrix.ModelObjs;

namespace engimatrix.ModelObjs
{
    public class ReplyItem
    {
        public int id { get; set; }
        public string email_token { get; set; }
        public string reply_token { get; set; }
        public string from { get; set; }
        public string to { get; set; }
        public string subject { get; set; }
        public string body { get; set; }
        public string date { get; set; }
        public string replied_by { get; set; }
        public string is_read { get; set; }
        public List<ReplyAttachmentItem> attachments { get; set; }

        public ReplyItem(int id, string email_token, string reply_token, string from, string to, string subject, string body, string date, string replied_by, string is_read, List<ReplyAttachmentItem> attachments)
        {
            this.id = id;
            this.email_token = email_token;
            this.reply_token = reply_token;
            this.from = from;
            this.to = to;
            this.subject = subject;
            this.body = body;
            this.date = date;
            this.replied_by = replied_by;
            this.is_read = is_read;
            this.attachments = attachments;
        }

        public ReplyItem(int id, string email_token, string reply_token, string from, string to, string subject, string body, string date, string is_read, string replied_by)
        {
            this.id = id;
            this.email_token = email_token;
            this.reply_token = reply_token;
            this.from = from;
            this.to = to;
            this.subject = subject;
            this.body = body;
            this.date = date;
            this.replied_by = replied_by;
            this.is_read = is_read;
            this.attachments = new List<ReplyAttachmentItem>();
        }

        public ReplyItem()
        { }

        public ReplyItem ToItem()
        {
            return new ReplyItem(this.id, this.email_token, this.reply_token, this.from, this.to, this.subject, this.body, this.date, this.replied_by, this.is_read, this.attachments);
        }
    }

    public class ReplyItemBuilder
    {
        private readonly ReplyItem _replyItem = new();

        public ReplyItemBuilder SetId(int id)
        {
            _replyItem.id = id;
            return this;
        }

        public ReplyItemBuilder SetEmailToken(string email_token)
        {
            _replyItem.email_token = email_token;
            return this;
        }

        public ReplyItemBuilder SetReplyToken(string reply_token)
        {
            _replyItem.reply_token = reply_token;
            return this;
        }

        public ReplyItemBuilder SetFrom(string from)
        {
            _replyItem.from = from;
            return this;
        }

        public ReplyItemBuilder SetTo(string to)
        {
            _replyItem.to = to;
            return this;
        }

        public ReplyItemBuilder SetSubject(string subject)
        {
            _replyItem.subject = subject;
            return this;
        }

        public ReplyItemBuilder SetBody(string body)
        {
            _replyItem.body = body;
            return this;
        }

        public ReplyItemBuilder SetDate(string date)
        {
            _replyItem.date = date;
            return this;
        }

        public ReplyItemBuilder SetRepliedBy(string replied_by)
        {
            _replyItem.replied_by = replied_by;
            return this;
        }

        public ReplyItemBuilder SetIsRead(string is_read)
        {
            _replyItem.is_read = is_read;
            return this;
        }

        public ReplyItemBuilder SetAttachments(List<ReplyAttachmentItem> attachments)
        {
            _replyItem.attachments = attachments;
            return this;
        }

        public ReplyItem Build()
        {
            return _replyItem;
        }
    }
}
