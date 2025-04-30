// // Copyright (c) 2024 Engibots. All rights reserved.

namespace engimatrix.ModelObjs
{
    public class OrderRatingItem : IRatingItem
    {
        public string order_token { get; set; }
        public int rating_type_id { get; set; }
        public char rating { get; set; }
        public DateTime? updated_at { get; set; }
        public string? updated_by { get; set; }
        public DateTime? created_at { get; set; }
        public string? created_by { get; set; }
    }

    public class OrderRatingItemBuilder
    {
        private readonly OrderRatingItem _orderRatingItem = new();

        public OrderRatingItemBuilder SetOrderToken(string orderToken)
        {
            _orderRatingItem.order_token = orderToken;
            return this;
        }

        public OrderRatingItemBuilder SetRatingTypeId(int ratingTypeId)
        {
            _orderRatingItem.rating_type_id = ratingTypeId;
            return this;
        }

        public OrderRatingItemBuilder SetRating(char rating)
        {
            _orderRatingItem.rating = rating;
            return this;
        }

        public OrderRatingItemBuilder SetUpdatedAt(DateTime? updatedAt)
        {
            _orderRatingItem.updated_at = updatedAt;
            return this;
        }

        public OrderRatingItemBuilder SetUpdatedBy(string? updatedBy)
        {
            _orderRatingItem.updated_by = updatedBy;
            return this;
        }

        public OrderRatingItemBuilder SetCreatedAt(DateTime? createdAt)
        {
            _orderRatingItem.created_at = createdAt;
            return this;
        }

        public OrderRatingItemBuilder SetCreatedBy(string? createdBy)
        {
            _orderRatingItem.created_by = createdBy;
            return this;
        }
        public OrderRatingItem Build()
        {
            return _orderRatingItem;
        }
    }
}
