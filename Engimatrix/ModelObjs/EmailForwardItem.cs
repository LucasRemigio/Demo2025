// // Copyright (c) 2024 Engibots. All rights reserved.

using engimatrix.ModelObjs;

namespace engimatrix.ModelObjs
{
    public class EmailForwardItem
    {
        public int id { get; set; }
        public string email_token { get; set; } = "";
        public int? email_id { get; set; } = 0;
        public string forwarded_by { get; set; } = "";
        public string forwarded_to { get; set; } = "";
        public DateTime forwarded_at { get; set; }
        public string? message { get; set; }

        // Default constructor
        public EmailForwardItem()
        { }

        // Parameterized constructor
        public EmailForwardItem(int id, string email_token, int? email_id, string forwarded_by, string forwarded_to, DateTime forwarded_at, string? message)
        {
            this.id = id;
            this.email_token = email_token;
            this.email_id = email_id;
            this.forwarded_by = forwarded_by;
            this.forwarded_to = forwarded_to;
            this.forwarded_at = forwarded_at;
            this.message = message;
        }

        // Method to convert object to string - debugging
        public override string ToString()
        {
            return $"EmailForward:\n" +
                   $"ID: {id}\n" +
                   $"Email Token: {email_token}\n" +
                   $"Email Id: {email_id}\n" +
                   $"Forwarded By: {forwarded_by}\n" +
                   $"Forwarded To: {forwarded_to}\n" +
                   $"Forwarded At: {forwarded_at}" +
                     $"Message: {message}";
        }

        // Check if the EmailForward object is empty
        public bool IsEmpty()
        {
            return id == 0 &&
                   string.IsNullOrEmpty(email_token) &&
                   email_id == 0 &&
                   string.IsNullOrEmpty(forwarded_by) &&
                   string.IsNullOrEmpty(forwarded_to) &&
                   forwarded_at == default(DateTime);
        }
    }
}
