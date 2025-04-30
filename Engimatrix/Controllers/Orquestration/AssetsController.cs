// // Copyright (c) 2024 Engibots. All rights reserved.

namespace engimatrix.Controllers.Orquestration
{
    using System;
    using Microsoft.AspNetCore.Mvc;
    using engimatrix.Config;
    using engimatrix.Filters;
    using engimatrix.Models.Orquestration;
    using engimatrix.ResponseMessages;
    using engimatrix.Utils;
    using engimatrix.Views.Orquestration;
    using engimatrix.ModelObjs.Orquestration;
    using static engimatrix.Views.Orquestration.AssetsResponse;
    using engimatrix.Models;
    using engimatrix.Views;

    [ApiController]
    [Route("api/assets")]
    public class AssetsController : ControllerBase
    {
        [HttpGet]
        [Route("getAssets")]
        [RequestLimit]
        [ValidateReferrer]
        [Authorize]
        public ActionResult<GetAssets> GetAssets()
        {
            string language = this.Request.Headers["client-lang"];
            if (string.IsNullOrEmpty(language))
            {
                language = ConfigManager.defaultLanguage;
            }
            string token = this.Request.Headers["Authorization"];
            string user_operation = UserModel.GetUserByToken(token);

            try
            {
                return new GetAssets(AssetsModel.GetAssets(user_operation, language), ResponseSuccessMessage.Success, language);
            }
            catch (Exception e)
            {
                Log.Error("GetAssets endpoint - Error - optional args - " + user_operation);
                return new GetAssets(ResponseErrorMessage.AssetsError, language);
            }
        }

        [HttpGet]
        [Route("getAssetByName")]
        [RequestLimit]
        [ValidateReferrer]
        [Authorize]
        public ActionResult<GetAsset> GetAssetByName(string assetName)
        {
            string language = this.Request.Headers["client-lang"];
            if (string.IsNullOrEmpty(language))
            {
                language = ConfigManager.defaultLanguage;
            }
            string token = this.Request.Headers["Authorization"];
            string user_operation = UserModel.GetUserByToken(token);

            if (string.IsNullOrEmpty(assetName))
            {
                Log.Error("GetAssets endpoint - Error - optional args - " + user_operation);
                return new GetAsset(ResponseErrorMessage.AssetsError, language);
            }

            try
            {
                AssetsItem assetItem = AssetsModel.GetAssetByName(assetName, user_operation);
                return new GetAsset(assetItem, ResponseErrorMessage.AssetsError, language);
            }
            catch (Exception e)
            {
                Log.Error("GetTextAssetByName endpoint - Error - optional args - " + user_operation);
                return new GetAsset(ResponseErrorMessage.AssetsError, language);
            }
        }

        [HttpPost]
        [Route("addAsset")]
        [RequestLimit]
        [ValidateReferrer]
        [Authorize]
        public ActionResult<GenericResponse> AddAsset(AssetsRequest.Add input)
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
                if (!AssetsModel.Add(input, executer_user))
                {
                    Log.Error("Add Asset - A problem occurred registing order- ");
                    return new GenericResponse(ResponseErrorMessage.ErrorAddingAsset, language);
                }

                return new GenericResponse(ResponseSuccessMessage.Success, language);
            }
            catch (Exception e)
            {
                Log.Error("Add Asset endpoint - Error - " + e);
                return new GenericResponse(ResponseErrorMessage.ErrorAddingAsset, language);
            }
        }

        [HttpPost]
        [Route("removeAsset")]
        [RequestLimit]
        [ValidateReferrer]
        [Authorize]
        public ActionResult<GenericResponse> removeAsset(string id)
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
                if (!AssetsModel.remove(id, executer_user))
                {
                    Log.Error("Remove Asset - A problem occurred removing Asset- ");
                    return new GenericResponse(ResponseErrorMessage.ErrorRemovingAsset, language);
                }

                return new GenericResponse(ResponseSuccessMessage.Success, language);
            }
            catch (Exception e)
            {
                Log.Error("Remove Asset endpoint - Error - " + e);
                return new GenericResponse(ResponseErrorMessage.ErrorRemovingAsset, language);
            }
        }

        [HttpPost]
        [Route("editAssets")]
        [RequestLimit]
        [ValidateReferrer]
        [Authorize]
        public ActionResult<GenericResponse> EditAssets(AssetsRequest.Edit input)
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
                if (!AssetsModel.edit(input, executer_user))
                {
                    Log.Error("Edit Asset - A problem occurred edit Asset- ");
                    return new GenericResponse(ResponseErrorMessage.ErrorEditingAsset, language);
                }

                return new GenericResponse(ResponseSuccessMessage.Success, language);
            }
            catch (Exception e)
            {
                Log.Error("Edit Asset endpoint - Error - " + e);
                return new GenericResponse(ResponseErrorMessage.ErrorEditingAsset, language);
            }
        }
    }
}
