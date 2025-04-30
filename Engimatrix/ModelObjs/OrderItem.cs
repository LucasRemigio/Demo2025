// // Copyright (c) 2024 Engibots. All rights reserved.

namespace engimatrix.ModelObjs;

public class OrderItem : OrderBase
{
    public int cancel_reason_id { get; set; }
    public string client_code { get; set; }
    public int transport_id { get; set; }
    public int status_id { get; set; }

    public override string ToString()
    {
        return base.ToString() +
               $"Cancel Reason ID: {cancel_reason_id}\n" +
               $"Transport ID: {transport_id}\n" +
               $"Client Code: {client_code}\n";
    }
}

public class OrderItemBuilder
{
    private readonly OrderItem _orderItem = new();

    public OrderItemBuilder SetId(int id)
    {
        _orderItem.id = id;
        return this;
    }

    public OrderItemBuilder SetToken(string token)
    {
        _orderItem.token = token;
        return this;
    }

    public OrderItemBuilder SetEmailToken(string? emailToken)
    {
        _orderItem.email_token = emailToken;
        return this;
    }

    public OrderItemBuilder SetIsDelivery(bool isDelivery)
    {
        _orderItem.is_delivery = isDelivery;
        return this;
    }

    public OrderItemBuilder SetPostalCode(string postalCode)
    {
        _orderItem.postal_code = postalCode;
        return this;
    }

    public OrderItemBuilder SetAddress(string address)
    {
        _orderItem.address = address;
        return this;
    }

    public OrderItemBuilder SetCity(string city)
    {
        _orderItem.city = city;
        return this;
    }

    public OrderItemBuilder SetDistrict(string district)
    {
        _orderItem.district = district;
        return this;
    }

    public OrderItemBuilder SetLocality(string locality)
    {
        _orderItem.locality = locality;
        return this;
    }

    public OrderItemBuilder SetIsDraft(bool isDraft)
    {
        _orderItem.is_draft = isDraft;
        return this;
    }

    public OrderItemBuilder SetCancelReasonId(int cancelReasonId)
    {
        _orderItem.cancel_reason_id = cancelReasonId;
        return this;
    }

    public OrderItemBuilder SetCanceledBy(string canceledBy)
    {
        _orderItem.canceled_by = canceledBy;
        return this;
    }

    public OrderItemBuilder SetCanceledAt(DateTime? canceledAt)
    {
        _orderItem.canceled_at = canceledAt;
        return this;
    }

    public OrderItemBuilder SetConfirmedBy(string confirmedBy)
    {
        _orderItem.confirmed_by = confirmedBy;
        return this;
    }

    public OrderItemBuilder SetConfirmedAt(DateTime? confirmedAt)
    {
        _orderItem.confirmed_at = confirmedAt;
        return this;
    }

    public OrderItemBuilder SetClientCode(string clientCode)
    {
        _orderItem.client_code = clientCode;
        return this;
    }

    public OrderItemBuilder SetClientNif(int? clientNif)
    {
        _orderItem.client_nif = clientNif;
        return this;
    }

    public OrderItemBuilder SetTransportId(int transportId)
    {
        _orderItem.transport_id = transportId;
        return this;
    }

    public OrderItemBuilder SetMapsAddress(string? mapsAddress)
    {
        _orderItem.maps_address = mapsAddress;
        return this;
    }

    public OrderItemBuilder SetDistance(int? distance)
    {
        _orderItem.distance = distance;
        return this;
    }

    public OrderItemBuilder SetTravelTime(int? travelTime)
    {
        _orderItem.travel_time = travelTime;
        return this;
    }

    public OrderItemBuilder SetObservations(string? observations)
    {
        _orderItem.observations = observations;
        return this;
    }

    public OrderItemBuilder SetContact(string? contact)
    {
        _orderItem.contact = contact;
        return this;
    }

    public OrderItemBuilder SetIsAdjudicated(bool isAdjudicated)
    {
        _orderItem.is_adjudicated = isAdjudicated;
        return this;
    }

    public OrderItemBuilder SetAdjudicatedAt(DateTime? adjudicatedAt)
    {
        _orderItem.adjudicated_at = adjudicatedAt;
        return this;
    }

    public OrderItemBuilder SetCreatedAt(DateTime createdAt)
    {
        _orderItem.created_at = createdAt;
        return this;
    }

    public OrderItemBuilder SetUpdatedAt(DateTime? updatedAt)
    {
        _orderItem.updated_at = updatedAt;
        return this;
    }

    public OrderItemBuilder SetCreatedBy(string? createdBy)
    {
        _orderItem.created_by = createdBy;
        return this;
    }

    public OrderItemBuilder SetUpdatedBy(string? updatedBy)
    {
        _orderItem.updated_by = updatedBy;
        return this;
    }

    public OrderItemBuilder SetStatusId(int statusId)
    {
        _orderItem.status_id = statusId;
        return this;
    }

    public OrderItemBuilder SetResolvedBy(string? resolvedBy)
    {
        _orderItem.resolved_by = resolvedBy;
        return this;
    }

    public OrderItemBuilder SetResolvedAt(DateTime? resolvedAt)
    {
        _orderItem.resolved_at = resolvedAt;
        return this;
    }

    public OrderItem Build()
    {
        return _orderItem;
    }

}