import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';
import { RouterModule } from '@angular/router';
import { MatIconModule } from '@angular/material/icon';
import { MatProgressBarModule } from '@angular/material/progress-bar';
import { MatButtonModule } from '@angular/material/button';
import { TranslocoModule } from '@ngneat/transloco';
import { OrderValidateCardsComponent } from './order-validate-cards.component';

@NgModule({
    declarations: [OrderValidateCardsComponent],
    imports: [
        CommonModule,
        RouterModule,
        MatIconModule,
        MatProgressBarModule,
        MatButtonModule,
        TranslocoModule,
    ],
    exports: [OrderValidateCardsComponent],
})
export class OrderValidateCardsModule {}
