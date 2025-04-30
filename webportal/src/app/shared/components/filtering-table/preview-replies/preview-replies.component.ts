/* eslint-disable arrow-parens */
import {
    Component,
    OnInit,
    Inject,
    ChangeDetectorRef,
    HostListener,
    OnDestroy,
} from '@angular/core';
import { MatDialogRef, MAT_DIALOG_DATA } from '@angular/material/dialog';
import { TranslocoService } from '@ngneat/transloco';
import { FuseSplashScreenService } from '@fuse/services/splash-screen';
import { FormBuilder } from '@angular/forms';
import { FilteringService } from 'app/modules/filtering/filtering.service';
import {
    FilteredEmailWithAttachmentsResponse,
    ReplyConcurrency,
    ReplyData,
} from 'app/modules/filtering/filtering.types';
import { DomSanitizer } from '@angular/platform-browser';
import { PrintingService } from '../preview-email/printing.service';
import DOMPurify from 'dompurify';
import { DatePipe } from '@angular/common';

interface Attachment {
    id: string;
    email: string;
    name: string;
    size: string;
    file: string;
}

@Component({
    selector: 'app-preview-replies',
    templateUrl: './preview-replies.component.html',
    styleUrls: ['./preview-replies.component.scss'],
    providers: [DatePipe],
})
export class PreviewRepliesComponent implements OnInit, OnDestroy {
    originalMessage: string = '';
    subject: string = '';

    originalEmail: FilteredEmailWithAttachmentsResponse;
    hasAnyAttachments: boolean = false;

    flashMessage: 'success' | 'error' | null = null;
    flashMessageText: string;

    replies: ReplyData[];
    reloadReplies: any;

    replyConcurrencyInfo: ReplyConcurrency;

    // Track loading state per reply
    updatingReplyState: { [key: string]: boolean } = {};
    showReplyForm: { [key: string]: boolean } = {};

    constructor(
        public dialogRef: MatDialogRef<PreviewRepliesComponent>,
        public matDialogRef: MatDialogRef<PreviewRepliesComponent>,
        private _changeDetectorRef: ChangeDetectorRef,
        private translocoService: TranslocoService,
        private _filteringService: FilteringService,
        private _fuseSplashScreen: FuseSplashScreenService,
        private _formBuilder: FormBuilder,
        private sanitizer: DomSanitizer,
        private _printingService: PrintingService,
        private datePipe: DatePipe,

        @Inject(MAT_DIALOG_DATA) public data: any
    ) {}

    @HostListener('window:beforeunload', ['$event'])
    handleBeforeUnload(event: Event): void {
        // Perform your cleanup here before the page is refreshed
        this.removeUserFromReplyConcurrency(this.data.emailToken);
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

    async ngOnInit(): Promise<void> {
        this._fuseSplashScreen.show();

        this.addUserToReplyConcurrency(this.data.emailToken);

        await this.getReplies();

        this._fuseSplashScreen.hide();

        this.subject = this.replies[0].subject.replace(/^Re:\s*/i, '');
        // Cut the original message from all replies
        this.replies.forEach((reply) => {
            reply.body = reply.body.split(
                '------------- Original Message -------------'
            )[0];
            reply.body = reply.body.split(
                '<div style="border:none;border-top:solid #E1E1E1 1.0pt;padding:3.0pt 0cm 0cm 0cm">'
            )[0];

            const rawEmailHtml = reply.body;
            // Sanitize the HTML content to make it safe for rendering
            reply.sanitizedBody =
                this.sanitizer.bypassSecurityTrustHtml(rawEmailHtml);
        });

        this._filteringService
            .getFilteredEmailByToken(this.replies[0].email_token)
            .subscribe(
                async (response: FilteredEmailWithAttachmentsResponse) => {
                    this.originalEmail = response;
                    this.updateHasAttachments();
                }
            );
    }

    ngOnDestroy(): void {
        this.removeUserFromReplyConcurrency(this.data.emailToken);
    }

    close(): void {
        // Close the dialog
        this.matDialogRef.close();
    }

    setReplyToRead(replyToken: string): void {
        this.updatingReplyState[replyToken] = true;
        this._filteringService.setReplyToRead(replyToken).subscribe(
            (response: any) => {
                if (response.result !== 'Sucesso') {
                    this.updatingReplyState[replyToken] = false;
                    this.showFlashMessage(
                        'error',
                        this.translocoService.translate(
                            'Email.set-read-error',
                            {}
                        )
                    );
                    return;
                }
                this.replies.find(
                    (reply) => reply.reply_token === replyToken
                ).is_read = 'true';
                this.updatingReplyState[replyToken] = false;
            },
            (error: any) => {
                this.updatingReplyState[replyToken] = false;
                this.showFlashMessage(
                    'error',
                    this.translocoService.translate('Email.set-read-error', {})
                );
                return;
            }
        );
    }

    toggleReplyForm(replyToken: string): void {
        this.showReplyForm[replyToken] = !this.showReplyForm[replyToken];
    }

    async getReplies(): Promise<ReplyData[]> {
        const emailToken = this.data.emailToken;
        // Return a promise that resolves with the replies
        return new Promise((resolve, reject) => {
            this._filteringService.getEmailReplies(emailToken).subscribe(
                (response: any) => {
                    if (response && response.replies) {
                        resolve(response.replies);
                        this.replies = response.replies;
                    } else {
                        reject('No replies found');
                    }
                },
                (error: any) => {
                    reject(error);
                }
            );
        });
    }

    downloadFile(attach: Attachment, event: MouseEvent): void {
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

    addUserToReplyConcurrency(emailToken: string): void {
        // Deal with concurrency when replying
        this._filteringService
            .postStartEmailConcurrency(emailToken)
            .subscribe((response: ReplyConcurrency) => {
                this.replyConcurrencyInfo = response;
                if (
                    this.replyConcurrencyInfo &&
                    !this.replyConcurrencyInfo.canReply
                ) {
                    this.replyConcurrencyInfo.replyInfo.date = new Date(
                        this.replyConcurrencyInfo.replyInfo.date
                    ).toLocaleString();
                }
            });
    }

    removeUserFromReplyConcurrency(emailToken: string): void {
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

    async printThread(
        isPrintBody: boolean,
        isPrintAttachments: boolean
    ): Promise<void> {
        let textToPrint: string = '';

        // Evoke all queries at the same time, only return when all done
        const repliesTextArray = await Promise.all(
            this.replies.map(async (reply) => {
                const response = await this.printEmail(
                    reply.reply_token,
                    isPrintBody,
                    isPrintAttachments,
                    true
                );
                return response;
            })
        );

        // Combine the results into one string
        textToPrint = repliesTextArray.join('');

        // Print the original email
        if (isPrintBody) {
            // print details of the original email, subject, date, from
            textToPrint += `
                <h3>${this.translocoService.translate(
                    'Notifications.subject'
                )}: ${this.originalEmail.filteredEmail.email.subject}</h3>
                <p>${this.translocoService.translate('sender')}: ${
                this.originalEmail.filteredEmail.email.from
            }</p>
                <p>${this.translocoService.translate(
                    'email-date'
                )}: ${this.getFormattedDate(
                this.originalEmail.filteredEmail.email.date
            )}</p>
            `;
            textToPrint += this.originalEmail.filteredEmail.email.body;

            // if not to print attachments, print a label and the attachment names
            if (
                !isPrintAttachments &&
                this.originalEmail.emailAttachments.length > 0
            ) {
                textToPrint += `<h2><b>${this.translate(
                    'attachments'
                )}: </b></h2>`;
                this.originalEmail.emailAttachments.forEach((attachment) => {
                    textToPrint += `<h3>${attachment.name}</h3>`;
                });
            }
        }
        if (isPrintAttachments) {
            textToPrint +=
                await this._printingService.renderAttachmentsAsImages(
                    this.originalEmail.emailAttachments
                );
        }

        this._printingService.printDiv(textToPrint);
    }

    async printEmail(
        replyToken: string,
        isPrintBody: boolean,
        isPrintAttachments: boolean,
        isToIncrement: boolean = false
    ): Promise<string | void> {
        if (!isPrintBody && !isPrintAttachments) {
            console.error('Combination not possible');
            return;
        }

        const tempDiv: HTMLDivElement = document.createElement('div');
        const reply = this.replies.find(
            (item) => item.reply_token === replyToken
        );

        if (isPrintBody) {
            // print the email reply info, including subject, sender and date
            const replyInfoDiv = document.createElement('div');
            replyInfoDiv.innerHTML = `
            <h3><b>${this.translocoService.translate(
                'Notifications.subject'
            )}:</b> ${reply.subject}</h3>
            <p><b>${this.translocoService.translate('sender')}:</b> ${
                reply.from
            }</p>
            <p><b>${this.translocoService.translate(
                'email-date'
            )}:</b> ${this.getFormattedDate(reply.date)}</p>
        `;

            tempDiv.appendChild(replyInfoDiv);

            // get the reply email body by the reply token and put it on the temp div
            tempDiv.insertAdjacentHTML('beforeend', reply.body);

            // if not to print attachments, print a label and the attachment names
            if (reply.attachments.length > 0) {
                tempDiv.insertAdjacentHTML(
                    'beforeend',
                    `<h2><b>${this.translate('attachments')}: </b></h2>`
                );
                reply.attachments.forEach((attachment) => {
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
                    // Search for the reply attachments by reply token
                    reply.attachments
                );

            attachmentsDivToPrint += attachmentsImagesHtml;

            tempDiv.insertAdjacentHTML(
                'beforeend',
                DOMPurify.sanitize(attachmentsDivToPrint)
            );
        }

        if (isToIncrement) {
            if (!isPrintBody && isPrintAttachments) {
                return tempDiv.innerHTML;
            }
            tempDiv.insertAdjacentHTML('beforeend', '<hr/><br/>');
            return tempDiv.innerHTML;
        }

        // Trigger the print
        this._printingService.printDiv(tempDiv.innerHTML);
    }

    updateHasAttachments(): void {
        const originalHasAttachments =
            this.originalEmail &&
            this.originalEmail.emailAttachments &&
            this.originalEmail.emailAttachments.length > 0;

        const repliesHaveAttachments =
            this.replies &&
            this.replies.some(
                (reply) => reply.attachments && reply.attachments.length > 0
            );

        // Return true if there are attachments in either the original email or any of the replies
        this.hasAnyAttachments =
            originalHasAttachments || repliesHaveAttachments;
    }

    handleResponseRefresh(response: string): void {
        this.replies.find((reply) => reply.id == Number(response)).is_read =
            'true';
    }
}
