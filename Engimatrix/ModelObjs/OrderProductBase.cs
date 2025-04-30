// // Copyright (c) 2024 Engibots. All rights reserved.

namespace engimatrix.ModelObjs;

public class OrderProductBase
{
    public int id { get; set; }
    public string order_token { get; set; }
    public decimal quantity { get; set; }
    public decimal calculated_price { get; set; }
    public decimal price_discount { get; set; }
    public int confidence { get; set; }
    public bool is_instant_match { get; set; }
    public bool is_manual_insert { get; set; }
    public DateTime? price_locked_at { get; set; }

    // Override ToString for debugging or logging
    public override string ToString()
    {
        return $"OrderProduct:\n" +
               $"ID: {id}\n" +
               $"Order Token: {order_token}\n" +
               $"Quantity: {quantity}\n" +
               $"Calculated Price: {calculated_price}";
    }

    // Check if the object is empty
    public bool IsEmpty()
    {
        return id == 0 &&
               string.IsNullOrEmpty(order_token) &&
               quantity == 0 &&
               calculated_price == 0;
    }
}
