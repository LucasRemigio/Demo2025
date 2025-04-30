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
import { Script } from './scripts.types';
import { MatDialogConfig } from '@angular/material/dialog';
import { fuseAnimations } from '@fuse/animations';
import { of } from 'rxjs';
import { MatDialog } from '@angular/material/dialog';
import { ScriptsService } from './scripts.service';
import { FuseConfirmationService } from '@fuse/services/confirmation';
import { SharedModule } from 'app/shared/shared.module';
import { TranslocoService } from '@ngneat/transloco';
import { EditScript } from './edit-script/edit-script.component';
import { AddScript } from '../add-script/add-script.component';
import { Router } from '@angular/router';
import { FlashMessageService } from 'app/shared/components/flash-message/flash-message.service';

@Component({
    selector: 'app-scripts',
    templateUrl: './scripts.component.html',
    styles: [
        /* language=SCSS */
        `
            .scripts-grid {
                grid-template-columns: 25% 25% 25% 25%;
            }
        `,
    ],
    encapsulation: ViewEncapsulation.None,
    changeDetection: ChangeDetectionStrategy.OnPush,
    animations: fuseAnimations,
})
export class ScriptsComponent implements OnInit {
    searchInputControl: FormControl = new FormControl();
    isLoading: boolean = false;
    selectedAccount: Script | null = null;
    selectedAccountForm: FormGroup;
    _unsubscribeAll: Subject<any> = new Subject<any>();
    searchEmailLabel: string = this.translocoService.translate(
        'search-by-email',
        {}
    );

    public gridData: Script[] = [];

    scripts$: Observable<Script[]>;
    scriptsArr: Script[] = [];
    length: number;
    password = '';
    passPatternError: string = this._sharedModule.getPasswordValidatorError();
    scripts: Script[] = [];

    constructor(
        private _changeDetectorRef: ChangeDetectorRef,
        private _formBuilder: FormBuilder,
        private _matDialog: MatDialog,
        private _scriptService: ScriptsService,
        private _fuseConfirmationService: FuseConfirmationService,
        private _sharedModule: SharedModule,
        private router: Router,
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

        this.updateScripts();
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

    updateScripts(id?: string) {
        this.isLoading = true;

        this._scriptService.getScripts(id).subscribe(
            (response) => {
                this.isLoading = false;
                if (response && response.scripts_items) {
                    this.scriptsArr = response.scripts_items;
                    this.scripts$ = of(this.scriptsArr);
                    this.gridData = response.scripts_items;
                }

                this._changeDetectorRef.markForCheck();
            },
            (err) => {
                this.isLoading = false;
                this._fm.error('error-loading-list');

                this._changeDetectorRef.markForCheck();
            }
        );
    }

    StartScripts(id: string): void {
        this.isLoading = true;

        this._scriptService.StartScripts(id).subscribe(
            (response) => {
                if (response.result_code <= 0) {
                    this._fm.error('Scripts.start-script-error');
                    this.isLoading = false;
                    return;
                }
                this.isLoading = false;
                this._fm.success('Scripts.run-script-finish');
                this.updateScripts();
            },
            (err) => {
                this.isLoading = false;
                this._fm.error('Scripts.start-script-error');

                this._changeDetectorRef.markForCheck();
            }
        );
    }

    goToExecutions(id: String): void {
        JSON.stringify(id);
        this.router
            .navigate(['jobs', id])
            .then((success) => {})
            .catch((error) => {});
    }

    getTriggersByScriptId(id: String): void {
        JSON.stringify(id);
        this.router
            .navigate(['triggers', id])
            .then((success) => {})
            .catch((error) => {});
    }

    deleteScript(id: string) {
        // Open the confirmation dialog
        this.isLoading = false;
        const confirmation = this._fuseConfirmationService.open({
            title: this.translocoService.translate('Scripts.delete-script', {}),
            message: this.translocoService.translate('Scripts.delete-text', {}),
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
                this._scriptService.deleteAccount(id).subscribe(
                    (response) => {
                        this.isLoading = false;

                        if (response && response.result_code > 0) {
                            this._fm.success('Scripts.delete-script-success');
                            this.selectedAccount = null;
                        } else {
                            this._fm.error('Scripts.delete-script-error');
                        }

                        this.updateScripts();
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
            this.selectedAccount.name === selectedType
        ) {
            // Close the details
            this.closeDetails();
            return;
        }

        this.scripts$.subscribe((accounts: Script[]) => {
            this.selectedAccount = accounts.find(
                (account) => account.name === selectedType
            );

            // Fill the form
            this.selectedAccountForm.patchValue({
                name: this.selectedAccount.name,
                password: decodeURIComponent(
                    escape(atob(this.selectedAccount.name))
                ),
            });

            // Mark for check
            this._changeDetectorRef.markForCheck();
        });
    }

    editScript(script: Script): void {
        // Open the dialog
        const dialogConfig: MatDialogConfig = {
            maxHeight: '100vh',
            minWidth: '60vh',
            data: {
                script: script,
            },
        };

        const dialogRef = this._matDialog.open(EditScript, dialogConfig);

        dialogRef.afterClosed().subscribe((result) => {
            if (result) {
                this.isLoading = true;
                const id = result.id;
                script.description = result.description;
                script.cron_job = result.cron_job;
                script.main_file = result.main_file;

                this._scriptService
                    .editScript(
                        script.name,
                        result.file_content,
                        script.description,
                        script.main_file,
                        script.cron_job,
                        id
                    )
                    .subscribe(
                        (response) => {
                            this.isLoading = false; // Reset loading state

                            if (response) {
                                this._fm.success('Scripts.edit-script-success');
                                this.updateScripts(); // Update script list
                            } else {
                                this._fm.error('Scripts.edit-script-error');
                            }
                        },
                        (error) => {
                            this.isLoading = false; // Reset loading state on error
                            console.error('Error editing script:', error);
                            this._fm.error('Scripts.edit-script-error');
                        }
                    );
            }
        });
    }

    importFile(): void {
        // Open the dialog
        const dialogConfig: MatDialogConfig = {
            maxHeight: '100vh',
            minWidth: '60vh',
        };

        const dialogRef = this._matDialog.open(AddScript, dialogConfig);

        dialogRef.afterClosed().subscribe((result) => {
            if (result) {
                this.isLoading = false;

                this._scriptService
                    .addScript(
                        result.name,
                        result.main_file,
                        result.description,
                        result.file_content
                    )
                    .subscribe(
                        (response) => {
                            // alert(response);
                            this.isLoading = false;
                            this._changeDetectorRef.detectChanges();

                            if (response && response.result_code > 0) {
                                this._fm.success(
                                    'Scripts.import-script-success'
                                );
                                this.updateScripts();
                            } else {
                                this._fm.error('Scripts.import-script-error');
                            }
                        },
                        (err) => {
                            this.isLoading = false;
                            this._changeDetectorRef.detectChanges();
                            this._fm.error('Scripts.import-script-error');

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

    searchScripts(Id: string) {
        let searchTerm = '@';

        if (Id) {
            this.StartScripts(Id);
        } else {
            this.updateScripts();
        }
    }

    /**
     * Create admin account
     */

    sendPassword(): void {
        if (!this.selectedAccount || !this.selectedAccount.name) {
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
                ) + this.selectedAccount.name,
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
                this._scriptService
                    .sendCredentials(this.selectedAccount.name)
                    .subscribe(
                        (response) => {
                            this.isLoading = false;

                            if (response && response.result_code > 0) {
                                const message =
                                    this.translocoService.translate(
                                        'credentials-sent-success',
                                        {}
                                    ) + this.selectedAccount.name;
                                this._fm.success(message);
                            } else {
                                const message =
                                    this.translocoService.translate(
                                        'error',
                                        {}
                                    ) + response.result_message;
                                this._fm.error(message);
                            }

                            this.updateScripts();
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
    trackById(index: number, item: any): any {
        return item.id || index;
    }
}
