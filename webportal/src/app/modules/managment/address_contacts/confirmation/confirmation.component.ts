import {
    ChangeDetectionStrategy,
    ChangeDetectorRef,
    Component,
    ElementRef,
    Inject,
    OnDestroy,
    OnInit,
    ViewChild,
    ViewEncapsulation,
} from '@angular/core';
import { DOCUMENT } from '@angular/common';
import { MatTabGroup } from '@angular/material/tabs';
import { Subject } from 'rxjs';
import { takeUntil } from 'rxjs/operators';
import { FuseMediaWatcherService } from '@fuse/services/media-watcher';

import { ManagmentService } from 'app/modules/managment/managment.service';
import {
    AbstractControl,
    FormArray,
    FormBuilder,
    FormControl,
    FormGroup,
    ValidationErrors,
    Validators,
} from '@angular/forms';
import { MatDialog, MatDialogConfig } from '@angular/material/dialog';
import { NavigationExtras, Router } from '@angular/router';
import { TranslocoService } from '@ngneat/transloco';
import { Client_Address } from '../../managment.types';
import { FuseConfirmationService } from '@fuse/services/confirmation';
import { FuseSplashScreenService } from '@fuse/services/splash-screen';
import { forEach } from 'lodash';

@Component({
    selector: 'app-client-address-confirmation',
    templateUrl: './confirmation.component.html',
    encapsulation: ViewEncapsulation.None,
    changeDetection: ChangeDetectionStrategy.OnPush,
})
export class ClientConfirmationComponent implements OnInit, OnDestroy {
    flashMessage: 'success' | 'error' | null = null;
    flashMessageText: string;
    drawerMode: 'over' | 'side' = 'side';
    drawerOpened: boolean = true;
    private _unsubscribeAll: Subject<any> = new Subject<any>();

    composeForm: FormGroup;
    isLoading: boolean;
    client_address: [] = [];
    /**
     * Constructor
     */
    constructor(
        @Inject(DOCUMENT) private _document: Document,
        private _managmentService: ManagmentService,
        private _changeDetectorRef: ChangeDetectorRef,
        private _elementRef: ElementRef,
        private _fuseMediaWatcherService: FuseMediaWatcherService,
        private _formBuilder: FormBuilder,
        private _router: Router,
        private readonly translocoService: TranslocoService,
        private _fuseSplashScreenService: FuseSplashScreenService,
        private _fuseConfirmationService: FuseConfirmationService
    ) {}

    // -----------------------------------------------------------------------------------------------------
    // @ Lifecycle hooks
    // -----------------------------------------------------------------------------------------------------

    /**
     * On init
     */
    ngOnInit(): void {
        this.createForm();

        // Subscribe to media changes
        this._fuseMediaWatcherService.onMediaChange$
            .pipe(takeUntil(this._unsubscribeAll))
            .subscribe(({ matchingAliases }) => {
                // Set the drawerMode and drawerOpened
                if (matchingAliases.includes('lg')) {
                    this.drawerMode = 'side';
                    this.drawerOpened = true;
                } else {
                    this.drawerMode = 'over';
                    this.drawerOpened = false;
                }

                // Mark for check
                this._changeDetectorRef.markForCheck();
            });
    }

    /**
     * On destroy
     */
    ngOnDestroy(): void {
        // Unsubscribe from all subscriptions
        this._unsubscribeAll.next();
        this._unsubscribeAll.complete();
    }

    createForm() {
        this.composeForm = this._formBuilder.group({
            addresses: this._formBuilder.array([]),
        });
        this._managmentService.client_addresses$
            .pipe(takeUntil(this._unsubscribeAll))
            .subscribe((addresses: any) => {
                if (addresses[0].updated_at) return;
                this.client_address = addresses;
                for (const field of addresses) {
                    const validators = [];
                    validators.push(Validators.required);

                    const newFormGroup = this._formBuilder.group({
                        entity_id: [
                            field.entity_id,
                            [
                                Validators.required,
                                Validators.minLength(1),
                                Validators.maxLength(100),
                            ],
                        ],

                        address_id: [
                            field.address_id,
                            [
                                Validators.required,
                                Validators.minLength(1),
                                Validators.maxLength(500),
                            ],
                        ],
                        address: [
                            field.address,
                            [
                                Validators.required,
                                Validators.minLength(1),
                                Validators.maxLength(500),
                            ],
                        ],
                        locality: [
                            { value: field.locality, disabled: false },
                            [
                                Validators.required,
                                Validators.minLength(1),
                                Validators.maxLength(500),
                            ],
                        ],
                        zip_code: [
                            field.zip_code,
                            [
                                Validators.required,
                                Validators.minLength(1),
                                Validators.maxLength(500),
                            ],
                        ],
                        zip_locality: [
                            field.zip_locality,
                            [
                                Validators.required,
                                Validators.minLength(1),
                                Validators.maxLength(500),
                            ],
                        ],
                        phone: [
                            field.phone,
                            [
                                Validators.pattern('[1-8][0-9]{8}'),
                                Validators.minLength(0),
                                Validators.maxLength(9),
                            ],
                        ],
                        mobile_phone: [
                            field.mobile_phone,
                            [
                                Validators.pattern('9[0-9]{8}'),
                                Validators.minLength(0),
                                Validators.maxLength(9),
                            ],
                        ],
                        email: [
                            field.email,
                            [
                                Validators.required,
                                Validators.minLength(1),
                                Validators.maxLength(500),
                                Validators.email,
                            ],
                        ],
                        name: [
                            field.name,
                            [
                                Validators.required,
                                Validators.minLength(1),
                                Validators.maxLength(500),
                            ],
                        ],
                        token: [
                            field.token,
                            [
                                Validators.required,
                                Validators.minLength(1),
                                Validators.maxLength(500),
                            ],
                        ],
                    });

                    (this.composeForm.get('addresses') as FormArray).push(
                        newFormGroup
                    );
                }

                // Mark for check
                this._changeDetectorRef.markForCheck();
            });
    }

    // -----------------------------------------------------------------------------------------------------
    // @ Public methods
    // -----------------------------------------------------------------------------------------------------

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

    saveAndClose(): void {
        if (this.composeForm.invalid) {
            return;
        }

        var showErroValidation: boolean = false;
        const addresses: Client_Address[] =
            this.composeForm.controls['addresses'].value;

        addresses.forEach((add: Client_Address) => {
            if (!add.mobile_phone && !add.phone) {
                showErroValidation = true;
                return;
            }
        });

        if (showErroValidation) {
            this.showFlashMessage(
                'error',
                this.translocoService.translate('Clients.label7', {})
            );
            return;
        }

        this._managmentService
            .edit({ addresses: addresses })
            .subscribe((response) => {
                this.isLoading = false;
                if (response) {
                    this.showFlashMessage(
                        'success',
                        this.translocoService.translate(
                            'Dados Atualizados com sucesso'
                        )
                    );
                    this._router.navigate(['/order/success'], {
                        queryParams: {
                            label: 'Contactos atualizados com sucesso',
                        },
                    });
                } else {
                    this.showFlashMessage('error', 'Erro a atualizar dados');
                    this.createForm();
                }
                this._changeDetectorRef.markForCheck();
            });
        // Close the dialog
    }

    // MOVE FUNCTION TO NEW POP UP INFO COMPONENTs
    remove(client_address) {
        const filter_add: Client_Address[] = this.client_address.filter(
            (add: Client_Address) =>
                add.address_id == client_address.get('address_id').value
        );

        // Open the confirmation dialog
        const confirmation = this._fuseConfirmationService.open({
            title: 'Eliminar Morada',
            message: 'Tem a certeza que pretende eliminar a morada? ',
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
                this._fuseSplashScreenService.show();
                this._managmentService
                    .remove(filter_add[0])
                    .subscribe((response) => {
                        this.isLoading = false;
                        if (response) {
                            this.showFlashMessage(
                                'success',
                                this.translocoService.translate(
                                    'Operators.operator-deleted',
                                    {}
                                )
                            );
                            location.reload();
                        } else {
                            this._fuseSplashScreenService.hide();
                            this.showFlashMessage(
                                'error',
                                this.translocoService.translate(
                                    'Operators.operator-error',
                                    {}
                                )
                            );
                        }
                        this._changeDetectorRef.markForCheck();
                    });
                this._changeDetectorRef.markForCheck();
            }
        });
    }

    // -----------------------------------------------------------------------------------------------------
    // @ Private methods
    // -----------------------------------------------------------------------------------------------------

    /**
     * Scrolls the current step element from
     * sidenav into the view. This only happens when
     * previous/next buttons pressed as we don't want
     * to change the scroll position of the sidebar
     * when the user actually clicks around the sidebar.
     *
     * @private
     */
    private _scrollCurrentStepElementIntoView(): void {
        // Wrap everything into setTimeout so we can make sure that the 'current-step' class points to correct element
        setTimeout(() => {
            // Get the current step element and scroll it into view
            const currentStepElement =
                this._document.getElementsByClassName('current-step')[0];
            if (currentStepElement) {
                currentStepElement.scrollIntoView({
                    behavior: 'smooth',
                    block: 'start',
                });
            }
        });
    }

    requirePhoneOrMobile(control: AbstractControl): ValidationErrors | null {
        const formGroup = control as FormGroup;
        const formValues = formGroup.value;

        const phone = formValues.phone;
        const mobilePhone = formValues.mobile_phone;

        // Verifica se pelo menos um dos campos está preenchido
        if (!phone && !mobilePhone) {
            return { requirePhoneOrMobile: false };
        }

        return null;
    }
}
