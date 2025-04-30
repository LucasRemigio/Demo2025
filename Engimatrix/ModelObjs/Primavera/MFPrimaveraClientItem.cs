// // Copyright (c) 2024 Engibots. All rights reserved.

namespace engimatrix.ModelObjs.Primavera;
public class MFPrimaveraClientItem
{
    public string? Cliente { get; set; }
    public string? Nome { get; set; }
    public string? Contribuinte { get; set; }
    public string? Morada { get; set; }
    public string? Morada1 { get; set; }
    public string? Localidade { get; set; }
    public string? CodPostal { get; set; }
    public string? CodPostalLocalidade { get; set; }
    public string? Pais { get; set; }
    public string? Distrito { get; set; }
    public string? Telemovel { get; set; }
    public string? Email { get; set; }
    public string? CondPag { get; set; }
    public string? ModoPag { get; set; }
    public string? Moeda { get; set; }
    public int TipoPvp { get; set; }
    public double Desconto { get; set; }
    public bool ClienteAnulado { get; set; }
    public string? Avaliacao { get; set; }
    public string? CodCesce { get; set; }
    public string? PlafoundCesce { get; set; }
    public string? EmailCliente { get; set; }
    public string? EmailQualidade { get; set; }
    public string? TipoTerceiro { get; set; }
    public string? Carro { get; set; }
}

public class MFPrimaveraClientItemNoAuth
{
    public string? Cliente { get; set; }
    public string? Nome { get; set; }
    public string? Email { get; set; }
    public string? Carro { get; set; }
}
