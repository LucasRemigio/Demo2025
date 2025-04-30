/* eslint-disable @typescript-eslint/naming-convention */
import { SafeHtml } from '@angular/platform-browser';
import { FilteredEmail } from 'app/shared/components/filtering-validate/details/details.types';
import { Attachments, Category, Email } from '../common/common.types';
import { BaseResponse } from '../configurations/clients/clients.types';
import { GenericResponse } from '../configurations/products/products.types';

type EmailCategoryKey =
    | 'ENCOMENDAS'
    | 'COTACOES'
    | 'COMPROVATIVOS'
    | 'OUTROS'
    | 'ERRO'
    | 'DUPLICADOS'
    | 'CERTIFICADOS'
    | 'SPAM';

export const EMAIL_CATEGORIES: Record<EmailCategoryKey, Category> = {
    ENCOMENDAS: {
        id: 1,
        title: 'Encomendas',
        slug: 'encomendas',
    },
    COTACOES: {
        id: 2,
        title: 'Cotações & Orçamentos',
        slug: 'pedidos',
    },
    COMPROVATIVOS: {
        id: 3,
        title: 'Comprovativos de Pagamento',
        slug: 'comprovativos',
    },
    OUTROS: {
        id: 4,
        title: 'Outros',
        slug: 'outros',
    },
    ERRO: {
        id: 5,
        title: 'Erro',
        slug: 'erro',
    },
    DUPLICADOS: {
        id: 6,
        title: 'Duplicados',
        slug: 'duplicados',
    },
    CERTIFICADOS: {
        id: 7,
        title: 'Certificados de Qualidade',
        slug: 'certificados',
    },
    SPAM: {
        id: 8,
        title: 'Spam',
        slug: 'spam',
    },
};

export interface Status {
    id: number;
    description: string;
}

type EmailStatusKey =
    | 'ATIVO'
    | 'INATIVO'
    | 'SUCESSO'
    | 'INSUCESSO'
    | 'ERRO'
    | 'REPROCESSADO'
    | 'TRIAGEM_REALIZADA'
    | 'APAGADO'
    | 'NOVO'
    | 'AGUARDA_VALIDACAO'
    | 'A_PROCESSAR'
    | 'RESOLVIDO_MANUALMENTE'
    | 'CANCELADO'
    | 'CONFIRMADO_POR_OPERADOR'
    | 'CONFIRMADO_POR_CLIENTE'
    | 'CANCELADO_POR_CLIENTE'
    | 'ENVIADO_PARA_PRIMAVERA'
    | 'PENDENTE_APROVACAO_ADMINISTRACAO'
    | 'APROVADO_DIRECAO_COMERCIAL'
    | 'PENDENTE_CONFIRMACAO_CLIENTE'
    | 'PENDENTE_APROVACAO_CREDITO';

export const EMAIL_STATUSES: Record<EmailStatusKey, Status> = {
    ATIVO: { id: 1, description: 'Ativo' },
    INATIVO: { id: 2, description: 'Inativo' },
    SUCESSO: { id: 3, description: 'Sucesso' },
    INSUCESSO: { id: 4, description: 'Insucesso' },
    ERRO: { id: 5, description: 'Erro' },
    REPROCESSADO: { id: 6, description: 'Reprocessado' },
    TRIAGEM_REALIZADA: { id: 7, description: 'Triagem Realizada' },
    APAGADO: { id: 8, description: 'Apagado' },
    NOVO: { id: 9, description: 'Novo' },
    AGUARDA_VALIDACAO: { id: 10, description: 'Aguarda Validação' },
    A_PROCESSAR: { id: 11, description: 'A Processar' },
    RESOLVIDO_MANUALMENTE: { id: 12, description: 'Resolvido Manualmente' },
    CANCELADO: { id: 13, description: 'Cancelado' },
    CONFIRMADO_POR_OPERADOR: { id: 14, description: 'Confirmado por Operador' },
    CONFIRMADO_POR_CLIENTE: { id: 15, description: 'Confirmado por Cliente' },
    CANCELADO_POR_CLIENTE: { id: 16, description: 'Cancelado por Cliente' },
    ENVIADO_PARA_PRIMAVERA: { id: 17, description: 'Enviado para Primavera' },
    PENDENTE_APROVACAO_ADMINISTRACAO: {
        id: 18,
        description: 'Pendente Aprovação Administrac',
    },
    APROVADO_DIRECAO_COMERCIAL: {
        id: 19,
        description: 'Aprovado Direção Comercial',
    },
    PENDENTE_CONFIRMACAO_CLIENTE: {
        id: 21,
        description: 'Pendente Confirmação Cliente',
    },
    PENDENTE_APROVACAO_CREDITO: {
        id: 22,
        description: 'Pendente Aprovação Crédito',
    },
};

export interface FilteredEmailResponse {
    filteredEmails: FilteredEmail[];
    result: string;
    result_code: number;
}

export interface FilteredEmailWithAttachmentsResponse {
    filteredEmail: FilteredEmail;
    emailAttachments: EmailAttachment[];
    result: string;
    result_code: number;
}

export interface EmailListResponse {
    emails: Email[];
    attachments: EmailAttachment[];
    result: string;
    result_code: number;
}

export interface EmailResponse {
    email: Email;
    attachments: EmailAttachment[];
    result: string;
    result_code: number;
}

export interface MailboxesResponse {
    mailboxes: string[];
    result: string;
    result_code: number;
}

export interface Statistics {
    total: number;
    automatic: number;
    manual: number;
    toValidate: number;
    error: number;
    lowConfidence: number;
    avgConfidence: number;
    orders: number;
    quotations: number;
    receipts: number;
    others: number;
    errors: number;
    duplicates: number;
    certificates: number;
    spams: number;
    resolved: number;
    unresolved: number;
    unresolved_quotations: number;
    total_replies_masterferro: number;
    total_replies_client: number;
    total_only_dates: number;
}

export interface ReplyConcurrency {
    canReply: boolean;
    replyInfo: ReplyInfo;
    result: string;
    resultCode: number;
}

export interface EmailAddressListResponse extends BaseResponse {
    addresses: string[];
}

export interface ReplyInfo {
    user: string;
    date: string;
}

export interface ReplyData {
    id: number;
    email_token: string;
    reply_token: string;
    from: string;
    to: string;
    subject: string;
    body: string;
    sanitizedBody?: SafeHtml;
    date: string;
    replied_by: string;
    is_read: string;
    attachments: ReplyAttachment[];
}

export interface ReplyAttachment {
    id: string;
    reply_token: string;
    name: string;
    size: string;
    file: string;
}

export interface ReplyByAI {
    response: string;
    result: string;
    result_code: number;
}

export interface SimpleMessage {
    message: string;
    result: string;
    result_code: number;
}

export interface FilteringPopupData {
    filteredEmail: FilteredEmail;
    emailAttachments: EmailAttachment[];
    result: string;
    result_code: number;
    isToChangeCategory: boolean;
}

export interface EmailAttachment {
    id: string;
    email: string;
    name: string;
    size: string;
    file: string;
}

export interface FwdEmail {
    filtered_email: FilteredEmail;
    email_to_list: string[];
}

export interface FwdPopupData {
    token: string;
    isForwarded: boolean;
}

export interface EmailForwardResponse {
    email_forward_list: EmailForward[];
    result: string;
    result_code: number;
}

export interface StatisticsResponse extends GenericResponse {
    statistics: Statistics;
}

export interface MostForwardedAddressesResponse {
    email_forward_list: string[];
    result: string;
    result_code: number;
}

export interface EmailForward {
    id: number;
    email_token: string;
    forwarded_by: string;
    forwarded_to: string;
    forwarded_at: string;
    message?: string | null;
}

export interface EmailRequest {
    to: string;
    subject: string;
    body: string;
    attachments?: File[];
}

export interface EmailReplyTemplateResponse extends BaseResponse {
    template: string | null;
    signature: string | null;
}

export interface EmailStatusUpdateWs {
    email_token: string;
    status_description: string;
    status_id: number;
}

export interface UserJoinedWs {
    email_token: string;
    user_email: string;
    date: Date;
}

export interface EmailInfoDetails {
    mailbox: string;
    to: string;
    cc: string;
    bcc: string;
    subject: string;
}
