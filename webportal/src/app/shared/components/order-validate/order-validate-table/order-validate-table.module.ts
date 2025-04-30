import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';
import { MatBadgeModule } from '@angular/material/badge';
import { MatButtonModule } from '@angular/material/button';
import { MatIconModule } from '@angular/material/icon';
import { MatProgressBarModule } from '@angular/material/progress-bar';
import { MatTooltipModule } from '@angular/material/tooltip';
import { TranslocoModule } from '@ngneat/transloco';
import { GridModule } from '@progress/kendo-angular-grid';
import { RouterModule } from '@angular/router';
import { OrderValidateTableComponent } from './order-validate-table.component';

@NgModule({
    declarations: [OrderValidateTableComponent],
    imports: [
        CommonModule,
        TranslocoModule,
        MatProgressBarModule,
        MatIconModule,
        MatBadgeModule,
        MatTooltipModule,
        MatButtonModule,
        GridModule,
        RouterModule,
    ],
    exports: [OrderValidateTableComponent],
})
export class OrderValidateTableModule {}
