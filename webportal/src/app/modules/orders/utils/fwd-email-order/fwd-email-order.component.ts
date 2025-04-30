import {
    ChangeDetectorRef,
    Component,
    Inject,
    OnInit,
    ViewEncapsulation,
} from '@angular/core';
import { FormBuilder, FormGroup, Validators, Form } from '@angular/forms';
import { MAT_DIALOG_DATA, MatDialogRef } from '@angular/material/dialog';
import { FuseSplashScreenService } from '@fuse/services/splash-screen';
import { TranslocoService } from '@ngneat/transloco';
import { DropDownFilterSettings } from '@progress/kendo-angular-dropdowns';
import { OperatorService } from 'app/modules/operator/operator.service';
import { Operators } from 'app/modules/operator/operator.types';

@Component({
    selector: 'app-fwd-order',
    templateUrl: './fwd-email-order.component.html',
    encapsulation: ViewEncapsulation.None,
})
export class FwdOrderComponent implements OnInit {
    composeForm: FormGroup;
    flashMessage: 'success' | 'error' | null = null;
    flashMessageText: string;
    email: string;
    title: string = null;
    operators: Operators[] = [];
    selectedOperators: Operators = null;

    filterSettings: DropDownFilterSettings = {
        caseSensitive: false,
        operator: 'contains',
    };
    constructor(
        private _changeDetectorRef: ChangeDetectorRef,
        public matDialogRef: MatDialogRef<FwdOrderComponent>,
        private _formBuilder: FormBuilder,
        private readonly translocoService: TranslocoService,
        private _operatorService: OperatorService,
        private _fuseSplashScreen: FuseSplashScreenService,
        @Inject(MAT_DIALOG_DATA) public data: any
    ) {
        this.title = this.data.title;
    }

    ngOnInit() {
        this._fuseSplashScreen.show();
        this._operatorService.getOperators('1').subscribe((response: any) => {
            this.operators = response.operators;
            this.selectedOperators = this.operators[0];
            this._fuseSplashScreen.hide();
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
        // Close the dialog
        this.matDialogRef.close({
            email: this.selectedOperators.operator_email,
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

    valueChange(item: Operators) {
        this.selectedOperators = item;
    }
}
