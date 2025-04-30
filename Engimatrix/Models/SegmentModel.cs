// // Copyright (c) 2024 Engibots. All rights reserved.

using System.Diagnostics;
using engimatrix.Exceptions;
using engimatrix.ModelObjs;

namespace engimatrix.Models;

public static class SegmentModel
{

    public static List<SegmentItem> GetSegments(string execute_user)
    {

        string query = "SELECT id, name FROM mf_segment WHERE id > 0";

        // Execute the query
        SqlExecuterItem response = SqlExecuter.ExecuteFunction(query, [], execute_user, false, "GetSegments");

        if (!response.operationResult)
        {
            throw new Exception("An error occurred fetching data from the segments table.");
        }

        List<SegmentItem> segments = [];

        if (response.out_data.Count <= 0)
        {
            return segments;
        }

        // Map results to ProductDiscountDTO list
        foreach (Dictionary<string, string> item in response.out_data)
        {
            SegmentItem segment = new(Int32.Parse(item["id"]), item["name"]);
            segments.Add(segment);
        }

        return segments;
    }

    public static SegmentItem GetSegmentById(int segmentId, string execute_user)
    {
        Dictionary<string, string> dic = new()
        {
            { "segmentId", segmentId.ToString() }
        };

        string query = "SELECT id, name " +
            "FROM mf_segment " +
            "WHERE id = @segmentId ";

        // Execute the query
        SqlExecuterItem response = SqlExecuter.ExecFunction(query, dic, execute_user, false, "GetSegmentById");

        if (!response.operationResult)
        {
            throw new Exception("An error occurred fetching data from the segments table.");
        }

        if (response.out_data.Count <= 0)
        {
            throw new InputNotValidException($"No segment with id {segmentId} was found");
        }

        Dictionary<string, string> item = response.out_data[0];

        SegmentItem segment = new(Int32.Parse(item["0"]), item["1"]);

        return segment;
    }

    public static void CreateSegment(SegmentItem segment, string execute_user)
    {
        // verify it there already is a segment with the given name
        Dictionary<string, string> dic = new()
        {
            { "name", segment.name }
        };

        string query = "SELECT id " +
            "FROM mf_segment " +
            "WHERE name = @name ";

        // Execute the query
        SqlExecuterItem response = SqlExecuter.ExecFunction(query, dic, execute_user, false, "CreateSegment");

        if (!response.operationResult)
        {
            throw new Exception("An error occurred fetching data from the segments table.");
        }

        if (response.out_data.Count > 0)
        {
            throw new InputNotValidException($"A segment with name {segment.name} already exists");
        }

        // Create the segment
        dic = new()
        {
            { "name", segment.name }
        };

        query = "INSERT INTO mf_segment (name) " +
            "VALUES (@name) ";

        // Execute the query
        response = SqlExecuter.ExecFunction(query, dic, execute_user, true, "CreateSegment");
        if (!response.operationResult)
        {
            throw new Exception("An error occurred creating the segment.");
        }
    }

    public static void UpdateSegment(SegmentItem segment, string execute_user)
    {
        // verify if the name is different than the current one
        Dictionary<string, string> dic = new()
        {
            { "segmentId", segment.id.ToString() },
            { "name", segment.name }
        };

        string query = "SELECT id, name " +
            "FROM mf_segment " +
            "WHERE name = @name ";

        // Execute the query
        SqlExecuterItem response = SqlExecuter.ExecFunction(query, dic, execute_user, false, "UpdateSegment");

        if (!response.operationResult)
        {
            throw new Exception("An error occurred fetching data from the segments table.");
        }

        if (response.out_data.Count > 0)
        {
            throw new InputNotValidException($"A segment with name {segment.name} already exists");
        }

        // check if current name is different than the new one
        Dictionary<string, string> item = response.out_data.Find((item) => item["1"].Equals(segment.name, StringComparison.OrdinalIgnoreCase));

        if (item != null)
        {
            throw new InputNotValidException($"The segment name is the same as the current one");
        }

        // Update the segment
        dic = new()
        {
            { "segmentId", segment.id.ToString() },
            { "name", segment.name }
        };

        query = "UPDATE mf_segment " +
            "SET name = @name " +
            "WHERE id = @segmentId ";

        // Execute the query
        response = SqlExecuter.ExecFunction(query, dic, execute_user, true, "UpdateSegment");
        if (!response.operationResult)
        {
            throw new Exception("An error occurred updating the segment.");
        }
    }

    public static void DeleteSegment(int segmentId, string execute_user)
    {
        Dictionary<string, string> dic = new()
        {
            { "segmentId", segmentId.ToString() }
        };

        string query = "DELETE FROM mf_segment " +
            "WHERE id = @segmentId ";

        // Execute the query
        SqlExecuterItem response = SqlExecuter.ExecFunction(query, dic, execute_user, true, "DeleteSegment");

        if (!response.operationResult)
        {
            throw new Exception("An error occurred deleting the segment.");
        }
    }
}
