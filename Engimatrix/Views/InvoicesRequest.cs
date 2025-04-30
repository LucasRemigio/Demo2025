/ // Copyright (c) 2024 Engibots. All rights reserved.

using engimatrix.Utils;

namespace engimatrix.Views
{
    public class InvoicesRequest
    {
        public class Add
        {
            public int id_invoice { get; set; }
            public int invoice_number { get; set; }
            public string client { get; set; }

            public int invoice_status { get; set; }
            public decimal total { get; set; }
            public string expire_date { get; set; }
            public int purchase_order { get; set; }

            public bool Validate()
            {
                if (!Util.IsValidInputString(this.client) || !Util.IsValidInputString(this.expire_date))
                {
                    return false;
                }

                return true;
            }
        }
    }
}
