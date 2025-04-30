import {
    ChangeDetectorRef,
    Component,
    OnInit,
    ViewEncapsulation,
} from '@angular/core';
import { FormBuilder, FormGroup, Validators } from '@angular/forms';
import { MatDialogRef } from '@angular/material/dialog';
import { FuseValidators } from '@fuse/validators';
import { TranslocoService } from '@ngneat/transloco';
import { UserService } from 'app/core/user/user.service';
import { Password } from 'app/core/user/user.types';
import { SharedModule } from 'app/shared/shared.module';
import { runInThisContext } from 'vm';

@Component({
    selector: 'app-change-password',
    templateUrl: './change-password.component.html',
    encapsulation: ViewEncapsulation.None,
})
export class ChangePasswordComponent implements OnInit {
    composeForm: FormGroup;
    passPatternError: string = this._sharedModule.getPasswordValidatorError();
    flashMessage: 'success' | 'error' | null = null;
    flashMessageText: string;

    constructor(
        public matDialogRef: MatDialogRef<ChangePasswordComponent>,
        private _formBuilder: FormBuilder,
        private _sharedModule: SharedModule,
        private _userService: UserService,
        private _changeDetectorRef: ChangeDetectorRef,
        private translocoService: TranslocoService
    ) {}

    ngOnInit(): void {
        // Create the form
        this.composeForm = this._formBuilder.group(
            {
                OldPass: [
                    '',
                    [
                        Validators.required,
                        Validators.minLength(2),
                        Validators.maxLength(100),
                    ],
                ],
                NewPass: [
                    '',
                    [
                        Validators.required,
                        Validators.pattern(
                            this._sharedModule.getPasswordValidatorPattern()
                        ),
                    ],
                ],
                RepeatNewPass: ['', [Validators.required]],
            },
            {
                validators: FuseValidators.mustMatch(
                    'NewPass',
                    'RepeatNewPass'
                ),
            }
        );
    }

    // -----------------------------------------------------------------------------------------------------
    // @ Public methods
    // -----------------------------------------------------------------------------------------------------
    close(): void {
        // Close the dialog
        this.matDialogRef.close();
    }

    translate(key: string, params?: object): string {
        return this.translocoService.translate(key, params || {});
    }

    submit(): void {
        if (
            this.composeForm.controls['OldPass'].errors ||
            this.composeForm.controls['NewPass'].errors ||
            this.composeForm.controls['RepeatNewPass'].errors
        ) {
            return;
        }

        const OldPass = this.composeForm.controls['OldPass'].value;
        const NewPass = this.composeForm.controls['NewPass'].value;
        const RepeatNewPass = this.composeForm.controls['RepeatNewPass'].value;
        const email = this._userService.getLoggedUserEmail();

        const newPassword: Password = {
            OldPass: btoa(OldPass),
            NewPass: btoa(NewPass),
            RepeatNewPass: btoa(RepeatNewPass),
            email: email,
        };
        this._userService.updatePass(newPassword).subscribe(
            (response) => {
                if (response && response.result_code > 0) {
                    this.showFlashMessage('success', 'Password changed');
                } else {
                    this.showFlashMessage('error', response.result);
                }
                this._changeDetectorRef.markForCheck();
            },
            (err) => {
                this.showFlashMessage('error', 'Error changing password');

                this._changeDetectorRef.markForCheck();
            }
        );
    }

    /**
     * Show flash message
     */
    showFlashMessage(type: 'success' | 'error', textMsg: string): void {
        // Show the message
        this.flashMessage = type;
        this.flashMessageText = textMsg;

        // Mark for check
        this._changeDetectorRef.markForCheck();

        // Hide it after 3 seconds
        setTimeout(() => {
            this.flashMessage = null;
            this.flashMessageText = null;

            // Mark for check
            this._changeDetectorRef.markForCheck();
        }, 5000);
    }
}
