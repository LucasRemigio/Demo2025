import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';
import { ClientsComponent } from './clients.component';
import { ReactiveFormsModule } from '@angular/forms';
import { MatButtonModule } from '@angular/material/button';
import { MatDialogModule } from '@angular/material/dialog';
import { MatFormFieldModule } from '@angular/material/form-field';
import { MatIconModule } from '@angular/material/icon';
import { MatInputModule } from '@angular/material/input';
import { MatTooltipModule } from '@angular/material/tooltip';
import { TranslocoModule } from '@ngneat/transloco';
import { GridModule } from '@progress/kendo-angular-grid';
import { MatTabsModule } from '@angular/material/tabs';
import { RatingsTableComponent } from './ratings-table/ratings-table.component';
import { MatSelectModule } from '@angular/material/select';
import { MatProgressSpinnerModule } from '@angular/material/progress-spinner';
import { SegmentsTableComponent } from './segments-table/segments-table.component';
import { ChangeClientSegmentComponent } from './segments-table/change-client-segment/change-client-segment.component';
import { EditClientRatingComponent } from './ratings-table/edit-client-rating/edit-client-rating.component';
import { ViewClientPrimaveraOrdersModule } from './ratings-table/view-client-primavera-orders/view-client-primavera-orders.module';
import { ViewClientPrimaveraInvoicesModule } from './ratings-table/view-client-primavera-invoices/view-client-primavera-invoices.module';
import { EditAllClientRatingsModule } from './ratings-table/edit-all-client-ratings/edit-all-client-ratings.module';
import { MatProgressBarModule } from '@angular/material/progress-bar';

@NgModule({
    declarations: [
        ClientsComponent,
        SegmentsTableComponent,
        RatingsTableComponent,
        ChangeClientSegmentComponent,
        EditClientRatingComponent,
    ],
    imports: [
        CommonModule,
        TranslocoModule,
        GridModule,
        MatIconModule,
        MatTooltipModule,
        MatButtonModule,
        MatDialogModule,
        ReactiveFormsModule,
        MatFormFieldModule,
        MatInputModule,
        MatTabsModule,
        MatSelectModule,
        MatProgressSpinnerModule,
        MatProgressBarModule,
        ViewClientPrimaveraOrdersModule,
        ViewClientPrimaveraInvoicesModule,
        EditAllClientRatingsModule,
    ],
})
export class ClientsModule {}
