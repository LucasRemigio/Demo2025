// // Copyright (c) 2024 Engibots. All rights reserved.

namespace engimatrix.ModelObjs
{
    public class QuoteRequestItem
    {
        public int id { get; set; }
        public int quote_id_erp { get; set; }
        public DateOnly quote_date { get; set; }
        public int client_id { get; set; }
        public string client_name { get; set; }
        public string product_code { get; set; }
        public decimal quantity_requested { get; set; }
        public decimal erp_price { get; set; }
        public decimal erp_price_modification_percent { get; set; }
        public bool alert_flag { get; set; }
        public bool special_flag { get; set; }
        public decimal final_price { get; set; }
        public decimal order_quantity { get; set; }
        public string order_id { get; set; }
        public string observation { get; set; }
        public decimal unit_price { get; set; }
        public decimal margin_percent { get; set; }
        public decimal price_difference_erp { get; set; }
        public decimal price_difference_percent_erp { get; set; }
        public decimal total_difference_erp { get; set; }
        public decimal total_difference_final { get; set; }

        public QuoteRequestItem(int id, int quoteIdErp, DateOnly quoteDate, int clientId, string clientName, string productCode, decimal quantityRequested,
            decimal erpPrice, decimal erpPriceModificationPercent, bool alertFlag, bool specialFlag, decimal finalPrice, decimal orderQuantity, string orderId,
            string observation, decimal unitPrice, decimal marginPercent, decimal priceDifferenceErp, decimal priceDifferencePercentErp, decimal totalDifferenceErp,
            decimal totalDifferenceFinal)
        {
            this.id = id;
            this.quote_id_erp = quoteIdErp;
            this.quote_date = quoteDate;
            this.client_id = clientId;
            this.client_name = clientName;
            this.product_code = productCode;
            this.quantity_requested = quantityRequested;
            this.erp_price = erpPrice;
            this.erp_price_modification_percent = erpPriceModificationPercent;
            this.alert_flag = alertFlag;
            this.special_flag = specialFlag;
            this.final_price = finalPrice;
            this.order_quantity = orderQuantity;
            this.order_id = orderId;
            this.observation = observation;
            this.unit_price = unitPrice;
            this.margin_percent = marginPercent;
            this.price_difference_erp = priceDifferenceErp;
            this.price_difference_percent_erp = priceDifferencePercentErp;
            this.total_difference_erp = totalDifferenceErp;
            this.total_difference_final = totalDifferenceFinal;
        }

        public QuoteRequestItem(int quoteIdErp, DateOnly quoteDate, int clientId, string clientName, string productCode, decimal quantityRequested,
           decimal erpPrice, decimal erpPriceModificationPercent, bool alertFlag, bool specialFlag, decimal finalPrice, decimal orderQuantity, string orderId,
           string observation, decimal unitPrice, decimal marginPercent, decimal priceDifferenceErp, decimal priceDifferencePercentErp, decimal totalDifferenceErp,
           decimal totalDifferenceFinal)
        {
            this.quote_id_erp = quoteIdErp;
            this.quote_date = quoteDate;
            this.client_id = clientId;
            this.client_name = clientName;
            this.product_code = productCode;
            this.quantity_requested = quantityRequested;
            this.erp_price = erpPrice;
            this.erp_price_modification_percent = erpPriceModificationPercent;
            this.alert_flag = alertFlag;
            this.special_flag = specialFlag;
            this.final_price = finalPrice;
            this.order_quantity = orderQuantity;
            this.order_id = orderId;
            this.observation = observation;
            this.unit_price = unitPrice;
            this.margin_percent = marginPercent;
            this.price_difference_erp = priceDifferenceErp;
            this.price_difference_percent_erp = priceDifferencePercentErp;
            this.total_difference_erp = totalDifferenceErp;
            this.total_difference_final = totalDifferenceFinal;
        }

        public QuoteRequestItem()
        { }

        public QuoteRequestItem ToItem()
        {
            return new QuoteRequestItem(this.id, this.quote_id_erp, this.quote_date, this.client_id, this.client_name, this.product_code, this.quantity_requested,
                this.erp_price, this.erp_price_modification_percent, this.alert_flag, this.special_flag, this.final_price, this.order_quantity, this.order_id,
                this.observation, this.unit_price, this.margin_percent, this.price_difference_erp, this.price_difference_percent_erp, this.total_difference_erp, this.total_difference_final);
        }

        public override string ToString()
        {
            return $"QuoteRequestItem:\n" +
                     $"id: {id}\n" +
                     $"quote_id_erp: {quote_id_erp}\n" +
                     $"quote_date: {quote_date}\n" +
                     $"client_id: {client_id}\n" +
                     $"client_name: {client_name}\n" +
                     $"product_code: {product_code}\n" +
                     $"quantity_requested: {quantity_requested}\n" +
                     $"erp_price: {erp_price}\n" +
                     $"erp_price_modification_percent: {erp_price_modification_percent}\n" +
                     $"alert_flag: {alert_flag}\n" +
                     $"special_flag: {special_flag}\n" +
                     $"final_price: {final_price}\n" +
                     $"order_quantity: {order_quantity}\n" +
                     $"order_id: {order_id}\n" +
                     $"observation: {observation}\n" +
                     $"unit_price: {unit_price}\n" +
                     $"margin_percent: {margin_percent}\n" +
                     $"price_difference_erp: {price_difference_erp}\n" +
                     $"price_difference_percent_erp: {price_difference_percent_erp}\n" +
                     $"total_difference_erp: {total_difference_erp}\n" +
                     $"total_difference_final: {total_difference_final}\n";
        }

        public bool IsEmpty()
        {
            return id == 0 &&
                   quote_id_erp == 0 &&
                   quote_date == default(DateOnly) &&
                   client_id == 0 &&
                   string.IsNullOrEmpty(client_name) &&
                   string.IsNullOrEmpty(product_code) &&
                   quantity_requested == 0 &&
                   erp_price == 0 &&
                   erp_price_modification_percent == 0 &&
                   alert_flag == false &&
                   special_flag == false &&
                   final_price == 0 &&
                   order_quantity == 0 &&
                   string.IsNullOrEmpty(order_id) &&
                   string.IsNullOrEmpty(observation) &&
                   unit_price == 0 &&
                   margin_percent == 0 &&
                   price_difference_erp == 0 &&
                   price_difference_percent_erp == 0 &&
                   total_difference_erp == 0 &&
                   total_difference_final == 0;
        }
    }
}
