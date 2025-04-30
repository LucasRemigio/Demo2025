// // Copyright (c) 2024 Engibots. All rights reserved.

using System.Globalization;
using System.Linq;
using System.Text.RegularExpressions;
using engimatrix.Config;
using engimatrix.Connector;
using engimatrix.Exceptions;
using engimatrix.ModelObjs;
using engimatrix.Utils;
using Engimatrix.ModelObjs;
using Microsoft.Graph.Models;
using Microsoft.Graph.Models.CallRecords;
using Microsoft.IdentityModel.Tokens;

namespace engimatrix.Models;

public static class ProductUnitModel
{
    private static Dictionary<string, ProductUnitItem> productUnitByAbbreviation = [];
    private static DateTime lastUpdate = DateTime.MinValue;

    public static List<ProductUnitItem> GetProductUnits(string execute_user)
    {
        string query = "SELECT id, abbreviation, name, slug " +
            "FROM product_unit";

        SqlExecuterItem response = SqlExecuter.ExecuteFunction(query, [], execute_user, false, "GetAllProductUnits");

        if (!response.operationResult)
        {
            throw new Exception("Error getting product units");
        }

        if (response.out_data.Count == 0)
        {
            return [];
        }

        List<ProductUnitItem> productUnits = [];
        foreach (Dictionary<string, string> item in response.out_data)
        {
            productUnits.Add(new ProductUnitItemBuilder()
                .SetId(Int32.Parse(item["id"]))
                .SetAbbreviation(item["abbreviation"])
                .SetName(item["name"])
                .SetSlug(item["slug"])
                .Build()
            );
        }

        return productUnits;
    }

    public static ProductUnitItem? GetProductUnitById(int id, string execute_user)
    {
        Dictionary<string, string> dic = new(){
            { "@id", id.ToString()}
        };

        string query = "SELECT id, abbreviation, name, slug " +
            "FROM product_unit " +
            "WHERE id = @id";

        SqlExecuterItem response = SqlExecuter.ExecuteFunction(query, dic, execute_user, false, "GetProductUnitById");

        if (!response.operationResult)
        {
            throw new Exception("Error getting product unit");
        }

        if (response.out_data.Count == 0)
        {
            return null;
        }

        Dictionary<string, string> item = response.out_data[0];

        ProductUnitItem productUnit = new ProductUnitItemBuilder()
            .SetId(Int32.Parse(item["id"]))
            .SetAbbreviation(item["abbreviation"])
            .SetName(item["name"])
            .SetSlug(item["slug"])
            .Build();

        return productUnit;
    }

    public static Dictionary<string, ProductUnitItem> GetHashedProductUnitsByAbbreviation(string execute_user)
    {
        if (!IsCacheValid())
        {
            List<ProductUnitItem> productUnits = GetProductUnits(execute_user);
            productUnitByAbbreviation = HashProductUnitByAbbreviation(productUnits);
            lastUpdate = DateTime.Now;
        }

        return productUnitByAbbreviation;
    }

    private static bool IsCacheValid()
    {
        return lastUpdate.AddMinutes(10) > DateTime.Now;
    }

    public static Dictionary<string, ProductUnitItem> HashProductUnitByAbbreviation(List<ProductUnitItem> productUnits)
    {
        Dictionary<string, ProductUnitItem> productUnitsHash = [];

        foreach (ProductUnitItem productUnit in productUnits)
        {
            productUnitsHash[productUnit.abbreviation] = productUnit;
        }

        return productUnitsHash;
    }

}
