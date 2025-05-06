/* eslint-disable @typescript-eslint/naming-convention */
import { Injectable } from '@angular/core';
import { HttpClient, HttpStatusCode } from '@angular/common/http';
import { BehaviorSubject, EMPTY, Observable, of, throwError } from 'rxjs';
import { FuseSplashScreenService } from '@fuse/services/splash-screen';
import { catchError, switchMap, tap } from 'rxjs/operators';
import {
    EmailAddressListResponse,
    EmailForwardResponse,
    EmailListResponse,
    EmailReplyTemplateResponse,
    EmailResponse,
    FilteredEmailResponse,
    FilteredEmailWithAttachmentsResponse,
    MailboxesResponse,
    MostForwardedAddressesResponse,
    ReplyConcurrency,
    StatisticsResponse,
} from './filtering.types';
import { environment } from 'environments/environment';
import { Category } from '../common/common.types';
import moment from 'moment';
import {
    FilteredEmail,
    FilteredEmailDTOResponse,
} from 'app/shared/components/filtering-validate/details/details.types';
import { BaseResponse } from '../configurations/clients/clients.types';
import { GenericResponse } from '../configurations/products/products.types';

@Injectable({
    providedIn: 'root',
})
export class FilteringService {
    today = moment(new Date()).format('YYYY-MM-DD');
    tomorrow = moment(new Date()).add(1, 'day').format('YYYY-MM-DD');
    isLoading: boolean = false;

    // Private
    private _filteredEmails: BehaviorSubject<FilteredEmail[] | null> =
        new BehaviorSubject(null);

    /**
     * Constructor
     */
    constructor(
        private _httpClient: HttpClient,
        private _fuseSplashScreenService: FuseSplashScreenService
    ) {
        //this.getElements();
    }

    // -----------------------------------------------------------------------------------------------------
    // @ Accessors
    // -----------------------------------------------------------------------------------------------------

    /**
     * Getter for E-mails
     */
    get filteredEmails$(): Observable<FilteredEmail[]> {
        return this._filteredEmails.asObservable();
    }

    // -----------------------------------------------------------------------------------------------------
    // @ Public methods
    // -----------------------------------------------------------------------------------------------------

    /**
     * Get All E-mails
     */

    getElements(categoryId: number = 0): Observable<FilteredEmailResponse> {
        const params = {
            start_date: this.today,
            categoryId: categoryId === 0 ? '' : categoryId,
        };
        if (this.isLoading) {
            return EMPTY;
        }
        this.isLoading = true;
        return this._httpClient
            .get<FilteredEmailResponse>(
                environment.currrentBaseURL + '/api/filtering/filtered',
                { params }
            )
            .pipe(
                tap((response: FilteredEmailResponse) => {
                    this.isLoading = false;
                    this._filteredEmails.next(response.filteredEmails);
                }),
                catchError((failure) => {
                    this.isLoading = false;
                    return throwError(() => failure);
                })
            );
    }

    getElementsInInterval(
        startDate: string,
        endDate: string,
        categoryId: number = 0,
        statusId: number = 0
    ): Observable<any> {
        const params = {
            start_date: startDate,
            end_date: endDate,
            categoryId: categoryId === 0 ? '' : categoryId,
            statusId: statusId === 0 ? '' : statusId,
        };

        if (this.isLoading) {
            // Return an empty observable if loading is in progress
            return EMPTY;
        }

        this.isLoading = true;

        return this._httpClient
            .get<FilteredEmail[]>(
                environment.currrentBaseURL + '/api/filtering/filtered',
                { params }
            )
            .pipe(
                switchMap((response: any) => {
                    this.isLoading = false;
                    return of(response.filteredEmails);
                }),
                tap(() => {
                    this.isLoading = false;
                }),
                catchError((responseError) => {
                    this.isLoading = false;
                    return throwError(() => responseError);
                })
            );
    }

    getToValidade(
        statusId: number,
        categoryId: number,
        startDate: Date,
        endDate: Date
    ): Observable<FilteredEmail[]> {
        const params = {
            status_id: statusId,
            category_id: categoryId,
            start_date: moment(startDate).format('YYYY-MM-DD'),
            end_date: moment(endDate).format('YYYY-MM-DD'),
        };
        return this._httpClient
            .get<FilteredEmail[]>(
                environment.currrentBaseURL + '/api/filtering/validate',
                { params }
            )
            .pipe(
                switchMap((response: any) => of(response.filteredEmails)),
                tap(() => {
                    this._fuseSplashScreenService.hide();
                })
            );
    }

    getToValidadeOrders(
        categoryId: number,
        startDate: Date,
        endDate: Date
    ): Observable<FilteredEmail[]> {
        const params = {
            category_id: categoryId,
            // this makes the request cleaner, instead of sending 2025-01-01T00:00:00Z we send only the Date
            start_date: moment(startDate).format('YYYY-MM-DD'),
            end_date: moment(endDate).format('YYYY-MM-DD'),
        };
        return this._httpClient
            .get<FilteredEmail[]>(
                environment.currrentBaseURL + '/api/filtering/validate/orders',
                { params }
            )
            .pipe(
                switchMap((response: any) => of(response.filteredEmails)),
                tap(() => {
                    this._fuseSplashScreenService.hide();
                })
            );
    }

    getToValidadePendingClient(
        startDate: Date,
        endDate: Date
    ): Observable<FilteredEmail[]> {
        const params = {
            start_date: moment(startDate).format('YYYY-MM-DD'),
            end_date: moment(endDate).format('YYYY-MM-DD'),
        };
        return this._httpClient
            .get<FilteredEmail[]>(
                environment.currrentBaseURL +
                    '/api/filtering/validate/pending-client',
                { params }
            )
            .pipe(
                switchMap((response: any) => {
                    return of(response.filteredEmails);
                }),
                tap(() => {
                    this._fuseSplashScreenService.hide();
                })
            );
    }

    /**
     * Get Filtered Email by id
     */
    getFilteredEmailToValidateById(id: string): Observable<FilteredEmail> {
        return this._httpClient.get<FilteredEmail>(
            environment.currrentBaseURL + '/api/filtering/validate/' + id,
            {}
        );
    }

    /**
     * Get Filtered Email by id
     */
    getFilteredEmailById(token: string): Observable<FilteredEmail> {
        return this._httpClient.get<FilteredEmail>(
            environment.currrentBaseURL + '/api/filtering/' + token,
            {}
        );
    }

    /**
     * Get Filtered Email DTO by id with product DTO
     */
    getFilteredEmailDTOById(id: string): Observable<FilteredEmailDTOResponse> {
        this._fuseSplashScreenService.show();
        return this._httpClient.get<FilteredEmailDTOResponse>(
            environment.currrentBaseURL + '/api/filtering/validate/' + id,
            {}
        );
    }

    /**
     * Get Filtered Email DTO by id with product DTO
     */
    getFilteredEmailForCategorization(
        id: string
    ): Observable<FilteredEmailDTOResponse> {
        this._fuseSplashScreenService.show();
        return this._httpClient.get<FilteredEmailDTOResponse>(
            environment.currrentBaseURL +
                '/api/filtering/validate/category/' +
                id,
            {}
        );
    }

    /**
     * Get Filtered Email DTO by id with product DTO
     */
    getFilteredEmailDTOByTokenNoAuth(
        token: string
    ): Observable<FilteredEmailDTOResponse> {
        return this._httpClient.get<FilteredEmailDTOResponse>(
            environment.currrentBaseURL +
                '/api/filtering/validate/noAuth/' +
                token,
            {}
        );
    }

    /*
     * Get Filtered Email DTO by the order token with product DTO
     */
    getOrderByTokenNoAuth(
        orderToken: string
    ): Observable<FilteredEmailDTOResponse> {
        return this._httpClient.get<FilteredEmailDTOResponse>(
            environment.currrentBaseURL + '/api/orders/noAuth/' + orderToken,
            {}
        );
    }

    /**
     * Get Filtered Email by token
     */
    getFilteredEmailByToken(
        token: string
    ): Observable<FilteredEmailWithAttachmentsResponse> {
        return this._httpClient.get<FilteredEmailWithAttachmentsResponse>(
            environment.currrentBaseURL + '/api/filtering/' + token,
            {}
        );
    }

    /**
     * Get Configured Mailboxes
     */
    getConfiguredMailboxes(): Observable<MailboxesResponse> {
        return this._httpClient.get<MailboxesResponse>(
            environment.currrentBaseURL + '/api/emails/mailboxes',
            {}
        );
    }

    /**
     * Get Emails sent on the platform
     */
    getEmailsSentOnPlatform(
        startDate: string,
        endDate: string
    ): Observable<EmailListResponse> {
        const params = {
            start_date: startDate,
            end_date: endDate,
        };
        return this._httpClient.get<EmailListResponse>(
            environment.currrentBaseURL + '/api/emails/sent',
            { params }
        );
    }

    /**
     * Get Email details
     */
    getEmailDetails(emailId: string): Observable<EmailResponse> {
        return this._httpClient.get<EmailResponse>(
            environment.currrentBaseURL + `/api/emails/${emailId}`,
            {}
        );
    }

    /*
     * Get Most forwarded addresses
     */
    getMostForwardedEmailAddresses(): Observable<MostForwardedAddressesResponse> {
        return this._httpClient.get<MostForwardedAddressesResponse>(
            environment.currrentBaseURL + '/api/forwards/recipients',
            {}
        );
    }

    /**
     * Get All Categories
     */
    getCategories() {
        return this._httpClient
            .get<Category[]>(
                environment.currrentBaseURL + '/api/filtering/categories'
            )
            .pipe(
                switchMap((response: any) => {
                    return of(response.categories);
                })
            );
    }

    /*
        Categorize E-mail
    */
    categorizeEmail(id: string, categoryId: string): Observable<BaseResponse> {
        return this._httpClient.post<BaseResponse>(
            environment.currrentBaseURL + '/api/filtering/categorize/' + id,
            {
                category: categoryId,
            }
        );
    }

    /*
        Create Email
    */
    postCreateEmail(formData: FormData): Observable<any> {
        return this._httpClient.post<any>(
            environment.currrentBaseURL + '/api/emails',
            formData
        );
    }

    /*
        Mark email as resolved
    */
    changeEmailStatus(emailToken: string, statusId: number): any {
        return this._httpClient.patch<HttpStatusCode>(
            environment.currrentBaseURL + '/api/filtering/changeStatus',
            { status_id: statusId, email_token: emailToken }
        );
    }

    /*
        Mark email as confirmed by client no auth
    */
    changeEmailStatusToConfirmedByClientNoAuth(emailToken: string): any {
        const url: string =
            environment.currrentBaseURL +
            '/api/filtering/no-auth/change-status/' +
            emailToken;
        return this._httpClient.patch<HttpStatusCode>(url, {});
    }

    /*
        Change email destinatary
    */
    changeEmailDestinatary(emailId: string, newDestinatary: string): any {
        return this._httpClient.patch<HttpStatusCode>(
            environment.currrentBaseURL +
                '/api/emails/' +
                emailId +
                '/destinatary',
            { destinatary: newDestinatary }
        );
    }

    changeEmailCategory(
        id: string,
        categoryId: string,
        previousCategory: string
    ): any {
        return this._httpClient.post<HttpStatusCode>(
            environment.currrentBaseURL + '/api/filtering/changeCategory/' + id,
            {
                categoryId: categoryId,
                previousCategory: previousCategory,
            }
        );
    }

    /*
     * Mark email as spam
     */
    markEmailAsSpam(emailToken: string): Observable<BaseResponse> {
        return this._httpClient.patch<BaseResponse>(
            environment.currrentBaseURL +
                '/api/filtering/' +
                emailToken +
                '/mark-spam',
            {}
        );
    }

    /*
        Send Email Reply
    */
    postEmailReply(emailId: string, formData: FormData): any {
        return this._httpClient.post<HttpStatusCode>(
            environment.currrentBaseURL + `/api/reply/${emailId}`,
            formData
        );
    }

    /*
        Get Email Reply
    */
    getEmailReplies(emailToken: string): any {
        return this._httpClient.get<HttpStatusCode>(
            environment.currrentBaseURL + `/api/reply/${emailToken}`,
            {}
        );
    }

    /*
        Change Reply is_read status to true
        ex: Set reply as Read
    */
    setReplyToRead(replyToken: string): any {
        return this._httpClient.patch<HttpStatusCode>(
            environment.currrentBaseURL + `/api/reply/${replyToken}/setRead`,
            {}
        );
    }

    /*
        Generate reply with AI
     */

    getAIGeneratedReply(emailToken: string, isReplyToOriginal: string): any {
        return this._httpClient.get<HttpStatusCode>(
            environment.currrentBaseURL +
                `/api/reply/${emailToken}/generateResponseAI`,
            {
                params: {
                    isReplyToOriginal: isReplyToOriginal,
                },
            }
        );
    }

    /*
        Get Email Signature
    */
    getEmailSignature(): any {
        return this._httpClient.get<HttpStatusCode>(
            environment.currrentBaseURL + '/api/signature',
            {}
        );
    }

    /*
     * Get recipients
     */
    getEmailRecipientsList(): Observable<EmailAddressListResponse> {
        return this._httpClient.get<EmailAddressListResponse>(
            environment.currrentBaseURL + '/api/emails/recipients'
        );
    }

    /*
        Post Reply Concurrency Register
    */
    postStartEmailConcurrency(
        emailToken: string
    ): Observable<ReplyConcurrency> {
        return this._httpClient.post<ReplyConcurrency>(
            environment.currrentBaseURL +
                `/api/reply/${emailToken}/startConcurrency`,
            {}
        );
    }

    /*
        Post Reply Concurrency Removal
    */
    postStopEmailConcurrency(emailToken: string): any {
        return this._httpClient.post<HttpStatusCode>(
            environment.currrentBaseURL +
                `/api/reply/${emailToken}/stopConcurrency`,
            {}
        );
    }

    fwdEmail(
        token: string,
        emailList: string[],
        message?: string
    ): Observable<any> {
        return this._httpClient
            .post<HttpStatusCode>(
                environment.currrentBaseURL + '/api/forwards/' + token,
                { email_to_list: emailList, message: message }
            )
            .pipe(switchMap((response: any) => of(response)));
    }

    getFwdInfoByEmailToken(
        emailToken: string
    ): Observable<EmailForwardResponse> {
        return this._httpClient.get<EmailForwardResponse>(
            environment.currrentBaseURL + '/api/forwards/' + emailToken
        );
    }

    /**
     * Get Filtering Statistics
     */
    getStatistics(
        from?: string,
        to?: string,
        categoryId?: number
    ): Observable<StatisticsResponse> {
        const params = { from: from, to: to, categoryId: categoryId };
        return this._httpClient.get<StatisticsResponse>(
            environment.currrentBaseURL + '/api/filtering/stats',
            {
                params,
            }
        );
    }

    /*
     * Change filtered email status to administration pending
     */
    changeEmailStatusToAdminPending(emailToken: string): any {
        return this._httpClient.patch<HttpStatusCode>(
            environment.currrentBaseURL +
                `/api/filtering/${emailToken}/set-administration-pending`,
            {}
        );
    }

    /*
     * Get reply email template
     */
    getReplyEmailTemplate(
        orderToken: string
    ): Observable<EmailReplyTemplateResponse> {
        return this._httpClient.get<EmailReplyTemplateResponse>(
            environment.currrentBaseURL + `/api/reply/template/${orderToken}`,
            {}
        );
    }

    generateAuditEmail(emailBody: string): Observable<GenericResponse> {
        return this._httpClient.post<GenericResponse>(
            environment.currrentBaseURL + '/api/filtering/generate-audit-email',
            { body: emailBody }
        );
    }
}
