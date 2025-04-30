// // Copyright (c) 2024 Engibots. All rights reserved.

using engimatrix.Utils;

namespace engimatrix.Views.Orquestration
{
    public class TransactionsRequest
    {
        public class AddTransaction
        {
           
            public string queue_id { get; set; }
            public string input_data { get; set; }
        }

        public class EditTransaction
        {
            public int id { get; set; }
            public string? status_id { get; set; }
            public string? started { get; set; }
            public string? ended { get; set; }
            public string? exception { get; set; }
            public string? output_data { get; set; }
        }
    }
}
