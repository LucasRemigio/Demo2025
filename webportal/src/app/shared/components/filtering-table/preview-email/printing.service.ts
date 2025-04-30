import { TranslocoService } from '@ngneat/transloco';
import * as pdfjsLib from 'pdfjs-dist/build/pdf';
import 'pdfjs-dist/build/pdf.worker.entry';
import { Injectable } from '@angular/core';
import {
    EmailAttachment,
    ReplyAttachment,
} from 'app/modules/filtering/filtering.types';

@Injectable({
    providedIn: 'root',
})
export class PrintingService {
    constructor(private translocoService: TranslocoService) {}

    translate(key: string, params?: object): string {
        return this.translocoService.translate(key, params || {});
    }

    // Function to load the PDF and render its pages as images
    renderAttachmentsAsImages = async (
        attachments: EmailAttachment[] | ReplyAttachment[]
    ): Promise<string> => {
        let attachmentsHtml = '';

        // Retreat immediatly if no attachments are available
        if (attachments.length === 0) {
            return attachmentsHtml;
        }

        // Loop through attachments and render each one
        for (const attachment of attachments) {
            // Check the file type by extension or MIME type
            if (this.isImage(attachment.name)) {
                const imageHtml = this.renderImageAttachment(attachment.file);
                attachmentsHtml += imageHtml;
            } else if (this.isPdf(attachment.name)) {
                const pdfImageHtml = await this.renderPdfPagesAsImages(
                    attachment.file
                );
                attachmentsHtml += pdfImageHtml;
            }
        }

        return attachmentsHtml;
    };

    // Render each page of the pdf file given as a separate image
    async renderPdfPagesAsImages(base64Pdf: string): Promise<string> {
        const decodedPdfData = atob(base64Pdf);
        const pdfDocument = await pdfjsLib.getDocument({ data: decodedPdfData })
            .promise;
        let renderedPagesHtml = '';

        // Loop through each page and render it
        for (
            let pageIndex = 1;
            pageIndex <= pdfDocument.numPages;
            pageIndex++
        ) {
            const pdfPage = await pdfDocument.getPage(pageIndex);

            // Canvas settings
            const renderScale = 3;
            const viewport = pdfPage.getViewport({ scale: renderScale });
            const canvas = document.createElement('canvas');
            const context = canvas.getContext('2d');
            canvas.height = viewport.height;
            canvas.width = viewport.width;

            const renderContext = {
                canvasContext: context,
                viewport: viewport,
            };

            await pdfPage.render(renderContext).promise;

            const imageDataUrl = canvas.toDataURL('image/png');

            renderedPagesHtml += `
                <div class="pdf-page-container" style="width: 100%; height:100%">
                    <img src="${imageDataUrl}" style="width: 100%; height: 100%;" />
                </div>
            `;
        }

        return renderedPagesHtml;
    }

    // Helper function to check if a file is an image (based on file extension)
    isImage(fileName: string): boolean {
        const imageExtensions = ['jpg', 'jpeg', 'png', 'gif', 'bmp'];
        const extension = fileName.split('.').pop()?.toLowerCase();
        return imageExtensions.includes(extension);
    }

    // Helper function to check if a file is a PDF
    isPdf(fileName: string): boolean {
        return fileName.toLowerCase().endsWith('.pdf');
    }

    // Function to render an image attachment as an <img> tag
    renderImageAttachment(base64Image: string): string {
        const imageDataUrl = `data:image/png;base64,${base64Image}`;
        return `
        <div class="image-container" style="width: 100%; margin-bottom: 16px;">
            <img src="${imageDataUrl}" style="width: 100%; height: auto;" />
        </div>
    `;
    }

    printDiv(contentHtml: string): void {
        const printWindow = window.open(
            '',
            '_blank',
            `width=${window.innerWidth},height=${window.innerHeight}`
        );

        printWindow.document.write(`<html>
            <head>
                <title>${document.title}</title>
                <link href="https://cdn.jsdelivr.net/npm/tailwindcss@2.2.19/dist/tailwind.min.css" rel="stylesheet">
                <style>
                    @media print {
                        /* Hide HTML title and footer elements, if any */
                        .no-print { display: none; }

                        /* Suppress margins that may be interpreted as headers/footers */
                        @page {
                            margin: 58px;
                        }

                        body {
                            padding: 20px; /* Add internal padding for safety */
                        }

                    }

                    ul {
                        list-style-type: disc !important;
                        padding-left: 20px; /* Adjust for desired indentation */
                    }
                    li.MsoListParagraph {
                        margin-left: 0 !important;
                        list-style: initial;
                    }
                </style>
            </head>`);

        printWindow.document.write('</head><body>');
        printWindow.document.write(contentHtml);
        printWindow.document.write('</body></html>');

        printWindow.document.close();

        // Wait for the new window to fully load before invoking print
        printWindow.onload = (): void => {
            printWindow.focus();
            printWindow.print();
        };

        printWindow.onafterprint = (): void => {
            printWindow.close();
        };
    }
}
