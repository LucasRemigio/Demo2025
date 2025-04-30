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
[Route("api/ctt/municipalities")]
public class CttMunicipalityController : Controller
{

    [HttpGet]
    [Route("")]
    [RequestLimit]
    [ValidateReferrer]
    public ActionResult<CttMunicipalityDtoListResponse> GetAllMunicipalitiesDto(string? dd)
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
            List<CttMunicipalityDto> municipalities = CttMunicipalityModel.GetAllDto(executer_user, dd);
            return new CttMunicipalityDtoListResponse(municipalities, ResponseSuccessMessage.Success, language);
        }
        catch (DatabaseException e)
        {
            Log.Error("GetAllDistricts endpoint - DatabaseException Error - " + e);
            return new CttMunicipalityDtoListResponse(ResponseErrorMessage.DatabaseQueryError, language);
        }
        catch (ResourceEmptyException e)
        {
            Log.Error("GetAllDistricts endpoint - ResourceEmptyException Error - " + e);
            return new CttMunicipalityDtoListResponse(ResponseErrorMessage.ResourceEmpty, language);
        }
        catch (Exception e)
        {
            Log.Error("UpdateCTTPostalCodes endpoint - Error - " + e);
            return new CttMunicipalityDtoListResponse(ResponseErrorMessage.InternalError, language);
        }
    }
}