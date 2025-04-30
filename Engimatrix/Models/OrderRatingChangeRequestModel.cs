// // Copyright (c) 2024 Engibots. All rights reserved.

using System.Diagnostics;
using System.Globalization;
using engimatrix.Config;
using engimatrix.Exceptions;
using engimatrix.ModelObjs;
using engimatrix.ModelObjs.Primavera;
using engimatrix.Utils;
using engimatrix.Views;
using Engimatrix.ModelObjs;
using Engimatrix.Models;
using Microsoft.AspNetCore.Http.HttpResults;
namespace engimatrix.Models;

public static class OrderRatingChangeRequestModel
{
    public static List<OrderRatingChangeRequestItem> GetByOrderToken(string orderToken, string executeUser)
    {
        Dictionary<string, string> dic = new()
        {
            { "@OrderToken", orderToken }
        };

        string query = "SELECT id, order_token, rating_type_id, new_rating, status, requested_by, requested_at, verified_by, verified_at " +
            "FROM order_rating_change_request " +
            "WHERE order_token = @OrderToken";

        SqlExecuterItem response = SqlExecuter.ExecuteFunction(query, dic, executeUser, true, "GetRatingChangeRequestItem");

        if (!response.operationResult)
        {
            throw new DatabaseException("Error getting rating change request item");
        }

        if (response.out_data.Count == 0)
        {
            return [];
        }

        List<OrderRatingChangeRequestItem> ratingChangeRequestItems = new();
        foreach (Dictionary<string, string> item in response.out_data)
        {
            OrderRatingChangeRequestItem ratingChangeRequestItem = ToItem(item);

            ratingChangeRequestItems.Add(ratingChangeRequestItem);
        }

        return ratingChangeRequestItems;
    }

    public static List<OrderRatingChangeRequestDto> GetDtoByOrderToken(string orderToken, string execute_user)
    {
        Dictionary<string, string> dic = new()
        {
            { "@order_token", orderToken }
        };

        string query = "SELECT orr.id AS request_id, orr.order_token AS order_token, rt.id AS rating_type_id, rt.description AS rating_type_description, " +
            "rt.slug AS rating_type_slug, rt.weight AS rating_type_weight, rd.rating AS rating_discount_rating, rd.percentage AS rating_discount_percentage, " +
            "orr.status AS status, orr.requested_by AS requested_by, orr.requested_at AS requested_at, orr.verified_at AS verified_at, orr.verified_by AS verified_by " +
            "FROM order_rating_change_request orr " +
                "JOIN mf_rating_type rt ON orr.rating_type_id = rt.id " +
                "JOIN mf_rating_discount rd ON rd.rating = orr.new_rating " +
            "WHERE orr.order_token = @order_token " +
            "ORDER BY orr.order_token, rt.id;";

        SqlExecuterItem response = SqlExecuter.ExecuteFunction(query, dic, execute_user, false, "GetOrderRatingsByOrderToken");

        if (response.out_data.Count <= 0)
        {
            return [];
        }

        List<OrderRatingChangeRequestDto> ratings = [];
        foreach (Dictionary<string, string> item in response.out_data)
        {
            RatingTypeItem rating_type = new(int.Parse(item["rating_type_id"]), item["rating_type_description"], item["rating_type_slug"], decimal.Parse(item["rating_type_weight"], CultureInfo.InvariantCulture));
            RatingDiscountItem rating_discount = new(Convert.ToChar(item["rating_discount_rating"]), decimal.Parse(item["rating_discount_percentage"], CultureInfo.InvariantCulture));

            DateTime? requestedAt = !string.IsNullOrEmpty(item["requested_at"]) ? DateTime.Parse(item["requested_at"]) : null;
            DateTime? verifiedAt = !string.IsNullOrEmpty(item["verified_at"]) ? DateTime.Parse(item["verified_at"]) : null;

            OrderRatingChangeRequestDto rating = new OrderRatingChangeRequestDtoBuilder()
                .SetId(int.Parse(item["request_id"]))
                .SetOrderToken(item["order_token"])
                .SetRatingType(rating_type)
                .SetNewRatingDiscount(rating_discount)
                .SetStatus(item["status"])
                .SetRequestedAt(requestedAt)
                .SetRequestedBy(item["requested_by"])
                .SetVerifiedAt(verifiedAt)
                .SetVerifiedBy(item["verified_by"])
                .Build();

            ratings.Add(rating);
        }

        return ratings;
    }

    public static List<OrderRatingChangeRequestItem> GetByOrderTokenByRequestTypeId(string orderToken, int requestTypeId, string executeUser)
    {
        string query = "SELECT id, order_token, rating_type_id, new_rating, status, requested_by, requested_at, verified_by, verified_at " +
            "FROM order_rating_change_request " +
            "WHERE order_token = @OrderToken AND rating_type_id = @RatingTypeId";

        Dictionary<string, string> dic = new(){
            { "@OrderToken", orderToken },
            { "@RatingTypeId", requestTypeId.ToString() }
        };

        SqlExecuterItem response = SqlExecuter.ExecuteFunction(query, dic, executeUser, true, "GetRatingChangeRequestItem");

        if (!response.operationResult)
        {
            throw new DatabaseException("Error getting rating change request item");
        }

        if (response.out_data.Count == 0)
        {
            return [];
        }

        List<OrderRatingChangeRequestItem> ratingChangeRequestItems = [];
        foreach (Dictionary<string, string> item in response.out_data)
        {
            OrderRatingChangeRequestItem changeRequest = ToItem(item);

            ratingChangeRequestItems.Add(changeRequest);
        }

        return ratingChangeRequestItems;
    }

    public static OrderRatingChangeRequestItem? GetById(int id, string executeUser)
    {
        string query = "SELECT id, order_token, rating_type_id, new_rating, status, requested_by, requested_at, verified_by, verified_at " +
            "FROM order_rating_change_request " +
            "WHERE id = @Id";

        Dictionary<string, string> dic = new(){
            { "@Id", id.ToString() }
        };

        SqlExecuterItem response = SqlExecuter.ExecuteFunction(query, dic, executeUser, true, "GetRatingChangeRequestItem");

        if (!response.operationResult)
        {
            throw new DatabaseException("Error getting rating change request item");
        }

        if (response.out_data.Count == 0)
        {
            return null;
        }

        Dictionary<string, string> item = response.out_data[0];
        OrderRatingChangeRequestItem changeRequest = ToItem(item);

        return changeRequest;
    }

    private static OrderRatingChangeRequestItem ToItem(Dictionary<string, string> item)
    {
        DateTime? requestedAt = !string.IsNullOrEmpty(item["requested_at"]) ? DateTime.Parse(item["requested_at"]) : null;
        DateTime? verifiedAt = !string.IsNullOrEmpty(item["verified_at"]) ? DateTime.Parse(item["verified_at"]) : null;

        return new OrderRatingChangeRequestItemBuilder()
            .SetId(int.Parse(item["id"]))
            .SetOrderToken(item["order_token"])
            .SetRatingTypeId(int.Parse(item["rating_type_id"]))
            .SetNewRating(char.Parse(item["new_rating"]))
            .SetStatus(item["status"])
            .SetRequestedAt(requestedAt)
            .SetRequestedBy(item["requested_by"])
            .SetVerifiedAt(verifiedAt)
            .SetVerifiedBy(item["verified_by"])
            .Build();
    }


    private static void Create(OrderRatingChangeRequestItem changeRequest, string executeUser)
    {
        Dictionary<string, string> dic = new()
        {
            { "@OrderToken", changeRequest.order_token },
            { "@RatingTypeId", changeRequest.rating_type_id.ToString() },
            { "@NewRating", changeRequest.new_rating.ToString() },
            { "@RequestedAt", DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss") },
            { "@RequestedBy", executeUser }
        };

        string query = "INSERT INTO order_rating_change_request (order_token, rating_type_id, new_rating, requested_at, requested_by) " +
            "VALUES (@OrderToken, @RatingTypeId, @NewRating, @RequestedAt, @RequestedBy)";

        SqlExecuterItem response = SqlExecuter.ExecuteFunction(query, dic, executeUser, true, "CreateRatingChangeRequestItem");

        if (!response.operationResult)
        {
            throw new DatabaseException("Error creating rating change request item");
        }

        Log.Info("Rating change request item created successfully");
    }

    public static void Create(List<OrderRatingChangeRequestItem> changeRequests, string executeUser)
    {
        bool valid = OrderRatingChangeRequestHelper.ValidateChangeRequest(changeRequests, executeUser);
        if (!valid)
        {
            Log.Debug($"A validation for the change requests creation failed");
            return;
        }

        foreach (OrderRatingChangeRequestItem changeRequest in changeRequests)
        {
            Create(changeRequest, executeUser);
        }

        // change the order status
        string orderToken = changeRequests[0].order_token;
        int statusId = StatusConstants.StatusCode.PENDENTE_APROVACAO_ADMINISTRACAO;
        OrderModel.PatchStatus(orderToken, statusId, executeUser);
    }

    private static void PatchStatus(string orderToken, int ratingTypeId, OrderRatingStatus status, string executeUser)
    {
        // get the request by id to check if it exists and is pending
        List<OrderRatingChangeRequestItem> requests = GetByOrderTokenByRequestTypeId(orderToken, ratingTypeId, executeUser);
        if (requests.Count == 0)
        {
            throw new NotFoundException("Rating change request item not found");
        }
        string pendingStatus = OrderRatingStatus.Pending.ToString().ToLowerInvariant();
        if (!requests.Any(r => r.status.Equals(pendingStatus, StringComparison.OrdinalIgnoreCase)))
        {
            throw new InputNotValidException("Rating change request item is not pending");
        }

        // the inteded request is the one that is pending. It should be only one
        OrderRatingChangeRequestItem intendedRequest = requests.First((x) => x.status.Equals(pendingStatus, StringComparison.OrdinalIgnoreCase));

        string statusLogType = status == OrderRatingStatus.Accepted ? "Confirm" : "Reject";

        Dictionary<string, string> dic = new()
        {
            { "@RequestId", intendedRequest.id.ToString() },
            { "@Status", status.ToString().ToLower() },
            { "@VerifiedAt", DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss") },
            { "@VerifiedBy", executeUser }
        };

        string query = "UPDATE order_rating_change_request " +
            "SET status = @Status, verified_at = @VerifiedAt, verified_by = @VerifiedBy " +
            "WHERE id = @RequestId";

        SqlExecuterItem response = SqlExecuter.ExecuteFunction(query, dic, executeUser, true, $"{statusLogType}RatingChangeRequestItem");

        if (!response.operationResult)
        {
            throw new DatabaseException($"Error {statusLogType}ing rating change request item");
        }

        Log.Info($"Rating change request item with token {orderToken} and ratingTypeId {ratingTypeId} {statusLogType}ed successfully");

        // if accepted, update the order rating
        if (status == OrderRatingStatus.Accepted)
        {
            OrderRatingItem orderRatingItem = new OrderRatingItemBuilder()
                .SetOrderToken(orderToken)
                .SetRatingTypeId(ratingTypeId)
                .SetRating(intendedRequest.new_rating)
                .Build();

            OrderRatingModel.PatchOrderRating(orderRatingItem, executeUser);
        }
    }

    public static void PatchStatus(List<UpdateOrderRatingChangeRequestRequest> changeRequests, string orderToken, string executeUser)
    {
        bool valid = OrderRatingChangeRequestHelper.ValidateChangeRequestResponse(changeRequests, orderToken, executeUser);
        if (!valid)
        {
            Log.Debug($"A validation for the change requests status update failed");
            return;
        }

        foreach (UpdateOrderRatingChangeRequestRequest request in changeRequests)
        {
            OrderRatingStatus status = request.is_accepted ? OrderRatingStatus.Accepted : OrderRatingStatus.Rejected;
            PatchStatus(orderToken, request.rating_type_id, status, executeUser);
        }

        // Always recalculate the order products price after the status is updated
        OrderModel.RecalculateOrderProductsPrice(orderToken, executeUser);

        int emailStatus = StatusConstants.StatusCode.APROVADO_DIRECAO_COMERCIAL;
        OrderModel.PatchStatus(orderToken, emailStatus, executeUser);
    }
}
