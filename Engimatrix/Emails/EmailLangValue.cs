// // Copyright (c) 2024 Engibots. All rights reserved.

namespace engimatrix.Emails
{
    public class EmailLangValue
    {
        public string Subject { get; set; }
        public string Body { get; set; }

        public EmailLangValue(string body)
        {
            Subject = string.Empty;
            Body = body;
        }

        public EmailLangValue(string subject, string body)
        {
            Subject = subject;
            Body = body;
        }

        public override string ToString()
        {
            return $"Content: {Body}";
        }
    }
}
