// // Copyright (c) 2024 Engibots. All rights reserved.

using Engimatrix.ModelObjs;
using engimatrix.ResponseMessages;
using Microsoft.IdentityModel.Tokens;
using engimatrix.Utils;
using engimatrix.Views;

namespace Engimatrix.Views
{
    public class Emails
    {
        public class GetEmailResponse
        {
            public EmailItem email { get; set; }
            public List<EmailAttachmentItem> attachments { get; set; }
            public string result { get; set; }
            public int result_code { get; set; }


            public GetEmailResponse(EmailItem email, List<EmailAttachmentItem> attachs, int result_code, string language)
            {
                this.email = email;
                this.attachments = attachs;
                this.result_code = result_code;
                this.result = ResponseMessage.GetResponseMessage(result_code, language);
            }
            public GetEmailResponse(EmailItem email, int result_code, string language)
            {
                this.email = email;
                this.result_code = result_code;
                this.result = ResponseMessage.GetResponseMessage(result_code, language);
            }

            public GetEmailResponse(List<EmailAttachmentItem> attachs, int result_code, string language)
            {
                this.attachments = attachs;
                this.result_code = result_code;
                this.result = ResponseMessage.GetResponseMessage(result_code, language);
            }

            public GetEmailResponse(int result_code, string language)
            {
                this.result_code = result_code;
                this.result = ResponseMessage.GetResponseMessage(result_code, language);
            }

        }

        public class GetEmailListResponse
        {
            public List<EmailItem> emails { get; set; }

            public List<EmailAttachmentItem> attachments { get; set; }
            public string result { get; set; }
            public int result_code { get; set; }


            public GetEmailListResponse(List<EmailItem> email, List<EmailAttachmentItem> attachs, int result_code, string language)
            {
                this.emails = email;
                this.attachments = attachs;
                this.result_code = result_code;
                this.result = ResponseMessage.GetResponseMessage(result_code, language);
            }
            public GetEmailListResponse(List<EmailItem> email, int result_code, string language)
            {
                this.emails = email;
                this.result_code = result_code;
                this.result = ResponseMessage.GetResponseMessage(result_code, language);
            }

            public GetEmailListResponse(int result_code, string language)
            {
                this.result_code = result_code;
                this.result = ResponseMessage.GetResponseMessage(result_code, language);
            }

        }

        public class GetMailboxesResponse
        {
            public List<string> mailboxes { get; set; }
            public string result { get; set; }
            public int result_code { get; set; }


            public GetMailboxesResponse(List<string> mailboxes, int result_code, string language)
            {
                this.mailboxes = mailboxes;
                this.result_code = result_code;
                this.result = ResponseMessage.GetResponseMessage(result_code, language);
            }

            public GetMailboxesResponse(int result_code, string language)
            {
                this.result_code = result_code;
                this.result = ResponseMessage.GetResponseMessage(result_code, language);
            }
        }

        public class ChangeDestinataryRequest
        {
            public required string destinatary { get; set; }

            public bool IsValid()
            {
                if (!Util.IsValidInputEmail(destinatary))
                {
                    return false;
                }

                return true;
            }
        }

        public class GetAddresses : BaseResponse
        {
            public List<string> addresses { get; set; }

            public GetAddresses(List<string> addresses, int result_code, string language) : base(result_code, language)
            {
                this.addresses = addresses;
            }

            public GetAddresses(int result_code, string language) : base(result_code, language)
            {
                addresses = [];
            }
        }
    }
}

