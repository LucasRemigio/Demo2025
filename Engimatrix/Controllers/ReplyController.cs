// // Copyright (c) 2024 Engibots. All rights reserved.

using engimatrix.Config;
using engimatrix.Filters;
using Engimatrix.Models;
using engimatrix.ResponseMessages;
using engimatrix.Views;
using Microsoft.AspNetCore.Mvc;
using engimatrix.Models;
using engimatrix.Utils;
using engimatrix.Connector;
using Engimatrix.ModelObjs;
using Engimatrix.Views;
using engimatrix.Exceptions;
using static Engimatrix.Views.EmailTemplateResponse;

namespace Engimatrix.Controllers
{
    [ApiController]
    [Route("api/reply")]
    public class ReplyController : ControllerBase
    {
        [HttpGet]
        [Route("{emailToken}")]
        [RequestLimit]
        [ValidateReferrer]
        [Authorize]
        public ActionResult<ReplyResponse.GetReplyResponse> GetRepliesByEmailToken(string emailToken)
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
                return new ReplyResponse.GetReplyResponse(ReplyModel.getReplies(executer_user, emailToken), ResponseSuccessMessage.Success, language);
            }
            catch (Exception e)
            {
                Log.Error("Filtered endpoint - Error - " + e);
                return new ReplyResponse.GetReplyResponse(ResponseErrorMessage.InternalError, language);
            }
        }

        [HttpPost]
        [Route("{emailId}")]
        [RequestLimit]
        [ValidateReferrer]
        [Authorize]
        public async Task<ActionResult<ReplyResponse.GetReplyResponse>> PostReply(string emailId, ReplyRequest replyReq)
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
                await MasterFerro.ReplyToEmailAsync(emailId, replyReq, executer_user, true);

                return new ReplyResponse.GetReplyResponse(ResponseSuccessMessage.Success, language);
            }
            catch (Exception e)
            {
                Log.Error("ReplyToEmailEndpoint - Error - " + e);
                return new ReplyResponse.GetReplyResponse(ResponseErrorMessage.InternalError, language);
            }
        }

        [HttpPost]
        [Route("{emailToken}/startConcurrency")]
        [RequestLimit]
        [ValidateReferrer]
        [Authorize]
        public ActionResult<ReplyConcurrencyResponse> StartReplyConcurrency(string emailToken)
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
                (bool canReply, ReplyInfo replyInfo) = ReplyModel.StartReplyConcurrency(emailToken, executer_user);

                // someone is replying
                if (!canReply)
                {
                    return new ReplyConcurrencyResponse(canReply, replyInfo, ResponseSuccessMessage.Success, language);
                }

                return new ReplyConcurrencyResponse(ResponseSuccessMessage.Success, language);
            }
            catch (Exception e)
            {
                Log.Error("StartReplyConcurrencyEndpoint - Error - " + e);
                return new ReplyConcurrencyResponse(ResponseErrorMessage.InternalError, language);
            }
        }

        [HttpPost]
        [Route("{emailToken}/stopConcurrency")]
        [RequestLimit]
        [ValidateReferrer]
        [Authorize]
        public ActionResult<ReplyConcurrencyResponse> StopReplyConcurrency(string emailToken)
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
                ReplyModel.StopReplyConcurrency(emailToken, executer_user);

                return new ReplyConcurrencyResponse(ResponseSuccessMessage.Success, language);
            }
            catch (ConcurrencyEmailTokenNotFoundException e)
            {
                Log.Error("StopReplyConcurrencyEndpoint - Email Token Not Found - " + e);
                return new ReplyConcurrencyResponse(ResponseErrorMessage.ErrorEmailTokenNotFoundInConcurrency, language);
            }
            catch (ConcurrencyNotTheOwnerException)
            {
                // Not an error, just to prevent another user from removing the dictionary register when
                // another user is the owner of the current concurrency
                return new ReplyConcurrencyResponse(ResponseErrorMessage.AnotherUserIsOwnerOfConcurrency, language);
            }
            catch (Exception e)
            {
                Log.Error("StopReplyConcurrencyEndpoint - Error - " + e);
                return new ReplyConcurrencyResponse(ResponseErrorMessage.InternalError, language);
            }
        }

        [HttpGet]
        [Route("{emailToken}/generateResponseAI")]
        [RequestLimit]
        [ValidateReferrer]
        [Authorize]
        public async Task<ActionResult<ReplyGeneratedByAIResponse>> GenerateResponseAI(string emailToken, bool isReplyToOriginal)
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
                string response = await ReplyModel.generateResponseAI(emailToken, isReplyToOriginal, executer_user);

                return new ReplyGeneratedByAIResponse(response, ResponseSuccessMessage.Success, language);
            }
            catch (EmailNotFoundException e)
            {
                Log.Error("GenerateResponseAI - Error - " + e);
                return new ReplyGeneratedByAIResponse(ResponseErrorMessage.EmailNotFound, language);
            }
            catch (Exception e)
            {
                Log.Error("GenerateResponseAI - Error - " + e);
                return new ReplyGeneratedByAIResponse(ResponseErrorMessage.InternalError, language);
            }
        }

        [HttpPatch]
        [Route("{replyToken}/setRead")]
        [RequestLimit]
        [ValidateReferrer]
        [Authorize]
        public ActionResult<ReplyResponse.GetReplyResponse> SetReplyToRead(string replyToken)

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
                bool resultOk = ReplyModel.SetReplyToRead(replyToken, executer_user);

                if (!resultOk)
                {
                    return new ReplyResponse.GetReplyResponse(ResponseErrorMessage.InternalError, language);
                }

                return new ReplyResponse.GetReplyResponse(ResponseSuccessMessage.Success, language);
            }
            catch (Exception e)
            {
                Log.Error("SetReplyToRead Endpoint - Error - " + e);
                return new ReplyResponse.GetReplyResponse(ResponseErrorMessage.InternalError, language);
            }
        }

        [HttpGet]
        [Route("template/{orderToken}")]
        [RequestLimit]
        [Authorize]
        public ActionResult<GetEmailTemplateResponse> GetReplyTemplate(string orderToken)
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
                string template = EmailModel.GetReplyTemplate(orderToken);
                string signature = SignatureModel.GetSignature(executer_user).signature;
                return new GetEmailTemplateResponse(template, signature, ResponseSuccessMessage.Success, language);
            }
            catch (Exception e)
            {
                Log.Error("GetReplyTemplate endpoint - Error - " + e);
                return new GetEmailTemplateResponse(ResponseErrorMessage.InternalError, language);
            }
        }
    }
}
