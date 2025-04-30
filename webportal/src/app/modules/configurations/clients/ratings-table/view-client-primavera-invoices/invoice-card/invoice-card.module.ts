import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';
import { InvoiceCardComponent } from './invoice-card.component';
import { TranslocoModule } from '@ngneat/transloco';

@NgModule({
    declarations: [InvoiceCardComponent],
    imports: [CommonModule, TranslocoModule],
    exports: [InvoiceCardComponent],
})
export class InvoiceCardModule {}
