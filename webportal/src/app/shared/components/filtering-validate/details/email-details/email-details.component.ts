/* eslint-disable @typescript-eslint/naming-convention */
import { DatePipe } from '@angular/common';
import { Component, Input, OnInit } from '@angular/core';
import { FormBuilder, FormGroup, Validators } from '@angular/forms';
import { DomSanitizer, SafeHtml } from '@angular/platform-browser';
import { Email } from 'app/modules/common/common.types';

@Component({
    selector: 'app-email-details',
    templateUrl: './email-details.component.html',
    styleUrls: ['./email-details.component.scss'],
})
export class EmailDetailsComponent implements OnInit {
    @Input() email: Email;
    @Input() showTitle: boolean = true;

    composeForm: FormGroup = new FormGroup({});
    sanitizedEmailBody: SafeHtml;

    constructor(
        private datePipe: DatePipe,
        private _formBuilder: FormBuilder,
        private sanitizer: DomSanitizer
    ) {}

    ngOnInit(): void {
        this.email.cc = this.formatAddressList(this.email.cc);
        this.email.bcc = this.formatAddressList(this.email.bcc);

        const rawEmailHtml = this.email.body;
        // Sanitize the HTML content to make it safe for rendering
        this.sanitizedEmailBody =
            this.sanitizer.bypassSecurityTrustHtml(rawEmailHtml);
        this.createForm();
    }

    // Method to format the date
    formatDate(date: any): string {
        return this.datePipe.transform(date, 'dd/MM/yyyy HH:mm:ss') || '';
    }

    formatAddressList(addresses?: string): string {
        if (!addresses) {
            return '';
        }
        if (addresses === '') {
            return '';
        }

        return addresses.replace(/,/g, ', ');
    }

    createForm(): void {
        this.composeForm = this._formBuilder.group({
            email_sender: [
                { value: this.email.from, disabled: true },
                Validators.required,
            ],
            email_date: [
                {
                    value: this.formatDate(this.email.date),
                    disabled: true,
                },
                Validators.required,
            ],
            email_subject: {
                value: this.email.subject,
                disabled: true,
            },
            email_cc: {
                value: this.email.cc,
                disabled: true,
            },
            email_bcc: {
                value: this.email.bcc,
                disabled: true,
            },
            products: this._formBuilder.array([]),
            addresses: this._formBuilder.array([]),
        });
    }
}
