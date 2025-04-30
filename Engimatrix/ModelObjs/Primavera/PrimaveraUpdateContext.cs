// // Copyright (c) 2024 Engibots. All rights reserved.

namespace engimatrix.ModelObjs.Primavera;

using System.Text.Json.Serialization;

public class PrimaveraUpdateContext
{
    public string Version { get; set; }
    public int StatusCode { get; set; }
    public string ErrorMessage { get; set; }
    public string Results { get; set; }
}
