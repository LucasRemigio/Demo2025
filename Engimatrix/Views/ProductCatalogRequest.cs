// // Copyright (c) 2024 Engibots. All rights reserved.

using engimatrix.ModelObjs;
using engimatrix.Models;
using engimatrix.ResponseMessages;

namespace engimatrix.Views
{
    public class ProductCatalogCreateRequest
    {
        public required string product_code { get; set; } 
        public required string description { get; set; }
        public required string unit { get; set; } 
        public required decimal stock_current { get; set; } 
        public required string currency { get; set; } 
        public required decimal price_pvp { get; set; } 
        public required decimal price_avg { get; set; } 
        public required decimal price_last { get; set; } 
        public required DateOnly date_last_entry { get; set; } 
        public required DateOnly date_last_exit { get; set; } 
        public required string family_id { get; set; } 
        public required decimal price_ref_market { get; set; } 

        public bool IsValid()
        {
            return !string.IsNullOrEmpty(product_code) && !string.IsNullOrEmpty(description) && !string.IsNullOrEmpty(unit) &&
                !string.IsNullOrEmpty(currency) && !string.IsNullOrEmpty(family_id);
        }
    }

    public class ProductCatalogUpdateRequest
    {
        public required string product_code { get; set; } 
        public required string description { get; set; } 
        public required string unit { get; set; } 
        public required decimal stock_current { get; set; }
        public required string currency { get; set; } 
        public required decimal price_pvp { get; set; } 
        public required decimal price_avg { get; set; } 
        public required decimal price_last { get; set; } 
        public required DateOnly date_last_entry { get; set; } 
        public required DateOnly date_last_exit { get; set; } 
        public required string family_id { get; set; } 
        public required decimal price_ref_market { get; set; } 

        public bool IsValid()
        {
            return !string.IsNullOrEmpty(product_code) && !string.IsNullOrEmpty(description) && !string.IsNullOrEmpty(unit) &&
                !string.IsNullOrEmpty(currency) && !string.IsNullOrEmpty(family_id);
        }
    }
}
