import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';
import { Route, RouterModule } from '@angular/router';
import { ClientOrderConfirmationComponent } from './client-order-confirmation.component';
import { TranslocoModule } from '@ngneat/transloco';
import { LanguagesModule } from '../../layout/common/languages/languages.module';
import { SuccessComponent } from './success/success.component';
import { MatIconModule } from '@angular/material/icon';
import { ConfirmOrderAddressModule } from 'app/shared/components/confirm-order-address/confirm-order-address.module';
import { EmailProductTableModule } from 'app/shared/components/email-product-table/email-product-table.module';
import { MatProgressSpinnerModule } from '@angular/material/progress-spinner';
import { NoOrdersPageComponent } from './no-orders-page/no-orders-page.component';
import { MatButtonModule } from '@angular/material/button';
import { MatTabsModule } from '@angular/material/tabs';
import { StepsListModule } from 'app/shared/components/steps-list/steps-list.module';
import { FlashMessageModule } from 'app/shared/components/flash-message/flash-message.module';
import { FlashMessageService } from 'app/shared/components/flash-message/flash-message.service';
import { MatSidenavModule } from '@angular/material/sidenav';
import { MAT_RIPPLE_GLOBAL_OPTIONS } from '@angular/material/core';
import { MatProgressBarModule } from '@angular/material/progress-bar';
import { ScrollingModule } from '@angular/cdk/scrolling';
import { FilteringValidateModule } from 'app/shared/components/filtering-validate/filtering-validate.module';

const ROUTES: Route[] = [
    {
        path: ':token/success',
        component: SuccessComponent,
    },
    {
        path: ':token',
        component: ClientOrderConfirmationComponent,
    },
];

@NgModule({
    declarations: [
        ClientOrderConfirmationComponent,
        SuccessComponent,
        NoOrdersPageComponent,
    ],
    imports: [
        CommonModule,
        RouterModule.forChild(ROUTES),
        LanguagesModule,
        TranslocoModule,
        MatIconModule,
        ConfirmOrderAddressModule,
        EmailProductTableModule,
        MatProgressSpinnerModule,
        MatProgressBarModule,
        MatButtonModule,
        MatTabsModule,
        StepsListModule,
        FlashMessageModule,
        MatSidenavModule,
        ScrollingModule,
        FilteringValidateModule,
    ],
})
export class ClientOrderConfirmationModule {}
