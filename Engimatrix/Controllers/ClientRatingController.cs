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

    [HttpPut]
    [Route("{clientCode}")]
    [RequestLimit]
    [ValidateReferrer]
    [Authorize]
    public ActionResult<ClientRatingItemResponse> UpdateClientRatings(string clientCode, UpdateClientRatings req)
    {
        string language = this.Request.Headers["client-lang"];
        if (string.IsNullOrEmpty(language))
        {
            language = ConfigManager.defaultLanguage;
        }
        string token = this.Request.Headers["Authorization"];
        string executer_user = UserModel.GetUserByToken(token);

        if (!req.IsValid())
        {
            return new ClientRatingItemResponse(ResponseErrorMessage.InvalidArgs, language);
        }

        try
        {
            ClientRatingModel.UpdateClientRatings(req, clientCode, executer_user);

            return new ClientRatingItemResponse(ResponseSuccessMessage.Success, language);
        }
        catch (InputNotValidException e)
        {
            Log.Error("UpdateClientRatings endpoint - Error - " + e);
            return new ClientRatingItemResponse(ResponseErrorMessage.InvalidArgs, language);
        }
        catch (Exception e)
        {
            Log.Error("UpdateClientRatings endpoint - Error - " + e);
            return new ClientRatingItemResponse(ResponseErrorMessage.InternalError, language);
        }
    }
}
