import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';
import { AttachmentDetailsComponent } from './attachment-details.component';
import { MatIconModule } from '@angular/material/icon';
import { TranslocoModule } from '@ngneat/transloco';
import { PdfViewerModule } from 'ng2-pdf-viewer';

@NgModule({
    declarations: [AttachmentDetailsComponent],
    imports: [CommonModule, MatIconModule, TranslocoModule, PdfViewerModule],
    exports: [AttachmentDetailsComponent],
})
export class AttachmentDetailsModule {}
