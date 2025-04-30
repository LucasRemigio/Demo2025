// // Copyright (c) 2024 Engibots. All rights reserved.

using engimatrix.ModelObjs;
using engimatrix.ModelObjs.Primavera;
using engimatrix.ResponseMessages;

namespace engimatrix.Views
{
    public class ClientPrimaveraOrdersResponse
    {
        public decimal total { get; set; }
        public List<PrimaveraOrderItem> primavera_orders { get; set; }
        public string result { get; set; }
        public int result_code { get; set; }

        public ClientPrimaveraOrdersResponse(List<PrimaveraOrderItem> primaveraOrders, decimal total, int result_code, string language)
        {
            this.primavera_orders = primaveraOrders;
            this.total = total;
            this.result_code = result_code;
            this.result = ResponseMessage.GetResponseMessage(result_code, language);
        }

        public ClientPrimaveraOrdersResponse(decimal total, int result_code, string language)
        {
            this.total = total;
            this.result_code = result_code;
            this.result = ResponseMessage.GetResponseMessage(result_code, language);
        }
         
        public ClientPrimaveraOrdersResponse(int result_code, string language)
        {
            this.result_code = result_code;
            this.result = ResponseMessage.GetResponseMessage(result_code, language);
        }
    }
}
