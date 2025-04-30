// // Copyright (c) 2024 Engibots. All rights reserved.

using Microsoft.AspNetCore.Mvc;
using engimatrix.Config;
using engimatrix.Filters;
using engimatrix.ResponseMessages;
using engimatrix.Views;
using engimatrix.Models;
using engimatrix.Processes;
using engimatrix.Utils;
using engimatrix.ModelObjs.CTT;
using engimatrix.Exceptions;

namespace engimatrix.Controllers;

[ApiController]
[Route("api/ctt/districts")]
public class CttDistrictController : Controller
{
    [HttpGet]
    [Route("")]
    [RequestLimit]
    [ValidateReferrer]
    public ActionResult<CttDistrictListResponse> GetAllDistricts()
    {
        string? language = this.Request.Headers["client-lang"];
        if (string.IsNullOrEmpty(language))
        {
            language = ConfigManager.defaultLanguage;
        }
        string? token = this.Request.Headers["Authorization"];
        string executer_user = string.Empty;
        if (!string.IsNullOrEmpty(token))
        {
            executer_user = UserModel.GetUserByToken(token);
        }

        try
        {
            List<CttDistrictItem> districts = CttDistrictModel.GetAll(executer_user);
            return new CttDistrictListResponse(districts, ResponseSuccessMessage.Success, language);
        }
        catch (DatabaseException e)
        {
            Log.Error("GetAllDistricts endpoint - DatabaseException Error - " + e);
            return new CttDistrictListResponse(ResponseErrorMessage.DatabaseQueryError, language);
        }
        catch (ResourceEmptyException e)
        {
            Log.Error("GetAllDistricts endpoint - ResourceEmptyException Error - " + e);
            return new CttDistrictListResponse(ResponseErrorMessage.ResourceEmpty, language);
        }
        catch (Exception e)
        {
            Log.Error("UpdateCTTPostalCodes endpoint - Error - " + e);
            return new CttDistrictListResponse(ResponseErrorMessage.InternalError, language);
        }
    }
}