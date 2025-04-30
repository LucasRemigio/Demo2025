import {
    ChangeDetectorRef,
    Component,
    OnInit,
    ViewChild,
    ViewEncapsulation,
} from '@angular/core';
import { FormBuilder, FormGroup, NgForm, Validators } from '@angular/forms';
import { MatDialog, MatDialogConfig } from '@angular/material/dialog';
import { ActivatedRoute, Router } from '@angular/router';
import { fuseAnimations } from '@fuse/animations';
import { FuseAlertType } from '@fuse/components/alert';
import { AuthService } from 'app/core/auth/auth.service';
import { environment } from 'environments/environment';
import { Auth2FComponent } from '../auth2f/auth2f.component';
import { ValidateAuth2f } from 'app/core/user/user.types';
import { TranslocoService } from '@ngneat/transloco';

@Component({
    selector: 'auth-sign-in',
    templateUrl: './sign-in.component.html',
    encapsulation: ViewEncapsulation.None,
    animations: fuseAnimations,
})
export class AuthSignInComponent implements OnInit {
    @ViewChild('signInNgForm') signInNgForm: NgForm;

    alert: { type: FuseAlertType; message: string } = {
        type: 'success',
        message: '',
    };
    signInForm: FormGroup;
    showAlert: boolean = false;
    captchaSolve: boolean = false;
    env = environment;
    composeForm: FormGroup;
    auth2f_isActive: boolean;
    useAuth2f: boolean;
    isLoading: boolean;
    flashMessage: 'success' | 'error' | null = null;
    flashMessageText: string;

    /**
     * Constructor
     */
    constructor(
        private _activatedRoute: ActivatedRoute,
        private _authService: AuthService,
        private _formBuilder: FormBuilder,
        private _router: Router,
        private _matDialog: MatDialog,
        private _changeDetectorRef: ChangeDetectorRef,
    ) {}

    // -----------------------------------------------------------------------------------------------------
    // @ Lifecycle hooks
    // -----------------------------------------------------------------------------------------------------

    /**
     * On init
     */
    ngOnInit(): void {
        // Create the form
        this.signInForm = this._formBuilder.group({
            email: ['', [Validators.required, Validators.email]],
            password: ['', Validators.required],
        });

        if (!environment.production) {
            this.captchaSolve = true;
        }
    }

    // -----------------------------------------------------------------------------------------------------
    // @ Public methods
    // -----------------------------------------------------------------------------------------------------

    public resolved(captchaResponse: string) {
        this.captchaSolve = true;
    }

    /**
     * Sign in
     */
    signIn(): void {
        // Return if the form is invalid
        if (this.signInForm.invalid) {
            return;
        }

        // Disable the form
        this.signInForm.disable();

        // Hide the alert
        this.showAlert = false;

        // Sign in
        this._authService
            .signIn(
                {
                    email: this.signInForm.controls['email'].value,
                    password: btoa(this.signInForm.controls['password'].value),
                },
                this.useAuth2f
            )
            .subscribe(
                (response) => {
                    if (
                        response.result_code &&
                        Number(response.result_code) < 0
                    ) {
                        this.loginError(response.result);
                        return;
                    }

                    // Set the redirect url.
                    // The '/signed-in-redirect' is a dummy url to catch the request and redirect the user
                    // to the correct page after a successful sign in. This way, that url can be set via
                    // routing file and we don't have to touch here.
                    const redirectURL =
                        this._activatedRoute.snapshot.queryParamMap.get(
                            'redirectURL'
                        ) || '/signed-in-redirect';

                    if (response.useAuth2f) {
                        //Open Auth2f pop-up
                        const dialogConfig: MatDialogConfig = {
                            maxHeight: '90vh',
                            minWidth: '80vh',
                            disableClose: true,
                            data: {
                                email: this.signInForm.controls['email'].value,
                                useAuth2f: this.useAuth2f,
                            },
                        };

                        const dialogRef = this._matDialog.open(
                            Auth2FComponent,
                            dialogConfig
                        );

                        //Close Auth2f pop-up
                        dialogRef.afterClosed().subscribe((result) => {
                            if (result) {
                                var newEntry: ValidateAuth2f = {
                                    auth2f_code: result.token,
                                    email: this.signInForm.controls['email']
                                        .value,
                                };

                                this._authService
                                    .validateAuth2FCode(newEntry)
                                    .subscribe((response) => {
                                        this.isLoading = false;

                                        if (
                                            response &&
                                            response.result_code > 0
                                        ) {
                                            // Navigate to the redirect url
                                            this._router.navigateByUrl(
                                                redirectURL
                                            );
                                        } else {
                                            this.loginError(
                                                'Error using two factor authentication. Please try again'
                                            );
                                        }
                                        (err) => {
                                            this.isLoading = false;
                                            this.loginError(
                                                'Please refresh the page and try again'
                                            );
                                        };
                                    });
                            }
                        });
                    } else {
                        // Navigate to the redirect url
                        this._router.navigateByUrl(redirectURL);
                    }
                },
                (response) => {
                    this.loginError('Try again later');
                }
            );
    }

    private loginError(message: string): void {
        // Re-enable the form
        this.signInForm.enable();

        // Reset the form
        this.signInNgForm.resetForm();

        // Set the alert
        this.alert = {
            type: 'error',
            message: message,
        };

        // Show the alert
        this.showAlert = true;
    }

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
