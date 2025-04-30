// // Copyright (c) 2024 Engibots. All rights reserved.


using System.Text.Json.Serialization;

namespace engimatrix.ModelObjs.Primavera;

public class PrimaveraOrderLineItem
{
    [JsonPropertyName("IdCabec")]
    public string IdCabec { get; set; }

    [JsonPropertyName("Id")]
    public string Id { get; set; }

    [JsonPropertyName("TipoDoc")]
    public string TipoDoc { get; set; }

    [JsonPropertyName("Serie")]
    public string Serie { get; set; }

    [JsonPropertyName("NumDoc")]
    public int NumDoc { get; set; }

    [JsonPropertyName("NumLinha")]
    public int NumLinha { get; set; }

    [JsonPropertyName("Artigo")]
    public string Artigo { get; set; }

    [JsonPropertyName("Descricao")]
    public string Descricao { get; set; }

    [JsonPropertyName("PrecUnit")]
    public decimal PrecUnit { get; set; }

    [JsonPropertyName("Quantidade")]
    public decimal Quantidade { get; set; }

    [JsonPropertyName("PrecoLiquido")]
    public decimal PrecoLiquido { get; set; }

    [JsonPropertyName("Moeda")]
    public string Moeda { get; set; }

    [JsonPropertyName("Unidade")]
    public string Unidade { get; set; }

    [JsonPropertyName("CodIva")]
    public string CodIva { get; set; }

    [JsonPropertyName("TaxaIva")]
    public decimal TaxaIva { get; set; }

    [JsonPropertyName("Armazem")]
    public string Armazem { get; set; }

    [JsonPropertyName("Lote")]
    public string Lote { get; set; }

    [JsonPropertyName("Desconto1")]
    public decimal Desconto1 { get; set; }

    [JsonPropertyName("Desconto2")]
    public decimal Desconto2 { get; set; }

    [JsonPropertyName("Desconto3")]
    public decimal Desconto3 { get; set; }

    [JsonPropertyName("DataLinha")]
    public DateTime DataLinha { get; set; }

    [JsonPropertyName("DataEntrega")]
    public DateTime? DataEntrega { get; set; }

    [JsonPropertyName("Vendedor")]
    public string Vendedor { get; set; }

    [JsonPropertyName("NomeVendedor")]
    public string NomeVendedor { get; set; }

    [JsonPropertyName("ComissaoVendedor")]
    public decimal ComissaoVendedor { get; set; }

    [JsonPropertyName("Observacoes")]
    public string? Observacoes { get; set; }

    [JsonPropertyName("TipoOperacao")]
    public string? TipoOperacao { get; set; }

    [JsonPropertyName("DescTipoOpercao")]
    public string? DescTipoOpercao { get; set; }

    [JsonPropertyName("IvaRegraCalculo")]
    public string IvaRegraCalculo { get; set; }

    [JsonPropertyName("CodigoPautal")]
    public string? CodigoPautal { get; set; }

    [JsonPropertyName("MassaLiqLinha")]
    public decimal MassaLiqLinha { get; set; }

    [JsonPropertyName("ValorLiqLinha")]
    public decimal ValorLiqLinha { get; set; }

    [JsonPropertyName("IdObra")]
    public string? IdObra { get; set; }
}
