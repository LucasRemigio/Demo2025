// // Copyright (c) 2024 Engibots. All rights reserved.

using System.Globalization;
using engimatrix.ModelObjs;
using engimatrix.Models;

namespace Engimatrix.Models;

public class CancelReasonModel
{
    /*
     *     id INT AUTO_INCREMENT PRIMARY KEY,
    reason VARCHAR(255) NOT NULL,
    slug VARCHAR(255) UNIQUE NOT NULL,
    description TEXT,
    is_active tinyint DEFAULT TRUE,
    created_at TIMESTAMP DEFAULT CURRENT_TIMESTAMP,
    updated_at TIMESTAMP DEFAULT CURRENT_TIMESTAMP ON UPDATE CURRENT_TIMESTAMP
    */

    // get all
    public static List<CancelReasonItem> GetCancelReasons(string execute_user, bool? isActive)
    {
        string query = "SELECT id, reason, description, slug, is_active, created_at, updated_at FROM `cancel_reason` ";

        // Add filter if isActive has a value
        if (isActive.HasValue)
        {
            query += " WHERE is_active = " + (isActive.Value ? "1" : "0");
        }

        SqlExecuterItem response = SqlExecuter.ExecuteFunction(query, [], execute_user, false, "GetCancelReasons");

        if (!response.operationResult)
        {
            throw new Exception("Something went wrong getting the cancel reasons from the database");
        }

        if (response.out_data.Count == 0)
        {
            return [];
        }

        List<CancelReasonItem> cancelReasons = [];
        foreach (Dictionary<string, string> item in response.out_data)
        {
            CancelReasonItem cancelReason = new CancelReasonItemBuilder()
                .SetId(Convert.ToInt32(item["id"]))
                .SetReason(item["reason"])
                .SetSlug(item["slug"])
                .SetDescription(item["description"])
                .SetIsActive(Convert.ToInt32(item["is_active"]) != 0)
                .SetCreatedAt(Convert.ToDateTime(item["created_at"]))
                .SetUpdatedAt(Convert.ToDateTime(item["updated_at"]))
                .Build();

            cancelReasons.Add(cancelReason);
        }

        return cancelReasons;
    }

    // get 1
    public static CancelReasonItem GetCancelReason(int id, string execute_user)
    {
        string query = "SELECT id, reason, description, slug, is_active, created_at, updated_at FROM `cancel_reason` WHERE id = @id";

        Dictionary<string, string> parameters = new()
        {
            { "@id", id.ToString() }
        };

        SqlExecuterItem response = SqlExecuter.ExecuteFunction(query, parameters, execute_user, false, "GetCancelReason");

        if (!response.operationResult)
        {
            throw new Exception("Something went wrong getting the cancel reason from the database");
        }

        if (response.out_data.Count == 0)
        {
            return null;
        }

        Dictionary<string, string> item = response.out_data[0];
        CancelReasonItem cancelReason = new CancelReasonItemBuilder()
            .SetId(Convert.ToInt32(item["id"]))
            .SetReason(item["reason"])
            .SetSlug(item["slug"])
            .SetDescription(item["description"])
            .SetIsActive(Convert.ToInt32(item["is_active"]) != 0)
            .SetCreatedAt(Convert.ToDateTime(item["created_at"]))
            .SetUpdatedAt(Convert.ToDateTime(item["updated_at"]))
            .Build();

        return cancelReason;
    }

    // create
    public static bool CreateCancelReason(string reason, string description, string execute_user)
    {
        string query = "INSERT INTO `cancel_reason` (reason, slug, description) " +
            "VALUES (@reason, @slug, @description)";

        Dictionary<string, string> parameters = new()
        {
            { "@reason", reason },
            { "@slug", GetSlug(reason) },
            { "@description", description }
        };

        SqlExecuterItem response = SqlExecuter.ExecuteFunction(query, parameters, execute_user, true, "CreateCancelReason");

        return response.operationResult;
    }

    // update
    public static bool UpdateCancelReason(int id, string reason, string description, string execute_user)
    {
        string query = "UPDATE `cancel_reason` SET reason = @reason, slug = @slug, description = @description " +
            "WHERE id = @id";

        Dictionary<string, string> parameters = new()
        {
            { "@id", id.ToString() },
            { "@reason", reason },
            { "@slug", GetSlug(reason) },
            { "@description", description }
        };

        SqlExecuterItem response = SqlExecuter.ExecuteFunction(query, parameters, execute_user, true, "UpdateCancelReason");

        return response.operationResult;
    }

    // generate slug from reason
    public static string GetSlug(string reason)
    {
        return reason.ToLower(CultureInfo.InvariantCulture).Replace(" ", "-", StringComparison.OrdinalIgnoreCase);
    }

    // Change active status
    public static bool ChangeActiveStatus(int id, bool status, string execute_user)
    {
        string query = "UPDATE `cancel_reason` SET is_active = @status WHERE id = @id";

        Dictionary<string, string> parameters = new()
        {
            { "@id", id.ToString() },
            { "@status", status.ToString() }
        };

        SqlExecuterItem response = SqlExecuter.ExecuteFunction(query, parameters, execute_user, true, "ChangeActiveStatus");

        return response.operationResult;
    }
}
