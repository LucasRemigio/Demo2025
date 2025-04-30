// // Copyright (c) 2024 Engibots. All rights reserved.

using System.Diagnostics;
using System.Globalization;
using engimatrix.Config;
using engimatrix.Exceptions;
using engimatrix.ModelObjs;
using engimatrix.Utils;
using Microsoft.Graph.Models;
using Microsoft.IdentityModel.Tokens;
using Newtonsoft.Json.Linq;
using Smartsheet.Api.Models;
using static engimatrix.Config.RatingConstants;

namespace engimatrix.Models;

public static class RatingDiscountModel
{
    public static List<RatingDiscountItem> GetRatingDiscounts(string execute_user)
    {
        string query = $"SELECT rating, percentage FROM mf_rating_discount";

        SqlExecuterItem response = SqlExecuter.ExecFunction(query, [], execute_user, false, "GetRatingDiscounts");

        List<RatingDiscountItem> ratingDiscounts = [];
        if (!response.operationResult)
        {
            throw new Exception("Error getting ratings from rating discounts");
        }

        if (response.out_data.Count <= 0)
        {
            throw new Exception("Table rating discount is empty");
        }

        foreach (Dictionary<string, string> item in response.out_data)
        {
            RatingDiscountItem ratingDiscount = new(item["0"][0], Decimal.Parse(item["1"]));
            ratingDiscounts.Add(ratingDiscount);
        }

        return ratingDiscounts;
    }

    public static RatingDiscountItem GetRatingDiscountByRating(char rating, string execute_user)
    {
        Dictionary<string, string> dic = new()
        {
            { "ratingChar", rating.ToString().ToUpper(CultureInfo.CurrentCulture) }
        };

        string query = $"SELECT rating, percentage " +
            $"FROM mf_rating_discount " +
            $"WHERE rating = @ratingChar";

        SqlExecuterItem response = SqlExecuter.ExecFunction(query, dic, execute_user, false, "GetRatingDiscountsByRating");

        if (!response.operationResult)
        {
            throw new Exception("Error getting ratings from rating discounts");
        }

        if (response.out_data.Count <= 0)
        {
            throw new RatingNotValidException("Error getting record for rating " + rating);
        }

        Dictionary<string, string> item = response.out_data[0];

        RatingDiscountItem ratingDiscount = new(item["0"][0], Decimal.Parse(item["1"]));

        return ratingDiscount;
    }

    public static void CreateRatingDiscount(RatingDiscountItem rating, string execute_user)
    {
        Dictionary<string, string> dic = new()
        {
            // first check if the rating already exists in the database
            { "ratingChar", rating.rating.ToString() }
        };
        string query = $"SELECT rating FROM mf_rating_discount WHERE rating = @ratingChar";

        SqlExecuterItem response = SqlExecuter.ExecFunction(query, dic, execute_user, false, "getRatingsDiscount");

        if (!response.operationResult)
        {
            throw new Exception("Error getting ratings from mf_rating_discount");
        }

        if (response.out_data.Count > 0)
        {
            throw new ItemAlreadyExistsException("Rating already exists in mf_rating_discount");
        }

        dic.Add("percentage", rating.percentage.ToString());

        query = $"INSERT INTO mf_rating_discount (rating,percentage) VALUES (@ratingChar, @percentage);";

        response = SqlExecuter.ExecFunction(query, dic, execute_user, true, "CreateRatingDiscount");

        if (!response.operationResult)
        {
            throw new Exception($"Error inserting values {rating.rating} {rating.percentage} into mf_rating_discount");
        }
    }

    public static void UpdateRatingDiscount(RatingDiscountItem rating, string execute_user)
    {
        Dictionary<string, string> dic = new()
        {
            // first check if the rating does not exist in the database
            { "ratingChar", rating.rating.ToString() }
        };
        string query = $"SELECT rating FROM mf_rating_discount WHERE rating = @ratingChar";

        SqlExecuterItem response = SqlExecuter.ExecFunction(query, dic, execute_user, false, "getRatingsByTableName");

        if (!response.operationResult)
        {
            throw new Exception("Error getting ratings from mf_rating_discount");
        }

        if (response.out_data.Count <= 0)
        {
            throw new ItemAlreadyExistsException($"Rating {rating} does not exist on the mf_rating_discount");
        }

        dic.Add("percentage", rating.percentage.ToString());

        query = $"UPDATE mf_rating_discount SET percentage = @percentage WHERE rating = @ratingChar";

        response = SqlExecuter.ExecFunction(query, dic, execute_user, true, "CreateRatingDiscountItem");

        if (!response.operationResult)
        {
            throw new Exception($"Error updating values {rating} from mf_rating_discount");
        }
    }

    public static void DeleteRatingDiscount(char rating, string execute_user)
    {
        Dictionary<string, string> dic = new()
        {
            // first check if the rating does not exist in the database
            { "ratingChar", rating.ToString() }
        };
        string query = $"SELECT rating FROM mf_rating_discount WHERE rating = @ratingChar";

        SqlExecuterItem response = SqlExecuter.ExecFunction(query, dic, execute_user, false, "getRatingsByTableName");

        if (!response.operationResult)
        {
            throw new Exception("Error getting ratings from mf_rating_discount");
        }

        if (response.out_data.Count <= 0)
        {
            throw new ItemAlreadyExistsException($"Rating {rating} does not exist on the mf_rating_discount");
        }

        query = $"DELETE FROM mf_rating_discount WHERE rating = @ratingChar";

        response = SqlExecuter.ExecFunction(query, dic, execute_user, false, "DeleteRatingDiscount");

        if (!response.operationResult)
        {
            throw new Exception($"Error deleting values {rating} from mf_rating_discount");
        }
    }
}
