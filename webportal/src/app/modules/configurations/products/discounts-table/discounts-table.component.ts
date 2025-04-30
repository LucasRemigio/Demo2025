import {
    ChangeDetectorRef,
    Component,
    EventEmitter,
    Input,
    OnInit,
    Output,
} from '@angular/core';
import { MatDialog, MatDialogConfig } from '@angular/material/dialog';
import { TranslocoService } from '@ngneat/transloco';
import { Family } from 'app/shared/components/email-product-table/products.types';
import { Segment } from '../../clients/clients.types';
import { ProductsService } from '../products.service';
import {
    ProductDiscount,
    ProductDiscountDTO,
    ProductFamily,
} from '../products.types';
import { ChangePricingStrategyComponent } from '../catalog-products-table/change-pricing-strategy/change-pricing-strategy.component';
import { EditProductDiscountsComponent } from './edit-product-discounts/edit-product-discounts.component';

@Component({
    selector: 'app-discounts-table',
    templateUrl: './discounts-table.component.html',
    styleUrls: ['./discounts-table.component.scss'],
})
export class DiscountsTableComponent implements OnInit {
    @Input() discounts: ProductDiscountDTO[] = [];
    @Input() productFamilies: Family[] = [];
    @Input() isLoading: boolean = true;
    @Output() refreshRequested = new EventEmitter<{
        selectedSegment?: number;
        selectedProductFamily?: string;
    }>();

    segments: Segment[] = [];
    selectedProductFamilyId: string = '';
    selectedSegmentId: number = 0;

    flashMessage: 'success' | 'error' | null = null;
    flashMessageText: string;

    constructor(
        private _matDialog: MatDialog,
        private _changeDetectorRef: ChangeDetectorRef,
        private _transloco: TranslocoService,
        private _productsService: ProductsService
    ) {}

    ngOnInit(): void {
        this._productsService.getSegments().subscribe((response) => {
            this.segments = response.segments;
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

    editProductDiscount(productDiscount: ProductDiscount): void {
        // open the dialog to edit the segment
        const dialogConfig: MatDialogConfig = {
            maxHeight: '80vh',
            minHeight: '200px',
            height: 'auto',
            maxWidth: '80vw',
            minWidth: '300px',
            width: 'auto',
            data: {
                productDiscount,
            },
        };

        const dialogRef = this._matDialog.open(
            EditProductDiscountsComponent,
            dialogConfig
        );

        dialogRef.afterClosed().subscribe((result) => {
            if (result) {
                // Refresh data after successful create or update
                this.showFlashMessage(
                    'success',
                    this._transloco.translate('edit-product-discount-success')
                );
                this.refreshData();
            }
        });
    }

    refreshData(): void {
        this.refreshRequested.emit({
            selectedSegment: this.selectedSegmentId,
            selectedProductFamily: this.selectedProductFamilyId,
        });
    }

    public getHeaderStyle(): any {
        return {
            'background-color': '#383838',
            // eslint-disable-next-line quote-props
            color: 'white',
            'font-weight': 'bold',
        };
    }

    public isSmallScreen(): boolean {
        return window.innerWidth < 768;
    }
}
