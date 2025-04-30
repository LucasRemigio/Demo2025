// // Copyright (c) 2024 Engibots. All rights reserved.

using engimatrix.ModelObjs.Orquestration;
using engimatrix.ResponseMessages;

namespace engimatrix.Views.Orquestration
{
    public class TransactionsResponse
    {
        public class GetTransactions
        {
            public List<TransactionsItem> transactions { get; set; }
            public string result { get; set; }
            public int result_code { get; set; }

            public GetTransactions(List<TransactionsItem> transactions, int result_code, string language)
            {
                this.transactions = transactions;
                this.result_code = result_code;
                this.result = ResponseMessage.GetResponseMessage(result_code, language);
            }

            public GetTransactions(int result_code, string language)
            {
                this.result_code = result_code;
                this.result = ResponseMessage.GetResponseMessage(result_code, language);
            }
        }

        public class AddTransactionResponse
        {
            public string result { get; set; }
            public int result_code { get; set; }
            public string transaction_id { get; set; }

            public AddTransactionResponse(int result_code, string language, string transaction_id = "")
            {
                this.result_code = result_code;
                this.result = ResponseMessage.GetResponseMessage(result_code, language);
                this.transaction_id = transaction_id;
            }

        }
    }
}
