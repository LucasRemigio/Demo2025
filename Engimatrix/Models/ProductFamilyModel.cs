// // Copyright (c) 2024 Engibots. All rights reserved.

using System.Globalization;
using engimatrix.Config;
using engimatrix.Exceptions;
using engimatrix.ModelObjs;
using engimatrix.Utils;
using Microsoft.Graph.Models;
using Microsoft.Graph.Models.CallRecords;
using Microsoft.IdentityModel.Tokens;

namespace engimatrix.Models;

public static class ProductFamilyModel
{
    public static List<ProductFamilyItem> GetProductFamilies(string execute_user)
    {
        string query = "SELECT id, name FROM mf_product_family";

        // Execute the query
        SqlExecuterItem response = SqlExecuter.ExecFunction(query, new Dictionary<string, string>(), execute_user, false, "GetProductFamilies");

        if (!response.operationResult)
        {
            throw new Exception("An error occurred fetching data from the product family table.");
        }

        List<ProductFamilyItem> productFamilies = new List<ProductFamilyItem>();

        if (response.out_data.Count <= 0)
        {
            return productFamilies;
        }

        // Map results to ProductDiscountDTO list
        foreach (Dictionary<string, string> item in response.out_data)
        {
            ProductFamilyItem productFamily = new ProductFamilyItem(item["0"], item["1"]);
            productFamilies.Add(productFamily);
        }
        return productFamilies;
    }

    public static ProductFamilyItem GetProductFamilyById(string productFamilyId, string execute_user)
    {
        Dictionary<string, string> dic = new();
        dic.Add("productFamilyId", productFamilyId);

        string query = "SELECT id, name " +
            "FROM mf_product_family " +
            "WHERE id = @productFamilyId ";

        // Execute the query
        SqlExecuterItem response = SqlExecuter.ExecFunction(query, dic, execute_user, false, "GetProductFamilyById");

        if (!response.operationResult)
        {
            throw new Exception("An error occurred fetching data from the product family table.");
        }

        if (response.out_data.Count <= 0)
        {
            throw new InputNotValidException($"No product family with id {productFamilyId} was found");
        }

        Dictionary<string, string> item = response.out_data[0];

        ProductFamilyItem productFamily = new ProductFamilyItem(item["0"], item["1"]);

        return productFamily;
    }

    public static void CreateProductFamily(ProductFamilyItem productFamily, string execute_user)
    {
        // Select to see if the product family already exists
        Dictionary<string, string> dic = new()
            {
            { "name", productFamily.name }
        };

        string query = "SELECT id " +
            "FROM mf_product_family " +
            "WHERE name = @name";

        // Execute the query
        SqlExecuterItem response = SqlExecuter.ExecFunction(query, dic, execute_user, false, "CreateProductFamily");

        if (!response.operationResult)
        {
            throw new Exception("An error occurred fetching data from the product family table.");
        }

        if (response.out_data.Count > 0)
        {
            throw new InputNotValidException($"A product family with name {productFamily.name} already exists");
        }

        dic.Clear();
        dic.Add("id", productFamily.id);
        dic.Add("name", productFamily.name);

        query = "INSERT INTO mf_product_family (id, name) " +
            "VALUES (@id, @name)";

        // Execute the query
        response = SqlExecuter.ExecFunction(query, dic, execute_user, true, "CreateProductFamily");

        if (!response.operationResult)
        {
            throw new Exception("An error occurred creating the product family.");
        }
    }

    public static void UpdateProductFamily(ProductFamilyItem productFamily, string execute_user)
    {
        // Select to see if the product family already exists
        Dictionary<string, string> dic = new()
            {
            { "id", productFamily.id }
        };

        string query = "SELECT id " +
            "FROM mf_product_family " +
            "WHERE id = @id";

        // Execute the query
        SqlExecuterItem response = SqlExecuter.ExecFunction(query, dic, execute_user, false, "UpdateProductFamily");

        if (!response.operationResult)
        {
            throw new Exception("An error occurred fetching data from the product family table.");
        }

        if (response.out_data.Count <= 0)
        {
            throw new InputNotValidException($"No product family with id {productFamily.id} was found");
        }

        dic.Clear();
        dic.Add("id", productFamily.id);
        dic.Add("name", productFamily.name);

        query = "UPDATE mf_product_family " +
            "SET name = @name " +
            "WHERE id = @id";

        // Execute the query
        response = SqlExecuter.ExecFunction(query, dic, execute_user, true, "UpdateProductFamily");

        if (!response.operationResult)
        {
            throw new Exception("An error occurred updating the product family.");
        }
    }

    public static void DeleteProductFamily(string productFamilyId, string execute_user)
    {
        Dictionary<string, string> dic = new()
        {
            { "productFamilyId", productFamilyId }
        };

        string query = "DELETE FROM mf_product_family " +
            "WHERE id = @productFamilyId";

        // Execute the query
        SqlExecuterItem response = SqlExecuter.ExecFunction(query, dic, execute_user, true, "DeleteProductFamily");

        if (!response.operationResult)
        {
            throw new Exception("An error occurred deleting the product family.");
        }
    }
}
