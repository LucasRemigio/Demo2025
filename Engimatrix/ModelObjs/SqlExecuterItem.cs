// // Copyright (c) 2024 Engibots. All rights reserved.

namespace engimatrix.ModelObjs
{
    public class SqlExecuterItem
    {
        public List<Dictionary<string, string>> out_data { get; set; }

        public bool operationResult { get; set; }
        public int? rowsAffected { get; set; }
    }
}
