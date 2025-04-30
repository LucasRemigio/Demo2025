import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';
import { ShowOrderTotalComponent } from './show-order-total.component';
import { TranslocoModule } from '@ngneat/transloco';

@NgModule({
    declarations: [ShowOrderTotalComponent],
    imports: [CommonModule, TranslocoModule],
    exports: [ShowOrderTotalComponent],
})
export class ShowOrderTotalModule {}
