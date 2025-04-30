import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';
import { ViewClientPrimaveraInvoicesComponent } from './view-client-primavera-invoices.component';
import { MatButtonModule } from '@angular/material/button';
import { MatIconModule } from '@angular/material/icon';
import { MatProgressSpinnerModule } from '@angular/material/progress-spinner';
import { TranslocoModule } from '@ngneat/transloco';
import { InvoiceCardModule } from './invoice-card/invoice-card.module';

@NgModule({
    declarations: [ViewClientPrimaveraInvoicesComponent],
    imports: [
        CommonModule,
        MatIconModule,
        MatButtonModule,
        MatProgressSpinnerModule,
        TranslocoModule,
        InvoiceCardModule,
    ],
    exports: [ViewClientPrimaveraInvoicesComponent],
})
export class ViewClientPrimaveraInvoicesModule {}
