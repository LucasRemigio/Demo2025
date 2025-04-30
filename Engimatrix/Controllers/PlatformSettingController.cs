// // Copyright (c) 2024 Engibots. All rights reserved.

using engimatrix.Config;
using engimatrix.Filters;
using engimatrix.ResponseMessages;
using engimatrix.Views;
using Microsoft.AspNetCore.Mvc;
using engimatrix.Models;
using engimatrix.Utils;
using engimatrix.ModelObjs;
using Engimatrix.Models;

namespace Engimatrix.Controllers;

[ApiController]
[Route("api/app-settings")]
public class PlatformSettingController : ControllerBase
{
    [HttpGet]
    [RequestLimit]
    [ValidateReferrer]
    [Authorize]
    public ActionResult<PlatformSettingListResponse> GetPlatformSettings()
    {
        // This endpoint will return all settings
        string language = this.Request.Headers["client-lang"];
        if (string.IsNullOrEmpty(language))
        {
            language = ConfigManager.defaultLanguage;
        }
        string token = this.Request.Headers["Authorization"];
        string executer_user = UserModel.GetUserByToken(token);

        try
        {
            List<PlatformSettingItem> settings = PlatformSettingModel.GetAll(executer_user);
            if (settings == null || settings.Count == 0)
            {
                return new PlatformSettingListResponse(ResponseErrorMessage.NotFound, language);
            }

            return new PlatformSettingListResponse(settings, ResponseSuccessMessage.Success, language);
        }
        catch (Exception e)
        {
            Log.Error("GetPlatformSettings endpoint - Error - " + e);
            return new PlatformSettingListResponse(ResponseErrorMessage.InternalError, language);
        }
    }

    [HttpGet]
    [Route("{settingId}")]
    [RequestLimit]
    [ValidateReferrer]
    [Authorize]
    public ActionResult<PlatformSettingResponse> GetSettingById(int settingId)
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
            PlatformSettingItem? settingItem = PlatformSettingModel.GetById(settingId, executer_user);
            if (settingItem == null)
            {
                return new PlatformSettingResponse(ResponseErrorMessage.NotFound, language);
            }

            return new PlatformSettingResponse(settingItem, ResponseSuccessMessage.Success, language);
        }
        catch (Exception e)
        {
            Log.Error("GetSettingById endpoint - Error - " + e);
            return new PlatformSettingResponse(ResponseErrorMessage.InternalError, language);
        }
    }

    [HttpPatch]
    [Route("{settingId}")]
    [RequestLimit]
    [ValidateReferrer]
    [Authorize]
    public ActionResult<PlatformSettingResponse> UpdateSetting(PlatformSettingUpdateRequest req, int settingId)
    {
        string language = this.Request.Headers["client-lang"];
        if (string.IsNullOrEmpty(language))
        {
            language = ConfigManager.defaultLanguage;
        }

        if (!req.IsValid())
        {
            return new PlatformSettingResponse(ResponseErrorMessage.InvalidArgs, language);
        }

        string token = this.Request.Headers["Authorization"];
        string executer_user = UserModel.GetUserByToken(token);

        try
        {
            PlatformSettingItem? settingItem = PlatformSettingModel.Patch(settingId, req.value, executer_user);
            if (settingItem == null)
            {
                return new PlatformSettingResponse(ResponseErrorMessage.NotFound, language);
            }

            return new PlatformSettingResponse(settingItem, ResponseSuccessMessage.Success, language);
        }
        catch (ArgumentException e)
        {
            Log.Error("UpdateSetting endpoint - Error - " + e);
            return new PlatformSettingResponse(ResponseErrorMessage.InvalidArgs, language);
        }
        catch (Exception e)
        {
            Log.Error("UpdateSetting endpoint - Error - " + e);
            return new PlatformSettingResponse(ResponseErrorMessage.InternalError, language);
        }
    }

}
