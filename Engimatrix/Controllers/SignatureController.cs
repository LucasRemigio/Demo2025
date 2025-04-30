// // Copyright (c) 2024 Engibots. All rights reserved.

using engimatrix.Config;
using engimatrix.Filters;
using engimatrix.ResponseMessages;
using engimatrix.Views;
using Microsoft.AspNetCore.Mvc;
using engimatrix.Models;
using engimatrix.Utils;

namespace Engimatrix.Controllers
{
    public class SignatureRequest
    {
        public string signature { get; set; }
    }

    [ApiController]
    [Route("api/signature")]
    public class SignatureController : ControllerBase
    {
        [HttpGet]
        [Route("")]
        [RequestLimit]
        [ValidateReferrer]
        [Authorize]
        public ActionResult<SignatureResponse> GetSignature()
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
                // Return the original email and the replies
                return new SignatureResponse(SignatureModel.GetSignature(executer_user), ResponseSuccessMessage.Success, language);
            }
            catch (Exception e)
            {
                Log.Error("Signature endpoint - Error - " + e);
                return new SignatureResponse(ResponseErrorMessage.InternalError, language);
            }
        }

        [HttpPatch]
        [Route("")]
        [RequestLimit]
        [ValidateReferrer]
        [Authorize]
        public ActionResult<SignatureResponse> PatchSignature([FromBody] SignatureRequest signature)
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
                SignatureModel.PatchSignature(signature.signature, executer_user);

                return new SignatureResponse(ResponseSuccessMessage.Success, language);
            }
            catch (Exception e)
            {
                Log.Error("Signature endpoint - Error - " + e);
                return new SignatureResponse(ResponseErrorMessage.InternalError, language);
            }
        }

        [HttpPost]
        [Route("")]
        [RequestLimit]
        [ValidateReferrer]
        [Authorize]
        public ActionResult<SignatureResponse> PostSignature([FromBody] SignatureRequest signature)
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
                SignatureModel.SaveSignature(signature.signature, executer_user, executer_user);

                return new SignatureResponse(ResponseSuccessMessage.Success, language);
            }
            catch (Exception e)
            {
                Log.Error("Signature endpoint - Error - " + e);
                return new SignatureResponse(ResponseErrorMessage.InternalError, language);
            }
        }

        [HttpGet]
        [Route("template")]
        [RequestLimit]
        [ValidateReferrer]
        [Authorize]
        public ActionResult<SimpleMessageResponse> GetDefaultFormattedSignature()
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
                // Return the original email and the replies
                return new SimpleMessageResponse(SignatureModel.GetDefaultFormattedSignature(executer_user), ResponseSuccessMessage.Success, language);
            }
            catch (Exception e)
            {
                Log.Error("Signature endpoint - Error - " + e);
                return new SimpleMessageResponse(ResponseErrorMessage.InternalError, language);
            }
        }


        [HttpGet]
        [Route("christmas")]
        [RequestLimit]
        [ValidateReferrer]
        [Authorize]
        public ActionResult<SimpleMessageResponse> GetChristmasGreeting()
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
                // Return the original email and the replies
                return new SimpleMessageResponse(SignatureModel.GetHappyChristmasGif(executer_user), ResponseSuccessMessage.Success, language);
            }
            catch (Exception e)
            {
                Log.Error("Signature endpoint - Error - " + e);
                return new SimpleMessageResponse(ResponseErrorMessage.InternalError, language);
            }
        }
    }
}
