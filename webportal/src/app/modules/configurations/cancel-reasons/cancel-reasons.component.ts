import { Component, OnInit } from '@angular/core';
import { MatDialog, MatDialogConfig } from '@angular/material/dialog';
import { getMatIconFailedToSanitizeLiteralError } from '@angular/material/icon';
import { CancelReasonService } from './cancel-reasons.service';
import {
    CancelReason,
    CancelReasonsResponse,
    ChangeCancelReasonActiveStatus,
    UpdateCancelReason,
} from './cancel-reasons.types';
import { EditCancelReasonComponent } from './edit-cancel-reason/edit-cancel-reason.component';

@Component({
    selector: 'app-cancel-reasons',
    templateUrl: './cancel-reasons.component.html',
    styleUrls: ['./cancel-reasons.component.scss'],
})
export class CancelReasonsComponent implements OnInit {
    cancelReasons: CancelReason[] = [];
    isLoading: boolean = false;
    isLoadingActiveStates: { [id: string]: boolean } = {};

    constructor(
        private _cancelReasonService: CancelReasonService,
        private _matDialog: MatDialog
    ) {}

    ngOnInit(): void {
        this.isLoading = true;
        this._cancelReasonService
            .getAllCancelReasons()
            .subscribe((data: CancelReasonsResponse) => {
                this.cancelReasons = data.cancel_reasons;
                this.isLoading = false;
            });
    }

    editCancelReason(cancelReason: CancelReason): void {
        // open dialog as a popup of edit-cancel-reason component
        this.openCancelReasonDialog(cancelReason);
    }

    createCancelReason(): void {
        // open dialog as a popup of edit-cancel-reason component
        this.openCancelReasonDialog();
    }

    openCancelReasonDialog(cancelReason?: CancelReason): void {
        // Configure dialog with dynamic title and behavior
        const dialogConfig: MatDialogConfig = {
            maxHeight: '60vh',
            minHeight: '40vh',
            height: '50vh',
            maxWidth: '60vw',
            minWidth: '40vw',
            width: '40vh',
            data: {
                cancelReason: cancelReason || null, // Pass null for "create" mode
            },
        };

        // Open the dialog dynamically based on mode
        const dialogRef = this._matDialog.open(
            EditCancelReasonComponent,
            dialogConfig
        );

        dialogRef
            .afterClosed()
            .subscribe((result: CancelReason | UpdateCancelReason) => {
                if (result) {
                    // Refresh data after successful create or update
                    this.refreshData();
                }
            });
    }

    refreshData(): void {
        this.isLoading = true;
        this._cancelReasonService
            .getAllCancelReasons()
            .subscribe((data: CancelReasonsResponse) => {
                this.cancelReasons = data.cancel_reasons;
                this.isLoading = false;
            });
    }

    changeActiveStatus(cancelReason: CancelReason): void {
        this.isLoadingActiveStates[cancelReason.id] = true;
        const updatedCancelReason: ChangeCancelReasonActiveStatus = {
            id: cancelReason.id,
            is_active: !cancelReason.is_active,
        };

        this._cancelReasonService
            .changeCancelReasonActiveStatus(updatedCancelReason)
            .subscribe(
                (response) => {
                    const index = this.cancelReasons.findIndex(
                        (reason) => reason.id === cancelReason.id
                    );
                    this.cancelReasons[index].is_active =
                        !this.cancelReasons[index].is_active;
                    this.isLoadingActiveStates[cancelReason.id] = false;
                },
                (error) => {
                    console.error(error);
                    this.isLoadingActiveStates[cancelReason.id] = false;
                }
            );
    }

    public getHeaderStyle(): any {
        return {
            'background-color': '#383838',
            color: 'white',
            'font-weight': 'bold',
        };
    }

    public isSmallScreen(): boolean {
        return window.innerWidth < 768;
    }
}
