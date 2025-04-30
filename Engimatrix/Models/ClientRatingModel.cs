// // Copyright (c) 2024 Engibots. All rights reserved.

using System.Diagnostics;
using System.Globalization;
using engimatrix.Config;
using engimatrix.Exceptions;
using engimatrix.ModelObjs;
using engimatrix.ModelObjs.Primavera;
using engimatrix.Utils;
using Smartsheet.Api.Models;
namespace engimatrix.Models;

public static class ClientRatingModel
{
    public static List<ClientRatingItem> GetClientRatingsByClientCode(string client_code, string execute_user)
    {
        Dictionary<string, string> dic = new()
        {
            { "client_code", client_code }
        };

        string query = $"SELECT client_code, rating_type_id, rating FROM mf_client_rating WHERE client_code = @client_code";

        SqlExecuterItem response = SqlExecuter.ExecuteFunction(query, dic, execute_user, false, "getClientRatings");

        List<ClientRatingItem> clientRatings = [];

        if (!response.operationResult)
        {
            throw new Exception("Error getting ratings from mf_client_rating");
        }

        if (response.out_data.Count <= 0)
        {
            throw new InputNotValidException("Error getting record for client ratings");
        }

        foreach (Dictionary<string, string> item in response.out_data)
        {
            ClientRatingItem clientRating = new ClientRatingItemBuilder()
                .SetClientCode(item["client_code"])
                .SetRatingTypeId(Int32.Parse(item["rating_type_id"]))
                .SetRating(Char.Parse(item["rating"]))
                .Build();

            clientRatings.Add(clientRating);
        }

        return clientRatings;
    }

    public static List<ClientRatingItem> GetByOrderToken(string orderToken, string executeUser)
    {
        // This function aims to get the client Ratings, but by order. That means that, if there is any rating change request
        // approved to change the client rating, we must prioritize that rating to the client ratings
        OrderItem? order = OrderModel.GetOrderByToken(orderToken, executeUser) ?? throw new NotFoundException("Order not found for token " + orderToken);

        if (string.IsNullOrEmpty(order.client_code))
        {
            throw new InputNotValidException("Order has no client code");
        }

        List<ClientRatingItem> clientRatings = GetClientRatingsByClientCode(order.client_code, executeUser);

        List<OrderRatingChangeRequestItem> changeRequests = OrderRatingChangeRequestModel.GetByOrderToken(orderToken, executeUser);
        if (changeRequests.Count <= 0)
        {
            return clientRatings;
        }

        List<OrderRatingChangeRequestItem> clientChangeRequests = [.. changeRequests.Where(r => RatingTypes.IsValidClientRatingType(r.rating_type_id))];
        if (clientChangeRequests.Count <= 0)
        {
            return clientRatings;
        }

        List<OrderRatingChangeRequestItem> approvedClientChangeRequests = [.. clientChangeRequests.Where(r => r.status.Equals(OrderRatingStatus.Accepted.ToString(), StringComparison.OrdinalIgnoreCase))];
        if (approvedClientChangeRequests.Count <= 0)
        {
            return clientRatings;
        }

        // if there are more than one approved change request for the same rating type, we must get only the most recent one
        Dictionary<int, OrderRatingChangeRequestItem> approvedClientChangeRequestsByRatingType = [];
        foreach (OrderRatingChangeRequestItem request in approvedClientChangeRequests)
        {
            if (!approvedClientChangeRequestsByRatingType.TryGetValue(request.rating_type_id, out OrderRatingChangeRequestItem? value))
            {
                approvedClientChangeRequestsByRatingType[request.rating_type_id] = request;
                continue;
            }

            if (value.requested_at < request.requested_at)
            {
                // override the older value
                approvedClientChangeRequestsByRatingType[request.rating_type_id] = request;
            }
        }

        approvedClientChangeRequests = [.. approvedClientChangeRequestsByRatingType.Values];

        foreach (OrderRatingChangeRequestItem request in approvedClientChangeRequests)
        {
            ClientRatingItem? clientRating = clientRatings.FirstOrDefault(r => r.rating_type_id == request.rating_type_id);

            if (clientRating == null)
            {
                Log.Debug($"Client rating not found for rating type id {request.rating_type_id}");
                continue;
            }

            clientRatings.Remove(clientRating);

            ClientRatingItem newClientRating = new ClientRatingItemBuilder()
                .SetClientCode(clientRating.client_code)
                .SetRatingTypeId(clientRating.rating_type_id)
                .SetRating(request.new_rating)
                .Build();

            clientRatings.Add(newClientRating);
        }

        clientRatings.Sort((a, b) => a.rating_type_id.CompareTo(b.rating_type_id));

        return clientRatings;
    }

    public static List<ClientRatingItem> GetClientRatingsByType(int typeId, string executeUser)
    {

        Dictionary<string, string> dic = new()
        {
            { "@RatingTypeId", typeId.ToString() }
        };

        string query = $"SELECT client_code, rating_type_id, rating, updated_at, updated_by, created_at, created_by " +
            "FROM mf_client_rating " +
            "WHERE rating_type_id = @RatingTypeId";

        SqlExecuterItem response = SqlExecuter.ExecuteFunction(query, dic, executeUser, false, "getClientRatings");

        List<ClientRatingItem> clientRatings = [];

        if (!response.operationResult)
        {
            throw new Exception("Error getting ratings from mf_client_rating");
        }

        if (response.out_data.Count <= 0)
        {
            throw new InputNotValidException("Error getting record for client ratings");
        }

        foreach (Dictionary<string, string> item in response.out_data)
        {
            DateTime? createdAt = !string.IsNullOrEmpty(item["created_at"]) ? DateTime.Parse(item["created_at"]) : null;
            DateTime? updatedAt = !string.IsNullOrEmpty(item["updated_at"]) ? DateTime.Parse(item["updated_at"]) : null;

            ClientRatingItem clientRating = new ClientRatingItemBuilder()
                .SetClientCode(item["client_code"])
                .SetRatingTypeId(int.Parse(item["rating_type_id"]))
                .SetRating(char.Parse(item["rating"]))
                .SetCreatedBy(item["created_by"])
                .SetCreatedAt(createdAt)
                .SetUpdatedBy(item["updated_by"])
                .SetUpdatedAt(updatedAt)
                .Build();

            clientRatings.Add(clientRating);
        }

        return clientRatings;
    }

    public static Dictionary<string, ClientRatingItem> GetClientRatingsByTypeByClientCodeHashed(int ratingTypeId, string executeUser)
    {
        List<ClientRatingItem> clientRatings = GetClientRatingsByType(ratingTypeId, executeUser);

        if (clientRatings.Count <= 0)
        {
            throw new InputNotValidException($"Error getting record for client ratings with rating type {ratingTypeId}");
        }

        // hash the ratings by client code
        Dictionary<string, ClientRatingItem> creditRatingsByClientCode = [];
        clientRatings.ForEach((clientRating) =>
        {
            creditRatingsByClientCode[clientRating.client_code] = clientRating;
        });

        return creditRatingsByClientCode;
    }

    public static List<ClientRatingItem> GetClientRatings(string execute_user)
    {
        string query = $"SELECT client_code, rating_type_id, rating FROM mf_client_rating";

        SqlExecuterItem response = SqlExecuter.ExecuteFunction(query, [], execute_user, false, "getClientRatings");

        List<ClientRatingItem> clientRatings = [];


        if (!response.operationResult)
        {
            throw new Exception("Error getting ratings from mf_client_rating");
        }

        if (response.out_data.Count <= 0)
        {
            throw new InputNotValidException("Error getting record for client ratings");
        }

        foreach (Dictionary<string, string> item in response.out_data)
        {
            ClientRatingItem clientRating = new ClientRatingItemBuilder()
                .SetClientCode(item["client_code"])
                .SetRatingTypeId(Int32.Parse(item["rating_type_id"]))
                .SetRating(Char.Parse(item["rating"]))
                .Build();

            clientRatings.Add(clientRating);
        }
        return clientRatings;
    }

    public static List<ClientRatingDTO> GetClientRatingsDTO(string execute_user)
    {
        string query = "SELECT cr.client_code AS client_code, rt.id AS rating_type_id, rt.description AS rating_type_description, " +
            "rt.slug AS rating_type_slug, rt.weight AS rating_type_weight, rd.rating AS rating_discount_rating, rd.percentage AS rating_discount_percentage, " +
            "cr.updated_at, cr.updated_by, cr.created_at, cr.created_by " +
            "FROM mf_client_rating cr " +
                "JOIN mf_rating_type rt ON cr.rating_type_id = rt.id " +
                "JOIN mf_rating_discount rd ON rd.rating = cr.rating " +
            "ORDER BY cr.client_code, rt.id;";
        SqlExecuterItem response = SqlExecuter.ExecuteFunction(query, [], execute_user, false, "getClientRatingsDTO");

        List<ClientRatingDTO> clientRatings = [];

        if (response.out_data.Count <= 0)
        {
            throw new InputNotValidException("Error getting record for client ratings");
        }

        foreach (Dictionary<string, string> item in response.out_data)
        {
            RatingTypeItem rating_type = new(Int32.Parse(item["rating_type_id"]), item["rating_type_description"], item["rating_type_slug"], Decimal.Parse(item["rating_type_weight"], CultureInfo.InvariantCulture));
            RatingDiscountItem rating_discount = new(Convert.ToChar(item["rating_discount_rating"]), Decimal.Parse(item["rating_discount_percentage"], CultureInfo.InvariantCulture));
            DateTime createdAt = !string.IsNullOrEmpty(item["created_at"]) ? DateTime.Parse(item["created_at"]) : default;
            DateTime? updatedAt = !string.IsNullOrEmpty(item["updated_at"]) ? DateTime.Parse(item["updated_at"]) : null;
            ClientRatingDTO clientRating = new ClientRatingDTOBuilder()
                .SetClientCode(item["client_code"])
                .SetRatingType(rating_type)
                .SetRatingDiscount(rating_discount)
                .SetCreatedBy(item["created_by"])
                .SetCreatedAt(createdAt)
                .SetUpdatedBy(item["updated_by"])
                .SetUpdatedAt(updatedAt)
                .Build();
            clientRatings.Add(clientRating);
        }

        return clientRatings;
    }

    public static List<ClientRatingDTO> GetClientRatingsByCodeDTO(string client_code, string execute_user)
    {
        Dictionary<string, string> dic = new()
        {
            { "client_code", client_code }
        };

        string query = "SELECT cr.client_code AS client_code, rt.id AS rating_type_id, rt.description AS rating_type_description, " +
            "rt.slug AS rating_type_slug, rt.weight AS rating_type_weight, rd.rating AS rating_discount_rating, rd.percentage AS rating_discount_percentage " +
            "FROM mf_client_rating cr " +
                "JOIN mf_rating_type rt ON cr.rating_type_id = rt.id " +
                "JOIN mf_rating_discount rd ON rd.rating = cr.rating " +
            "WHERE cr.client_code = @client_code " +
            "ORDER BY cr.client_code, rt.id;";

        SqlExecuterItem response = SqlExecuter.ExecuteFunction(query, dic, execute_user, false, "getClientRatingsByCodeDTO");

        List<ClientRatingDTO> clientRatings = [];

        if (response.out_data.Count <= 0)
        {
            throw new InputNotValidException("Error getting record for client ratings");
        }

        foreach (Dictionary<string, string> item in response.out_data)
        {
            RatingTypeItem rating_type = new(Int32.Parse(item["rating_type_id"]), item["rating_type_description"], item["rating_type_slug"], Decimal.Parse(item["rating_type_weight"], CultureInfo.InvariantCulture));
            RatingDiscountItem rating_discount = new(Convert.ToChar(item["rating_discount_rating"]), Decimal.Parse(item["rating_discount_percentage"], CultureInfo.InvariantCulture));
            ClientRatingDTO clientRating = new ClientRatingDTOBuilder().SetClientCode(item["client_code"]).SetRatingType(rating_type).SetRatingDiscount(rating_discount).Build();
            clientRatings.Add(clientRating);
        }

        return clientRatings;
    }

    public static List<ClientRatingDTO> GetClientRatingsByCodeDTO(List<string> clientCodes, string execute_user)
    {
        if (clientCodes.Count <= 0)
        {
            return [];
        }

        Dictionary<string, string> dic = new();

        for (int i = 0; i < clientCodes.Count; i++)
        {
            dic.Add($"@ClientCode{i}", clientCodes[i]);
        }

        string query = "SELECT cr.client_code AS client_code, rt.id AS rating_type_id, rt.description AS rating_type_description, " +
            "rt.slug AS rating_type_slug, rt.weight AS rating_type_weight, rd.rating AS rating_discount_rating, rd.percentage AS rating_discount_percentage " +
            "FROM mf_client_rating cr " +
                "JOIN mf_rating_type rt ON cr.rating_type_id = rt.id " +
                "JOIN mf_rating_discount rd ON rd.rating = cr.rating " +
            $"WHERE cr.client_code IN ({string.Join(",", dic.Keys)}) " +
            "ORDER BY cr.client_code, rt.id;";

        SqlExecuterItem response = SqlExecuter.ExecuteFunction(query, dic, execute_user, false, "getClientRatingsByCodeDTO");

        List<ClientRatingDTO> clientRatings = [];

        if (response.out_data.Count <= 0)
        {
            throw new InputNotValidException("Error getting record for client ratings");
        }

        foreach (Dictionary<string, string> item in response.out_data)
        {
            RatingTypeItem rating_type = new(Int32.Parse(item["rating_type_id"]), item["rating_type_description"], item["rating_type_slug"], Decimal.Parse(item["rating_type_weight"], CultureInfo.InvariantCulture));
            RatingDiscountItem rating_discount = new(Convert.ToChar(item["rating_discount_rating"]), Decimal.Parse(item["rating_discount_percentage"], CultureInfo.InvariantCulture));
            ClientRatingDTO clientRating = new ClientRatingDTOBuilder().SetClientCode(item["client_code"]).SetRatingType(rating_type).SetRatingDiscount(rating_discount).Build();
            clientRatings.Add(clientRating);
        }

        return clientRatings;
    }

    public static List<ClientRatingDTO> GetClientRatingsDTOByCodeByOrderToken(string clientCode, string orderToken, string executeUser)
    {
        List<ClientRatingDTO> clientRatings = GetClientRatingsByCodeDTO(clientCode, executeUser);

        if (clientRatings.Count <= 0)
        {
            throw new InputNotValidException("Error getting record for client ratings");
        }

        List<OrderRatingChangeRequestItem> changeRequests = OrderRatingChangeRequestModel.GetByOrderToken(orderToken, executeUser);
        if (changeRequests.Count <= 0)
        {
            return clientRatings;
        }

        List<OrderRatingChangeRequestItem> clientChangeRequests = [.. changeRequests.Where(r => RatingTypes.IsValidClientRatingType(r.rating_type_id))];
        if (clientChangeRequests.Count <= 0)
        {
            return clientRatings;
        }

        List<OrderRatingChangeRequestItem> approvedClientChangeRequests = [.. clientChangeRequests.Where(r => r.status.Equals(OrderRatingStatus.Accepted.ToString(), StringComparison.OrdinalIgnoreCase))];
        if (approvedClientChangeRequests.Count <= 0)
        {
            return clientRatings;
        }

        // for every rating type, if there is more than 1 approved, we need to take into account ONLY the most recent one
        Dictionary<int, OrderRatingChangeRequestItem> approvedClientChangeRequestsByRatingType = [];
        foreach (OrderRatingChangeRequestItem request in approvedClientChangeRequests)
        {
            if (!approvedClientChangeRequestsByRatingType.TryGetValue(request.rating_type_id, out OrderRatingChangeRequestItem? value))
            {
                approvedClientChangeRequestsByRatingType[request.rating_type_id] = request;
                continue;
            }

            if (value.requested_at < request.requested_at)
            {
                approvedClientChangeRequestsByRatingType[request.rating_type_id] = request;
            }
        }

        approvedClientChangeRequests = [.. approvedClientChangeRequestsByRatingType.Values];

        List<RatingDiscountItem> ratingDiscounts = RatingDiscountModel.GetRatingDiscounts(executeUser);

        foreach (OrderRatingChangeRequestItem request in approvedClientChangeRequests)
        {
            ClientRatingDTO? clientRating = clientRatings.FirstOrDefault(r => r.rating_type.id == request.rating_type_id);
            if (clientRating == null)
            {
                Log.Debug($"Client rating not found for rating type id {request.rating_type_id} and client code {clientCode}");
                continue;
            }

            RatingDiscountItem? overrideRatingDiscount = ratingDiscounts.FirstOrDefault(r => r.rating == request.new_rating);
            if (overrideRatingDiscount == null)
            {
                Log.Debug($"Rating discount not found for rating {request.new_rating}");
                continue;
            }

            clientRatings.Remove(clientRating);
            ClientRatingDTO newClientRating = new ClientRatingDTOBuilder().SetClientCode(clientRating.client_code).SetRatingType(clientRating.rating_type).SetRatingDiscount(overrideRatingDiscount).Build();
            clientRatings.Add(newClientRating);
        }

        clientRatings.Sort((a, b) => a.rating_type.id.CompareTo(b.rating_type.id));

        return clientRatings;
    }

    public static void PatchClientRating(ClientRatingItem clientRating, string execute_user)
    {
        Dictionary<string, string> dic = new()
        {
            { "@client_code", clientRating.client_code },
            { "@rating_type_id", clientRating.rating_type_id.ToString() },
            { "@rating", clientRating.rating.ToString() },
            { "@updated_by", execute_user },
            { "@updated_at", DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss") }

        };

        string query = "UPDATE mf_client_rating " +
            "SET rating = @rating, updated_at = @updated_at, updated_by = @updated_by " +
            "WHERE client_code = @client_code AND rating_type_id = @rating_type_id";

        SqlExecuterItem response = SqlExecuter.ExecuteFunction(query, dic, execute_user, true, "patchClientRating");

        if (!response.operationResult)
        {
            throw new Exception($"Error updating client rating with {clientRating.client_code} and rating type {clientRating.rating_type_id}, with rating {clientRating.rating}");
        }
    }

    public static decimal CalculateWeightedClientRatingByDTO(List<ClientRatingDTO> clientRatings)
    {
        decimal rating = 0;
        clientRatings.ForEach((clientRating) =>
        {
            rating += clientRating.rating_discount.percentage * clientRating.rating_type.weight;
        });
        return rating;
    }

    public static decimal CalculateWeightedClientRating(string clientCode, string executeUser)
    {
        List<RatingDiscountItem> ratingDiscounts = RatingDiscountModel.GetRatingDiscounts(executeUser);
        List<RatingTypeItem> ratingTypes = RatingTypeModel.GetClientRatingTypes(executeUser);
        List<ClientRatingItem> clientRatings = GetClientRatingsByClientCode(clientCode, executeUser);

        decimal rating = OrderRatingModel.CalculateWeightedRating(clientRatings, ratingTypes, ratingDiscounts);
        return rating;
    }

}
