// // Copyright (c) 2024 Engibots. All rights reserved.

using engimatrix.Utils;

namespace engimatrix.Views.Orquestration
{
    public class TriggersRequest
    {
        public class AddTrigger
        {
            public string name { get; set; }
            public string cron_expression { get; set; }
            public string script_name { get; set; }
        }

        public class EditTrigger
        {
            public string id { get; set; }
            public string name { get; set; }
            public string cron_expression { get; set; }
        }

        public class RemoveTrigger
        {
            public string id { get; set; }
        }
    }
}
