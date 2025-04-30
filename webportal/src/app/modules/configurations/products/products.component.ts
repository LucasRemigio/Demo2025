import { Component, OnInit, ViewEncapsulation } from '@angular/core';
import { FuseSplashScreenService } from '@fuse/services/splash-screen';
import {
    Family,
    ProductConversionDTO,
} from 'app/shared/components/email-product-table/products.types';
import { ProductCatalogDTO } from 'app/shared/components/filtering-validate/details/details.types';
import { Segment } from '../clients/clients.types';
import { ProductsService } from './products.service';
import {
    ProductDiscount,
    ProductDiscountDTO,
    ProductFamily,
} from './products.types';

@Component({
    selector: 'app-products',
    templateUrl: './products.component.html',
    styleUrls: ['./products.component.scss'],
    encapsulation: ViewEncapsulation.None,
})
export class ProductsComponent implements OnInit {
    productDiscounts: ProductDiscountDTO[] = [];
    catalogProducts: ProductCatalogDTO[];
    productConversions: ProductConversionDTO[] = [];

    isLoading = false;

    selectedSegment: number = null;
    selectedProductFamily: string = null;
    productFamilies: Family[] = [];

    selectedProductFamilyCatalog: string = '';

    constructor(
        private _productsService: ProductsService,
        private _fuseSplashScreenService: FuseSplashScreenService
    ) {}

    ngOnInit(): void {
        this.refreshDiscounts(true);
        this.refreshCatalogProducts();
        this.fetchConversions();
        this._productsService.getProductFamilies().subscribe((response) => {
            this.productFamilies = response.product_families;
        });
    }

    onRefreshDiscountsRequested(event: {
        selectedSegment: number;
        selectedProductFamily: string;
    }): void {
        // Update the parent component state based on the emitted values
        this.selectedSegment = event.selectedSegment;
        this.selectedProductFamily = event.selectedProductFamily;

        this.refreshDiscounts();
    }

    refreshDiscounts(isShowSplashScreen: boolean = false): void {
        this.isLoading = true;
        if (isShowSplashScreen) {
            this._fuseSplashScreenService.show();
        }

        this._productsService
            .getAllProducts(this.selectedProductFamily, this.selectedSegment)
            .subscribe((response) => {
                this.isLoading = false;
                this.productDiscounts = response.product_discounts;

                if (isShowSplashScreen) {
                    this._fuseSplashScreenService.hide();
                }
            });
    }

    onRefreshCatalogProductsRequested(event: {
        selectedProductFamily: string;
    }): void {
        this.selectedProductFamilyCatalog = event.selectedProductFamily;

        this.refreshCatalogProducts();
    }

    refreshCatalogProducts(): void {
        this._productsService
            .getCatalogProducts(this.selectedProductFamilyCatalog)
            .subscribe(
                (response) => {
                    if (response.result_code <= 0) {
                        return;
                    }

                    this.catalogProducts = response.product_catalogs;
                },
                (error) => {
                    console.error('ERRO!');
                }
            );
    }

    fetchConversions(): void {
        this._productsService.getProductConversions().subscribe(
            (response) => {
                if (response.result_code <= 0) {
                    return;
                }
                this.productConversions = response.product_conversions;
            },
            (error) => {
                console.error('ERRO!');
            }
        );
    }
}
