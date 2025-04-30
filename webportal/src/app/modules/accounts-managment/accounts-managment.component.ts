/* eslint-disable @typescript-eslint/naming-convention */
import {
    Component,
    OnInit,
    ViewEncapsulation,
    ChangeDetectorRef,
    ChangeDetectionStrategy,
    ViewChild,
} from '@angular/core';
import {
    FormBuilder,
    FormControl,
    FormGroup,
    Validators,
} from '@angular/forms';
import { merge, Observable, Subject } from 'rxjs';
import {
    UpdateAccountEntry,
    AccountEntry,
    AccountManagmentEntry,
    DepartmentRoleEntry,
} from './accounts-managment.types';
import { fuseAnimations } from '@fuse/animations';
import { of } from 'rxjs';
import { tap } from 'rxjs/operators';
import { MatDialog } from '@angular/material/dialog';
import { AddAccountComponent } from './add-account/add-account.component';
import { AccountsManagmentService } from './accounts-managment.service';
import { FuseConfirmationService } from '@fuse/services/confirmation';
import { SharedModule } from 'app/shared/shared.module';
import { TranslocoService } from '@ngneat/transloco';
import { RoleManagerComponent } from './role-manager/role-manager.component';

@Component({
    selector: 'app-accounts-managment',
    templateUrl: './accounts-managment.component.html',
    styles: [
        /* language=SCSS */
        `
            .accounts-managment-grid {
                grid-template-columns: 15% 15% 10% 20% 20% 20%;
            }
        `,
    ],
    encapsulation: ViewEncapsulation.None,
    changeDetection: ChangeDetectionStrategy.OnPush,
    animations: fuseAnimations,
})
export class AccountsManagmentComponent implements OnInit {
    searchInputControl: FormControl = new FormControl();
    isLoading: boolean = false;
    selectedAccount: AccountManagmentEntry | null = null;
    selectedAccountForm: FormGroup;

    flashMessage: 'success' | 'error' | null = null;
    flashMessageText: string;
    private _unsubscribeAll: Subject<any> = new Subject<any>();
    searchEmailLabel: string = this.translocoService.translate(
        'search-by-email',
        {}
    );

    accounts$: Observable<AccountManagmentEntry[]>;
    private accountsArr: AccountManagmentEntry[] = [];
    length: number;
    password = '';
    passPatternError: string = this._sharedModule.getPasswordValidatorError();

    // access to role checkbox values
    @ViewChild(RoleManagerComponent) roleManager: RoleManagerComponent;

    constructor(
        private _changeDetectorRef: ChangeDetectorRef,
        private _formBuilder: FormBuilder,
        private _matDialog: MatDialog,
        private _accountMngService: AccountsManagmentService,
        private _fuseConfirmationService: FuseConfirmationService,
        private _sharedModule: SharedModule,
        private readonly translocoService: TranslocoService
    ) {}

    ngOnInit(): void {
        // Create the selected account form
        this.selectedAccountForm = this._formBuilder.group({
            email: [''],
            name: [''],
            // password: [
            //     '',
            //     [
            //         Validators.required,
            //         Validators.pattern(
            //             this._sharedModule.getPasswordValidatorPattern()
            //         ),
            //     ],
            // ],
        });

        this.updateAccounts();
    }

    ngOnDestroy(): void {
        // Unsubscribe from all subscriptions
        this._unsubscribeAll.next();
        this._unsubscribeAll.complete();
    }

    // -----------------------------------------------------------------------------------------------------
    // @ Public methods
    // -----------------------------------------------------------------------------------------------------

    generateNewPass(): void {
        this.password = this._sharedModule.generateNewPass();
        this.selectedAccountForm.controls['password'].setValue(this.password);
    }

    deleteSelectedAccount(): void {
        if (!this.selectedAccount.email) {
            return;
        }

        // Open the confirmation dialog
        const confirmation = this._fuseConfirmationService.open({
            title: this.translocoService.translate('delete-account', {}),
            message: this.translocoService.translate(
                'are-you-sure-account',
                {}
            ),
            actions: {
                confirm: {
                    label: this.translocoService.translate('delete', {}),
                },

                cancel: {
                    label: this.translocoService.translate('cancel', {}),
                },
            },
        });

        // Subscribe to the confirmation dialog closed action
        confirmation.afterClosed().subscribe((result) => {
            // If the confirm button pressed...
            if (result === 'confirmed') {
                this._accountMngService
                    .deleteAccount(this.selectedAccount.email)
                    .subscribe(
                        (response) => {
                            this.isLoading = false;

                            if (response && response.result_code > 0) {
                                this.showFlashMessage(
                                    'success',
                                    this.translocoService.translate(
                                        'account-removed-success',
                                        {}
                                    )
                                );
                                this.selectedAccount = null;
                            } else {
                                this.showFlashMessage(
                                    this.translocoService.translate(
                                        'error',
                                        {}
                                    ),
                                    response.result
                                );
                            }

                            this.updateAccounts();
                        },
                        (err) => {
                            this.isLoading = false;
                            this.showFlashMessage(
                                'error',
                                this.translocoService.translate(
                                    'account-removed-error',
                                    {}
                                )
                            );
                        }
                    );
            }
        });
    }

    updateSelectedAccount() {
        if (!this.selectedAccount || !this.selectedAccount.email) {
            return;
        }

        if (
            !this.selectedAccountForm.value['name'] ||
            // !this.selectedAccountForm.value['password'] ||
            this.selectedAccountForm.invalid
        ) {
            this.showFlashMessage(
                'error',
                this.translocoService.translate('name-pass-invalid', {})
            );
            return;
        }

        this.selectedAccountForm.markAsPristine();
        const pw: string = btoa(
            unescape(
                encodeURIComponent(this.selectedAccountForm.value['password'])
            )
        );
        const accountIn: UpdateAccountEntry = {
            updated_name: this.selectedAccountForm.value['name'],
            email: this.selectedAccount.email,
            updated_password: pw,
            updated_departments: this.roleManager.getDepartments(),
        };
        this._accountMngService.updateAccount(accountIn).subscribe(
            (response) => {
                this.isLoading = false;

                if (response && response.result_code > 0) {
                    this.showFlashMessage(
                        'success',
                        this.translocoService.translate(
                            'account-updated-success',
                            {}
                        )
                    );
                } else {
                    this.showFlashMessage('error', response.result);
                }

                this.updateAccounts();
            },
            (err) => {
                this.isLoading = false;
                this.showFlashMessage(
                    'error',
                    this.translocoService.translate('account-updated-error', {})
                );
            }
        );
    }

    /**
     * Toggle account details
     *
     * @param accountEmail
     */
    toggleDetails(accountEmail: string): void {
        // If the account is already selected...
        if (
            this.selectedAccount &&
            this.selectedAccount.email === accountEmail
        ) {
            // Close the details
            this.closeDetails();
            return;
        }

        this.accounts$.subscribe((accounts: AccountManagmentEntry[]) => {
            this.selectedAccount = accounts.find(
                (account) => account.email === accountEmail
            );

            // Fill the form
            this.selectedAccountForm.patchValue({
                email: accountEmail,
                name: this.selectedAccount.name,
                password: decodeURIComponent(
                    escape(atob(this.selectedAccount.password))
                ),
            });

            // Mark for check
            this._changeDetectorRef.markForCheck();
        });
    }

    /**
     * Close the details
     */
    closeDetails(): void {
        this.selectedAccount = null;
    }

    searchAccount(): void {
        let searchTerm = this.searchInputControl.value;

        if (searchTerm) {
            this.updateAccounts(searchTerm);
        } else {
            this.updateAccounts();
        }
    }

    /**
     * Create admin account
     */
    createAccount(): void {
        // Open the dialog
        const dialogRef = this._matDialog.open(AddAccountComponent);

        dialogRef.afterClosed().subscribe((result) => {
            if (result) {
                this._accountMngService
                    .addAccount({
                        email: result.email,
                        name: result.name,
                        password: btoa(result.password),
                        user_role_id: result.role,
                        departments: result.departments.map(
                            (entry) => entry.department
                        ),
                    })
                    .subscribe(
                        (response) => {
                            this.isLoading = false;
                            if (response && response.result_code > 0) {
                                this.showFlashMessage(
                                    'success',
                                    this.translocoService.translate(
                                        'account-created-success',
                                        {}
                                    )
                                );
                            } else {
                                this.showFlashMessage(
                                    this.translocoService.translate(
                                        'error',
                                        {}
                                    ),
                                    response.result
                                );
                            }

                            this.updateAccounts();
                        },
                        (err) => {
                            this.isLoading = false;
                            this.showFlashMessage(
                                'error',
                                this.translocoService.translate(
                                    'account-created-error',
                                    {}
                                )
                            );
                        }
                    );
            }
        });
    }

    sendPassword(): void {
        if (!this.selectedAccount || !this.selectedAccount.email) {
            return;
        }

        if (this.selectedAccountForm.dirty) {
            this.showFlashMessage(
                'error',
                this.translocoService.translate('update-or-refresh', {})
            );
            return;
        }

        // Open the confirmation dialog
        const confirmation = this._fuseConfirmationService.open({
            title: this.translocoService.translate(
                'send-credentials-confirmation',
                {}
            ),
            message:
                this.translocoService.translate(
                    'send-credentials-to-account',
                    {}
                ) + this.selectedAccount.email,
            actions: {
                confirm: {
                    label: this.translocoService.translate('confirm', {}),
                },
                cancel: {
                    label: this.translocoService.translate('cancel', {}),
                },
            },
        });

        // Subscribe to the confirmation dialog closed action
        confirmation.afterClosed().subscribe((result) => {
            // If the confirm button pressed...
            if (result === 'confirmed') {
                this._accountMngService
                    .sendCredentials(this.selectedAccount.email)
                    .subscribe(
                        (response) => {
                            this.isLoading = false;

                            if (response && response.result_code > 0) {
                                this.showFlashMessage(
                                    'success',
                                    this.translocoService.translate(
                                        'credentials-sent-success',
                                        {}
                                    ) + this.selectedAccount.email
                                );
                            } else {
                                this.showFlashMessage(
                                    this.translocoService.translate(
                                        'error',
                                        {}
                                    ),
                                    response.result
                                );
                            }

                            this.updateAccounts();
                        },
                        (err) => {
                            this.isLoading = false;
                            this.showFlashMessage(
                                'error',
                                this.translocoService.translate(
                                    'credentials-sent-error',
                                    {}
                                )
                            );
                        }
                    );
            }
        });
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

    /**
     * Track by function for ngFor loops
     *
     * @param index
     * @param item
     */
    trackByFn(index: number, item: any): any {
        return item.id || index;
    }

    private updateAccounts(emailFilter?: string) {
        this.isLoading = true;

        this._accountMngService.getAccounts(emailFilter).subscribe(
            (response) => {
                this.isLoading = false;

                if (response && response.user_items) {
                    this.accountsArr = response.user_items;
                    this.accounts$ = of(this.accountsArr);
                }

                this._changeDetectorRef.markForCheck();
            },
            (err) => {
                this.isLoading = false;
                this.showFlashMessage(
                    'error',
                    this.translocoService.translate('error-loading-list', {})
                );

                this._changeDetectorRef.markForCheck();
            }
        );
    }
}
