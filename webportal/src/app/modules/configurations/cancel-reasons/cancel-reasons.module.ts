import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';
import { CancelReasonsComponent } from './cancel-reasons.component';
import { TranslocoModule } from '@ngneat/transloco';
import { MatIconModule } from '@angular/material/icon';
import { GridModule } from '@progress/kendo-angular-grid';
import { MatTooltipModule } from '@angular/material/tooltip';
import { MatButtonModule } from '@angular/material/button';
import { EditCancelReasonComponent } from './edit-cancel-reason/edit-cancel-reason.component';
import { MatDialogModule } from '@angular/material/dialog';
import { ReactiveFormsModule } from '@angular/forms';
import { MatFormFieldModule } from '@angular/material/form-field';
import { MatInputModule } from '@angular/material/input';

@NgModule({
    declarations: [CancelReasonsComponent, EditCancelReasonComponent],
    imports: [
        CommonModule,
        TranslocoModule,
        GridModule,
        MatIconModule,
        MatTooltipModule,
        MatButtonModule,
        MatDialogModule,
        ReactiveFormsModule,
        MatFormFieldModule,
        MatInputModule,
    ],
})
export class CancelReasonsModule {}
