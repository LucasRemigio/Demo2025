import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';
import { RatingCardComponent } from './rating-card.component';
import { TranslocoModule } from '@ngneat/transloco';
import { MatIconModule } from '@angular/material/icon';
import { MatTooltipModule } from '@angular/material/tooltip';

@NgModule({
    declarations: [RatingCardComponent],
    imports: [CommonModule, TranslocoModule, MatIconModule, MatTooltipModule],
    exports: [RatingCardComponent],
})
export class RatingCardModule {}
