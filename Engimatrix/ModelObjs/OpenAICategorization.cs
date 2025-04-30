// // Copyright (c) 2024 Engibots. All rights reserved.

namespace Engimatrix.ModelObjs;

public class OpenAiEmailCategorization
{
    public string? confianca { get; set; }
    public string? categoria { get; set; }
    public string? justificacao { get; set; }
    public string? email_remetente { get; set; }
    public string? confianca_reencaminhamento { get; set; }
}

public class OpenAiEmailCategorizationBuilder
{
    private readonly OpenAiEmailCategorization _openAiEmailCategorization = new();

    public OpenAiEmailCategorizationBuilder SetConfianca(string? confianca)
    {
        _openAiEmailCategorization.confianca = confianca;
        return this;
    }

    public OpenAiEmailCategorizationBuilder SetCategoria(string? categoria)
    {
        _openAiEmailCategorization.categoria = categoria;
        return this;
    }

    public OpenAiEmailCategorizationBuilder SetJustificacao(string? justificacao)
    {
        _openAiEmailCategorization.justificacao = justificacao;
        return this;
    }

    public OpenAiEmailCategorizationBuilder SetEmailRemetente(string? email_remetente)
    {
        _openAiEmailCategorization.email_remetente = email_remetente;
        return this;
    }

    public OpenAiEmailCategorizationBuilder SetConfiancaReencaminhamento(string? confianca_reencaminhamento)
    {
        _openAiEmailCategorization.confianca_reencaminhamento = confianca_reencaminhamento;
        return this;
    }

    public OpenAiEmailCategorization Build()
    {
        return _openAiEmailCategorization;
    }
}