import { NgModule } from '@angular/core';
import { CommonModule, formatDate } from '@angular/common';
import { FormsModule, ReactiveFormsModule } from '@angular/forms';
import { EmailReplyFormComponent } from './components/email-reply-form/email-reply-form.component';
import { TranslocoModule } from '@ngneat/transloco';
import { MatFormFieldModule } from '@angular/material/form-field';
import { MatIconModule } from '@angular/material/icon';
import { MatInputModule } from '@angular/material/input';
import { MatButtonModule } from '@angular/material/button';
import { QuillModule } from 'ngx-quill';
import { MatProgressSpinnerModule } from '@angular/material/progress-spinner';
import { MatTooltipModule } from '@angular/material/tooltip';
import { StepsListComponent } from './components/steps-list/steps-list.component';
import { OrderValidateComponent } from './components/order-validate/order-validate.component';
import { OrderTableComponent } from './components/order-table/order-table.component';

@NgModule({
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
        MatTooltipModule,
    ],
    exports: [
        CommonModule,
        FormsModule,
        ReactiveFormsModule,
        TranslocoModule,
        MatFormFieldModule,
        MatIconModule,
        MatInputModule,
        QuillModule,
        MatButtonModule,
        MatProgressSpinnerModule,
        MatTooltipModule,
    ],
})
export class SharedModule {
    generateNewPass(): string {
        const lenghtThreshold = 20;
        const validCharsLowerLetters = 'abcdefghijklmnopqrstuvwxyz';
        const validCharsUpperLetters = 'ABCDEFGHIJKLMNOPQRSTUVWXYZ';
        const validCharsNumbers = '1234567890';
        const validCharsSpecialChars = '!@#$%^&*()+=-|?/<>,.';

        var generatedPassword = '';

        for (let i = 0; i < lenghtThreshold / 4; i++) {
            let indexLowerLetters = Math.floor(
                Math.random() * validCharsLowerLetters.length
            );
            let indexUpperLetters = Math.floor(
                Math.random() * validCharsUpperLetters.length
            );
            let indexCharsNumbers = Math.floor(
                Math.random() * validCharsNumbers.length
            );
            let indexCharsSpecialChars = Math.floor(
                Math.random() * validCharsSpecialChars.length
            );
            generatedPassword +=
                validCharsLowerLetters[indexLowerLetters] +
                validCharsUpperLetters[indexUpperLetters] +
                validCharsNumbers[indexCharsNumbers] +
                validCharsSpecialChars[indexCharsSpecialChars];
        }

        return generatedPassword;
    }

    getPasswordValidatorPattern(): string {
        return '(?=.*[a-z])(?=.*[A-Z])(?=.*[0-9]).{5,}';
    }

    getPasswordValidatorError(): string {
        return 'Password must have between 5 and 30 characters length, lowercase, and upper case letters and at least one number';
    }

    static adjustForTimezone(date: Date): Date {
        var timeOffsetInMS: number = date.getTimezoneOffset() * 60000;
        date.setTime(date.getTime() + timeOffsetInMS);
        return date;
    }

    static isValidDate(dateStr) {
        const date = new Date(dateStr);
        return !isNaN(date.getTime()); // Retorna true se for uma data válida
    }

    static validateFormatDate(dateStr): string {
        if (!this.isValidDate(dateStr)) {
            return '';
        }

        if (dateStr) {
            return formatDate(dateStr, 'yyyy-MM-dd', 'en-US');
        }

        return '';
    }
}
