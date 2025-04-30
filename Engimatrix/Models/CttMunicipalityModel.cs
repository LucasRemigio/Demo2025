// // Copyright (c) 2024 Engibots. All rights reserved.


using engimatrix.Exceptions;
using engimatrix.ModelObjs;
using engimatrix.ModelObjs.CTT;

namespace engimatrix.Models;

public static class CttMunicipalityModel
{
    public static List<CttMunicipalityItem> GetAll(string executeUser)
    {
        string query = "SELECT cc, dd, name FROM ctt_municipality";

        SqlExecuterItem result = SqlExecuter.ExecuteFunction(query, [], executeUser, false, "GetAllCttMunicipality");

        if (!result.operationResult)
        {
            throw new DatabaseException("Error fetching the ctt municipalities from the database");
        }

        if (result.out_data.Count == 0)
        {
            throw new ResourceEmptyException("No ctt municipalities found in the database");
        }

        List<CttMunicipalityItem> municipalities = [];
        foreach (Dictionary<string, string> item in result.out_data)
        {
            CttMunicipalityItem municipality = new(item["cc"], item["dd"], item["name"]);
            municipalities.Add(municipality);
        }

        return municipalities;
    }

    public static CttMunicipalityItem? GetByCcAndDd(string cc, string dd, string executeUser)
    {
        Dictionary<string, string> dic = [];
        dic.Add("@Cc", cc);
        dic.Add("@Dd", dd);

        string query = "SELECT cc, dd, name FROM ctt_municipality " +
            "WHERE cc = @Cc AND dd = @Dd";

        SqlExecuterItem result = SqlExecuter.ExecuteFunction(query, dic, executeUser, false, "GetCttMunicipalityById");

        if (!result.operationResult)
        {
            throw new DatabaseException("Error fetching the ctt municipality from the database");
        }

        if (result.out_data.Count == 0)
        {
            return null;
        }

        Dictionary<string, string> item = result.out_data[0];
        CttMunicipalityItem municipality = new(item["cc"], item["dd"], item["name"]);

        return municipality;
    }

    public static List<CttMunicipalityDto> GetAllDto(string executeUser, string? dd)
    {
        Dictionary<string, string> dic = [];

        string query = "SELECT mun.cc AS cc, dis.dd AS dd, dis.name AS dis_name, mun.name AS mun_name " +
            "FROM ctt_municipality mun " +
            "JOIN ctt_district dis ON dis.dd = mun.dd " +
            "WHERE 1 = 1 ";

        if (!string.IsNullOrEmpty(dd))
        {
            dic.Add("dd", dd);
            query += "AND mun.dd = @dd ";
        }

        SqlExecuterItem result = SqlExecuter.ExecuteFunction(query, dic, executeUser, false, "GetAllCttMunicipalityDto");

        if (!result.operationResult)
        {
            throw new DatabaseException("Error fetching the ctt municipalities dtos from the database");
        }

        if (result.out_data.Count == 0)
        {
            throw new ResourceEmptyException("No ctt municipalities dtos found in the database");
        }

        List<CttMunicipalityDto> municipalities = [];
        foreach (Dictionary<string, string> item in result.out_data)
        {
            CttDistrictItem district = new(item["dd"], item["dis_name"]);
            CttMunicipalityDto municipality = new(item["cc"], district, item["mun_name"]);
            municipalities.Add(municipality);
        }

        return municipalities;
    }

}
