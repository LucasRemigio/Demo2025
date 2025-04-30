// // Copyright (c) 2024 Engibots. All rights reserved.

namespace engimatrix.ModelObjs
{
    public class RatingCriteriaItem
    {
        public int id { get; set; }
        public int rating_type_id { get; set; }
        public char rating { get; set; }
        public string criteria { get; set; }

        public RatingCriteriaItem(int id, int ratingTypeId, char rating, string criteria)
        {
            this.id = id;
            this.rating_type_id = ratingTypeId;
            this.rating = rating;
            this.criteria = criteria;
        }

        public RatingCriteriaItem(int ratingTypeId, char rating, string criteria)
        {
            this.rating_type_id = ratingTypeId;
            this.rating = rating;
            this.criteria = criteria;
        }

        public RatingCriteriaItem()
        { }

        public RatingCriteriaItem ToItem()
        {
            return new RatingCriteriaItem(this.rating_type_id, this.rating, this.criteria);
        }

        public override string ToString()
        {
            return $"RatingCriteriaItem:\n" +
                $"rating_type_id: {rating_type_id}" +
                $"rating: {rating}\n" +
                $"criteria: {criteria}\n";
        }

        public bool IsEmpty()
        {
            return rating_type_id == 0 &&
                   rating == ' ' &&
                   string.IsNullOrEmpty(criteria);
        }
    }
}
