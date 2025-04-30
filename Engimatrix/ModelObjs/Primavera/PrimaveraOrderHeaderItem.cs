// // Copyright (c) 2024 Engibots. All rights reserved.


using System.Text.Json.Serialization;

namespace engimatrix.ModelObjs.Primavera;

public class PrimaveraOrderHeaderItem
{
    [JsonPropertyName("Id")]
    public string Id { get; set; }

    [JsonPropertyName("TipoDoc")]
    public string TipoDoc { get; set; }

    [JsonPropertyName("Serie")]
    public string Serie { get; set; }

    [JsonPropertyName("NumDoc")]
    public int NumDoc { get; set; }

    [JsonPropertyName("TipoEntidade")]
    public string TipoEntidade { get; set; }

    [JsonPropertyName("Entidade")]
    public string Entidade { get; set; }

    [JsonPropertyName("Nome")]
    public string Nome { get; set; }

    [JsonPropertyName("NumContribuiente")]
    public string NumContribuiente { get; set; }

    [JsonPropertyName("TotalDocumento")]
    public decimal TotalDocumento { get; set; }

    [JsonPropertyName("Moeda")]
    public string Moeda { get; set; }

    [JsonPropertyName("Cambio")]
    public decimal Cambio { get; set; }

    [JsonPropertyName("DataDoc")]
    public DateTime DataDoc { get; set; }

    [JsonPropertyName("Estado")]
    public string Estado { get; set; }

    [JsonPropertyName("DescEstado")]
    public string DescEstado { get; set; }

    [JsonPropertyName("MoradaAltEntrega")]
    public string? MoradaAltEntrega { get; set; }

    [JsonPropertyName("Morada")]
    public string Morada { get; set; }

    [JsonPropertyName("Morada2")]
    public string? Morada2 { get; set; }

    [JsonPropertyName("Localidade")]
    public string Localidade { get; set; }

    [JsonPropertyName("CodPostal")]
    public string CodPostal { get; set; }

    [JsonPropertyName("CodPostalLocalidade")]
    public string CodPostalLocalidade { get; set; }

    [JsonPropertyName("Distrito")]
    public string? Distrito { get; set; }

    [JsonPropertyName("Pais")]
    public string Pais { get; set; }

    [JsonPropertyName("DataCarga")]
[JsonConverter(typeof(NullableDateTimeConverter))]
    public DateTime? DataCarga { get; set; }

    [JsonPropertyName("Matricula")]
    public string? Matricula { get; set; }

    [JsonPropertyName("LocalCarga")]
    public string LocalCarga { get; set; }

    [JsonPropertyName("MoradaCarga")]
    public string MoradaCarga { get; set; }

    [JsonPropertyName("Morada2Carga")]
    public string? Morada2Carga { get; set; }

    [JsonPropertyName("LocalidadeCarga")]
    public string LocalidadeCarga { get; set; }

    [JsonPropertyName("CodPostalCarga")]
    public string CodPostalCarga { get; set; }

    [JsonPropertyName("CodPostalLocalidadeCarga")]
    public string CodPostalLocalidadeCarga { get; set; }

    [JsonPropertyName("DistritoCarga")]
    public string? DistritoCarga { get; set; }

    [JsonPropertyName("PaisCarga")]
    public string PaisCarga { get; set; }

    [JsonPropertyName("DataDescarga")]
    public DateTime? DataDescarga { get; set; }

    [JsonPropertyName("LocalDescarga")]
    public string LocalDescarga { get; set; }

    [JsonPropertyName("MoradaDescarga")]
    public string MoradaDescarga { get; set; }

    [JsonPropertyName("Morada2Descarga")]
    public string? Morada2Descarga { get; set; }

    [JsonPropertyName("LocalidadeDescarga")]
    public string LocalidadeDescarga { get; set; }

    [JsonPropertyName("CodPostalDescarga")]
    public string CodPostalDescarga { get; set; }

    [JsonPropertyName("CodPostalLocalidadeDescarga")]
    public string CodPostalLocalidadeDescarga { get; set; }

    [JsonPropertyName("DistritoDescarga")]
    public string? DistritoDescarga { get; set; }

    [JsonPropertyName("PaisDescarga")]
    public string PaisDescarga { get; set; }

    [JsonPropertyName("EntidadeDescarga")]
    public string EntidadeDescarga { get; set; }

    [JsonPropertyName("TipoEntidadeDescarga")]
    public string TipoEntidadeDescarga { get; set; }

    [JsonPropertyName("ObservacoesCabec")]
    public string? ObservacoesCabec { get; set; }

    [JsonPropertyName("Referencia")]
    public string? Referencia { get; set; }

    [JsonPropertyName("DescontoEntidade")]
    public decimal DescontoEntidade { get; set; }

    [JsonPropertyName("DescontoFinanceiro")]
    public decimal DescontoFinanceiro { get; set; }

    [JsonPropertyName("ModoPagamento")]
    public string ModoPagamento { get; set; }

    [JsonPropertyName("CondicaoPagamento")]
    public string CondicaoPagamento { get; set; }

    [JsonPropertyName("DataVencimento")]
    public DateTime DataVencimento { get; set; }

    [JsonPropertyName("ModoExpedicao")]
    public string? ModoExpedicao { get; set; }

    [JsonPropertyName("TipoEntidadeFaturacao")]
    public string TipoEntidadeFaturacao { get; set; }

    [JsonPropertyName("EntidadeFaturacao")]
    public string EntidadeFaturacao { get; set; }

    [JsonPropertyName("MoradaFaturacao")]
    public string MoradaFaturacao { get; set; }

    [JsonPropertyName("Morada2Faturacao")]
    public string? Morada2Faturacao { get; set; }

    [JsonPropertyName("LocalidadeFaturacao")]
    public string LocalidadeFaturacao { get; set; }

    [JsonPropertyName("CodPostalFaturacao")]
    public string CodPostalFaturacao { get; set; }

    [JsonPropertyName("LocalidadeCodPostalFaturacao")]
    public string LocalidadeCodPostalFaturacao { get; set; }

    [JsonPropertyName("DistritoFaturacao")]
    public string? DistritoFaturacao { get; set; }

    [JsonPropertyName("EspacoFiscal")]
    public string EspacoFiscal { get; set; }

    [JsonPropertyName("RegimeIva")]
    public string RegimeIva { get; set; }

    [JsonPropertyName("TipoOperacaoCabec")]
    public string? TipoOperacaoCabec { get; set; }

    [JsonPropertyName("DescTipoOperacaoCabec")]
    public string? DescTipoOperacaoCabec { get; set; }

    [JsonPropertyName("Documento")]
    public string Documento { get; set; }

    [JsonPropertyName("VendedorCabec")]
    public string VendedorCabec { get; set; }

    [JsonPropertyName("NomeVendedorCabec")]
    public string NomeVendedorCabec { get; set; }
}
