/* eslint-disable arrow-parens */
import {
    ChangeDetectorRef,
    Component,
    Inject,
    Input,
    OnInit,
    ViewEncapsulation,
} from '@angular/core';
import { FormBuilder, FormGroup, Validators, Form } from '@angular/forms';
import { MAT_DIALOG_DATA, MatDialogRef } from '@angular/material/dialog';
import { FuseSplashScreenService } from '@fuse/services/splash-screen';
import { TranslocoService } from '@ngneat/transloco';
import { ResizeBatchService } from '@progress/kendo-angular-common';
import { DropDownFilterSettings } from '@progress/kendo-angular-dropdowns';
import { PopupService } from '@progress/kendo-angular-popup';
import { AccountsManagmentService } from 'app/modules/accounts-managment/accounts-managment.service';
import { FwdUser } from 'app/modules/operator/operator.types';
import { FilteringService } from '../../../../modules/filtering/filtering.service';
import {
    EmailForward,
    EmailForwardResponse,
    FwdPopupData,
    MostForwardedAddressesResponse,
} from '../../../../modules/filtering/filtering.types';

@Component({
    selector: 'app-fwd-email',
    templateUrl: './fwd-email.component.html',
    styleUrls: ['./fwd-email.component.scss'],
    encapsulation: ViewEncapsulation.None,
    providers: [PopupService, ResizeBatchService],
})
export class FwdEmailComponent implements OnInit {
    composeForm: FormGroup;

    title = this.translocoService.translate('Order.fwd', {});

    flashMessage: 'success' | 'error' | null = null;
    flashMessageText: string;

    email: string;
    insertedEmailsToFwd: string[] = [];
    newUserEmail: string = '';

    emailForwardList: EmailForward[];
    message: string = '';

    mostForwardedAddresses: string[];
    filteredAddresses: string[] = [];

    filterSettings: DropDownFilterSettings = {
        caseSensitive: false,
        operator: 'contains',
    };
    constructor(
        private _changeDetectorRef: ChangeDetectorRef,
        public matDialogRef: MatDialogRef<FwdEmailComponent>,
        private readonly translocoService: TranslocoService,
        private _filteringService: FilteringService,

        @Inject(MAT_DIALOG_DATA) public data: FwdPopupData
    ) {}

    ngOnInit(): void {
        this._filteringService
            .getMostForwardedEmailAddresses()
            .subscribe((response: MostForwardedAddressesResponse) => {
                this.mostForwardedAddresses = response.email_forward_list;
                this.filteredAddresses = [...this.mostForwardedAddresses];
            });

        if (!this.data.isForwarded) {
            // No need to fetch for info
            return;
        }

        this._filteringService
            .getFwdInfoByEmailToken(this.data.token)
            .subscribe((response: EmailForwardResponse) => {
                this.emailForwardList = response.email_forward_list;
                this._changeDetectorRef.markForCheck();
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

    // -----------------------------------------------------------------------------------------------------
    // @ Public methods
    // -----------------------------------------------------------------------------------------------------

    /**
     * Add a user to the insertedEmailsToFwd list
     */
    addUser(userEmail?: string): void {
        if (userEmail) {
            this.newUserEmail = userEmail;
        }

        if (!this.newUserEmail) {
            return;
        }

        // Check if email is in valid format
        const emailRegex =
            /^(([^<>()[\]\.,;:\s@\"]+(\.[^<>()[\]\.,;:\s@\"]+)*)|(\".+\"))@(([^<>()[\]\.,;:\s@\"]+\.)+[^<>()[\]\.,;:\s@\"]{2,})$/i;
        if (!emailRegex.test(this.newUserEmail.trim())) {
            this.showFlashMessage(
                'error',
                this.translocoService.translate('email-address-not-valid')
            );
            return;
        }

        // Check if the email is already in the list
        const emailAlreadyInList = this.insertedEmailsToFwd.some(
            (email) => email === this.newUserEmail.trim()
        );

        if (emailAlreadyInList) {
            this.showFlashMessage(
                'error',
                this.translocoService.translate('email-address-not-valid')
            );
            return;
        }

        const newUser: string = this.newUserEmail.trim();
        // Add the new user to the insertedEmailsToFwd list
        this.insertedEmailsToFwd.push(newUser);
        // Clear the input field after adding
        this.newUserEmail = '';
        this._changeDetectorRef.markForCheck();
    }

    // Function to filter autocomplete options
    onInput(): void {
        if (this.newUserEmail) {
            const filterValue = this.newUserEmail.toLowerCase();
            this.filteredAddresses = this.mostForwardedAddresses.filter(
                (email) => email.toLowerCase().includes(filterValue)
            );
        } else {
            this.filteredAddresses = [...this.mostForwardedAddresses];
        }
    }

    /**
     * Remove a user from the insertedEmailsToFwd list
     */
    removeUser(user: string): void {
        this.insertedEmailsToFwd = this.insertedEmailsToFwd.filter(
            (email) => email !== user
        );
    }

    close(): void {
        // Close the dialog
        this.matDialogRef.close();
    }


    /**
     * Save and close the dialog with selected users
     */
    saveAndClose(): void {
        if (this.newUserEmail && this.newUserEmail.trim() !== '') {
            const userConfirmed = window.confirm(
                this.translocoService.translate(
                    'Filtering.forward-email-left-to-save'
                )
            );

            if (!userConfirmed) {
                // User chose to continue editing
                return;
            }
        }

        if (this.message.length > 1000) {
            this.showFlashMessage(
                'error',
                this.translocoService.translate('message-too-long')
            );
            return;
        }

        // Close the dialog and pass the selected users
        this.matDialogRef.close({
            insertedEmailsToFwd: this.insertedEmailsToFwd,
            message: this.message,
        });
    }
}
