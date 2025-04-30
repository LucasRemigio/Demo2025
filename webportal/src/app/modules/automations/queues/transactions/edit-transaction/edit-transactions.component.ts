import {
    ChangeDetectorRef,
    Component,
    Inject,
    OnInit,
    ViewEncapsulation,
} from '@angular/core';
import { FormBuilder, FormControl, FormGroup, Validators } from '@angular/forms';
import { MAT_DIALOG_DATA, MatDialogRef } from '@angular/material/dialog';
import { TranslocoService } from '@ngneat/transloco';
import { Transactions } from '../transactions.types';

@Component({
    selector: 'app-edit-transactions',
    templateUrl: './edit-transactions.component.html',
    encapsulation: ViewEncapsulation.None,
})
export class EditTransactions implements OnInit {
    composeForm: FormGroup;
    statusList: {id: number, name: string} [];
    flashMessage: 'success' | 'error' | null = null;
    flashMessageText: string;
    transaction: Transactions;
    isLoading: boolean = false;

    constructor(
        private _changeDetectorRef: ChangeDetectorRef,
        public matDialogRef: MatDialogRef<EditTransactions>,
        private _formBuilder: FormBuilder,
        private translocoService: TranslocoService,
        @Inject(MAT_DIALOG_DATA) public data: any
    ) {
        this.transaction = this.data.transaction;
        this.statusList = this.data.statusList;
    }

    ngOnInit(): void {

        this.composeForm = this._formBuilder.group({
            id: [this.transaction.id, Validators.required],
            status_id: [this.transaction.status_id, [Validators.minLength(0), Validators.maxLength(500)]],
            started: [this.transaction.started, [Validators.minLength(0), Validators.maxLength(500)]],
            ended: [this.transaction.ended, [Validators.minLength(0), Validators.maxLength(500)]],
            exception: [this.transaction.exception, [Validators.minLength(0), Validators.maxLength(500)]],
            output_data: [this.transaction.output_data, [Validators.required, Validators.min(0)]]
        });

        this.composeForm.get('status_id').valueChanges.subscribe((value) => {
        });

        // alert(this.composeForm.get('autoRetry').value);
        // alert(this.queue.autoRetry);
    }

    // -----------------------------------------------------------------------------------------------------
    // @ Public methods
    // -----------------------------------------------------------------------------------------------------

    close(): void {
        // Close the dialog
        this.isLoading = false;
        this.matDialogRef.close();
    }

    /**
     * Save and close
     */
    saveAndClose(): void {
        if (this.composeForm.invalid) {
            return;
        }

        const formValue = this.composeForm.value;

        // trying to verify the payload

        // Close the dialog and send form data
        this.matDialogRef.close(formValue);

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