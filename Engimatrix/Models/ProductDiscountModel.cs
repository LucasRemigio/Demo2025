// // Copyright (c) 2024 Engibots. All rights reserved.

using System.Globalization;
using engimatrix.Config;
using engimatrix.Exceptions;
using engimatrix.ModelObjs;
using engimatrix.Utils;
using Microsoft.Graph.Models;
using Microsoft.IdentityModel.Tokens;

namespace engimatrix.Models;

public static class ProductDiscountModel
{
    public static List<ProductDiscountDTO> GetProductDiscountsDTO(string execute_user, string? productFamilyId, int? segmentId)
    {
        Dictionary<string, string> dic = [];

        string query = "SELECT f.id AS family_id, f.name AS family_name, s.id AS segment_id, s.name AS segment_name, " +
                       "p.mb_min AS min_margin, p.desc_max AS max_discount " +
                       "FROM mf_product_discount p " +
                       "JOIN mf_segment s ON p.segment_id = s.id " +
                       "JOIN mf_product_family f ON p.product_family_id = f.id " +
                       "WHERE 1=1 ";

        if (!string.IsNullOrEmpty(productFamilyId))
        {
            // Only productFamilyId is provided
            query += "AND f.id = @prodFamilyId ";
            dic.Add("prodFamilyId", productFamilyId);
        }
        if (segmentId.HasValue)
        {
            // Only segmentId is provided
            query += "AND s.id = @segmentId ";
            dic.Add("segmentId", segmentId.Value.ToString());
        }

        query += "ORDER BY f.id, s.id";

        // Execute the query
        SqlExecuterItem response = SqlExecuter.ExecuteFunction(query, dic, execute_user, false, "GetProductDiscountsDTO");

        if (!response.operationResult)
        {
            throw new Exception("An error occurred fetching data from the product discounts table.");
        }

        List<ProductDiscountDTO> products = [];

        if (response.out_data.Count <= 0)
        {
            return products;
        }

        // Map results to ProductDiscountDTO list
        foreach (Dictionary<string, string> item in response.out_data)
        {
            ProductFamilyItem productFamily = new(item["family_id"], item["family_name"]);
            SegmentItem segment = new(Int32.Parse(item["segment_id"]), item["segment_name"]);
            ProductDiscountDTO productDiscount = new(productFamily, segment, Decimal.Parse(item["min_margin"]), Decimal.Parse(item["max_discount"]));
            products.Add(productDiscount);
        }

        return products;
    }

    public static ProductDiscountDTO GetProductDiscountByProductFamilyIdBySegmentIdDTO(string productFamilyId, int segmentId, string execute_user)
    {
        Dictionary<string, string> dic = new()
        {
            { "prodFamilyId", productFamilyId },
            { "segmentId", segmentId.ToString() }
        };

        string query = "SELECT f.id, f.name, s.id, s.name, p.mb_min, p.desc_max " +
                       "FROM mf_product_discount p " +
                       "JOIN mf_segment s ON p.segment_id = s.id " +
                       "JOIN mf_product_family f ON p.product_family_id = f.id " +
                       "WHERE f.id = @prodFamilyId AND s.id = @segmentId " +
                       "ORDER BY f.id, s.id";

        // Execute the query
        SqlExecuterItem response = SqlExecuter.ExecFunction(query, dic, execute_user, false, "GetProductDiscountByProductFamilyIdBySegmentIdDTO");

        if (!response.operationResult)
        {
            throw new Exception("An error occurred fetching data from the product discounts table.");
        }

        if (response.out_data.Count <= 0)
        {
            return new ProductDiscountDTO();
        }

        Dictionary<string, string> item = response.out_data[0];

        ProductFamilyItem productFamily = new(item["0"], item["1"]);
        SegmentItem segment = new(Int32.Parse(item["2"]), item["3"]);
        ProductDiscountDTO productDiscount = new(productFamily, segment, Decimal.Parse(item["4"]), Decimal.Parse(item["5"]));

        return productDiscount;
    }

    public static ProductDiscountItem? GetProductDiscountByProductFamilyIdBySegmentId(string productFamilyId, int segmentId, string execute_user)
    {
        Dictionary<string, string> dic = [];
        dic.Add("@segmentId", segmentId.ToString());
        dic.Add("@prodFamilyId", productFamilyId);

        string query = "SELECT product_family_id, segment_id, mb_min, desc_max " +
                       "FROM mf_product_discount " +
                       "WHERE product_family_id = @prodFamilyId " +
                            "AND segment_id = @segmentId ";

        // Execute the query
        SqlExecuterItem response = SqlExecuter.ExecFunction(query, dic, execute_user, false, "GetProductDiscountByProductFamilyIdBySegmentIdDTO");

        if (!response.operationResult)
        {
            throw new Exception("An error occurred fetching data from the product discounts table.");
        }

        if (response.out_data.Count <= 0)
        {
            return null;
        }

        // Map results to ProductDiscountDTO list
        Dictionary<string, string> item = response.out_data[0];

        ProductDiscountItem productDiscount = new(item["0"], Int32.Parse(item["1"]), Decimal.Parse(item["2"]), Decimal.Parse(item["3"]));

        return productDiscount;
    }

    public static List<ProductDiscountItem> GetProductDiscountsByProductFamilyIdListBySegmentId(List<string> productFamilyIdList, int segmentId, string execute_user)
    {
        Dictionary<string, string> dic = new()
        {
            { "@segmentId", segmentId.ToString() }
        };

        // Properly quote the product family IDs
        string quotedProductFamilyIds = String.Join(", ", productFamilyIdList.Select(id => $"'{id}'"));

        string query = "SELECT product_family_id, segment_id, mb_min, desc_max " +
                       "FROM mf_product_discount " +
                       $"WHERE product_family_id IN ({quotedProductFamilyIds}) AND segment_id = @segmentId ";

        // Execute the query
        SqlExecuterItem response = SqlExecuter.ExecFunction(query, dic, execute_user, false, "GetProductDiscountByProductFamilyIdBySegmentIdDTO");

        if (!response.operationResult)
        {
            throw new Exception("An error occurred fetching data from the product discounts table.");
        }

        List<ProductDiscountItem> products = [];

        if (response.out_data.Count <= 0)
        {
            return products;
        }

        // Map results to ProductDiscountDTO list
        foreach (Dictionary<string, string> item in response.out_data)
        {
            ProductDiscountItem productDiscount = new(item["0"], Int32.Parse(item["1"]), Decimal.Parse(item["2"]), Decimal.Parse(item["3"]));
            products.Add(productDiscount);
        }

        return products;
    }

    public static void CreateProductDiscount(ProductDiscountItem productDiscount, string execute_user)
    {
        Dictionary<string, string> dic = new()
        {
            { "prodFamilyId", productDiscount.product_family_id },
            { "segmentId", productDiscount.segment_id.ToString() },
            { "mbMin", productDiscount.mb_min.ToString(CultureInfo.InvariantCulture) },
            { "descMax", productDiscount.desc_max.ToString(CultureInfo.InvariantCulture) }
        };

        string query = "INSERT INTO mf_product_discount (product_family_id, segment_id, mb_min, desc_max) " +
                       "VALUES (@prodFamilyId, @segmentId, @mbMin, @descMax)";

        // Execute the query
        SqlExecuterItem response = SqlExecuter.ExecFunction(query, dic, execute_user, true, "CreateProductDiscount");

        if (!response.operationResult)
        {
            throw new Exception("An error occurred creating the product discount.");
        }
    }

    public static void UpdateProductDiscount(ProductDiscountItem productDiscount, string execute_user)
    {
        Dictionary<string, string> dic = new()
        {
            { "prodFamilyId", productDiscount.product_family_id },
            { "segmentId", productDiscount.segment_id.ToString() },
            { "mbMin", productDiscount.mb_min.ToString(CultureInfo.InvariantCulture) },
            { "descMax", productDiscount.desc_max.ToString(CultureInfo.InvariantCulture) }
        };

        string query = "UPDATE mf_product_discount " +
                       "SET mb_min = @mbMin, desc_max = @descMax " +
                       "WHERE product_family_id = @prodFamilyId AND segment_id = @segmentId";

        // Execute the query
        SqlExecuterItem response = SqlExecuter.ExecFunction(query, dic, execute_user, true, "UpdateProductDiscount");

        if (!response.operationResult)
        {
            throw new Exception("An error occurred updating the product discount.");
        }
    }

    public static void DeleteProductDiscount(string productFamilyId, int segmentId, string execute_user)
    {
        Dictionary<string, string> dic = new()
        {
            { "prodFamilyId", productFamilyId },
            { "segmentId", segmentId.ToString() }
        };

        string query = "DELETE FROM mf_product_discount " +
                       "WHERE product_family_id = @prodFamilyId AND segment_id = @segmentId";

        // Execute the query
        SqlExecuterItem response = SqlExecuter.ExecFunction(query, dic, execute_user, true, "DeleteProductDiscount");

        if (!response.operationResult)
        {
            throw new Exception("An error occurred deleting the product discount.");
        }
    }
}
