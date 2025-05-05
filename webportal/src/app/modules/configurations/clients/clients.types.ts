/* eslint-disable @typescript-eslint/naming-convention */

import { GenericResponse } from '../products/products.types';
import {
    PrimaveraClient,
    PrimaveraOrderHeader,
    PrimaveraOrderLine,
} from './primavera.types';

export interface BaseResponse {
    result: string;
    result_code: number;
}

export interface SegmentsResponse extends BaseResponse {
    segments: Segment[];
}

export interface ClientsResponse extends BaseResponse {
    clients: Client[];
}

export interface ClientResponse extends BaseResponse {
    client: Client;
}

export interface Client {
    id: number;
    code: string;
    token: string;
    segment: Segment;
    primavera_client?: PrimaveraClient | null;
    ratings: ClientRatingDTO[];
    created_at: Date | null;
    updated_at: Date | null;
    created_by: string | null;
    updated_by: string | null;
    weighted_rating: number;
}

export interface ClientCodeResponse extends BaseRatingDTO {
    client_code: string;
}

export interface ClientRatingDTO {
    client_code: string;
    rating_type: RatingType;
    rating_discount: RatingDiscount;
    rating_valid_until: string;
    recommended_rating_discount: RatingDiscount;
    updated_at: string;
    updated_by: string;
    created_at: string;
    created_by: string;
}

export interface BaseRatingDTO {
    rating_type: RatingType;
    rating_discount: RatingDiscount;
    created_at: Date | null;
    updated_at: Date | null;
    created_by: string | null;
    updated_by: string | null;
}

export interface RatingDiscount {
    rating: string;
    percentage: number;
}

export interface Segment {
    id: number;
    name: string;
}

export interface RatingType {
    id: number;
    description: string;
    slug: RatingSlugs;
    weight: number;
}
export interface RatingTypesResponse extends BaseResponse {
    rating_types: RatingType[];
}

export interface RatingUpdate {
    client_code: string;
    rating_type_id: number;
    rating_discount: string;
}

export interface RatingDiscountsResponse extends BaseResponse {
    rating_discounts: RatingDiscount[];
}

export enum RatingSlugs {
    Credit = 'credit',
    PaymentCompliance = 'payment-compliance',
    HistoricalVolume = 'historical-volume',
    PotentialVolume = 'potential-volume',
    OperationalCost = 'operational-cost',
    Logistic = 'logistic',
}

export interface ClientPrimaveraOrdersResponse extends GenericResponse {
    total: number;
    primavera_orders: PrimaveraOrder[];
}

export interface ClientPrimaveraPendingInvoiceOrdersResponse
    extends GenericResponse {
    orders: PrimaveraOrder[];
    invoices: PrimaveraInvoice[];
    orders_total: number;
    invoices_total: InvoiceTotal;
    average_payment_time?: AveragePaymentTime | null;
}

export interface InvoiceTotal {
    valor_total: number;
    valor_pendente: number;
    valor_liquidacao: number;
}

export interface PrimaveraOrder {
    primavera_order_header: PrimaveraOrderHeader;
    primavera_order_line: PrimaveraOrderLine[];
}

export interface SyncClientResponse extends BaseResponse {
    synced_clients: number;
}

export interface SyncPrimaveraStatsResponse extends BaseResponse {
    statistics: SyncPrimaveraStats;
}

export interface SyncPrimaveraStats {
    time_elapsed_ms: number;
    total_syncs: number;
}

export interface SyncingRatingStates {
    credit: boolean;
    paymentCompliance: boolean;
    historicalVolume: boolean;
}

export interface PrimaveraInvoice {
    modulo: string;
    ano: null | number;
    tipoDoc: string;
    serie: string;
    numDoc: number;
    numDocExt: string;
    dataDoc: string;
    tipoEntidade: string;
    entidade: string;
    tipoMov: string;
    valorTotal: number;
    valorPendente: number;
    valorLiquidacao: number;
    codigo: string;
    dataVencimento: null | string;
    dataLiquidacao: null | string;
    moeda: string;
    numAvisos: number;
}

export interface AveragePaymentTime {
    average_payment_time_days: number;
    average_deadline_time_days: number;
}

export interface ClientPrimaveraInvoicesResponse extends BaseResponse {
    average_payment_time?: AveragePaymentTime | null;
    primavera_invoices: PrimaveraInvoice[];
    invoices_total: InvoiceTotal;
}

export interface AveragePaymentTime {
    average_payment_time_days: number;
    average_deadline_time_days: number;
}

export interface UpdateClientRatings {
    rating_type_id: number;
    rating: string;
    rating_valid_until: string;
}

export interface UpdateClientRatingsRequest {
    ratings: UpdateClientRatings[];
}
