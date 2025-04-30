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
    using Engimatrix.Views;
    using Engimatrix.ModelObjs;
    using System.Text.Json;
    using engimatrix.Connector;
    using System.Linq.Expressions;
    using static Engimatrix.Views.EmailTemplateResponse;

    [ApiController]
    [Route("api/emails")]
    public class EmailController : ControllerBase
    {
        [HttpGet]
        [Route("{id}")]
        [RequestLimit]
        [ValidateReferrer]
        [Authorize]
        public ActionResult<Emails.GetEmailResponse> getEmailDetails(string id)
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
                EmailItem emailItem = EmailModel.getEmail(id, executer_user);
                List<EmailAttachmentItem> attachmentItems = AttachmentModel.getAttachments("", emailItem.id);
                return new Emails.GetEmailResponse(emailItem, attachmentItems, ResponseSuccessMessage.Success, language);
            }
            catch (Exception e)
            {
                Log.Error("GetEmailDetails endpoint - Error - " + e);
                return new Emails.GetEmailResponse(ResponseErrorMessage.InternalError, language);
            }
        }

        [HttpGet]
        [Route("getAllAttachments")]
        [RequestLimit]
        [ValidateReferrer]
        [Authorize]
        public ActionResult<Emails.GetEmailResponse> getAllAttachments()
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
                List<EmailAttachmentItem> attachments = AttachmentModel.GetAllAttachments(executer_user);
                return new Emails.GetEmailResponse(attachments, ResponseSuccessMessage.Success, language);
            }
            catch (Exception e)
            {
                Log.Error("getAllAttachments endpoint - Error - " + e);
                return new Emails.GetEmailResponse(ResponseErrorMessage.InternalError, language);
            }
        }

        [HttpPost]
        [Route("")]
        [RequestLimit]
        [ValidateReferrer]
        [Authorize]
        public async Task<ActionResult<Emails.GetEmailResponse>> CreateEmail(EmailRequest email)
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
                await MasterFerro.SendEmailAsync(email, email.attachments, email.mailbox);
            }
            catch (Exception e)
            {
                Log.Error("CreateEmail endpoint - Error Sending Email - " + e);
                return new Emails.GetEmailResponse(ResponseErrorMessage.ErrorSendingEmail, language);
            }

            try
            {
                EmailItem emailToSave = new(String.Empty, email.mailbox, email.to, email.cc, email.bcc, email.subject, email.body, DateTime.Now);
                await EmailModel.CreateEmailAndSaveAttachments(emailToSave, email.attachments, executer_user);

                return new Emails.GetEmailResponse(ResponseSuccessMessage.Success, language);
            }
            catch (InvalidOperationException e)
            {
                Log.Error("CreateEmail endpoint - Error Saving Email - " + e);
                return new Emails.GetEmailResponse(ResponseErrorMessage.ErrorSavingEmail, language);
            }
            catch (AttachmentNotValidException e)
            {
                Log.Error("CreateEmail endpoint - Error Saving Attachments - " + e);
                return new Emails.GetEmailResponse(ResponseErrorMessage.ErrorSavingAttachment, language);
            }
            catch (Exception e)
            {
                Log.Error("CreateEmail endpoint - Error saving email or attachment - " + e);
                return new Emails.GetEmailResponse(ResponseErrorMessage.InternalError, language);
            }
        }

        [HttpGet]
        [Route("mailboxes")]
        [RequestLimit]
        [Authorize]
        public ActionResult<Emails.GetMailboxesResponse> GetConfigMailboxes()
        {
            string language = this.Request.Headers["client-lang"];
            if (string.IsNullOrEmpty(language))
            {
                language = ConfigManager.defaultLanguage;
            }
            try
            {
                // Retrieve config manager mailboxes
                return new Emails.GetMailboxesResponse(EmailModel.GetMailboxes(), ResponseSuccessMessage.Success, language);
            }
            catch (Exception e)
            {
                Log.Error("GetMailboxes endpoint - Error - " + e);
                return new Emails.GetMailboxesResponse(ResponseErrorMessage.InternalError, language);
            }
        }

        [HttpPatch]
        [Route("{emailId}/destinatary")]
        [RequestLimit]
        [ValidateReferrer]
        [Authorize]
        public ActionResult<Emails.GetEmailResponse> UpdateEmailDestinatary(Emails.ChangeDestinataryRequest destinataryReq, string emailId)
        {
            string language = this.Request.Headers["client-lang"];
            if (string.IsNullOrEmpty(language))
            {
                language = ConfigManager.defaultLanguage;
            }

            if (!destinataryReq.IsValid())
            {
                return new Emails.GetEmailResponse(ResponseErrorMessage.EmailNotFound, language);
            }
            string token = this.Request.Headers["Authorization"];
            string executer_user = UserModel.GetUserByToken(token);

            try
            {
                EmailModel.PatchEmailDestinatary(destinataryReq.destinatary, emailId, executer_user);
                return new Emails.GetEmailResponse(ResponseSuccessMessage.Success, language);
            }
            catch (Exception e)
            {
                Log.Error("Filtered endpoint - Error - " + e);
                return new Emails.GetEmailResponse(ResponseErrorMessage.InternalError, language);
            }
        }

        [HttpGet]
        [Route("sent")]
        [RequestLimit]
        [Authorize]
        public ActionResult<Emails.GetEmailListResponse> GetSentEmailsFromPlatform(string? start_date, string? end_date)
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
                // Retrieve config manager mailboxes
                return new Emails.GetEmailListResponse(EmailModel.GetEmailsCreatedInPlatform(start_date, end_date, executer_user), ResponseSuccessMessage.Success, language);
            }
            catch (Exception e)
            {
                Log.Error("GetMailboxes endpoint - Error - " + e);
                return new Emails.GetEmailListResponse(ResponseErrorMessage.InternalError, language);
            }
        }

        [HttpGet]
        [Route("recipients")]
        [RequestLimit]
        [Authorize]
        public ActionResult<Emails.GetAddresses> GetRecipientsSentFromPlatform()
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
                // Retrieve config manager mailboxes
                List<string> addresses = EmailModel.GetSentEmailAddressesList(executer_user);
                return new Emails.GetAddresses(addresses, ResponseSuccessMessage.Success, language);
            }
            catch (Exception e)
            {
                Log.Error("GetMailboxes endpoint - Error - " + e);
                return new Emails.GetAddresses(ResponseErrorMessage.InternalError, language);
            }
        }

        [HttpGet]
        [Route("blacklisted-spam")]
        [RequestLimit]
        [ValidateReferrer]
        [Authorize]
        public ActionResult<Emails.GetAddresses> GetBlacklistedDomains()
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
                List<string> domains = EmailModel.GetSpamBlacklistedDomains(executer_user);
                return new Emails.GetAddresses(domains, ResponseSuccessMessage.Success, language);
            }
            catch (Exception e)
            {
                Log.Error("GetBlacklistedDomains endpoint - Error - " + e);
                return new Emails.GetAddresses(ResponseErrorMessage.InternalError, language);
            }
        }

    }
}
