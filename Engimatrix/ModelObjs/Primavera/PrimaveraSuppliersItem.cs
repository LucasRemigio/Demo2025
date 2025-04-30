// // Copyright (c) 2024 Engibots. All rights reserved.

using System.Text.Json.Serialization;

namespace engimatrix.ModelObjs.Primavera;

public class PrimaveraSuppliersItem
{
    [JsonPropertyName("DataSet")]
    public PrimaveraSuppliersDataSet DataSet { get; set; }

    [JsonPropertyName("Query")]
    public string Query { get; set; }
}

public class PrimaveraSuppliersDataSet
{
    [JsonPropertyName("Table")]
    public List<PrimaveraSuppliersTableItem> Table { get; set; }
}

public class PrimaveraSuppliersTableItem
{
    [JsonPropertyName("Fornecedor")]
    public string Fornecedor { get; set; }

    [JsonPropertyName("Nome")]
    public string Nome { get; set; }

    [JsonPropertyName("Morada")]
    public string Morada { get; set; }

    [JsonPropertyName("Pais")]
    public string Pais { get; set; }

    [JsonPropertyName("NumContrib")]
    public string NumeroContribuinte { get; set; }
}
