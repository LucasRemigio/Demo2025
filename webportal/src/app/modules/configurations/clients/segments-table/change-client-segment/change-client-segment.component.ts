import {
    ChangeDetectorRef,
    Component,
    Inject,
    Input,
    OnInit,
    Output,
} from '@angular/core';
import { FormBuilder, FormGroup, Validators } from '@angular/forms';
import { MatDialogRef, MAT_DIALOG_DATA } from '@angular/material/dialog';
import { TranslocoService } from '@ngneat/transloco';
import { ClientsService } from '../../clients.service';
import { Client, Segment } from '../../clients.types';

@Component({
    selector: 'app-change-client-segment',
    templateUrl: './change-client-segment.component.html',
    styleUrls: ['./change-client-segment.component.scss'],
})
export class ChangeClientSegmentComponent implements OnInit {
    segments: Segment[] = [];
    changeClientSegmentForm: FormGroup;
    isLoading = false;
    client: Client;

    flashMessage: 'success' | 'error' | null = null;
    flashMessageText: string;

    constructor(
        private clientsService: ClientsService,
        private fb: FormBuilder,
        private _changeDetectorRef: ChangeDetectorRef,
        private _transloco: TranslocoService,
        private dialogRef: MatDialogRef<ChangeClientSegmentComponent>,
        @Inject(MAT_DIALOG_DATA)
        public injectedClient: any
    ) {
        this.client = injectedClient.client;
    }

    ngOnInit(): void {
        this.isLoading = true;

        this.changeClientSegmentForm = this.fb.group({
            segment: [this.client.segment.id, Validators.required],
        });

        this.clientsService.getSegments().subscribe((response) => {
            this.segments = response.segments;
            this.isLoading = false;
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

    close(): void {
        this.dialogRef.close();
    }

    save(): void {
        // validate form
        if (this.changeClientSegmentForm.invalid) {
            this.showFlashMessage(
                'error',
                this._transloco.translate('form-invalid')
            );
            return;
        }

        this.isLoading = true;
        this.clientsService
            .patchClientSegment(
                this.client.code,
                this.changeClientSegmentForm.value.segment
            )
            .subscribe(
                (result) => {
                    this.isLoading = false;

                    if (result.result_code !== 1) {
                        this.showFlashMessage(
                            'error',
                            this._transloco.translate('change-segment-error')
                        );
                        return;
                    }

                    this.dialogRef.close(true);
                },
                (error) => {
                    this.showFlashMessage(
                        'error',
                        this._transloco.translate('change-segment-error')
                    );
                    this.isLoading = false;
                }
            );
    }
}
