import { Component, Inject, OnInit } from '@angular/core';
import { FormBuilder, FormGroup, Validators } from '@angular/forms';
import { MatDialogRef, MAT_DIALOG_DATA } from '@angular/material/dialog';
import { CancelReasonService } from '../cancel-reasons.service';
import {
    CancelReason,
    CreateCancelReason,
    UpdateCancelReason,
} from '../cancel-reasons.types';

@Component({
    selector: 'app-edit-cancel-reason',
    templateUrl: './edit-cancel-reason.component.html',
    styleUrls: ['./edit-cancel-reason.component.scss'],
})
export class EditCancelReasonComponent implements OnInit {
    cancelReasonForm: FormGroup;
    cancelReason: CancelReason;

    constructor(
        private fb: FormBuilder,
        private dialogRef: MatDialogRef<EditCancelReasonComponent>,
        private _cancelReasonService: CancelReasonService,
        @Inject(MAT_DIALOG_DATA)
        public data: any
    ) {}

    ngOnInit(): void {
        this.cancelReason = this.data.cancelReason || {};
        this.cancelReasonForm = this.fb.group({
            reason: [
                this.cancelReason.reason ?? '',
                [
                    Validators.required,
                    Validators.maxLength(50),
                    Validators.minLength(5),
                ],
            ],
            description: [
                this.cancelReason.description ?? '',
                [Validators.maxLength(255)],
            ],
        });
    }

    save(): void {
        if (!this.cancelReasonForm.valid) {
            return;
        }

        if (!this.isEmptyObject(this.cancelReason)) {
            this.updateCancelReason();
        } else {
            this.createCancelReason();
        }
    }

    updateCancelReason(): void {
        const updatedCancelReason: UpdateCancelReason = {
            id: this.cancelReason.id,
            reason: this.cancelReasonForm.value.reason,
            description: this.cancelReasonForm.value.description,
        };

        this._cancelReasonService
            .updateCancelReason(updatedCancelReason)
            .subscribe(
                () => {
                    this.dialogRef.close(updatedCancelReason);
                },
                (error) => {
                    console.error(error);
                }
            );
    }

    createCancelReason(): void {
        const newCancelReason: CreateCancelReason = {
            reason: this.cancelReasonForm.value.reason,
            description: this.cancelReasonForm.value.description,
        };

        this._cancelReasonService.createCancelReason(newCancelReason).subscribe(
            () => {
                this.dialogRef.close(newCancelReason);
            },
            (error) => {
                console.error(error);
            }
        );
    }

    isEmptyObject(obj: any): boolean {
        return obj && Object.keys(obj).length === 0;
    }

    close(): void {
        this.dialogRef.close();
    }
}
