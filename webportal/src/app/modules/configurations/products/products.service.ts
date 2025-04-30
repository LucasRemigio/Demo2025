/* eslint-disable @typescript-eslint/naming-convention */
import { HttpClient, HttpParams } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { ProductConversionsDtoResponse } from 'app/shared/components/email-product-table/products.types';
import { environment } from 'environments/environment';
import { Observable } from 'rxjs';
import { SegmentsResponse } from '../clients/clients.types';
import {
    GenericResponse,
    PricingStrategyResponse,
    ProductCatalogResponseDTO,
    ProductDiscount,
    ProductDiscountResponse,
    ProductFamiliesResponse,
} from './products.types';

@Injectable({
    providedIn: 'root',
})
export class ProductsService {
    constructor(private _httpClient: HttpClient) {}

    /**
     * Get All products
     */
    getAllProducts(
        productFamilyId?: string,
        segmentId?: number
    ): Observable<ProductDiscountResponse> {
        let params = new HttpParams();

        if (productFamilyId) {
            params = params.set('productFamilyId', productFamilyId);
        }
        if (segmentId) {
            params = params.set('segmentId', segmentId.toString());
        }

        return this._httpClient.get<ProductDiscountResponse>(
            environment.currrentBaseURL + '/api/products/discounts',
            { params }
        );
    }

    /*
     * Update product discount
     */

    updateProductDiscount(productDiscount: ProductDiscount): Observable<any> {
        return this._httpClient.put<any>(
            environment.currrentBaseURL +
                `/api/products/discounts/${productDiscount.product_family_id}/${productDiscount.segment_id}`,
            {
                mb_min: productDiscount.mb_min,
                desc_max: productDiscount.desc_max,
            }
        );
    }
    /*
     * Get all the segments
     */

    getSegments(): Observable<SegmentsResponse> {
        return this._httpClient.get<SegmentsResponse>(
            environment.currrentBaseURL + '/api/segments'
        );
    }

    /*
     * Get all the product families
     */
    getProductFamilies(): Observable<ProductFamiliesResponse> {
        return this._httpClient.get<ProductFamiliesResponse>(
            environment.currrentBaseURL + '/api/products/families'
        );
    }

    /*
     *   Get current pricing strategy and available ones
     */
    getPricingStrategies(): Observable<PricingStrategyResponse> {
        return this._httpClient.get<PricingStrategyResponse>(
            environment.currrentBaseURL + '/api/pricing-strategies'
        );
    }

    /*
            Get all catalog products
    */
    getCatalogProducts(
        family_id?: string
    ): Observable<ProductCatalogResponseDTO> {
        return this._httpClient.get<ProductCatalogResponseDTO>(
            environment.currrentBaseURL +
                '/api/products/catalogs?family_id=' +
                family_id ?? ''
        );
    }

    /*
     * Update products pricing strategy
     */
    patchProductPricingStrategy(
        pricingStrategyId: number,
        familyId?: string,
        catalogProductId?: number
    ): Observable<GenericResponse> {
        if (!familyId || familyId === '0') {
            familyId = null;
        }
        if (!catalogProductId) {
            catalogProductId = null;
        }

        return this._httpClient.patch<GenericResponse>(
            environment.currrentBaseURL +
                '/api/products/catalogs/pricing-strategy',
            {
                pricing_strategy_id: pricingStrategyId,
                family_id: familyId ?? null,
                product_id: catalogProductId ?? null,
            }
        );
    }

    /*
     *    get the product conversions
     */
    getProductConversions(): Observable<ProductConversionsDtoResponse> {
        return this._httpClient.get<ProductConversionsDtoResponse>(
            environment.currrentBaseURL + '/api/products/conversions/dto'
        );
    }
}
