// // Copyright (c) 2024 Engibots. All rights reserved.

using engimatrix.Config;
using engimatrix.Filters;
using engimatrix.ResponseMessages;
using engimatrix.Views;
using Microsoft.AspNetCore.Mvc;
using engimatrix.Models;
using engimatrix.Utils;
using static engimatrix.Config.RatingConstants;
using engimatrix.Exceptions;
using engimatrix.ModelObjs;
using engimatrix.ModelObjs.Primavera;

namespace Engimatrix.Controllers;

[ApiController]
[Route("api/primavera-orders")]
public class PrimaveraOrderController : ControllerBase
{

    [HttpGet]
    [Route("client/{client_code}")]
    [RequestLimit]
    [ValidateReferrer]
    [Authorize]
    public async Task<ActionResult<ClientPrimaveraOrdersResponse>> GetClientPrimaveraOrders(string client_code)
    {
        string language = this.Request.Headers["client-lang"];
        if (string.IsNullOrEmpty(language))
        {
            language = ConfigManager.defaultLanguage;
        }
        string token = this.Request.Headers["Authorization"];
        string executer_user = UserModel.GetUserByToken(token);
        if (string.IsNullOrEmpty(client_code) || string.IsNullOrEmpty(executer_user))
        {
            return new ClientPrimaveraOrdersResponse(ResponseErrorMessage.InvalidArgs, language);
        }

        try
        {
            List<PrimaveraOrderItem> clientOrders = await PrimaveraOrderModel.GetClientOrdersPrimaveraLastYear(client_code);
            decimal total = 0;
            if (clientOrders.Count > 0)
            {
                List<PrimaveraOrderHeaderItem> clientHeaders = clientOrders.Select(x => x.primavera_order_header).ToList();
                total = PrimaveraOrderModel.GetOrdersTotal(clientHeaders);
            }

            return new ClientPrimaveraOrdersResponse(clientOrders, total, ResponseSuccessMessage.Success, language);
        }
        catch (Exception e)
        {
            Log.Error("GetClientPrimaveraOrders endpoint - Error - " + e);
            return new ClientPrimaveraOrdersResponse(ResponseErrorMessage.InternalError, language);
        }
    }

    [HttpGet]
    [Route("client/{client_code}/month")]
    [RequestLimit]
    [ValidateReferrer]
    [Authorize]
    public async Task<ActionResult<ClientPrimaveraOrdersResponse>> GetClientPrimaveraOrdersLastMonth(string client_code)
    {
        string language = this.Request.Headers["client-lang"];
        if (string.IsNullOrEmpty(language))
        {
            language = ConfigManager.defaultLanguage;
        }
        string token = this.Request.Headers["Authorization"];
        string executer_user = UserModel.GetUserByToken(token);
        if (string.IsNullOrEmpty(client_code) || string.IsNullOrEmpty(executer_user))
        {
            return new ClientPrimaveraOrdersResponse(ResponseErrorMessage.InvalidArgs, language);
        }

        try
        {
            List<PrimaveraOrderItem> clientOrders = await PrimaveraOrderModel.GetClientOrdersPrimaveraLastMonth(client_code);
            decimal total = 0;
            if (clientOrders.Count > 0)
            {
                List<PrimaveraOrderHeaderItem> clientHeaders = clientOrders.Select(x => x.primavera_order_header).ToList();
                total = PrimaveraOrderModel.GetOrdersTotal(clientHeaders);
            }

            return new ClientPrimaveraOrdersResponse(clientOrders, total, ResponseSuccessMessage.Success, language);
        }
        catch (Exception e)
        {
            Log.Error("GetClientPrimaveraOrders endpoint - Error - " + e);
            return new ClientPrimaveraOrdersResponse(ResponseErrorMessage.InternalError, language);
        }
    }

    [HttpGet]
    [Route("")]
    [RequestLimit]
    [ValidateReferrer]
    [AuthorizeAdmin]
    public async Task<ActionResult<ClientPrimaveraOrdersResponse>> GetOrdersTotal()
    {

        string language = this.Request.Headers["client-lang"];
        if (string.IsNullOrEmpty(language))
        {
            language = ConfigManager.defaultLanguage;
        }
        string token = this.Request.Headers["Authorization"];
        string executer_user = UserModel.GetUserByToken(token);
        if (string.IsNullOrEmpty(executer_user))
        {
            return new ClientPrimaveraOrdersResponse(ResponseErrorMessage.InvalidArgs, language);
        }

        try
        {
            decimal total = await PrimaveraOrderModel.GetAllOrdersTotal();

            return new ClientPrimaveraOrdersResponse(total, ResponseSuccessMessage.Success, language);
        }
        catch (InputNotValidException e)
        {
            Log.Error("UpdateRatingType - RatingType - Error - " + e);
            return BadRequest(new ClientPrimaveraOrdersResponse(ResponseErrorMessage.InvalidArgs, language));
        }
        catch (Exception e)
        {
            Log.Error("UpdateRatingType endpoint - Error - " + e);
            return new ClientPrimaveraOrdersResponse(ResponseErrorMessage.InternalError, language);
        }
    }
}
