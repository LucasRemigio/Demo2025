// // Copyright (c) 2024 Engibots. All rights reserved.

namespace engimatrix.ModelObjs
{
    public class OrderRatingDTO
    {
        public string order_token { get; set; }
        public RatingTypeItem rating_type { get; set; }
        public RatingDiscountItem rating_discount { get; set; }
        public DateTime? updated_at { get; set; }
        public string? updated_by { get; set; }
        public DateTime created_at { get; set; }
        public string created_by { get; set; }
    }

    public class OrderRatingDtoBuilder
    {
        private readonly OrderRatingDTO _orderRatingDTO = new();

        public OrderRatingDtoBuilder SetOrderToken(string orderToken)
        {
            _orderRatingDTO.order_token = orderToken;
            return this;
        }

        public OrderRatingDtoBuilder SetRatingType(RatingTypeItem ratingType)
        {
            _orderRatingDTO.rating_type = ratingType;
            return this;
        }

        public OrderRatingDtoBuilder SetRatingDiscount(RatingDiscountItem ratingDiscount)
        {
            _orderRatingDTO.rating_discount = ratingDiscount;
            return this;
        }

        public OrderRatingDtoBuilder SetUpdatedAt(DateTime? updatedAt)
        {
            _orderRatingDTO.updated_at = updatedAt;
            return this;
        }

        public OrderRatingDtoBuilder SetUpdatedBy(string? updatedBy)
        {
            _orderRatingDTO.updated_by = updatedBy;
            return this;
        }

        public OrderRatingDtoBuilder SetCreatedAt(DateTime createdAt)
        {
            _orderRatingDTO.created_at = createdAt;
            return this;
        }

        public OrderRatingDtoBuilder SetCreatedBy(string createdBy)
        {
            _orderRatingDTO.created_by = createdBy;
            return this;
        }

        public OrderRatingDTO Build()
        {
            return _orderRatingDTO;
        }
    }
}
