// // Copyright (c) 2024 Engibots. All rights reserved.

using System.ComponentModel.DataAnnotations;

namespace engimatrix.ModelObjs.Primavera;

public class PrimaveraDocumentLineCreationItem
{
    public string? Artigo { get; set; }
    public string? Descricao { get; set; }
    public string? Unidade { get; set; }
    public decimal? Quantidade { get; set; }
    public decimal? PrecUnit { get; set; }
    public decimal? Desconto1 { get; set; }
    public decimal? Desconto2 { get; set; }
    public decimal? Desconto3 { get; set; }
    public decimal? TaxaIva { get; set; }
    public string? CodIva { get; set; }
    /*
        As Encomendas têm de ser criadas de forma igual ao Orçamento, mas têm um detalhe importante relativo à rastreabilidade dos documentos no ERP.
        Para cada linha da Encomenda, tenho de indicar o ID de linha do Orçamento relacionado.
        Para isso devem preencher nas linhas o campo "IDLinhaOriginal": " UID da linha do orçamento relacionado "
    */
    public string? IDLinhaOriginal { get; set; }


    // Special lines (e.g. comments, portes) can use this property.
    public string? TipoLinha { get; set; }

    // Additional custom fields for the line
    public List<CampoUtilCreationItem>? CamposUtil { get; set; }
}

public class CampoUtilCreationItem
{
    public string? Nome { get; set; }
    public object? Valor { get; set; }
}

public class PrimaveraDocumentCreationItem
{
    [Required]
    public string Filial { get; set; }

    [Required]
    public string Tipodoc { get; set; }

    [Required]
    public string Serie { get; set; }

    [Required]
    public string TipoEntidade { get; set; }

    [Required]
    public string Entidade { get; set; }

    [Required]
    public DateTime DataDoc { get; set; }

    [Required]
    public bool HoraDefinida { get; set; }

    [Required]
    public decimal Cambio { get; set; }

    public string? Nome { get; set; }
    public string? Morada { get; set; }
    public string? Morada2 { get; set; }
    public string? Localidade { get; set; }
    public string? CodigoPostal { get; set; }
    public string? LocalidadeCodigoPostal { get; set; }
    public string? Pais { get; set; }
    public string? Distrito { get; set; }
    public string? NumContribuinte { get; set; }
    public string? Referencia { get; set; }
    public string? ModoPag { get; set; }
    public string? CondPag { get; set; }
    public string? ModoExp { get; set; }
    public string? Moeda { get; set; }
    public string? TipoOperacao { get; set; }
    public string? Seccao { get; set; }
    public string? Origem { get; set; }
    public string? EspacoFiscal { get; set; }
    public string? RegimeIva { get; set; }
    public string? RegimeIvaReembolsos { get; set; }
    public string? LocalOperacao { get; set; }
    public string? TipoEntidadeFac { get; set; }
    public string? EntidadeFac { get; set; }
    public string? NomeFac { get; set; }
    public string? MoradaFac { get; set; }
    public string? Morada2Fac { get; set; }
    public string? LocalidadeFac { get; set; }
    public string? CodigoPostalFac { get; set; }
    public string? LocalidadeCodigoPostalFac { get; set; }
    public string? NumContribuinteFac { get; set; }
    public string? PaisFac { get; set; }
    public string? DistritoFac { get; set; }
    public string? EntidadeDescarga { get; set; }
    public string? LocalDescarga { get; set; }
    public string? TipoEntidadeEntrega { get; set; }
    public string? EntidadeEntrega { get; set; }
    public string? NomeEntrega { get; set; }
    public string? MoradaEntrega { get; set; }
    public string? Morada2Entrega { get; set; }
    public string? LocalidadeEntrega { get; set; }
    public string? CodPostalEntrega { get; set; }
    public string? CodPostalLocalidadeEntrega { get; set; }
    public string? DistritoEntrega { get; set; }
    public string? Observacoes { get; set; }
    public List<PrimaveraDocumentLineCreationItem>? Linhas { get; set; }

    public PrimaveraDocumentCreationItem()
    {
    }

    public override string ToString()
    {
        return $"Filial: {Filial}, Tipodoc: {Tipodoc}, Serie: {Serie}, TipoEntidade: {TipoEntidade}, Entidade: {Entidade}, " +
            $"DataDoc: {DataDoc}, HoraDefinida: {HoraDefinida}, Cambio: {Cambio}, Nome: {Nome}, Morada: {Morada}, Morada2: {Morada2}, " +
            $"Localidade: {Localidade}, CodigoPostal: {CodigoPostal}, LocalidadeCodigoPostal: {LocalidadeCodigoPostal}, Pais: {Pais}, " +
            $"Distrito: {Distrito}, NumContribuinte: {NumContribuinte}, Referencia: {Referencia}, ModoPag: {ModoPag}, CondPag: {CondPag}, " +
            $"ModoExp: {ModoExp}, Moeda: {Moeda}, TipoOperacao: {TipoOperacao}, Seccao: {Seccao}, Origem: {Origem}, EspacoFiscal: {EspacoFiscal}, " +
            $"RegimeIva: {RegimeIva}, RegimeIvaReembolsos: {RegimeIvaReembolsos}, LocalOperacao: {LocalOperacao}, TipoEntidadeFac: {TipoEntidadeFac}, " +
            $"EntidadeFac: {EntidadeFac}, NomeFac: {NomeFac}, MoradaFac: {MoradaFac}, Morada2Fac: {Morada2Fac}, LocalidadeFac: {LocalidadeFac}, " +
            $"CodigoPostalFac: {CodigoPostalFac}, LocalidadeCodigoPostalFac: {LocalidadeCodigoPostalFac}, NumContribuinteFac: {NumContribuinteFac}, " +
            $"PaisFac: {PaisFac}, DistritoFac: {DistritoFac}, EntidadeDescarga: {EntidadeDescarga}, LocalDescarga: {LocalDescarga}, " +
            $"TipoEntidadeEntrega: {TipoEntidadeEntrega}, EntidadeEntrega: {EntidadeEntrega}, NomeEntrega: {NomeEntrega}, MoradaEntrega: {MoradaEntrega}, " +
            $"Morada2Entrega: {Morada2Entrega}, LocalidadeEntrega: {LocalidadeEntrega}, CodPostalEntrega: {CodPostalEntrega}, " +
            $"CodPostalLocalidadeEntrega: {CodPostalLocalidadeEntrega}, DistritoEntrega: {DistritoEntrega}, Observacoes: {Observacoes}, Linhas: {Linhas}";
    }
}
