// // Copyright (c) 2024 Engibots. All rights reserved.


using System.Text.Json.Serialization;

namespace engimatrix.ModelObjs.Primavera;

public class PrimaveraUnitConversionItem
{
    [JsonPropertyName("Artigo")]
    public string Artigo { get; set; }

    [JsonPropertyName("UnidadeOrigem")]
    public string UnidadeOrigem { get; set; }

    [JsonPropertyName("UnidadeDestino")]
    public string UnidadeDestino { get; set; }

    [JsonPropertyName("FactorConversao")]
    public double FactorConversao { get; set; }
}
