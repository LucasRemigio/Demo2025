// // Copyright (c) 2024 Engibots. All rights reserved.

using System.Text.Json.Serialization;

namespace engimatrix.ModelObjs.Primavera;

public class PrimaveraWarehousesItem
{
    [JsonPropertyName("DataSet")]
    public PrimaveraWarehousesDataSet DataSet { get; set; }

    [JsonPropertyName("Query")]
    public string Query { get; set; }
}

public class PrimaveraWarehousesDataSet
{
    [JsonPropertyName("Table")]
    public List<PrimaveraWarehousesTableItem> Table { get; set; }
}

public class PrimaveraWarehousesTableItem
{
    [JsonPropertyName("Armazem")]
    public string Armazem { get; set; }

    [JsonPropertyName("Descricao")]
    public string Descricao { get; set; }
}
