// // Copyright (c) 2024 Engibots. All rights reserved.

namespace Engimatrix.ModelObjs;

public class Produto
{
    public int Id { get; set; }
    public string? IdProduto { get; set; }
    public string? produtoSolicitado { get; set; }
    public CaracteristicasProduto? CaracteristicasProduto { get; set; }
    public string? TraducaoNomeProduto { get; set; }
    public string? MedidasProduto { get; set; }
    public string? Quantidade { get; set; }
    public string? UnidadeQuantidade { get; set; }
    public string? Justificacao { get; set; }
    public string? Confianca { get; set; }

    public override string ToString()
    {
        return $"Id: {Id}, \n" +
            $"IdProduto: {IdProduto}, \n" +
            $"produtoSolicitado: {produtoSolicitado}, \n" +
            $"TraducaoNomeProduto: {TraducaoNomeProduto}, \n" +
            $"CaracteristicasProduto - TipoDeProduto: {CaracteristicasProduto.TipoDeProduto}, \n" +
            $"CaracteristicasProduto - TipoMaterial: {CaracteristicasProduto.TipoMaterial}, \n" +
            $"CaracteristicasProduto - FormaProduto: {CaracteristicasProduto.FormaProduto}, \n" +
            $"CaracteristicasProduto - FinalizacaoProduto: {CaracteristicasProduto.FinalizacaoProduto}, \n" +
            $"CaracteristicasProduto - SuperficieProduto: {CaracteristicasProduto.SuperficieProduto}, \n" +
            $"CaracteristicasProduto - Dimensoes - Comprimento: {CaracteristicasProduto.Dimensoes.Comprimento}, \n" +
            $"CaracteristicasProduto - Dimensoes - Largura: {CaracteristicasProduto.Dimensoes.Largura}, \n" +
            $"CaracteristicasProduto - Dimensoes - Altura: {CaracteristicasProduto.Dimensoes.Altura}, \n" +
            $"CaracteristicasProduto - Dimensoes - Diametro: {CaracteristicasProduto.Dimensoes.Diametro}, \n" +
            $"MedidasProduto: {MedidasProduto}, \n" +
            $"Quantidade: {Quantidade}, \n" +
            $"UnidadeQuantidade: {UnidadeQuantidade}, \n" +
            $"Justificacao: {Justificacao}, \n" +
            $"Confianca: {Confianca}";
    }
}

public class CaracteristicasProduto
{
    public string? TipoDeProduto { get; set; }
    public string? TipoMaterial { get; set; }
    public string? FormaProduto { get; set; }
    public string? FinalizacaoProduto { get; set; }
    public string? SuperficieProduto { get; set; }
    public Dimensoes? Dimensoes { get; set; }
}

public class Dimensoes
{
    public string? Comprimento { get; set; }
    public string? Largura { get; set; }
    public string? Altura { get; set; }
    public string? Espessura { get; set; }
    public string? Diametro { get; set; }
}

public class ProductExtractionOpenAIResponse
{
    public string? Confianca { get; set; }
    public List<Produto>? Produtos { get; set; }
}

public class ProdutoComparado
{
    public required string IdProdutoSolicitado { get; set; }
    public required string NomeProdutoSolicitado { get; set; }
    public required string MedidasProdutoSolicitado { get; set; }
    public required string IdProdutoCatalogo { get; set; }
    public required string NomeProdutoCatalogo { get; set; }
    public required string MedidasProdutoCatalogo { get; set; }
    public required string Quantidade { get; set; }
    public required string UnidadeQuantidade { get; set; }
    public string? Justificacao { get; set; }
    public required string Confianca { get; set; }
    public bool IsMatchInstantaneo { get; set; }

    public override string ToString()
    {
        return $"IdProdutoSolicitado: {IdProdutoSolicitado}, \n" +
            $"NomeProdutoSolicitado: {NomeProdutoSolicitado}, \n" +
            $"MedidasProdutoSolicitado: {MedidasProdutoSolicitado}, \n" +
            $"IdProdutoCatalogo: {IdProdutoCatalogo}, \n" +
            $"NomeProdutoCatalogo: {NomeProdutoCatalogo}, \n" +
            $"MedidasProdutoCatalogo: {MedidasProdutoCatalogo}, \n" +
            $"Quantidade: {Quantidade}, \n" +
            $"UnidadeQuantidade: {UnidadeQuantidade}, \n" +
            $"Justificacao: {Justificacao}, \n" +
            $"Confianca: {Confianca}";
    }
}

public class ProdutoRaw
{
    public int Id { get; set; }
    public string? ProdutoSolicitado { get; set; }
    public string? Medidas { get; set; }
    public string? Altura { get; set; }
    public string? Largura { get; set; }
    public string? Comprimento { get; set; }
    public string? Espessura { get; set; }
    public string? Diametro { get; set; }
    public string? Quantidade { get; set; }
    public string? UnidadeQuantidade { get; set; }
    public string? Justificacao { get; set; }
    public string? TipoProduto { get; set; }
    public int TipoProdutoId { get; set; }
}

public class ProdutoComTipo
{
    public int Id { get; set; }
    public string ProdutoSolicitado { get; set; }
    public int TipoProdutoId { get; set; }
    public string TipoProduto { get; set; }
    public string Justificacao { get; set; }
}

public class ProdutosResponse
{
    public List<ProdutoRaw> Produtos { get; set; }

    public ProdutosResponse()
    {
        Produtos = [];
    }
}

public class CheckClientAdjudicated
{
    public string? pediu_retificacao { get; set; }
    public string? cliente_adjudicou { get; set; }
    public string? confianca { get; set; }
    public string? justificacao { get; set; }

    public override string ToString()
    {
        return $"pediu_retificacao: {pediu_retificacao}, \n" +
            $"cliente_adjudicou: {cliente_adjudicou}, \n" +
            $"confianca: {confianca}, \n" +
            $"justificacao: {justificacao}";
    }
}
