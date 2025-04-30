/* eslint-disable @typescript-eslint/naming-convention */
/* eslint-disable arrow-parens */
import {
    Component,
    OnInit,
    Inject,
    ViewChild,
    ChangeDetectorRef,
    OnDestroy,
    HostListener,
    ViewEncapsulation,
} from '@angular/core';
import { MatDialogRef, MAT_DIALOG_DATA } from '@angular/material/dialog';
import { TranslocoService } from '@ngneat/transloco';
import { MatDrawer } from '@angular/material/sidenav';
import { FuseSplashScreenService } from '@fuse/services/splash-screen';
import { FormBuilder, FormGroup, Validators } from '@angular/forms';
import { FilteringService } from 'app/modules/filtering/filtering.service';
import {
    FilteringPopupData,
    ReplyConcurrency,
    EMAIL_CATEGORIES,
    EmailAttachment,
    UserJoinedWs,
    EmailStatusUpdateWs,
    EMAIL_STATUSES,
} from 'app/modules/filtering/filtering.types';
import { CardCategory } from 'app/modules/orders/order.types';
import { Category } from 'app/modules/common/common.types';
import { UserService } from 'app/core/user/user.service';
import { DomSanitizer, SafeHtml } from '@angular/platform-browser';
import 'pdfjs-dist/build/pdf.worker.entry';
import { PrintingService } from './printing.service';
import DOMPurify from 'dompurify';
import { DatePipe } from '@angular/common';
import { FlashMessageService } from '../../flash-message/flash-message.service';
import { AuditHubService } from '../audit-hub.service';

@Component({
    selector: 'app-preview-email',
    templateUrl: './preview-email.component.html',
    styleUrls: ['../filtering-table.component.scss'],
    encapsulation: ViewEncapsulation.None,
    providers: [DatePipe],
    styles: [
        `
            .panel-title {
                padding-left: 2rem;
                padding-top: 2rem;
                padding-bottom: 2rem;
                box-sizing: border-box;
            }

            pdf-viewer {
                ::ng-deep {
                    .ng2-pdf-viewer-container {
                        height: -webkit-fill-available;
                        width: -webkit-fill-available;
                    }
                }
            }

            .drawer-content {
                width: 50rem;
            }

            @media (min-width: 320px) and (max-width: 960px) {
                .drawer-content {
                    width: auto;
                }
            }
        `,
    ],
})
export class PreviewEmailComponent implements OnInit, OnDestroy {
    @ViewChild('drawer') drawer: MatDrawer;
    isLoading: boolean = false;
    isMarkingEmailAsSpam: boolean = false;

    categoryForm: FormGroup;
    hasAttachments: boolean;
    isAnyAttachmentPdf: boolean;
    pdfAttachments: EmailAttachment[] = [];

    categories: CardCategory[];

    emailCategories = EMAIL_CATEGORIES;

    drawerMode: 'over' | 'side' = 'side';
    drawerOpened: boolean = true;
    panels: any[] = [];
    selectedPanel: string = 'emailPanel';
    showPdfPanel: boolean = false;
    replyConcurrencyInfo: ReplyConcurrency;

    signature: SafeHtml;
    sanitizedEmailBody: SafeHtml;
    rawEmailHtml: string;

    previousResponseWritten: string = '';
    isEditingFrom: boolean = false;
    emailFrom: string;
    distinctEmails: string[] = [];

    flashMessage: 'success' | 'error' | null = null;
    flashMessageText: string;

    constructor(
        public matDialogRef: MatDialogRef<PreviewEmailComponent>,
        private _changeDetectorRef: ChangeDetectorRef,
        private translocoService: TranslocoService,
        private _fuseSplashScreen: FuseSplashScreenService,
        private _formBuilder: FormBuilder,
        private _filteringService: FilteringService,
        private sanitizer: DomSanitizer,
        private _printingService: PrintingService,
        private datePipe: DatePipe,
        private _flashMessageService: FlashMessageService,
        private _auditHubWs: AuditHubService,

        @Inject(MAT_DIALOG_DATA) public data: FilteringPopupData
    ) {}

    @HostListener('window:beforeunload', ['$event'])
    handleBeforeUnload(event: Event): void {
        // Perform your cleanup here before the page is refreshed
        this.removeUserFromReplyConcurrency(this.data.filteredEmail.token);
    }

    translate(key: string, params?: object): string {
        return this.translocoService.translate(key, params || {});
    }

    getFormattedDate(date: string | Date): string {
        return this.datePipe.transform(date, 'dd/MM/yyyy HH:mm:ss') || '';
    }

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
        this._fuseSplashScreen.show();

        this.distinctEmails = this.getDistinctDomainsOfAddresses;

        this.handleAttachments(this.data.emailAttachments);

        this.emailFrom = this.data.filteredEmail.email.from;
        // Corrigindo o operador de atribuição para o operador de igualdade
        this.rawEmailHtml = this.data.filteredEmail.email.body;
        // Sanitize the HTML content to make it safe for rendering
        this.sanitizedEmailBody = this.sanitizer.bypassSecurityTrustHtml(
            this.rawEmailHtml
        );

        this.subscribeToChanges();

        // Deal with concurrency when replying
        this._filteringService
            .postStartEmailConcurrency(this.data.filteredEmail.token)
            .subscribe((response: ReplyConcurrency) => {
                this.replyConcurrencyInfo = response;
                if (
                    this.replyConcurrencyInfo &&
                    !this.replyConcurrencyInfo.canReply
                ) {
                    this.replyConcurrencyInfo.replyInfo.date = new Date(
                        this.replyConcurrencyInfo.replyInfo.date
                    ).toLocaleString();
                    if (!this.data.isToChangeCategory) {
                        //this.composeForm.disable();
                    }
                }
            });

        this.panels = [
            {
                id: 'emailPanel',
                icon: 'heroicons_outline:mail',
                title: this.translate('Email.preview'),
            },
            {
                id: 'attachmentPanel',
                icon: 'heroicons_outline:paper-clip',
                title: this.translate('Email.attachments'),
            },
        ];

        // If not to change category, is to reply the email
        if (!this.data.isToChangeCategory) {
            this.panels.push({
                id: 'responsePanel',
                icon: 'heroicons_outline:inbox-in',
                title: this.translate('Email.response'),
            });
        } else {
            this.panels.push({
                id: 'changeCategoryPanel',
                icon: 'heroicons_outline:tag',
                title: this.translate('Email.change-category'),
            });

            // Get the categories
            this._filteringService
                .getCategories()
                .subscribe((categories: Category[]) => {
                    // Get all categories except the actual category of the email editing
                    this.categories = categories.filter(
                        (category) =>
                            category.title !==
                                this.data.filteredEmail.category &&
                            category.title !== 'Erro'
                    );

                    // Mark for check
                    this._changeDetectorRef.markForCheck();

                    // Manually trigger change detection after categories are loaded
                    this._changeDetectorRef.detectChanges();
                });

            this.categoryForm = this._formBuilder.group({
                category: ['', [Validators.required]],
            });
        }

        this._fuseSplashScreen.hide();
    }

    subscribeToChanges(): void {
        this._auditHubWs.joinEmailGroup(this.data.filteredEmail.token);

        this._auditHubWs.messageReceived$.subscribe((message: string) => {
            const payload: EmailStatusUpdateWs = JSON.parse(message);
            this.data.filteredEmail.status = payload.status_description;
        });

        this._auditHubWs.userJoined$.subscribe((data: UserJoinedWs) => {
            if (data.email_token !== this.data.filteredEmail.token) {
                return;
            }

            this.replyConcurrencyInfo = {
                canReply: false,
                replyInfo: {
                    user: data.user_email,
                    date: new Date().toLocaleString(),
                },
                result: 'Sucesso',
                resultCode: 1,
            };
        });

        this._auditHubWs.userExited$.subscribe((data: UserJoinedWs) => {
            if (data.email_token !== this.data.filteredEmail.token) {
                return;
            }

            this.replyConcurrencyInfo = {
                canReply: true,
                replyInfo: {
                    user: '',
                    date: '',
                },
                result: 'Sucesso',
                resultCode: 1,
            };
        });
    }

    // Function to toggle edit mode
    toggleEditFrom(): void {
        // if is editing form and clicks button again, undo the changes
        if (this.isEditingFrom) {
            this.emailFrom = this.data.filteredEmail.email.from;
        }
        this.isEditingFrom = !this.isEditingFrom;
    }

    handleAttachments(attachments: EmailAttachment[]): void {
        this.hasAttachments = attachments.length > 0;

        if (!this.hasAttachments) {
            return;
        }

        // create temporary variables for the pdf attachments
        // in order not to cause a re-render on any change which could cause
        // slow down of the page
        let hasPdfs: boolean = false;
        const pdfList: EmailAttachment[] = [];

        attachments.map((attachment: EmailAttachment) => {
            const attachmentName = attachment.name.toLowerCase().trim();
            if (attachmentName.endsWith('.pdf')) {
                hasPdfs = true;
                pdfList.push(attachment);
            }
        });

        this.isAnyAttachmentPdf = hasPdfs;
        this.pdfAttachments = pdfList;
    }

    // Function to save the edited email
    changeEmailDestinatary(newFrom: string): void {
        if (!newFrom) {
            return;
        }

        // validate email before sending
        const emailRegex = /^[a-zA-Z0-9._%+-]+@[a-zA-Z0-9.-]+\.[a-zA-Z]{2,}$/;

        if (!emailRegex.test(newFrom)) {
            // Invalid email format, show error message
            this.showFlashMessage(
                'error',
                this.translate('email-address-not-valid')
            );
            return;
        }

        // send request to api to change the destinatary
        this._filteringService
            .changeEmailDestinatary(this.data.filteredEmail.email.id, newFrom)
            .subscribe((response) => {
                if (response && response.result_code < 0) {
                    //ERRO
                    this.showFlashMessage(
                        'error',
                        this.translate('sender-error')
                    );

                    this.emailFrom = this.data.filteredEmail.email.from;

                    return;
                }

                this.showFlashMessage(
                    'success',
                    this.translate('sender-success')
                );

                // change field back to original from
                this.data.filteredEmail.email.from = newFrom;
            });

        this.isEditingFrom = false; // Turn off edit mode
    }

    markEmailAsSpam(): void {
        this.isLoading = true;
        this.isMarkingEmailAsSpam = true;
        this._filteringService
            .markEmailAsSpam(this.data.filteredEmail.token)
            .subscribe(
                (response) => {
                    if (response.result_code <= 0) {
                        this.showFlashMessage(
                            'error',
                            this.translate('Email.mark-as-spam-error')
                        );
                        return;
                    }
                    this._flashMessageService.success(
                        'Email.mark-as-spam-success'
                    );
                    this.matDialogRef.close();
                },
                (error) => {
                    this.showFlashMessage(
                        'error',
                        this.translate('Email.mark-as-spam-error')
                    );
                },
                () => {
                    this.isLoading = false;
                    this.isMarkingEmailAsSpam = false;
                }
            );
    }

    changeEmailStatus(): void {
        let statusId = EMAIL_STATUSES.RESOLVIDO_MANUALMENTE.id;
        if (
            this.data.filteredEmail.status !==
            EMAIL_STATUSES.RESOLVIDO_MANUALMENTE.description
        ) {
            statusId = EMAIL_STATUSES.RESOLVIDO_MANUALMENTE.id;
        } else {
            statusId = EMAIL_STATUSES.TRIAGEM_REALIZADA.id;
        }

        this.isLoading = true;
        this._filteringService
            .changeEmailStatus(this.data.filteredEmail.token, statusId)
            .subscribe(
                (response: any) => {
                    if (response.result !== 'Sucesso') {
                        this.showFlashMessage(
                            'error',
                            this.translocoService.translate(
                                'email-status-change-error',
                                {}
                            )
                        );
                        return;
                    }

                    const status = Object.values(EMAIL_STATUSES).find(
                        (s) => s.id === statusId
                    );
                    const newStatusDescription = status
                        ? status.description
                        : 'Unknown';

                    const payload: EmailStatusUpdateWs = {
                        email_token: this.data.filteredEmail.token,
                        status_description: newStatusDescription,
                        status_id: statusId,
                    };
                    this._auditHubWs.sendMessage(JSON.stringify(payload));

                    this._flashMessageService.success(
                        'email-status-change-success'
                    );

                    const statusDescription =
                        this.getStatusDescriptionById(statusId);

                    this.data.filteredEmail.status = statusDescription;
                },
                (error) => {
                    this.showFlashMessage(
                        'error',
                        this.translocoService.translate(
                            'email-status-change-error',
                            {}
                        )
                    );
                },
                () => {
                    this.isLoading = false;
                    this._changeDetectorRef.markForCheck();
                }
            );
    }

    getStatusDescriptionById(statusId: number): string {
        const status = Object.values(EMAIL_STATUSES).find(
            (s) => s.id === statusId
        );
        return status ? status.description : '';
    }

    async printEmail(
        isPrintBody: boolean,
        isPrintAttachments: boolean
    ): Promise<void> {
        if (!isPrintBody && !isPrintAttachments) {
            console.error('Combination not possible');
            return;
        }
        // Create a temporary container to manipulate the HTML
        const tempDiv: HTMLDivElement = document.createElement('div');

        if (isPrintBody) {
            let formattedFrom = this.data.filteredEmail.email.from.replace(
                /</g,
                ' - '
            );
            formattedFrom = formattedFrom.replace(/>/g, '');

            // Insert the email info first, including subject, sender and date with the label
            tempDiv.insertAdjacentHTML(
                'beforeend',
                `<h3><b>${this.translocoService.translate(
                    'Notifications.subject'
                )}: </b>${this.data.filteredEmail.email.subject}</h3>`
            );
            tempDiv.insertAdjacentHTML(
                'beforeend',
                `<p><b>${this.translocoService.translate(
                    'sender'
                )}:</b> ${formattedFrom}</p>`
            );
            tempDiv.insertAdjacentHTML(
                'beforeend',
                `<p><b>${this.translocoService.translate(
                    'email-date'
                )}:</b> ${this.getFormattedDate(
                    this.data.filteredEmail.email.date
                )}</p>`
            );

            // separator
            tempDiv.insertAdjacentHTML('beforeend', '<hr>');
            // Insert the email body to the temp div
            tempDiv.insertAdjacentHTML(
                'beforeend',
                DOMPurify.sanitize(this.data.filteredEmail.email.body)
            );

            // if not to print attachments, print a label and the attachment names
            if (this.data.emailAttachments.length > 0) {
                tempDiv.insertAdjacentHTML(
                    'beforeend',
                    `<h2><b>${this.translate('attachments')}: </b></h2>`
                );
                this.data.emailAttachments.forEach((attachment) => {
                    tempDiv.insertAdjacentHTML(
                        'beforeend',
                        `<h3>${attachment.name}</h3>`
                    );
                });
            }
        }

        if (isPrintAttachments) {
            let attachmentsDivToPrint = '';
            // Render the attachments as images and print
            const attachmentsImagesHtml =
                await this._printingService.renderAttachmentsAsImages(
                    this.data.emailAttachments
                );

            attachmentsDivToPrint += attachmentsImagesHtml;

            tempDiv.insertAdjacentHTML(
                'beforeend',
                DOMPurify.sanitize(attachmentsDivToPrint)
            );
        }

        // Trigger the print
        this._printingService.printDiv(tempDiv.innerHTML);
    }

    /*
        This function handles the case when the user starts typing a response
        But if he wants to go check again the email or the pdf
        This saves the previous response written
    */
    handleResponseWritten(response: string): void {
        this.previousResponseWritten = response;
    }

    togglePdfPanel(): void {
        // Alterna entre a exibição do painel de PDF
        this.showPdfPanel = !this.showPdfPanel;

        // Fecha o drawer no modo 'over'
        if (this.drawerMode === 'over') {
            this.drawer.close();
        }
    }

    goToPanel(panel: string): void {
        this.selectedPanel = panel;

        // Close the drawer on 'over' mode
        if (this.drawerMode === 'over') {
            this.drawer.close();
        }
    }

    getPanelInfo(id: string): any {
        return this.panels.find((panel) => panel.id === id);
    }

    close(): void {
        // Close the dialog
        this.matDialogRef.close();
    }

    verifyCategorizeForm(): void {
        if (!this.categoryForm.valid) {
            return;
        }

        const categoryId = this.categoryForm.get('category')?.value;

        // Close the dialog, sends the response to the higher component
        this.matDialogRef.close({
            categoryId: categoryId,
        });
    }

    saveAndClose(): void {
        this.matDialogRef.close();
    }

    ngOnDestroy(): void {
        this.removeUserFromReplyConcurrency(this.data.filteredEmail.token);
    }

    removeUserFromReplyConcurrency(emailToken: string): void {
        this._auditHubWs.leaveEmailGroup(emailToken);

        this._filteringService.postStopEmailConcurrency(emailToken).subscribe(
            (response: any) => {
                if (response.result !== 'Sucesso') {
                    this.showFlashMessage(
                        'error',
                        this.translocoService.translate('concurrency-error', {})
                    );
                    return;
                }
            },
            (error: any) => {
                this.showFlashMessage(
                    'error',
                    this.translocoService.translate('concurrency-error', {})
                );
                return;
            }
        );
    }

    handleResponseSent(response: string): void {}

    downloadFile(attach: EmailAttachment, event: MouseEvent): void {
        event.preventDefault();

        const byteCharacters = atob(attach.file); // Decode base64
        const byteNumbers = new Array(byteCharacters.length);
        for (let i = 0; i < byteCharacters.length; i++) {
            byteNumbers[i] = byteCharacters.charCodeAt(i);
        }
        const byteArray = new Uint8Array(byteNumbers);
        const blob = new Blob([byteArray], {
            type: 'application/octet-stream',
        });
        const url = window.URL.createObjectURL(blob);

        // Create a link element, set its download attribute and click it
        const a = document.createElement('a');
        a.href = url;
        a.download = attach.name; // Set the file name
        document.body.appendChild(a);
        a.click();

        // Clean up by revoking the object URL and removing the link element
        window.URL.revokeObjectURL(url);
        document.body.removeChild(a);
    }

    getFormattedEmails(field: 'cc' | 'bcc' | 'from'): string {
        const emailsField = this.data.filteredEmail.email[field]; // Access the cc or bcc field
        if (!emailsField) {
            return '';
        }

        // check if email has < and >, if not, check if email is valid
        const emailsList = emailsField.split(',').map((email) => email.trim());

        // Extract emails that have < and > (e.g. "Name" <email>)
        const emailsWithBrackets = emailsField.match(/<([^>]+)>/g);

        // If there are emails inside < >, extract them
        let emailList = [];
        if (emailsWithBrackets) {
            emailList = emailsWithBrackets.map((email) =>
                email.replace(/[<>]/g, '').trim()
            );
        }

        // Check if there are emails without < > and validate them
        const validEmailPattern =
            /^[a-zA-Z0-9._%+-]+@[a-zA-Z0-9.-]+\.[a-zA-Z]{2,}$/;
        emailsList.forEach((email) => {
            if (!emailList.includes(email) && validEmailPattern.test(email)) {
                emailList.push(email); // Add valid emails without < > to the list
            }
        });

        return emailList.join(', ');
    }

    get getDistinctDomainsOfAddresses(): string[] {
        // Get the emails from the email fields
        const fromEmails = this.getFormattedEmails('from').split(', ');
        const ccEmails = this.getFormattedEmails('cc').split(', ');
        const bccEmails = this.getFormattedEmails('bcc').split(', ');

        // Get the domains of the emails and filter out any invalid domains
        const fromDomains = fromEmails
            .map((email) => email.split('@')[1])
            .filter(Boolean);
        const ccDomains = ccEmails
            .map((email) => email.split('@')[1])
            .filter(Boolean);
        const bccDomains = bccEmails
            .map((email) => email.split('@')[1])
            .filter(Boolean);

        // Concatenate all the domains
        const allDomains = fromDomains.concat(ccDomains, bccDomains);

        // Remove duplicates using Set and convert it back to an array
        const distinctDomains = Array.from(new Set(allDomains));

        return distinctDomains;
    }

    get getDistinctEmailsLength(): number {
        return this.distinctEmails.length;
    }

    get getFormattedDistinctEmails(): string {
        return this.distinctEmails.join(', ');
    }
}
