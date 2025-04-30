// // Copyright (c) 2024 Engibots. All rights reserved.
using System.Text.Json.Serialization;

namespace engimatrix.ModelObjs.Primavera;

public class PrimaveraClientsItem
{
    [JsonPropertyName("DataSet")]
    public PrimaveraClientsDataSet DataSet { get; set; }

    [JsonPropertyName("Query")]
    public string Query { get; set; }
}

public class PrimaveraClientsDataSet
{
    [JsonPropertyName("Table")]
    public List<PrimaveraClientsTableItem> Table { get; set; }
}

public class PrimaveraClientsTableItem
{
    [JsonPropertyName("Cliente")]
    public string Cliente { get; set; }

    [JsonPropertyName("Nome")]
    public string Nome { get; set; }

    [JsonPropertyName("Fac_Mor")]
    public string MoradaFaturacao { get; set; }

    [JsonPropertyName("Pais")]
    public string Pais { get; set; }

    [JsonPropertyName("NumContrib")]
    public string NumeroContribuinte { get; set; }
}
