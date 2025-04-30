// // Copyright (c) 2024 Engibots. All rights reserved.

namespace engimatrix.Controllers
{
    using System;
    using engimatrix.Views;
    using Microsoft.AspNetCore.Mvc;
    using engimatrix.Config;
    using engimatrix.Exceptions;
    using engimatrix.Filters;
    using engimatrix.Models;
    using engimatrix.ResponseMessages;
    using engimatrix.Utils;
    using Engimatrix.Models;
    using Engimatrix.ModelObjs;
    using engimatrix.ModelObjs;
    using System.Text.Json;

    [ApiController]
    [Route("api/forwards")]
    public class EmailForwardController : ControllerBase
    {
        [HttpGet]
        [Route("{emailToken}")]
        [RequestLimit]
        [ValidateReferrer]
        [Authorize]
        public ActionResult<EmailForwardResponse> GetForwardsByEmailToken(string emailToken)
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
                List<EmailForwardItem> forwards = EmailForwardModel.GetEmailForwardsByTokenOrId(emailToken, executer_user);
                return new EmailForwardResponse(forwards, ResponseSuccessMessage.Success, language);
            }
            catch (Exception e)
            {
                Log.Error("Filtered endpoint - Error - " + e);
                return new EmailForwardResponse(ResponseErrorMessage.InternalError, language);
            }
        }

        [HttpPost]
        [Route("{emailToken}")]
        [RequestLimit]
        [ValidateReferrer]
        [Authorize]
        public async Task<ActionResult<GenericResponse>> FwdEmailAsync(Filtering.FwdEmail input, string emailToken)
        {
            string language = this.Request.Headers["client-lang"];
            if (string.IsNullOrEmpty(language))
            {
                language = ConfigManager.defaultLanguage;
            }

            if (!input.Validate())
            {
                return new GenericResponse(ResponseErrorMessage.InvalidArgs, language);
            }

            string token = this.Request.Headers["Authorization"];
            string executer_user = UserModel.GetUserByToken(token);
            try
            {
                await EmailForwardModel.FwdEmail(emailToken, input.email_to_list, input.message, executer_user);
                return new GenericResponse(ResponseSuccessMessage.Success, language);
            }
            catch (Exception e)
            {
                Log.Error("FwdOrder endpoint - Error - " + e);
                return new GenericResponse(ResponseErrorMessage.InternalError, language);
            }
        }

        [HttpGet]
        [Route("recipients")]
        [RequestLimit]
        [ValidateReferrer]
        [Authorize]
        public ActionResult<EmailRecipientsForwardResponse> GetMostForwardedRecipientEmails()
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
                List<string> emails = EmailForwardModel.GetMostForwardedEmailRecipients(executer_user);
                return new EmailRecipientsForwardResponse(emails, ResponseSuccessMessage.Success, language);
            }
            catch (Exception e)
            {
                Log.Error("Filtered endpoint - Error - " + e);
                return new EmailRecipientsForwardResponse(ResponseErrorMessage.InternalError, language);
            }
        }
    }
}
