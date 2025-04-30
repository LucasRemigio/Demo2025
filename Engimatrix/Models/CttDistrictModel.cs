// // Copyright (c) 2024 Engibots. All rights reserved.


using engimatrix.Exceptions;
using engimatrix.ModelObjs;
using engimatrix.ModelObjs.CTT;

namespace engimatrix.Models;

public static class CttDistrictModel
{
    public static List<CttDistrictItem> GetAll(string executeUser)
    {
        string query = "SELECT dd, name FROM ctt_district";

        List<CttDistrictItem> districts = SqlExecuter.ExecuteFunction<CttDistrictItem>(query, [], executeUser, false, "GetAllCttDistrict");

        if (districts.Count == 0)
        {
            throw new ResourceEmptyException("No ctt districts found in the database");
        }

        return districts;
    }

    public static CttDistrictItem? GetByDd(string dd, string executeUser)
    {
        Dictionary<string, string> dic = [];
        dic.Add("@Dd", dd);

        string query = "SELECT dd, name FROM ctt_district " +
            "WHERE dd = @Dd";

        SqlExecuterItem result = SqlExecuter.ExecuteFunction(query, dic, executeUser, false, "GetCttDistrictById");

        if (!result.operationResult)
        {
            throw new DatabaseException("Error fetching the ctt district from the database");
        }

        if (result.out_data.Count == 0)
        {
            return null;
        }

        Dictionary<string, string> item = result.out_data[0];
        CttDistrictItem district = new(item["dd"], item["name"]);
        return district;
    }
}
