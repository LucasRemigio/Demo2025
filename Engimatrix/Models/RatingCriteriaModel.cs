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

public static class RatingCriteriaModel
{
    public static List<RatingCriteriaItem> GetRatings(string execute_user)
    {
        Dictionary<string, string> dic = [];

        string query = $"SELECT id, rating_type_id, rating, criteria FROM mf_rating_criteria";

        SqlExecuterItem response = SqlExecuter.ExecFunction(query, dic, execute_user, false, "GetRatings");

        List<RatingCriteriaItem> ratingCriteria = [];

        if (!response.operationResult)
        {
            throw new Exception("Error getting ratings from rating criteria");
        }

        if (response.out_data.Count <= 0)
        {
            throw new Exception("Table rating criteria is empty");
        }

        foreach (Dictionary<string, string> item in response.out_data)
        {
            RatingCriteriaItem ratingCriterion = new(Int32.Parse(item["0"]), Int32.Parse(item["1"]), item["2"][0], item["3"]);
            ratingCriteria.Add(ratingCriterion);
        }

        return ratingCriteria;
    }

    public static RatingCriteriaItem GetRatingCriteriaById(int id, string execute_user)
    {
        Dictionary<string, string> dic = new()
        {
            { "id", id.ToString() }
        };

        string query = $"SELECT id, rating_type_id, rating, criteria " +
            $"FROM mf_rating_criteria " +
            $"WHERE id = @id";

        SqlExecuterItem response = SqlExecuter.ExecFunction(query, dic, execute_user, false, "GetRatingCriteriaById");

        if (!response.operationResult)
        {
            throw new Exception("Error getting ratings from rating criteria");
        }

        if (response.out_data.Count <= 0)
        {
            throw new Exception("Table rating criteria is empty");
        }

        Dictionary<string, string> item = response.out_data[0];

        RatingCriteriaItem ratingCriterion = new(Int32.Parse(item["0"]), Int32.Parse(item["1"]), item["2"][0], item["3"]);

        return ratingCriterion;
    }

    public static List<RatingCriteriaItem> GetRatingsByRatingType(int ratingTypeId, string execute_user)
    {
        Dictionary<string, string> dic = new()
        {
            { "ratingTypeId", ratingTypeId.ToString() }
        };

        string query = $"SELECT id, rating, criteria " +
            $"FROM mf_rating_criteria " +
            $"WHERE rating_type_id = @ratingTypeId";

        SqlExecuterItem response = SqlExecuter.ExecFunction(query, dic, execute_user, false, "GetRatingsByRatingType");

        List<RatingCriteriaItem> ratingCriteria = [];

        if (!response.operationResult)
        {
            throw new Exception("Error getting ratings from rating criteria");
        }

        if (response.out_data.Count <= 0)
        {
            throw new Exception("Table rating criteria is empty");
        }

        foreach (Dictionary<string, string> item in response.out_data)
        {
            RatingCriteriaItem ratingCriterion = new(Int32.Parse(item["0"]), ratingTypeId, item["1"][0], item["2"]);
            ratingCriteria.Add(ratingCriterion);
        }

        return ratingCriteria;
    }

    public static List<RatingCriteriaItem> GetRatingsByRating(char rating, string execute_user)
    {
        Dictionary<string, string> dic = new()
        {
            { "ratingChar", rating.ToString().ToUpper(CultureInfo.CurrentCulture) }
        };

        string query = $"SELECT id, rating_type_id, rating, criteria " +
            $"FROM mf_rating_criteria " +
            $"WHERE rating = @ratingChar";

        SqlExecuterItem response = SqlExecuter.ExecFunction(query, dic, execute_user, false, "GetRatingsByRating");

        List<RatingCriteriaItem> ratingCriteria = [];

        if (!response.operationResult)
        {
            throw new Exception("Error getting ratings from rating criteria");
        }

        if (response.out_data.Count <= 0)
        {
            throw new Exception("Table rating criteria is empty");
        }

        foreach (Dictionary<string, string> item in response.out_data)
        {
            RatingCriteriaItem ratingCriterion = new(Int32.Parse(item["0"]), Int32.Parse(item["1"]), item["2"][0], item["3"]);
            ratingCriteria.Add(ratingCriterion);
        }

        return ratingCriteria;
    }

    public static RatingCriteriaItem GetRating(int ratingTypeId, char rating, string execute_user)
    {
        Dictionary<string, string> dic = new()
        {
            { "ratingTypeId", ratingTypeId.ToString() },
            { "ratingChar", rating.ToString().ToUpper(CultureInfo.CurrentCulture) }
        };

        string query = $"SELECT id, rating_type_id, rating, criteria " +
            $"FROM mf_rating_criteria " +
            $"WHERE rating_type_id = @ratingTypeId AND rating = @ratingChar";

        SqlExecuterItem response = SqlExecuter.ExecFunction(query, dic, execute_user, false, "GetRating");

        if (!response.operationResult)
        {
            throw new Exception("Error getting ratings from rating criteria");
        }

        if (response.out_data.Count <= 0)
        {
            throw new Exception("Table rating criteria is empty");
        }

        Dictionary<string, string> item = response.out_data[0];

        RatingCriteriaItem ratingCriterion = new(Int32.Parse(item["0"]), Int32.Parse(item["1"]), item["2"][0], item["3"]);

        return ratingCriterion;
    }

    public static void CreateRatingCriteria(RatingCriteriaItem rating, string execute_user)
    {
        Dictionary<string, string> dic = [];

        // get last id from databaes and add 1 to get the new id
        string query = $"SELECT MAX(id) FROM mf_rating_criteria";

        SqlExecuterItem response = SqlExecuter.ExecFunction(query, dic, execute_user, false, "CreateRatingCriteria");

        if (!response.operationResult)
        {
            throw new Exception("Error creating rating criteria");
        }

        int id = Int32.Parse(response.out_data[0]["0"]) + 1;

        rating.id = id;
        dic.Add("ratingId", rating.id.ToString());
        dic.Add("ratingTypeId", rating.rating_type_id.ToString());
        dic.Add("ratingChar", rating.rating.ToString().ToUpper(CultureInfo.CurrentCulture));
        dic.Add("criteria", rating.criteria);

        query = $"INSERT INTO mf_rating_criteria (id,rating_type_id, rating, criteria) " +
            $"VALUES (@ratingId, @ratingTypeId, @ratingChar, @criteria)";

        response = SqlExecuter.ExecFunction(query, dic, execute_user, true, "CreateRatingCriteria");

        if (!response.operationResult)
        {
            throw new Exception("Error creating rating criteria");
        }
    }

    public static void UpdateRatingCriteria(RatingCriteriaItem rating, string execute_user)
    {
        Dictionary<string, string> dic = new()
        {
            { "ratingId", rating.id.ToString() },
            { "ratingTypeId", rating.rating_type_id.ToString() },
            { "ratingChar", rating.rating.ToString().ToUpper(CultureInfo.CurrentCulture) },
            { "criteria", rating.criteria }
        };

        string query = $"UPDATE mf_rating_criteria " +
            $"SET rating_type_id = @ratingTypeId, rating = @ratingChar, criteria = @criteria " +
            $"WHERE id = @ratingId";

        SqlExecuterItem response = SqlExecuter.ExecFunction(query, dic, execute_user, true, "UpdateRatingCriteria");

        if (!response.operationResult)
        {
            throw new Exception("Error updating rating criteria");
        }
    }

    public static void DeleteRatingCriteria(int id, string execute_user)
    {
        Dictionary<string, string> dic = new()
        {
            { "ratingId", id.ToString() }
        };

        string query = $"DELETE FROM mf_rating_criteria " +
            $"WHERE id = @ratingId";

        SqlExecuterItem response = SqlExecuter.ExecFunction(query, dic, execute_user, true, "DeleteRatingCriteria");

        if (!response.operationResult)
        {
            throw new Exception("Error deleting rating criteria");
        }
    }

    public static List<RatingCriteriaDTO> GetRatingsDTO(string execute_user)
    {
        Dictionary<string, string> dic = [];

        string query = $"SELECT c.id, c.rating_type_id, t.description, t.slug, c.rating, c.criteria, t.weight " +
            $"FROM mf_rating_criteria c " +
            $"JOIN mf_rating_type t ON c.rating_type_id = t.id";

        SqlExecuterItem response = SqlExecuter.ExecFunction(query, dic, execute_user, false, "GetRatingsDTO");

        List<RatingCriteriaDTO> ratingCriteria = [];

        if (!response.operationResult)
        {
            throw new Exception("Error getting ratings from rating criteria");
        }

        if (response.out_data.Count <= 0)
        {
            throw new Exception("Table rating criteria is empty");
        }

        foreach (Dictionary<string, string> item in response.out_data)
        {
            RatingTypeItem ratingType = new(Int32.Parse(item["1"]), item["2"], item["3"], Convert.ToDecimal(item["6"]));
            RatingCriteriaDTO ratingCriterion = new(Int32.Parse(item["0"]), ratingType, item["4"][0], item["5"]);
            ratingCriteria.Add(ratingCriterion);
        }

        return ratingCriteria;
    }

    public static RatingCriteriaDTO GetRatingCriteriaByIdDTO(int id, string execute_user)
    {
        Dictionary<string, string> dic = new()
        {
            { "id", id.ToString() }
        };

        string query = $"SELECT c.id AS criteria_id, c.rating_type_id AS criteria_rating_type_id, t.description AS type_description, " +
            "t.slug AS type_slug, t.weight AS type_weight, c.rating AS criteria_rating, c.criteria AS criteria_rating " +
            $"FROM mf_rating_criteria c " +
            $"JOIN mf_rating_type t ON c.rating_type_id = t.id " +
            $"WHERE c.id = @id";

        SqlExecuterItem response = SqlExecuter.ExecuteFunction(query, dic, execute_user, false, "GetRatingCriteriaByIdDTO");

        if (!response.operationResult)
        {
            throw new Exception("Error getting ratings from rating criteria");
        }

        if (response.out_data.Count <= 0)
        {
            throw new Exception("Table rating criteria is empty");
        }

        Dictionary<string, string> item = response.out_data[0];

        RatingTypeItem ratingType = new(Int32.Parse(item["criteria_rating_type_id"]), item["type_description"], item["type_slug"], Convert.ToDecimal(item["type_weight"]));
        RatingCriteriaDTO ratingCriterion = new(Int32.Parse(item["criteria_id"]), ratingType, item["criteria_rating"][0], item["criteria_rating"]);

        return ratingCriterion;
    }
}
