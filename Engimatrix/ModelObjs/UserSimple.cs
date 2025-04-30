// // Copyright (c) 2024 Engibots. All rights reserved.

using Engimatrix.ModelObjs;

namespace engimatrix.ModelObjs
{
    public class UserSimple
    {
        public string user { get; set; }
        public string token { get; set; }
        public string role { get; set; }
        public bool expiredPass { get; set; }
        public List<DepartmentItem> departments { get; set; }

        public UserSimple(string user, string token, string role, bool expiredPass)
        {
            this.user = user;
            this.token = token;
            this.role = role;
            this.expiredPass = expiredPass;
            this.departments = new List<DepartmentItem>();
        }

        public UserSimple(string user, string token, string role, bool expiredPass, List<DepartmentItem> departments)
        {
            this.user = user;
            this.token = token;
            this.role = role;
            this.expiredPass = expiredPass;
            this.departments = departments;
        }

        public bool IsValid()
        {
            if (string.IsNullOrEmpty(this.user) || string.IsNullOrEmpty(this.token) || string.IsNullOrEmpty(this.role))
            {
                return false;
            }

            return true;
        }
    }
}
