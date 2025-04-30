import { NgModule } from '@angular/core';
import { RouterModule } from '@angular/router';
import { MatButtonModule } from '@angular/material/button';
import { MatFormFieldModule } from '@angular/material/form-field';
import { MatIconModule } from '@angular/material/icon';
import { MatInputModule } from '@angular/material/input';
import { MatProgressSpinnerModule } from '@angular/material/progress-spinner';
import { FuseCardModule } from '@fuse/components/card';
import { FuseAlertModule } from '@fuse/components/alert';
import { SharedModule } from 'app/shared/shared.module';
import { authResetPasswordRoutes } from 'app/modules/auth/reset-password/reset-password.routing';
import { PasswordStrengthBarComponent } from './password-strength-bar.component';

@NgModule({
    declarations: [
        PasswordStrengthBarComponent
    ],
    imports     : [
        MatButtonModule,
        MatFormFieldModule,
        MatIconModule,
        MatInputModule,
        MatProgressSpinnerModule,
        FuseCardModule,
        FuseAlertModule,
        SharedModule,
    ],
    exports: [
        PasswordStrengthBarComponent
    ]
})
export class PasswordStrengthBarModule
{
}
