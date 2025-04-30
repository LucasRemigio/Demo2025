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

public static class QuoteRequestModel
{
    public static List<QuoteRequestItem> GetQuoteRequest(string execute_user)
    {
        //                     0        1                              2                    3         4          5                 6
        string query = "SELECT id, quote_id_erp, DATE_FORMAT(quote_date, '%d-%m-%Y'), client_id, client_name, product_code, quantity_requested, " +
            //    7            8                            9              10         11
            "erp_price, erp_price_modification_percent, alert_flag, special_flag, final_price, " +
            //   12             13          14          15            16                17
            "order_quantity, order_id, observation, unit_price, margin_percent, price_difference_erp, " +
            //           18                        19                     20
            "price_difference_percent_erp, total_difference_erp, total_difference_final " +
            "FROM mf_quote_request ";

        // Execute the query
        SqlExecuterItem response = SqlExecuter.ExecFunction(query, [], execute_user, false, "GetQuoteRequest");

        if (!response.operationResult)
        {
            throw new Exception("An error occurred fetching data from the quote requests table.");
        }

        List<QuoteRequestItem> quoteRequests = [];

        if (response.out_data.Count <= 0)
        {
            return quoteRequests;
        }

        // Map results to ProductDiscountDTO list
        foreach (Dictionary<string, string> item in response.out_data)
        {
            QuoteRequestItem quoteRequest = ParseQuoteRequestItem(item);

            quoteRequests.Add(quoteRequest);
        }
        return quoteRequests;
    }

    public static QuoteRequestItem GetQuoteRequestById(int id, string execute_user)
    {
        Dictionary<string, string> dic = [];
        dic.Add("id", id.ToString());
        //                     0        1                              2                    3         4          5                 6
        string query = "SELECT id, quote_id_erp, DATE_FORMAT(quote_date, '%d-%m-%Y'), client_id, client_name, product_code, quantity_requested, " +
            //    7            8                            9              10         11
            "erp_price, erp_price_modification_percent, alert_flag, special_flag, final_price, " +
            //   12             13          14          15            16                17
            "order_quantity, order_id, observation, unit_price, margin_percent, price_difference_erp, " +
            //           18                        19                     20
            "price_difference_percent_erp, total_difference_erp, total_difference_final " +
            "FROM mf_quote_request " +
            "WHERE id = @id ";

        // Execute the query
        SqlExecuterItem response = SqlExecuter.ExecFunction(query, dic, execute_user, false, "GetQuoteRequest");

        if (!response.operationResult)
        {
            throw new Exception("An error occurred fetching data from the quote requests table.");
        }

        if (response.out_data.Count <= 0)
        {
            throw new InputNotValidException($"Quote requests with id given {id} not found");
        }

        Dictionary<string, string> item = response.out_data[0];

        QuoteRequestItem quoteRequest = ParseQuoteRequestItem(item);

        return quoteRequest;
    }

    public static void CreateQuoteRequest(QuoteRequestItem quoteRequest, string execute_user)
    {
        Dictionary<string, string> dic = new Dictionary<string, string>();
        dic.Add("quote_id_erp", quoteRequest.quote_id_erp.ToString());
        dic.Add("quote_date", quoteRequest.quote_date.ToString("yyyy-MM-dd"));
        dic.Add("client_id", quoteRequest.client_id.ToString());
        dic.Add("client_name", quoteRequest.client_name);
        dic.Add("product_code", quoteRequest.product_code);
        dic.Add("quantity_requested", quoteRequest.quantity_requested.ToString());
        dic.Add("erp_price", quoteRequest.erp_price.ToString());
        dic.Add("erp_price_modification_percent", quoteRequest.erp_price_modification_percent.ToString());
        dic.Add("alert_flag", quoteRequest.alert_flag ? "1" : "0");
        dic.Add("special_flag", quoteRequest.special_flag ? "1" : "0");
        dic.Add("final_price", quoteRequest.final_price.ToString());
        dic.Add("order_quantity", quoteRequest.order_quantity.ToString());
        dic.Add("order_id", quoteRequest.order_id);
        dic.Add("observation", quoteRequest.observation);
        dic.Add("unit_price", quoteRequest.unit_price.ToString());
        dic.Add("margin_percent", quoteRequest.margin_percent.ToString());
        dic.Add("price_difference_erp", quoteRequest.price_difference_erp.ToString());
        dic.Add("price_difference_percent_erp", quoteRequest.price_difference_percent_erp.ToString());
        dic.Add("total_difference_erp", quoteRequest.total_difference_erp.ToString());
        dic.Add("total_difference_final", quoteRequest.total_difference_final.ToString());

        string query = "INSERT INTO mf_quote_request (quote_id_erp, quote_date, client_id, client_name, product_code, quantity_requested, " +
            "erp_price, erp_price_modification_percent, alert_flag, special_flag, final_price, " +
            "order_quantity, order_id, observation, unit_price, margin_percent, price_difference_erp, " +
            "price_difference_percent_erp, total_difference_erp, total_difference_final) " +
            "VALUES (@quote_id_erp, @quote_date, @client_id, @client_name, @product_code, @quantity_requested, " +
            "@erp_price, @erp_price_modification_percent, @alert_flag, @special_flag, @final_price, " +
            "@order_quantity, @order_id, @observation, @unit_price, @margin_percent, @price_difference_erp, " +
            "@price_difference_percent_erp, @total_difference_erp, @total_difference_final)";

        SqlExecuterItem response = SqlExecuter.ExecFunction(query, dic, execute_user, true, "CreateQuoteRequest");

        if (!response.operationResult)
        {
            throw new Exception("An error occurred creating a new quote request.");
        }
    }

    public static void UpdateQuoteRequest(QuoteRequestItem quoteRequest, string execute_user)
    {
        // Select the quote request to update so we can check what parameters changed and update them
        QuoteRequestItem oldQuoteRequest = GetQuoteRequestById(quoteRequest.id, execute_user);

        Dictionary<string, string> dic = [];

        string query = "UPDATE mf_quote_request SET ";

        //  check what parameters changed and then string join them separated by commas
        List<string> updateFields = [];

        if (oldQuoteRequest.quote_id_erp != quoteRequest.quote_id_erp)
        {
            updateFields.Add("quote_id_erp = @quote_id_erp");
            dic.Add("quote_id_erp", quoteRequest.quote_id_erp.ToString());
        }

        if (oldQuoteRequest.quote_date != quoteRequest.quote_date)
        {
            updateFields.Add("quote_date = @quote_date");
            dic.Add("quote_date", quoteRequest.quote_date.ToString("yyyy-MM-dd"));
        }

        if (oldQuoteRequest.client_id != quoteRequest.client_id)
        {
            updateFields.Add("client_id = @client_id");
            dic.Add("client_id", quoteRequest.client_id.ToString());
        }

        if (oldQuoteRequest.client_name != quoteRequest.client_name)
        {
            updateFields.Add("client_name = @client_name");
            dic.Add("client_name", quoteRequest.client_name);
        }

        if (oldQuoteRequest.product_code != quoteRequest.product_code)
        {
            updateFields.Add("product_code = @product_code");
            dic.Add("product_code", quoteRequest.product_code);
        }

        if (oldQuoteRequest.quantity_requested != quoteRequest.quantity_requested)
        {
            updateFields.Add("quantity_requested = @quantity_requested");
            dic.Add("quantity_requested", quoteRequest.quantity_requested.ToString());
        }

        if (oldQuoteRequest.erp_price != quoteRequest.erp_price)
        {
            updateFields.Add("erp_price = @erp_price");
            dic.Add("erp_price", quoteRequest.erp_price.ToString());
        }

        if (oldQuoteRequest.erp_price_modification_percent != quoteRequest.erp_price_modification_percent)
        {
            updateFields.Add("erp_price_modification_percent = @erp_price_modification_percent");
            dic.Add("erp_price_modification_percent", quoteRequest.erp_price_modification_percent.ToString());
        }

        if (oldQuoteRequest.alert_flag != quoteRequest.alert_flag)
        {
            updateFields.Add("alert_flag = @alert_flag");
            dic.Add("alert_flag", quoteRequest.alert_flag ? "1" : "0");
        }

        if (oldQuoteRequest.special_flag != quoteRequest.special_flag)
        {
            updateFields.Add("special_flag = @special_flag");
            dic.Add("special_flag", quoteRequest.special_flag ? "1" : "0");
        }

        if (oldQuoteRequest.final_price != quoteRequest.final_price)
        {
            updateFields.Add("final_price = @final_price");
            dic.Add("final_price", quoteRequest.final_price.ToString());
        }

        if (oldQuoteRequest.order_quantity != quoteRequest.order_quantity)
        {
            updateFields.Add("order_quantity = @order_quantity");
            dic.Add("order_quantity", quoteRequest.order_quantity.ToString());
        }

        if (oldQuoteRequest.order_id != quoteRequest.order_id)
        {
            updateFields.Add("order_id = @order_id");
            dic.Add("order_id", quoteRequest.order_id);
        }

        if (oldQuoteRequest.observation != quoteRequest.observation)
        {
            updateFields.Add("observation = @observation");
            dic.Add("observation", quoteRequest.observation);
        }

        if (oldQuoteRequest.unit_price != quoteRequest.unit_price)
        {
            updateFields.Add("unit_price = @unit_price");
            dic.Add("unit_price", quoteRequest.unit_price.ToString());
        }

        if (oldQuoteRequest.margin_percent != quoteRequest.margin_percent)
        {
            updateFields.Add("margin_percent = @margin_percent");
            dic.Add("margin_percent", quoteRequest.margin_percent.ToString());
        }

        if (oldQuoteRequest.price_difference_erp != quoteRequest.price_difference_erp)
        {
            updateFields.Add("price_difference_erp = @price_difference_erp");
            dic.Add("price_difference_erp", quoteRequest.price_difference_erp.ToString());
        }

        if (oldQuoteRequest.price_difference_percent_erp != quoteRequest.price_difference_percent_erp)
        {
            updateFields.Add("price_difference_percent_erp = @price_difference_percent_erp");
            dic.Add("price_difference_percent_erp", quoteRequest.price_difference_percent_erp.ToString());
        }

        if (oldQuoteRequest.total_difference_erp != quoteRequest.total_difference_erp)
        {
            updateFields.Add("total_difference_erp = @total_difference_erp");
            dic.Add("total_difference_erp", quoteRequest.total_difference_erp.ToString());
        }

        if (oldQuoteRequest.total_difference_final != quoteRequest.total_difference_final)
        {
            updateFields.Add("total_difference_final = @total_difference_final");
            dic.Add("total_difference_final", quoteRequest.total_difference_final.ToString());
        }

        if (updateFields.Count <= 0)
        {
            throw new InputNotValidException("No fields to update.");
        }

        query += string.Join(", ", updateFields) + " WHERE id = @id";
        dic.Add("id", quoteRequest.id.ToString());

        SqlExecuterItem response = SqlExecuter.ExecFunction(query, dic, execute_user, true, "UpdateQuoteRequest");

        if (!response.operationResult)
        {
            throw new Exception("An error occurred updating the quote request.");
        }
    }

    public static void DeleteQuoteRequest(int id, string execute_user)
    {
        Dictionary<string, string> dic = [];
        dic.Add("id", id.ToString());

        string query = "DELETE FROM mf_quote_request WHERE id = @id";

        SqlExecuterItem response = SqlExecuter.ExecFunction(query, dic, execute_user, true, "DeleteQuoteRequest");

        if (!response.operationResult)
        {
            throw new Exception("An error occurred deleting the quote request.");
        }
    }

    public static QuoteRequestItem ParseQuoteRequestItem(Dictionary<string, string> item)
    {
        // Integer fields
        int id = String.IsNullOrEmpty(item["0"]) ? default : Int32.Parse(item["0"]);
        int quoteIdErp = String.IsNullOrEmpty(item["1"]) ? default : Int32.Parse(item["1"]);
        int clientId = String.IsNullOrEmpty(item["3"]) ? default : Int32.Parse(item["3"]);

        // Date field (expects format dd-MM-yyyy)
        DateOnly quoteDate = String.IsNullOrEmpty(item["2"]) ? default : DateOnly.ParseExact(item["2"], "dd-MM-yyyy", null);

        // String fields
        string clientName = String.IsNullOrEmpty(item["4"]) ? string.Empty : item["4"];
        string productCode = String.IsNullOrEmpty(item["5"]) ? string.Empty : item["5"];
        string orderId = String.IsNullOrEmpty(item["13"]) ? string.Empty : item["13"];
        string observation = String.IsNullOrEmpty(item["14"]) ? string.Empty : item["14"];

        // Decimal fields
        decimal quantityRequested = String.IsNullOrEmpty(item["6"]) ? default : Decimal.Parse(item["6"]);
        decimal erpPrice = String.IsNullOrEmpty(item["7"]) ? default : Decimal.Parse(item["7"]);
        decimal erpPriceModificationPercent = String.IsNullOrEmpty(item["8"]) ? default : Decimal.Parse(item["8"]);
        decimal finalPrice = String.IsNullOrEmpty(item["11"]) ? default : Decimal.Parse(item["11"]);
        decimal orderQuantity = String.IsNullOrEmpty(item["12"]) ? default : Decimal.Parse(item["12"]);
        decimal unitPrice = String.IsNullOrEmpty(item["15"]) ? default : Decimal.Parse(item["15"]);
        decimal marginPercent = String.IsNullOrEmpty(item["16"]) ? default : Decimal.Parse(item["16"]);
        decimal priceDifferenceErp = String.IsNullOrEmpty(item["17"]) ? default : Decimal.Parse(item["17"]);
        decimal priceDifferencePercentErp = String.IsNullOrEmpty(item["18"]) ? default : Decimal.Parse(item["18"]);
        decimal totalDifferenceErp = String.IsNullOrEmpty(item["19"]) ? default : Decimal.Parse(item["19"]);
        decimal totalDifferenceFinal = String.IsNullOrEmpty(item["20"]) ? default : Decimal.Parse(item["20"]);

        // Boolean fields (1 = true, 0 = false)
        bool alertFlag = !String.IsNullOrEmpty(item["9"]) && item["9"] == "1";
        bool specialFlag = !String.IsNullOrEmpty(item["10"]) && item["10"] == "1";

        // Construct and return the QuoteRequestItem object
        return new QuoteRequestItem(
            id, quoteIdErp, quoteDate, clientId, clientName, productCode, quantityRequested,
            erpPrice, erpPriceModificationPercent, alertFlag, specialFlag, finalPrice,
            orderQuantity, orderId, observation, unitPrice, marginPercent, priceDifferenceErp,
            priceDifferencePercentErp, totalDifferenceErp, totalDifferenceFinal
        );
    }
}
