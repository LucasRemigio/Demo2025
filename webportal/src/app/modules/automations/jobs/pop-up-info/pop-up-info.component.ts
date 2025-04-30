import { ChangeDetectorRef, Component, Inject, OnInit, ViewEncapsulation } from '@angular/core';
import { MAT_DIALOG_DATA, MatDialogRef } from '@angular/material/dialog';

@Component({
    selector: 'app-pop-up-info',
    templateUrl: './pop-up-info.component.html',
    encapsulation: ViewEncapsulation.None,
})
export class PopUpInfo implements OnInit {
    jobDetails: string;
    flashMessage: 'success' | 'error' | null = null;
    flashMessageText: string;

    constructor(
        private _changeDetectorRef: ChangeDetectorRef,
        public matDialogRef: MatDialogRef<PopUpInfo>,
        @Inject(MAT_DIALOG_DATA) public data: any
    ) {
    }
    ngOnInit(): void {
    
        if  (this.data.jobs.job_Details) { 
            
            this.jobDetails = this.data.jobs.job_Details;
        } 
    }

    close(): void {
        // Close the dialog
        this.matDialogRef.close();
    }
}