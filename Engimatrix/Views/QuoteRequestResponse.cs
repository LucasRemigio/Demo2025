// // Copyright (c) 2024 Engibots. All rights reserved.

using engimatrix.ModelObjs;
using engimatrix.Models;
using engimatrix.ResponseMessages;

namespace engimatrix.Views
{
    public class QuoteRequestListResponse
    {
        public List<QuoteRequestItem> quote_requests { get; set; }
        public string result { get; set; }
        public int result_code { get; set; }

        public QuoteRequestListResponse(List<QuoteRequestItem> quoteRequests, int result_code, string language)
        {
            this.quote_requests = quoteRequests;
            this.result_code = result_code;
            this.result = ResponseMessage.GetResponseMessage(result_code, language);
        }

        public QuoteRequestListResponse(int result_code, string language)
        {
            this.quote_requests = [];
            this.result_code = result_code;
            this.result = ResponseMessage.GetResponseMessage(result_code, language);
        }
    }

    public class QuoteRequestItemResponse
    {
        public QuoteRequestItem quote_request { get; set; }
        public string result { get; set; }
        public int result_code { get; set; }

        public QuoteRequestItemResponse(QuoteRequestItem quoteRequest, int result_code, string language)
        {
            this.quote_request = quoteRequest;
            this.result_code = result_code;
            this.result = ResponseMessage.GetResponseMessage(result_code, language);
        }

        public QuoteRequestItemResponse(int result_code, string language)
        {
            this.quote_request = new();
            this.result_code = result_code;
            this.result = ResponseMessage.GetResponseMessage(result_code, language);
        }
    }
}
