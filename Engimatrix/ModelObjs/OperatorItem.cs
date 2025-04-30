// // Copyright (c) 2024 Engibots. All rights reserved.

namespace engimatrix.ModelObjs
{
    public class OperatorItem
    {
        public int id { get; set; }
        public string operator_name { get; set; }
        public string operator_email { get; set; }
        public string operator_area { get; set; }

        public OperatorItem(int id, string operator_name, string operator_email, string operator_area)
        {
            this.id = id;
            this.operator_name = operator_name;
            this.operator_email = operator_email;
            this.operator_area = operator_area;
        }

        public OperatorItem ToItem()
        {
            return new OperatorItem(this.id, this.operator_name, this.operator_email, this.operator_area);
        }
    }
}
