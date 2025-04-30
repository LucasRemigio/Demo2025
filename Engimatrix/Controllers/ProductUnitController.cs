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
[Route("api/products/sizes")]
public class ProductUnitController : ControllerBase
{
    [HttpGet]
    [Route("")]
    [RequestLimit]
    [ValidateReferrer]
    public async Task<ActionResult<ProductUnitResponse>> GetProductUnits()
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
            List<ProductUnitItem> productUnits = ProductUnitModel.GetProductUnits(executer_user);
            return new ProductUnitResponse(productUnits, ResponseSuccessMessage.Success, language);
        }
        catch (Exception e)
        {
            Log.Error("GetSegments endpoint - Error - " + e);
            return new ProductUnitResponse(ResponseErrorMessage.InternalError, language);
        }
    }
}