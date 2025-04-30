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
[Route("api/ctt/postal-codes")]
public class CttPostalCodeController : Controller
{
    [HttpGet]
    [Route("")]
    [RequestLimit]
    [ValidateReferrer]
    public ActionResult<CttPostalCodeDtoListResponse> GetAllPostalCodesDto(string? cc, string? dd, string? cp4, string? cp3)
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
            List<CttPostalCodeDto> postalCodes = CttPostalCodeModel.GetAllDto(executer_user, cc, dd, cp4, cp3);
            return new CttPostalCodeDtoListResponse(postalCodes, ResponseSuccessMessage.Success, language);
        }
        catch (DatabaseException e)
        {
            Log.Error("GetAllDistricts endpoint - DatabaseException Error - " + e);
            return new CttPostalCodeDtoListResponse(ResponseErrorMessage.DatabaseQueryError, language);
        }
        catch (ResourceEmptyException e)
        {
            Log.Error("GetAllDistricts endpoint - ResourceEmptyException Error - " + e);
            return new CttPostalCodeDtoListResponse(ResponseErrorMessage.ResourceEmpty, language);
        }
        catch (Exception e)
        {
            Log.Error("UpdateCTTPostalCodes endpoint - Error - " + e);
            return new CttPostalCodeDtoListResponse(ResponseErrorMessage.InternalError, language);
        }
    }

    [HttpPost]
    [Route("update")]
    [RequestLimit]
    public ActionResult<GenericResponse> UpdateCttPostalCodes(string key)
    {
        string? language = this.Request.Headers["client-lang"];
        if (string.IsNullOrEmpty(language))
        {
            language = ConfigManager.defaultLanguage;
        }
        string? token = this.Request.Headers["Authorization"];

        string executer_user = "System";
        if (!string.IsNullOrEmpty(token))
        {
            executer_user = UserModel.GetUserByToken(token);
        }

        try
        {
            // validate key
            if (string.IsNullOrEmpty(key) || !key.Equals(ConfigManager.engimatrixInternalApiKey, StringComparison.Ordinal))
            {
                return new GenericResponse(ResponseErrorMessage.InvalidArgs, language);
            }

            bool success = CttPostalCodesProcess.UpdatePostalCodes(executer_user);
            if (!success)
            {
                return new GenericResponse(ResponseErrorMessage.ScriptError, language);
            }

            return new GenericResponse(ResponseSuccessMessage.Success, language);
        }
        catch (Exception e)
        {
            Log.Error("UpdateCTTPostalCodes endpoint - Error - " + e);
            return new GenericResponse(ResponseErrorMessage.InternalError, language);
        }
    }
}