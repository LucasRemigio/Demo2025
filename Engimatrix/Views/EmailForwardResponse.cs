// // Copyright (c) 2024 Engibots. All rights reserved.

using engimatrix.ModelObjs;
using engimatrix.Models;
using engimatrix.ResponseMessages;

namespace engimatrix.Views
{
    public class EmailForwardResponse
    {
        public List<EmailForwardItem> email_forward_list { get; set; }
        public string result { get; set; }
        public int result_code { get; set; }

        public EmailForwardResponse(List<EmailForwardItem> emailForwardList, int result_code, string language)
        {
            this.email_forward_list = emailForwardList;
            this.result_code = result_code;
            this.result = ResponseMessage.GetResponseMessage(result_code, language);
        }

        public EmailForwardResponse(int result_code, string language)
        {
            this.result_code = result_code;
            this.result = ResponseMessage.GetResponseMessage(result_code, language);
        }
    }

    public class EmailRecipientsForwardResponse
    {
        public List<string> email_forward_list { get; set; }
        public string result { get; set; }
        public int result_code { get; set; }

        public EmailRecipientsForwardResponse(List<string> emailForwardList, int result_code, string language)
        {
            this.email_forward_list = emailForwardList;
            this.result_code = result_code;
            this.result = ResponseMessage.GetResponseMessage(result_code, language);
        }

        public EmailRecipientsForwardResponse(int result_code, string language)
        {
            this.email_forward_list = new List<string>();
            this.result_code = result_code;
            this.result = ResponseMessage.GetResponseMessage(result_code, language);
        }
    }
}
