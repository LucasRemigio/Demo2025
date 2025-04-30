// // Copyright (c) 2024 Engibots. All rights reserved.


using System.Text.Json.Serialization;

namespace engimatrix.ModelObjs.Primavera;

public class PrimaveraOrderItem
{
    public PrimaveraOrderHeaderItem primavera_order_header { get; set; }
    public List<PrimaveraOrderLineItem> primavera_order_line { get; set; }

    public PrimaveraOrderItem()
    {
        this.primavera_order_header = new();
        this.primavera_order_line = [];
    }
}
