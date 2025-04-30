// // Copyright (c) 2024 Engibots. All rights reserved.

namespace engimatrix.ModelObjs.Orquestration
{
    public class QueuesItem
    {
        public int id { get; set; }
        public string name { get; set; }
        public string description { get; set; }
        public string autoRetry { get; set; }
        public int numberRetry { get; set; }
        public int status_id { get; set; }
        public string script_name { get; set; }
        public string time { get; set; }
        public string successCount { get; set; }
        public string insuccessCount { get; set; }
        public string sysException { get; set; }
        public string busException { get; set; }
        public List<TransactionsItem> Transactions { get; set; }

        public QueuesItem(int id, string name, string description, string autoRetry, int numberRetry, int status_id, string script_name, string time, string successCount, string insuccessCount, string sysException, string busException, List<TransactionsItem> transactions)
        {
            this.id = id;
            this.name = name;
            this.description = description;
            this.autoRetry = autoRetry;
            this.numberRetry = numberRetry;
            this.status_id = status_id;
            this.script_name = script_name;
            this.time = time;
            this.successCount = successCount;
            this.insuccessCount = insuccessCount;
            this.sysException = sysException;
            this.busException = busException;
            this.Transactions = transactions;

        }

        public QueuesItem ToItem()
        {
            return new QueuesItem(this.id, this.name, this.description, this.autoRetry, this.numberRetry, this.status_id, this.script_name, this.time, this.successCount, this.insuccessCount, this.sysException, this.busException, this.Transactions);
        }
    }
}
