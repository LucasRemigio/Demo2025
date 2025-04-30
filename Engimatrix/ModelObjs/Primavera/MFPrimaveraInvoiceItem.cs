// // Copyright (c) 2024 Engibots. All rights reserved.

namespace engimatrix.ModelObjs.Primavera;


using System.Text.Json.Serialization;

public class MFPrimaveraInvoiceItem
{
    public string Modulo { get; set; }
    public int? Ano { get; set; }
    public string TipoDoc { get; set; }
    public string Serie { get; set; }
    public int NumDoc { get; set; }
    public string NumDocExt { get; set; }
    public DateTime DataDoc { get; set; }
    public string TipoEntidade { get; set; }
    public string Entidade { get; set; }
    public string TipoMov { get; set; }
    public double ValorTotal { get; set; }
    public double ValorPendente { get; set; }
    public double ValorLiquidacao { get; set; }
    public string Codigo { get; set; }
    public DateTime? DataVencimento { get; set; }
    public DateTime? DataLiquidacao { get; set; }
    public string Moeda { get; set; }
    public int NumAvisos { get; set; }
}
