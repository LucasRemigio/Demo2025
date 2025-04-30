import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';
import { OrderValidateComponent } from './order-validate.component';
import { TranslocoModule } from '@ngneat/transloco';
import { ScrollingModule } from '@angular/cdk/scrolling';
import { FormsModule, ReactiveFormsModule } from '@angular/forms';
import { MatButtonModule } from '@angular/material/button';
import { MatNativeDateModule } from '@angular/material/core';
import { MatDatepickerModule } from '@angular/material/datepicker';
import { MatFormFieldModule } from '@angular/material/form-field';
import { MatIconModule } from '@angular/material/icon';
import { MatInputModule } from '@angular/material/input';
import { MatProgressBarModule } from '@angular/material/progress-bar';
import { MatSelectModule } from '@angular/material/select';
import { OrderValidateTableComponent } from './order-validate-table/order-validate-table.component';
import { OrderValidateTableModule } from './order-validate-table/order-validate-table.module';
import { OrderValidateCardsModule } from './order-validate-cards/order-validate-cards.module';

@NgModule({
    declarations: [OrderValidateComponent],
    imports: [
        CommonModule,
        TranslocoModule,
        FormsModule,
        ReactiveFormsModule,
        MatProgressBarModule,
        MatFormFieldModule,
        MatSelectModule,
        MatDatepickerModule,
        MatNativeDateModule,
        MatInputModule,
        MatIconModule,
        MatButtonModule,
        ScrollingModule,
        OrderValidateTableModule,
        OrderValidateCardsModule,
    ],
    exports: [OrderValidateComponent],
})
export class OrderValidateModule {}
