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

public static class ProductTypeModel
{
    public static List<ProductTypeItem> GetUsedProductTypes(string execute_user)
    {
        string query = "SELECT pt.id AS type_id, pt.name AS type_name " +
            "FROM mf_product_catalog pc " +
            "JOIN mf_product_type pt ON pc.type_id = pt.id " +
            "WHERE pc.type_id IS NOT NULL " +
            "GROUP BY pc.type_id";

        SqlExecuterItem response = SqlExecuter.ExecuteFunction(query, [], execute_user, false, "GetUserProductTypes");

        if (!response.operationResult)
        {
            throw new Exception("Error getting used product types");
        }

        if (response.out_data.Count == 0)
        {
            return [];
        }

        List<ProductTypeItem> productTypes = [];
        foreach (Dictionary<string, string> item in response.out_data)
        {
            int id = Int32.Parse(item["type_id"]);
            string name = item["type_name"];
            productTypes.Add(new ProductTypeItem(id, name));
        }

        return productTypes;
    }

    public static List<ProductTypePropertiesItem> GetAllPropertiesAssociatedWithProductType(List<int> productTypeId, string execute_user)
    {
        string query = "SELECT pt.id AS type_id, pt.name AS type_name, " +
                "GROUP_CONCAT(DISTINCT pm.name ORDER BY pm.name SEPARATOR ', ') AS materials, " +
                "GROUP_CONCAT(DISTINCT pf.name ORDER BY pf.name SEPARATOR ', ') AS finishings, " +
                "GROUP_CONCAT(DISTINCT ps.name ORDER BY ps.name SEPARATOR ', ') AS shapes, " +
                "GROUP_CONCAT(DISTINCT psurf.name ORDER BY psurf.name SEPARATOR ', ') AS surfaces " +
            "FROM mf_product_catalog pc " +
                "JOIN mf_product_type pt ON pc.type_id = pt.id " +
                "LEFT JOIN mf_product_material pm ON pc.material_id = pm.id " +
                "LEFT JOIN mf_product_finishing pf ON pc.finishing_id = pf.id " +
                "LEFT JOIN mf_product_shape ps ON pc.shape_id = ps.id " +
                "LEFT JOIN mf_product_surface psurf ON pc.surface_id = psurf.id " +
            "WHERE pc.type_id IS NOT NULL " +
            "AND pt.id IN (" + string.Join(",", productTypeId) + ") " +
            "GROUP BY pt.id, pt.name;";

        SqlExecuterItem response = SqlExecuter.ExecuteFunction(query, [], execute_user, false, "GetAllPropertiesAssociatedWithProductType");

        if (!response.operationResult)
        {
            throw new Exception("Error getting properties associated with product type");
        }

        if (response.out_data.Count == 0)
        {
            return [];
        }

        List<ProductTypePropertiesItem> productTypes = [];
        foreach (Dictionary<string, string> item in response.out_data)
        {
            int id = Int32.Parse(item["type_id"]);
            string name = item["type_name"];
            string materials = item["materials"];
            string finishings = item["finishings"];
            string shapes = item["shapes"];
            string surfaces = item["surfaces"];

            ProductTypePropertiesItem productType = new(id, name, materials, finishings, shapes, surfaces);
            productTypes.Add(productType);
        }

        return productTypes;
    }
}
