// // Copyright (c) 2024 Engibots. All rights reserved.

using engimatrix.Config;
using engimatrix.Filters;
using engimatrix.ResponseMessages;
using engimatrix.Views;
using Microsoft.AspNetCore.Mvc;
using engimatrix.Models;
using engimatrix.Utils;
using engimatrix.Exceptions;
using engimatrix.ModelObjs;

namespace Engimatrix.Controllers;
[ApiController]
[Route("api/orders/ratings/change-requests")]
public class OrderRatingChangeRequestController : ControllerBase
{
    [HttpPost]
    [Route("{order_token}")]
    [RequestLimit]
    [ValidateReferrer]
    [Authorize]
    public ActionResult<GenericResponse> Create(List<CreateOrderRatingChangeRequestRequest> rating_requests, string order_token)
    {
        string? language = this.Request.Headers["client-lang"];
        if (string.IsNullOrEmpty(language))
        {
            language = ConfigManager.defaultLanguage;
        }

        foreach (CreateOrderRatingChangeRequestRequest request in rating_requests)
        {
            if (!request.IsValid())
            {
                return new GenericResponse(ResponseErrorMessage.InvalidArgs, language);
            }
        }

        string token = this.Request.Headers["Authorization"];
        string executer_user = UserModel.GetUserByToken(token);

        try
        {
            List<OrderRatingChangeRequestItem> ratingRequests = [];
            foreach (CreateOrderRatingChangeRequestRequest request in rating_requests)
            {
                OrderRatingChangeRequestItem orderRatingItem = new OrderRatingChangeRequestItemBuilder()
                    .SetOrderToken(order_token)
                    .SetRatingTypeId(request.rating_type_id)
                    .SetNewRating(request.new_rating)
                    .Build();

                ratingRequests.Add(orderRatingItem);
            }

            OrderRatingChangeRequestModel.Create(ratingRequests, executer_user);
            return new GenericResponse(ResponseSuccessMessage.Success, language);
        }
        catch (InputNotValidException e)
        {
            Log.Error("CreateOrderRatingChangeRequest endpoint - Input not valid - " + e);
            return new GenericResponse(ResponseErrorMessage.InvalidArgs, language);
        }
        catch (NotFoundException e)
        {
            Log.Error("CreateOrderRatingChangeRequest endpoint - Not found - " + e);
            return new GenericResponse(ResponseErrorMessage.NotFound, language);
        }
        catch (ItemAlreadyExistsException e)
        {
            Log.Error("CreateOrderRatingChangeRequest endpoint - Item already exists - " + e);
            return new GenericResponse(ResponseErrorMessage.ItemAlreadyExists, language);
        }
        catch (DatabaseException e)
        {
            Log.Error("CreateOrderRatingChangeRequest endpoint - Database Error - " + e);
            return new GenericResponse(ResponseErrorMessage.DatabaseQueryError, language);
        }
        catch (Exception e)
        {
            Log.Error("CreateOrderRatingChangeRequest endpoint - Error - " + e);
            return new GenericResponse(ResponseErrorMessage.InternalError, language);
        }
    }

    [HttpPatch]
    [Route("{order_token}")]
    [RequestLimit]
    [ValidateReferrer]
    [Authorize]
    public ActionResult<GenericResponse> Update(List<UpdateOrderRatingChangeRequestRequest> rating_requests, string order_token)
    {
        string? language = this.Request.Headers["client-lang"];
        if (string.IsNullOrEmpty(language))
        {
            language = ConfigManager.defaultLanguage;
        }

        foreach (UpdateOrderRatingChangeRequestRequest request in rating_requests)
        {
            if (!request.IsValid())
            {
                return new GenericResponse(ResponseErrorMessage.InvalidArgs, language);
            }
        }

        string token = this.Request.Headers["Authorization"];
        string executer_user = UserModel.GetUserByToken(token);

        if (!UserModel.IsUserAdminOrSupervisor(executer_user))
        {
            return new GenericResponse(ResponseErrorMessage.Unauthorized, language);
        }

        try
        {
            OrderRatingChangeRequestModel.PatchStatus(rating_requests, order_token, executer_user);

            return new GenericResponse(ResponseSuccessMessage.Success, language);
        }
        catch (InputNotValidException e)
        {
            Log.Error("UpdateOrderRatingChangeRequest endpoint - Input not valid - " + e);
            return new GenericResponse(ResponseErrorMessage.InvalidArgs, language);
        }
        catch (NotFoundException e)
        {
            Log.Error("UpdateOrderRatingChangeRequest endpoint - Not found - " + e);
            return new GenericResponse(ResponseErrorMessage.NotFound, language);
        }
        catch (DatabaseException e)
        {
            Log.Error("UpdateOrderRatingChangeRequest endpoint - Database Error - " + e);
            return new GenericResponse(ResponseErrorMessage.DatabaseQueryError, language);
        }
        catch (Exception e)
        {
            Log.Error("UpdateOrderRatingChangeRequest endpoint - Error - " + e);
            return new GenericResponse(ResponseErrorMessage.InternalError, language);
        }
    }
}