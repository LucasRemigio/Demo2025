// // Copyright (c) 2024 Engibots. All rights reserved.

namespace engimatrix.ModelObjs.Primavera;
public class PrimaveraListResponseItem<T>
{
    public List<object> Fields { get; set; }  // Assuming Fields is always a list of objects
    public List<T> Data { get; set; }  // Data is of type T, which can be specific for each case
    public string? Message { get; set; }
    public bool IsError => !string.IsNullOrEmpty(Message);

    public PrimaveraListResponseItem()
    {
        Fields = [];
        Data = [];
    }

    public PrimaveraListResponseItem(List<object> fields, List<T> data, string? message)
    {
        Fields = fields;
        Data = data;
        Message = message;
    }

}