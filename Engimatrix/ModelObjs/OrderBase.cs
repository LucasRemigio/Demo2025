// // Copyright (c) 2024 Engibots. All rights reserved.

namespace engimatrix.ModelObjs;

public class OrderBase
{
    public int id { get; set; }
    public string token { get; set; }
    public string? email_token { get; set; }
    public bool is_delivery { get; set; }
    public string? postal_code { get; set; }
    public string? address { get; set; }
    public string? city { get; set; } // or municipality
    public string? district { get; set; }
    public string? locality { get; set; } // the postal code locality 
    public string? maps_address { get; set; }
    public int? distance { get; set; }
    public int? travel_time { get; set; }
    public bool is_draft { get; set; }
    public string? canceled_by { get; set; }
    public DateTime? canceled_at { get; set; }
    public string? confirmed_by { get; set; }
    public DateTime? confirmed_at { get; set; }
    public string? observations { get; set; }
    public string? contact { get; set; }
    public bool is_adjudicated { get; set; }
    public DateTime? adjudicated_at { get; set; }
    public DateTime created_at { get; set; }
    public DateTime? updated_at { get; set; }
    public string? created_by { get; set; }
    public string? updated_by { get; set; }
    public int? client_nif { get; set; }
    public string? resolved_by { get; set; }
    public DateTime? resolved_at { get; set; }

    public override string ToString()
    {
        return $"OrderItem:\n" +
               $"ID: {id}\n" +
               $"Token: {token}\n" +
               $"Postal Code: {postal_code}\n" +
               $"Address: {address}\n" +
               $"City: {city}\n" +
               $"Is Draft: {is_draft}\n" +
               $"Canceled By: {canceled_by}\n" +
               $"Canceled At: {canceled_at}\n";
    }

    // Check if the object is empty
    public bool IsEmpty()
    {
        return id == 0 &&
               string.IsNullOrEmpty(token);
    }
}
