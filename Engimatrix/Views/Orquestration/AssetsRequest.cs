// // Copyright (c) 2024 Engibots. All rights reserved.

using engimatrix.Utils;

namespace engimatrix.Views.Orquestration
{
    public class AssetsRequest
    {
        public class Add
        {
            public string description { get; set; }
            public string type { get; set; }
            public string? text { get; set; }
            public string? user { get; set; }
            public string? password { get; set; }

        }

        public class Edit
        {
            public string id { get; set; }
            public string description { get; set; }
            public string type { get; set; }
            public string? text { get; set; }
            public string? user { get; set; }
            public string? password { get; set; }

        }
    }
}
