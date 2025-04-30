// // Copyright (c) 2024 Engibots. All rights reserved.

using Engimatrix.ModelObjs;

namespace engimatrix.ModelObjs
{
    public class UserItem
    {
        public string email { get; set; }
        public string name { get; set; }
        public string password { get; set; }
        public string active_since { get; set; }
        public string last_login { get; set; }
        public string role { get; set; }
        public List<DepartmentItem> departments { get; set; }

        public UserItem(string email, string name, string password, string activeSince, string lastLogin, string role, List<DepartmentItem> departments)
        {
            this.email = email;
            this.name = name;
            this.password = password;
            this.active_since = activeSince;
            this.last_login = lastLogin;
            this.role = role;
            this.departments = departments;
        }
    }
}
