import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';
import { FormsModule, ReactiveFormsModule } from '@angular/forms';
import { MatButtonModule } from '@angular/material/button';
import { MatFormFieldModule } from '@angular/material/form-field';
import { MatIconModule } from '@angular/material/icon';
import { MatInputModule } from '@angular/material/input';
import { MatProgressSpinnerModule } from '@angular/material/progress-spinner';
import { MatTooltipModule } from '@angular/material/tooltip';
import { TranslocoModule } from '@ngneat/transloco';
import { QuillModule } from 'ngx-quill';
import { EmailReplyFormComponent } from './email-reply-form.component';
import { MatAutocompleteModule } from '@angular/material/autocomplete';

@NgModule({
    declarations: [EmailReplyFormComponent],
    imports: [
        CommonModule,
        FormsModule,
        ReactiveFormsModule,
        TranslocoModule,
        MatFormFieldModule,
        MatIconModule,
        MatInputModule,
        MatButtonModule,
        QuillModule.forRoot(),
        MatProgressSpinnerModule,
        MatAutocompleteModule,
        MatTooltipModule,
    ],
    exports: [EmailReplyFormComponent],
})
export class EmailReplyFormModule {}
