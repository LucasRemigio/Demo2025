// // Copyright (c) 2024 Engibots. All rights reserved.

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

public static class RatingTypeModel
{

    public static List<RatingTypeItem> GetRatingTypes(string execute_user)
    {

        string query = $"SELECT id, description, slug, weight FROM mf_rating_type";

        SqlExecuterItem response = SqlExecuter.ExecuteFunction(query, [], execute_user, false, "getRatingTypes");

        List<RatingTypeItem> ratingTypes = [];

        if (!response.operationResult)
        {
            throw new Exception("Error getting ratings from mf_rating_type");
        }

        if (response.out_data.Count <= 0)
        {
            throw new InputNotValidException("Error getting record for rating types");
        }

        foreach (Dictionary<string, string> item in response.out_data)
        {
            RatingTypeItem ratingType = new(Int32.Parse(item["id"]), item["description"], item["slug"], Decimal.Parse(item["weight"])); ratingTypes.Add(ratingType);
        }

        return ratingTypes;
    }

    public static List<RatingTypeItem> GetClientRatingTypes(string executeUser)
    {
        List<int> ratingTypeIds = [(int)RatingTypes.RatingType.Credit, (int)RatingTypes.RatingType.HistoricalVolume, (int)RatingTypes.RatingType.PotentialVolume, (int)RatingTypes.RatingType.PaymentCompliance];
        string ratingTypeIdList = string.Join(",", ratingTypeIds);

        // the client ratings are the first 4, the credit, historical volume, potential volume and payment compliance
        string query = $"SELECT id, description, slug, weight FROM mf_rating_type " +
            $"WHERE id IN ({ratingTypeIdList})";

        SqlExecuterItem response = SqlExecuter.ExecuteFunction(query, [], executeUser, false, "getClientRatingTypes");

        List<RatingTypeItem> ratingTypes = [];
        foreach (Dictionary<string, string> ratingType in response.out_data)
        {
            RatingTypeItem ratingTypeItem = new(int.Parse(ratingType["id"]), ratingType["description"], ratingType["slug"], decimal.Parse(ratingType["weight"]));
            ratingTypes.Add(ratingTypeItem);
        }

        return ratingTypes;
    }

    public static List<RatingTypeItem> GetOrderRatingTypes(string executeUser)
    {
        List<int> ratingTypeIds = [(int)RatingTypes.RatingType.OperationalCost, (int)RatingTypes.RatingType.Logistic];
        string ratingTypeIdList = string.Join(",", ratingTypeIds);

        // the order ratings are the last 4, the order volume, order value, order frequency and order compliance
        string query = $"SELECT id, description, slug, weight FROM mf_rating_type " +
            $"WHERE id IN ({ratingTypeIdList})";

        SqlExecuterItem response = SqlExecuter.ExecuteFunction(query, [], executeUser, false, "getOrderRatingTypes");

        List<RatingTypeItem> ratingTypes = [];
        foreach (Dictionary<string, string> ratingType in response.out_data)
        {
            RatingTypeItem ratingTypeItem = new(int.Parse(ratingType["id"]), ratingType["description"], ratingType["slug"], decimal.Parse(ratingType["weight"]));
            ratingTypes.Add(ratingTypeItem);
        }

        return ratingTypes;
    }

    public static RatingTypeItem GetRatingType(int id, string execute_user)
    {
        Dictionary<string, string> dic = new()
        {
            { "id", id.ToString() }
        };

        string query = $"SELECT id, description, slug, weight " +
            $"FROM mf_rating_type " +
            $"WHERE id = @id";

        SqlExecuterItem response = SqlExecuter.ExecuteFunction(query, dic, execute_user, false, "getRatingType");

        if (!response.operationResult)
        {
            throw new Exception("Error getting ratings from mf_rating_type");
        }

        if (response.out_data.Count <= 0)
        {
            throw new InputNotValidException("Error getting record for rating type id " + id.ToString());
        }

        Dictionary<string, string> item = response.out_data[0];

        RatingTypeItem ratingType = new(Int32.Parse(item["id"]), item["description"], item["slug"], Decimal.Parse(item["weight"]));

        return ratingType;
    }

    public static void CreateRatingType(RatingTypeItem rating, string execute_user)
    {
        Dictionary<string, string> dic = new()
        {
            { "description", rating.description },
            { "slug", rating.slug },
            { "weight", rating.weight.ToString() }
        };

        string query = $"INSERT INTO mf_rating_type (description, slug, weight) VALUES (@description, @slug, @weight);";

        SqlExecuterItem response = SqlExecuter.ExecFunction(query, dic, execute_user, true, "createRatingType");

        if (!response.operationResult)
        {
            throw new Exception($"Error inserting values {rating.description} into mf_rating_type");
        }
    }

    public static void UpdateRatingType(RatingTypeItem rating, string execute_user)
    {
        Dictionary<string, string> dic = new()
        {
            { "slug", rating.slug },
            { "description", rating.description },
            { "id", rating.id.ToString() },
            { "weight", rating.weight.ToString() }
        };

        string query = $"UPDATE mf_rating_type SET description = @description, slug = @slug, weight = @weight WHERE id = @id";

        SqlExecuterItem response = SqlExecuter.ExecFunction(query, dic, execute_user, true, "updateRatingType");

        if (!response.operationResult)
        {
            throw new Exception($"Error updating values {rating.description} from mf_rating_type");
        }
    }

    public static void DeleteRatingType(int id, string execute_user)
    {
        Dictionary<string, string> dic = new()
        {
            { "id", id.ToString() }
        };

        string query = $"DELETE FROM mf_rating_type WHERE id = @id";

        SqlExecuterItem response = SqlExecuter.ExecFunction(query, dic, execute_user, true, "deleteRatingType");

        if (!response.operationResult)
        {
            throw new Exception($"Error deleting values {id} from mf_rating_type");
        }
    }
}
