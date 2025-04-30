// // Copyright (c) 2024 Engibots. All rights reserved.

namespace engimatrix.ModelObjs
{
    public class LogsItem
    {
        public string id { get; set; }
        public string operation { get; set; }
        public string stateOperation { get; set; }
        public string user_operation { get; set; }
        public string operationContext { get; set; }
        public string dateTime { get; set; }

        public LogsItem(string id, string operation, string user_operation1, string state, string operation_context, string date_time)
        {
            this.id = id;
            this.operation = operation;
            this.stateOperation = state;
            this.user_operation = user_operation1;
            this.dateTime = date_time;
            this.operationContext = operation_context;
        }
    }
}
