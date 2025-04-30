namespace engimatrix.ModelObjs
{
    public class InvoiceItem
    {
        public int id { get; set; }
        public string doc_type_id { get; set; }
        public string depot_id { get; set; }
        public string entity_id { get; set; }
        public string address_id { get; set; }
        public string address_name { get; set; }
        public string date { get; set; }
        public string due_date { get; set; }
        public string po_number { get; set; }
        public int status { get; set; }
        public int category { get; set; }
        public string token { get; set; }
        public string obs { get; set; }
        public List<InvoiceDetailsItem> items { get; set; }

        public InvoiceItem()
        {
        }

        public InvoiceItem(int id, string doc_type_id, string depot_id, string entity_id, string address_id, string address_name, string date, string due_date, string po_number, int status, int category, string token, string obs, List<InvoiceDetailsItem> items)
        {
            this.id = id;
            this.doc_type_id = doc_type_id;
            this.depot_id = depot_id;
            this.entity_id = entity_id;
            this.address_id = address_id;
            this.address_name = address_name;
            this.date = date;
            this.due_date = due_date;
            this.po_number = po_number;
            this.status = status;
            this.category = category;
            this.token = token;
            this.obs = obs;
            this.items = items;
        }

        public bool Validate()
        {
            if (string.IsNullOrEmpty(this.id.ToString()) || string.IsNullOrEmpty(this.doc_type_id) || string.IsNullOrEmpty(this.depot_id) || string.IsNullOrEmpty(this.entity_id) || string.IsNullOrEmpty(this.address_id) || string.IsNullOrEmpty(this.address_name) || string.IsNullOrEmpty(this.date) || string.IsNullOrEmpty(this.due_date) || string.IsNullOrEmpty(this.po_number) || string.IsNullOrEmpty(this.status.ToString()) || string.IsNullOrEmpty(this.category.ToString()) || string.IsNullOrEmpty(this.token) || string.IsNullOrEmpty(this.obs))
            {
                return false;
            }

            return true;
        }

        public InvoiceItem ToItem()
        {
            return new InvoiceItem(this.id, this.doc_type_id, this.depot_id, this.entity_id, this.address_id, this.address_name, this.date, this.due_date, this.po_number, this.status, this.category, this.token, this.obs, this.items);
        }
    }

    public class InvoiceDetailsItem
    {
        public string id { get; set; }
        public string invoice_id { get; set; }
        public string unit_amount { get; set; }
        public string vat_rate { get; set; }
        public string obs { get; set; }

        public InvoiceDetailsItem(string id, string invoice_id, string unit_amount, string vat_rate, string obs)
        {
            this.id = id;
            this.invoice_id = invoice_id;
            this.unit_amount = unit_amount;
            this.vat_rate = vat_rate;
            this.obs = obs;
        }

        public bool Validate()
        {
            if (string.IsNullOrEmpty(this.id) || string.IsNullOrEmpty(this.invoice_id) || string.IsNullOrEmpty(this.unit_amount) || string.IsNullOrEmpty(this.vat_rate))
            {
                return false;
            }

            return true;
        }

        public InvoiceDetailsItem ToItem()
        {
            return new InvoiceDetailsItem(this.id, this.invoice_id, this.unit_amount, this.vat_rate, this.obs);
        }
    }
}
