// // Copyright (c) 2024 Engibots. All rights reserved.

namespace engimatrix.ModelObjs;

public class CancelReasonItem
{
    public int id { get; set; }
    public string reason { get; set; }
    public string slug { get; set; }
    public string description { get; set; }
    public bool is_active { get; set; }
    public DateTime created_at { get; set; }
    public DateTime updated_at { get; set; }

    // Optional: ToString Method for Debugging
    public override string ToString()
    {
        return $"id: {id}, reason: {reason}, slug: {slug}, description: {description}, is_active: {is_active}, created_at: {created_at}, updated_at: {updated_at}";
    }
}

public class CancelReasonItemBuilder
{
    private readonly CancelReasonItem _cancelReasonItem = new();

    public CancelReasonItemBuilder SetId(int id)
    {
        _cancelReasonItem.id = id;
        return this;
    }

    public CancelReasonItemBuilder SetReason(string reason)
    {
        _cancelReasonItem.reason = reason;
        return this;
    }

    public CancelReasonItemBuilder SetSlug(string slug)
    {
        _cancelReasonItem.slug = slug;
        return this;
    }

    public CancelReasonItemBuilder SetDescription(string description)
    {
        _cancelReasonItem.description = description;
        return this;
    }

    public CancelReasonItemBuilder SetIsActive(bool is_active)
    {
        _cancelReasonItem.is_active = is_active;
        return this;
    }

    public CancelReasonItemBuilder SetCreatedAt(DateTime created_at)
    {
        _cancelReasonItem.created_at = created_at;
        return this;
    }

    public CancelReasonItemBuilder SetUpdatedAt(DateTime updated_at)
    {
        _cancelReasonItem.updated_at = updated_at;
        return this;
    }

    public CancelReasonItem Build()
    {
        return _cancelReasonItem;
    }
}