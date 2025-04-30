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
[Route("api/clients/ratings")]
public class ClientRatingsController : ControllerBase
{


    [HttpPatch]
    [Route("{clientCode}/{ratingTypeId}")]
    [RequestLimit]
    [ValidateReferrer]
    [Authorize]
    public ActionResult<ClientRatingItemResponse> PatchClientRating(string clientCode, int ratingTypeId, PatchClientRating clientRating)
    {
        string language = this.Request.Headers["client-lang"];
        if (string.IsNullOrEmpty(language))
        {
            language = ConfigManager.defaultLanguage;
        }
        string token = this.Request.Headers["Authorization"];
        string executer_user = UserModel.GetUserByToken(token);

        if (!clientRating.IsValid())
        {
            return new ClientRatingItemResponse(ResponseErrorMessage.InvalidArgs, language);
        }

        try
        {
            ClientRatingItem clientRatingItem = new ClientRatingItemBuilder()
                .SetClientCode(clientCode)
                .SetRatingTypeId(ratingTypeId)
                .SetRating(clientRating.rating)
                .Build();

            ClientRatingModel.PatchClientRating(clientRatingItem, executer_user);

            return new ClientRatingItemResponse(ResponseSuccessMessage.Success, language);
        }
        catch (InputNotValidException e)
        {
            Log.Error("PatchClientRating endpoint - Error - " + e);
            return new ClientRatingItemResponse(ResponseErrorMessage.InvalidArgs, language);
        }
        catch (Exception e)
        {
            Log.Error("PatchClientRating endpoint - Error - " + e);
            return new ClientRatingItemResponse(ResponseErrorMessage.InternalError, language);
        }
    }

    [HttpPost]
    [Route("sync-primavera/{syncType}")]
    [RequestLimit]
    [ValidateReferrer]
    public async Task<ActionResult<SyncPrimaveraStatsResponse>> SyncPrimaveraCredit(string key, string syncType)
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

            ISyncPrimaveraStrategy strategy = syncType switch
            {
                "credit" => new SyncPrimaveraCreditStrategy(),
                "payment-compliance" => new SyncPrimaveraPaymentComplianceStrategy(),
                "historical-volume" => new SyncPrimaveraHistoricalVolumeStrategy(),
                _ => throw new ArgumentException($"Given sync type {syncType} is invalid")
            };

            SyncPrimaveraStats stats = await strategy.Execute(executer_user);
            return new SyncPrimaveraStatsResponse(stats, ResponseSuccessMessage.Success, language);
        }
        catch (ArgumentException e)
        {
            Log.Error($"SyncPrimavera {syncType} endpoint - Argument Error - " + e);
            return new SyncPrimaveraStatsResponse(ResponseErrorMessage.InvalidArgs, language);
        }
        catch (PrimaveraApiErrorException e)
        {
            Log.Error($"SyncPrimavera {syncType} endpoint - Primavera Api Error - " + e);
            return new SyncPrimaveraStatsResponse(ResponseErrorMessage.PrimaveraApiError, language);
        }
        catch (ResourceEmptyException e)
        {
            Log.Error($"SyncPrimavera {syncType} endpoint - Resource Empty Error - " + e);
            return new SyncPrimaveraStatsResponse(ResponseErrorMessage.ResourceEmpty, language);
        }
        catch (Exception e)
        {
            Log.Error($"SyncPrimavera {syncType} endpoint - Error - " + e);
            return new SyncPrimaveraStatsResponse(ResponseErrorMessage.InternalError, language);
        }
    }
}
