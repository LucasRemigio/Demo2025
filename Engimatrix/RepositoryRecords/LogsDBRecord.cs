// // Copyright (c) 2024 Engibots. All rights reserved.

namespace engimatrix.RepositoryRecords
{
    using engimatrix.ModelObjs;

    public class LogsDBRecord
    {
        public string id { get; set; }
        public string operation { get; set; }

        public string stateOperation { get; set; }

        public string user_operation { get; set; }
        public string operationContext { get; set; }

        public string dateTime { get; set; }

        public LogsDBRecord(string id, string operation, string user_operation1, string state, string operation_context, string date_time)
        {
            this.id = id;
            this.operation = operation;
            this.user_operation = user_operation1;
            this.stateOperation = state;
            this.operationContext = operation_context;
            this.dateTime = date_time;
        }

        public LogsItem ToLogsItem()
        {
            return new LogsItem(this.id, this.operation, this.user_operation, this.stateOperation, this.operationContext, this.dateTime);
        }
    }
}
