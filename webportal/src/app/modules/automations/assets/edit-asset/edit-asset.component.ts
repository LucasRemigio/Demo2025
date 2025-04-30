import {
    ChangeDetectorRef,
    Component,
    Inject,
    OnInit,
    ViewEncapsulation,
} from '@angular/core';
import { FormBuilder, FormGroup, Validators } from '@angular/forms';
import { MAT_DIALOG_DATA, MatDialogRef } from '@angular/material/dialog';
import { TranslocoService } from '@ngneat/transloco';
import { FilesToUpload } from '../../../managment/managment.types';

@Component({
    selector: 'app-edit-asset',
    templateUrl: './edit-asset.component.html',
    encapsulation: ViewEncapsulation.None,
})
export class EditAsset implements OnInit {
    composeForm: FormGroup;
    flashMessage: 'success' | 'error' | null = null;
    flashMessageText: string;
    passwordVisible: boolean;
    loading: boolean = false;

    constructor(
        private _changeDetectorRef: ChangeDetectorRef,
        public matDialogRef: MatDialogRef<EditAsset>,
        private _formBuilder: FormBuilder,
        private translocoService: TranslocoService,
        @Inject(MAT_DIALOG_DATA) public data: any
    ) {}

    ngOnInit(): void {
        if (this.data && this.data.asset) {
          this.composeForm = this._formBuilder.group({
            description: [
              this.data.asset.description,
              [Validators.minLength(1), Validators.maxLength(500)],
            ],
            type: [
              this.data.asset.type,
              [Validators.minLength(1), Validators.maxLength(500)],
            ],
            text: [this.data.asset.text || ''],
            user: [this.data.asset.user || ''],
            password: [this.data.asset.password || ''],
          });

          this._changeDetectorRef.detectChanges();

        }

    }

    isCredentialsType() {
        return this.composeForm.get('type').value === 'credentials';
    }

    // -----------------------------------------------------------------------------------------------------
    // @ Public methods
    // -----------------------------------------------------------------------------------------------------

    close(): void {
        // Close the dialog
        this.loading = false;
        this.matDialogRef.close();
    }

    /**
     * Save and close
     */
    saveAndClose(): void {
        if (this.composeForm.invalid) {
            return;
        }

        const description = this.composeForm.controls['description'].value;

        if (!description) {
            // Show an error message
            this.showFlashMessage('error', this.translocoService.translate('Scripts.description-field-required', {}));
            return; // Prevent further execution
        }

        const type = this.composeForm.controls['type'].value;
        const text = this.composeForm.controls['text'].value;
        const user = this.composeForm.controls['user'].value;
        const password = this.composeForm.controls['password'].value;

        // Close the dialog
        this.matDialogRef.close({
            description: description,
            type: type,
            text: text,
            user: user,
            password: password,
        });

        this.loading = false;
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
