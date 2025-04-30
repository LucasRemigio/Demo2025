import {
    ChangeDetectorRef,
    Component,
    EventEmitter,
    Input,
    OnDestroy,
    OnInit,
    Output,
    ViewChild,
} from '@angular/core';
import {
    AbstractControl,
    FormBuilder,
    FormGroup,
    ValidationErrors,
    ValidatorFn,
    Validators,
} from '@angular/forms';
import { MatDialogRef } from '@angular/material/dialog';
import { TranslocoService } from '@ngneat/transloco';
import { UserService } from 'app/core/user/user.service';
import { SignatureResponse } from 'app/layout/common/user/user-settings.types';
import { PreviewEmailComponent } from '../filtering-table/preview-email/preview-email.component';
import { FilteringService } from 'app/modules/filtering/filtering.service';
import {
    ReplyByAI,
    ReplyConcurrency,
} from 'app/modules/filtering/filtering.types';
import { QuillEditorComponent } from 'ngx-quill';
import Quill from 'quill';
import { FlashMessageService } from '../flash-message/flash-message.service';

@Component({
    selector: 'app-email-reply-form',
    templateUrl: './email-reply-form.component.html',
    styleUrls: ['./email-reply-form.component.scss'],
})
export class EmailReplyFormComponent implements OnInit, OnDestroy {
    @Input() emailId: string;
    @Input() isReplyToOriginal: string;
    @Input() filteredToken: string;
    @Input() previousResponseWritten: string;
    @Input() cc: string;
    @Input() bcc: string;
    @Input() to: string;

    @Output() responseWritten: EventEmitter<string> =
        new EventEmitter<string>();

    @Output() responseSent: EventEmitter<string> = new EventEmitter<string>();

    composeForm: FormGroup;
    response: string = '';
    selectedFiles: File[] = [];
    invalidFiles: File[] = [];
    quillEditor: any;
    signatureHtml: string;
    showCcBccFields: boolean = true;
    showRecipientField: boolean = true;

    recipients: string[] = [];
    filteredRecipients: string[] = [];

    quillModules = {
        toolbar: [
            [{ header: [1, 2, false] }],
            [{ font: [] }, { size: [] }],
            ['bold', 'italic', 'underline'],
            [{ color: [] }, { background: [] }],
            [{ align: [] }],
            ['link'],
        ],
        clipboard: {
            matchVisual: false,
            matchers: [],
        },
    };

    replyConcurrencyInfo: ReplyConcurrency;
    isLoadingAIResponse: boolean = false;

    isSendingEmail: boolean = false;

    flashMessage: 'success' | 'error' | null = null;
    flashMessageText: string;

    constructor(
        public matDialogRef: MatDialogRef<PreviewEmailComponent>,
        private translocoService: TranslocoService,
        private _formBuilder: FormBuilder,
        private _filteringService: FilteringService,
        private _cdr: ChangeDetectorRef,
        private _userService: UserService,
        private _flashMessageService: FlashMessageService
    ) {}

    translate(key: string, params?: object): string {
        return this.translocoService.translate(key, params || {});
    }

    onEditorCreated(editorInstance: any): void {
        this.quillEditor = editorInstance;
    }

    /**
     * Show flash message
     */
    showFlashMessage(type: 'success' | 'error', textMsg: string): void {
        // Show the message
        this.flashMessage = type;
        this.flashMessageText = textMsg;

        // Mark for check
        this._cdr.markForCheck();

        // Hide it after 3 seconds
        setTimeout(() => {
            this.flashMessage = null;
            this.flashMessageText = null;

            // Mark for check
            this._cdr.markForCheck();
        }, 5000);
    }

    ngOnInit(): void {
        this.composeForm = this._formBuilder.group({
            response: ['', [Validators.required]],
            attachments: [[]],
            cc: [this.cc, [this.multipleEmailsValidator()]],
            bcc: [this.bcc, [this.multipleEmailsValidator()]],
            recipient: [this.to, [this.multipleEmailsValidator()]],
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

        // Define a custom whitelist of formats
        const font = Quill.import('formats/font');
        font.whitelist = ['sans-serif', 'serif', 'monospace'];
        Quill.register(font, true);

        if (
            this.previousResponseWritten !== '' &&
            this.previousResponseWritten !== undefined
        ) {
            // If the user has written a response before, fill the form with that response
            this.composeForm.patchValue({
                response: this.previousResponseWritten,
            });
        } else {
            // If not, just fill the form with his signature to start the reply
            this._filteringService
                .getEmailSignature()
                .subscribe((response: SignatureResponse) => {
                    this.signatureHtml =
                        '<br><br><br><br>' + response.signature.signature;

                    this.composeForm.patchValue({
                        response: this.signatureHtml,
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
    }

    ngOnDestroy(): void {
        const currentResponse = this.composeForm.get('response').value;
        this.responseWritten.emit(currentResponse);
    }

    verifyResponseForm(): void {
        if (this.composeForm.invalid) {
            return;
        }

        const response = this.composeForm.controls['response'].value;
        const attachments = this.selectedFiles;

        /*
        // Close the dialog, sends the response to the higher component
        this.matDialogRef.close({
            response: response,
            attachments: attachments,
        });
        */
        this.createResponse(response, attachments);
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
            this.selectedFiles = this.selectedFiles.concat(...validFiles);
            this.composeForm.patchValue({ attachments: this.selectedFiles });
            this.composeForm.get('attachments')?.updateValueAndValidity();
        }
    }

    removeFile(index: number): void {
        // Create a new array without the file at the specified index
        this.selectedFiles = this.selectedFiles.filter((_, i) => i !== index);

        // If you're tracking the files in a FormControl, update it too
        if (this.composeForm.get('attachments')) {
            this.composeForm.get('attachments').setValue(this.selectedFiles);
            this.composeForm.get('attachments').updateValueAndValidity();
        }
    }

    /**
     * Clears the list of invalid files
     */
    clearInvalidFiles(): void {
        this.invalidFiles = [];

        // If you need to update form validity
        if (this.composeForm.get('attachments')) {
            this.composeForm.get('attachments').updateValueAndValidity();
        }

        // If you're using change detection manually
        this._cdr.markForCheck();
    }

    createResponse(message: string, attachments: File[]): void {
        this.isSendingEmail = true;
        this._flashMessageService.info('Email.sending-reply');

        const formData = new FormData();
        formData.append('response', message);
        attachments.forEach((file) => {
            formData.append('attachments', file, file.name);
        });
        formData.append('isReplyToOriginalEmail', this.isReplyToOriginal);
        formData.append('cc', this.composeForm.get('cc').value);
        formData.append('bcc', this.composeForm.get('bcc').value);
        // if destinatary is not empty, it means its valid, so send the form value
        // Otherwise, send empty, the backend will assume original sender
        formData.append(
            'destinatary',
            this.showRecipientField
                ? this.composeForm.get('recipient').value
                : ''
        );

        // Logic to create response
        this._filteringService
            .postEmailReply(this.emailId, formData)
            .subscribe((response: any) => {
                this.isSendingEmail = false;

                if (response.result !== 'Sucesso') {
                    this.showFlashMessage(
                        'error',
                        this.translocoService.translate('email-sent-error', {})
                    );

                    return;
                }

                this.showFlashMessage(
                    'success',
                    this.translocoService.translate('email-sent-success', {})
                );

                this.responseSent.emit(this.emailId);
                setTimeout(() => {
                    this.matDialogRef.close('Response sent');
                }, 500);
            });
    }

    generateResponseWithAI(emailToken: string): void {
        this.isLoadingAIResponse = true;
        this._filteringService
            .getAIGeneratedReply(emailToken, this.isReplyToOriginal)
            .subscribe(
                (response: ReplyByAI) => {
                    if (response.result !== 'Sucesso') {
                        this.showFlashMessage(
                            'error',
                            this.translocoService.translate(
                                'reply-generation-error',
                                {}
                            )
                        );
                        return;
                    }
                    // Attach to the end of response the name of the employee
                    response.response +=
                        '\n' + this._userService.getLoggedUserName();

                    response.response = this.formatToHtml(response.response);

                    // Typewrite the response to the input box
                    this.typeCharacterByCharacter(response.response, () => {
                        this.isLoadingAIResponse = false;
                    });
                },
                (error: any) => {
                    this.isLoadingAIResponse = false;
                    this.showFlashMessage(
                        'error',
                        this.translocoService.translate(
                            'reply-generation-error',
                            {}
                        )
                    );
                    return;
                }
            );
    }

    formatToHtml = (text: string): string => {
        // Split the text into paragraphs by double line breaks
        const paragraphs = text.split('\n\n');

        // Wrap each paragraph with <p> tags and replace single line breaks within paragraphs with <br>
        const formattedHtml = paragraphs
            .map((paragraph) => {
                const formattedParagraph = paragraph.replace(/\n/g, '<br>');
                return `<p>${formattedParagraph}</p><br>`;
            })
            .join('');

        return formattedHtml;
    };

    typeCharacterByCharacter(
        responseText: string,
        onComplete: () => void
    ): void {
        const currentResponse = this.composeForm.get('response').value;

        let index = 0;
        const speed = 1; // milliseconds per character

        const typingEffect = (): void => {
            if (index < responseText.length) {
                // Append the next character to the current response
                this.composeForm
                    .get('response')
                    .setValue(
                        responseText.substring(0, index + 1) + currentResponse
                    );
                index++;
                // Schedule the next character
                setTimeout(typingEffect, speed);
            } else {
                // Apply styles to all images within .ql-editor using JavaScript
                document
                    .querySelectorAll('div.ql-editor p > img')
                    .forEach((img: any) => {
                        img.style.width = 'auto';
                        img.style.maxWidth = '200px';
                        img.style.height = 'auto';
                        img.style.display = 'inline-block';
                    });

                onComplete();
            }
        };

        typingEffect();
    }

    toggleCcBccFields(): void {
        this.showCcBccFields = !this.showCcBccFields;
    }

    toggleRecipientField(): void {
        this.showRecipientField = !this.showRecipientField;
    }

    multipleEmailsValidator(): ValidatorFn {
        return (control: AbstractControl): ValidationErrors | null => {
            if (!control.value || control.value.trim() === '') {
                // If the field is empty, no error should be returned (valid).
                return null;
            }

            const emails = control.value
                .split(',')
                .map((email: string) => email.trim());

            const invalidEmails = emails.filter(
                (email) => !this.isValidEmail(email)
            );

            return invalidEmails.length > 0 ? { invalidEmails: true } : null;
        };
    }

    // Function to check if an email is valid
    isValidEmail(email: string): boolean {
        email = email.trim();

        // Check for "Name" <email@example.com> format
        const angleBracketsIndex = email.indexOf('<');

        if (angleBracketsIndex === -1) {
            // If no angle brackets, validate the email directly
            return this.isSimpleEmailValid(email);
        }

        // If there are angle brackets, ensure that there is a closing bracket
        const closingAngleBracketIndex = email.indexOf('>');
        if (
            closingAngleBracketIndex === -1 ||
            angleBracketsIndex >= email.lastIndexOf('>')
        ) {
            return false;
        }
        // Extract the email part between the angle brackets
        const emailPart = email
            .slice(angleBracketsIndex + 1, closingAngleBracketIndex)
            .trim();
        if (!this.isSimpleEmailValid(emailPart)) {
            return false;
        }

        return true; // Valid email if it passed all checks
    }

    // Function to check if the email format is simple
    isSimpleEmailValid(email: string): boolean {
        const parts = email.split('@');
        // Basic checks for a valid email
        if (parts.length !== 2) {
            return false;
        } // Must have exactly one '@'
        const [localPart, domainPart] = parts;

        // Check local part length
        if (localPart.length === 0 || localPart.length > 64) {
            return false;
        }

        // Check domain part length and format
        const domainParts = domainPart.split('.');
        if (
            domainParts.length < 2 ||
            domainParts.some((part) => part.length === 0)
        ) {
            return false; // Domain must contain at least one '.' and cannot be empty
        }

        return true; // Valid simple email
    }
}
