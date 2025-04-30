import {
    ChangeDetectorRef,
    Component,
    Inject,
    OnInit,
    ViewEncapsulation,
} from '@angular/core';
import { FormBuilder, FormGroup, Validators } from '@angular/forms';
import { MAT_DIALOG_DATA, MatDialogRef } from '@angular/material/dialog';

@Component({
    selector: 'app-pop-up-info',
    templateUrl: './pop-up-info.component.html',
    encapsulation: ViewEncapsulation.None,
})
export class PopUpInfo implements OnInit {
    composeForm: FormGroup;
    flashMessage: 'success' | 'error' | null = null;
    flashMessageText: string;

    constructor(
        private _changeDetectorRef: ChangeDetectorRef,
        public matDialogRef: MatDialogRef<PopUpInfo>,
        private _formBuilder: FormBuilder,
        @Inject(MAT_DIALOG_DATA) public data: any
    ) {}

    ngOnInit(): void {
        // Create the form
        this.composeForm = this._formBuilder.group({
            client_email: [
                this.data.client.client_email,
                [Validators.maxLength(500)],
            ],
            client_id: [
                this.data.client.client_id,
                [Validators.maxLength(500)],
            ],
            client_vat: [
                this.data.client.client_vat,
                [Validators.maxLength(9)],
            ],
        });
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

        const client_email = this.composeForm.controls['client_email'].value;
        const client_id = this.composeForm.controls['client_id'].value;
        const client_vat = this.composeForm.controls['client_vat'].value;

        // Close the dialog
        this.matDialogRef.close({
            client_email: client_email,
            client_id: client_id,
            client_vat: client_vat,
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
