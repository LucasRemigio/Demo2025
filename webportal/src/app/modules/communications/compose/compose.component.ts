/* eslint-disable arrow-parens */
import { HttpStatusCode } from '@angular/common/http';
import { ChangeDetectorRef, Component, OnInit, ViewChild } from '@angular/core';
import {
    AbstractControl,
    FormBuilder,
    FormGroup,
    ValidationErrors,
    ValidatorFn,
    Validators,
} from '@angular/forms';
import { TranslocoService } from '@ngneat/transloco';
import { SignatureResponse } from 'app/layout/common/user/user-settings.types';
import { FilteringService } from 'app/modules/filtering/filtering.service';
import {
    EmailInfoDetails,
    MailboxesResponse,
} from 'app/modules/filtering/filtering.types';
import { FlashMessageService } from 'app/shared/components/flash-message/flash-message.service';
import { EmailFormComponent } from './email-form/email-form.component';

@Component({
    selector: 'app-compose',
    templateUrl: './compose.component.html',
    styleUrls: ['./compose.component.scss'],
})
export class ComposeComponent implements OnInit {
    @ViewChild(EmailFormComponent) emailFormComponent!: EmailFormComponent;
    emailForm: FormGroup;

    selectedFiles: File[] = [];
    invalidFiles: File[] = [];

    flashMessage: 'success' | 'error' | null = null;
    flashMessageText: string;

    showCommaEmailInfo: boolean = true;

    isSendingEmail: boolean = false;


    constructor(
        private fb: FormBuilder,
        private _filteringService: FilteringService,
        private _changeDetectorRef: ChangeDetectorRef,
        private translocoService: TranslocoService,
        private _fm: FlashMessageService
    ) {}

    /**
     * Show flash message
     */
    showFlashMessage(type: 'success' | 'error', textMsg: string): void {
        // Show the message
        this.flashMessage = type;
        this.flashMessageText = textMsg;

        // Mark for check
        this._changeDetectorRef.markForCheck();

        // Hide it after 3 seconds
        setTimeout(() => {
            this.flashMessage = null;
            this.flashMessageText = null;

            // Mark for check
            this._changeDetectorRef.markForCheck();
        }, 5000);
    }

    ngOnInit(): void {
        this.fetchSignature();

        this.emailForm = this.fb.group({
            body: ['', Validators.required],
            attachments: [[]],
        });
    }

    fetchSignature(): void {
        this._filteringService
            .getEmailSignature()
            .subscribe((response: SignatureResponse) => {
                const signatureHtml =
                    '<br><br><br><br>' + response.signature.signature;
                this.emailForm.patchValue({
                    body: signatureHtml,
                });

                // Apply styles to all images within .ql-editor using JavaScript
                document
                    .querySelectorAll('div.ql-editor p > img')
                    .forEach((img: any) => {
                        img.style.width = 'auto';
                        img.style.maxWidth = '200px';
                        img.style.height = 'auto';
                        img.style.display = 'inline-block';
                    });
            });
    }

    sendEmail(): void {
        if (!this.emailForm.valid || !this.emailFormComponent.emailForm.valid) {
            this.emailForm.markAllAsTouched();
            this.emailFormComponent.emailForm.markAllAsTouched();
            this._fm.error('email-form-invalid');
            return;
        }

        this.isSendingEmail = true;
        this._fm.info('Email.sending-email');
        // the mailbox, to, cc, bcc and subject are on the child component
        const emailData: EmailInfoDetails = this.emailFormComponent.formDetails;

        const formData = new FormData();
        formData.append('mailbox', emailData.mailbox);
        formData.append('to', emailData.to);
        formData.append('cc', emailData.cc || '');
        formData.append('bcc', emailData.bcc || '');
        formData.append('subject', emailData.subject);

        formData.append('body', this.emailForm.get('body')?.value);

        // Append file attachments
        for (const file of this.selectedFiles) {
            formData.append('attachments', file, file.name);
        }

        const errorMessages: { [key: number]: string } = {
            [-65]: 'email-sent-error', // Error sending the email
            [-66]: 'email-sent-but-error-saving-email', // Error saving email in the database
            [-67]: 'email-sent-but-error-saving-attachment', // Error saving attachments in the database
        };

        // Call the createEmail method from the service
        this._filteringService.postCreateEmail(formData).subscribe(
            (response) => {
                this._fm.clear();
                if (!response || response.result_code <= -1) {
                    const errorMessageValue =
                        errorMessages[response?.result_code] ||
                        'email-sent-error';

                    this._fm.error(errorMessageValue);

                    return;
                }

                this._fm.success('email-sent-success');

                setTimeout(() => {
                    this.emailForm.reset();
                    this.emailFormComponent.emailForm.reset();

                    this.fetchSignature();
                }, 3000);
            },
            (error) => {
                this._fm.error('email-sent-error');
            },
            () => {
                this.isSendingEmail = false;
            }
        );
    }

    getFileSizeInMB(sizeInBytes: number): string {
        const sizeInMB = sizeInBytes / (1024 * 1024);
        return sizeInMB.toFixed(2) + ' MB';
    }

    onFileSelected(event: any): void {
        const maxFileSize = 20 * 1024 * 1024; // 20MB size limit
        const allowedExtensions = [
            '.bmp',
            '.gif',
            '.jpg',
            '.jpeg',
            '.pdf',
            '.doc',
            '.docx',
            '.xls',
            '.xlsx',
            '.csv',
            '.zip',
        ];
        const validFiles: File[] = [];
        this.invalidFiles = [];
        let totalSize = 0;

        if (event.target.files) {
            const selectedFiles = Array.from(event.target.files) as File[];

            selectedFiles.forEach((file) => {
                const fileExtension =
                    '.' + file.name.split('.').pop()?.toLowerCase();
                const fileSize = file.size;

                if (
                    allowedExtensions.includes(fileExtension) &&
                    fileSize <= maxFileSize &&
                    totalSize + fileSize <= maxFileSize
                ) {
                    validFiles.push(file);
                    totalSize += fileSize;
                } else {
                    this.invalidFiles.push(file);
                }
            });
            this.selectedFiles = validFiles;
            this.emailForm.patchValue({ attachments: this.selectedFiles });
            this.emailForm.get('attachments')?.updateValueAndValidity();
        }
    }

    closeForwardedEmailInfo(): void {
        this.showCommaEmailInfo = false;
    }
}
