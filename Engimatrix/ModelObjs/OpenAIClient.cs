// // Copyright (c) 2024 Engibots. All rights reserved.

using engimatrix.ModelObjs.Primavera;

namespace Engimatrix.ModelObjs;

public class Cliente
{
    public string NomeCliente { get; set; }
    public string NomeEmpresa { get; set; }
    public string Contribuinte { get; set; }
    public string Telemovel { get; set; }
    public string Email { get; set; }
    public string Morada { get; set; }
    public string Localidade { get; set; }
    public string CodPostal { get; set; }
    public string CodPostalLocalidade { get; set; }
    public string Pais { get; set; }
    public string Distrito { get; set; }

    public Cliente()
    {
        NomeCliente = string.Empty;
        NomeEmpresa = string.Empty;
        Contribuinte = string.Empty;
        Telemovel = string.Empty;
        Email = string.Empty;
        Morada = string.Empty;
        Localidade = string.Empty;
        CodPostal = string.Empty;
        CodPostalLocalidade = string.Empty;
        Pais = string.Empty;
        Distrito = string.Empty;
    }

    public override string ToString()
    {
        return $"NomeCliente: {NomeCliente} \n" +
                $"NomeEmpresa: {NomeEmpresa} \n" +
                $"Contribuinte: {Contribuinte} \n" +
                $"Telemovel: {Telemovel} \n" +
                $"Email: {Email} \n" +
                $"Morada: {Morada} \n" +
                $"Localidade: {Localidade} \n" +
                $"CodPostal: {CodPostal} \n" +
                $"CodPostalLocalidade: {CodPostalLocalidade} \n" +
                $"Pais: {Pais} \n" +
                $"Distrito: {Distrito} \n";
    }

}

public class OpenAiPrimaveraClient : PrimaveraClientItem
{
    public int? Confianca { get; set; }

}