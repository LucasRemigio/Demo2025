// // Copyright (c) 2024 Engibots. All rights reserved.

namespace engimatrix.ModelObjs
{
    public class ClientRatingItem : IRatingItem
    {
        public string client_code { get; set; }
        public int rating_type_id { get; set; }
        public char rating { get; set; }
        public DateTime? updated_at { get; set; }
        public string? updated_by { get; set; }
        public DateTime? created_at { get; set; }
        public string? created_by { get; set; }
    }

    public class ClientRatingItemBuilder
    {
        private readonly ClientRatingItem clientRatingItem = new();

        public ClientRatingItemBuilder SetClientCode(string clientCode)
        {
            clientRatingItem.client_code = clientCode;
            return this;
        }

        public ClientRatingItemBuilder SetRatingTypeId(int ratingTypeId)
        {
            clientRatingItem.rating_type_id = ratingTypeId;
            return this;
        }

        public ClientRatingItemBuilder SetRating(char rating)
        {
            clientRatingItem.rating = rating;
            return this;
        }

        public ClientRatingItemBuilder SetUpdatedAt(DateTime? updatedAt)
        {
            clientRatingItem.updated_at = updatedAt;
            return this;
        }

        public ClientRatingItemBuilder SetUpdatedBy(string? updatedBy)
        {
            clientRatingItem.updated_by = updatedBy;
            return this;
        }

        public ClientRatingItemBuilder SetCreatedAt(DateTime? createdAt)
        {
            clientRatingItem.created_at = createdAt;
            return this;
        }

        public ClientRatingItemBuilder SetCreatedBy(string? createdBy)
        {
            clientRatingItem.created_by = createdBy;
            return this;
        }
        public ClientRatingItem Build()
        {
            return clientRatingItem;
        }
    }
}
