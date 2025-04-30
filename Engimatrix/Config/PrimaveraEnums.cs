
using System.ComponentModel;
using System.Reflection;

namespace engimatrix.Config;

public static class PrimaveraEnums
{
    public enum TipoDoc
    {
        // ECL - Encomenda de Compra
        ECL,
        // ORC - Orçamento de Compra 
        ORC,
    }

    public enum EstadoEncomenda
    {
        // P - Pendente
        // T - Transformado
        // R - Rejeitado\\Anulado
        PENDENTE = 'P',
        TRANSFORMADO = 'T',
        REJEITADO_ANULADO = 'R',
    }

    public enum TipoTerceiroSegmento
    {
        NENHUM = 50,
        REVENDEDOR = 51,
        CONSTRUTORES = 52,
        SERRALHARIA = 53,
        PICHELEIROS = 54,
        INDUSTRIA_METALOMECANICA = 55
    }

    public enum AnulaDocumentoMotivo
    {
        [Description("001")]
        DEVOLUCAO_ARTIGOS,

        [Description("002")]
        ERRO_NAS_QUANTIDADES_PRECOS,

        [Description("003")]
        ERRO_ENTIDADE,

        [Description("C01")]
        ERRO_DATA,

        [Description("C02")]
        ALTERACAO_DIARIO_DOCUMENTO,

        [Description("C03")]
        ERRO_CONTA,

        [Description("C04")]
        ERRO_VALOR,

        [Description("C05")]
        ALTERACAO_CLASSE_IVA,

        [Description("C06")]
        VERIFICACAO_PERIODICA_CONTABILIDADE
    }

    public static string GetCode(this AnulaDocumentoMotivo reason)
    {
        // Get the field info; if it's null, fall back to the enum's ToString()
        FieldInfo? fieldInfo = reason.GetType().GetField(reason.ToString());
        if (fieldInfo is null)
        {
            return reason.ToString();
        }

        // Get the DescriptionAttribute if it exists; this returns null if not found.
        DescriptionAttribute? attribute = fieldInfo.GetCustomAttribute<DescriptionAttribute>();

        // Return the Description if available, otherwise the enum name.
        return attribute?.Description ?? reason.ToString();
    }

    public enum CampoUtilNome
    {
        CDU_LinVar1,
        CDU_LinVar2,
        CDU_LinVar3,
        CDU_LinVar4,
        CDU_LinVar5,
        CDU_LinVar1ENC,
        CDU_UnidadeAlternativa,
        CDU_QuantidadeAlternativa,
        CDU_LinVar2ENC,
        CDU_LinVar3ENC,
        CDU_FactorConversaoAlternativa,
        CDU_LinVar4ENC,
        CDU_LinVar5ENC,
        CDU_UnidadeAuxiliar,
        CDU_UnidadeAuxiliarQuant,
        CDU_PrecoCustoUnit,
        CDU_CustoTotal,
        CDU_MargemTotal,
        CDU_PercMargemTotal,
        CDU_QtSegunda,
        CDU_QtTerca,
        CDU_QtQuarta,
        CDU_QtQuinta,
        CDU_QtSexta,
        CDU_QtSabado,
        CDU_QtDomingo,
        CDU_QtFeriado1,
        CDU_QtFeriado2,
        CDU_Inativa,
        CDU_Observacoes,
        CDU_ArmazemDestino,
        CDU_QtdBonus,
        CDU_ReferenciaCliente,
        CDU_CodBarrasEntidade,
        CDU_PercAjuste,
        CDU_QtdProducao,
        CDU_ImpSortidos,
        CDU_CodSortido,
        CDU_QtdSortido,
        CDU_QtdTotalSortido,
        CDU_LocalizacaoDestino,
        CDU_Certificado
    }

    public enum TipoLinha
    {
        Comentario = 60,
        Portes = 50,
        AcertoValor = 30,
    }

    public enum DocumentResultTypes
    {
        Documento,
        Filial,
        TipoDocumento,
        Serie,
        NumeroDocumento,
        Id,
    }

}