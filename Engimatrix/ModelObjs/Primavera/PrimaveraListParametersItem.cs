// // Copyright (c) 2024 Engibots. All rights reserved.

namespace engimatrix.ModelObjs.Primavera;

public class PrimaveraListParametersItem
{
    public required string ListGuid { get; set; }
    public int Offset { get; set; }
    public int Limit { get; set; }
    public required string SqlWhere { get; set; }
}
