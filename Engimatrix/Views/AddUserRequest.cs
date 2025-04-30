// // Copyright (c) 2024 Engibots. All rights reserved.

using engimatrix.Utils;
using Engimatrix.ModelObjs;

namespace engimatrix.Views
{
    public class AddUserRequest
    {
        public string email { get; set; }
        public string name { get; set; }
        public string password { get; set; }
        public int user_role_id { get; set; }
        public List<DepartmentItem> departments { get; set; }
        public bool Validate()
        {
            if (!Util.IsValidInputEmail(this.email) || !Util.IsValidInputString(this.name) || !Util.IsValidInputString(this.password))
            {
                return false;
            }

            return true;
        }
    }
}
