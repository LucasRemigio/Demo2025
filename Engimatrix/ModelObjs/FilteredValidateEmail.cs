// // Copyright (c) 2024 Engibots. All rights reserved.

namespace Engimatrix.ModelObjs
{
    public class FilteredValidateEmail
    {
        public EmailItem email { get; set; } = new EmailItem();

        public string category { get; set; } = "";

        public string status { get; set; } = "";

        public string date { get; set; } = "";

        public string confidence { get; set; } = "";

        public string reason { get; set; } = "";

        public FilteredValidateEmail() { }
        public FilteredValidateEmail(EmailItem email, string category, string status, string date, string confidence)
        {
            this.email = email;
            this.category = category;
            this.status = status;
            this.date = date;
            this.confidence = confidence;
        }

        public FilteredValidateEmail toItem()
        {
            return new FilteredValidateEmail(this.email, this.category, this.status, this.date, this.confidence);
        }
    }
}
