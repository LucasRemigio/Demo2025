// // Copyright (c) 2024 Engibots. All rights reserved.

using System.Diagnostics;
using System.Globalization;
using engimatrix.Config;
using engimatrix.Connector;
using engimatrix.Exceptions;
using engimatrix.ModelObjs;
using engimatrix.ModelObjs.Primavera;
using engimatrix.Utils;
using Engimatrix.ModelObjs;
namespace engimatrix.Models;

public static class OrderRatingModel
{
    public static List<OrderRatingItem> GetOrderRatings(string execute_user)
    {
        string query = $"SELECT order_token, rating_type_id, rating FROM order_rating";

        SqlExecuterItem response = SqlExecuter.ExecuteFunction(query, [], execute_user, false, "getClientRatings");

        if (!response.operationResult)
        {
            throw new Exception("Error getting ratings from mf_client_rating");
        }

        if (response.out_data.Count <= 0)
        {
            throw new InputNotValidException("Error getting record for client ratings");
        }

        List<OrderRatingItem> orderRatings = [];
        foreach (Dictionary<string, string> item in response.out_data)
        {
            OrderRatingItem orderRating = new OrderRatingItemBuilder()
                .SetOrderToken(item["order_token"])
                .SetRatingTypeId(int.Parse(item["rating_type_id"]))
                .SetRating(char.Parse(item["rating"]))
                .Build();

            orderRatings.Add(orderRating);
        }

        return orderRatings;
    }

    public static List<OrderRatingItem> GetOrderRatingsByOrderToken(string orderToken, string executeUser)
    {
        if (string.IsNullOrEmpty(orderToken))
        {
            throw new ArgumentNullException(nameof(orderToken), "Order token cannot be null or empty");
        }

        Dictionary<string, string> dic = new()
        {
            { "@OrderToken", orderToken }
        };

        string query = "SELECT order_token, rating_type_id, rating FROM order_rating WHERE order_token = @OrderToken";

        SqlExecuterItem response = SqlExecuter.ExecuteFunction(query, dic, executeUser, false, "getOrderRatingsByOrderToken");

        if (!response.operationResult)
        {
            throw new Exception($"Error getting ratings for order with token {orderToken}");
        }

        if (response.out_data.Count <= 0)
        {
            throw new InputNotValidException($"No ratings found for the order with token {orderToken}");
        }

        List<OrderRatingItem> orderRatings = [];
        foreach (Dictionary<string, string> item in response.out_data)
        {
            OrderRatingItem orderRating = new OrderRatingItemBuilder()
                .SetOrderToken(item["order_token"])
                .SetRatingTypeId(int.Parse(item["rating_type_id"]))
                .SetRating(char.Parse(item["rating"]))
                .Build();

            orderRatings.Add(orderRating);
        }
        return orderRatings;
    }

    public static OrderRatingItem? GetByOrderTokenByRatingTypeId(string orderToken, int ratingTypeId, string executeUser)
    {
        if (string.IsNullOrEmpty(orderToken))
        {
            throw new ArgumentNullException(nameof(orderToken), "Order token cannot be null or empty");
        }

        Dictionary<string, string> dic = new()
        {
            { "@OrderToken", orderToken },
            { "@RatingTypeId", ratingTypeId.ToString() }
        };

        string query = "SELECT order_token, rating_type_id, rating " +
            "FROM order_rating " +
            "WHERE order_token = @OrderToken AND rating_type_id = @RatingTypeId";

        SqlExecuterItem response = SqlExecuter.ExecuteFunction(query, dic, executeUser, true, "getOrderRatingByOrderTokenByRatingTypeId");

        if (!response.operationResult)
        {
            throw new Exception($"Error getting rating for order with token {orderToken} and rating type {ratingTypeId}");
        }

        if (response.out_data.Count <= 0)
        {
            return null;
        }

        Dictionary<string, string> item = response.out_data[0];
        OrderRatingItem orderRating = new OrderRatingItemBuilder()
            .SetOrderToken(item["order_token"])
            .SetRatingTypeId(int.Parse(item["rating_type_id"]))
            .SetRating(char.Parse(item["rating"]))
            .Build();

        return orderRating;
    }

    public static List<OrderRatingDTO> GetOrderRatingsByOrderTokenDTO(string orderToken, string execute_user)
    {
        Dictionary<string, string> dic = new()
        {
            { "@order_token", orderToken }
        };

        string query = "SELECT orr.order_token AS order_token, rt.id AS rating_type_id, rt.description AS rating_type_description, " +
            "rt.slug AS rating_type_slug, rt.weight AS rating_type_weight, rd.rating AS rating_discount_rating, rd.percentage AS rating_discount_percentage " +
            "FROM order_rating orr " +
                "JOIN mf_rating_type rt ON orr.rating_type_id = rt.id " +
                "JOIN mf_rating_discount rd ON rd.rating = orr.rating " +
            "WHERE orr.order_token = @order_token " +
            "ORDER BY orr.order_token, rt.id;";

        SqlExecuterItem response = SqlExecuter.ExecuteFunction(query, dic, execute_user, false, "GetOrderRatingsByOrderToken");

        if (response.out_data.Count <= 0)
        {
            throw new InputNotValidException("Error getting record for client ratings");
        }

        List<OrderRatingDTO> ratings = [];
        foreach (Dictionary<string, string> item in response.out_data)
        {
            RatingTypeItem rating_type = new(int.Parse(item["rating_type_id"]), item["rating_type_description"], item["rating_type_slug"], decimal.Parse(item["rating_type_weight"], CultureInfo.InvariantCulture));
            RatingDiscountItem rating_discount = new(Convert.ToChar(item["rating_discount_rating"]), decimal.Parse(item["rating_discount_percentage"], CultureInfo.InvariantCulture));
            OrderRatingDTO rating = new OrderRatingDtoBuilder().SetOrderToken(item["order_token"]).SetRatingType(rating_type).SetRatingDiscount(rating_discount).Build();
            ratings.Add(rating);
        }

        return ratings;
    }

    public static void PatchOrderRating(OrderRatingItem orderRating, string execute_user)
    {
        Dictionary<string, string> dic = new()
        {
            { "@order_token", orderRating.order_token },
            { "@rating_type_id", orderRating.rating_type_id.ToString() },
            { "@rating", orderRating.rating.ToString() },
            { "@updated_by", execute_user },
            { "@updated_at", DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss") }
        };

        string query = "UPDATE order_rating " +
            "SET rating = @rating, updated_at = @updated_at, updated_by = @updated_by " +
            "WHERE order_token = @order_token AND rating_type_id = @rating_type_id";

        SqlExecuterItem response = SqlExecuter.ExecuteFunction(query, dic, execute_user, true, "patchClientRating");

        if (!response.operationResult)
        {
            throw new Exception($"Error updating client rating with {orderRating.order_token} and rating type {orderRating.rating_type_id}, with rating {orderRating.rating}");
        }
    }

    public static void CreateDefaultRatings(string orderToken, string executeUser)
    {
        if (string.IsNullOrEmpty(orderToken))
        {
            throw new ArgumentNullException(nameof(orderToken), "Order token cannot be null or empty");
        }

        // This is so that, if we create more ratings, the next statement updates automatically
        List<RatingTypeItem> ratingTypes = RatingTypeModel.GetOrderRatingTypes(executeUser);

        // TODO: Hardcoded default rating
        char defaultRating = 'D';
        Dictionary<string, string> dic = new()
        {
            { "@OrderToken", orderToken },
            { "@DefaultRating", defaultRating.ToString() },
            { "@CreatedAt", DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss") },
            { "@CreatedBy", executeUser }
        };

        string insertQuery = $"INSERT INTO order_rating (order_token, rating_type_id, rating, created_at, created_by) VALUES ";

        List<string> valueList = [];
        foreach (RatingTypeItem ratingType in ratingTypes)
        {
            valueList.Add($"(@OrderToken, {ratingType.id}, @DefaultRating, @CreatedAt, @CreatedBy)");
        }

        // Join the values with commas and append to the insert query
        insertQuery += string.Join(", ", valueList);

        SqlExecuterItem response = SqlExecuter.ExecuteFunction(insertQuery, dic, executeUser, false, "InsertDefaultRatings");

        if (!response.operationResult)
        {
            throw new Exception($"Error creating default ratings for order with token {orderToken}");
        }

        Log.Debug($"Default ratings for order with token {orderToken} created successfully.");
    }

    public static decimal GetOrderWeightedRating(string orderToken, string executeUser)
    {
        OrderItem? order = OrderModel.GetOrderByToken(orderToken, executeUser);
        if (order == null)
        {
            throw new InputNotValidException($"Order with token {orderToken} not found");
        }

        decimal weightedRating = GetOrderWeightedRating(order, executeUser);
        return weightedRating;
    }

    public static decimal GetOrderWeightedRating(OrderItem order, string executeUser)
    {
        // Get the ratings types and discounts
        decimal totalWeightedRating = 0;
        List<RatingDiscountItem> ratingDiscounts = RatingDiscountModel.GetRatingDiscounts(executeUser);
        List<RatingTypeItem> ratingTypes = RatingTypeModel.GetRatingTypes(executeUser);

        // order ratings
        List<OrderRatingItem> orderRatings = GetOrderRatingsByOrderToken(order.token, executeUser);
        decimal orderWeightedRating = CalculateWeightedRating(orderRatings, ratingTypes, ratingDiscounts);
        totalWeightedRating += orderWeightedRating;

        // check if there is a client on the order
        if (string.IsNullOrEmpty(order.client_code))
        {
            // no client, return early
            return totalWeightedRating;
        }

        // calculate the client ratings if client exists
        List<ClientRatingItem> clientRatings = ClientRatingModel.GetByOrderToken(order.token, executeUser);
        decimal clientWeightedRating = CalculateWeightedRating(clientRatings, ratingTypes, ratingDiscounts);
        totalWeightedRating += clientWeightedRating;

        return totalWeightedRating;
    }

    public static decimal CalculateWeightedRating(IEnumerable<IRatingItem> ratings, List<RatingTypeItem> ratingTypes, List<RatingDiscountItem> ratingDiscounts)
    {
        decimal weightedRating = 0;

        Dictionary<int, RatingTypeItem> ratingTypesById = ratingTypes.ToDictionary(x => x.id, x => x);
        Dictionary<char, RatingDiscountItem> ratingDiscountsByRating = ratingDiscounts.ToDictionary(x => x.rating, x => x);

        foreach (IRatingItem rating in ratings)
        {
            if (!ratingTypesById.TryGetValue(rating.rating_type_id, out RatingTypeItem? ratingType))
            {
                Log.Debug("CalculateWeightedRating - Rating type not found");
                continue;
            }

            if (!ratingDiscountsByRating.TryGetValue(rating.rating, out RatingDiscountItem? ratingDiscount))
            {
                Log.Debug("CalculateWeightedRating - Rating discount not found");
                continue;
            }

            weightedRating += ratingDiscount.percentage * ratingType.weight;
        }

        return weightedRating;
    }

    /* ================================================================================================================
    * Rating: Operational Cost
    * ================================================================================================================ */

    public static OrderRatingItem PatchOperationalCostRating(string orderToken, string executeUser)
    {
        bool orderExists = OrderModel.OrderExists(orderToken, executeUser);
        if (!orderExists)
        {
            throw new InputNotValidException($"Order with token {orderToken} not found");
        }

        int ratingType = (int)RatingTypes.RatingType.OperationalCost;

        List<OrderRatingChangeRequestItem> ratingReqs =
            OrderRatingChangeRequestModel.GetByOrderTokenByRequestTypeId(orderToken, ratingType, executeUser);

        string acceptedStatus = OrderRatingStatus.Accepted.ToString().ToLower().Trim();
        if (ratingReqs.Count > 0 && ratingReqs.Any(x => x.status.Equals(acceptedStatus, StringComparison.OrdinalIgnoreCase)))
        {
            Log.Debug($"Operational cost rating already approved for order {orderToken}");
            return GetByOrderTokenByRatingTypeId(orderToken, ratingType, executeUser)!;
        }

        char rating = CalculateOrderOperationalCostRating(orderToken, executeUser);

        OrderRatingItem orderRating = new OrderRatingItemBuilder()
            .SetOrderToken(orderToken)
            .SetRatingTypeId(ratingType)
            .SetRating(rating)
            .Build();

        PatchOrderRating(orderRating, executeUser);

        return orderRating;
    }

    public static char CalculateOrderOperationalCostRating(string orderToken, string executeUser)
    {
        /*
            até 500€ = D
            501 a 1499€ = C
            1500€ a 2500€ = B
            2500€ = A
        */

        OrderTotalItem orderTotal = OrderModel.CalculateOrderTotal(orderToken, executeUser);

        // TODO: Which one to use?
        decimal total = orderTotal.total;

        char rating = total switch
        {
            <= 500 => 'D',
            <= 1499 => 'C',
            <= 2500 => 'B',
            _ => 'A',
        };

        Log.Debug($"Calculated operational cost rating for order {orderToken} with total {total}: {rating}");

        return rating;
    }


    /* ================================================================================================================
    * Rating: Logistic
    * ================================================================================================================ */

    public async static Task<(DestinationDetailsItem, OrderRatingItem)> PatchLogisticRating(string orderToken, string executeUser)
    {
        OrderItem? orderItem = OrderModel.GetOrderByToken(orderToken, executeUser);
        if (orderItem == null)
        {
            throw new NotFoundException("Order not found with order token " + orderToken);
        }

        int ratingType = (int)RatingTypes.RatingType.Logistic;

        List<OrderRatingChangeRequestItem> ratingReqs =
            OrderRatingChangeRequestModel.GetByOrderTokenByRequestTypeId(orderToken, ratingType, executeUser);

        DestinationDetailsItem details = new();
        string acceptedStatus = OrderRatingStatus.Accepted.ToString().ToLower().Trim();
        if (ratingReqs.Count > 0 && ratingReqs.Any(x => x.status.Equals(acceptedStatus, StringComparison.OrdinalIgnoreCase)))
        {
            Log.Debug($"Logistic rating already approved for order {orderToken}");

            details = await GetHereDestinationDetails(orderItem, executeUser);
            OrderRatingItem currentRating = GetByOrderTokenByRatingTypeId(orderToken, ratingType, executeUser)!;

            return (details, currentRating);
        }

        char rating;
        if (orderItem.is_delivery)
        {
            details = await GetHereDestinationDetails(orderItem, executeUser);
            rating = CalculateLogisticRating(details.distance);
        }
        else
        {
            rating = 'A';
        }

        OrderRatingItem orderRating = new OrderRatingItemBuilder()
            .SetOrderToken(orderToken)
            .SetRatingTypeId(ratingType)
            .SetRating(rating)
            .Build();

        PatchOrderRating(orderRating, executeUser);

        return (details, orderRating);
    }

    private async static Task<DestinationDetailsItem> GetHereDestinationDetails(OrderItem order, string executeUser)
    {
        string orderAddress = $"{order.address}, {order.postal_code} {order.locality}, {order.district}, {order.city}, Portugal";
        HereRoutesItemResponse routes = await HereCalculateDistanceFromMasterFerro(orderAddress);

        int distanceMts = routes.Routes[0].Sections[0].Summary.Length;
        int duration = routes.Routes[0].Sections[0].Summary.Duration;
        string coordinates = OrderModel.GetDestinationCoordinates(routes);

        OrderModel.UpdateOrderHereMapsAddress(order.token, routes, executeUser);

        DestinationDetailsItem details = new(coordinates, distanceMts, duration);
        return details;
    }

    public async static Task<HereRoutesItemResponse> HereCalculateDistanceFromMasterFerro(string address)
    {
        // first step is to geocode the address with the geocode function
        // then we get the coordinates and call the route function
        string coordinates = await HereGetGeocodedAddress(address);

        // next is to get the distance by calculating the route with the route function
        HereRoutesItemResponse routes = await HereCalculateRoute(coordinates);
        return routes;
    }

    public async static Task<string> HereGetGeocodedAddress(string address)
    {
        HereGeocodeItemResponse? geocoded = await HereApi.Geocode(address);
        if (geocoded == null)
        {
            throw new Exception("Error geocoding address");
        }

        if (geocoded.Items.Count <= 0)
        {
            throw new Exception("No results found for geocoding address");
        }

        GeocodeItem item = geocoded.Items[0];
        string coordinates = $"{item.Position.Lat},{item.Position.Lng}";
        Log.Debug($"Geocoded address {address} to coordinates {coordinates}");
        return coordinates;
    }

    public async static Task<HereRoutesItemResponse> HereCalculateRoute(string destination)
    {
        HereRoutesItemResponse? routeResponse = await HereApi.RouteFromMasterFerro(destination);
        if (routeResponse == null)
        {
            throw new Exception("Error calculating route from MasterFerro");
        }

        if (routeResponse.Routes.Count <= 0)
        {
            throw new Exception("No results found for route calculation");
        }

        return routeResponse;
    }

    public static char CalculateLogisticRating(int distanceMts)
    {
        /*
        Ratings Logística:
            + 100km = D
            60 a 99 km = C
            30 a 59 = B
            até 29 = A
        */
        int kmsToMts = 1000;
        int ratingABound = 29 * kmsToMts;
        int ratingBBound = 59 * kmsToMts;
        int ratingCBound = 99 * kmsToMts;

        char rating = 'D';
        switch (distanceMts)
        {
            case int distance when distance <= ratingABound:
                rating = 'A';
                break;
            case int distance when distance <= ratingBBound:
                rating = 'B';
                break;
            case int distance when distance <= ratingCBound:
                rating = 'C';
                break;
            default:
                rating = 'D';
                break;
        }

        return rating;
    }
}
