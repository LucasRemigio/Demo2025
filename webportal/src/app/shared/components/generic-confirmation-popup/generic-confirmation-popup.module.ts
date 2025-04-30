import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';
import { GenericConfirmationPopupComponent } from './generic-confirmation-popup.component';
import { TranslocoModule } from '@ngneat/transloco';
import { MatButtonModule } from '@angular/material/button';
import { MatIconModule } from '@angular/material/icon';
import { MatDialogModule } from '@angular/material/dialog';

@NgModule({
    declarations: [GenericConfirmationPopupComponent],
    imports: [
        CommonModule,
        TranslocoModule,
        MatButtonModule,
        MatIconModule,
        MatDialogModule,
    ],
    exports: [GenericConfirmationPopupComponent],
})
export class GenericConfirmationPopupModule {}
