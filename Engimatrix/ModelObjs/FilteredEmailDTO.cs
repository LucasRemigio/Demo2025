// // Copyright (c) 2024 Engibots. All rights reserved.

using engimatrix.ModelObjs;

namespace Engimatrix.ModelObjs;

public class FilteredEmailDTO
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
    public string forwarded_by { get; set; }
    public DateTime forwarded_at { get; set; }

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

public class FilteredEmailDTOBuilder
{
    private readonly FilteredEmailDTO _filteredEmailDTO = new();

    public FilteredEmailDTOBuilder SetEmail(EmailItem email)
    {
        _filteredEmailDTO.email = email;
        return this;
    }

    public FilteredEmailDTOBuilder SetCategory(string category)
    {
        _filteredEmailDTO.category = category;
        return this;
    }

    public FilteredEmailDTOBuilder SetStatus(string status)
    {
        _filteredEmailDTO.status = status;
        return this;
    }

    public FilteredEmailDTOBuilder SetDate(DateTime date)
    {
        _filteredEmailDTO.date = date;
        return this;
    }

    public FilteredEmailDTOBuilder SetToken(string token)
    {
        _filteredEmailDTO.token = token;
        return this;
    }

    public FilteredEmailDTOBuilder SetConfidence(string confidence)
    {
        _filteredEmailDTO.confidence = confidence;
        return this;
    }

    public FilteredEmailDTOBuilder SetValidated(string validated)
    {
        _filteredEmailDTO.validated = validated;
        return this;
    }

    public FilteredEmailDTOBuilder SetReplyCount(int reply_count)
    {
        _filteredEmailDTO.reply_count = reply_count;
        return this;
    }

    public FilteredEmailDTOBuilder SetUnreadRepliesCount(int unread_replies_count)
    {
        _filteredEmailDTO.unread_replies_count = unread_replies_count;
        return this;
    }

    public FilteredEmailDTOBuilder SetRepliedBy(string replied_by)
    {
        _filteredEmailDTO.replied_by = replied_by;
        return this;
    }

    public FilteredEmailDTOBuilder SetRepliedAt(DateTime replied_at)
    {
        _filteredEmailDTO.replied_at = replied_at;
        return this;
    }

    public FilteredEmailDTOBuilder SetResolvedBy(string resolved_by)
    {
        _filteredEmailDTO.resolved_by = resolved_by;
        return this;
    }

    public FilteredEmailDTOBuilder SetResolvedAt(DateTime resolved_at)
    {
        _filteredEmailDTO.resolved_at = resolved_at;
        return this;
    }

    public FilteredEmailDTOBuilder SetForwardedBy(string forwarded_by)
    {
        _filteredEmailDTO.forwarded_by = forwarded_by;
        return this;
    }

    public FilteredEmailDTOBuilder SetForwardedAt(DateTime forwarded_at)
    {
        _filteredEmailDTO.forwarded_at = forwarded_at;
        return this;
    }

    public FilteredEmailDTO Build()
    {
        return _filteredEmailDTO;
    }
}

public class FilteredEmailDTONew
{
    public EmailItem email { get; set; }
    public CategoryItem category { get; set; }
    public StatusItem status { get; set; }
    public DateTime date { get; set; }
    public string token { get; set; }
    public int confidence { get; set; }
    public bool validated { get; set; }
    public int? reply_count { get; set; }
    public int? unread_replies_count { get; set; }
    public string? replied_by { get; set; }
    public DateTime? replied_at { get; set; }
    public string? resolved_by { get; set; }
    public DateTime? resolved_at { get; set; }
    public string? forwarded_by { get; set; }
    public DateTime? forwarded_at { get; set; }
}

public class FilteredEmailDTONewBuilder
{
    private readonly FilteredEmailDTONew _filteredEmailDTO = new();

    public FilteredEmailDTONewBuilder SetEmail(EmailItem email)
    {
        _filteredEmailDTO.email = email;
        return this;
    }

    public FilteredEmailDTONewBuilder SetCategory(CategoryItem category)
    {
        _filteredEmailDTO.category = category;
        return this;
    }

    public FilteredEmailDTONewBuilder SetStatus(StatusItem status)
    {
        _filteredEmailDTO.status = status;
        return this;
    }

    public FilteredEmailDTONewBuilder SetDate(DateTime date)
    {
        _filteredEmailDTO.date = date;
        return this;
    }

    public FilteredEmailDTONewBuilder SetToken(string token)
    {
        _filteredEmailDTO.token = token;
        return this;
    }

    public FilteredEmailDTONewBuilder SetConfidence(int confidence)
    {
        _filteredEmailDTO.confidence = confidence;
        return this;
    }

    public FilteredEmailDTONewBuilder SetValidated(bool validated)
    {
        _filteredEmailDTO.validated = validated;
        return this;
    }

    public FilteredEmailDTONewBuilder SetReplyCount(int reply_count)
    {
        _filteredEmailDTO.reply_count = reply_count;
        return this;
    }

    public FilteredEmailDTONewBuilder SetUnreadRepliesCount(int unread_replies_count)
    {
        _filteredEmailDTO.unread_replies_count = unread_replies_count;
        return this;
    }

    public FilteredEmailDTONewBuilder SetRepliedBy(string? replied_by)
    {
        _filteredEmailDTO.replied_by = replied_by;
        return this;
    }

    public FilteredEmailDTONewBuilder SetRepliedAt(DateTime? replied_at)
    {
        _filteredEmailDTO.replied_at = replied_at;
        return this;
    }

    public FilteredEmailDTONewBuilder SetResolvedBy(string? resolved_by)
    {
        _filteredEmailDTO.resolved_by = resolved_by;
        return this;
    }

    public FilteredEmailDTONewBuilder SetResolvedAt(DateTime? resolved_at)
    {
        _filteredEmailDTO.resolved_at = resolved_at;
        return this;
    }

    public FilteredEmailDTONewBuilder SetForwardedBy(string? forwarded_by)
    {
        _filteredEmailDTO.forwarded_by = forwarded_by;
        return this;
    }

    public FilteredEmailDTONewBuilder SetForwardedAt(DateTime? forwarded_at)
    {
        _filteredEmailDTO.forwarded_at = forwarded_at;
        return this;
    }

    public FilteredEmailDTONew Build()
    {
        return _filteredEmailDTO;
    }

}



