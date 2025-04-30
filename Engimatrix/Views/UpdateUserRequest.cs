// // Copyright (c) 2024 Engibots. All rights reserved.

using engimatrix.Utils;
using Engimatrix.ModelObjs;

namespace engimatrix.Views
{
    public class UpdateUserRequest
    {
        public string updated_name { get; set; }
        public string email { get; set; }
        public string updated_password { get; set; }

        public List<DepartmentItem> updated_departments { get; set; }
        public bool Validate()
        {
            if (string.IsNullOrEmpty(this.updated_name) || !Util.IsValidInputEmail(this.email) || string.IsNullOrEmpty(this.updated_password))
            {
                return false;
            }

            return true;
        }
    }
}
