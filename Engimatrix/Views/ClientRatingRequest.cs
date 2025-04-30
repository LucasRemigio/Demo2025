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
}
