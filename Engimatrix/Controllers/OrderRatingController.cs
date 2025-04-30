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
[Route("api/orders/ratings")]
public class OrderRatingController : ControllerBase
{
    [HttpGet]
    [Route("rating/{orderToken}")]
    [RequestLimit]
    [ValidateReferrer]
    [Authorize]
    public ActionResult<GenericResponse> CalculateOrderWeightedRating(string orderToken)
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
            OrderRatingModel.GetOrderWeightedRating(orderToken, executer_user);
            return new GenericResponse(ResponseSuccessMessage.Success, language);
        }
        catch (Exception e)
        {
            Log.Error("GetRatingTypes endpoint - Error - " + e);
            return new GenericResponse(ResponseErrorMessage.InternalError, language);
        }
    }
}