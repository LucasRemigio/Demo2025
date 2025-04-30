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
import { ScriptsService } from '../scripts/scripts.service';

@Component({
    selector: 'app-add-queues',
    templateUrl: './add-queues.component.html',
    encapsulation: ViewEncapsulation.None,
})
export class AddQueues implements OnInit {
    composeForm: FormGroup;
    flashMessage: 'success' | 'error' | null = null;
    flashMessageText: string;
    filesToUpload: FilesToUpload[] = [];
    scriptNames: string[] = [];
    isLoading: boolean = false;

    constructor(
        private _changeDetectorRef: ChangeDetectorRef,
        public matDialogRef: MatDialogRef<AddQueues>,
        private _formBuilder: FormBuilder,
        private scriptsService: ScriptsService,
        private translocoService: TranslocoService,
        @Inject(MAT_DIALOG_DATA) public data: any
    ) {}

    ngOnInit(): void {
        this.fetchScriptNamesFromBE();

        // Create the form
        this.composeForm = this._formBuilder.group({
          name: [
            '',
            [Validators.minLength(0), Validators.maxLength(500)],
          ],
          description: [
            '',
            [Validators.minLength(0), Validators.maxLength(500)],
          ],
          autoRetry: [false],
          numberRetry: [
            0,  // Initial value
            [Validators.required, Validators.min(0)]
          ],
          script_name: ['', [Validators.required]]
        });
    }

    fetchScriptNamesFromBE(): void {
        this.isLoading = true;

        this.scriptsService.getScripts().subscribe(
            (response) => {
                this.isLoading = false;
                if (response && response.scripts_items) {
                    // Filter out scripts with status '2' and extract names
                    this.scriptNames = response.scripts_items
                        .filter(script => script.status !== '2')
                        .map(script => script.name);

                    // Trigger change detection
                    this._changeDetectorRef.markForCheck();
                } else {
                }
            },
            (err) => {
                this.isLoading = false;
                this.showFlashMessage(
                    'error',
                    this.translocoService.translate('error-loading-list', {})
                );

                this._changeDetectorRef.markForCheck();
            }
        );
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

        const name = this.composeForm.controls['name'].value;
        const description = this.composeForm.controls['description'].value;
        const autoRetry = this.composeForm.controls['autoRetry'].value;
        const numberRetry = this.composeForm.controls['numberRetry'].value;
        const inactive = false;
        const script_name = this.composeForm.controls['script_name'].value;

        if (!script_name) {
            // Show our desired error message
            this.showFlashMessage('error', this.translocoService.translate('Scripts.script-name-required', {}));
            return; // Prevent further execution
        }

        // Close the dialog
        this.matDialogRef.close({
            name: name,
            description: description,
            autoRetry: autoRetry,
            numberRetry: numberRetry,
            script_name: script_name,
            inactive: inactive
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