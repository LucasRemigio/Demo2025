// // Copyright (c) 2024 Engibots. All rights reserved.

namespace engimatrix.ModelObjs.Orquestration
{
    public class TransactionsItem
    {
        public int id { get; set; }
        public string status_id { get; set; }
        public string reference { get; set; }
        public string started { get; set; }
        public string ended { get; set; }
        public string exception { get; set; }
        public int queue_id { get; set; }
        public string input_data {  get; set; }
        public string output_data { get; set; }
        public int number_retry_queue {  get; set; }
        public string script_name { get; set; }

        public TransactionsItem(int id, string status_id, string reference, string started, string ended, string exception, int queue_id, string input_data, string output_data, int number_retry_queue, string script_name = null)
        {
            this.id = id;
            this.status_id = status_id;
            this.reference = reference;
            this.started = started;
            this.ended = ended;
            this.exception = exception;
            this.queue_id = queue_id;
            this.input_data = input_data;
            this.output_data = output_data;
            this.number_retry_queue = number_retry_queue;
            this.script_name = script_name;
        }

        public TransactionsItem ToItem()
        {
            return new TransactionsItem(this.id, this.status_id, this.reference, this.started, this.ended, this.exception, this.queue_id, this.input_data, this.output_data, this.number_retry_queue, this.script_name);
        }

    }
}
