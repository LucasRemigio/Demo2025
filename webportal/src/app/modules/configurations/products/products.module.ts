import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';
import { DiscountsTableComponent } from './discounts-table/discounts-table.component';
import { ReactiveFormsModule } from '@angular/forms';
import { MatButtonModule } from '@angular/material/button';
import { MatDialogModule } from '@angular/material/dialog';
import { MatFormFieldModule } from '@angular/material/form-field';
import { MatIconModule } from '@angular/material/icon';
import { MatInputModule } from '@angular/material/input';
import { MatProgressSpinnerModule } from '@angular/material/progress-spinner';
import { MatSelectModule } from '@angular/material/select';
import { MatTabsModule } from '@angular/material/tabs';
import { MatTooltipModule } from '@angular/material/tooltip';
import { TranslocoModule } from '@ngneat/transloco';
import { GridModule } from '@progress/kendo-angular-grid';
import { ProductsComponent } from './products.component';
import { MatProgressBarModule } from '@angular/material/progress-bar';
import { EditProductDiscountsComponent } from './discounts-table/edit-product-discounts/edit-product-discounts.component';
import { ChangePricingStrategyComponent } from './catalog-products-table/change-pricing-strategy/change-pricing-strategy.component';
import { MatCheckboxModule } from '@angular/material/checkbox';
import { CatalogProductsTableComponent } from './catalog-products-table/catalog-products-table.component';
import { ConversionsTableComponent } from './conversions-table/conversions-table.component';
import { CatalogCategorizationDetailsComponent } from './catalog-products-table/catalog-categorization-details/catalog-categorization-details.component';

@NgModule({
    declarations: [
        ProductsComponent,
        DiscountsTableComponent,
        EditProductDiscountsComponent,
        ChangePricingStrategyComponent,
        CatalogProductsTableComponent,
        ConversionsTableComponent,
        CatalogCategorizationDetailsComponent,
    ],
    imports: [
        CommonModule,
        TranslocoModule,
        GridModule,
        MatIconModule,
        MatTooltipModule,
        MatButtonModule,
        MatDialogModule,
        ReactiveFormsModule,
        MatFormFieldModule,
        MatInputModule,
        MatTabsModule,
        MatSelectModule,
        MatProgressSpinnerModule,
        MatProgressBarModule,
    ],
})
export class ProductsModule {}
