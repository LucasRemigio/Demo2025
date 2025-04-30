// // Copyright (c) 2024 Engibots. All rights reserved.

using engimatrix.ModelObjs;
using engimatrix.ResponseMessages;

namespace engimatrix.Views;
public class OrderPrimaveraDocumentResponse : GenericResponse
{
    public OrderPrimaveraDocumentItem? document { get; set; }

    public OrderPrimaveraDocumentResponse(OrderPrimaveraDocumentItem doc, int result_code, string language) : base(result_code, language)
    {
        this.document = doc;
    }

    public OrderPrimaveraDocumentResponse(int result_code, string language) : base(result_code, language)
    {
    }
}

public class OrderPrimaveraDocumentListResponse : GenericResponse
{
    public List<OrderPrimaveraDocumentItem>? documents { get; set; }

    public OrderPrimaveraDocumentListResponse(List<OrderPrimaveraDocumentItem> docs, int result_code, string language) : base(result_code, language)
    {
        this.documents = docs;
    }

    public OrderPrimaveraDocumentListResponse(int result_code, string language) : base(result_code, language)
    {
    }
}