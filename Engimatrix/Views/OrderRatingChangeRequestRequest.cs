// // Copyright (c) 2024 Engibots. All rights reserved.

using engimatrix.Config;
using engimatrix.Utils;

namespace engimatrix.Views;
public class CreateOrderRatingChangeRequestRequest
{
    public int rating_type_id { get; set; }
    public char new_rating { get; set; }

    public bool IsValid()
    {
        if (!RatingTypes.IsValidRatingType(rating_type_id))
        {
            return false;
        }

        if (!RatingConstants.IsValidRating(new_rating))
        {
            return false;
        }

        return true;
    }
}


public class UpdateOrderRatingChangeRequestRequest
{
    public int rating_type_id { get; set; }
    public bool is_accepted { get; set; }

    public bool IsValid()
    {
        if (!RatingTypes.IsValidRatingType(rating_type_id))
        {
            return false;
        }

        return true;
    }
}

