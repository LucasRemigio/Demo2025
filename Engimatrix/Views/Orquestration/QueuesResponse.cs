// // Copyright (c) 2024 Engibots. All rights reserved.

using engimatrix.ModelObjs.Orquestration;
using engimatrix.ResponseMessages;

namespace engimatrix.Views.Orquestration
{
    public class QueuesResponse
    {
        public class GetQueues
        {
            public List<QueuesItem> queues { get; set; }
            public List<TransactionsItem> transactions { get; set; }
            public string result { get; set; }
            public int result_code { get; set; }

            public GetQueues(List<QueuesItem> queues, List<TransactionsItem> transactions, int result_code, string language)
            {
                this.queues = queues;
                this.transactions = transactions;
                this.result_code = result_code;
                this.result = ResponseMessage.GetResponseMessage(result_code, language);
            }

            public GetQueues(int result_code, string language)
            {
                this.result_code = result_code;
                this.result = ResponseMessage.GetResponseMessage(result_code, language);
            }
        }
    }
}
