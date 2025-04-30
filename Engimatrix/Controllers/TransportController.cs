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
[Route("api/transports")]
public class TransportController : ControllerBase
{
    [HttpGet]
    [Route("")]
    [RequestLimit]
    [ValidateReferrer]
    public async Task<ActionResult<TransportListResponse>> GetTransports()
    {
        string language = this.Request.Headers["client-lang"];
        if (string.IsNullOrEmpty(language))
        {
            language = ConfigManager.defaultLanguage;
        }
        string token = this.Request.Headers["Authorization"];
        string executer_user = string.Empty;
        if (!string.IsNullOrEmpty(token))
        {
            executer_user = UserModel.GetUserByToken(token);
        }

        try
        {
            List<TransportItem> transports = TransportModel.GetTranports(executer_user);
            return new TransportListResponse(transports, ResponseSuccessMessage.Success, language);
        }
        catch (Exception e)
        {
            Log.Error("GetTransports endpoint - Error - " + e);
            return new TransportListResponse(ResponseErrorMessage.InternalError, language);
        }
    }

    [HttpGet]
    [Route("{id}")]
    [RequestLimit]
    [ValidateReferrer]
    [Authorize]
    public async Task<ActionResult<TransportItemResponse>> GetTransportById(int id)
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
            TransportItem? transport = TransportModel.GetTransportById(id, executer_user);
            if (transport == null)
            {
                return new TransportItemResponse(ResponseErrorMessage.NotFound, language);
            }

            return new TransportItemResponse(transport, ResponseSuccessMessage.Success, language);
        }
        catch (Exception e)
        {
            Log.Error("GetTransportById endpoint - Error - " + e);
            return new TransportItemResponse(ResponseErrorMessage.InternalError, language);
        }
    }
}