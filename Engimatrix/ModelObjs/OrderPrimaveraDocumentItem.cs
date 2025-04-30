// // Copyright (c) 2024 Engibots. All rights reserved.

namespace engimatrix.ModelObjs;
public class OrderPrimaveraDocumentItem
{
    public int id { get; set; }
    public string order_token { get; set; }
    public string name { get; set; }
    public string type { get; set; }
    public string series { get; set; }
    public string number { get; set; }
    public DateTime created_at { get; set; }
    public string created_by { get; set; }
    public string invoice_html { get; set; }

    public OrderPrimaveraDocumentItem()
    {
        id = 0;
        order_token = string.Empty;
        name = string.Empty;
        type = string.Empty;
        series = string.Empty;
        number = string.Empty;
        created_at = DateTime.Now;
        created_by = string.Empty;
    }

    public OrderPrimaveraDocumentItem(int id, string order_token, string name, string type, string series, string number, DateTime created_at, string created_by)
    {
        this.id = id;
        this.order_token = order_token;
        this.name = name;
        this.type = type;
        this.series = series;
        this.number = number;
        this.created_at = created_at;
        this.created_by = created_by;
    }

    public override string ToString()
    {
        return $"OrderPrimaveraDocumentItem:\n" +
               $"Id: {id}\n" +
               $"OrderToken: {order_token}\n" +
               $"Name: {name}\n" +
               $"Type: {type}\n" +
               $"Series: {series}\n" +
               $"Number: {number}\n" +
               $"CreatedAt: {created_at}\n" +
               $"CreatedBy: {created_by}\n" +
                $"InvoiceHtml: {invoice_html}\n";
    }
}

