import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';
import { ShowEmailPopupComponent } from './show-email-popup.component';
import { MatButtonModule } from '@angular/material/button';
import { MatDialogModule } from '@angular/material/dialog';
import { MatFormFieldModule } from '@angular/material/form-field';
import { MatIconModule } from '@angular/material/icon';
import { MatTabsModule } from '@angular/material/tabs';
import { FilteringValidateModule } from '../filtering-validate/filtering-validate.module';
import { MatProgressBarModule } from '@angular/material/progress-bar';
import { MatInputModule } from '@angular/material/input';
import { AttachmentDetailsComponent } from '../filtering-validate/details/attachment-details/attachment-details.component';
import { EmailDetailsComponent } from '../filtering-validate/details/email-details/email-details.component';
import { EmailDetailsModule } from '../filtering-validate/details/email-details/email-details.module';
import { AttachmentDetailsModule } from '../filtering-validate/details/attachment-details/attachment-details.module';
import { TranslocoModule } from '@ngneat/transloco';

@NgModule({
    declarations: [ShowEmailPopupComponent],
    imports: [
        CommonModule,
        TranslocoModule,
        MatDialogModule,
        MatButtonModule,
        MatIconModule,
        MatTabsModule,
        MatFormFieldModule,
        MatInputModule,
        MatProgressBarModule,
        EmailDetailsModule,
        AttachmentDetailsModule,
    ],
    exports: [ShowEmailPopupComponent],
})
export class ShowEmailPopupModule {}
