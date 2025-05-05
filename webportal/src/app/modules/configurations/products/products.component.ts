/* eslint-disable @typescript-eslint/naming-convention */
import {
    AfterViewInit,
    Component,
    OnInit,
    ViewChild,
    ViewEncapsulation,
} from '@angular/core';
import { MatTabGroup } from '@angular/material/tabs';
import { Router, ActivatedRoute } from '@angular/router';
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
export class ProductsComponent implements OnInit, AfterViewInit {
    @ViewChild('tabGroup') tabGroup: MatTabGroup;

    productDiscounts: ProductDiscountDTO[] = [];
    catalogProducts: ProductCatalogDTO[];
    productConversions: ProductConversionDTO[] = [];

    isLoading = false;

    selectedSegment: number = null;
    selectedProductFamily: string = null;
    productFamilies: Family[] = [];

    selectedProductFamilyCatalog: string = '';

    // Define tab IDs
    readonly TAB_IDS = {
        DISCOUNTS: 'discounts',
        CATALOG: 'catalog',
        CONVERSIONS: 'conversions',
    };

    constructor(
        private _productsService: ProductsService,
        private _fuseSplashScreenService: FuseSplashScreenService,
        private _router: Router,
        private _route: ActivatedRoute
    ) {}

    ngOnInit(): void {
        this.refreshDiscounts(true);
        this.refreshCatalogProducts();
        this.fetchConversions();
        this._productsService.getProductFamilies().subscribe((response) => {
            this.productFamilies = response.product_families;
        });
    }

    ngAfterViewInit(): void {
        // Wait for the view to be initialized before selecting the tab
        this._route.queryParams.subscribe((params) => {
            const tab = params['tab'];
            if (tab) {
                // Navigate to the appropriate tab based on name
                this.selectTabByName(tab);
            }
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

    onTabChange(event: any): void {
        // Get tab index
        const tabIndex = event.index;

        // Use a lookup to convert index to tab ID
        const tabIds = [
            this.TAB_IDS.DISCOUNTS,
            this.TAB_IDS.CATALOG,
            this.TAB_IDS.CONVERSIONS,
        ];
        const tabId = tabIds[tabIndex] || '';

        this._router.navigate([], {
            relativeTo: this._route,
            queryParams: { tab: tabId },
            queryParamsHandling: 'merge',
        });
    }

    selectTabByName(tabId: string): void {
        if (!this.tabGroup) {
            return;
        }

        // Map tabId to index using indexOf
        const tabIds = [
            this.TAB_IDS.DISCOUNTS,
            this.TAB_IDS.CATALOG,
            this.TAB_IDS.CONVERSIONS,
        ];
        const index = tabIds.indexOf(tabId);

        if (index !== -1) {
            this.tabGroup.selectedIndex = index;
        }
    }
}
