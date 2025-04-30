/* eslint-disable quote-props */
import {
    ChangeDetectorRef,
    Component,
    EventEmitter,
    Input,
    OnInit,
    Output,
    ViewEncapsulation,
} from '@angular/core';
import { MatDialog, MatDialogConfig } from '@angular/material/dialog';
import { TranslocoService } from '@ngneat/transloco';
import { Family } from 'app/shared/components/email-product-table/products.types';
import { ProductCatalogDTO } from 'app/shared/components/filtering-validate/details/details.types';
import { ChangePricingStrategyComponent } from './change-pricing-strategy/change-pricing-strategy.component';
import { FlashMessageService } from 'app/shared/components/flash-message/flash-message.service';
import { CatalogCategorizationDetailsComponent } from './catalog-categorization-details/catalog-categorization-details.component';
import { PrimaveraSyncingService } from '../../clients/primavera-syncing.service';

@Component({
    selector: 'app-catalog-products-table',
    templateUrl: './catalog-products-table.component.html',
    styleUrls: ['./catalog-products-table.component.scss'],
    encapsulation: ViewEncapsulation.None,
})
export class CatalogProductsTableComponent implements OnInit {
    @Input() catalogProducts: ProductCatalogDTO[] = [];
    @Input() productFamilies: Family[] = [];
    @Input() isLoading: boolean = true;
    @Output() refreshRequested = new EventEmitter<{
        selectedProductFamily?: string;
    }>();

    families: Family[] = [];
    selectedProductFamilyId: string = '';
    isSyncingPrices: boolean = false;

    constructor(
        private _matDialog: MatDialog,
        private _transloco: TranslocoService,
        private _flashMessageService: FlashMessageService,
        private _primaveraSyncingService: PrimaveraSyncingService
    ) {}

    ngOnInit(): void {}

    /*
     *       PUBLIC METHODS
     */

    refreshData(): void {
        this.refreshRequested.emit({
            selectedProductFamily: this.selectedProductFamilyId,
        });
    }

    public getHeaderStyle(): any {
        return {
            'background-color': '#383838',
            color: 'white',
            'font-weight': 'bold',
            height: 'auto',
            'vertical-align': 'middle',
            'text-align': 'center',
            'max-width': '150px',
        };
    }

    changePricingStrategy(catalogProduct?: ProductCatalogDTO | null): void {
        // open the dialog to edit the segment
        const dialogConfig: MatDialogConfig =
            this.getDialogConfigForFamily(catalogProduct);

        const dialogRef = this._matDialog.open(
            ChangePricingStrategyComponent,
            dialogConfig
        );

        dialogRef.afterClosed().subscribe((result) => {
            if (result) {
                // Refresh data after successful create or update
                this.refreshData();
                this._flashMessageService.success(
                    'change-pricing-strategy-success'
                );
            }
        });
    }

    openDetails(product: ProductCatalogDTO): void {
        // open the dialog to edit the segment
        // only send the product
        const dialogConfig = this.getDialogConfigForFamily(product);

        const dialogRef = this._matDialog.open(
            CatalogCategorizationDetailsComponent,
            dialogConfig
        );

        dialogRef.afterClosed().subscribe((result) => {
            if (result) {
                // Refresh data after successful create or update
                this.refreshData();
                this._flashMessageService.success(
                    'change-pricing-strategy-success'
                );
            }
        });
    }

    getDialogConfigForFamily(
        catalogProduct?: ProductCatalogDTO
    ): MatDialogConfig<any> {
        // open the dialog to edit the segment
        const dialogConfig: MatDialogConfig = {
            maxHeight: '80vh',
            minHeight: '200px',
            height: 'auto',
            maxWidth: '80vw',
            minWidth: '50vw',
            width: 'auto',
            data: {
                families: this.productFamilies,
                catalogProduct: catalogProduct || null,
            },
        };

        return dialogConfig;
    }

    getUniqueFamilies(products: ProductCatalogDTO[]): Family[] {
        const uniqueFamiliesMap = new Map<string, Family>();

        // map for O(1) lookup time. Overall speed of this is O(n) where n is the number of products
        for (const product of products) {
            const family = product.family;
            if (!uniqueFamiliesMap.has(family.id)) {
                uniqueFamiliesMap.set(family.id, family);
            }
        }

        const uniqueFamilies = Array.from(uniqueFamiliesMap.values());

        // Sort by `id`
        uniqueFamilies.sort((a, b) => a.id.localeCompare(b.id));

        return uniqueFamilies;
    }

    syncPrimaveraPrices(): void {
        this.isSyncingPrices = true;

        this._primaveraSyncingService.syncProductsCatalogs().subscribe(
            (response) => {
                if (response.result_code <= 0) {
                    this._flashMessageService.error(
                        'sync-product-catalog-primavera-error'
                    );
                    return;
                }

                if (response.statistics.total_syncs > 0) {
                    const message =
                        response.statistics.total_syncs +
                        ' ' +
                        this._transloco.translate(
                            'sync-product-catalog-primavera-success'
                        ) +
                        ' ' +
                        this._transloco.translate('in') +
                        ' ' +
                        response.statistics.time_elapsed_ms +
                        ' ms';
                    this._flashMessageService.success(message);

                    // emit update only if anything changed
                    this.refreshData();
                } else {
                    this._flashMessageService.info(
                        'primavera-product-catalog-already-up-to-date'
                    );
                }
            },
            (error) => {
                this._flashMessageService.error(
                    'sync-product-catalog-primavera-error'
                );
                return null;
            },
            () => {
                this.isSyncingPrices = false;
            }
        );
    }

    formatPrice(price: number): string {
        if (price === null || price === 0) {
            return 'N/A';
        }

        // The price should be formatted with 3 decimal places, and always have 3
        // digits after the decimal point.
        const formattedPrice = price.toFixed(3);

        return `${formattedPrice} €`;
    }

    public isSmallScreen(): boolean {
        return window.innerWidth < 768;
    }
}
