// // Copyright (c) 2024 Engibots. All rights reserved.

using engimatrix.ModelObjs;
using engimatrix.Models;
using engimatrix.ResponseMessages;

namespace engimatrix.Views
{
    public class RatingCriteriaCreateRequest
    {
        public int id { get; set; }
        public int rating_type_id { get; set; }
        public char rating { get; set; }
        public string criteria { get; set; }

        public bool IsValid()
        {
            return !string.IsNullOrEmpty(this.criteria) || this.rating != ' ' || this.rating_type_id <= 0 || this.id <= 0;
        }
    }

    public class RatingCriteriaUpdateRequest
    {
        public int rating_type_id { get; set; }
        public char rating { get; set; }
        public string criteria { get; set; }

        public bool IsValid()
        {
            return !string.IsNullOrEmpty(this.criteria) || this.rating != ' ' || this.rating_type_id <= 0;
        }
    }

    public class RatingDiscountCreateRequest
    {
        public char rating { get; set; }
        public decimal percentage { get; set; }

        public bool IsValid()
        {
            return this.rating != ' ';
        }
    }

    public class RatingDiscountUpdateRequest
    {
        public decimal percentage { get; set; }
    }

    public class RatingWeightingCreateRequest
    {
        public string description { get; set; }
        public decimal value { get; set; }

        public bool IsValid()
        {
            return !String.IsNullOrEmpty(this.description);
        }
    }

    public class RatingTypeCreateRequest
    {
        public string description { get; set; }
        public string slug { get; set; }
        public decimal weight { get; set; }

        public bool IsValid()
        {
            return !String.IsNullOrEmpty(this.description) && !String.IsNullOrEmpty(this.slug) && weight > 0 && weight < 1;
        }
    }

    public class RatingTypeUpdateRequest
    {
        public string description { get; set; }
        public string slug { get; set; }

        public bool IsValid()
        {
            return !String.IsNullOrEmpty(this.description) || !String.IsNullOrEmpty(this.slug);
        }
    }
}
