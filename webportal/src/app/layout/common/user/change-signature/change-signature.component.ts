import { ChangeDetectorRef, Component, OnInit, ViewChild } from '@angular/core';
import { FormBuilder, FormGroup, Validators } from '@angular/forms';
import { MatDialogRef } from '@angular/material/dialog';
import { TranslocoService } from '@ngneat/transloco';
import { SharedModule } from 'app/shared/shared.module';
import { UserSettingsService } from '../user-settings.service';
import {
    Signature,
    SignatureResponse,
    SimpleMessage,
} from '../user-settings.types';
import { sign } from 'crypto';
import Quill from 'quill';

@Component({
    selector: 'app-change-signature',
    templateUrl: './change-signature.component.html',
    styleUrls: ['./change-signature.component.scss'],
})
export class ChangeSignatureComponent implements OnInit {
    composeForm: FormGroup;

    quillModules = {
        toolbar: [
            [{ header: [1, 2, false] }],
            [{ font: [] }, { size: [] }],
            ['bold', 'italic', 'underline'],
            [{ color: [] }, { background: [] }],
            [{ align: [] }],
            ['link'],
        ],
        clipboard: {
            matchVisual: false,
            matchers: [],
        },
    };

    flashMessage: 'success' | 'error' | null = null;
    flashMessageText: string;

    constructor(
        public matDialogRef: MatDialogRef<ChangeSignatureComponent>,
        private _formBuilder: FormBuilder,
        private _sharedModule: SharedModule,
        private _changeDetectorRef: ChangeDetectorRef,
        private translocoService: TranslocoService,
        private _userSettingsService: UserSettingsService
    ) {}

    ngOnInit(): void {
        // Create the form
        this.composeForm = this._formBuilder.group({
            signature: [
                '',
                [
                    Validators.required,
                    Validators.minLength(5),
                    Validators.maxLength(9999),
                ],
            ],
        });

        const font = Quill.import('formats/font');
        font.whitelist = ['sans-serif', 'serif', 'monospace'];
        Quill.register(font, true);

        this._userSettingsService.getSignature().subscribe(
            (response: SignatureResponse) => {
                this.composeForm.patchValue({
                    signature: response.signature.signature,
                });

                // Apply styles to all images within .ql-editor using JavaScript
                this.styleSignatureImages();
            },
            (error) => {
                console.error('Error fetching signature:', error);
            }
        );
    }

    // -----------------------------------------------------------------------------------------------------
    // @ Public methods
    // -----------------------------------------------------------------------------------------------------
    translate(key: string, params?: object): string {
        return this.translocoService.translate(key, params || {});
    }

    close(): void {
        // Close the dialog
        this.matDialogRef.close();
    }

    submit(): void {
        if (this.composeForm.controls['signature'].errors) {
            return;
        }
        this.styleSignatureImages();

        const signature = this.composeForm.controls['signature'].value;
        // send patch request to backend
        this._userSettingsService.patchSignature(signature).subscribe(
            (response: SignatureResponse) => {
                if (response.result !== 'Sucesso') {
                    this.showFlashMessage(
                        'error',
                        this.translocoService.translate('signature-error', {})
                    );
                    return;
                }
                this.showFlashMessage(
                    'success',
                    this.translocoService.translate('signature-success', {})
                );
                return;
            },
            (error: any) => {
                this.showFlashMessage(
                    'error',
                    this.translocoService.translate('signature-error', {})
                );
                return;
            }
        );

        return;
    }

    generateTemplate(): void {
        // If nothing is the in box, current text will assume ''
        const currentText = this.composeForm.get('signature')?.value ?? '';
        this._userSettingsService
            .getTemplateSignature()
            .subscribe((response: SimpleMessage) => {
                this.composeForm.setValue({
                    signature: currentText + response.message,
                });
                this.styleSignatureImages();
            });
    }

    getChristmasGreeting(): void {
        // If nothing is the in box, current text will assume ''
        const currentText = this.composeForm.get('signature')?.value ?? '';
        this._userSettingsService
            .getChristmasGreeting()
            .subscribe((response: SimpleMessage) => {
                this.composeForm.setValue({
                    signature: currentText + response.message,
                });
                this.styleSignatureImages();
            });
    }

    styleSignatureImages(): void {
        // Apply styles to all images within .ql-editor using JavaScript
        document
            .querySelectorAll('div.ql-editor p > img')
            .forEach((img: any) => {
                img.style.width = 'auto';
                img.style.maxWidth = '200px';
                img.style.height = 'auto';
                img.style.display = 'inline-block';
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

            if (type === 'success') {
                this.matDialogRef.close();
            }
        }, 5000);
    }
}
