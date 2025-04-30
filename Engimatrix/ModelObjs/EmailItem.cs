// // Copyright (c) 2024 Engibots. All rights reserved.

using engimatrix.ModelObjs;
using static Engimatrix.ModelObjs.SmartSheet;

namespace Engimatrix.ModelObjs
{
    public class EmailItem
    {
        public string id { get; set; } = "";
        public string from { get; set; } = "";
        public string to { get; set; } = "";
        public string cc { get; set; } = "";
        public string bcc { get; set; } = "";
        public string subject { get; set; } = "";
        public string body { get; set; } = "";
        public DateTime date { get; set; }

        public EmailItem(string id, string from, string to, string subject, string body, DateTime date)
        {
            this.id = id;
            this.from = from;
            this.to = to;
            this.subject = subject;
            this.body = body;
            this.date = date;
        }

        public EmailItem(string id, string from, string to, string cc, string bcc, string subject, string body, DateTime date)
        {
            this.id = id;
            this.from = from;
            this.to = to;
            this.cc = cc;
            this.bcc = bcc;
            this.subject = subject;
            this.body = body;
            this.date = date;
        }

        public EmailItem() { }
        public EmailItem ToItem()
        {
            return new EmailItem(this.id, this.from, this.to, this.subject, this.body, this.date);
        }

        public override string ToString()
        {
            return $"EmailItem:\n" +
                   $"ID: {id}\n" +
                   $"From: {from}\n" +
                   $"To: {to}\n" +
                   $"Subject: {subject}\n" +
                   $"Body: {body}\n" +
                   $"Date: {date}";
        }

        public bool IsEmpty()
        {
            return string.IsNullOrEmpty(id) &&
                   string.IsNullOrEmpty(from) &&
                   string.IsNullOrEmpty(to) &&
                   string.IsNullOrEmpty(subject) &&
                   string.IsNullOrEmpty(body) &&
                   date == default(DateTime);
        }
    }

    public class EmailItemBuilder
    {
        private readonly EmailItem emailItem = new();

        public EmailItemBuilder SetId(string id)
        {
            emailItem.id = id;
            return this;
        }

        public EmailItemBuilder SetFrom(string from)
        {
            emailItem.from = from;
            return this;
        }

        public EmailItemBuilder SetTo(string to)
        {
            emailItem.to = to;
            return this;
        }

        public EmailItemBuilder SetCc(string cc)
        {
            emailItem.cc = cc;
            return this;
        }

        public EmailItemBuilder SetBcc(string bcc)
        {
            emailItem.bcc = bcc;
            return this;
        }

        public EmailItemBuilder SetSubject(string subject)
        {
            emailItem.subject = subject;
            return this;
        }

        public EmailItemBuilder SetBody(string body)
        {
            emailItem.body = body;
            return this;
        }

        public EmailItemBuilder SetDate(DateTime date)
        {
            emailItem.date = date;
            return this;
        }

        public EmailItem Build()
        {
            return emailItem;
        }
    }
}
