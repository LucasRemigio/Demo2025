import { NgModule } from '@angular/core';
import { Route, RouterModule } from '@angular/router';
import { QueuesComponent } from './queues.component';
import { AddAssets } from '../add-assets/add-assets.component';
import { MatButtonModule } from '@angular/material/button';
import { MatFormFieldModule } from '@angular/material/form-field';
import { MatSidenavModule } from '@angular/material/sidenav';
import { MatSlideToggleModule } from '@angular/material/slide-toggle';
import { MatIconModule } from '@angular/material/icon';
import { MatDatepickerModule } from '@angular/material/datepicker';
import { MatInputModule } from '@angular/material/input';
import { MatTabsModule } from '@angular/material/tabs';
import { SharedModule } from 'app/shared/shared.module';
import { MatProgressBarModule } from '@angular/material/progress-bar';
import { MatTooltipModule } from '@angular/material/tooltip';
import { TranslocoModule } from '@ngneat/transloco';
import { PasswordStrengthBarModule } from '../../auth/password-strength-bar/password-strenght-bar.module';
import { MatSelectModule } from '@angular/material/select';
import { GridModule } from '@progress/kendo-angular-grid';
// import { PopUpInfoTransaction } from './pop-up-info-transaction/pop-up-info-transaction.component';
import { AddQueues } from '../add-queues/add-queues.component';
import { MatCheckboxModule } from '@angular/material/checkbox';
import { EditQueues } from '../edit-queues/edit-queues.component';
import { TransactionsComponent } from './transactions/transactions.component';

const QueuesRoutes: Route[] = [
    {
        path: '',
        component: QueuesComponent,
    },
];

@NgModule({
    declarations: [
        QueuesComponent,
        AddQueues,
        EditQueues
    ],
    imports: [
        RouterModule.forChild(QueuesRoutes),
        MatFormFieldModule,
        MatIconModule,
        MatInputModule,
        SharedModule,
        MatButtonModule,
        MatProgressBarModule,
        TranslocoModule,
        MatProgressBarModule,
        MatTooltipModule,
        MatTabsModule,
        MatDatepickerModule,
        PasswordStrengthBarModule,
        MatSelectModule,
        GridModule,
        MatCheckboxModule,
        MatSidenavModule,
        MatSlideToggleModule
    ],
})
export class QueuesModule {}