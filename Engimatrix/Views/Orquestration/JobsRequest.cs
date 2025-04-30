namespace engimatrix.Views.Orquestration
{
    public class JobsRequest
    {
        public class Add
        {
            public string queue_name { get; set; }
            public string status_name { get; set; }
            public string date_time { get; set; }

            public List<string> job_details { get; set; } = new List<string>();
        }
    }
}
