// // Copyright (c) 2024 Engibots. All rights reserved.

namespace engimatrix.ModelObjs
{
    public class OrderRatingChangeRequestItem
    {
        public int id { get; set; }
        public string order_token { get; set; }
        public int rating_type_id { get; set; }
        public char new_rating { get; set; }
        public string status { get; set; }
        public DateTime? requested_at { get; set; }
        public string? requested_by { get; set; }
        public DateTime? verified_at { get; set; }
        public string? verified_by { get; set; }

        public override string ToString()
        {
            return $"OrderRatingChangeRequestItem(id: {id}, order_token: {order_token}, rating_type_id: {rating_type_id}, new_rating: {new_rating}, status: {status}, requested_at: {requested_at}, requested_by: {requested_by}, verified_at: {verified_at}, verified_by: {verified_by})";
        }

    }

    public class OrderRatingChangeRequestItemBuilder
    {
        private readonly OrderRatingChangeRequestItem _orderRatingChangeRequestItem = new();

        public OrderRatingChangeRequestItemBuilder SetId(int id)
        {
            _orderRatingChangeRequestItem.id = id;
            return this;
        }

        public OrderRatingChangeRequestItemBuilder SetOrderToken(string orderToken)
        {
            _orderRatingChangeRequestItem.order_token = orderToken;
            return this;
        }

        public OrderRatingChangeRequestItemBuilder SetRatingTypeId(int ratingTypeId)
        {
            _orderRatingChangeRequestItem.rating_type_id = ratingTypeId;
            return this;
        }

        public OrderRatingChangeRequestItemBuilder SetNewRating(char newRating)
        {
            _orderRatingChangeRequestItem.new_rating = newRating;
            return this;
        }

        public OrderRatingChangeRequestItemBuilder SetStatus(string status)
        {
            _orderRatingChangeRequestItem.status = status;
            return this;
        }

        public OrderRatingChangeRequestItemBuilder SetRequestedAt(DateTime? requestedAt)
        {
            _orderRatingChangeRequestItem.requested_at = requestedAt;
            return this;
        }

        public OrderRatingChangeRequestItemBuilder SetRequestedBy(string? requestedBy)
        {
            _orderRatingChangeRequestItem.requested_by = requestedBy;
            return this;
        }

        public OrderRatingChangeRequestItemBuilder SetVerifiedAt(DateTime? verifiedAt)
        {
            _orderRatingChangeRequestItem.verified_at = verifiedAt;
            return this;
        }

        public OrderRatingChangeRequestItemBuilder SetVerifiedBy(string? verifiedBy)
        {
            _orderRatingChangeRequestItem.verified_by = verifiedBy;
            return this;
        }

        public OrderRatingChangeRequestItem Build()
        {
            return _orderRatingChangeRequestItem;
        }
    }
}
