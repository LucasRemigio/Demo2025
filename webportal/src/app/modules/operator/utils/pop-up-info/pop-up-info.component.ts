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
            operator_name: [
                this.data.operator.operator_name,
                [
                    Validators.required,
                    Validators.minLength(1),
                    Validators.maxLength(500),
                ],
            ],
            operator_email: [
                this.data.operator.operator_email,
                [
                    Validators.required,
                    Validators.minLength(1),
                    Validators.maxLength(500),
                    Validators.email,
                ],
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

        const operator_name = this.composeForm.controls['operator_name'].value;
        const operator_email =
            this.composeForm.controls['operator_email'].value;

        // Close the dialog
        this.matDialogRef.close({
            operator_name: operator_name,
            operator_email: operator_email,
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
