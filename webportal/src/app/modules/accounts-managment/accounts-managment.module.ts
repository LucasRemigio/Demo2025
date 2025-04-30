import { NgModule } from '@angular/core';
import { Route, RouterModule } from '@angular/router';
import { AccountsManagmentComponent } from './accounts-managment.component';
import { MatFormFieldModule } from '@angular/material/form-field';
import { MatButtonModule } from '@angular/material/button';
import { MatIconModule } from '@angular/material/icon';
import { MatInputModule } from '@angular/material/input';
import { SharedModule } from 'app/shared/shared.module';
import { AddAccountComponent } from './add-account/add-account.component';
import { MatProgressBarModule } from '@angular/material/progress-bar';
import { MatTooltipModule } from '@angular/material/tooltip';
import { TranslocoModule } from '@ngneat/transloco';
import { PasswordStrengthBarModule } from '../auth/password-strength-bar/password-strenght-bar.module';
import { MatSelectModule } from '@angular/material/select';
import { RoleManagerComponent } from './role-manager/role-manager.component';


const AccountsManagmentRoutes: Route[] = [
  {
      path     : '',
      component: AccountsManagmentComponent
  }
];

@NgModule({
  declarations: [
    AccountsManagmentComponent,
    AddAccountComponent,
    RoleManagerComponent
  ],
  imports: [
    RouterModule.forChild(AccountsManagmentRoutes),
    MatFormFieldModule,
    MatIconModule,
    MatInputModule,
    SharedModule,
    MatButtonModule,
    TranslocoModule,
    MatProgressBarModule,
    MatTooltipModule,
    PasswordStrengthBarModule,
    MatSelectModule,
  ]
})
export class AccountsManagmentModule { }


