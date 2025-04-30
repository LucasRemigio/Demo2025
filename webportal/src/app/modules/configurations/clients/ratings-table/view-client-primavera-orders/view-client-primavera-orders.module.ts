import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';
import { ViewClientPrimaveraOrdersComponent } from './view-client-primavera-orders.component';
import { MatButtonModule } from '@angular/material/button';
import { MatIconModule } from '@angular/material/icon';
import { MatProgressSpinnerModule } from '@angular/material/progress-spinner';
import { TranslocoModule } from '@ngneat/transloco';

@NgModule({
    declarations: [ViewClientPrimaveraOrdersComponent],
    imports: [
        CommonModule,
        MatIconModule,
        MatButtonModule,
        MatProgressSpinnerModule,
        TranslocoModule,
    ],
    exports: [ViewClientPrimaveraOrdersComponent],
})
export class ViewClientPrimaveraOrdersModule {}
