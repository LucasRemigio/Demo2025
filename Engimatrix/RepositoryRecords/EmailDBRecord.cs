// // Copyright (c) 2024 Engibots. All rights reserved.

namespace engimatrix.RepositoryRecords
{
    public class EmailDBRecord
    {
        public int EmailId { get; set; }
        public string SendTo { get; set; }
        public string Subject { get; set; }
        public string EmailTemplate { get; set; }
        public string EmailArgs { get; set; }
        public string EmailLanguage { get; set; }
        public DateTime LastSend { get; set; }
        public int Retries { get; set; }

        public EmailDBRecord(int emailId, string sendTo, string subject, string emailTemplate, string emailArgs, string emailLanguage, DateTime lastSend, int retries)
        {
            this.EmailId = emailId;
            this.SendTo = sendTo;
            this.Subject = subject;
            this.EmailTemplate = emailTemplate;
            this.EmailArgs = emailArgs;
            this.EmailLanguage = emailLanguage;
            this.LastSend = lastSend;
            this.Retries = retries;
        }
    }
}
