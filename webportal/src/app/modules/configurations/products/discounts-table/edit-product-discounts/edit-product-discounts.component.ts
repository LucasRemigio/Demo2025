/* eslint-disable @typescript-eslint/naming-convention */
import { ChangeDetectorRef, Component, Inject, OnInit } from '@angular/core';
import { FormGroup, FormBuilder, Validators } from '@angular/forms';
import { MatDialogRef, MAT_DIALOG_DATA } from '@angular/material/dialog';
import { TranslocoService } from '@ngneat/transloco';
import {
    Segment,
} from 'app/modules/configurations/clients/clients.types';
import { ChangeClientSegmentComponent } from 'app/modules/configurations/clients/segments-table/change-client-segment/change-client-segment.component';
import { ProductsService } from '../../products.service';
import { ProductDiscount, ProductDiscountDTO } from '../../products.types';

@Component({
    selector: 'app-edit-product-discounts',
    templateUrl: './edit-product-discounts.component.html',
    styleUrls: ['./edit-product-discounts.component.scss'],
})
export class EditProductDiscountsComponent implements OnInit {
    isLoading = false;
    editProductDiscount: FormGroup;
    productDiscount: ProductDiscountDTO;

    minFormValue = 0;
    maxFormValue = 100;

    flashMessage: 'success' | 'error' | null = null;
    flashMessageText: string;

    constructor(
        private _productsService: ProductsService,
        private fb: FormBuilder,
        private _changeDetectorRef: ChangeDetectorRef,
        private _transloco: TranslocoService,
        private dialogRef: MatDialogRef<ChangeClientSegmentComponent>,
        @Inject(MAT_DIALOG_DATA)
        public injectedDiscount: any
    ) {
        this.productDiscount = injectedDiscount.productDiscount;
    }

    ngOnInit(): void {
        this.isLoading = true;

        this.editProductDiscount = this.fb.group({
            mb_min: [
                this.productDiscount.mb_min,
                [
                    Validators.required,
                    Validators.min(this.minFormValue),
                    Validators.max(this.maxFormValue),
                ],
            ],
            desc_max: [
                this.productDiscount.desc_max,
                [
                    Validators.required,
                    Validators.min(this.minFormValue),
                    Validators.max(this.maxFormValue),
                ],
            ],
        });

        this.isLoading = false;
    }

    /**
     * Show flash message
     */
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

    close(): void {
        this.dialogRef.close();
    }

    saveChanges(): void {
        this.isLoading = true;

        const productDiscount: ProductDiscount = {
            product_family_id: this.productDiscount.product_family.id,
            segment_id: this.productDiscount.segment.id,
            mb_min: this.editProductDiscount.value.mb_min,
            desc_max: this.editProductDiscount.value.desc_max,
        };

        this._productsService.updateProductDiscount(productDiscount).subscribe(
            (response) => {
                if (response.result_code <= 0) {
                    this.showFlashMessage(
                        'error',
                        this._transloco.translate('edit-product-discount-error')
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
                    this._transloco.translate('edit-product-discount-error')
                );
            }
        );
    }
}
