import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';
import { EmailProductTableComponent } from './email-product-table.component';
import { FormsModule, ReactiveFormsModule } from '@angular/forms';
import { MatAutocompleteModule } from '@angular/material/autocomplete';
import { MatButtonModule } from '@angular/material/button';
import { MatFormFieldModule } from '@angular/material/form-field';
import { MatIconModule } from '@angular/material/icon';
import { MatInputModule } from '@angular/material/input';
import { TranslocoModule } from '@ngneat/transloco';
import { MatTooltipModule } from '@angular/material/tooltip';
import { MatProgressSpinnerModule } from '@angular/material/progress-spinner';
import { QuoteProductTableComponent } from './quote-product-table/quote-product-table.component';
import { RatingCardModule } from '../rating-card/rating-card.module';
import { ShowOrderTotalComponent } from './show-order-total/show-order-total.component';
import { ShowOrderTotalModule } from './show-order-total/show-order-total.module';

@NgModule({
    declarations: [EmailProductTableComponent, QuoteProductTableComponent],
    imports: [
        CommonModule,
        FormsModule,
        ReactiveFormsModule,
        MatFormFieldModule,
        MatInputModule,
        MatIconModule,
        MatButtonModule,
        MatAutocompleteModule,
        TranslocoModule,
        MatProgressSpinnerModule,
        MatTooltipModule,
        RatingCardModule,
        ShowOrderTotalModule,
    ],
    exports: [EmailProductTableComponent],
})
export class EmailProductTableModule {}
