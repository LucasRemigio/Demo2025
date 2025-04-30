import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';
import { ReactiveFormsModule } from '@angular/forms';
import { MatButtonModule } from '@angular/material/button';
import { MatFormFieldModule } from '@angular/material/form-field';
import { MatIconModule } from '@angular/material/icon';
import { MatInputModule } from '@angular/material/input';
import { MatProgressSpinnerModule } from '@angular/material/progress-spinner';
import { TranslocoModule } from '@ngneat/transloco';
import { ViewClientPrimaveraInvoicesModule } from 'app/modules/configurations/clients/ratings-table/view-client-primavera-invoices/view-client-primavera-invoices.module';
import { ViewClientPrimaveraOrdersModule } from 'app/modules/configurations/clients/ratings-table/view-client-primavera-orders/view-client-primavera-orders.module';
import { RatingCardModule } from 'app/shared/components/rating-card/rating-card.module';
import { ClientDetailsComponent } from './client-details.component';
import { ChangeOrderClientComponent } from './change-order-client/change-order-client.component';
import { MatAutocompleteModule } from '@angular/material/autocomplete';

@NgModule({
    declarations: [ClientDetailsComponent, ChangeOrderClientComponent],
    imports: [
        // Angular modules
        CommonModule,
        ReactiveFormsModule,
        TranslocoModule,

        // Material modules
        MatButtonModule,
        MatFormFieldModule,
        MatIconModule,
        MatInputModule,
        MatProgressSpinnerModule,
        MatAutocompleteModule,

        // App modules
        RatingCardModule,
    ],
    exports: [ClientDetailsComponent, ChangeOrderClientComponent],
})
export class ClientDetailsModule {}
