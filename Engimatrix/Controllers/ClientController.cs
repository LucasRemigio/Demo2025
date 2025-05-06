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
[Route("api/clients")]
public class ClientController : ControllerBase
{
    [HttpGet]
    [Route("")]
    [RequestLimit]
    [ValidateReferrer]
    [Authorize]
    public async Task<ActionResult<ClientListResponse>> GetClients()
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
            List<ClientDTO> clients = await ClientModel.GetClientsWithRatingsDTO(executer_user);
            return new ClientListResponse(clients, ResponseSuccessMessage.Success, language);
        }
        catch (Exception e)
        {
            Log.Error("GetSegments endpoint - Error - " + e);
            return new ClientListResponse(ResponseErrorMessage.InternalError, language);
        }
    }

    [HttpGet]
    [Route("search")]
    [Authorize]
    [RequestLimit]
    [ValidateReferrer]
    public async Task<ActionResult<ClientListResponse>> SearchClients([FromQuery] string? query)
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
            List<ClientDTO> clients = await ClientModel.SearchClients(query, language);
            return new ClientListResponse(clients, ResponseSuccessMessage.Success, language);
        }
        catch (Exception e)
        {
            Log.Error("SearchClients endpoint - Error - " + e);
            return new ClientListResponse(ResponseErrorMessage.InternalError, language);
        }
    }

    [HttpGet]
    [Route("{clientCode}")]
    [RequestLimit]
    [ValidateReferrer]
    [Authorize]
    public async Task<ActionResult<ClientItemResponse>> GetClientByCode(string clientCode)
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
            ClientDTO client = await ClientModel.GetClientWithRatingsByCodeDTO(clientCode, executer_user);
            return new ClientItemResponse(client, ResponseSuccessMessage.Success, language);
        }
        catch (Exception e)
        {
            Log.Error("GetSegments endpoint - Error - " + e);
            return new ClientItemResponse(ResponseErrorMessage.InternalError, language);
        }
    }

    [HttpGet]
    [Route("no-auth/{clientToken}")]
    [RequestLimit]
    [ValidateReferrer]
    public async Task<ActionResult<ClientNoAuthItemResponse>> GetClientByToken(string clientToken)
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
            ClientNoAuthDTO? client = await ClientModel.GetClientNoAuthByTokenDto(clientToken, executer_user);
            if (client == null)
            {
                return new ClientNoAuthItemResponse(ResponseErrorMessage.InvalidArgs, language);
            }

            return new ClientNoAuthItemResponse(client, ResponseSuccessMessage.Success, language);
        }
        catch (Exception e)
        {
            Log.Error("GetSegments endpoint - Error - " + e);
            return new ClientNoAuthItemResponse(ResponseErrorMessage.InternalError, language);
        }
    }

    [HttpGet]
    [Route("{clientCode}/{orderToken}")]
    [RequestLimit]
    [ValidateReferrer]
    [Authorize]
    public async Task<ActionResult<ClientItemResponse>> GetClientWithRatingsByCodeDTOByOrderToken(string clientCode, string orderToken)
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
            ClientDTO client = await ClientModel.GetClientWithRatingsByCodeDTOByOrderToken(clientCode, orderToken, executer_user);

            return new ClientItemResponse(client, ResponseSuccessMessage.Success, language);
        }
        catch (InputNotValidException e)
        {
            Log.Error("GetClientWithRatingsByCodeDTOByOrderToken endpoint - Error - " + e);
            return new ClientItemResponse(ResponseErrorMessage.InvalidArgs, language);
        }
        catch (Exception e)
        {
            Log.Error("GetClientWithRatingsByCodeDTOByOrderToken endpoint - Error - " + e);
            return new ClientItemResponse(ResponseErrorMessage.InternalError, language);
        }
    }

    [HttpPatch]
    [Route("{clientCode}/segment")]
    [RequestLimit]
    [ValidateReferrer]
    [Authorize]
    public ActionResult<ClientRatingItemResponse> UpdateClientRatingSegment(string clientCode, PatchClientSegment segmentReq)
    {
        string language = this.Request.Headers["client-lang"];
        if (string.IsNullOrEmpty(language))
        {
            language = ConfigManager.defaultLanguage;
        }
        string token = this.Request.Headers["Authorization"];
        string executer_user = UserModel.GetUserByToken(token);

        if (!segmentReq.IsValid())
        {
            return new ClientRatingItemResponse(ResponseErrorMessage.InvalidArgs, language);
        }

        try
        {
            ClientModel.PatchSegment(clientCode, segmentReq.segment_id, executer_user);

            return new ClientRatingItemResponse(ResponseSuccessMessage.Success, language);
        }
        catch (InputNotValidException e)
        {
            Log.Error("UpdateClientRatingSegment endpoint - Error - " + e);
            return new ClientRatingItemResponse(ResponseErrorMessage.InvalidArgs, language);
        }
        catch (Exception e)
        {
            Log.Error("UpdateClientRatingSegment endpoint - Error - " + e);
            return new ClientRatingItemResponse(ResponseErrorMessage.InternalError, language);
        }
    }

    [HttpPut]
    [Route("fix-codes")]
    [RequestLimit]
    [ValidateReferrer]
    [Authorize]
    public ActionResult<RatingTypeItemResponse> UpdateClientCodes()
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
            ClientModel.FixClientCodes(executer_user);

            return new RatingTypeItemResponse(ResponseSuccessMessage.Success, language);
        }
        catch (InputNotValidException e)
        {
            Log.Error("UpdateRatingType - RatingType - Error - " + e);
            return BadRequest(new RatingTypeItemResponse(ResponseErrorMessage.InvalidArgs, language));
        }
        catch (Exception e)
        {
            Log.Error("UpdateRatingType endpoint - Error - " + e);
            return new RatingTypeItemResponse(ResponseErrorMessage.InternalError, language);
        }
    }

    [HttpPost]
    [Route("{clientCode}")]
    [RequestLimit]
    [ValidateReferrer]
    [Authorize]
    public async Task<ActionResult<ClientItemResponse>> CreateClient(string clientCode)
    {
        string language = this.Request.Headers["client-lang"];
        if (string.IsNullOrEmpty(language))
        {
            language = ConfigManager.defaultLanguage;
        }
        string token = this.Request.Headers["Authorization"];
        string executer_user = UserModel.GetUserByToken(token);

        if (string.IsNullOrEmpty(clientCode) || clientCode.Length < 4 || clientCode.Length > 10)
        {
            return new ClientItemResponse(ResponseErrorMessage.InvalidArgs, language);
        }

        try
        {
            await ClientModel.AddPrimaveraClient(clientCode, executer_user);

            return new ClientItemResponse(ResponseSuccessMessage.Success, language);
        }
        catch (InputNotValidException e)
        {
            Log.Error("CreateClient endpoint - Error - " + e);
            return new ClientItemResponse(ResponseErrorMessage.InvalidArgs, language);
        }
        catch (Exception e)
        {
            Log.Error("CreateClient endpoint - Error - " + e);
            return new ClientItemResponse(ResponseErrorMessage.InternalError, language);
        }
    }

    [HttpPost]
    [Route("sync-primavera")]
    [RequestLimit]
    [ValidateReferrer]
    public async Task<ActionResult<SyncPrimaveraStatsResponse>> SyncPrimaveraClients(string key)
    {
        string language = this.Request.Headers["client-lang"];
        if (string.IsNullOrEmpty(language))
        {
            language = ConfigManager.defaultLanguage;
        }
        string token = this.Request.Headers["Authorization"];
        string executer_user = "System";
        if (!string.IsNullOrEmpty(token))
        {
            executer_user = UserModel.GetUserByToken(token);
        }

        try
        {
            if (string.IsNullOrEmpty(key) || !key.Equals(ConfigManager.engimatrixInternalApiKey, StringComparison.Ordinal))
            {
                return new SyncPrimaveraStatsResponse(ResponseErrorMessage.InvalidArgs, language);
            }

            SyncPrimaveraStats stats = await ClientModel.SyncPrimaveraClientsAndCreatePending(executer_user);

            return new SyncPrimaveraStatsResponse(stats, ResponseSuccessMessage.Success, language);
        }
        catch (InputNotValidException e)
        {
            Log.Error("CreateClient endpoint - Error - " + e);
            return new SyncPrimaveraStatsResponse(ResponseErrorMessage.InvalidArgs, language);
        }
        catch (AlreadyLoadingException e)
        {
            Log.Error("UpdateRatingType - RatingType - Error - " + e);
            return new SyncPrimaveraStatsResponse(ResponseErrorMessage.AlreadyLoading, language);
        }
        catch (Exception e)
        {
            Log.Error("CreateClient endpoint - Error - " + e);
            return new SyncPrimaveraStatsResponse(ResponseErrorMessage.InternalError, language);
        }
    }

}
