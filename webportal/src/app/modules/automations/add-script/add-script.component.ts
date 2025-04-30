import {
    ChangeDetectorRef,
    Component,
    Inject,
    OnInit,
    ViewEncapsulation,
} from '@angular/core';
import { FormBuilder, FormGroup, Validators } from '@angular/forms';
import { MAT_DIALOG_DATA, MatDialogRef } from '@angular/material/dialog';
import { FilesToUpload } from '../../managment/managment.types';
import { TranslocoService } from '@ngneat/transloco';

@Component({
    selector: 'app-add-script',
    templateUrl: './add-script.component.html',
    encapsulation: ViewEncapsulation.None,
})
export class AddScript implements OnInit {
    composeForm: FormGroup;
    flashMessage: 'success' | 'error' | null = null;
    flashMessageText: string;
    filesToUpload: FilesToUpload[] = [];

    constructor(
        private _changeDetectorRef: ChangeDetectorRef,
        public matDialogRef: MatDialogRef<AddScript>,
        private _formBuilder: FormBuilder,
        private translocoService: TranslocoService,
        @Inject(MAT_DIALOG_DATA) public data: any
    ) {}

    ngOnInit(): void {
        // Create the form
        this.composeForm = this._formBuilder.group({
            file: ['', []],
            name: ['', [Validators.minLength(1), Validators.maxLength(500)]],
            main_file: [
                '',
                [Validators.minLength(1), Validators.maxLength(500)],
            ],
            description: [
                '',
                [Validators.minLength(0), Validators.maxLength(500)],
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
        const file_content =
            this.filesToUpload.length > 0
                ? this.filesToUpload[0].fileData
                : null;

        const name = this.composeForm.controls['name'].value;
        const main_file = this.composeForm.controls['main_file'].value;
        const description = this.composeForm.controls['description'].value;

        // Close the dialog
        this.matDialogRef.close({
            name: name,
            file_content: file_content,
            main_file: main_file,
            description: description,
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

    uploadFile(fileList: FileList): void {
        if (fileList.length != 1) {
            // this.showFlashMessage('error', 'No valid files selected to upload');
            return;
        }

        let currentFile = fileList[0];
        if (!this.fileValidation(currentFile)) {
            return;
        }

        this.readAsDataURL(currentFile).then((data) => {
            this.filesToUpload.push({
                controlName: 'file',
                fileName: currentFile.name,
                fileExtension: currentFile.type,
                fileData: data,
            });
            this.composeForm.controls['file'].setValue(currentFile.name);
        });
    }

    private readAsDataURL(file: File): Promise<any> {
        // Return a new promise
        return new Promise((resolve, reject) => {
            // Create a new reader
            const reader = new FileReader();

            // Resolve the promise on success
            reader.onload = (): void => {
                resolve(reader.result.toString().split(',')[1]);
            };

            // Reject the promise on error
            reader.onerror = (e): void => {
                reject(e);
            };

            // Read the file as the
            reader.readAsDataURL(file);
        });
    }

    fileValidation(currentFile: File): boolean {
        const allowedTypes = [
            'application/zip',
            'application/x-zip-compressed',
        ];

        if (!allowedTypes.includes(currentFile.type)) {
            this.showFlashMessage(
                'error',
                this.translocoService.translate(
                    'Scripts.import-script-format',
                    {}
                )
            );
            return false;
        }

        if (currentFile.size > 100 * 1024 * 1024) {
            this.showFlashMessage(
                'error',
                this.translocoService.translate(
                    'Scripts.import-script-size',
                    {}
                )
            );
            return false;
        }

        return true;
    }
}
