import {
    Component,
    OnInit,
    ViewEncapsulation,
    ChangeDetectorRef,
    ChangeDetectionStrategy,
} from '@angular/core';
import {
    FormBuilder,
    FormControl,
    FormGroup,
    Validators,
} from '@angular/forms';
import { Observable, Subject } from 'rxjs';
import { Assets } from './assets.types';
import { MatDialogConfig } from '@angular/material/dialog';
import { fuseAnimations } from '@fuse/animations';
import { of } from 'rxjs';
import { MatDialog } from '@angular/material/dialog';
import { AssetsService } from './assets.service';
import { FuseConfirmationService } from '@fuse/services/confirmation';
import { SharedModule } from 'app/shared/shared.module';
import { TranslocoService } from '@ngneat/transloco';
import { EditAsset } from './edit-asset/edit-asset.component';
import { AddAssets } from '../add-assets/add-assets.component';
import { FlashMessageService } from 'app/shared/components/flash-message/flash-message.service';

@Component({
    selector: 'app-assets',
    templateUrl: './assets.component.html',
    styles: [
        /* language=SCSS */
        `
            .assets-grid {
                grid-template-columns: 25% 25% 25% 25%;
            }
        `,
    ],
    encapsulation: ViewEncapsulation.None,
    changeDetection: ChangeDetectionStrategy.OnPush,
    animations: fuseAnimations,
})
export class AssetsComponent implements OnInit {
    searchInputControl: FormControl = new FormControl();
    isLoading: boolean = false;
    selectedAccount: Assets | null = null;
    selectedAccountForm: FormGroup;
    _unsubscribeAll: Subject<any> = new Subject<any>();
    searchEmailLabel: string = this.translocoService.translate(
        'search-by-email',
        {}
    );

    public gridData: Assets[] = [];

    assets$: Observable<Assets[]>;
    assetsArr: Assets[] = [];
    length: number;
    password = '';
    passPatternError: string = this._sharedModule.getPasswordValidatorError();
    assets: Assets[] = [];

    constructor(
        private _changeDetectorRef: ChangeDetectorRef,
        private _formBuilder: FormBuilder,
        private _matDialog: MatDialog,
        private _assetService: AssetsService,
        private _fuseConfirmationService: FuseConfirmationService,
        private _sharedModule: SharedModule,
        private readonly translocoService: TranslocoService,
        private _fm: FlashMessageService
    ) {}

    state: any = {
        skip: 0,
        take: 10,
        sort: [],
    };

    public pagesizes = [
        {
            text: '10',
            value: 10,
        },

        {
            text: '25',
            value: 25,
        },

        {
            text: '50',
            value: 50,
        },
    ];

    ngOnInit(): void {
        // Create the selected account form
        this.selectedAccountForm = this._formBuilder.group({
            name: [''],
            password: [
                '',
                [
                    Validators.required,
                    Validators.pattern(
                        this._sharedModule.getPasswordValidatorPattern()
                    ),
                ],
            ],
        });

        this.updateAssets();
    }

    expandedItemId: string | null = null;

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

    updateAssets(emailFilter?: string) {
        this.isLoading = true;

        this._assetService.getAssets(emailFilter).subscribe(
            (response) => {
                this.isLoading = false;

                if (response && response.assets) {
                    this.assetsArr = response.assets;
                    this.assets$ = of(this.assetsArr);
                    this.gridData = response.assets;
                }

                this._changeDetectorRef.detectChanges();
                // this._changeDetectorRef.markForCheck();
            },
            (err) => {
                this.isLoading = false;
                this._fm.error('error-loading-list');

                this._changeDetectorRef.detectChanges();
                // this._changeDetectorRef.markForCheck();
            }
        );
    }

    deleteAsset(id: string) {
        // Open the confirmation dialog
        const confirmation = this._fuseConfirmationService.open({
            title: this.translocoService.translate('Scripts.delete-asset', {}),
            message: this.translocoService.translate(
                'Scripts.delete-asset-text',
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
                this._assetService.deleteAsset(id).subscribe(
                    (response) => {
                        this.isLoading = false;

                        if (response && response.result_code > 0) {
                            this._fm.success('Scripts.delete-asset-success');
                            this.selectedAccount = null;
                        } else {
                            this._fm.error('Scripts.delete-asset-error');
                        }

                        this.updateAssets();
                    },
                    (err) => {
                        this.isLoading = false;
                        this._fm.error('account-removed-error');
                    }
                );
            }
        });
    }

    /**
     * Toggle account details
     *
     * @param type
     *
     */
    toggleDetails(selectedType: string): void {
        // If the account is already selected...
        if (
            this.selectedAccount &&
            this.selectedAccount.description === selectedType
        ) {
            // Close the details
            this.closeDetails();
            return;
        }

        this.assets$.subscribe((accounts: Assets[]) => {
            this.selectedAccount = accounts.find(
                (account) => account.description === selectedType
            );

            // Fill the form
            this.selectedAccountForm.patchValue({
                name: this.selectedAccount.description,
                password: decodeURIComponent(
                    escape(atob(this.selectedAccount.description))
                ),
            });

            // Mark for check
            this._changeDetectorRef.markForCheck();
        });
    }

    editAsset(asset: Assets): void {
        // Open the dialog
        const dialogConfig: MatDialogConfig = {
            maxHeight: '100vh',
            minWidth: '60vh',
            data: {
                asset: asset,
            },
        };

        const dialogRef = this._matDialog.open(EditAsset, dialogConfig);

        dialogRef.afterClosed().subscribe((result) => {
            if (result) {
                this.isLoading = true;
                asset.description = result.description;
                asset.user = result.user;
                asset.password = result.password;
                asset.text = result.text;
                asset.type = result.type;

                this._assetService
                    .editAsset(
                        asset.description,
                        asset.type,
                        asset.text,
                        asset.user,
                        result.password,
                        asset.id
                    )
                    .subscribe((response) => {
                        this.isLoading = false;
                        if (response && response.result_code > 0) {
                            this._fm.success('Scripts.edit-asset-success');
                            this.updateAssets();
                        } else {
                            this._fm.error('Scripts.edit-asset-error');
                        }
                        this.updateAssets();
                    });
            }
        });
    }

    addAsset(): void {
        // Open the dialog
        const dialogConfig: MatDialogConfig = {
            maxHeight: '100vh',
            minWidth: '60vh',
        };

        const dialogRef = this._matDialog.open(AddAssets, dialogConfig);

        dialogRef.afterClosed().subscribe((result) => {
            if (result) {
                this.isLoading = true;

                this._assetService
                    .addAsset(
                        result.description,
                        result.type,
                        result.text,
                        result.user,
                        result.password
                    )
                    .subscribe(
                        (response) => {
                            // alert(response);
                            this.isLoading = false;

                            if (response && response.result_code > 0) {
                                this._fm.success('Scripts.add-asset-success');
                                this.updateAssets();
                                this.isLoading = false;
                            } else {
                                this._fm.error('Scripts.edit-asset-error');
                            }
                        },
                        (err) => {
                            this.isLoading = false;
                            this._fm.error('Scripts.edit-asset-error');

                            this._changeDetectorRef.markForCheck();
                        }
                    );
            } else {
                this.isLoading = false;
            }
        });
    }

    /**
     * Close the details
     */
    closeDetails(): void {
        this.selectedAccount = null;
    }

    searchAssets(Id: string) {
        let searchTerm = '@';

        if (Id) {
        } else {
            this.updateAssets();
        }
    }

    /**
     * Create admin account
     */

    sendPassword(): void {
        if (!this.selectedAccount || !this.selectedAccount.description) {
            return;
        }

        if (this.selectedAccountForm.dirty) {
            this._fm.error('update-or-refresh');
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
                ) + this.selectedAccount.description,
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
                this._assetService
                    .sendCredentials(this.selectedAccount.description)
                    .subscribe(
                        (response) => {
                            this.isLoading = false;

                            if (response && response.result_code > 0) {
                                const message =
                                    this.translocoService.translate(
                                        'credentials-sent-success',
                                        {}
                                    ) + this.selectedAccount.description;
                                this._fm.success(message);
                            } else {
                                const message =
                                    this.translocoService.translate(
                                        'error',
                                        {}
                                    ) + response.result_message;
                                this._fm.error(message);
                            }

                            this.updateAssets();
                        },
                        (err) => {
                            this.isLoading = false;
                            this._fm.error('credentials-sent-error');
                        }
                    );
            }
        });
    }

    /**
     * Track by function for ngFor loops
     *
     * @param index
     * @param item
     */
    trackById(index: number, asset: any): any {
        return asset.id || index;
    }
}
