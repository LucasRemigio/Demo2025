// // Copyright (c) 2024 Engibots. All rights reserved.

using System.Text.Json.Serialization;

namespace engimatrix.ModelObjs.Primavera;

public class PrimaveraProductsItem
{
    [JsonPropertyName("DataSet")]
    public PrimaveraProductDataSet DataSet { get; set; }

    [JsonPropertyName("Query")]
    public string Query { get; set; }
}

public class PrimaveraProductDataSet
{
    [JsonPropertyName("Table")]
    public List<PrimaveraProductTableItem> Table { get; set; }
}

public class PrimaveraProductTableItem
{
    [JsonPropertyName("Artigo")]
    public string Artigo { get; set; }

    [JsonPropertyName("Descricao")]
    public string Descricao { get; set; }
}
