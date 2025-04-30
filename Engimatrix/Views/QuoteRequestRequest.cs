// // Copyright (c) 2024 Engibots. All rights reserved.

using engimatrix.ModelObjs;
using engimatrix.Models;
using engimatrix.ResponseMessages;

namespace engimatrix.Views;

public class QuoteRequestCreateRequest
{
    public required int quote_id_erp { get; set; }
    public required DateOnly quote_date { get; set; } 
    public required int client_id { get; set; } 
    public required string client_name { get; set; } 
    public required string product_code { get; set; } 
    public required decimal quantity_requested { get; set; }
    public required decimal erp_price { get; set; }
    public required decimal erp_price_modification_percent { get; set; } 
    public required bool alert_flag { get; set; } 
    public required bool special_flag { get; set; } 
    public required decimal final_price { get; set; } 
    public required decimal order_quantity { get; set; } 
    public required string order_id { get; set; } 
    public required string observation { get; set; }
    public required decimal unit_price { get; set; } 
    public required decimal margin_percent { get; set; }
    public required decimal price_difference_erp { get; set; } 
    public required decimal price_difference_percent_erp { get; set; } 
    public required decimal total_difference_erp { get; set; }
    public required decimal total_difference_final { get; set; } 

    public bool IsValid()
    {
        return quote_id_erp > 0 && client_id > 0 && quantity_requested > 0 && erp_price > 0 && erp_price_modification_percent > 0;
    }
}

public class QuoteRequestUpdateRequest
{
    public required int quote_id_erp { get; set; } 
    public required DateOnly quote_date { get; set; } 
    public required int client_id { get; set; } 
    public required string client_name { get; set; }
    public required string product_code { get; set; } 
    public required decimal quantity_requested { get; set; }
    public required decimal erp_price { get; set; }
    public required decimal erp_price_modification_percent { get; set; } 
    public required bool alert_flag { get; set; } 
    public required bool special_flag { get; set; }
    public required decimal final_price { get; set; } 
    public required decimal order_quantity { get; set; }
    public required string order_id { get; set; }
    public required string observation { get; set; } 
    public required decimal unit_price { get; set; } 
    public required decimal margin_percent { get; set; } 
    public required decimal price_difference_erp { get; set; } 
    public required decimal price_difference_percent_erp { get; set; } 
    public required decimal total_difference_erp { get; set; } 
    public required decimal total_difference_final { get; set; } 

    public bool IsValid()
    {
        return this.quote_id_erp > 0 && this.client_id > 0 && this.quantity_requested > 0 && this.erp_price > 0 && this.erp_price_modification_percent > 0;
    }
}
