// // Copyright (c) 2024 Engibots. All rights reserved.

using System.Text;

namespace engimatrix.ModelObjs.Primavera;

public class PrimaveraDocumentCreationResponse
{
    public string? Version { get; set; }
    public int StatusCode { get; set; }
    public string? ErrorMessage { get; set; }
    public required List<PrimaveraDocumentResult>? Results { get; set; }

    public override string ToString()
    {
        StringBuilder message = new($"PrimaveraDocumentResponse: Version={Version}, StatusCode={StatusCode}, ErrorMessage={ErrorMessage}, Results=\n");
        if (Results == null)
        {
            message = message.Append("null");
            return message.ToString();
        }

        foreach (PrimaveraDocumentResult result in Results)
        {
            message = message.AppendLine(result.ToString());
        }

        return message.ToString();
    }
}

public class PrimaveraDocumentResult
{
    public string? Conteudo { get; set; }
    public string? Nome { get; set; }
    public object? Valor { get; set; }
    public object? Objecto { get; set; }
    public int Tipo { get; set; }
    public string? ChaveLog { get; set; }
    public string? EstadoBE { get; set; }
    public int TipoSimplificado { get; set; }
    public override string ToString()
    {
        return $"PrimaveraDocumentResult: Conteudo={Conteudo}, Nome={Nome}, Valor={Valor}, Objecto={Objecto}, Tipo={Tipo}, ChaveLog={ChaveLog}, EstadoBE={EstadoBE}, TipoSimplificado={TipoSimplificado}";
    }
}
