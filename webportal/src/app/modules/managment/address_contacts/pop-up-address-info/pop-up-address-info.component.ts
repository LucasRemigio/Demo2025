import {
    ChangeDetectorRef,
    Component,
    Inject,
    OnInit,
    ViewEncapsulation,
} from '@angular/core';
import { FormArray, FormBuilder, FormGroup, Validators } from '@angular/forms';
import { MAT_DIALOG_DATA, MatDialogRef } from '@angular/material/dialog';
import { Client_Address } from '../../managment.types';
import { FuseConfirmationService } from '@fuse/services/confirmation';

@Component({
    selector: 'app-pop-up-address-info',
    templateUrl: 'pop-up-address-info.component.html',
    encapsulation: ViewEncapsulation.None,
})
export class PopUpAddressInfo implements OnInit {
    composeForm: FormGroup;
    flashMessage: 'success' | 'error' | null = null;
    flashMessageText: string;
    disableSendButton: boolean = false;
    constructor(
        private _fuseConfirmationService: FuseConfirmationService,
        private _changeDetectorRef: ChangeDetectorRef,
        public matDialogRef: MatDialogRef<PopUpAddressInfo>,
        private _formBuilder: FormBuilder,
        @Inject(MAT_DIALOG_DATA) public data: any
    ) {}

    ngOnInit(): void {
        // Create the form
        this.composeForm = this._formBuilder.group({
            addresses: this._formBuilder.array([]),
        });

        for (const field of this.data.addresses) {
            if (field.updated_at) {
                this.disableSendButton = true;
            }
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

            (this.composeForm.get('addresses') as FormArray).push(newFormGroup);
        }
    }

    // -----------------------------------------------------------------------------------------------------
    // @ Public methods
    // -----------------------------------------------------------------------------------------------------

    close(): void {
        // Close the dialog
        this.matDialogRef.close();
    }

    /**
     * Save and close
     */
    saveAndClose(): void {
        if (this.composeForm.invalid) {
            return;
        }

        const addresses = this.composeForm.controls['addresses'].value;

        // Close the dialog
        this.matDialogRef.close({
            addresses: addresses,
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
}
