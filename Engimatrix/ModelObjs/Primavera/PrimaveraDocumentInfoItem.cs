// // Copyright (c) 2024 Engibots. All rights reserved.

namespace engimatrix.ModelObjs.Primavera;


public class PrimaveraDocumentInfoItem
{
    public string primavera_document { get; set; }
    public string primavera_document_type { get; set; }
    public string primavera_document_series { get; set; }
    public string primavera_document_number { get; set; }

    public PrimaveraDocumentInfoItem()
    {
        primavera_document = string.Empty;
        primavera_document_type = string.Empty;
        primavera_document_series = string.Empty;
        primavera_document_number = string.Empty;
    }

    public PrimaveraDocumentInfoItem(string primavera_document, string primavera_document_type, string primavera_document_series, string primavera_document_number)
    {
        this.primavera_document = primavera_document;
        this.primavera_document_type = primavera_document_type;
        this.primavera_document_series = primavera_document_series;
        this.primavera_document_number = primavera_document_number;
    }

    public override string ToString()
    {
        return $"PrimaveraDocumentInfoItem:\n" +
               $"Document: {primavera_document}\n" +
               $"Type: {primavera_document_type}\n" +
               $"Series: {primavera_document_series}\n" +
               $"Number: {primavera_document_number}\n";
    }



}