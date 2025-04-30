// // Copyright (c) 2024 Engibots. All rights reserved.

using engimatrix.ModelObjs;

namespace Engimatrix.ModelObjs
{
    public class FilteredEmail
    {
        public EmailItem email { get; set; } = new EmailItem();
        public string category { get; set; } = "";
        public string status { get; set; } = "";
        public DateTime date { get; set; }
        public string token { get; set; } = "";
        public string confidence { get; set; } = "";
        public string validated { get; set; } = "0";
        public int reply_count { get; set; }
        public int unread_replies_count { get; set; }
        public string replied_by { get; set; }
        public DateTime replied_at { get; set; }
        public string resolved_by { get; set; }
        public DateTime resolved_at { get; set; }
        public List<ProductItem> products { get; set; }
        public string forwarded_by { get; set; }
        public DateTime forwarded_at { get; set; }

        public FilteredEmail()
        { }

        public FilteredEmail(EmailItem email, string category, string status, DateTime date, string token, string confidence, string validated, int replyCount, int unreadRepliesCount, string repliedBy, DateTime repliedAt, string resolvedBy, DateTime resolved_at, string forwardedBy, DateTime forwardedAt)
        {
            this.email = email;
            this.category = category;
            this.status = status;
            this.date = date;
            this.token = token;
            this.confidence = confidence;
            this.validated = validated;
            this.reply_count = replyCount;
            this.unread_replies_count = unreadRepliesCount;
            this.replied_by = repliedBy;
            this.replied_at = repliedAt;
            this.resolved_by = resolvedBy;
            this.resolved_at = resolved_at;
            this.forwarded_by = forwardedBy;
            this.forwarded_at = forwardedAt;
        }

        public FilteredEmail(EmailItem email, string category, string status, DateTime date, string token, string confidence, string validated)
        {
            this.email = email;
            this.category = category;
            this.status = status;
            this.date = date;
            this.token = token;
            this.confidence = confidence;
            this.validated = validated;
        }

        public FilteredEmail(EmailItem email, string category, string status, DateTime date, string token, string confidence, string validated, string resolvedBy, DateTime resolvedAt)
        {
            this.email = email;
            this.category = category;
            this.status = status;
            this.date = date;
            this.token = token;
            this.confidence = confidence;
            this.validated = validated;
            this.resolved_by = resolvedBy;
            this.resolved_at = resolvedAt;
        }

        public FilteredEmail(EmailItem email, string category, string status, DateTime date, string token, string confidence, string validated, string resolved_by, DateTime resolved_at, List<ProductItem> products)
        {
            this.email = email;
            this.category = category;
            this.status = status;
            this.date = date;
            this.token = token;
            this.confidence = confidence;
            this.validated = validated;
            this.resolved_by = resolved_by;
            this.resolved_at = resolved_at;
            this.products = products;
        }

        public FilteredEmail toItem()
        {
            return new FilteredEmail(this.email, this.category, this.status, this.date, this.token, this.confidence, this.validated, this.reply_count, this.unread_replies_count, this.replied_by, this.replied_at, this.resolved_by, this.resolved_at, this.forwarded_by, this.forwarded_at);
        }

        public bool IsEmpty()
        {
            return // EmailItem
                String.IsNullOrEmpty(email.id) &&
                String.IsNullOrEmpty(email.from) &&
                String.IsNullOrEmpty(email.to) &&
                String.IsNullOrEmpty(email.subject) &&
                String.IsNullOrEmpty(email.body) &&
                email.date == DateTime.MinValue &&
                // Filtered Email
                String.IsNullOrEmpty(category) &&
                String.IsNullOrEmpty(status) &&
                date == DateTime.MinValue &&
                String.IsNullOrEmpty(token) &&
                String.IsNullOrEmpty(confidence);
        }
    }

    public class FilteredEmailItemBuilder
    {
        private readonly FilteredEmail filteredEmail = new();

        public FilteredEmailItemBuilder SetEmail(EmailItem email)
        {
            filteredEmail.email = email;
            return this;
        }

        public FilteredEmailItemBuilder SetCategory(string category)
        {
            filteredEmail.category = category;
            return this;
        }

        public FilteredEmailItemBuilder SetStatus(string status)
        {
            filteredEmail.status = status;
            return this;
        }

        public FilteredEmailItemBuilder SetDate(DateTime date)
        {
            filteredEmail.date = date;
            return this;
        }

        public FilteredEmailItemBuilder SetToken(string token)
        {
            filteredEmail.token = token;
            return this;
        }

        public FilteredEmailItemBuilder SetConfidence(string confidence)
        {
            filteredEmail.confidence = confidence;
            return this;
        }

        public FilteredEmailItemBuilder SetValidated(string validated)
        {
            filteredEmail.validated = validated;
            return this;
        }

        public FilteredEmailItemBuilder SetReplyCount(int replyCount)
        {
            filteredEmail.reply_count = replyCount;
            return this;
        }

        public FilteredEmailItemBuilder SetUnreadRepliesCount(int unreadRepliesCount)
        {
            filteredEmail.unread_replies_count = unreadRepliesCount;
            return this;
        }

        public FilteredEmailItemBuilder SetRepliedBy(string repliedBy)
        {
            filteredEmail.replied_by = repliedBy;
            return this;
        }

        public FilteredEmailItemBuilder SetRepliedAt(DateTime repliedAt)
        {
            filteredEmail.replied_at = repliedAt;
            return this;
        }

        public FilteredEmailItemBuilder SetResolvedBy(string resolvedBy)
        {
            filteredEmail.resolved_by = resolvedBy;
            return this;
        }

        public FilteredEmailItemBuilder SetResolvedAt(DateTime resolvedAt)
        {
            filteredEmail.resolved_at = resolvedAt;
            return this;
        }

        public FilteredEmailItemBuilder SetForwardedBy(string forwardedBy)
        {
            filteredEmail.forwarded_by = forwardedBy;
            return this;
        }

        public FilteredEmailItemBuilder SetForwardedAt(DateTime forwardedAt)
        {
            filteredEmail.forwarded_at = forwardedAt;
            return this;
        }

        public FilteredEmailItemBuilder SetProducts(List<ProductItem> products)
        {
            filteredEmail.products = products;
            return this;
        }

        public FilteredEmail Build()
        {
            return filteredEmail;

        }
    }
}
