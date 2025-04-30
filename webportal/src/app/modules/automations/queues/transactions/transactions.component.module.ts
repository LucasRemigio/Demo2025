import { NgModule } from '@angular/core';
import { Route, RouterModule } from '@angular/router';
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
import { MatSelectModule } from '@angular/material/select';
import { GridModule } from '@progress/kendo-angular-grid';
import { MatCheckboxModule } from '@angular/material/checkbox';
import { TransactionsComponent } from './transactions.component';
import { PasswordStrengthBarModule } from 'app/modules/auth/password-strength-bar/password-strenght-bar.module';
import { MatDialogModule } from '@angular/material/dialog';
import { FormsModule, ReactiveFormsModule } from '@angular/forms';
import { EditTransactions } from './edit-transaction/edit-transactions.component';
import { ViewTransactionComponent } from './view-transactions/view-transactions.component';

const TransactionsRoutes: Route[] = [
    {
        path: '',
        component: TransactionsComponent,
    },
];

@NgModule({
    declarations: [
        TransactionsComponent,
        EditTransactions,
        ViewTransactionComponent,
    ],
    imports: [
        RouterModule.forChild(TransactionsRoutes),
        MatFormFieldModule,
        MatTabsModule,
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
        MatSlideToggleModule,
        MatDialogModule,
        FormsModule,
        ReactiveFormsModule,
    ],
})
export class TransactionsModule {}