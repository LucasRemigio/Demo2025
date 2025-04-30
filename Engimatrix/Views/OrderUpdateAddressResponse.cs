// // Copyright (c) 2024 Engibots. All rights reserved.

using System.Text.RegularExpressions;
using engimatrix.ModelObjs;
using engimatrix.Utils;

namespace engimatrix.Views
{
    public class OrderUpdateAddressResponse : GenericResponse
    {
        public DestinationDetailsItem? destination_details { get; set; }
        public OrderRatingItem? logistic_rating { get; set; }

        public OrderUpdateAddressResponse(DestinationDetailsItem details, OrderRatingItem logistic_rating, int result_code, string language)
            : base(result_code, language)
        {
            this.destination_details = details;
            this.logistic_rating = logistic_rating;
        }

        public OrderUpdateAddressResponse(int result_code, string language)
            : base(result_code, language)
        {
        }
    }
}
