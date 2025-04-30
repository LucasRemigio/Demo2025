/* eslint-disable arrow-parens */
import { ChangeDetectorRef, Component, Input, OnInit } from '@angular/core';
import {
    AbstractControl,
    FormBuilder,
    FormGroup,
    ValidationErrors,
    ValidatorFn,
    Validators,
} from '@angular/forms';
import { TranslocoService } from '@ngneat/transloco';
import { FilteringService } from 'app/modules/filtering/filtering.service';
import {
    EmailInfoDetails,
    MailboxesResponse,
} from 'app/modules/filtering/filtering.types';

@Component({
    selector: 'app-email-form',
    templateUrl: './email-form.component.html',
    styleUrls: ['./email-form.component.scss'],
})
export class EmailFormComponent implements OnInit {
    @Input() destinatary: string = '';

    emailForm: FormGroup;

    copyFields: { cc: boolean; bcc: boolean } = {
        cc: false,
        bcc: false,
    };

    mailboxes: string[];

    filteredRecipients: string[] = [];
    recipients: string[] = [];

    constructor(
        private fb: FormBuilder,
        private _filteringService: FilteringService
    ) {}

    ngOnInit(): void {
        this._filteringService
            .getConfiguredMailboxes()
            .subscribe((response: MailboxesResponse) => {
                this.mailboxes = response.mailboxes;
            });

        this._filteringService
            .getEmailRecipientsList()
            .subscribe((response) => {
                if (response.result_code <= 0) {
                    return;
                }

                this.recipients = response.addresses;
                this.filteredRecipients = [...this.recipients];
            });

        this.emailForm = this.fb.group({
            mailbox: ['', [Validators.required]],
            to: [
                this.destinatary,
                [Validators.required, this.multipleEmailsValidator()],
            ],
            subject: ['', Validators.required],
            cc: ['', [this.multipleEmailsValidator()]],
            bcc: ['', [this.multipleEmailsValidator()]],
        });
    }

    onInput(value: string): void {
        value = value.toLowerCase().trim();

        this.filteredRecipients = this.recipients.filter((recipient) =>
            recipient.toLowerCase().includes(value)
        );
    }

    showCopyField(name: string): void {
        // Return if the name is not one of the available names
        if (name !== 'cc' && name !== 'bcc') {
            return;
        }

        // Show the field
        this.copyFields[name] = !this.copyFields[name];
    }

    get formDetails(): EmailInfoDetails {
        const { mailbox, to, cc, bcc, subject } = this.emailForm.value;

        return {
            mailbox,
            to: this.sanitizeCommaEmails(to),
            cc: cc ? this.sanitizeCommaEmails(cc) : '',
            bcc: bcc ? this.sanitizeCommaEmails(bcc) : '',
            subject,
        };
    }

    private sanitizeCommaEmails(emails: string): string {
        return emails
            .split(',')
            .map((email: string) => email.trim())
            .join(',');
    }

    private multipleEmailsValidator(): ValidatorFn {
        return (control: AbstractControl): ValidationErrors | null => {
            if (!control.value || control.value.trim() === '') {
                // If the field is empty, no error should be returned (valid).
                return null;
            }

            const emails = control.value
                ?.split(',')
                .map((email: string) => email.trim());
            const emailPattern =
                /^[a-zA-Z0-9._-]+@[a-zA-Z0-9.-]+\.[a-zA-Z]{2,6}$/;
            const invalidEmails = emails.filter(
                (email: string) => !emailPattern.test(email)
            );

            return invalidEmails.length > 0 ? { invalidEmails: true } : null;
        };
    }
}
