// // Copyright (c) 2024 Engibots. All rights reserved.

using engimatrix.ModelObjs;
using engimatrix.Models;
using engimatrix.ResponseMessages;

namespace engimatrix.Views
{
    public class PatchClientRating
    {
        public char rating { get; set; }

        public bool IsValid()
        {
            return Config.RatingConstants.IsValidRating(rating);
        }
    }

    public class UpdateClientRatings
    {
        public List<UpdateClientRatingItem> ratings { get; set; }

        public bool IsValid()
        {
            foreach (UpdateClientRatingItem rating in ratings)
            {
                if (!rating.IsValid())
                {
                    return false;
                }
            }
            return true;
        }
    }
    public class UpdateClientRatingItem
    {
        public int rating_type_id { get; set; }
        public char rating { get; set; }
        public DateTime rating_valid_until { get; set; }

        public bool IsValid()
        {
            if (!Config.RatingConstants.IsValidRating(rating))
            {
                return false;
            }

            if (!Config.RatingTypes.IsValidClientRatingType(rating_type_id))
            {
                return false;
            }

            if (rating_valid_until < DateTime.Now)
            {
                return false;
            }

            return true;
        }
    }
}
