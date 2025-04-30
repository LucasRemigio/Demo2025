// // Copyright (c) 2024 Engibots. All rights reserved.

using System.Configuration;
using Engimatrix.ModelObjs;

namespace engimatrix.ModelObjs;

public class OrderDTO : OrderBase
{
    public FilteredEmailDTONew? filtered_email { get; set; }
    public CancelReasonItem? cancel_reason { get; set; }
    public string? municipality_cc { get; set; }
    public string? district_dd { get; set; }
    public ClientDTO? client { get; set; }
    public TransportItem? transport { get; set; }
    public List<OrderProductDTO> products { get; set; } = [];
    public List<OrderRatingDTO> ratings { get; set; } = [];
    public List<OrderRatingChangeRequestDto> rating_change_requests { get; set; } = [];
    public OrderTotalItem order_total { get; set; }
    public StatusItem status { get; set; }


    // Override ToString for debugging or logging
    public override string ToString()
    {
        return base.ToString() +
               $"Cancel Reason: {cancel_reason}\n" +
               $"Products: {products}\n" +
               $"Client: {client}\n" +
               $"Filtered Email: {filtered_email}\n" +
                $"Transport: {transport}\n";
    }

}

public class OrderDTOBuilder
{
    private readonly OrderDTO _orderDTO = new();

    public OrderDTOBuilder SetId(int id)
    {
        _orderDTO.id = id;
        return this;
    }

    public OrderDTOBuilder SetToken(string token)
    {
        _orderDTO.token = token;
        return this;
    }

    public OrderDTOBuilder SetEmailToken(string email_token)
    {
        _orderDTO.email_token = email_token;
        return this;
    }

    public OrderDTOBuilder SetIsDelivery(bool isDelivery)
    {
        _orderDTO.is_delivery = isDelivery;
        return this;
    }

    public OrderDTOBuilder SetPostalCode(string? postal_code)
    {
        _orderDTO.postal_code = postal_code;
        return this;
    }

    public OrderDTOBuilder SetAddress(string? address)
    {
        _orderDTO.address = address;
        return this;
    }

    public OrderDTOBuilder SetCity(string? city)
    {
        _orderDTO.city = city;
        return this;
    }

    public OrderDTOBuilder SetDistrict(string? district)
    {
        _orderDTO.district = district;
        return this;
    }

    public OrderDTOBuilder SetLocality(string? locality)
    {
        _orderDTO.locality = locality;
        return this;
    }

    public OrderDTOBuilder SetIsDraft(bool is_draft)
    {
        _orderDTO.is_draft = is_draft;
        return this;
    }

    public OrderDTOBuilder SetCancelReason(CancelReasonItem? cancel_reason)
    {
        _orderDTO.cancel_reason = cancel_reason;
        return this;
    }

    public OrderDTOBuilder SetCanceledBy(string? canceled_by)
    {
        _orderDTO.canceled_by = canceled_by;
        return this;
    }

    public OrderDTOBuilder SetCanceledAt(DateTime? canceled_at)
    {
        _orderDTO.canceled_at = canceled_at;
        return this;
    }

    public OrderDTOBuilder SetConfirmedBy(string? confirmed_by)
    {
        _orderDTO.confirmed_by = confirmed_by;
        return this;
    }

    public OrderDTOBuilder SetConfirmedAt(DateTime? confirmed_at)
    {
        _orderDTO.confirmed_at = confirmed_at;
        return this;
    }

    public OrderDTOBuilder SetProducts(List<OrderProductDTO> products)
    {
        _orderDTO.products = products;
        return this;
    }

    public OrderDTOBuilder SetClient(ClientDTO client)
    {
        _orderDTO.client = client;
        return this;
    }

    public OrderDTOBuilder SetClientNif(int? client_nif)
    {
        _orderDTO.client_nif = client_nif;
        return this;
    }

    public OrderDTOBuilder SetFilteredEmail(FilteredEmailDTONew filtered_email)
    {
        _orderDTO.filtered_email = filtered_email;
        return this;
    }

    public OrderDTOBuilder SetTransport(TransportItem? transport)
    {
        _orderDTO.transport = transport;
        return this;
    }

    public OrderDTOBuilder SetRatings(List<OrderRatingDTO> ratings)
    {
        _orderDTO.ratings = ratings;
        return this;
    }

    public OrderDTOBuilder SetRatingChangeRequests(List<OrderRatingChangeRequestDto> rating_change_requests)
    {
        _orderDTO.rating_change_requests = rating_change_requests;
        return this;
    }

    public OrderDTOBuilder SetMunicipalityCc(string? municipality_cc)
    {
        _orderDTO.municipality_cc = municipality_cc;
        return this;
    }

    public OrderDTOBuilder SetDistrictDd(string? district_dd)
    {
        _orderDTO.district_dd = district_dd;
        return this;
    }

    public OrderDTOBuilder SetMapsAddress(string? maps_address)
    {
        _orderDTO.maps_address = maps_address;
        return this;
    }

    public OrderDTOBuilder SetDistance(int? distance)
    {
        _orderDTO.distance = distance;
        return this;
    }

    public OrderDTOBuilder SetTravelTime(int? travelTime)
    {
        _orderDTO.travel_time = travelTime;
        return this;
    }

    public OrderDTOBuilder SetObservations(string? observations)
    {
        _orderDTO.observations = observations;
        return this;
    }

    public OrderDTOBuilder SetContact(string? contact)
    {
        _orderDTO.contact = contact;
        return this;
    }

    public OrderDTOBuilder SetOrderTotal(OrderTotalItem order_total)
    {
        _orderDTO.order_total = order_total;
        return this;
    }

    public OrderDTOBuilder SetIsAdjudicated(bool isAdjudicated)
    {
        _orderDTO.is_adjudicated = isAdjudicated;
        return this;
    }

    public OrderDTOBuilder SetStatus(StatusItem? status)
    {
        _orderDTO.status = status;
        return this;
    }

    public OrderDTOBuilder SetCreatedAt(DateTime created_at)
    {
        _orderDTO.created_at = created_at;
        return this;
    }

    public OrderDTOBuilder SetUpdatedAt(DateTime updated_at)
    {
        _orderDTO.updated_at = updated_at;
        return this;
    }

    public OrderDTOBuilder SetCreatedBy(string? created_by)
    {
        _orderDTO.created_by = created_by;
        return this;
    }

    public OrderDTOBuilder SetUpdatedBy(string? updated_by)
    {
        _orderDTO.updated_by = updated_by;
        return this;
    }

    public OrderDTOBuilder SetResolvedBy(string? resolved_by)
    {
        _orderDTO.resolved_by = resolved_by;
        return this;
    }

    public OrderDTOBuilder SetResolvedAt(DateTime? resolved_at)
    {
        _orderDTO.resolved_at = resolved_at;
        return this;
    }

    public OrderDTO Build()
    {
        return _orderDTO;
    }
}



public class OrderDTONoAuth : OrderBase
{
    public FilteredEmailDTO? filtered_email { get; set; }
    public CancelReasonItem? cancel_reason { get; set; }
    public string? municipality_cc { get; set; }
    public string? district_dd { get; set; }
    public ClientNoAuthDTO? client { get; set; }
    public TransportItem? transport { get; set; }
    public List<OrderProductDTONoAuth> products { get; set; } = [];
    public OrderTotalItem order_total { get; set; }
    public bool is_adjudicated { get; set; }
    public StatusItem status { get; set; }
}