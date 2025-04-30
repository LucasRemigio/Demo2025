import { Component, OnInit, ViewChild, ViewEncapsulation } from '@angular/core';
import { FormBuilder, FormGroup, NgForm, Validators } from '@angular/forms';
import { finalize } from 'rxjs/operators';
import { fuseAnimations } from '@fuse/animations';
import { FuseValidators } from '@fuse/validators';
import { FuseAlertType } from '@fuse/components/alert';
import { AuthService } from 'app/core/auth/auth.service';
import { ActivatedRoute, Params, Router } from '@angular/router';
import { SharedModule } from 'app/shared/shared.module';
import { environment } from 'environments/environment';

@Component({
    selector: 'auth-reset-password',
    templateUrl: './reset-password.component.html',
    encapsulation: ViewEncapsulation.None,
    animations: fuseAnimations
})
export class AuthResetPasswordComponent implements OnInit {
    alert: { type: FuseAlertType; message: string } = {
        type: 'success',
        message: ''
    };
    resetPasswordForm: FormGroup;
    showAlert: boolean = false;
    captchaSolve: boolean = false;
    passPatternError: string = this._sharedModule.getPasswordValidatorError();
    env = environment;
    token: string = '';
    email: string = '';

    /**
     * Constructor
     */
    constructor(
        private _activatedRoute: ActivatedRoute,
        private _authService: AuthService,
        private _formBuilder: FormBuilder,
        private _router: Router,
        private _sharedModule: SharedModule
    ) {
    }

    // -----------------------------------------------------------------------------------------------------
    // @ Lifecycle hooks
    // -----------------------------------------------------------------------------------------------------

    /**
     * On init
     */
    ngOnInit(): void {
        // Create the form
        this.resetPasswordForm = this._formBuilder.group({
            password: ['', [Validators.required, Validators.pattern(this._sharedModule.getPasswordValidatorPattern())]],
            passwordConfirm: ['', Validators.required]
        },
            {
                validators: FuseValidators.mustMatch('password', 'passwordConfirm')
            }
        );

        if (!environment.production) {
            this.captchaSolve = true;
        }

        this._activatedRoute.params.subscribe((params: Params) => {
            this.email = params['email'],
                this.token = params['token']
        });
    }

    // -----------------------------------------------------------------------------------------------------
    // @ Public methods
    // -----------------------------------------------------------------------------------------------------

    public resolved(captchaResponse: string) {
        this.captchaSolve = true;
    }

    /**
     * Reset password
     */
    resetPassword(): void {
        // Return if the form is invalid
        if (this.resetPasswordForm.invalid || !this.email || !this.token) {
            return;
        }

        // Disable the form
        this.resetPasswordForm.disable();

        // Hide the alert
        this.showAlert = false;
        var password: string = btoa(this.resetPasswordForm.get('password').value);

        // Send the request to the server
        this._authService.resetPassword(this.email, this.token, password)
            .pipe(
                finalize(() => {
                    // Re-enable the form
                    this.resetPasswordForm.enable();
                    this.resetPasswordForm.reset();
                    // Show the alert
                    this.showAlert = true;
                })
            )
            .subscribe(
                (response) => {
                    if (response.result_code && Number(response.result_code) > 0){
                        this.alert = {
                            type: 'success',
                            message: 'Your password has been reset. Please try to login using your new password'
                        }
                        setTimeout(() => { this._router.navigate(['/sign-in']); }, 5000);
                    }
                    // InvalidArgsResetPass and EqualsRecentHistoryPass 
                    else if (response.result_code && (Number(response.result_code) === -28 || Number(response.result_code) === -29)){
                        this.alert = {
                            type: 'success',
                            message: response.result
                        }
                    }
                    else {
                        this.alert = {
                            type: 'error',
                            message: 'Something went wrong, please try again.'
                        };
                    }
                },
                (response) => {
                    this.alert = {
                        type: 'error',
                        message: 'Something went wrong, please try again.'
                    };
                }
            );
    }
}
