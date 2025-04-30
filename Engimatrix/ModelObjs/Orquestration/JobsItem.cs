using System;

namespace engimatrix.ModelObjs.Orquestration
{
    public class JobsItem
    {
        public string id { get; set; }
        public string script_id { get; set; }
        public string script_name { get; set; }
        public string stateOperation { get; set; }
        public string user_operation { get; set; }
        public string dateTime { get; set; }

        public string job_Details { get; set; }



        public JobsItem(string id, string script_id, string script_name, string user_operation, string state, string date_time, string job_Details)
        {
            this.id = id;
            this.script_id = script_id;
            this.script_name = script_name;
            this.stateOperation = state;
            this.user_operation = user_operation;
            this.dateTime = date_time;
            this.job_Details = job_Details;

        }
    }
}
