import {
    ChangeDetectorRef,
    Component,
    Inject,
    OnInit,
    ViewEncapsulation,
} from '@angular/core';
import { FormBuilder, FormControl, FormGroup, Validators } from '@angular/forms';
import { MAT_DIALOG_DATA, MatDialogRef } from '@angular/material/dialog';
import { FilesToUpload } from '../../managment/managment.types';
import { TranslocoService } from '@ngneat/transloco';
import { Queues } from '../queues/queues.types';

@Component({
    selector: 'app-edit-queues',
    templateUrl: './edit-queues.component.html',
    encapsulation: ViewEncapsulation.None,
})
export class EditQueues implements OnInit {
    composeForm: FormGroup;
    flashMessage: 'success' | 'error' | null = null;
    flashMessageText: string;
    filesToUpload: FilesToUpload[] = [];
    queue: Queues;
    isLoading: boolean = false;

    constructor(
        private _changeDetectorRef: ChangeDetectorRef,
        public matDialogRef: MatDialogRef<EditQueues>,
        private _formBuilder: FormBuilder,
        private translocoService: TranslocoService,
        @Inject(MAT_DIALOG_DATA) public data: any
    ) {
        this.queue = this.data.queue;
    }

    ngOnInit(): void {
        // if (this.data && this.data.queue) {
        // Convert the autoRetry field to a boolean
        let autoRetryBoolean: boolean;

        if (typeof this.queue.autoRetry === 'string') {
            autoRetryBoolean = (this.queue.autoRetry as string).toLowerCase() === 'true';
        } else {
            autoRetryBoolean = !!this.queue.autoRetry;
        }    

        this.composeForm = this._formBuilder.group({
        // id: [this.data.queue.id],
        name: [
            this.queue.name,
            [Validators.minLength(0), Validators.maxLength(500)],
        ],
        description: [
            this.queue.description,
            [Validators.minLength(0), Validators.maxLength(500)],
        ],
        autoRetry: [autoRetryBoolean],
        numberRetry: [
            this.queue.numberRetry,  // Initial value
            [Validators.required, Validators.min(0)]
        ] 
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

        const name = this.composeForm.controls['name'].value;
        const description = this.composeForm.controls['description'].value;
        const autoRetry = this.composeForm.controls['autoRetry'].value;
        const numberRetry = this.composeForm.controls['numberRetry'].value;

        // Close the dialog
        this.matDialogRef.close({
            name: name,
            description: description,
            autoRetry: autoRetry,
            numberRetry: numberRetry,
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