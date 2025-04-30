// // Copyright (c) 2024 Engibots. All rights reserved.

using engimatrix.ModelObjs;
using engimatrix.ModelObjs.Primavera;
using engimatrix.Models;
using engimatrix.ResponseMessages;

namespace engimatrix.Views
{
    public class ClientListResponse
    {
        public List<ClientDTO> clients { get; set; }
        public string result { get; set; }
        public int result_code { get; set; }

        public ClientListResponse(List<ClientDTO> clientRatings, int result_code, string language)
        {
            this.clients = clientRatings;
            this.result_code = result_code;
            this.result = ResponseMessage.GetResponseMessage(result_code, language);
        }

        public ClientListResponse(int result_code, string language)
        {
            this.result_code = result_code;
            this.result = ResponseMessage.GetResponseMessage(result_code, language);
        }
    }

    public class ClientItemResponse
    {
        public ClientDTO client { get; set; }
        public string result { get; set; }
        public int result_code { get; set; }

        public ClientItemResponse(ClientDTO clientRating, int result_code, string language)
        {
            this.client = clientRating;
            this.result_code = result_code;
            this.result = ResponseMessage.GetResponseMessage(result_code, language);
        }

        public ClientItemResponse(int result_code, string language)
        {
            this.result_code = result_code;
            this.result = ResponseMessage.GetResponseMessage(result_code, language);
        }
    }

    public class ClientPendingOrdersAndInvoicesResponse : GenericResponse
    {
        public List<PrimaveraOrderItem> orders { get; set; }
        public List<MFPrimaveraInvoiceItem> invoices { get; set; }
        public decimal orders_total { get; set; }
        public MFPrimaveraInvoiceTotalItem invoices_total { get; set; }
        public AveragePaymentTimeItem? average_payment_time { get; set; }

        public ClientPendingOrdersAndInvoicesResponse(List<PrimaveraOrderItem> orders, List<MFPrimaveraInvoiceItem> invoices, decimal orders_total, MFPrimaveraInvoiceTotalItem invoices_total, AveragePaymentTimeItem? averagePaymentTime, int result_code, string language) : base(result_code, language)
        {
            this.orders = orders;
            this.invoices = invoices;
            this.orders_total = orders_total;
            this.invoices_total = invoices_total;
            this.average_payment_time = averagePaymentTime;
        }

        public ClientPendingOrdersAndInvoicesResponse(int result_code, string language) : base(result_code, language)
        {
            this.orders = [];
            this.invoices = [];
        }

    }

    public class ClientNoAuthItemResponse : BaseResponse
    {
        public ClientNoAuthDTO? client { get; set; }

        public ClientNoAuthItemResponse(ClientNoAuthDTO client, int result_code, string language) : base(result_code, language)
        {
            this.client = client;
        }

        public ClientNoAuthItemResponse(int result_code, string language) : base(result_code, language)
        {
        }
    }
}
