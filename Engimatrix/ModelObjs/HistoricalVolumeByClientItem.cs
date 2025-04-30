// // Copyright (c) 2024 Engibots. All rights reserved.

namespace engimatrix.ModelObjs;
public class HistoricalVolumeByClientItem
{
    public string client_code { get; set; }
    public decimal orders_total { get; set; }

    public HistoricalVolumeByClientItem(string client_code, decimal orders_total)
    {
        this.client_code = client_code;
        this.orders_total = orders_total;
    }

}

