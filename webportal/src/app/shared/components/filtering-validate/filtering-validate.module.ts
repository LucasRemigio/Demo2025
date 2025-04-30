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
import { MatSidenavModule } from '@angular/material/sidenav';
import { MatSlideToggleModule } from '@angular/material/slide-toggle';
import { FuseFindByKeyPipeModule } from '@fuse/pipes/find-by-key';
import { MatDatepickerModule } from '@angular/material/datepicker';
import { MatTabsModule } from '@angular/material/tabs';
import { ChartsModule } from '@progress/kendo-angular-charts';
import { DropDownsModule } from '@progress/kendo-angular-dropdowns';
import { GridModule } from '@progress/kendo-angular-grid';
import { RecaptchaModule } from 'ng-recaptcha';
import { PdfViewerModule } from 'ng2-pdf-viewer';
import { MatBadgeModule } from '@angular/material/badge';
import { MatProgressSpinnerModule } from '@angular/material/progress-spinner';
import { MatMenuModule } from '@angular/material/menu';
import { FilteringValidateComponent } from './filtering-validate.component';
import { ValidateDetailsComponent } from './details/details.component';
import { RouterModule } from '@angular/router';
import { FilteringTableModule } from '../filtering-table/filtering-table.module';
import { ValidateConfirmationComponent } from './confirmation/confirmation.component';
import { EmailProductTableModule } from '../email-product-table/email-product-table.module';
import { ConfirmOrderAddressModule } from '../confirm-order-address/confirm-order-address.module';
import { SelectCancelReasonComponent } from './select-cancel-reason/select-cancel-reason.component';
import { GenericConfirmationPopupComponent } from '../generic-confirmation-popup/generic-confirmation-popup.component';
import { MatAutocompleteModule } from '@angular/material/autocomplete';
import { MatCheckboxModule } from '@angular/material/checkbox';
import { FuseAlertModule } from '@fuse/components/alert';
import { OrderValidationComponent } from './details/order-validation/order-validation.component';
import { ChangeEmailCategoryComponent } from './details/change-email-category/change-email-category.component';
import { AddressDetailsComponent } from './details/address-details/address-details.component';
import { RatingCardModule } from '../rating-card/rating-card.module';
import { ProposeNewOrderRatingsComponent } from './details/order-validation/propose-new-order-ratings/propose-new-order-ratings.component';
import { ReplyProposedRatingsComponent } from './details/order-validation/reply-proposed-ratings/reply-proposed-ratings.component';
import { PreviewOrderDocumentsComponent } from './details/preview-order-documents/preview-order-documents.component';
import { QuillModule } from 'ngx-quill';
import { EmailDetailsModule } from './details/email-details/email-details.module';
import { AttachmentDetailsModule } from './details/attachment-details/attachment-details.module';
import { ShowEmailPopupModule } from '../show-email-popup/show-email-popup.module';
import { ApproveRatingCardComponent } from './details/order-validation/reply-proposed-ratings/approve-rating-card/approve-rating-card.component';
import { ValidateCardsModule } from './validate-cards/validate-cards.module';
import { ValidateTableModule } from './validate-table/validate-table.module';
import { CreditOrderValidationComponent } from './details/credit-order-validation/credit-order-validation.component';
import { CreditOrderValidationModule } from './details/credit-order-validation/credit-order-validation.module';
import { ClientDetailsModule } from './details/client-details/client-details.module';
import { EmailFormComponent } from 'app/modules/communications/compose/email-form/email-form.component';
import { EmailFormModule } from 'app/modules/communications/compose/email-form/email-form.module';
import { StepsListModule } from '../steps-list/steps-list.module';
import { ClientDetailsComponent } from './details/client-details/client-details.component';

@NgModule({
    declarations: [
        FilteringValidateComponent,
        ValidateDetailsComponent,
        ValidateConfirmationComponent,
        SelectCancelReasonComponent,
        GenericConfirmationPopupComponent,
        OrderValidationComponent,
        ChangeEmailCategoryComponent,
        AddressDetailsComponent,
        ProposeNewOrderRatingsComponent,
        ReplyProposedRatingsComponent,
        PreviewOrderDocumentsComponent,
        ApproveRatingCardComponent,
    ],
    imports: [
        RouterModule,
        MatButtonModule,
        MatFormFieldModule,
        MatIconModule,
        MatInputModule,
        MatProgressBarModule,
        MatProgressSpinnerModule,
        MatSelectModule,
        MatSidenavModule,
        MatSlideToggleModule,
        MatTooltipModule,
        FuseFindByKeyPipeModule,
        MatTabsModule,
        MatAutocompleteModule,
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
        FilteringTableModule,
        EmailProductTableModule,
        ConfirmOrderAddressModule,
        MatCheckboxModule,
        FuseAlertModule,
        RatingCardModule,
        QuillModule.forRoot(),
        EmailDetailsModule,
        AttachmentDetailsModule,
        ShowEmailPopupModule,
        MatSlideToggleModule,
        ValidateCardsModule,
        ValidateTableModule,
        CreditOrderValidationModule,
        ClientDetailsModule,
        StepsListModule,
        EmailFormModule,
    ],
    exports: [
        FilteringValidateComponent,
        ValidateDetailsComponent,
        ChangeEmailCategoryComponent,
        AddressDetailsComponent,
        ClientDetailsComponent,
        OrderValidationComponent,
    ],
})
export class FilteringValidateModule {}
