import { Component, Inject, OnInit } from '@angular/core';
import { MatDialogRef, MAT_DIALOG_DATA } from '@angular/material/dialog';

@Component({
    selector: 'app-generic-confirmation-popup',
    templateUrl: './generic-confirmation-popup.component.html',
    styleUrls: ['./generic-confirmation-popup.component.scss'],
})
export class GenericConfirmationPopupComponent implements OnInit {
    constructor(
        public dialogRef: MatDialogRef<GenericConfirmationPopupComponent>,
        @Inject(MAT_DIALOG_DATA) public data: { title: string; message: string }
    ) {}

    ngOnInit(): void {}

    close(): void {
        this.dialogRef.close(); // Close the dialog
    }

    onConfirm(): void {
        this.dialogRef.close(true); // Return true on confirm
    }

    onCancel(): void {
        this.dialogRef.close(false); // Return false on cancel
    }
}
