// // Copyright (c) 2024 Engibots. All rights reserved.

namespace engimatrix.ModelObjs.Primavera;


public class PrimaveraDocument
{
    // Header & general properties
    public string? TipoEntidadeTransporte { get; set; }
    public string? EntidadeTransporte { get; set; }
    public bool TransportePropriaEntidade { get; set; }
    public string Template { get; set; } = string.Empty;
    public List<string> OnlinePaymentIds { get; set; } = new();
    public string Categoria { get; set; } = string.Empty;
    public string DescricaoEntidade { get; set; } = string.Empty;
    public string IDDiarioCaixa { get; set; } = string.Empty;
    public bool OrigemPOS { get; set; }
    public string CodigoTabLog { get; set; } = string.Empty;
    public string ChaveLog { get; set; } = string.Empty;
    public string EstadoBE { get; set; } = string.Empty;
    public string PropExcluirLog { get; set; } = string.Empty;

    // Document lines and removals
    public List<Linha> Linhas { get; set; } = new();
    public List<object> LinhasRemovidas { get; set; } = new(); // Define a proper type if available
    public List<object> Retencoes { get; set; } = new();      // Define a proper type if available
    public List<ResumoIva> ResumoIva { get; set; } = new();

    // Identification and dates
    public string ID { get; set; } = string.Empty;
    public string IDCabecMovCbl { get; set; } = string.Empty;
    public string Referencia { get; set; } = string.Empty;
    public string Grupo { get; set; } = string.Empty;
    public string Origem { get; set; } = string.Empty;
    public string Tipodoc { get; set; } = string.Empty;
    public int NumDoc { get; set; }
    public DateTime DataDoc { get; set; }
    public DateTime DataVenc { get; set; }
    public string Zona { get; set; } = string.Empty;
    public string TipoEntidade { get; set; } = string.Empty;
    public string Entidade { get; set; } = string.Empty;
    public string CondPag { get; set; } = string.Empty;
    public double DescFinanceiro { get; set; }
    public bool DocImpresso { get; set; }
    public int CBLEstado { get; set; }
    public string ModoPag { get; set; } = string.Empty;
    public double DescEntidade { get; set; }
    public string Seccao { get; set; } = string.Empty;
    public string RegimeIva { get; set; } = string.Empty;
    public string Moeda { get; set; } = string.Empty;
    public double Cambio { get; set; }
    public string Requisicao { get; set; } = string.Empty;
    public string TipoOperacao { get; set; } = string.Empty;
    public string DocsOriginais { get; set; } = string.Empty;
    public string Observacoes { get; set; } = string.Empty;
    public string Estado { get; set; } = string.Empty;
    public string ModoExp { get; set; } = string.Empty;
    public string Filial { get; set; } = string.Empty;
    public string Serie { get; set; } = string.Empty;
    public bool MoedaDaUEM { get; set; }
    public int Arredondamento { get; set; }
    public int ArredondamentoIva { get; set; }
    public double TotalMerc { get; set; }
    public double TotalEcotaxa { get; set; }
    public double TotalIva { get; set; }
    public double TotalRecargo { get; set; }
    public double TotalDesc { get; set; }
    public double TotalOutros { get; set; }
    public DateTime DataUltimaActualizacao { get; set; }

    // Customer and address
    public string Nome { get; set; } = string.Empty;
    public string Morada { get; set; } = string.Empty;
    public string Morada2 { get; set; } = string.Empty;
    public string Localidade { get; set; } = string.Empty;
    public string CodigoPostal { get; set; } = string.Empty;
    public string LocalidadeCodigoPostal { get; set; } = string.Empty;
    public string Distrito { get; set; } = string.Empty;
    public string NumContribuinte { get; set; } = string.Empty;
    public string CBLDiario { get; set; } = string.Empty;
    public int CBLNumDiario { get; set; }
    public int CBLAno { get; set; }
    public string Responsavel { get; set; } = string.Empty;
    public string Posto { get; set; } = string.Empty;
    public string Utilizador { get; set; } = string.Empty;

    // Financial & retention data
    public double PercentagemRetencao { get; set; }
    public double TotalRetencao { get; set; }
    public double TotalRetencaoGarantia { get; set; }
    public string Fluxo { get; set; } = string.Empty;
    public bool SujeitoRecargo { get; set; }
    public double TrocoValor { get; set; }
    public string TrocoMoeda { get; set; } = string.Empty;
    public double TrocoCambio { get; set; }
    public double ValorEntregue { get; set; }
    public string ValorEntregueMoeda { get; set; } = string.Empty;
    public double ValorEntregueCambio { get; set; }
    public bool EmModoEdicao { get; set; }

    // Extra fields at document level
    public List<CampoUtil> CamposUtil { get; set; } = new();

    // Additional document properties
    public string IdGDOC { get; set; } = string.Empty;
    public bool EmImpressao { get; set; }
    public bool OcultaCargaDescarga { get; set; }
    public string MotivoEmissao { get; set; } = string.Empty;
    public string DescricaoMotivoEmissao { get; set; } = string.Empty;
    public bool Fechado { get; set; }
    public bool Anulado { get; set; }
    public List<object>? Devolucoes { get; set; } // Define a type if available

    // Additional identification and tracking
    public string IDObra { get; set; } = string.Empty;
    public string WBSItem { get; set; } = string.Empty;
    public string IDEstorno { get; set; } = string.Empty;
    public string IDAvenca { get; set; } = string.Empty;
    public double CambioMBase { get; set; }
    public double CambioMAlt { get; set; }
    public string LocalOperacao { get; set; } = string.Empty;
    public string SimboloMoeda { get; set; } = string.Empty;
    public string Versao { get; set; } = string.Empty;
    public string ContaDomiciliacao { get; set; } = string.Empty;
    public string DE_IL { get; set; } = string.Empty;
    public string TipoLancamento { get; set; } = string.Empty;
    public string TipoEntidadeFac { get; set; } = string.Empty;
    public string EntidadeFac { get; set; } = string.Empty;
    public string NomeFac { get; set; } = string.Empty;
    public string MoradaFac { get; set; } = string.Empty;
    public string Morada2Fac { get; set; } = string.Empty;
    public string LocalidadeFac { get; set; } = string.Empty;
    public string CodigoPostalFac { get; set; } = string.Empty;
    public string LocalidadeCodigoPostalFac { get; set; } = string.Empty;
    public string NumContribuinteFac { get; set; } = string.Empty;
    public string DistritoFac { get; set; } = string.Empty;
    public double TotalIEC { get; set; }
    public bool GeraPendentePorLinha { get; set; }
    public string Assinatura { get; set; } = string.Empty;
    public string VersaoAssinatura { get; set; } = string.Empty;
    public bool DocumentoCertificado { get; set; }
    public int RegimeIvaReembolsos { get; set; }
    public int EspacoFiscal { get; set; }
    public DateTime DataGravacao { get; set; }
    public bool Rascunho { get; set; }
    public bool AntRascunho { get; set; }
    public string Pais { get; set; } = string.Empty;
    public string PaisFac { get; set; } = string.Empty;
    public bool CambioADataDoc { get; set; }
    public string IdOportunidade { get; set; } = string.Empty;
    public int NumProposta { get; set; }
    public string IDDocB2B { get; set; } = string.Empty;
    public bool B2BAssociacaoManual { get; set; }
    public bool B2BTrataTrans { get; set; }
    public bool B2BEnvioNaGravacao { get; set; }
    public string RefDocOrig { get; set; } = string.Empty;
    public string RefTipoDocOrig { get; set; } = string.Empty;
    public string RefSerieDocOrig { get; set; } = string.Empty;
    public string Certificado { get; set; } = string.Empty;
    public string CertificadoRecuperacao { get; set; } = string.Empty;
    public string IdDocOrigem { get; set; } = string.Empty;
    public string ModuloOrigem { get; set; } = string.Empty;
    public string ContratoFactoring { get; set; } = string.Empty;
    public string ATDocCodeID { get; set; } = string.Empty;

    // Nested objects
    public CargaDescarga CargaDescarga { get; set; } = new();
    public string EntidadeDescarga { get; set; } = string.Empty;
    public string LocalDescarga { get; set; } = string.Empty;
    public DateTime? DataHoraDescarga { get; set; }
    public bool UtilizaMoradaAlternativaEntreg { get; set; }
    public string MoradaAlternativaEntrega { get; set; } = string.Empty;
    public string TipoEntidadeEntrega { get; set; } = string.Empty;
    public string EntidadeEntrega { get; set; } = string.Empty;
    public string NomeEntrega { get; set; } = string.Empty;
    public string MoradaEntrega { get; set; } = string.Empty;
    public string Morada2Entrega { get; set; } = string.Empty;
    public string LocalidadeEntrega { get; set; } = string.Empty;
    public string CodPostalEntrega { get; set; } = string.Empty;
    public string CodPostalLocalidadeEntrega { get; set; } = string.Empty;
    public string DistritoEntrega { get; set; } = string.Empty;
    public string LocalCarga { get; set; } = string.Empty;
    public DateTime DataHoraCarga { get; set; }
    public string Matricula { get; set; } = string.Empty;
    public string CAE { get; set; } = string.Empty;
    public bool Resumo { get; set; }
    public double TotalIS { get; set; }
    public string IDRegularizacao { get; set; } = string.Empty;
    public List<object> ResumoIS { get; set; } = new();
    public bool TrataIvaCaixa { get; set; }
    public string Documento { get; set; } = string.Empty;
    public string IdContrato { get; set; } = string.Empty;
    public bool CalculoManual { get; set; }
    public string TipoTerceiro { get; set; } = string.Empty;
    public string IdAgendamento { get; set; } = string.Empty;
    public string TipoFiscal { get; set; } = string.Empty;
    public bool HoraDefinida { get; set; }
    public bool ServContinuados { get; set; }
    public TaxFree TaxFree { get; set; } = new();
    public double PercentagemCativacao { get; set; }
    public string ATCUD { get; set; } = string.Empty;
    public string RefCodFiscalDocOrig { get; set; } = string.Empty;
    public string AbvtApl { get; set; } = string.Empty;
}

public class Linha
{
    public string ChaveLog { get; set; } = string.Empty;
    public string EstadoBE { get; set; } = string.Empty;
    public int EstadoBD { get; set; }
    public string PropExcluirLog { get; set; } = string.Empty;
    public List<object> NumerosSerie { get; set; } = new();
    public string Artigo { get; set; } = string.Empty;
    public double Desconto1 { get; set; }
    public double Desconto2 { get; set; }
    public double Desconto3 { get; set; }
    public string Descricao { get; set; } = string.Empty;
    public double PercIncidenciaIVA { get; set; }
    public double TaxaRecargo { get; set; }
    public double TaxaIva { get; set; }
    public string CodIva { get; set; } = string.Empty;
    public string Unidade { get; set; } = string.Empty;
    public double FactorConv { get; set; }
    public double ArredFConv { get; set; }
    public double Quantidade { get; set; }
    public double QuantidadeTransformar { get; set; }
    public double DifPCMedio { get; set; }
    public int TipoCustoPrevisto { get; set; }
    public double CustoPrevisto { get; set; }
    public double Margem { get; set; }
    public double PercentagemMargem { get; set; }
    public double PrecUnit { get; set; }
    public string RegimeIva { get; set; } = string.Empty;
    public string TipoLinha { get; set; } = string.Empty;
    public string Armazem { get; set; } = string.Empty;
    public string Localizacao { get; set; } = string.Empty;
    public string MovStock { get; set; } = string.Empty;
    public bool MovimentaStock { get; set; }
    public DateTime DataStock { get; set; }
    public DateTime DataEntrega { get; set; }
    public double DescontoComercial { get; set; }
    public string Formula { get; set; } = string.Empty;
    public double VariavelA { get; set; }
    public double VariavelB { get; set; }
    public double VariavelC { get; set; }
    public double QuantFormula { get; set; }
    public double Comissao { get; set; }
    public string Lote { get; set; } = string.Empty;
    public double PrecoLiquido { get; set; }
    public double TotalIliquido { get; set; }
    public double TotalDA { get; set; }
    public double TotalDC { get; set; }
    public double TotalDF { get; set; }
    public double TotalRecargo { get; set; }
    public double TotalIva { get; set; }
    public double TotalIvaInversaoSujPassivo { get; set; }
    public string Vendedor { get; set; } = string.Empty;
    public string IntrastatCodigoPautal { get; set; } = string.Empty;
    public double IntrastatMassaLiq { get; set; }
    public string IntrastatRegiao { get; set; } = string.Empty;
    public double IntrastatValorLiq { get; set; }
    public int NumLinDocOriginal { get; set; }
    public string IdLinha { get; set; } = string.Empty;
    public List<CampoUtil> CamposUtil { get; set; } = new();
    public string Conteudo { get; set; } = string.Empty;
    public string IDLinhaOriginal { get; set; } = string.Empty;
    public double QuantReservada { get; set; }
    public double QuantSatisfeita { get; set; }
    public string IDHistorico { get; set; } = string.Empty;
    public string EstadoAdiantamento { get; set; } = string.Empty;
    public string Estado { get; set; } = string.Empty;
    public string EstadoOrigem { get; set; } = string.Empty;
    public bool Devolucao { get; set; }
    public double PCMDevolucao { get; set; }
    public bool Fechado { get; set; }
    public string IDObra { get; set; } = string.Empty;
    public string IDItem { get; set; } = string.Empty;
    public string ItemCod { get; set; } = string.Empty;
    public string ItemDesc { get; set; } = string.Empty;
    public string WBSItem { get; set; } = string.Empty;
    public int ClasseActividade { get; set; }
    public int SubEmpreitada { get; set; }
    public int Categoria { get; set; }
    public string TipoAuto { get; set; } = string.Empty;
    public string AutoID { get; set; } = string.Empty;
    public string IdLinhaPai { get; set; } = string.Empty;
    public string ModuloOrigemCopia { get; set; } = string.Empty;
    public string IdLinhaOrigemCopia { get; set; } = string.Empty;
    public string ContaCBL { get; set; } = string.Empty;
    public string CCustoCBL { get; set; } = string.Empty;
    public string AnaliticaCBL { get; set; } = string.Empty;
    public string FuncionalCBL { get; set; } = string.Empty;
    public double PercIvaDedutivel { get; set; }
    public double IvaNaoDedutivel { get; set; }
    public int TipoIVA { get; set; }
    public string IDB2BLinhaOrig { get; set; } = string.Empty;
    public int B2BNumLinhaOrig { get; set; }
    public List<object> LinhasHistoricoResiduo { get; set; } = new();
    public List<object> LinhasHistoricoIEC { get; set; } = new();
    public double Ecotaxa { get; set; }
    public double TotalEcotaxa { get; set; }
    public string CodIvaEcotaxa { get; set; } = string.Empty;
    public double TaxaIvaEcotaxa { get; set; }
    public double IvaRegraCalculo { get; set; }
    public string MotivoEstorno { get; set; } = string.Empty;
    public string IdLinhaEstorno { get; set; } = string.Empty;
    public string EstadoPendente { get; set; } = string.Empty;
    public double ValorIEC { get; set; }
    public double TotalIEC { get; set; }
    public string CodIvaIEC { get; set; } = string.Empty;
    public double TaxaIvaIEC { get; set; }
    public string TipoOperacao { get; set; } = string.Empty;
    public string AlternativaGPR { get; set; } = string.Empty;
    public string IntrastatPaisOrigem { get; set; } = string.Empty;
    public double BaseCalculoIncidencia { get; set; }
    public double BaseIncidencia { get; set; }
    public int RegraCalculoIncidencia { get; set; }
    public DadosAdiantamento DadosAdiantamento { get; set; } = new();
    public DadosImpostoSelo DadosImpostoSelo { get; set; } = new();
    public double ValorLiquidoDesconto { get; set; }
    public double IvaValorDesconto { get; set; }
    public string IdContrato { get; set; } = string.Empty;
    public string ContratoID { get; set; } = string.Empty;
    public string ProcessoID { get; set; } = string.Empty;
    public double QuantCopiada { get; set; }
    public string OrganicaCBL { get; set; } = string.Empty;
    public string OrcamentalCBL { get; set; } = string.Empty;
    public string FonteCBL { get; set; } = string.Empty;
    public string ActividadeCBL { get; set; } = string.Empty;
    public string ProgMedidaCBL { get; set; } = string.Empty;
    public string ClassEconCBL { get; set; } = string.Empty;
    public string ProcessoCBL { get; set; } = string.Empty;
    public int LinhaProcessoCBL { get; set; }
    public double AcertoProcessoCBL { get; set; }
    public bool ValorProcessoCIvaCBL { get; set; }
    public int NumEstado { get; set; }
    public int NumEstadoLng { get; set; }
    public string IdAgendamento { get; set; } = string.Empty;
    public string INV_EstadoOrigem { get; set; } = string.Empty;
    public string INV_EstadoDestino { get; set; } = string.Empty;
    public string INV_IDReserva { get; set; } = string.Empty;
    public bool MovimentoSistema { get; set; }
    public List<object> ReservaStock { get; set; } = new();
    public double ValorProcessoAnoN { get; set; }
    public double ValorProcessoAnoN1 { get; set; }
    public double ValorProcessoAnoN2 { get; set; }
    public double ValorProcessoAnoN3 { get; set; }
    public double ValorProcessoAnoN4 { get; set; }
    public double ValorProcessoAnosSeguintes { get; set; }
    public string NumCompromisso { get; set; } = string.Empty;
    public int NumLinha { get; set; }
    public string AbvtApl { get; set; } = string.Empty;
    public string DescricaoEntidade { get; set; } = string.Empty;
}

public class CampoUtil
{
    public string Conteudo { get; set; } = string.Empty;
    public string Nome { get; set; } = string.Empty;
    public object? Valor { get; set; }
    public object? Objecto { get; set; }
    public int Tipo { get; set; }
    public string ChaveLog { get; set; } = string.Empty;
    public string EstadoBE { get; set; } = string.Empty;
    public int TipoSimplificado { get; set; }
}

public class ResumoIva
{
    public string ChaveLog { get; set; } = string.Empty;
    public string EstadoBE { get; set; } = string.Empty;
    public string ID { get; set; } = string.Empty;
    public string Modulo { get; set; } = string.Empty;
    public string Filial { get; set; } = string.Empty;
    public string Serie { get; set; } = string.Empty;
    public string Tipodoc { get; set; } = string.Empty;
    public int NumDoc { get; set; }
    public double TaxaRecargo { get; set; }
    public double TaxaIva { get; set; }
    public string CodIva { get; set; } = string.Empty;
    public double Incidencia { get; set; }
    public double Valor { get; set; }
    public double ValorRecargo { get; set; }
    public bool EmModoEdicao { get; set; }
    public bool IVAIndeferido { get; set; }
    public string IDOrig { get; set; } = string.Empty;
    public string IdLinhaLiq { get; set; } = string.Empty;
    public string IdHistorico { get; set; } = string.Empty;
    public string Conteudo { get; set; } = string.Empty;
    public double AcertoIVA { get; set; }
    public double IvaNaoDedutivel { get; set; }
    public double ValorDesconto { get; set; }
    public string AbvtApl { get; set; } = string.Empty;
    public string Categoria { get; set; } = string.Empty;
    public string DescricaoEntidade { get; set; } = string.Empty;
}

public class DadosAdiantamento
{
    public string Conteudo { get; set; } = string.Empty;
    public string MoedaDocOrig { get; set; } = string.Empty;
    public double CambioDocOrig { get; set; }
    public double CambioMBaseDocOrig { get; set; }
    public double CambioMAltDocOrig { get; set; }
    public string IDHistorico { get; set; } = string.Empty;
    public string Estado { get; set; } = string.Empty;
    public double DifArredondamentoMBase { get; set; }
    public double DifArredondamentoMAlt { get; set; }
    public double DifCambioMAlt { get; set; }
    public double DifCambioMBase { get; set; }
    public string EstadoBE { get; set; } = string.Empty;
    public string ChaveLog { get; set; } = string.Empty;
    public string CodigoTabLog { get; set; } = string.Empty;
    public string PropExcluirLog { get; set; } = string.Empty;
    public string AbvtApl { get; set; } = string.Empty;
    public string Categoria { get; set; } = string.Empty;
    public string DescricaoEntidade { get; set; } = string.Empty;
}

public class DadosImpostoSelo
{
    public int Ano { get; set; }
    public string Selo { get; set; } = string.Empty;
    public double IncidenciaIS { get; set; }
    public double ValorIS { get; set; }
    public string Conteudo { get; set; } = string.Empty;
    public string EstadoBE { get; set; } = string.Empty;
    public string ChaveLog { get; set; } = string.Empty;
    public string CodigoTabLog { get; set; } = string.Empty;
    public string PropExcluirLog { get; set; } = string.Empty;
    public string AbvtApl { get; set; } = string.Empty;
    public string Categoria { get; set; } = string.Empty;
    public string DescricaoEntidade { get; set; } = string.Empty;
}

public class CargaDescarga
{
    public string Conteudo { get; set; } = string.Empty;
    public string EstadoBE { get; set; } = string.Empty;
    public string CodigoTabLog { get; set; } = string.Empty;
    public string ChaveLog { get; set; } = string.Empty;
    public string Modulo { get; set; } = string.Empty;
    public string EntidadeDescarga { get; set; } = string.Empty;
    public string LocalDescarga { get; set; } = string.Empty;
    public DateTime DataHoraDescarga { get; set; }
    public bool UsaMoradaAlternativaEntrega { get; set; }
    public string MoradaAlternativaEntrega { get; set; } = string.Empty;
    public string TipoEntidadeEntrega { get; set; } = string.Empty;
    public string EntidadeEntrega { get; set; } = string.Empty;
    public string NomeEntrega { get; set; } = string.Empty;
    public string MoradaEntrega { get; set; } = string.Empty;
    public string Morada2Entrega { get; set; } = string.Empty;
    public string LocalidadeEntrega { get; set; } = string.Empty;
    public string CodPostalEntrega { get; set; } = string.Empty;
    public string CodPostalLocalidadeEntrega { get; set; } = string.Empty;
    public string DistritoEntrega { get; set; } = string.Empty;
    public string PaisEntrega { get; set; } = string.Empty;
    public string LocalCarga { get; set; } = string.Empty;
    public DateTime DataHoraCarga { get; set; }
    public string Matricula { get; set; } = string.Empty;
    public string MoradaCarga { get; set; } = string.Empty;
    public string Morada2Carga { get; set; } = string.Empty;
    public string LocalidadeCarga { get; set; } = string.Empty;
    public string CodPostalCarga { get; set; } = string.Empty;
    public string CodPostalLocalidadeCarga { get; set; } = string.Empty;
    public string DistritoCarga { get; set; } = string.Empty;
    public string PaisCarga { get; set; } = string.Empty;
    public string ATDocCodeID { get; set; } = string.Empty;
    public object? CodigoIEC { get; set; }
    public bool IsentoIEC { get; set; }
    public object? CodigoIsencaoIEC { get; set; }
    public string AbvtApl { get; set; } = string.Empty;
    public string Categoria { get; set; } = string.Empty;
    public string DescricaoEntidade { get; set; } = string.Empty;
}

public class TaxFree
{
    public string Conteudo { get; set; } = string.Empty;
    public string? IdCabecDoc { get; set; }
    public string? CodigoTaxFree { get; set; }
    public double ValorCaucao { get; set; }
    public double ValorIva { get; set; }
    public double ValorDocumento { get; set; }
    public bool SujeitoTaxFree { get; set; }
    public string Passaporte { get; set; } = string.Empty;
    public string PaisEmissorPassaporte { get; set; } = string.Empty;
    public DateTime DataNascimento { get; set; }
    public string TipoEntidadeBroker { get; set; } = string.Empty;
    public string Broker { get; set; } = string.Empty;
    public List<ResumoIva> ResumoIva { get; set; } = new();
    public string? EstadoBE { get; set; }
    public string CodigoTabLog { get; set; } = string.Empty;
    public string ChaveLog { get; set; } = string.Empty;
    public string AbvtApl { get; set; } = string.Empty;
    public string Categoria { get; set; } = string.Empty;
    public string DescricaoEntidade { get; set; } = string.Empty;
}
