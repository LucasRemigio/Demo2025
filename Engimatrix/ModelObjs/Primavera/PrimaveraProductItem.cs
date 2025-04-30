// // Copyright (c) 2024 Engibots. All rights reserved.

namespace engimatrix.ModelObjs.Primavera;


using System.Text.Json.Serialization;

public class PrimaveraProductItem
{
    [JsonPropertyName("Artigo")]
    public string Artigo { get; set; }

    [JsonPropertyName("Descricao")]
    public string Descricao { get; set; }

    [JsonPropertyName("Caracteristicas")]
    public string Caracteristicas { get; set; }

    [JsonPropertyName("CodBarras")]
    public string CodBarras { get; set; }

    [JsonPropertyName("UnidadeBase")]
    public string UnidadeBase { get; set; }

    [JsonPropertyName("IVA")]
    public string IVA { get; set; }

    [JsonPropertyName("Peso")]
    public double Peso { get; set; }

    [JsonPropertyName("Volume")]
    public double Volume { get; set; }

    [JsonPropertyName("Marca")]
    public string Marca { get; set; }
}
