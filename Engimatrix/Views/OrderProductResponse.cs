// // Copyright (c) 2024 Engibots. All rights reserved.

using System.Text.RegularExpressions;
using engimatrix.ModelObjs;
using engimatrix.Utils;
using Engimatrix.ModelObjs;

namespace engimatrix.Views
{
    public class OrderProductResponse : GenericResponse
    {
        public List<OrderProductItem> products { get; set; }

        public OrderProductResponse(List<OrderProductItem> products, int result_code, string language)
            : base(result_code, language)
        {
            this.products = products;
        }

        public OrderProductResponse(int result_code, string language)
            : base(result_code, language)
        {
            this.products = [];
        }
    }

    public class ClientPatchResponse : OrderProductResponse
    {
        public AddressFillingDetails? address_filling_details { get; set; }
        public DestinationDetailsItem? destination_details { get; set; }
        public OrderRatingItem? logistic_rating { get; set; }

        public ClientPatchResponse(List<OrderProductItem> products, AddressFillingDetails? address_filling_details, DestinationDetailsItem? destination_details, OrderRatingItem? logistic_rating, int result_code, string language)
            : base(products, result_code, language)
        {
            this.address_filling_details = address_filling_details;
            this.destination_details = destination_details;
            this.logistic_rating = logistic_rating;
        }

        public ClientPatchResponse(int result_code, string language)
            : base(result_code, language)
        {
        }
    }

    public class OrderProductUpdateResponse : OrderProductResponse
    {
        public List<OrderRatingDTO> order_ratings { get; set; }
        public OrderTotalItem? order_total { get; set; }

        public OrderProductUpdateResponse(List<OrderProductItem> products, OrderTotalItem order_total, List<OrderRatingDTO> order_ratings, int result_code, string language)
            : base(products, result_code, language)
        {
            this.order_ratings = order_ratings;
            this.order_total = order_total;
        }

        public OrderProductUpdateResponse(int result_code, string language)
            : base(result_code, language)
        {
        }
    }

    public class OrderProductUpdateResponseNoAuth : OrderProductResponse
    {
        public OrderTotalItem? order_total { get; set; }

        public OrderProductUpdateResponseNoAuth(List<OrderProductItem> products, OrderTotalItem order_total, int result_code, string language)
            : base(products, result_code, language)
        {
            this.order_total = order_total;
        }

        public OrderProductUpdateResponseNoAuth(int result_code, string language)
            : base(result_code, language)
        {
        }
    }
}
