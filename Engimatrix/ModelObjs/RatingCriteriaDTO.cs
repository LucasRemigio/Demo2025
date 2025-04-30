// // Copyright (c) 2024 Engibots. All rights reserved.

using Microsoft.Graph.Models;

namespace engimatrix.ModelObjs
{
    public class RatingCriteriaDTO
    {
        public int id { get; set; }
        public RatingTypeItem rating_type { get; set; }
        public char rating { get; set; }
        public string criteria { get; set; }

        public RatingCriteriaDTO(int id, RatingTypeItem ratingType, char rating, string criteria)
        {
            this.id = id;
            this.rating_type = ratingType;
            this.rating = rating;
            this.criteria = criteria;
        }

        public RatingCriteriaDTO(RatingTypeItem ratingType, char rating, string criteria)
        {
            this.rating_type = ratingType;
            this.rating = rating;
            this.criteria = criteria;
        }

        public RatingCriteriaDTO()
        { }

        public RatingCriteriaDTO ToItem()
        {
            return new RatingCriteriaDTO(this.id, this.rating_type, this.rating, this.criteria);
        }

        public override string ToString()
        {
            return $"RatingCriteriaItem:\n" +
                $"rating_type_id: {rating_type.id}" +
                $"rating: {rating}\n" +
                $"criteria: {criteria}\n";
        }

        public bool IsEmpty()
        {
            return rating_type.id == 0 &&
                   rating == ' ' &&
                   string.IsNullOrEmpty(criteria);
        }
    }
}
