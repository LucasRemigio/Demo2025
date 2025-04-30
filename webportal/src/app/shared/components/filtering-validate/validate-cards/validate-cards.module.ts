import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';
import { RouterModule } from '@angular/router';
import { MatIconModule } from '@angular/material/icon';
import { MatProgressBarModule } from '@angular/material/progress-bar';
import { MatButtonModule } from '@angular/material/button';
import { TranslocoModule } from '@ngneat/transloco';
import { ValidateCardsComponent } from './validate-cards.component';

@NgModule({
    declarations: [ValidateCardsComponent],
    imports: [
        CommonModule,
        RouterModule,
        MatIconModule,
        MatProgressBarModule,
        MatButtonModule,
        TranslocoModule,
    ],
    exports: [ValidateCardsComponent],
})
export class ValidateCardsModule {}
