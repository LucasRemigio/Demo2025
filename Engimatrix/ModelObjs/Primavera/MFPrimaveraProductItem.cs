// // Copyright (c) 2024 Engibots. All rights reserved.

namespace engimatrix.ModelObjs.Primavera;


using System.Text.Json.Serialization;

public class MFPrimaveraProductItem
{
    public string Artigo { get; set; }
    public string Descricao { get; set; }
    public string UnidadeBase { get; set; }
    public string CodigoIva { get; set; }
    public string Familia { get; set; }
    public string? SubFamilia { get; set; }
    public string? Marca { get; set; }
    public string? Modelo { get; set; }
    public int TratamentoDim { get; set; }
    public string? ArtigoPai { get; set; }
    public double UltimoPrecoCompra { get; set; }
    public double? PrecoCustoMedio { get; set; }
    public double PVP1 { get; set; }
    public string UnidadeAuxiliarVenda { get; set; }
    public string UnidadeAuxiliarCompra { get; set; }
    public string? TipoDimensao1 { get; set; }
    public string? Dimensao1 { get; set; }
    public string? RubDimensao1 { get; set; }
    public string? TipoDimensao2 { get; set; }
    public string? Dimensao2 { get; set; }
    public string? RubDimensao2 { get; set; }
    public string? TipoDimensao3 { get; set; }
    public string? Dimensao3 { get; set; }
    public string? RubDimensao3 { get; set; }
}
