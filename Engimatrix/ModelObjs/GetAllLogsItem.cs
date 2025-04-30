// // Copyright (c) 2024 Engibots. All rights reserved.

namespace engimatrix.ModelObjs
{
    public class GetAllLogsItem
    {
        public string email { get; set; }
        public string short_name { get; set; }
        public string state { get; set; }

        public string id { get; set; }

        public GetAllLogsItem(string email, string shortName, string state, string id)
        {
            this.email = email;
            this.short_name = shortName;
            this.state = state;
            this.id = id;
        }
    }
}
