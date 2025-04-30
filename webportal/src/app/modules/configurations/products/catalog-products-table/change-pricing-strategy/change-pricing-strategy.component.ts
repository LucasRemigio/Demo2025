/* eslint-disable arrow-parens */
import { ChangeDetectorRef, Component, Inject, OnInit } from '@angular/core';
import { FormBuilder, FormGroup, Validators } from '@angular/forms';
import { MatDialogRef, MAT_DIALOG_DATA } from '@angular/material/dialog';
import { SafeHtml } from '@angular/platform-browser';
import { TranslocoService } from '@ngneat/transloco';
import { ChangeClientSegmentComponent } from 'app/modules/configurations/clients/segments-table/change-client-segment/change-client-segment.component';
import { Family } from 'app/shared/components/email-product-table/products.types';
import {
    PricingStrategy,
    ProductCatalogDTO,
} from 'app/shared/components/filtering-validate/details/details.types';
import { ProductsService } from '../../products.service';
import { GenericResponse, PricingStrategyResponse } from '../../products.types';

@Component({
    selector: 'app-change-pricing-strategy',
    templateUrl: './change-pricing-strategy.component.html',
    styleUrls: ['./change-pricing-strategy.component.scss'],
})
export class ChangePricingStrategyComponent implements OnInit {
    pricingStrategies: PricingStrategy[];
    families: Family[];
    catalogProduct: ProductCatalogDTO;
    isLoading: boolean = false;
    pricingForm: FormGroup;
    currentPricingDescription: SafeHtml = '';

    flashMessage: 'success' | 'error' | null = null;
    flashMessageText: string;

    constructor(
        private _productsService: ProductsService,
        private fb: FormBuilder,
        private _changeDetectorRef: ChangeDetectorRef,
        private _transloco: TranslocoService,
        private dialogRef: MatDialogRef<ChangeClientSegmentComponent>,
        @Inject(MAT_DIALOG_DATA)
        public data: { families: Family[]; catalogProduct?: ProductCatalogDTO }
    ) {
        this.families = data.families || [];
        this.catalogProduct = data.catalogProduct || null;
    }

    ngOnInit(): void {
        this.isLoading = true;
        this._productsService
            .getPricingStrategies()
            .subscribe((response: PricingStrategyResponse) => {
                if (response.result_code <= 0) {
                    console.error('oops');
                    this.isLoading = false;
                    return;
                }

                this.isLoading = false;
                this.pricingStrategies = response.pricing_strategies;

                const firstPricingStrategy = response.pricing_strategies[0];

                this.pricingForm = this.fb.group({
                    pricingStrategy: [
                        firstPricingStrategy.id,
                        Validators.required,
                    ],
                    family: [0],
                });
                this.currentPricingDescription =
                    this.getCurrentStrategyDescription(firstPricingStrategy.id);
                this.pricingForm
                    .get('pricingStrategy')
                    ?.valueChanges.subscribe((value) => {
                        this.currentPricingDescription =
                            this.getCurrentStrategyDescription(value);
                    });
            });
    }

    showFlashMessage(type: 'success' | 'error', textMsg: string): void {
        // Show the message
        this.flashMessage = type;
        this.flashMessageText = textMsg;

        // Mark for check
        this._changeDetectorRef.markForCheck();

        // Hide it after 3 seconds
        setTimeout(() => {
            this.flashMessage = null;
            this.flashMessageText = null;

            // Mark for check
            this._changeDetectorRef.markForCheck();
        }, 5000);
    }
    /*
     *     PUBLIC METHODS
     */

    close(): void {
        this.dialogRef.close();
    }

    saveChanges(): void {
        if (this.pricingForm.invalid) {
            return;
        }
        this.isLoading = true;

        const currentPricingId = this.pricingForm.get('pricingStrategy').value;
        const familyId: string = this.pricingForm.get('family').value;

        this._productsService
            .patchProductPricingStrategy(
                currentPricingId,
                familyId ?? null,
                this.catalogProduct?.id ?? null
            )
            .subscribe(
                (response: GenericResponse) => {
                    if (response.result_code <= 0) {
                        this.isLoading = false;
                        this.showFlashMessage(
                            'error',
                            this._transloco.translate(
                                'change-pricing-strategy-error'
                            )
                        );
                        return;
                    }
                    this.isLoading = false;
                    this.dialogRef.close(true);
                },
                (error) => {
                    this.isLoading = false;
                    this.showFlashMessage(
                        'error',
                        this._transloco.translate(
                            'change-pricing-strategy-error'
                        )
                    );
                    return;
                }
            );
    }

    // Method to get the description dynamically based on selected strategy
    getCurrentStrategyDescription(id: number): string {
        const slug = this.pricingStrategies.find((x) => x.id === id).slug;
        const translationKey = 'PricingStrategies.' + slug + '-description';
        return this._transloco.translate(translationKey);
    }
}
