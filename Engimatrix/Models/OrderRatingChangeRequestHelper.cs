// // Copyright (c) 2024 Engibots. All rights reserved.

using engimatrix.Config;
using engimatrix.Exceptions;
using engimatrix.ModelObjs;
using engimatrix.Utils;
using engimatrix.Views;
namespace engimatrix.Models;

public static class OrderRatingChangeRequestHelper
{
    /* ===========================================
    * Validate the creation of the change requests
    * ========================================= */

    public static bool ValidateOrderToken(List<OrderRatingChangeRequestItem> changeRequests, string executeUser)
    {
        // check if all order tokens are the same and the order exists
        if (changeRequests == null || changeRequests.Count == 0)
        {
            throw new InputNotValidException("No change requests provided");
        }

        // validate the tokens
        foreach (OrderRatingChangeRequestItem request in changeRequests)
        {
            if (!Util.IsValidInputString(request.order_token))
            {
                throw new InputNotValidException($"Invalid order token {request.order_token}");
            }
        }

        // Use a HashSet to track unique order tokens
        HashSet<string> orderTokens = [];

        foreach (OrderRatingChangeRequestItem request in changeRequests)
        {
            orderTokens.Add(request.order_token);
        }

        // If there's more than one unique order token, validation fails
        // And hash set only stores unique records, so if there is a count higher than 1,
        // it means that there are multiple order tokens
        if (orderTokens.Count > 1)
        {
            throw new InputNotValidException("All change requests must be for the same order");
        }

        // Check if the order exists
        string orderToken = orderTokens.First();
        bool orderExists = OrderModel.OrderExists(orderToken, executeUser);

        if (!orderExists)
        {
            throw new NotFoundException($"Order with token {orderToken} not found");
        }

        return true;
    }

    public static bool ValidateOrderRatings(List<OrderRatingChangeRequestItem> changeRequests, string executeUser)
    {
        // check if the change requests are different than the current ratings
        string orderToken = changeRequests[0].order_token;
        List<OrderRatingItem> currentRatings = OrderRatingModel.GetOrderRatingsByOrderToken(orderToken, executeUser);

        foreach (OrderRatingChangeRequestItem request in changeRequests)
        {
            OrderRatingItem? currentRating = currentRatings.FirstOrDefault(r => r.rating_type_id == request.rating_type_id) ?? throw new NotFoundException($"Rating not found for order token {orderToken} and rating type id {request.rating_type_id}");

            // Check if the new rating is different than the current rating
            if (currentRating.rating == request.new_rating)
            {
                throw new InputNotValidException($"Rating for order token {orderToken} and rating type id {request.rating_type_id} is already the same as the new rating");
            }
        }

        return true;
    }

    public static bool ValidateClientRatings(List<OrderRatingChangeRequestItem> changeRequests, string executeUser)
    {
        string orderToken = changeRequests[0].order_token;

        // Check the current ratings of the client, having in mind the current order
        List<ClientRatingItem> clientRatings = ClientRatingModel.GetByOrderToken(orderToken, executeUser);

        foreach (OrderRatingChangeRequestItem request in changeRequests)
        {
            ClientRatingItem? clientRating = clientRatings.FirstOrDefault(r => r.rating_type_id == request.rating_type_id) ?? throw new NotFoundException($"Client rating not found for rating type id {request.rating_type_id}");

            // All ratings must be different than the ones that are already set up
            if (clientRating.rating == request.new_rating)
            {
                throw new InputNotValidException($"Rating for order token {orderToken} and rating type id {request.rating_type_id} is already the same as the new rating");
            }
        }

        return true;
    }

    public static bool ValidateChangeRequest(List<OrderRatingChangeRequestItem> changeRequests, string executeUser)
    {
        // check if all order tokens are the same and the order exists
        // check if there is already any pending request
        // we must check if all rating types are different
        // we must check if all ratings are valid
        // we must check if all ratings are different than the current rating
        if (!ValidateOrderToken(changeRequests, executeUser))
        {
            return false;
        }

        // validate if all ratings are valid
        foreach (OrderRatingChangeRequestItem request in changeRequests)
        {
            if (!RatingTypes.IsValidRatingType(request.rating_type_id))
            {
                throw new InputNotValidException($"Invalid rating type id {request.rating_type_id}");
            }

            if (!RatingConstants.IsValidRating(request.new_rating))
            {
                throw new InputNotValidException($"Invalid new rating {request.new_rating}");
            }
        }

        // check if already is any pending request
        List<OrderRatingChangeRequestItem> orderCurrentChangeRequests = OrderRatingChangeRequestModel.GetByOrderToken(changeRequests[0].order_token, executeUser);
        if (orderCurrentChangeRequests.Count > 0)
        {
            if (orderCurrentChangeRequests.Any(r => r.status.Equals(OrderRatingStatus.Pending.ToString(), StringComparison.OrdinalIgnoreCase)))
            {
                throw new ItemAlreadyExistsException("There is already a pending request for this order");
            }
        }

        // check if all rating types are different
        HashSet<int> ratingTypes = [];
        foreach (OrderRatingChangeRequestItem request in changeRequests)
        {
            ratingTypes.Add(request.rating_type_id);
        }

        Log.Debug($"Rating types: {string.Join(", ", ratingTypes)}");
        Log.Debug($"Change requests: {string.Join(", ", changeRequests.Select(r => r.rating_type_id))}");

        if (ratingTypes.Count != changeRequests.Count)
        {
            throw new InputNotValidException("All change requests must have different rating types");
        }

        // check if all ratings are different than the current ratings
        // first by checking the order ratings
        List<OrderRatingChangeRequestItem> orderChangeRequests = [.. changeRequests.Where(r => RatingTypes.IsValidOrderRatingType(r.rating_type_id))];
        if (orderChangeRequests.Count > 0)
        {
            if (!ValidateOrderRatings(orderChangeRequests, executeUser))
            {
                throw new InputNotValidException("All order change requests must have different rating types");
            }
        }

        // then by checking the client ratings
        List<OrderRatingChangeRequestItem> clientChangeRequests = [.. changeRequests.Where(r => RatingTypes.IsValidClientRatingType(r.rating_type_id))];
        if (clientChangeRequests.Count > 0)
        {
            if (!ValidateClientRatings(clientChangeRequests, executeUser))
            {
                throw new InputNotValidException("All client change requests must have different rating types");
            }
        }

        return true;
    }

    /* ===========================================
    * Validate the acceptance or rejection of the pending change requests
    * ========================================= */

    public static bool ValidateChangeRequestResponse(List<UpdateOrderRatingChangeRequestRequest> changeRequests, string orderToken, string executeUser)
    {
        // this function accepts a list of the answers to the pending change requests, with an order token, and then the list
        // has the rating_type_id and the is_accepted boolean

        // first check if even the order exists and has any pending requests
        // we must validate if the amount of requests is the same as the amount of pending requests
        // also check if all the rating types are different and valid
        // then check if every rating type has a match on the pendings list and no pending rating is missing

        bool orderExists = OrderModel.OrderExists(orderToken, executeUser);
        if (!orderExists)
        {
            throw new NotFoundException("Order not found for token " + orderToken);
        }

        List<OrderRatingChangeRequestItem> currentOrderRatingRequests = OrderRatingChangeRequestModel.GetByOrderToken(orderToken, executeUser);
        if (currentOrderRatingRequests.Count == 0)
        {
            throw new NotFoundException("No rating change requests found for order token " + orderToken);
        }

        string pendingStatus = OrderRatingStatus.Pending.ToString();

        List<OrderRatingChangeRequestItem> pendingOrderRatingRequests = [.. currentOrderRatingRequests.Where(r => r.status.Equals(pendingStatus, StringComparison.OrdinalIgnoreCase))];
        if (pendingOrderRatingRequests.Count == 0)
        {
            throw new NotFoundException("No pending rating change requests found for order token " + orderToken);
        }

        // check if the amount of requests is the same as the amount of pending requests
        if (changeRequests.Count != pendingOrderRatingRequests.Count)
        {
            throw new InputNotValidException("The amount of requests is not the same as the amount of pending requests");
        }

        // check if all the rating types are different and valid
        HashSet<int> ratingTypes = [];
        foreach (UpdateOrderRatingChangeRequestRequest request in changeRequests)
        {
            ratingTypes.Add(request.rating_type_id);
        }

        // by validating if the rating types count is different than the original change request counts
        // we are automatically also validating if the sended change requests count is the same
        // as the pending requests count
        if (ratingTypes.Count != changeRequests.Count)
        {
            throw new InputNotValidException("All change requests must have different rating types");
        }

        // this aims to check if given rating type id is the one matching with the pending request
        // given the number of inputs is the same as the pending request count, and if one is incorrect, it must not match and throw an error here
        foreach (UpdateOrderRatingChangeRequestRequest request in changeRequests)
        {
            OrderRatingChangeRequestItem? pendingRequest = pendingOrderRatingRequests.FirstOrDefault(r => r.rating_type_id == request.rating_type_id) ?? throw new NotFoundException($"Pending request not found for rating type id {request.rating_type_id}");

            if (!pendingRequest.status.Equals(pendingStatus, StringComparison.OrdinalIgnoreCase))
            {
                throw new InputNotValidException($"Pending request for rating type id {request.rating_type_id} is not pending");
            }

        }

        return true;
    }
}
