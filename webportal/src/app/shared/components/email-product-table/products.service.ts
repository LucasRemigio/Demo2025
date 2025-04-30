/* eslint-disable arrow-parens */
/* eslint-disable @typescript-eslint/no-shadow */
/* eslint-disable @typescript-eslint/naming-convention */
import { Injectable } from '@angular/core';
import { HttpClient, HttpStatusCode } from '@angular/common/http';
import { FuseSplashScreenService } from '@fuse/services/splash-screen';
import { environment } from 'environments/environment';
import {
    Product,
    ProductCatalogDto,
    ProductCatalogResponse,
    ProductConversionDTO,
    ProductConversionsDtoResponse,
    ProductUnit,
    ProductUnitsResponse,
    ProductUpdateResponse,
} from './products.types';
import { Observable, of } from 'rxjs';
import {
    OrderProduct,
    ProductCatalogDTO,
} from '../filtering-validate/details/details.types';
import { tap } from 'rxjs/operators';

@Injectable({
    providedIn: 'root',
})
export class ProductsService {
    private catalogProductsCache: ProductCatalogDto[] | null = null;
    private productConversionsCache: ProductConversionDTO[] | null = null;
    private productUnitsCache: ProductUnit[] | null = null;

    /**
     * Constructor
     */
    constructor(private _httpClient: HttpClient) {}

    // -----------------------------------------------------------------------------------------------------
    // @ Public methods
    // -----------------------------------------------------------------------------------------------------

    /*
    Get all catalog products
    */
    fetchCatalogProducts(url: string): Observable<ProductCatalogResponse> {
        if (this.catalogProductsCache) {
            // Return cached value using the 'of' helper for simplicity
            return of({
                product_catalogs: this.catalogProductsCache,
                result: 'success',
                result_code: 1,
            });
        }

        return this._httpClient.get<ProductCatalogResponse>(url).pipe(
            tap((response) => {
                this.catalogProductsCache = response.product_catalogs;
            })
        );
    }

    getCatalogProducts(): Observable<ProductCatalogResponse> {
        const url =
            environment.currrentBaseURL +
            '/api/products/catalogs?has_stock=false';
        return this.fetchCatalogProducts(url);
    }

    getCatalogProductsNoAuth(): Observable<ProductCatalogResponse> {
        const url =
            environment.currrentBaseURL +
            '/api/products/catalogs/no-auth?has_stock=true';
        return this.fetchCatalogProducts(url);
    }

    getCatalogProductsBySearchNoAuth(
        searchValue: string,
        hasStock: boolean
    ): Observable<ProductCatalogResponse> {
        const url =
            environment.currrentBaseURL +
            `/api/products/catalogs/no-auth/search?query=${searchValue}&has_stock=${hasStock}`;
        return this._httpClient.get<ProductCatalogResponse>(url);
    }

    /*
     * Get product sizes
     */
    getProductUnits(): Observable<ProductUnitsResponse> {
        if (this.productUnitsCache) {
            return new Observable((observer) => {
                observer.next({
                    product_units: this.productUnitsCache,
                    result: 'success',
                    result_code: 1,
                });
                observer.complete();
            });
        }

        return this._httpClient
            .get<ProductUnitsResponse>(
                environment.currrentBaseURL + '/api/products/sizes'
            )
            .pipe(
                tap((response) => {
                    this.productUnitsCache = response.product_units;
                })
            );
    }

    /*
     *    get the product conversions
     */
    getProductConversions(): Observable<ProductConversionsDtoResponse> {
        if (this.productConversionsCache) {
            return new Observable((observer) => {
                observer.next({
                    product_conversions: this.productConversionsCache,
                    result: 'success',
                    result_code: 1,
                });
                observer.complete();
            });
        }

        return this._httpClient
            .get<ProductConversionsDtoResponse>(
                environment.currrentBaseURL + '/api/products/conversions/dto'
            )
            .pipe(
                tap((response) => {
                    this.productConversionsCache = response.product_conversions;
                })
            );
    }

    /*
     * Clear all cached data
     */
    clearCache(): void {
        this.catalogProductsCache = null;
        this.productConversionsCache = null;
        this.productUnitsCache = null;
    }

    /*
        Update the email products
    */
    updateEmailProducts(emailToken: string, products: OrderProduct[]): any {
        return this._httpClient.put<HttpStatusCode>(
            environment.currrentBaseURL + `/api/products/${emailToken}`,
            {
                products: products,
            }
        );
    }

    /*
    Update order products
    */

    updateOrderProducts(
        orderToken: string,
        products: OrderProduct[],
        noAuth: boolean
    ): Observable<ProductUpdateResponse> {
        return this._httpClient.put<ProductUpdateResponse>(
            environment.currrentBaseURL +
                `/api/orders/products/${noAuth ? 'no-auth/' : ''}${orderToken}`,
            {
                products: products,
            }
        );
    }

    /* ======================== ==================================================================================
     * UTILITIES
     */

    formatStock(product: ProductCatalogDto): string {
        const baseStock = `${product.stock_current} ${product.unit}`;

        if (product.stock_current <= 0) {
            return baseStock;
        }

        // get the conversion stock as well
        const conversions = product.product_conversions;
        if (!conversions) {
            return baseStock;
        }

        const conversion = conversions.find(
            (conversion) => conversion.origin_unit.abbreviation === product.unit
        );

        if (!conversion) {
            return baseStock;
        }

        const stockInConversion = product.stock_current * conversion.rate;
        const stockConversionFormatted = `${stockInConversion.toFixed(2)} ${
            conversion.end_unit.abbreviation
        }`;

        return `${baseStock}, ${stockConversionFormatted}`;
    }
}
