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
import { Subscription } from 'rxjs';

@Component({
    selector: 'app-edit-script',
    templateUrl: './edit-script.component.html',
    encapsulation: ViewEncapsulation.None,
})
export class EditScript implements OnInit {
    isLoading = false;
    private subscriptions: Subscription[] = [];
    composeForm: FormGroup;
    flashMessage: 'success' | 'error' | null = null;
    flashMessageText: string;
    filesToUpload: FilesToUpload[] = [];

    constructor(
        private _changeDetectorRef: ChangeDetectorRef,
        public matDialogRef: MatDialogRef<EditScript>,
        private _formBuilder: FormBuilder,
        private translocoService: TranslocoService,
        @Inject(MAT_DIALOG_DATA) public data: any
    ) {}

    ngOnInit(): void {
        if (this.data.script && this.data) {
            this.composeForm = this._formBuilder.group({
                file: ['', []],
                name: [
                    this.data.script.name,
                    [Validators.minLength(1), Validators.maxLength(500)],
                ],
                description: [
                    this.data.script.description,
                    [Validators.minLength(1), Validators.maxLength(500)],
                ],
                cron_Job: [
                    this.data.script.cron_job,
                    [Validators.minLength(1), Validators.maxLength(500)],
                ],
                main_file: [
                    this.data.script.main_file,
                    [Validators.minLength(1), Validators.maxLength(500)],
                ],
            });
        }
    }

    // -----------------------------------------------------------------------------------------------------
    // @ Public methods
    // -----------------------------------------------------------------------------------------------------

    close(): void {
        // Close the dialog
        if (!this.isLoading) {
            this.matDialogRef.close();
            this._changeDetectorRef.detectChanges(); // Manually trigger change detection
        }
    }

    /**
     * Save and close
     */
    saveAndClose(): void {
        if (this.composeForm.invalid) {
            return;
        }

        this.isLoading = true;

        const file_content =
            this.filesToUpload.length > 0
                ? this.filesToUpload[0].fileData
                : null;
        const name = this.composeForm.controls['name'].value;

        const description = this.composeForm.controls['description'].value;
        const cron_job = this.composeForm.controls['cron_Job'].value;
        const main_file = this.composeForm.controls['main_file'].value;
        const id = this.data.script.id;

        // Close the dialog
        this.matDialogRef.close({
            id: id,
            name: name,
            description: description,
            file_content: file_content,
            cron_job: cron_job,
            main_file: main_file,
        });

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
