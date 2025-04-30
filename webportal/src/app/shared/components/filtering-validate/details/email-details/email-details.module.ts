import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';
import { EmailDetailsComponent } from './email-details.component';
import { TranslocoModule } from '@ngneat/transloco';
import { ReactiveFormsModule } from '@angular/forms';
import { MatDatepickerModule } from '@angular/material/datepicker';
import { MatFormFieldModule } from '@angular/material/form-field';
import { MatInputModule } from '@angular/material/input';

@NgModule({
    declarations: [EmailDetailsComponent],
    imports: [
        CommonModule,
        ReactiveFormsModule,
        MatFormFieldModule,
        MatInputModule,
        MatDatepickerModule,
        TranslocoModule,
    ],
    exports: [EmailDetailsComponent],
})
export class EmailDetailsModule {}
