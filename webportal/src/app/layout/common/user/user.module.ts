import { NgModule } from '@angular/core';
import { MatButtonModule } from '@angular/material/button';
import { MatDividerModule } from '@angular/material/divider';
import { MatFormFieldModule } from '@angular/material/form-field';
import { MatIconModule } from '@angular/material/icon';
import { MatInputModule } from '@angular/material/input';
import { MatMenuModule } from '@angular/material/menu';
import { UserComponent } from 'app/layout/common/user/user.component';
import { SharedModule } from 'app/shared/shared.module';
import { ChangePasswordComponent } from './change-password/change-password.component';
import { ChangeSignatureComponent } from './change-signature/change-signature.component';
import { TranslocoModule } from '@ngneat/transloco';
import { QuillModule } from 'ngx-quill';

@NgModule({
    declarations: [
        UserComponent,
        ChangePasswordComponent,
        ChangeSignatureComponent,
    ],
    imports: [
        MatButtonModule,
        MatDividerModule,
        MatIconModule,
        MatMenuModule,
        SharedModule,
        MatFormFieldModule,
        MatInputModule,
        TranslocoModule,
        QuillModule.forRoot(),
    ],
    exports: [UserComponent],
})
export class UserModule {}
