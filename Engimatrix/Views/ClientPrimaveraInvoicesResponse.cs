// // Copyright (c) 2024 Engibots. All rights reserved.

using engimatrix.ModelObjs;
using engimatrix.ModelObjs.Primavera;
using engimatrix.ResponseMessages;
using Engimatrix.Controllers;

namespace engimatrix.Views
{
    public class ClientPrimaveraInvoicesResponse : BaseResponse
    {
        public AveragePaymentTimeItem? average_payment_time { get; set; }
        public List<MFPrimaveraInvoiceItem>? primavera_invoices { get; set; }
        public MFPrimaveraInvoiceTotalItem? invoices_total { get; set; }

        public ClientPrimaveraInvoicesResponse(List<MFPrimaveraInvoiceItem>? primaveraInvoices, AveragePaymentTimeItem? averagePaymentTime, MFPrimaveraInvoiceTotalItem? invoicesTotal, int result_code, string language)
            : base(result_code, language)
        {
            this.primavera_invoices = primaveraInvoices;
            this.average_payment_time = averagePaymentTime;
            this.invoices_total = invoicesTotal;
        }

        public ClientPrimaveraInvoicesResponse(int result_code, string language)
            : base(result_code, language)
        {
        }
    }
}
