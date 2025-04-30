import {
    ChangeDetectorRef,
    Component,
    Inject,
    OnInit,
    ViewEncapsulation,
} from '@angular/core';
import { FormBuilder, FormGroup, Validators } from '@angular/forms';
import { MAT_DIALOG_DATA, MatDialogRef } from '@angular/material/dialog';
import { FilesToUpload } from '../../managment.types';
import { TranslocoService } from '@ngneat/transloco';

@Component({
    selector: 'app-import-file',
    templateUrl: './import-file.component.html',
    encapsulation: ViewEncapsulation.None,
})
export class ImportFile implements OnInit {
    composeForm: FormGroup;
    flashMessage: 'success' | 'error' | null = null;
    flashMessageText: string;
    filesToUpload: FilesToUpload[] = [];

    constructor(
        private _changeDetectorRef: ChangeDetectorRef,
        public matDialogRef: MatDialogRef<ImportFile>,
        private _formBuilder: FormBuilder,
        private translocoService: TranslocoService,
        @Inject(MAT_DIALOG_DATA) public data: any
    ) {}

    ngOnInit(): void {
        // Create the form
        this.composeForm = this._formBuilder.group({
            file: ['', []],
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

        // Close the dialog
        this.matDialogRef.close({
            file_content: file_content,
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
            if (this.validateAndDecodeBase64File(data)) {
                this.composeForm.controls['file'].setValue(currentFile.name);
            }
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
        const allowedTypes = ['application/json'];

        if (!allowedTypes.includes(currentFile.type)) {
            this.showFlashMessage(
                'error',
                this.translocoService.translate(
                    'Clients.import-file-format',
                    {}
                )
            );
            return false;
        }

        if (currentFile.size > 100 * 1024 * 1024) {
            this.showFlashMessage(
                'error',
                this.translocoService.translate('Clients.import-file-size', {})
            );
            return false;
        }

        return true;
    }

    async validateAndDecodeBase64File(base64File: string): Promise<boolean> {
        return true;
        try {
            const decodedFile = atob(base64File);
            const parsedFile = JSON.parse(decodedFile);

            if (parsedFile.clientes && parsedFile.clientes.length > 0) {
                for (const cliente of parsedFile.clientes) {
                    if (
                        cliente.idEntidade &&
                        cliente.Nome &&
                        cliente.moradas &&
                        cliente.moradas.length > 0
                    ) {
                        for (const morada of cliente.moradas) {
                            if (
                                morada.idMorada &&
                                morada.Morada &&
                                morada.Localidade &&
                                morada.CodPostal
                            ) {
                                continue; // Continue looping for other addresses
                            } else {
                                throw new Error(
                                    this.translocoService.translate(
                                        'Clients.import-file-error',
                                        {}
                                    ) +
                                        cliente.idEntidade +
                                        '-' +
                                        cliente.Nome
                                );
                            }
                        }
                    } else {
                        throw new Error(
                            this.translocoService.translate(
                                'Clients.import-file-error',
                                {}
                            ) +
                                cliente.idEntidade +
                                '-' +
                                cliente.Nome
                        );
                    }
                }
                return true; // All entries are valid
            } else {
                throw new Error(
                    this.translocoService.translate('Clients.no-records', {})
                );
            }
        } catch (error) {
            this.showFlashMessage('error', error);
            return false;
        }
    }
}
