/* eslint-disable @typescript-eslint/naming-convention */

import {
    PricingStrategy,
    ProductCatalogDTO,
} from 'app/shared/components/filtering-validate/details/details.types';
import { BaseResponse } from '../clients/clients.types';

export interface ProductDiscountResponse extends BaseResponse {
    product_discounts: ProductDiscountDTO[];
}

export interface ProductDiscountDTO {
    product_family: ProductFamily;
    segment: Segment;
    mb_min: number;
    desc_max: number;
}

export interface Segment {
    id: number;
    name: string;
}

export interface ProductFamily {
    id: string;
    name: string;
}

export interface ProductDiscount {
    product_family_id: string;
    segment_id: number;
    mb_min: number;
    desc_max: number;
}

export interface ProductFamiliesResponse extends BaseResponse {
    product_families: ProductFamily[];
}

export interface ProductFamily {
    id: string;
    name: string;
}

export interface PricingStrategyResponse extends BaseResponse {
    pricing_strategies: PricingStrategy[];
}

export interface GenericResponse {
    result: string;
    result_code: number;
}

export interface ProductCatalogResponseDTO extends BaseResponse {
    product_catalogs: ProductCatalogDTO[];
}

export interface TimeElapsedResponse extends GenericResponse {
    time_elapsed_ms: number;
}
