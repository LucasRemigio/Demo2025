// // Copyright (c) 2024 Engibots. All rights reserved.

using engimatrix.Utils;

namespace engimatrix.Views
{
    public class OperatorRequest
    {
        public class Add
        {
            public string operator_name { get; set; }
            public string operator_email { get; set; }
            public string operator_area { get; set; }

            public bool Validate()
            {
                if (!Util.IsValidInputString(this.operator_name) || !Util.IsValidInputEmail(operator_email) || !Util.IsValidInputString(this.operator_area))
                {
                    return false;
                }

                return true;
            }
        }

        public class Edit
        {
            public string id { get; set; }
            public string operator_name { get; set; }
            public string operator_email { get; set; }

            public bool Validate()
            {
                if (!Util.IsValidInputString(this.operator_name) || !Util.IsValidInputEmail(operator_email) || !Util.IsValidInputString(this.id))
                {
                    return false;
                }

                return true;
            }
        }
    }
}
