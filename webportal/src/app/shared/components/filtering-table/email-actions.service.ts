import { Injectable } from '@angular/core';
import { MatDialog, MatDialogConfig } from '@angular/material/dialog';
import { FuseSplashScreenService } from '@fuse/services/splash-screen';
import { FilteringService } from 'app/modules/filtering/filtering.service';
import { EMAIL_STATUSES } from 'app/modules/filtering/filtering.types';
import { Observable, of, throwError } from 'rxjs';
import { switchMap, catchError, tap, filter, map } from 'rxjs/operators';
import { OrderService } from '../confirm-order-address/order.service';
import {
    FilteredEmail,
    OrderDTO,
} from '../filtering-validate/details/details.types';
import { FlashMessageService } from '../flash-message/flash-message.service';
import { FwdEmailComponent } from './fwd-email/fwd-email.component';
import { PreviewEmailComponent } from './preview-email/preview-email.component';
import { PreviewRepliesComponent } from './preview-replies/preview-replies.component';

@Injectable({
    providedIn: 'root',
})
export class EmailActionsService {
    constructor(
        private _fsss: FuseSplashScreenService,
        private _fs: FilteringService,
        private _md: MatDialog,
        private _fms: FlashMessageService,
        private _os: OrderService
    ) {}

    previewEmail(
        token: string,
        isToChangeCategory: boolean = false
    ): Observable<any> {
        this._fsss.show();

        return this._fs.getFilteredEmailById(token).pipe(
            tap((response) => {
                // Hide splash screen as soon as we have the response.
                this._fsss.hide();
            }),
            switchMap((response: any) => {
                if (!response || !response.filteredEmail) {
                    // If no valid email data, return an observable of null.
                    return of(null);
                }

                const emailData = response;
                // If needed later by a categorization method, you can return the filtered email data as part of the dialog result.
                emailData.isToChangeCategory = isToChangeCategory;
                const dialogConfig: MatDialogConfig = {
                    maxHeight: '160vh',
                    maxWidth: '100vw',
                    data: emailData,
                };
                const dialogRef = this._md.open(
                    PreviewEmailComponent,
                    dialogConfig
                );

                // After the dialog is closed, combine its result with the emailData.
                return dialogRef.afterClosed().pipe(
                    map((dialogResult: any) => ({
                        ...dialogResult,
                        emailData,
                    }))
                );
            }),
            catchError((error) => {
                this._fsss.hide();
                return throwError(error);
            })
        );
    }

    /***
     * Changes the category of an email.
     * @param emailData The email data containing the email id and current category.
     * @param categoryId The ID of the new category.
     * @returns An observable of the categorization response.
     */
    categorize(emailData: FilteredEmail, categoryId: string): Observable<any> {
        return this._fs
            .changeEmailCategory(
                emailData.email.id,
                categoryId,
                emailData.category
            )
            .pipe(
                tap((response: any) => {
                    if (response.result_code <= 0) {
                        this._fms.error('email-categorized-error');
                        return;
                    }

                    this._fms.success('email-categorized-success');
                }),
                catchError((error) => {
                    this._fms.error('email-categorized-error');
                    return throwError(error);
                })
            );
    }

    /***
     * Opens the preview replies dialog.
     * @param emailToken The email token used to retrieve replies.
     * @returns An observable that emits when the dialog is closed.
     */
    previewReplies(emailToken: string): Observable<any> {
        const dialogConfig: MatDialogConfig = {
            maxHeight: '160vh',
            maxWidth: '80vw',
            minWidth: '60vw',
            width: '60vh',
            data: {
                emailToken: emailToken,
            },
        };

        const dialogRef = this._md.open(PreviewRepliesComponent, dialogConfig);
        return dialogRef.afterClosed();
    }

    /***
     * Opens the forward email dialog and executes the forward API call upon dialog close.
     * @param filteredEmail The email data that is used to open the dialog and perform the forward action.
     * @returns An observable that emits the API response.
     */
    forwardEmail(filteredEmail: FilteredEmail): Observable<any> {
        const dialogConfig: MatDialogConfig = {
            maxHeight: '90vh',
            minWidth: '50vh',
            data: {
                token: filteredEmail.token,
                isForwarded: filteredEmail.forwarded_by !== '',
            },
        };

        const dialogRef = this._md.open(FwdEmailComponent, dialogConfig);

        return dialogRef.afterClosed().pipe(
            // Only proceed if the dialog returns a truthy result.
            filter(
                (
                    result:
                        | { insertedEmailsToFwd: string[]; message: string }
                        | undefined
                ) => !!result
            ),
            switchMap(
                (result: { insertedEmailsToFwd: string[]; message: string }) =>
                    this._fs.fwdEmail(
                        filteredEmail.token,
                        result.insertedEmailsToFwd,
                        result.message
                    )
            ),
            tap((response: any) => {
                if (!response || response.result_code <= 0) {
                    this._fms.error('forward-error');
                } else {
                    this._fms.success('forward-success');
                }
            }),
            catchError((err) => {
                this._fms.error('forward-error');
                return throwError(err);
            })
        );
    }

    /***
     * Changes the status of an email based on its current status.
     * @param token The token identifying the email.
     * @param email The current order/email data (which includes its status).
     * @returns An observable that emits the API response.
     */
    changeEmailStatus(token: string, email: FilteredEmail): Observable<any> {
        // Determine the new status based on the current status.
        let newStatus = EMAIL_STATUSES.RESOLVIDO_MANUALMENTE;
        if (
            email.status === EMAIL_STATUSES.TRIAGEM_REALIZADA.description ||
            email.status === EMAIL_STATUSES.AGUARDA_VALIDACAO.description
        ) {
            newStatus = EMAIL_STATUSES.RESOLVIDO_MANUALMENTE;
        } else if (
            email.status === EMAIL_STATUSES.RESOLVIDO_MANUALMENTE.description ||
            email.status === EMAIL_STATUSES.TRIAGEM_REALIZADA.description
        ) {
            newStatus = EMAIL_STATUSES.TRIAGEM_REALIZADA;
        }

        return this._fs.changeEmailStatus(token, newStatus.id).pipe(
            tap((response: any) => {
                if (response.result_code <= 0) {
                    this._fms.error('email-status-change-error');
                } else {
                    this._fms.success('email-status-change-success');
                }
            }),
            catchError((error) => {
                this._fms.error('email-status-change-error');
                return throwError(error);
            })
        );
    }

    /***
     * Changes the status of an email based on its current status.
     * @param token The token identifying the email.
     * @param order The current order/email data (which includes its status).
     * @returns An observable that emits the API response.
     */
    changeOrderStatus(token: string, order: OrderDTO): Observable<any> {
        // Determine the new status based on the current status.
        let newStatus = EMAIL_STATUSES.RESOLVIDO_MANUALMENTE;

        if (order.status.id === EMAIL_STATUSES.RESOLVIDO_MANUALMENTE.id) {
            newStatus = EMAIL_STATUSES.AGUARDA_VALIDACAO;
        }

        return this._os.patchStatus(token, newStatus.id).pipe(
            tap((response: any) => {
                if (response.result_code <= 0) {
                    this._fms.error('email-status-change-error');
                } else {
                    this._fms.success('email-status-change-success');
                }
            }),
            catchError((error) => {
                this._fms.error('email-status-change-error');
                return throwError(error);
            })
        );
    }
}
