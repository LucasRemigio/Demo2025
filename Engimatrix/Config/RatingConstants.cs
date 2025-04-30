// // Copyright (c) 2024 Engibots. All rights reserved.

using System.ComponentModel;
using System.Globalization;
using System.Reflection;
using System.Runtime.Serialization;
using Engimatrix.ModelObjs;
using static engimatrix.Config.RatingConstants;

namespace engimatrix.Config
{
    // tables names:
    // mf_rating_credit, mf_rating_payment_compliance, mf_rating_historical_volume, mf_rating_potential_volume, mf_rating_operational_cost, mf_rating_logistic

    public static class RatingConstants
    {
        // Define valid ratings as an enum or constant list
        public enum Rating
        {
            A,
            B,
            C,
            D
        }

        // Method to validate rating
        public static bool IsValidRating(char rating)
        {
            string ratingStr = rating.ToString().ToUpper(CultureInfo.InvariantCulture);

            // Try to parse the string into the Rating enum.
            // The 'true' parameter makes the parsing case-insensitive.
            return Enum.TryParse<Rating>(ratingStr, true, out _);
        }
    }

    public static class RatingTypes
    {
        public enum RatingType
        {
            // Client ratings
            Credit = 1,
            PaymentCompliance = 2,
            HistoricalVolume = 3,
            PotentialVolume = 4,

            // Order ratings
            OperationalCost = 5,
            Logistic = 6
        }

        public static bool IsValidRatingType(int ratingId)
        {
            return Enum.IsDefined(typeof(RatingType), ratingId);
        }

        public static bool IsValidClientRatingType(int ratingId)
        {
            if (!IsValidRatingType(ratingId))
            {
                return false;
            }

            List<int> validClientRatingTypes = [
                (int)RatingType.Credit,
                (int)RatingType.PaymentCompliance,
                (int)RatingType.HistoricalVolume,
                (int)RatingType.PotentialVolume
            ];

            return validClientRatingTypes.Contains(ratingId);
        }

        public static bool IsValidOrderRatingType(int ratingId)
        {
            // Validate first that the ratingId is defined
            if (!IsValidRatingType(ratingId))
            {
                return false;
            }

            // Check if it is one of the order ratings.
            List<int> validOrderRatingTypes = [
                (int)RatingType.OperationalCost,
                (int)RatingType.Logistic
            ];

            return validOrderRatingTypes.Contains(ratingId);
        }
    }
}
