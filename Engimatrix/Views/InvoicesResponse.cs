// // Copyright (c) 2024 Engibots. All rights reserved.

using engimatrix.ModelObjs;
using engimatrix.ResponseMessages;

namespace engimatrix.Views
{
    public class InvoicesResponse
    {
        public class GetAllInvoicesResponse
        {
            public List<InvoiceItem> invoices { get; set; }
            public string result { get; set; }
            public int result_code { get; set; }

            public GetAllInvoicesResponse(List<InvoiceItem> invoices, int result_code, string language)
            {
                this.invoices = invoices;
                this.result_code = result_code;
                this.result = ResponseMessage.GetResponseMessage(result_code, language);
            }

            public GetAllInvoicesResponse(int result_code, string language)
            {
                this.result_code = result_code;
                this.result = ResponseMessage.GetResponseMessage(result_code, language);
            }
        }

        public class GetInvoiceResponse
        {
            public InvoiceItem invoice { get; set; }
            public string result { get; set; }
            public int result_code { get; set; }

            public GetInvoiceResponse(InvoiceItem invoice, int result_code, string language)
            {
                this.invoice = invoice;
                this.result_code = result_code;
                this.result = ResponseMessage.GetResponseMessage(result_code, language);
            }

            public GetInvoiceResponse(int result_code, string language)
            {
                this.result_code = result_code;
                this.result = ResponseMessage.GetResponseMessage(result_code, language);
            }
        }
    }
}
