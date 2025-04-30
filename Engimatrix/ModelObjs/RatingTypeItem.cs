// // Copyright (c) 2024 Engibots. All rights reserved.

namespace engimatrix.ModelObjs
{
    public class RatingTypeItem
    {
        public int id { get; set; }
        public string description { get; set; }
        public string slug { get; set; }
        public decimal weight { get; set; }

        public RatingTypeItem(int id, string description, string slug, decimal weight)
        {
            this.id = id;
            this.description = description;
            this.slug = slug;
            this.weight = weight;
        }

        public RatingTypeItem(string description, string slug, decimal weight)
        {
            this.description = description;
            this.slug = slug;
            this.weight = weight;
        }

        public RatingTypeItem()
        { }

        public RatingTypeItem ToItem()
        {
            return new RatingTypeItem(this.id, this.description, this.slug, this.weight);
        }

        public override string ToString()
        {
            return $"RatingCriteriaItem:\n"
                + $"id: {id}\n"
                    + $"description: {description}\n";
        }

        public bool IsEmpty()
        {
            return id == 0 &&
                   string.IsNullOrEmpty(description);
        }
    }
}
