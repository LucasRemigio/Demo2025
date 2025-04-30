// // Copyright (c) 2024 Engibots. All rights reserved.

using engimatrix.Utils;

namespace engimatrix.Views.Orquestration
{
    public class QueuesRequest
    {
        public class Add
        {
            public string? name { get; set; }
            public string? description { get; set; }
            public Boolean autoRetry { get; set; }
            public int numberRetry { get; set; }
            public int status_id { get; set; }
            public string script_name { get; set; }

        }

        public class Edit
        {
            public string id { get; set; }
            public string? name { get; set; }
            public string? description { get; set; }
            public Boolean autoRetry { get; set; }
            public int numberRetry { get; set; }
            public int status_id { get; set; }

        }
    }
}
