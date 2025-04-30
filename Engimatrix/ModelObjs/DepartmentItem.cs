// // Copyright (c) 2024 Engibots. All rights reserved.

using engimatrix.ModelObjs;

namespace Engimatrix.ModelObjs
{
    public class DepartmentItem
    {
        public string id { get; set; } = "";
        public string name { get; set; } = "";

        public DepartmentItem(string id, string name)
        {
            this.id = id;
            this.name = name;
        }

        public DepartmentItem()
        { }

        public DepartmentItem ToItem()
        {
            return new DepartmentItem(this.id, this.name);
        }
    }
}
