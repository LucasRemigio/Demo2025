// // Copyright (c) 2024 Engibots. All rights reserved.

using System.Data;
using engimatrix.ModelObjs;
using engimatrix.ResponseMessages;
using engimatrix.Utils;
using Engimatrix.ModelObjs;

namespace engimatrix.Views
{
    public class Filtering
    {
        public class GetFilteredResponse
        {
            public List<FilteredEmail> filteredEmails { get; set; } = new List<FilteredEmail>();
            public string result { get; set; }
            public int result_code { get; set; }

            public GetFilteredResponse(List<FilteredEmail> filteredEmails, int result_code, string language)
            {
                this.filteredEmails = filteredEmails;
                this.result_code = result_code;
                this.result = ResponseMessage.GetResponseMessage(result_code, language);
            }

            public GetFilteredResponse(int result_code, string language)
            {
                this.result_code = result_code;
                this.result = ResponseMessage.GetResponseMessage(result_code, language);
            }
        }

        public class GetFilteredEmailResponse
        {
            public FilteredEmail filteredEmail { get; set; } = new FilteredEmail();
            public List<EmailAttachmentItem> emailAttachments { get; set; } = new List<EmailAttachmentItem>();
            public OrderDTO? order { get; set; }
            public string result { get; set; }
            public int result_code { get; set; }

            public GetFilteredEmailResponse(FilteredEmail filteredEmail, List<EmailAttachmentItem> emailAttachments, int result_code, string language)
            {
                this.filteredEmail = filteredEmail;
                this.emailAttachments = emailAttachments;
                this.result_code = result_code;
                this.result = ResponseMessage.GetResponseMessage(result_code, language);
            }

            public GetFilteredEmailResponse(FilteredEmail filteredEmail, List<EmailAttachmentItem> emailAttachments, OrderDTO? order, int result_code, string language)
            {
                this.filteredEmail = filteredEmail;
                this.emailAttachments = emailAttachments;
                this.order = order;
                this.result_code = result_code;
                this.result = ResponseMessage.GetResponseMessage(result_code, language);
            }

            public GetFilteredEmailResponse(int result_code, string language)
            {
                this.result_code = result_code;
                this.result = ResponseMessage.GetResponseMessage(result_code, language);
            }
        }

        public class GetFilteredEmailResponseNoAuth : BaseResponse
        {
            public FilteredEmail filteredEmail { get; set; } = new FilteredEmail();
            public List<EmailAttachmentItem> emailAttachments { get; set; } = new List<EmailAttachmentItem>();
            public OrderDTONoAuth? order { get; set; }

            public GetFilteredEmailResponseNoAuth(FilteredEmail filteredEmail, List<EmailAttachmentItem> emailAttachments, OrderDTONoAuth? order, int result_code, string language) : base(result_code, language)
            {
                this.filteredEmail = filteredEmail;
                this.emailAttachments = emailAttachments;
                this.order = order;
            }

            public GetFilteredEmailResponseNoAuth(int result_code, string language) : base(result_code, language)
            {
            }
        }

        public class GetOrderToValidateResponse : BaseResponse
        {
            public FilteredEmail filtered_email { get; set; } = new FilteredEmail();
            public List<EmailAttachmentItem> email_attachments { get; set; } = new List<EmailAttachmentItem>();
            public OrderDTO? order { get; set; }

            public GetOrderToValidateResponse(FilteredEmail filteredEmail, List<EmailAttachmentItem> emailAttachments, OrderDTO? order, int result_code, string language) : base(result_code, language)
            {
                this.filtered_email = filteredEmail;
                this.email_attachments = emailAttachments;
                this.order = order;
            }

            public GetOrderToValidateResponse(int result_code, string language) : base(result_code, language)
            {
            }
        }

        public class GetFilteredEmailDTOResponse
        {
            public FilteredEmailDTO filteredEmail { get; set; } = new();
            public List<EmailAttachmentItem> emailAttachments { get; set; } = [];
            public string result { get; set; }
            public int result_code { get; set; }

            public GetFilteredEmailDTOResponse(FilteredEmailDTO filteredEmail, List<EmailAttachmentItem> emailAttachments, int result_code, string language)
            {
                this.filteredEmail = filteredEmail;
                this.emailAttachments = emailAttachments;
                this.result_code = result_code;
                this.result = ResponseMessage.GetResponseMessage(result_code, language);
            }

            public GetFilteredEmailDTOResponse(int result_code, string language)
            {
                this.result_code = result_code;
                this.result = ResponseMessage.GetResponseMessage(result_code, language);
            }
        }

        public class GetAllCategoriesResponse
        {
            public List<CategoriesItem> categories { get; set; } = new List<CategoriesItem>();
            public string result { get; set; }
            public int result_code { get; set; }

            public GetAllCategoriesResponse(List<CategoriesItem> categories, int result_code, string language)
            {
                this.categories = categories;
                this.result_code = result_code;
                this.result = ResponseMessage.GetResponseMessage(result_code, language);
            }

            public GetAllCategoriesResponse(int result_code, string language)
            {
                this.result_code = result_code;
                this.result = ResponseMessage.GetResponseMessage(result_code, language);
            }
        }

        public class CreateUpdateResponse
        {
            public string result { get; set; }
            public int result_code { get; set; }

            public CreateUpdateResponse(int result_code, string language)
            {
                this.result_code = result_code;
                this.result = ResponseMessage.GetResponseMessage(result_code, language);
            }
        }

        public class CategorizeEmailRequest
        {
            public string category { get; set; } = "";
        }

        public class GetStatisticsResponse
        {
            public FilteringStatisticsItem statistics { get; set; } = null;
            public string result { get; set; }
            public int result_code { get; set; }

            public GetStatisticsResponse(FilteringStatisticsItem statistics, int result_code, string language)
            {
                this.statistics = statistics;
                this.result_code = result_code;
                this.result = ResponseMessage.GetResponseMessage(result_code, language);
            }

            public GetStatisticsResponse(int result_code, string language)
            {
                this.result_code = result_code;
                this.result = ResponseMessage.GetResponseMessage(result_code, language);
            }
        }

        public class GetStatusOfFilteredIfChecked
        {
            public bool is_order_checked { get; set; }
            public string result { get; set; }
            public int result_code { get; set; }

            public GetStatusOfFilteredIfChecked(bool is_order_checked, int result_code, string language)
            {
                this.is_order_checked = is_order_checked;
                this.result_code = result_code;
                this.result = ResponseMessage.GetResponseMessage(result_code, language);
            }

            public GetStatusOfFilteredIfChecked(int result_code, string language)
            {
                this.result_code = result_code;
                this.result = ResponseMessage.GetResponseMessage(result_code, language);
            }
        }

        public class FwdEmail
        {
            public List<string> email_to_list { get; set; }
            public string? message { get; set; }

            public bool Validate()
            {
                foreach (string email in email_to_list)
                {
                    if (!Util.IsValidInputEmail(email))
                    {
                        return false;
                    }
                }

                if (!string.IsNullOrEmpty(message))
                {
                    if (message.Length > 1000)
                    {
                        return false;
                    }
                }

                return true;
            }
        }

        public class ValidateRequest
        {
            public int status_id { get; set; }
            public int category_id { get; set; }
            public DateOnly? start_date { get; set; }
            public DateOnly? end_date { get; set; }
        }
    }
}
