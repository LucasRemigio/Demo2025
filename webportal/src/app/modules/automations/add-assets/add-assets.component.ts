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

@Component({
    selector: 'app-add-assets',
    templateUrl: './add-assets.component.html',
    encapsulation: ViewEncapsulation.None,
})
export class AddAssets implements OnInit {
    composeForm: FormGroup;
    flashMessage: 'success' | 'error' | null = null;
    flashMessageText: string;
    passwordVisible: boolean = false;
    isLoading: boolean = false;

    constructor(
        private _changeDetectorRef: ChangeDetectorRef,
        // public matDialogRef: MatDialogRef<ImportAssets>,
        public matDialogRef: MatDialogRef<AddAssets>,
        private _formBuilder: FormBuilder,
        private translocoService: TranslocoService,
        @Inject(MAT_DIALOG_DATA) public data: any
    ) {}

    ngOnInit(): void {

        // Create the form
        this.composeForm = this._formBuilder.group({
          description: [
            '',
            [Validators.minLength(1), Validators.maxLength(500)],
          ],
          type: ['text', [Validators.minLength(1), Validators.maxLength(500)],
        ],
          text: [''],
          user: [null],
          password: [null]
        });

        // Subscribe to changes in the type control
        this.composeForm.get('type').valueChanges.subscribe((value) => {
            if (value === 'credentials') {
            // If the type changes to "credentials," clear the user and password fields
            this.composeForm.patchValue({ user: null, password: null });
            }
        });
    }

    togglePasswordVisibility(): void {
      this.passwordVisible = !this.passwordVisible;
    }

    isCredentialsType() {
        return this.composeForm.get('type').value === 'Crendenciais';
    }

    // -----------------------------------------------------------------------------------------------------
    // @ Public methods
    // -----------------------------------------------------------------------------------------------------

    close(): void {
        // Close the dialog
        this.matDialogRef.close();
        this.isLoading = false;
    }

    /**
     * Save and close
     */
    saveAndClose(): void {

        const text = this.composeForm.controls['text'].value;
        const description = this.composeForm.controls['description'].value;

        // Check if either description or type is empty
        if (!description) {
            // Show an error message
            this.showFlashMessage('error', this.translocoService.translate('Scripts.description-field-required', {}));
            return; // Prevent further execution
        }

        const type = this.composeForm.controls['type'].value;

                if (!type) {
                    // Show an error message
                    this.showFlashMessage('error', this.translocoService.translate('Scripts.type-field-required', {}));
                    return; // Prevent further execution
                }

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

        this.isLoading = false;
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
