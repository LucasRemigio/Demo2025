// // Copyright (c) 2024 Engibots. All rights reserved.

namespace engimatrix.ModelObjs.Primavera;

using System.Collections.Generic;
using System.Text.Json.Serialization;

public class PrimaveraInternalDocumentsItem
{
    [JsonPropertyName("DataSet")]
    public PrimaveraInternalDocumentsDataSet DataSet { get; set; }
}

public class PrimaveraInternalDocumentsDataSet
{
    [JsonPropertyName("Table")]
    public List<PrimaveraInternalDocumentsTableItem> Table { get; set; }
}

public class PrimaveraInternalDocumentsTableItem
{
    [JsonPropertyName("Documento")]
    public string Documento { get; set; }

    [JsonPropertyName("Descricao")]
    public string Descricao { get; set; }

    [JsonPropertyName("TipoDocumento")]
    public int TipoDocumento { get; set; }

    [JsonPropertyName("LigaStocks")]
    public bool LigaStocks { get; set; }
}
