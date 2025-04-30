import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';
import { TranslocoModule } from '@ngneat/transloco';
import { MatFormFieldModule } from '@angular/material/form-field';
import { MatButtonModule } from '@angular/material/button';
import { MatIconModule } from '@angular/material/icon';
import { MatInputModule } from '@angular/material/input';
import { SharedModule } from 'app/shared/shared.module';
import { MatProgressBarModule } from '@angular/material/progress-bar';
import { MatTooltipModule } from '@angular/material/tooltip';
import { MatSelectModule } from '@angular/material/select';
import { LanguagesModule } from 'app/layout/common/languages/languages.module';
import { MatSidenavModule } from '@angular/material/sidenav';
import { MatSlideToggleModule } from '@angular/material/slide-toggle';
import { FuseFindByKeyPipeModule } from '@fuse/pipes/find-by-key';
import { MatDatepickerModule } from '@angular/material/datepicker';
import { MatTabsModule } from '@angular/material/tabs';
import { ChartsModule } from '@progress/kendo-angular-charts';
import {
    DropDownListModule,
    DropDownsModule,
} from '@progress/kendo-angular-dropdowns';
import { GridModule } from '@progress/kendo-angular-grid';
import { RecaptchaModule } from 'ng-recaptcha';
import { PdfViewerModule } from 'ng2-pdf-viewer';
import { MatBadgeModule } from '@angular/material/badge';
import { MatProgressSpinnerModule } from '@angular/material/progress-spinner';
import { FilteringTableComponent } from './filtering-table.component';
import { PreviewEmailComponent } from './preview-email/preview-email.component';
import { PreviewRepliesComponent } from './preview-replies/preview-replies.component';
import { EmailReplyFormComponent } from '../email-reply-form/email-reply-form.component';
import { MatMenuModule } from '@angular/material/menu';
import { EmailProductTableComponent } from '../email-product-table/email-product-table.component';
import { FwdEmailComponent } from './fwd-email/fwd-email.component';
import {
    MatAutocomplete,
    MatAutocompleteModule,
} from '@angular/material/autocomplete';
import { ConfirmOrderAddressModule } from '../confirm-order-address/confirm-order-address.module';
import { MatCheckboxModule } from '@angular/material/checkbox';
import { FuseAlertModule } from '@fuse/components/alert';
import { EmailFwdListComponent } from './fwd-email/email-fwd-list/email-fwd-list.component';
import { EmailReplyFormModule } from '../email-reply-form/email-reply-form.module';

@NgModule({
    declarations: [
        FilteringTableComponent,
        PreviewEmailComponent,
        PreviewRepliesComponent,
        FwdEmailComponent,
        EmailFwdListComponent,
    ],
    imports: [
        EmailReplyFormModule,
        MatButtonModule,
        MatFormFieldModule,
        MatIconModule,
        MatInputModule,
        MatProgressBarModule,
        MatProgressSpinnerModule,
        MatSelectModule,
        MatSidenavModule,
        MatCheckboxModule,
        MatTooltipModule,
        FuseFindByKeyPipeModule,
        MatTabsModule,
        MatDatepickerModule,
        GridModule,
        TranslocoModule,
        PdfViewerModule,
        ChartsModule,
        RecaptchaModule,
        DropDownsModule,
        CommonModule,
        MatBadgeModule,
        MatMenuModule,
        SharedModule,
        DropDownsModule,
        DropDownListModule,
        MatAutocompleteModule,
        ConfirmOrderAddressModule,
        FuseAlertModule,
    ],
    exports: [FilteringTableComponent],
})
export class FilteringTableModule {}
