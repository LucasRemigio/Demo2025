// // Copyright (c) 2024 Engibots. All rights reserved.

using System.Text.Json;

namespace Engimatrix.ModelObjs;
public class OrderTotalItem
{
    public decimal total { get; set; }
    public decimal totalDiscount { get; set; }
    public decimal totalDiscountPlusTax { get; set; }

    public OrderTotalItem(decimal total, decimal totalDiscount, decimal totalDiscountPlusTax)
    {
        this.total = total;
        this.totalDiscount = totalDiscount;
        this.totalDiscountPlusTax = totalDiscountPlusTax;
    }

    public override string ToString()
    {
        return JsonSerializer.Serialize(this);
    }
}

