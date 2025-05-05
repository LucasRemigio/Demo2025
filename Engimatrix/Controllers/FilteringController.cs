

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
    using Microsoft.IdentityModel.Tokens;
    using static engimatrix.Views.Filtering;

    [ApiController]
    [Route("api/filtering")]
    public class FilteringController : ControllerBase
    {
        [HttpGet]
        [Route("filtered")]
        [RequestLimit]
        [ValidateReferrer]
        [Authorize]
        public async Task<ActionResult<GetFilteredResponse>> getFilteredEmails(string? start_date, string? end_date, string? categoryId, int? statusId)
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
                bool showOnlyNonResolved = false;
                if (statusId == StatusConstants.StatusCode.RESOLVIDO_MANUALMENTE)
                {
                    showOnlyNonResolved = true;
                }
                List<FilteredEmail> filteredEmails = await FilteringModel.GetFiltered(start_date, end_date, categoryId, showOnlyNonResolved, executer_user, []);
                return new GetFilteredResponse(filteredEmails, ResponseSuccessMessage.Success, language);
            }
            catch (Exception e)
            {
                Log.Error("getFilteredEmails endpoint - Error - " + e);
                return new GetFilteredResponse(ResponseErrorMessage.InternalError, language);
            }
        }

        [HttpGet]
        [Route("{emailToken}")]
        [RequestLimit]
        [ValidateReferrer]
        [Authorize]
        public ActionResult<GetFilteredEmailResponse> getFilteredEmailByToken(string emailToken)
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
                FilteredEmail filteredEmail = FilteringModel.getFilteredEmail(executer_user, emailToken, true);
                List<EmailAttachmentItem> attachmentItems = AttachmentModel.getAttachments(executer_user, filteredEmail.email.id);

                return new GetFilteredEmailResponse(filteredEmail, attachmentItems, ResponseSuccessMessage.Success, language);
            }
            catch (Exception e)
            {
                Log.Error("getFilteredEmailByToken endpoint - Error - " + e);
                return new GetFilteredEmailResponse(ResponseErrorMessage.InternalError, language);
            }
        }

        [HttpGet]
        [Route("validate")]
        [RequestLimit]
        [ValidateReferrer]
        [Authorize]
        public ActionResult<GetFilteredResponse> getToValidateEmails([FromQuery] ValidateRequest req)
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
                List<FilteredEmail> filteredEmails = FilteringModel.getToValidate(executer_user, req.status_id, req.category_id, req.start_date, req.end_date);
                return new GetFilteredResponse(filteredEmails, ResponseSuccessMessage.Success, language);
            }
            catch (Exception e)
            {
                Log.Error("getToValidateEmails endpoint - Error - " + e);
                return new GetFilteredResponse(ResponseErrorMessage.InternalError, language);
            }
        }

        [HttpGet]
        [Route("validate/orders")]
        [RequestLimit]
        [ValidateReferrer]
        [Authorize]
        public ActionResult<GetFilteredResponse> getToValidateOrders([FromQuery] int category_id, [FromQuery] DateOnly start_date, [FromQuery] DateOnly end_date)
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
                List<FilteredEmail> filteredEmails = FilteringModel.GetToValidateOrders(executer_user, category_id, start_date, end_date);
                return new GetFilteredResponse(filteredEmails, ResponseSuccessMessage.Success, language);
            }
            catch (Exception e)
            {
                Log.Error("getToValidateEmails endpoint - Error - " + e);
                return new GetFilteredResponse(ResponseErrorMessage.InternalError, language);
            }
        }

        [HttpGet]
        [Route("validate/pending-client")]
        [RequestLimit]
        [ValidateReferrer]
        [Authorize]
        public ActionResult<GetFilteredResponse> GetToValidatePendingClient([FromQuery] DateOnly start_date, [FromQuery] DateOnly end_date)
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
                List<FilteredEmail> filteredEmails = FilteringModel.GetToValidatePendingClient(executer_user, start_date, end_date);
                return new GetFilteredResponse(filteredEmails, ResponseSuccessMessage.Success, language);
            }
            catch (Exception e)
            {
                Log.Error("getToValidateEmails endpoint - Error - " + e);
                return new GetFilteredResponse(ResponseErrorMessage.InternalError, language);
            }
        }

        [HttpGet]
        [Route("validate/category/{id}")]
        [RequestLimit]
        [ValidateReferrer]
        [Authorize]
        public ActionResult<GetFilteredEmailResponse> GetToValidateCategoryEmail(string id)
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
                FilteredEmail filteredEmail = FilteringModel.getFilteredEmail(executer_user, id, false);

                if (filteredEmail.IsEmpty())
                {
                    throw new InputNotValidException("Email not found");
                }

                List<EmailAttachmentItem> attachmentItems = AttachmentModel.getAttachments(executer_user, id);

                return new GetFilteredEmailResponse(filteredEmail, attachmentItems, null, ResponseSuccessMessage.Success, language);
            }
            catch (DatabaseException e)
            {
                Log.Error("GetToValidateEmail endpoint - Database Error - " + e);
                return new GetFilteredEmailResponse(ResponseErrorMessage.DatabaseQueryError, language);
            }
            catch (InputNotValidException e)
            {
                Log.Error("GetToValidateEmail endpoint - Input not valid Error - " + e);
                return new GetFilteredEmailResponse(ResponseErrorMessage.InvalidArgs, language);
            }
            catch (EmailProcessingException e)
            {
                Log.Error("GetToValidateEmail endpoint - Email Processing Error - " + e);
                return new GetFilteredEmailResponse(ResponseErrorMessage.EmailProcessing, language);
            }
            catch (Exception e)
            {
                Log.Error("GetToValidateEmail endpoint - Error - " + e);
                return new GetFilteredEmailResponse(ResponseErrorMessage.InternalError, language);
            }
        }

        [HttpGet]
        [Route("validate/{id}")]
        [RequestLimit]
        [ValidateReferrer]
        [Authorize]
        public ActionResult<GetFilteredEmailResponse> GetToValidateEmail(string id)
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
                FilteredEmail filteredEmail = FilteringModel.getFilteredEmail(executer_user, id, false);

                if (filteredEmail.IsEmpty())
                {
                    throw new InputNotValidException("Email not found");
                }

                List<EmailAttachmentItem> attachmentItems = AttachmentModel.getAttachments(executer_user, id);

                /*
                * WARNING: This line should be commented out in production: We are still not creating orders from emails
                */
                OrderDTO? order = OrderModel.GetOrderDTOByEmailToken(filteredEmail.token, executer_user);

                return new GetFilteredEmailResponse(filteredEmail, attachmentItems, order, ResponseSuccessMessage.Success, language);
            }
            catch (DatabaseException e)
            {
                Log.Error("GetToValidateEmail endpoint - Database Error - " + e);
                return new GetFilteredEmailResponse(ResponseErrorMessage.DatabaseQueryError, language);
            }
            catch (InputNotValidException e)
            {
                Log.Error("GetToValidateEmail endpoint - Input not valid Error - " + e);
                return new GetFilteredEmailResponse(ResponseErrorMessage.InvalidArgs, language);
            }
            catch (EmailProcessingException e)
            {
                Log.Error("GetToValidateEmail endpoint - Email Processing Error - " + e);
                return new GetFilteredEmailResponse(ResponseErrorMessage.EmailProcessing, language);
            }
            catch (Exception e)
            {
                Log.Error("GetToValidateEmail endpoint - Error - " + e);
                return new GetFilteredEmailResponse(ResponseErrorMessage.InternalError, language);
            }
        }

        [HttpGet]
        [Route("validate/noAuth/{token}")]
        [RequestLimit]
        [ValidateReferrer]
        public ActionResult<GetFilteredEmailResponseNoAuth> GetToValidateEmailNoAuth(string token)
        {
            string language = this.Request.Headers["client-lang"];
            if (string.IsNullOrEmpty(language))
            {
                language = ConfigManager.defaultLanguage;
            }
            string authToken = this.Request.Headers["Authorization"];
            string executer_user = "Anonymous";
            if (!string.IsNullOrEmpty(authToken))
            {
                executer_user = UserModel.GetUserByToken(authToken);
            }
            try
            {
                FilteredEmail filteredEmail = FilteringModel.getFilteredEmail(executer_user, token, true);

                if (filteredEmail.IsEmpty())
                {
                    return new GetFilteredEmailResponseNoAuth(ResponseErrorMessage.EmailNotFound, language);
                }

                List<EmailAttachmentItem> attachmentItems = AttachmentModel.getAttachments(executer_user, filteredEmail.email.id);

                OrderDTONoAuth? order = OrderModel.GetOrderDTOByTokenNoAuth(filteredEmail.token, OrderConstants.TokenType.EmailToken, executer_user);

                return new GetFilteredEmailResponseNoAuth(filteredEmail, attachmentItems, order, ResponseSuccessMessage.Success, language);
            }
            catch (Exception e)
            {
                Log.Error("GetToValidateEmail endpoint - Error - " + e);
                return new GetFilteredEmailResponseNoAuth(ResponseErrorMessage.InternalError, language);
            }
        }

        [HttpGet]
        [Route("validate/dto/{id}")]
        [RequestLimit]
        [ValidateReferrer]
        [Authorize]
        public ActionResult<GetFilteredEmailDTOResponse> GetToValidateEmailDTO(string id)
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
                FilteredEmailDTO filteredEmail = FilteringModel.getFilteredEmailDTO(executer_user, id);

                if (filteredEmail.IsEmpty())
                {
                    return new GetFilteredEmailDTOResponse(ResponseErrorMessage.EmailNotFound, language);
                }

                List<EmailAttachmentItem> attachmentItems = AttachmentModel.getAttachments(executer_user, id);

                return new GetFilteredEmailDTOResponse(filteredEmail, attachmentItems, ResponseSuccessMessage.Success, language);
            }
            catch (Exception e)
            {
                Log.Error("GetToValidateEmailDTO endpoint - Error - " + e);
                return new GetFilteredEmailDTOResponse(ResponseErrorMessage.InternalError, language);
            }
        }

        [HttpGet]
        [Route("categories")]
        [RequestLimit]
        [ValidateReferrer]
        [Authorize]
        public ActionResult<GetAllCategoriesResponse> getCategories()
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
                List<CategoriesItem> categories = FilteringModel.getCategories(executer_user);

                return new GetAllCategoriesResponse(categories, ResponseSuccessMessage.Success, language);
            }
            catch (Exception e)
            {
                Log.Error("getCategories endpoint - Error - " + e);
                return new GetAllCategoriesResponse(ResponseErrorMessage.InternalError, language);
            }
        }

        [HttpPost]
        [Route("categorize/{id}")]
        [RequestLimit]
        [ValidateReferrer]
        [Authorize]
        public ActionResult<CreateUpdateResponse> postCategorizedEmail(string id, [FromBody] JsonElement payload)
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
                string categoryId = payload.GetProperty("category").ToString();
                FilteringModel.updateFilteredEmail(executer_user, id, Int32.Parse(categoryId));

                return new CreateUpdateResponse(ResponseSuccessMessage.Success, language);
            }
            catch (Exception e)
            {
                Log.Error("postCategorizedEmail endpoint - Error - " + e);
                return new CreateUpdateResponse(ResponseErrorMessage.InternalError, language);
            }
        }

        [HttpPost]
        [Route("changeCategory/{emailId}")]
        [RequestLimit]
        [ValidateReferrer]
        [Authorize]
        public async Task<ActionResult<CreateUpdateResponse>> ChangeEmailCategory(string emailId, [FromBody] JsonElement payload)
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
                string categoryId = payload.GetProperty("categoryId").ToString();

                await FilteringModel.ChangeFilteredEmailCategory(executer_user, emailId, false, Int32.Parse(categoryId));

                return new CreateUpdateResponse(ResponseSuccessMessage.Success, language);
            }
            catch (Exception e)
            {
                Log.Error("ChangeEmailCategory endpoint - Error - " + e);
                return new CreateUpdateResponse(ResponseErrorMessage.InternalError, language);
            }
        }

        [HttpPatch]
        [Route("changeStatus")]
        [RequestLimit]
        [ValidateReferrer]
        [Authorize]
        public ActionResult<CreateUpdateResponse> ChangeEmailStatus(PatchFilteredStatusRequest statusRequest)
        {
            string language = this.Request.Headers["client-lang"];
            if (string.IsNullOrEmpty(language))
            {
                language = ConfigManager.defaultLanguage;
            }

            if (!statusRequest.Validate())
            {
                return new CreateUpdateResponse(ResponseErrorMessage.InvalidArgs, language);
            }

            string token = this.Request.Headers["Authorization"];
            string executer_user = UserModel.GetUserByToken(token);

            try
            {
                FilteringModel.ChangeEmailStatus(statusRequest.email_token, statusRequest.status_id.ToString(), executer_user);

                return new CreateUpdateResponse(ResponseSuccessMessage.Success, language);
            }
            catch (Exception e)
            {
                Log.Error("ChangeEmailStatus endpoint - Error - " + e);
                return new CreateUpdateResponse(ResponseErrorMessage.InternalError, language);
            }
        }

        [HttpPatch]
        [Route("no-auth/change-status/{filteredToken}")]
        [RequestLimit]
        [ValidateReferrer]
        public ActionResult<CreateUpdateResponse> ChangeEmailStatusToConfirmedByClient(string filteredToken)
        {
            string language = this.Request.Headers["client-lang"];
            if (string.IsNullOrEmpty(language))
            {
                language = ConfigManager.defaultLanguage;
            }

            string token = this.Request.Headers["Authorization"];

            string executer_user = "Anonymous Client";
            if (!String.IsNullOrEmpty(token))
            {
                executer_user = UserModel.GetUserByToken(token);
            }

            try
            {
                FilteringModel.ChangeEmailStatus(filteredToken, StatusConstants.StatusCode.CONFIRMADO_POR_CLIENTE.ToString(), executer_user);

                return new CreateUpdateResponse(ResponseSuccessMessage.Success, language);
            }
            catch (Exception e)
            {
                Log.Error("ChangeEmailStatus endpoint - Error - " + e);
                return new CreateUpdateResponse(ResponseErrorMessage.InternalError, language);
            }
        }

        [HttpGet]
        [Route("stats")]
        [RequestLimit]
        [ValidateReferrer]
        [Authorize]
        public async Task<ActionResult<GetStatisticsResponse>> getStatistics(string? from, string? to, int? categoryId)
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
                FilteringStatisticsItem stats = await FilteringModel.getStatistics(executer_user, from, to, categoryId);

                return new GetStatisticsResponse(stats, ResponseSuccessMessage.Success, language);
            }
            catch (Exception e)
            {
                Log.Error("getStatistics endpoint - Error - " + e);
                return new GetStatisticsResponse(ResponseErrorMessage.InternalError, language);
            }
        }

        [HttpGet]
        [Route("{filteredToken}/checkConfirmed")]
        [RequestLimit]
        [ValidateReferrer]
        public ActionResult<GetStatusOfFilteredIfChecked> CheckIfOrderIsConfirmed(string filteredToken)
        {
            string language = this.Request.Headers["client-lang"];
            if (string.IsNullOrEmpty(language))
            {
                language = ConfigManager.defaultLanguage;
            }

            string token = this.Request.Headers["Authorization"];

            string executer_user = "Anonymous Client";
            if (!String.IsNullOrEmpty(token))
            {
                executer_user = UserModel.GetUserByToken(token);
            }

            try
            {
                bool isChecked = FilteringModel.CheckOrderIsChecked(filteredToken, executer_user);

                return new GetStatusOfFilteredIfChecked(isChecked, ResponseSuccessMessage.Success, language);
            }
            catch (Exception e)
            {
                Log.Error("Filtered CheckIfOrderIsConfirmed endpoint - Error - " + e);
                return new GetStatusOfFilteredIfChecked(ResponseErrorMessage.InternalError, language);
            }
        }

        [HttpPatch]
        [Route("{filteredToken}/mark-spam")]
        [RequestLimit]
        [ValidateReferrer]
        [Authorize]
        public async Task<ActionResult<BaseResponse>> MarkEmailAsSpam(string filteredToken)
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
                await FilteringModel.MarkEmailAsSpam(filteredToken, executer_user);

                return new BaseResponse(ResponseSuccessMessage.Success, language);
            }
            catch (Exception e)
            {
                Log.Error("Filtered CheckIfOrderIsConfirmed endpoint - Error - " + e);
                return new BaseResponse(ResponseErrorMessage.InternalError, language);
            }
        }

        [HttpPatch]
        [Route("{filteredToken}/set-administration-pending")]
        [RequestLimit]
        [ValidateReferrer]
        [Authorize]
        public ActionResult<BaseResponse> SetEmailAsAdministrationPending(string filteredToken)
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
                FilteringModel.SetEmailAsAdministrationPending(filteredToken, executer_user);

                return new BaseResponse(ResponseSuccessMessage.Success, language);
            }
            catch (Exception e)
            {
                Log.Error("Filtered CheckIfOrderIsConfirmed endpoint - Error - " + e);
                return new BaseResponse(ResponseErrorMessage.InternalError, language);
            }
        }


        [HttpPost]
        [Route("generate-audit-email")]
        [RequestLimit]
        [ValidateReferrer]
        [Authorize]
        public async Task<ActionResult<BaseResponse>> GenerateAuditEmail([FromBody] GenerateAuditEmailRequest req)
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
                await FilteringModel.GenerateAuditEmail(req.body);

                return new BaseResponse(ResponseSuccessMessage.Success, language);
            }
            catch (Exception e)
            {
                Log.Error("Filtered CheckIfOrderIsConfirmed endpoint - Error - " + e);
                return new BaseResponse(ResponseErrorMessage.InternalError, language);
            }
        }


    }
}
