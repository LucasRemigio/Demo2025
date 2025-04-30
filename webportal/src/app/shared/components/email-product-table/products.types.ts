import { BaseResponse } from 'app/modules/configurations/clients/clients.types';
import { EmailAttachment } from 'app/modules/filtering/filtering.types';
import { DestinationDetails, OrderRatingItem } from '../confirm-order-address/confirm-order-address.types';
import {
    AddressFillingDetails,
    FilteredEmail,
    OrderDTO,
    OrderRatingDTO,
} from '../filtering-validate/details/details.types';

/* eslint-disable @typescript-eslint/naming-convention */
export interface Product {
    id: number;
    email_token: string;
    name: string;
    size: string;
    quantity: number;
    quantity_unit: string;
    confidence: number;
}

export interface ProductCatalogResponse {
    product_catalogs: ProductCatalogDto[];
    result: string;
    result_code: number;
}

export interface ProductCatalogDto {
    id: number;
    product_code: string;
    description: string;
    type: Type;
    shape: Type;
    material: Type;
    finishing: Type;
    surface: Type;
    length: number;
    width: number;
    height: number;
    unit: string;
    stock_current: number;
    currency: string;
    price_pvp: number;
    price_avg: number;
    price_last: number;
    date_last_entry: string;
    date_last_exit: string;
    family: Family;
    price_ref_market: number;
    pricing_strategy: PricingStrategy;
    product_conversions: ProductConversionDTO[];
}

export interface PricingStrategy {
    id: number;
    name: string;
    slug: string;
}

export interface Family {
    id: string;
    name: string;
}

export interface Type {
    id: number;
    name: string;
}

export interface ProductUnitsResponse extends BaseResponse {
    product_units: ProductUnit[];
}

export interface ProductUnit {
    id: number;
    abbreviation: string;
    name: string;
    slug: string;
}

export interface OrderProductsItemResponse extends BaseResponse {
    products: OrderProductItem[];
    address_filling_details: AddressFillingDetails | null;
    destination_details: DestinationDetails | null;
    logistic_rating: OrderRatingItem | null;
}

export interface ConfirmOrderResponse extends BaseResponse {
    document: OrderDocument;
}

export interface OrderDocumentsResponse extends BaseResponse {
    documents: OrderDocument[];
}

export interface GenerateInvoiceResponse extends BaseResponse {
    document: OrderDocument;
}

export interface OrderDocument {
    id: number;
    order_token: string;
    name: string;
    type: string;
    series: string;
    // number is restricted by javascript
    nnumber: string;
    created_at: string;
    created_by: string;
    invoice_html: string;
}

export interface ProductUpdateResponse extends OrderProductsItemResponse {
    order_ratings: OrderRatingDTO[];
    order_total: OrderTotalItem;
}

export interface OrderTotalItem {
    total: number;
    totalDiscount: number;
    totalPlusTax: number;
    totalDiscountPlusTax: number;
}

export interface MeterRate {
    order_product_id: number;
    meter_rate: number;
}

export interface OrderProductItem {
    id: number;
    order_token: string;
    product_catalog_id: number;
    product_unit_id: number;
    quantity: number;
    quantity_unit: string;
    calculated_price: number;
    price_discount: number;
}

export interface ProductConversionDTO {
    origin_unit: ProductUnit;
    end_unit: ProductUnit;
    id: number;
    product_code: string;
    product_catalog_description: string;
    rate: number;
}

export interface ProductConversionsDtoResponse extends BaseResponse {
    product_conversions: ProductConversionDTO[];
}

export interface OrderResponse extends BaseResponse {
    order: OrderDTO;
}

export interface OrderListResponse extends BaseResponse {
    orders: OrderDTO[];
}

export interface OrderToValidateResponse extends BaseResponse {
    order: OrderDTO;
    filtered_email: FilteredEmail;
    email_attachments: EmailAttachment[];
}
export interface Step {
    order: number;
    type: string; // e.g. 'email', 'attachments', 'client', etc.
    title: string;
    subtitle: string;
}
