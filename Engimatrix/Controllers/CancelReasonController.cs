// // Copyright (c) 2024 Engibots. All rights reserved.

namespace engimatrix.Controllers;

using System;
using engimatrix.Views;
using Microsoft.AspNetCore.Mvc;
using engimatrix.Config;
using engimatrix.Exceptions;
using engimatrix.Filters;
using engimatrix.Models;
using engimatrix.ResponseMessages;
using engimatrix.Utils;
using Engimatrix.Models;
using Engimatrix.Views;
using Engimatrix.ModelObjs;
using System.Text.Json;
using engimatrix.Connector;
using engimatrix.ModelObjs;

[ApiController]
[Route("api/cancelReasons")]
public class CancelReasonController : ControllerBase
{
    [HttpGet]
    [Route("")]
    [RequestLimit]
    [ValidateReferrer]
    public ActionResult<CancelReasonResponseList> GetAllCancelReasons(bool? isActive)
    {
        string language = this.Request.Headers["client-lang"];
        if (string.IsNullOrEmpty(language))
        {
            language = ConfigManager.defaultLanguage;
        }
        string token = this.Request.Headers["Authorization"];

        string executer_user = "Anonymous Client";
        if (!String.IsNullOrEmpty(token))
        {
            executer_user = UserModel.GetUserByToken(token);
        }

        try
        {
            List<CancelReasonItem> cancelReasons = CancelReasonModel.GetCancelReasons(executer_user, isActive);
            return new CancelReasonResponseList(cancelReasons, ResponseSuccessMessage.Success, language);
        }
        catch (Exception e)
        {
            Log.Error("GetAllCancelReasons endpoint - Error - " + e);
            return new CancelReasonResponseList(ResponseErrorMessage.InternalError, language);
        }
    }

    [HttpGet]
    [Route("{id}")]
    [RequestLimit]
    [ValidateReferrer]
    [Authorize]
    public ActionResult<CancelReasonResponse> GetCancelReason(int id)
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
            CancelReasonItem cancelReason = CancelReasonModel.GetCancelReason(id, executer_user);
            return new CancelReasonResponse(cancelReason, ResponseSuccessMessage.Success, language);
        }
        catch (Exception e)
        {
            Log.Error("GetCancelReason endpoint - Error - " + e);
            return new CancelReasonResponse(ResponseErrorMessage.InternalError, language);
        }
    }

    [HttpPost]
    [Route("")]
    [RequestLimit]
    [ValidateReferrer]
    [Authorize]
    public ActionResult<CancelReasonResponse> CreateCancelReason([FromBody] CancelReasonCreateRequest request)
    {
        string language = this.Request.Headers["client-lang"];
        if (string.IsNullOrEmpty(language))
        {
            language = ConfigManager.defaultLanguage;
        }

        if (!request.IsValid())
        {
            return new CancelReasonResponse(ResponseErrorMessage.InvalidArgs, language);
        }

        string token = this.Request.Headers["Authorization"];
        string executer_user = UserModel.GetUserByToken(token);

        try
        {
            bool success = CancelReasonModel.CreateCancelReason(request.reason, request.description, executer_user);
            if (!success)
            {
                return new CancelReasonResponse(ResponseErrorMessage.InternalError, language);
            }
            return new CancelReasonResponse(ResponseSuccessMessage.Success, language);
        }
        catch (Exception e)
        {
            Log.Error("CreateCancelReason endpoint - Error - " + e);
            return new CancelReasonResponse(ResponseErrorMessage.InternalError, language);
        }
    }

    [HttpPut]
    [Route("{id}")]
    [RequestLimit]
    [ValidateReferrer]
    [Authorize]
    public ActionResult<CancelReasonResponse> UpdateCancelReason(int id, [FromBody] CancelReasonUpdateRequest request)
    {
        string language = this.Request.Headers["client-lang"];
        if (string.IsNullOrEmpty(language))
        {
            language = ConfigManager.defaultLanguage;
        }

        if (!request.IsValid())
        {
            return new CancelReasonResponse(ResponseErrorMessage.InvalidArgs, language);
        }

        string token = this.Request.Headers["Authorization"];
        string executer_user = UserModel.GetUserByToken(token);

        try
        {
            bool success = CancelReasonModel.UpdateCancelReason(id, request.reason, request.description, executer_user);
            if (!success)
            {
                return new CancelReasonResponse(ResponseErrorMessage.InternalError, language);
            }
            return new CancelReasonResponse(ResponseSuccessMessage.Success, language);
        }
        catch (Exception e)
        {
            Log.Error("UpdateCancelReason endpoint - Error - " + e);
            return new CancelReasonResponse(ResponseErrorMessage.InternalError, language);
        }
    }

    [HttpPatch]
    [Route("{id}")]
    [RequestLimit]
    [ValidateReferrer]
    [Authorize]
    public ActionResult<CancelReasonResponse> UpdateCancelReasonStatus(int id, [FromBody] CancelReasonStatusUpdateRequest request)
    {
        string language = this.Request.Headers["client-lang"];
        if (string.IsNullOrEmpty(language))
        {
            language = ConfigManager.defaultLanguage;
        }

        if (!request.IsValid())
        {
            return new CancelReasonResponse(ResponseErrorMessage.InvalidArgs, language);
        }

        string token = this.Request.Headers["Authorization"];
        string executer_user = UserModel.GetUserByToken(token);

        try
        {
            bool success = CancelReasonModel.ChangeActiveStatus(id, request.is_active, executer_user);
            if (!success)
            {
                return new CancelReasonResponse(ResponseErrorMessage.InternalError, language);
            }
            return new CancelReasonResponse(ResponseSuccessMessage.Success, language);
        }
        catch (Exception e)
        {
            Log.Error("UpdateCancelReasonStatus endpoint - Error - " + e);
            return new CancelReasonResponse(ResponseErrorMessage.InternalError, language);
        }
    }
}
