// // Copyright (c) 2024 Engibots. All rights reserved.

using System.ComponentModel;
using System.Reflection;
using Engimatrix.ModelObjs;

namespace engimatrix.Config
{
    public static class StatusConstants
    {
        public static readonly int ULTIMO_ID_VALIDO = 21;

        public static class StatusCode
        {
            public static readonly int ATIVO = 1;
            public static readonly int INATIVO = 2;
            public static readonly int SUCESSO = 3;
            public static readonly int INSUCESSO = 4;
            public static readonly int ERRO = 5;
            public static readonly int REPROCESSADO = 6;
            public static readonly int TRIAGEM_REALIZADA = 7;
            public static readonly int APAGADO = 8;
            public static readonly int NOVO = 9;
            public static readonly int AGUARDA_VALIDACAO = 10;
            public static readonly int A_PROCESSAR = 11;
            public static readonly int RESOLVIDO_MANUALMENTE = 12;
            public static readonly int CANCELADO_POR_OPERADOR = 13;
            public static readonly int CONFIRMADO_POR_OPERADOR = 14;
            public static readonly int CONFIRMADO_POR_CLIENTE = 15;
            public static readonly int CANCELADO_POR_CLIENTE = 16;
            public static readonly int ENVIADO_PARA_PRIMAVERA = 17;
            public static readonly int PENDENTE_APROVACAO_ADMINISTRACAO = 18;
            public static readonly int APROVADO_DIRECAO_COMERCIAL = 19;
            public static readonly int ERRO_ENVIAR_PRIMAVERA = 20;
            public static readonly int PENDENTE_CONFIRMACAO_CLIENTE = 21;
            public static readonly int PENDENTE_APROVACAO_CREDITO = 22;
            public static readonly int CREDITO_REJEITADO = 23;
        }
    }

    [AttributeUsage(AttributeTargets.Field, AllowMultiple = false)]
    public class MultiDescriptionAttribute : Attribute
    {
        public string[] Descriptions { get; }

        public MultiDescriptionAttribute(params string[] descriptions)
        {
            Descriptions = descriptions;
        }
    }

    public enum StatusEnum
    {
        [MultiDescription("Ativo", "Active")]
        Ativo = 1,

        [MultiDescription("Inativo", "Inactive")]
        Inativo = 2,

        [MultiDescription("Sucesso", "Success")]
        Sucesso = 3,

        [MultiDescription("Insucesso", "Insuccess")]
        Insucesso = 4,

        [MultiDescription("Erro", "Error")]
        Erro = 5,

        [MultiDescription("Reprocessado", "Reprocessed")]
        Reprocessado = 6,

        [MultiDescription("Triagem Realizada", "Screening Completed")]
        TriagemRealizada = 7,

        [MultiDescription("Apagado", "Deleted", "Removed")]
        Apagado = 8,

        [MultiDescription("Novo", "New")]
        Novo = 9,

        [MultiDescription("Aguarda Validação", "Awaits Validation")]
        AguardaValidacao = 10,

        [MultiDescription("A Processar", "Processing")]
        AProcessar = 11,

        [MultiDescription("Resolvido Manualmente", "Manually Resolved")]
        ResolvidoManualmente = 12,

        [MultiDescription("Cancelado", "Canceled")]
        Cancelado = 13,

        [MultiDescription("Confirmado por Operador", "Confirmed by Operator")]
        ConfirmadoPorOperador = 14,

        [MultiDescription("Confirmado por Cliente", "Confirmed by Client")]
        ConfirmadoPorCliente = 15,

        [MultiDescription("Cancelado por Cliente", "Canceled by Client")]
        CanceladoPorCliente = 16,

        [MultiDescription("Enviado para Primavera", "Sent to Primavera")]
        EnviadoParaPrimavera = 17,

        // database limits status to 30 characters, so full administraçao does not fit. This is to match it
        [MultiDescription("Pendente Aprovação Administrac", "Pending Administration Approval")]
        PendenteAprovacaoAdministracao = 18,

        [MultiDescription("Aprovado Direção Comercial", "Approved by Commercial Direction")]
        AprovadoDirecaoComercial = 19,

        [MultiDescription("Erro ao Enviar para Primavera", "Error Sending to Primavera")]
        ErroEnviarPrimavera = 20,

        [MultiDescription("Pendente Confirmação Cliente", "Pending Client Confirmation")]
        PendenteConfirmacaoCliente = 21,

        [MultiDescription("Pendente Aprovação Crédito", "Pending Credit Approval")]
        PendenteAprovacaoCredito = 22,

        [MultiDescription("Crédito Rejeitado", "Credit Rejected")]
        CreditoRejeitado = 23,
    }

    public static class StatusConverter
    {
        public static string Convert(int statusCode)
        {
            // Get the enum value corresponding to the statusCode
            StatusEnum statusEnum = (StatusEnum)statusCode;

            // Get the MultiDescription attribute value for the enum value
            MultiDescriptionAttribute descriptionAttribute = (MultiDescriptionAttribute)Attribute.GetCustomAttribute(typeof(StatusEnum).GetField(statusEnum.ToString()), typeof(MultiDescriptionAttribute));

            // If MultiDescription attribute exists, return the first description; otherwise, return the enum name
            return descriptionAttribute != null ? descriptionAttribute.Descriptions[0] : statusEnum.ToString();
        }

        public static int Convert(string statusName)
        {
            // Iterate through all enum values and their attributes
            foreach (var field in typeof(StatusEnum).GetFields(BindingFlags.Public | BindingFlags.Static))
            {
                var attribute = (MultiDescriptionAttribute)Attribute.GetCustomAttribute(field, typeof(MultiDescriptionAttribute));
                if (attribute != null && attribute.Descriptions.Contains(statusName, StringComparer.OrdinalIgnoreCase))
                {
                    return (int)Enum.Parse(typeof(StatusEnum), field.Name);
                }
            }

            return -1; // Return -1 or throw an exception if not found
        }
    }
}
