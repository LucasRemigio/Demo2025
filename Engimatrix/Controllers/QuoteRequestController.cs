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

namespace Engimatrix.Controllers;

[ApiController]
[Route("api/quotes/requests")]
public class QuoteRequestsController : ControllerBase
{
    [HttpGet]
    [Route("")]
    [RequestLimit]
    [ValidateReferrer]
    [Authorize]
    public ActionResult<QuoteRequestListResponse> GetQuoteRequests()
    {
        string language = this.Request.Headers["client-lang"];
        if (string.IsNullOrEmpty(language))
        {
            language = ConfigManager.defaultLanguage;
        }
        string token = this.Request.Headers["Authorization"];
        string executer_user = UserModel.GetUserByToken(token);

        try
        {
            return new QuoteRequestListResponse(QuoteRequestModel.GetQuoteRequest(executer_user), ResponseSuccessMessage.Success, language);
        }
        catch (Exception e)
        {
            Log.Error("GetQuoteRequests endpoint - Error - " + e);
            return new QuoteRequestListResponse(ResponseErrorMessage.InternalError, language);
        }
    }

    [HttpGet]
    [Route("{id}")]
    [RequestLimit]
    [ValidateReferrer]
    [Authorize]
    public ActionResult<QuoteRequestItemResponse> GetClientRatingsByClientId(int id)
    {
        string language = this.Request.Headers["client-lang"];
        if (string.IsNullOrEmpty(language))
        {
            language = ConfigManager.defaultLanguage;
        }
        string token = this.Request.Headers["Authorization"];
        string executer_user = UserModel.GetUserByToken(token);

        try
        {
            return new QuoteRequestItemResponse(QuoteRequestModel.GetQuoteRequestById(id, executer_user), ResponseSuccessMessage.Success, language);
        }
        catch (InputNotValidException e)
        {
            Log.Error("GetClientRatingsByClientId endpoint - Error - " + e);
            return new QuoteRequestItemResponse(ResponseErrorMessage.InvalidArgs, language);
        }
        catch (Exception e)
        {
            Log.Error("GetClientRatingsByClientId endpoint - Error - " + e);
            return new QuoteRequestItemResponse(ResponseErrorMessage.InternalError, language);
        }
    }

    [HttpPost]
    [Route("")]
    [RequestLimit]
    [ValidateReferrer]
    [Authorize]
    public ActionResult<QuoteRequestItemResponse> CreateQuoteRequest(QuoteRequestCreateRequest quoteRequest)
    {
        string language = this.Request.Headers["client-lang"];
        if (string.IsNullOrEmpty(language))
        {
            language = ConfigManager.defaultLanguage;
        }
        string token = this.Request.Headers["Authorization"];
        string executer_user = UserModel.GetUserByToken(token);

        if (!quoteRequest.IsValid())
        {
            return new QuoteRequestItemResponse(ResponseErrorMessage.InvalidArgs, language);
        }

        try
        {
            QuoteRequestItem quoteRequestItem = new(quoteRequest.quote_id_erp, quoteRequest.quote_date, quoteRequest.client_id, quoteRequest.client_name, quoteRequest.product_code, quoteRequest.quantity_requested,
                quoteRequest.erp_price, quoteRequest.erp_price_modification_percent, quoteRequest.alert_flag, quoteRequest.special_flag, quoteRequest.final_price, quoteRequest.order_quantity, quoteRequest.order_id,
                quoteRequest.observation, quoteRequest.unit_price, quoteRequest.margin_percent, quoteRequest.price_difference_erp, quoteRequest.price_difference_percent_erp, quoteRequest.total_difference_erp, quoteRequest.total_difference_final);

            QuoteRequestModel.CreateQuoteRequest(quoteRequestItem, executer_user);

            return new QuoteRequestItemResponse(ResponseSuccessMessage.Success, language);
        }
        catch (InputNotValidException e)
        {
            Log.Error("CreateQuoteRequest endpoint - Error - " + e);
            return new QuoteRequestItemResponse(ResponseErrorMessage.InvalidArgs, language);
        }
        catch (Exception e)
        {
            Log.Error("CreateQuoteRequest endpoint - Error - " + e);
            return new QuoteRequestItemResponse(ResponseErrorMessage.InternalError, language);
        }
    }

    [HttpPut]
    [Route("{id}")]
    [RequestLimit]
    [ValidateReferrer]
    [Authorize]
    public ActionResult<QuoteRequestItemResponse> UpdateQuoteRequest(int id, QuoteRequestUpdateRequest quoteRequest)
    {
        string language = this.Request.Headers["client-lang"];
        if (string.IsNullOrEmpty(language))
        {
            language = ConfigManager.defaultLanguage;
        }
        string token = this.Request.Headers["Authorization"];
        string executer_user = UserModel.GetUserByToken(token);

        if (!quoteRequest.IsValid())
        {
            return new QuoteRequestItemResponse(ResponseErrorMessage.InvalidArgs, language);
        }

        try
        {
            QuoteRequestItem quoteRequestItem = new(id, quoteRequest.quote_id_erp, quoteRequest.quote_date, quoteRequest.client_id, quoteRequest.client_name, quoteRequest.product_code, quoteRequest.quantity_requested,
                quoteRequest.erp_price, quoteRequest.erp_price_modification_percent, quoteRequest.alert_flag, quoteRequest.special_flag, quoteRequest.final_price, quoteRequest.order_quantity, quoteRequest.order_id,
                quoteRequest.observation, quoteRequest.unit_price, quoteRequest.margin_percent, quoteRequest.price_difference_erp, quoteRequest.price_difference_percent_erp, quoteRequest.total_difference_erp, quoteRequest.total_difference_final);

            QuoteRequestModel.UpdateQuoteRequest(quoteRequestItem, executer_user);

            return new QuoteRequestItemResponse(ResponseSuccessMessage.Success, language);
        }
        catch (InputNotValidException e)
        {
            Log.Error("UpdateQuoteRequest endpoint - Error - " + e);
            return new QuoteRequestItemResponse(ResponseErrorMessage.InvalidArgs, language);
        }
        catch (Exception e)
        {
            Log.Error("UpdateQuoteRequest endpoint - Error - " + e);
            return new QuoteRequestItemResponse(ResponseErrorMessage.InternalError, language);
        }
    }

    [HttpDelete]
    [Route("{id}")]
    [RequestLimit]
    [ValidateReferrer]
    [Authorize]
    public ActionResult<QuoteRequestItemResponse> DeleteQuoteRequest(int id)
    {
        string language = this.Request.Headers["client-lang"];
        if (string.IsNullOrEmpty(language))
        {
            language = ConfigManager.defaultLanguage;
        }
        string token = this.Request.Headers["Authorization"];
        string executer_user = UserModel.GetUserByToken(token);

        try
        {
            QuoteRequestModel.DeleteQuoteRequest(id, executer_user);

            return new QuoteRequestItemResponse(ResponseSuccessMessage.Success, language);
        }
        catch (InputNotValidException e)
        {
            Log.Error("DeleteQuoteRequest endpoint - Error - " + e);
            return new QuoteRequestItemResponse(ResponseErrorMessage.InvalidArgs, language);
        }
        catch (Exception e)
        {
            Log.Error("DeleteQuoteRequest endpoint - Error - " + e);
            return new QuoteRequestItemResponse(ResponseErrorMessage.InternalError, language);
        }
    }
}
