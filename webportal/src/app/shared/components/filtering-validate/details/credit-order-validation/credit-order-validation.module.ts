import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';
import { ScrollingModule } from '@angular/cdk/scrolling';
import { MatButtonModule } from '@angular/material/button';
import { MatIconModule } from '@angular/material/icon';
import { MatSidenavModule } from '@angular/material/sidenav';
import { MatTabsModule } from '@angular/material/tabs';
import { RouterModule } from '@angular/router';
import { TranslocoModule } from '@ngneat/transloco';
import { CreditOrderValidationComponent } from './credit-order-validation.component';
import { ClientDetailsModule } from '../client-details/client-details.module';
import { ViewClientPrimaveraInvoicesModule } from 'app/modules/configurations/clients/ratings-table/view-client-primavera-invoices/view-client-primavera-invoices.module';
import { ViewClientPrimaveraOrdersModule } from 'app/modules/configurations/clients/ratings-table/view-client-primavera-orders/view-client-primavera-orders.module';
import { ShowOrderTotalModule } from 'app/shared/components/email-product-table/show-order-total/show-order-total.module';
import { MatProgressBarModule } from '@angular/material/progress-bar';
import { MatTooltipModule } from '@angular/material/tooltip';
import { BrowserAnimationsModule } from '@angular/platform-browser/animations';

@NgModule({
    declarations: [CreditOrderValidationComponent],
    imports: [
        // Angular Modules
        CommonModule,
        RouterModule,
        ScrollingModule,
        TranslocoModule,

        // Material modules
        MatButtonModule,
        MatIconModule,
        MatSidenavModule,
        MatTabsModule,
        MatProgressBarModule,
        MatTooltipModule,

        // App modules
        ClientDetailsModule,
        ViewClientPrimaveraOrdersModule,
        ViewClientPrimaveraInvoicesModule,
        ShowOrderTotalModule,
    ],
    exports: [CreditOrderValidationComponent],
})
export class CreditOrderValidationModule {}
