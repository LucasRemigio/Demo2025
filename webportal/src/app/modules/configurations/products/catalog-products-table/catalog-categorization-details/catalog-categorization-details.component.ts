import { Component, Inject, OnInit } from '@angular/core';
import { MatDialogRef, MAT_DIALOG_DATA } from '@angular/material/dialog';
import { Family } from 'app/shared/components/email-product-table/products.types';
import { ProductCatalogDTO } from 'app/shared/components/filtering-validate/details/details.types';

@Component({
    selector: 'app-catalog-categorization-details',
    templateUrl: './catalog-categorization-details.component.html',
    styleUrls: ['./catalog-categorization-details.component.scss'],
})
export class CatalogCategorizationDetailsComponent implements OnInit {
    product: ProductCatalogDTO;

    constructor(
        private _matDialogRef: MatDialogRef<CatalogCategorizationDetailsComponent>,
        @Inject(MAT_DIALOG_DATA)
        public data: { families: Family[]; catalogProduct?: ProductCatalogDTO }
    ) {
        this.product = data.catalogProduct || null;
    }

    ngOnInit(): void {}

    close(): void {
        this._matDialogRef.close();
    }
}
