import { Component, Input, OnInit } from '@angular/core';
import { EmailAttachment } from 'app/modules/filtering/filtering.types';

@Component({
    selector: 'app-attachment-details',
    templateUrl: './attachment-details.component.html',
    styleUrls: ['./attachment-details.component.scss'],
})
export class AttachmentDetailsComponent implements OnInit {
    @Input() attachments: EmailAttachment[] = [];
    @Input() showTitle: boolean = true;

    hasAttachments: boolean = false;
    isAnyAttachmentPdf: boolean = false;
    pdfAttachments: EmailAttachment[] = [];

    constructor() {}

    ngOnInit(): void {
        this.handleAttachments(this.attachments);
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
}
