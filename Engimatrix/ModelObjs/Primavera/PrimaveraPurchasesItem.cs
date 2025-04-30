// // Copyright (c) 2024 Engibots. All rights reserved.

using System.Text.Json.Serialization;

namespace engimatrix.ModelObjs.Primavera;

using System;
using System.Collections.Generic;
using System.Text.Json.Serialization;

public class PrimaveraPurchasesItem
{
    [JsonPropertyName("CodigoTabLog")]
    public string CodigoTabLog { get; set; }

    [JsonPropertyName("ChaveLog")]
    public string ChaveLog { get; set; }

    [JsonPropertyName("EstadoBE")]
    public string EstadoBE { get; set; }

    [JsonPropertyName("Documento")]
    public string Documento { get; set; }

    [JsonPropertyName("Serie")]
    public string Serie { get; set; }

    [JsonPropertyName("Ordem")]
    public int Ordem { get; set; }

    [JsonPropertyName("Descricao")]
    public string Descricao { get; set; }

    [JsonPropertyName("SerieInactiva")]
    public bool SerieInactiva { get; set; }

    [JsonPropertyName("SeriePorDefeito")]
    public bool SeriePorDefeito { get; set; }

    [JsonPropertyName("LimiteInferior")]
    public double LimiteInferior { get; set; }

    [JsonPropertyName("LimiteSuperior")]
    public double LimiteSuperior { get; set; }

    [JsonPropertyName("Numerador")]
    public double Numerador { get; set; }

    [JsonPropertyName("DataUltimoDocumento")]
    public DateTime DataUltimoDocumento { get; set; }

    [JsonPropertyName("DataInicial")]
    public DateTime DataInicial { get; set; }

    [JsonPropertyName("DataFinal")]
    public DateTime DataFinal { get; set; }

    [JsonPropertyName("AlterarData")]
    public bool AlterarData { get; set; }

    [JsonPropertyName("IvaIncluido")]
    public bool IvaIncluido { get; set; }

    [JsonPropertyName("SugereDataSistema")]
    public bool SugereDataSistema { get; set; }

    [JsonPropertyName("NumeracaoAuto")]
    public bool NumeracaoAuto { get; set; }

    [JsonPropertyName("UtilizadaPOS")]
    public bool UtilizadaPOS { get; set; }

    [JsonPropertyName("DescricaoAbreviada")]
    public bool DescricaoAbreviada { get; set; }

    [JsonPropertyName("AbrirGaveta")]
    public bool AbrirGaveta { get; set; }

    [JsonPropertyName("ImprimirTaloesDirect")]
    public bool ImprimirTaloesDirect { get; set; }

    [JsonPropertyName("ConfigPOS")]
    public string ConfigPOS { get; set; }

    [JsonPropertyName("Config")]
    public string Config { get; set; }

    [JsonPropertyName("NumVias")]
    public int NumVias { get; set; }

    [JsonPropertyName("Previsao")]
    public bool Previsao { get; set; }

    [JsonPropertyName("EmModoEdicao")]
    public bool EmModoEdicao { get; set; }

    [JsonPropertyName("DescricaoVia01")]
    public string DescricaoVia01 { get; set; }

    [JsonPropertyName("DescricaoVia02")]
    public string DescricaoVia02 { get; set; }

    [JsonPropertyName("DescricaoVia03")]
    public string DescricaoVia03 { get; set; }

    [JsonPropertyName("DescricaoVia04")]
    public string DescricaoVia04 { get; set; }

    [JsonPropertyName("DescricaoVia05")]
    public string DescricaoVia05 { get; set; }

    [JsonPropertyName("DescricaoVia06")]
    public string DescricaoVia06 { get; set; }

    [JsonPropertyName("ArmazemSugestao")]
    public string ArmazemSugestao { get; set; }

    [JsonPropertyName("LocalSugestao")]
    public string LocalSugestao { get; set; }

    [JsonPropertyName("UtilizadoEmPMS")]
    public bool UtilizadoEmPMS { get; set; }

    [JsonPropertyName("CamposUtil")]
    public List<string> CamposUtil { get; set; }

    [JsonPropertyName("TipoLancamento")]
    public string TipoLancamento { get; set; }

    [JsonPropertyName("TipoEntidade")]
    public int TipoEntidade { get; set; }

    [JsonPropertyName("CopiaDocOriginal")]
    public bool CopiaDocOriginal { get; set; }

    [JsonPropertyName("DisponivelNoEditor")]
    public bool DisponivelNoEditor { get; set; }

    [JsonPropertyName("Conteudo")]
    public string Conteudo { get; set; }

    [JsonPropertyName("MostraEcovalor")]
    public bool MostraEcovalor { get; set; }

    [JsonPropertyName("TipoComunicacao")]
    public int TipoComunicacao { get; set; }

    [JsonPropertyName("CAESugerido")]
    public string CAESugerido { get; set; }

    [JsonPropertyName("SugereMoradaArmazem")]
    public bool SugereMoradaArmazem { get; set; }

    [JsonPropertyName("AutoFacturacao")]
    public bool AutoFacturacao { get; set; }

    [JsonPropertyName("EntidadeTipo")]
    public string EntidadeTipo { get; set; }

    [JsonPropertyName("Entidade")]
    public string Entidade { get; set; }

    [JsonPropertyName("Origem")]
    public int Origem { get; set; }

    [JsonPropertyName("SemActividadeEmpresarial")]
    public bool SemActividadeEmpresarial { get; set; }

    [JsonPropertyName("Interna")]
    public bool Interna { get; set; }

    [JsonPropertyName("eGarAbreDocumento")]
    public bool EGarAbreDocumento { get; set; }

    [JsonPropertyName("eGAR_Comunica")]
    public bool EGARComunica { get; set; }

    [JsonPropertyName("TaxFree_Comunica")]
    public bool TaxFreeComunica { get; set; }

    [JsonPropertyName("IdSerieMaquinasFiscais")]
    public string IdSerieMaquinasFiscais { get; set; }

    [JsonPropertyName("ImprimeMencao")]
    public bool ImprimeMencao { get; set; }

    [JsonPropertyName("ImpressaoPorEntidade")]
    public bool ImpressaoPorEntidade { get; set; }

    [JsonPropertyName("EstadoComunicacao")]
    public int EstadoComunicacao { get; set; }

    [JsonPropertyName("CodigoFiscal")]
    public string CodigoFiscal { get; set; }

    [JsonPropertyName("DescricaoEstadoComunicacao")]
    public string DescricaoEstadoComunicacao { get; set; }

    [JsonPropertyName("NumeradorComunicacao")]
    public int NumeradorComunicacao { get; set; }

    [JsonPropertyName("ComunicacaoManual")]
    public bool ComunicacaoManual { get; set; }

    [JsonPropertyName("ArmazemDestinoSugestao")]
    public string ArmazemDestinoSugestao { get; set; }

    [JsonPropertyName("LocalDestinoSugestao")]
    public string LocalDestinoSugestao { get; set; }

    [JsonPropertyName("SerieRappel")]
    public bool SerieRappel { get; set; }

    [JsonPropertyName("AssinaturaDigital")]
    public bool AssinaturaDigital { get; set; }

    [JsonPropertyName("Modulo")]
    public string Modulo { get; set; }

    [JsonPropertyName("AbvtApl")]
    public string AbvtApl { get; set; }

    [JsonPropertyName("Categoria")]
    public string Categoria { get; set; }

    [JsonPropertyName("DescricaoEntidade")]
    public string DescricaoEntidade { get; set; }
}
