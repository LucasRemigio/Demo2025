/* eslint-disable quote-props */
/* eslint-disable arrow-parens */
import {
    ChangeDetectorRef,
    Component,
    Input,
    OnInit,
    ViewChild,
} from '@angular/core';
import { FormBuilder, FormGroup, Validators } from '@angular/forms';
import { ActivatedRoute, Router } from '@angular/router';
import { translate } from '@ngneat/transloco';
import { EmailFormComponent } from 'app/modules/communications/compose/email-form/email-form.component';
import { FilteringService } from 'app/modules/filtering/filtering.service';
import {
    EmailInfoDetails,
    EmailReplyTemplateResponse,
} from 'app/modules/filtering/filtering.types';
import { OrderObservationsItem } from 'app/modules/orders/order.types';
import { OrderService } from 'app/shared/components/confirm-order-address/order.service';
import { OrderDocument } from 'app/shared/components/email-product-table/products.types';
import { FlashMessageService } from 'app/shared/components/flash-message/flash-message.service';
import { FilteredEmail, OrderDTO } from '../details.types';

const PRODUCTS_TAG = '@Produtos';
@Component({
    selector: 'app-preview-order-documents',
    templateUrl: './preview-order-documents.component.html',
    styleUrls: ['./preview-order-documents.component.scss'],
})
export class PreviewOrderDocumentsComponent implements OnInit {
    @ViewChild(EmailFormComponent) emailFormComponent: EmailFormComponent;

    @Input() order: OrderDTO;
    @Input() filteredEmail: FilteredEmail | null;
    @Input() observations: OrderObservationsItem;

    documents: OrderDocument[] = [];
    isLoading: boolean = true;
    isSendingEmail: boolean = false;
    emailForm: FormGroup;

    zoom: number = 0.6;
    isFullscreen: boolean = false;
    template: string;

    quillEditor: any;
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

    constructor(
        private _orderService: OrderService,
        private _changeDetector: ChangeDetectorRef,
        private _formBuilder: FormBuilder,
        private _filteringService: FilteringService,
        private _fm: FlashMessageService,
        private _router: Router,
        private _activatedRoute: ActivatedRoute
    ) {}

    ngOnInit(): void {
        this.fetchData();

        this.createForm();
    }

    createForm(): void {
        this.emailForm = this._formBuilder.group({
            body: [
                '',
                [
                    Validators.required,
                    Validators.minLength(10),
                    Validators.maxLength(10000),
                ],
            ],
        });
    }

    getDestinatary(): string {
        if (this.filteredEmail) {
            return this.filteredEmail.email.from || '';
        }

        let email = '';
        const client = this.order.client.primavera_client;
        if (client) {
            email = client.emailCliente || client.email || '';
        }

        return email;
    }

    fetchData(): void {
        this._orderService.getDocuments(this.order.token).subscribe((res) => {
            if (res.result_code <= 0) {
                // error
                return;
            }

            this.documents = res.documents;
            this._changeDetector.markForCheck();
            this.isLoading = false;
        });

        this._filteringService
            .getReplyEmailTemplate(this.order.token)
            .subscribe((response: EmailReplyTemplateResponse) => {
                const signatureHtml = '<br><br><br><br>' + response.signature;

                this.template = response.template;
                const quillTemplate = this.substitudeProductsWithTag(
                    response.template
                );
                const initialBody = quillTemplate + signatureHtml;

                this.emailForm.patchValue({
                    body: initialBody,
                });

                this.emailForm.updateValueAndValidity();
                this._changeDetector.detectChanges();
            });
    }

    substitudeProductsWithTag(template: string): string {
        // the template will have a table <table>etc...</table> with the products, change all that
        // with a @Products tag
        if (this.order.is_draft) {
            return template;
        }

        const productsTag = PRODUCTS_TAG;
        const productsTable = template.match(/<table.*<\/table>/g);
        if (!productsTable) {
            return template;
        }

        return template.replace(productsTable[0], productsTag);
    }

    getBodyWithTagSubstitingWithProducts(): string {
        // check if there is the tag in9 the body, else return the body
        const emailBody = this.emailForm.get('body')?.value;

        if (this.order.is_draft) {
            return emailBody;
        }

        if (!emailBody.includes(PRODUCTS_TAG)) {
            return '';
        }
        // grab the initial products from the original template
        const productsTable = this.template.match(/<table.*<\/table>/g);
        if (!productsTable) {
            return emailBody;
        }

        // then find the @Produtos tag and substitude the tag with the template
        return emailBody.replace(PRODUCTS_TAG, productsTable[0]);
    }

    formatQuillContent(): void {
        document
            .querySelectorAll('div.ql-editor p > img')
            .forEach((img: any) => {
                img.style.width = 'auto';
                img.style.maxWidth = '200px';
                img.style.height = 'auto';
                img.style.display = 'inline-block';
            });

        // Directly apply height to all .ql-container elements
        const height = '600px';
        document.querySelectorAll('.ql-container').forEach((container: any) => {
            container.style.height = height;
            container.style.maxHeight = height;
        });

        document.querySelectorAll('.ql-editor').forEach((editor: any) => {
            editor.style.height = height;
            editor.style.maxHeight = height;
        });
    }

    onEditorCreated(editorInstance: any): void {
        this.quillEditor = editorInstance;

        this.formatQuillContent();

        this.quillEditor.on('editor-change', () => {
            if (!this.quillEditor.formattingApplied) {
                this.formatQuillContent();
                this.quillEditor.formattingApplied = true;
            }
        });
    }

    downloadFile(orderDoc: OrderDocument, event: MouseEvent): void {
        event.preventDefault();

        const bytes = this.convertBase64ToBinary(orderDoc.invoice_html);
        const blob = new Blob([bytes], {
            type: 'application/octet-stream',
        });
        const url = window.URL.createObjectURL(blob);

        // Create a link element, set its download attribute and click it
        const a = document.createElement('a');
        a.href = url;
        a.download = orderDoc.name + '.pdf'; // Set the file name
        document.body.appendChild(a);
        a.click();

        // Clean up by revoking the object URL and removing the link element
        window.URL.revokeObjectURL(url);
        document.body.removeChild(a);
    }

    printFile(orderDoc: OrderDocument, event: MouseEvent): void {
        event.preventDefault();

        // Convert the base64 string to binary data.
        const bytes = this.convertBase64ToBinary(orderDoc.invoice_html);
        // Use 'application/pdf' as the MIME type (make sure your data really is PDF)
        const blob = new Blob([bytes], { type: 'application/pdf' });
        const url = window.URL.createObjectURL(blob);

        // Open a new window or tab with the blob URL.
        const printWindow = window.open(url, '_blank');

        if (!printWindow) {
            console.error('Popup blocked - unable to open print window.');
            return;
        }

        printWindow.onafterprint = (): void => {
            printWindow.close();
        };

        // Once the new window loads, trigger printing.
        printWindow.addEventListener('load', () => {
            printWindow.focus();
            printWindow.print();
        });
    }

    sendEmail(): void {
        if (!this.emailForm.valid) {
            console.error('Invalid email form');
            return;
        }

        // Set sending status and info message.
        this.isSendingEmail = true;
        this._fm.info(
            this.filteredEmail ? 'Email.sending-reply' : 'Email.sending-email'
        );

        // Build the common FormData object.
        const formData = this.buildEmailFormData();
        if (!formData) {
            this.isSendingEmail = false;
            return;
        }
        // Append file attachments to FormData.
        this.appendAttachments(formData);

        this.executeSendOrReplyEmail(formData);
    }

    executeSendOrReplyEmail(formData: FormData): void {
        // Choose the correct service call based on email type.
        const emailRequest = this.filteredEmail
            ? this._filteringService.postEmailReply(
                  this.filteredEmail.email.id,
                  formData
              )
            : this._filteringService.postCreateEmail(formData);

        // Subscribe to the email sending observable.
        emailRequest.subscribe(
            (response: any) => {
                // Handle different error conditions for reply vs. root email.
                if (this.filteredEmail) {
                    if (response.result_code <= 0) {
                        this._fm.error('email-sent-error');
                        return;
                    }
                } else {
                    this._fm.clear();
                    if (!response || response.result_code <= -1) {
                        const errorMessages: { [key: number]: string } = {
                            [-65]: 'email-sent-error', // Error sending the email
                            [-66]: 'email-sent-but-error-saving-email', // Error saving email in the DB
                            [-67]: 'email-sent-but-error-saving-attachment', // Error saving attachments
                        };
                        const errorMessageValue =
                            errorMessages[response?.result_code] ||
                            'email-sent-error';
                        this._fm.error(errorMessageValue);
                        return;
                    }
                }
                // On success, show a success message and confirm the order.
                this._fm.success('email-sent-success');
                this.setPendingClientConfirmation();
            },
            (error) => {
                this._fm.error('email-sent-error');
            },
            () => {
                this.isSendingEmail = false;
            }
        );
    }

    buildEmailFormData(): FormData | null {
        const formData = new FormData();

        if (this.filteredEmail) {
            // Reply email: add response body and recipient info.
            const body = this.getBodyWithTagSubstitingWithProducts();
            if (!body) {
                this._fm.nterror(
                    PRODUCTS_TAG + ' ' + translate('Order.tag-is-missing')
                );
                return null;
            }
            formData.append('response', body);
            formData.append('destinatary', this.filteredEmail.email.from);
            formData.append('isReplyToOriginalEmail', 'true');
        } else {
            // Root email: validate child form as well.
            if (!this.emailFormComponent.emailForm.valid) {
                this.emailForm.markAllAsTouched();
                this.emailFormComponent.emailForm.markAllAsTouched();
                this._fm.error('email-form-invalid');
                return null;
            }
            const emailData: EmailInfoDetails =
                this.emailFormComponent.formDetails;
            formData.append('mailbox', emailData.mailbox);
            formData.append('to', emailData.to);
            formData.append('cc', emailData.cc || '');
            formData.append('bcc', emailData.bcc || '');
            formData.append('subject', emailData.subject);
            formData.append('body', this.emailForm.get('body')?.value);
        }

        return formData;
    }

    appendAttachments(formData: FormData): void {
        this.documents.forEach((doc) => {
            // Convert Base64 string to binary data.
            const bytes = this.convertBase64ToBinary(doc.invoice_html);
            // Create a PDF file from the binary data.
            const pdfFile = new File([bytes], `${doc.name}.pdf`, {
                type: 'application/pdf',
            });
            formData.append('attachments', pdfFile);
        });
    }

    setPendingClientConfirmation(): void {
        this._orderService
            .setPendingClientConfirmation(this.order.token)
            .subscribe(
                (response) => {
                    if (response.result_code <= 0) {
                        this._fm.error('Order.confirm-error');
                        this.isSendingEmail = false;
                        return;
                    }

                    this._fm.success('Order.confirm-success');

                    this._router.navigate(['../'], {
                        relativeTo: this._activatedRoute,
                    });
                },
                (error) => {
                    this._fm.error('Order.confirm-error');
                },
                () => {
                    this.isSendingEmail = false;
                }
            );
    }

    zoomIn(): void {
        this.zoom += 0.1;
    }

    zoomOut(): void {
        if (this.zoom <= 0.4) {
            return;
        }

        this.zoom -= 0.1;
    }

    toggleFullscreenPDF(): void {
        this.isFullscreen = !this.isFullscreen;
    }

    private convertBase64ToBinary(base64: string): Uint8Array {
        const binaryString = atob(base64);
        const len = binaryString.length;
        const bytes = new Uint8Array(len);
        for (let i = 0; i < len; i++) {
            bytes[i] = binaryString.charCodeAt(i);
        }
        return bytes;
    }
}
