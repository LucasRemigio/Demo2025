// // Copyright (c) 2024 Engibots. All rights reserved.

namespace engimatrix.ModelObjs
{
    public class RatingDiscountItem
    {
        public char rating { get; set; } 
        public decimal percentage { get; set; } 

        public RatingDiscountItem(char rating, decimal percentage)
        {
            this.rating = rating;
            this.percentage = percentage;
        }

        public RatingDiscountItem()
        { }

        public RatingDiscountItem ToItem()
        {
            return new RatingDiscountItem(this.rating, this.percentage);
        }

        public override string ToString()
        {
            return $"RatingDiscountItem:\n" +
                $"rating: {rating}\n" +
                $"percentage: {percentage}\n";
        }

        public bool IsEmpty()
        {
            return rating == ' ' &&
                   percentage == 0;
        }
    }
}
