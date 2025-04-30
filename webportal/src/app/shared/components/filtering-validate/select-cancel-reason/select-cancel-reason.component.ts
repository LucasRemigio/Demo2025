import { ChangeDetectorRef, Component, Inject, OnInit } from '@angular/core';
import {
    FormBuilder,
    FormControl,
    FormGroup,
    Validators,
} from '@angular/forms';
import { MatDialogRef, MAT_DIALOG_DATA } from '@angular/material/dialog';
import { CancelReasonService } from 'app/modules/configurations/cancel-reasons/cancel-reasons.service';
import { CancelReason } from 'app/modules/configurations/cancel-reasons/cancel-reasons.types';
import { OrderService } from '../../confirm-order-address/order.service';

@Component({
    selector: 'app-select-cancel-reason',
    templateUrl: './select-cancel-reason.component.html',
    styleUrls: ['./select-cancel-reason.component.scss'],
})
export class SelectCancelReasonComponent implements OnInit {
    cancelReasons: CancelReason[] = [];
    selectCancelReasonForm: FormGroup;
    isLoading: boolean = false;

    flashMessage: 'success' | 'error' | null = null;
    flashMessageText: string | null = null;

    constructor(
        private _cancelReasonService: CancelReasonService,
        private _dialogRef: MatDialogRef<SelectCancelReasonComponent>,
        private _orderService: OrderService,
        private _changeDetectorRef: ChangeDetectorRef,
        private fb: FormBuilder,
        @Inject(MAT_DIALOG_DATA)
        public data: any
    ) {}

    ngOnInit(): void {
        this.isLoading = true;
        this._cancelReasonService
            .getActiveCancelReasons()
            .subscribe((cancelReasons) => {
                this.cancelReasons = cancelReasons.cancel_reasons;
                this.isLoading = false;
                this._changeDetectorRef.markForCheck();
            });

        // Create form for dropdown of cancel reason selection
        this.selectCancelReasonForm = this.fb.group({
            cancelReasonId: new FormControl('', Validators.required),
        });
    }

    save(): void {
        if (!this.selectCancelReasonForm.valid) {
            return;
        }

        const selectedCancelReasonId =
            this.selectCancelReasonForm.get('cancelReasonId').value;

        const orderToken = this.data.orderToken;

        this._orderService
            .cancelOrder(orderToken, selectedCancelReasonId)
            .subscribe(
                (response) => {
                    if (response.result_code < 0) {
                        this.showFlashMessage('error', response.message);
                        return;
                    }
                    // sucesso
                    this._dialogRef.close(selectedCancelReasonId);
                },
                (error) => {
                    this.showFlashMessage('error', error.message);
                }
            );
    }

    close(): void {
        this._dialogRef.close();
    }

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
