import {
    ChangeDetectionStrategy,
    ChangeDetectorRef,
    Component,
    Input,
    OnDestroy,
    OnInit,
    ViewEncapsulation,
} from '@angular/core';
import { Router } from '@angular/router';
import { BooleanInput } from '@angular/cdk/coercion';
import { Subject } from 'rxjs';
import { takeUntil } from 'rxjs/operators';
import { User } from 'app/core/user/user.types';
import { UserService } from 'app/core/user/user.service';
import { MatDialog } from '@angular/material/dialog';
import { ChangePasswordComponent } from './change-password/change-password.component';
import { Password } from 'app/core/user/user.types';
import { TranslocoService } from '@ngneat/transloco';
import { ChangeSignatureComponent } from './change-signature/change-signature.component';

@Component({
    selector: 'user',
    templateUrl: './user.component.html',
    encapsulation: ViewEncapsulation.None,
    changeDetection: ChangeDetectionStrategy.OnPush,
    exportAs: 'user',
})
export class UserComponent implements OnInit, OnDestroy {
    /* eslint-disable @typescript-eslint/naming-convention */
    static ngAcceptInputType_showAvatar: BooleanInput;
    /* eslint-enable @typescript-eslint/naming-convention */

    @Input() showAvatar: boolean = true;

    user: User;
    flashMessage: 'success' | 'error' | null = null;
    flashMessageText: string;

    private _unsubscribeAll: Subject<any> = new Subject<any>();

    /**
     * Constructor
     */
    constructor(
        private _changeDetectorRef: ChangeDetectorRef,
        private _router: Router,
        private _userService: UserService,
        private _matDialog: MatDialog,
        private translocoService: TranslocoService
    ) {}

    // -----------------------------------------------------------------------------------------------------
    // @ Lifecycle hooks
    // -----------------------------------------------------------------------------------------------------

    /**
     * On init
     */
    ngOnInit(): void {
        // Subscribe to user changes
        this._userService.user$
            .pipe(takeUntil(this._unsubscribeAll))
            .subscribe((user: User) => {
                this.user = user;

                // Mark for check
                this._changeDetectorRef.markForCheck();
            });
    }

    /**
     * On destroy
     */
    ngOnDestroy(): void {
        // Unsubscribe from all subscriptions
        this._unsubscribeAll.next();
        this._unsubscribeAll.complete();
    }

    // -----------------------------------------------------------------------------------------------------
    // @ Public methods
    // -----------------------------------------------------------------------------------------------------

    translate(key: string, params?: object): string {
        return this.translocoService.translate(key, params || {});
    }

    /**
     * Update the user status
     *
     * @param status
     */
    updateUserStatus(status: string): void {
        // Return if user is not available
        if (!this.user) {
            return;
        }

        // Update the user
        this._userService
            .update({
                ...this.user,
                status,
            })
            .subscribe();
    }

    /**
     * Sign out
     */
    signOut(): void {
        this._router.navigate(['/sign-out']);
    }

    changePassword(): void {
        // Open the dialog
        const dialogRef = this._matDialog.open(ChangePasswordComponent);

        /*  dialogRef.afterClosed()
          .subscribe((result) => {
            const email = this._userService.getLoggedUserEmail();
            if (result) {
              const newPassword: Password = { OldPass: btoa(result.OldPass), NewPass: btoa(result.NewPass), RepeatNewPass: btoa(result.RepeatNewPass), email: email };
              this._userService.updatePass(newPassword)
                .subscribe(
                  (response) => {
                    if(response && response.result_code > 0){
                        this.showFlashMessage('success', 'Password changed');
                      } else {
                        this.showFlashMessage('error', response.result);
                      }
                      this._changeDetectorRef.markForCheck();
                      },
                    (err) => {
                      this.showFlashMessage('error', 'Error changing password');
            
                      this._changeDetectorRef.markForCheck();
                    }
                );
            }
          }); */
    }

    changeSignature(): void {
        const dialogRef = this._matDialog.open(ChangeSignatureComponent);
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
}
