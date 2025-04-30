// // Copyright (c) 2024 Engibots. All rights reserved.

using engimatrix.ModelObjs;

namespace engimatrix.Models;
public static class PricingStrategyModel
{

    public static List<PricingStrategyItem> GetPricingStrategies(string executeUser)
    {

        string query = "SELECT id, name, slug FROM mf_pricing_strategy ORDER BY id";

        SqlExecuterItem response = SqlExecuter.ExecuteFunction(query, [], executeUser, false, "GetPricingStrategies");

        if (!response.operationResult)
        {
            throw new Exception("Something wrong happened getting the pricing strategies");
        }

        if (response.out_data.Count <= 0)
        {
            // Its excepted for this table to have at least 2 records, always
            throw new Exception("Error retrieving the pricing strategies");
        }

        List<PricingStrategyItem> pricingStrategies = [];
        foreach (Dictionary<string, string> item in response.out_data)
        {
            int id = Int32.Parse(item["id"]);
            string name = item["name"];
            string slug = item["slug"];

            PricingStrategyItem pricing = new(id, name, slug);
            pricingStrategies.Add(pricing);
        }

        return pricingStrategies;
    }
}
