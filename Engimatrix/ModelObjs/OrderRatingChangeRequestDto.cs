// // Copyright (c) 2024 Engibots. All rights reserved.

namespace engimatrix.ModelObjs
{
    public class OrderRatingChangeRequestDto
    {
        public int id { get; set; }
        public string order_token { get; set; }
        public RatingTypeItem rating_type { get; set; }
        public RatingDiscountItem new_rating_discount { get; set; }
        public string status { get; set; }
        public DateTime? requested_at { get; set; }
        public string? requested_by { get; set; }
        public DateTime? verified_at { get; set; }
        public string? verified_by { get; set; }
    }

    public class OrderRatingChangeRequestDtoBuilder
    {
        private readonly OrderRatingChangeRequestDto _orderRatingChangeRequestItem = new();

        public OrderRatingChangeRequestDtoBuilder SetId(int id)
        {
            _orderRatingChangeRequestItem.id = id;
            return this;
        }

        public OrderRatingChangeRequestDtoBuilder SetOrderToken(string orderToken)
        {
            _orderRatingChangeRequestItem.order_token = orderToken;
            return this;
        }

        public OrderRatingChangeRequestDtoBuilder SetRatingType(RatingTypeItem ratingTypeId)
        {
            _orderRatingChangeRequestItem.rating_type = ratingTypeId;
            return this;
        }

        public OrderRatingChangeRequestDtoBuilder SetNewRatingDiscount(RatingDiscountItem newRatingDiscount)
        {
            _orderRatingChangeRequestItem.new_rating_discount = newRatingDiscount;
            return this;
        }

        public OrderRatingChangeRequestDtoBuilder SetStatus(string status)
        {
            _orderRatingChangeRequestItem.status = status;
            return this;
        }

        public OrderRatingChangeRequestDtoBuilder SetRequestedAt(DateTime? requestedAt)
        {
            _orderRatingChangeRequestItem.requested_at = requestedAt;
            return this;
        }

        public OrderRatingChangeRequestDtoBuilder SetRequestedBy(string? requestedBy)
        {
            _orderRatingChangeRequestItem.requested_by = requestedBy;
            return this;
        }

        public OrderRatingChangeRequestDtoBuilder SetVerifiedAt(DateTime? verifiedAt)
        {
            _orderRatingChangeRequestItem.verified_at = verifiedAt;
            return this;
        }

        public OrderRatingChangeRequestDtoBuilder SetVerifiedBy(string? verifiedBy)
        {
            _orderRatingChangeRequestItem.verified_by = verifiedBy;
            return this;
        }

        public OrderRatingChangeRequestDto Build()
        {
            return _orderRatingChangeRequestItem;
        }
    }
}
