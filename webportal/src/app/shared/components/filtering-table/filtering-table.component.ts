/* eslint-disable arrow-parens */
/* eslint-disable @typescript-eslint/naming-convention */
import {
    ChangeDetectionStrategy,
    ChangeDetectorRef,
    Component,
    Input,
    OnChanges,
    OnDestroy,
    OnInit,
    ViewEncapsulation,
} from '@angular/core';
import { MatSelectChange } from '@angular/material/select';
import { MatSlideToggleChange } from '@angular/material/slide-toggle';
import { BehaviorSubject, Subject } from 'rxjs';
import { UserService } from 'app/core/user/user.service';
import { MatDialog } from '@angular/material/dialog';
import { PageChangeEvent } from '@progress/kendo-angular-grid';
import { TranslocoService } from '@ngneat/transloco';
import { DomSanitizer, SafeStyle } from '@angular/platform-browser';
import moment from 'moment';
import { MatDatepickerInputEvent } from '@angular/material/datepicker';
import { MomentDateAdapter } from '@angular/material-moment-adapter';
import {
    DateAdapter,
    MAT_DATE_FORMATS,
    MAT_DATE_LOCALE,
} from '@angular/material/core';
import { FormControl, FormGroup } from '@angular/forms';
import { isEmpty } from 'lodash';
import {
    EmailStatusUpdateWs,
    EMAIL_STATUSES,
    FilteredEmailResponse,
} from 'app/modules/filtering/filtering.types';
import { FilteringService } from 'app/modules/filtering/filtering.service';
import { Category } from 'app/modules/common/common.types';
import { FuseSplashScreenService } from '@fuse/services/splash-screen';
import { environment } from 'environments/environment';
import { FlashMessageService } from 'app/shared/components/flash-message/flash-message.service';
import { CompositeFilterDescriptor } from '@progress/kendo-data-query';
import {
    FilteredEmail,
    ResolvedStatus as ResolvedStatusEnum,
} from '../filtering-validate/details/details.types';
import { AuditHubService } from './audit-hub.service';
import { EmailActionsService } from './email-actions.service';

export const MY_FORMATS = {
    parse: {
        dateInput: 'LL',
    },
    display: {
        dateInput: 'DD/MM/YYYY',
        monthYearLabel: 'YYYY',
        dateA11yLabel: 'LL',
        monthYearA11yLabel: 'YYYY',
    },
};

@Component({
    selector: 'app-filtering-table',
    templateUrl: './filtering-table.component.html',
    styleUrls: ['./filtering-table.component.scss'],
    encapsulation: ViewEncapsulation.None,
    changeDetection: ChangeDetectionStrategy.OnPush,
    providers: [
        {
            provide: DateAdapter,
            useClass: MomentDateAdapter,
            deps: [MAT_DATE_LOCALE],
        },

        { provide: MAT_DATE_FORMATS, useValue: MY_FORMATS },
    ],
})
export class FilteringTableComponent implements OnInit, OnDestroy, OnChanges {
    @Input() tableCategory: Category;

    flashMessage: 'success' | 'error' | null = null;
    flashMessageText: string;
    isAdmin: boolean = false;
    isLoading: boolean = false;
    filteredEmailData: FilteredEmail;

    filteredMapByToken: Map<string, FilteredEmail> = new Map<
        string,
        FilteredEmail
    >();

    filteredEmails: FilteredEmail[];
    filteredFilteredEmails: FilteredEmail[];
    showNonResolvedOnly: boolean = false;

    filters: {
        categorySlug$: BehaviorSubject<string>;
        query$: BehaviorSubject<string>;
        hideCompleted$: BehaviorSubject<boolean>;
    } = {
        categorySlug$: new BehaviorSubject('all'),
        query$: new BehaviorSubject(''),
        hideCompleted$: new BehaviorSubject(false),
    };

    lblLancadaManualmente: string = this.translocoService.translate(
        'OrderStatus.manual-processed',
        {}
    );
    lblAguardaConfirmacao: string = this.translocoService.translate(
        'OrderStatus.waiting-response',
        {}
    );

    lblEnviada: string = this.translocoService.translate(
        'OrderStatus.sent',
        {}
    );

    start_date: string;
    end_date: string;
    dateRangeForm: FormGroup;
    statusId: number = 0;
    localStorageName: string = '{category}-audit_filter';

    state: any = {
        skip: 0,
        take: 50,
        sort: [],
    };
    savedFilter: CompositeFilterDescriptor | null = null;

    isLoadingEmailStatusChange: boolean = false;
    resolvedStatusEnums = ResolvedStatusEnum;

    public pagesizes = [
        {
            text: '10',
            value: 10,
        },
        {
            text: '25',
            value: 25,
        },
        {
            text: '50',
            value: 50,
        },
        {
            text: '100',
            value: 100,
        },
        {
            text: '150',
            value: 150,
        },
        {
            text: '200',
            value: 200,
        },
    ];

    private refreshInterval: any;
    private _unsubscribeAll: Subject<any> = new Subject<any>();

    /*
     * Constructor
     */
    constructor(
        private _changeDetectorRef: ChangeDetectorRef,
        private _filteringService: FilteringService,
        private _userService: UserService,
        private _matDialog: MatDialog,
        private translocoService: TranslocoService,
        private sanitizer: DomSanitizer,
        private _fuseSplashScreenService: FuseSplashScreenService,
        private _flashMessageService: FlashMessageService,
        private _auditHubWs: AuditHubService,
        private _emailActionsService: EmailActionsService
    ) {
        this.isAdmin = this._userService.isAdmin();

        this.start_date = String(moment(new Date()).format('YYYY-MM-DD'));
        this.end_date = String(
            moment(new Date()).add(1, 'day').format('YYYY-MM-DD')
        );

        this.dateRangeForm = new FormGroup({
            start: new FormControl(this.start_date),
            end: new FormControl(this.end_date),
        });
    }

    // -----------------------------------------------------------------------------------------------------
    // @ Lifecycle hooks
    // -----------------------------------------------------------------------------------------------------

    /**
     * On init
     */
    ngOnInit(): void {
        if (!this.tableCategory) {
            this.tableCategory = { id: 0 };
        }

        // get the stored filter from cache
        this.localStorageName = this.localStorageName.replace(
            '{category}',
            this.tableCategory.id.toString()
        );
        this.savedFilter = this.getLocalStorageFilters();

        this.subscribeToChanges();

        this._fuseSplashScreenService.show();
        this._filteringService.getElements(this.tableCategory.id).subscribe(
            (response: FilteredEmailResponse) => {
                this._fuseSplashScreenService.hide();
                // Fetch the emails within this date range
                this._filteringService.filteredEmails$.subscribe(
                    (filteredEmails: FilteredEmail[]) => {
                        this.filteredEmails = filteredEmails;
                        this.filteredFilteredEmails = filteredEmails;
                        this._changeDetectorRef.markForCheck();
                        this.filteredMapByToken =
                            this.mapEmailsByToken(filteredEmails);
                    }
                );

                // Automatically refresh data every x time
                this.refreshInterval = setInterval(() => {
                    this.refreshData();
                }, environment.refreshTimeMs);
            },
            (error) => {
                this._fuseSplashScreenService.hide();
            }
        );
    }

    subscribeToChanges(): void {
        this._auditHubWs.messageReceived$.subscribe((message) => {
            const payload: EmailStatusUpdateWs = JSON.parse(message);
            // get the filtered email
            const filteredEmail: FilteredEmail = this.filteredMapByToken.get(
                payload.email_token
            );
            filteredEmail.status = payload.status_description;
            this._changeDetectorRef.detectChanges();
        });
    }
    ngOnChanges(): void {}

    /**
     * On destroy
     */
    ngOnDestroy(): void {
        // Unsubscribe from all subscriptions
        this._unsubscribeAll.next();
        this._unsubscribeAll.complete();

        if (this.refreshInterval) {
            clearInterval(this.refreshInterval);
        }
    }

    // -----------------------------------------------------------------------------------------------------
    // @ Public methods
    // -----------------------------------------------------------------------------------------------------

    getLocalStorageFilters(): CompositeFilterDescriptor {
        const storedFilter = localStorage.getItem(this.localStorageName);
        if (!storedFilter) {
            return;
        }

        const parsedFilter = JSON.parse(storedFilter);

        if (parsedFilter.filters.length > 0) {
            this._flashMessageService.warning('warning-saved-filters');
        }

        return parsedFilter;
    }

    onFilterChanged(filter: CompositeFilterDescriptor): void {
        // Save the filter to localStorage
        localStorage.setItem(this.localStorageName, JSON.stringify(filter));
    }

    /**
     * Filter by search query
     *
     * @param query
     */
    filterByQuery(query: string): void {
        this.filters.query$.next(query);
    }

    isSmallScreen(): boolean {
        return window.innerWidth <= 768;
    }
    /**
     * Filter by category
     *
     * @param change
     */
    filterByCategory(change: MatSelectChange): void {
        this.filters.categorySlug$.next(change.value);
    }

    /**
     * Show/hide completed sources
     *
     * @param change
     */
    toggleCompleted(change: MatSlideToggleChange): void {
        this.filters.hideCompleted$.next(change.checked);
    }

    /**
     * Track by function for ngFor loops
     *
     * @param index
     * @param item
     */
    trackByFn(index: number, item: any): any {
        return item.id || index;
    }

    customResolvedSort = (a: any, b: any): number => {
        const aIsResolved = a.status === 'Resolvido Manualmente' ? 1 : 0;
        const bIsResolved = b.status === 'Resolvido Manualmente' ? 1 : 0;

        // Sort non-resolved first, then resolved
        if (aIsResolved !== bIsResolved) {
            return aIsResolved - bIsResolved;
        }

        // Fallback to default sorting by reply_count if both are in the same resolved category
        return a.reply_count - b.reply_count;
    };

    filterNonResolvedEmails(): void {
        if (this.showNonResolvedOnly) {
            this.statusId = EMAIL_STATUSES.RESOLVIDO_MANUALMENTE.id;
        } else {
            this.statusId = 0;
        }

        this.refreshData();
    }

    previewReplies(emailToken: string): void {
        this._emailActionsService.previewReplies(emailToken).subscribe(() => {
            this.refreshData();
        });
    }

    previewEmail(token: string, isToChangeCategory: boolean = false): void {
        this._emailActionsService
            .previewEmail(token, isToChangeCategory)
            .subscribe((result: any) => {
                if (!result || !isToChangeCategory) {
                    this.refreshData();
                    return;
                }

                this._emailActionsService
                    .categorize(
                        result.emailData.filteredEmail,
                        result.categoryId
                    )
                    .subscribe(() => {
                        this.refreshData();
                    });
            });
    }

    public colorCode(code: string): SafeStyle {
        let result;

        switch (code) {
            case this.translocoService.translate(
                'OrderStatus.contact-requested',
                {}
            ):
                result = '#ffe6cc';
                break;

            default:
                result = 'transparent';
                break;
        }

        return this.sanitizer.bypassSecurityTrustStyle(result);
    }

    public onPageChange(e: PageChangeEvent): void {
        // Skip is the number of records to skip from the table
        this.state.skip = e.skip;
        // Take is the amount of records shown at each table page
        this.state.take = e.take;
    }

    fwdEmail(filteredEmail: FilteredEmail): void {
        // Set loading state if needed.
        this.isLoading = true;
        this._emailActionsService.forwardEmail(filteredEmail).subscribe({
            next: (response) => {
                this.isLoading = false;
                // Refresh orders or perform additional actions.
                this.refreshData();
            },
            error: () => {
                this.isLoading = false;
            },
        });
    }

    changeEmailStatus(email: FilteredEmail): void {
        // Set loading state if needed.
        this.isLoading = true;
        // Find the order matching the given token.

        this._emailActionsService
            .changeEmailStatus(email.token, email)
            .subscribe({
                next: () => {
                    this.isLoading = false;
                    this.refreshData();
                },
                error: () => {
                    this.isLoading = false;
                },
            });
    }

    refreshData(): void {
        // If IsFoRefresh is filled, do not show the splash screen
        this.isLoading = true;
        this._changeDetectorRef.markForCheck();
        this._filteringService
            .getElementsInInterval(
                this.start_date,
                this.end_date,
                this.tableCategory.id,
                this.statusId
            )
            .subscribe(
                (filteredEmails: FilteredEmail[]) => {
                    this.filteredEmails = filteredEmails;
                    this.filteredFilteredEmails = filteredEmails;
                    this.isLoading = false;
                    this.goToValidPage();
                    this._changeDetectorRef.markForCheck();
                    this.filteredMapByToken = this.mapEmailsByToken(
                        this.filteredEmails
                    );
                },
                (error) => {
                    this.isLoading = false;
                }
            );
    }

    mapEmailsByToken(emails: FilteredEmail[]): Map<string, FilteredEmail> {
        const map = new Map<string, FilteredEmail>();
        emails.forEach((email) => {
            map.set(email.token, email);
        });
        return map;
    }

    goToValidPage(): void {
        if (!this.isCurrentPageValid) {
            // TODO: Maybe in the future, instead of navigating to last page,
            // Navigate to the page that has the email the user last interacted with
            // Maybe by saving the id in a variable and checking where in the array is that id
            // and with that index, calculate the page
            this.state.skip = this.getLastValidSkip;
        }
    }

    get getLastValidSkip(): number {
        let lastValidSkip = this.state.skip;
        const emailsLength = this.filteredEmails.length;

        // get the last valid skip divisible by the take
        // Note: this remainder calculation is essential because the pager only highlights
        // the current page if the skip is divisible by the take
        const amountToTake = emailsLength % this.state.take;
        lastValidSkip = emailsLength - amountToTake;

        return lastValidSkip;
    }

    get isCurrentPageValid(): boolean {
        // Check if current skip is outside the range of the filtered emails amount
        // Which send the user to an invalid page
        return this.state.skip < this.filteredEmails.length;
    }

    onSelectionChange(event: string): void {
        switch (event) {
            // Hoje
            case '0':
                this.start_date = String(
                    moment(new Date()).format('YYYY-MM-DD')
                );
                this.end_date = String(moment(new Date()).format('YYYY-MM-DD'));
                break;
            // Ontem e Hoje
            case '1':
                this.start_date = String(
                    moment(new Date()).add(-1, 'days').format('YYYY-MM-DD')
                );
                this.end_date = String(moment(new Date()).format('YYYY-MM-DD'));
                break;
            // Ontem
            case '2':
                this.start_date = String(
                    moment(new Date()).add(-1, 'days').format('YYYY-MM-DD')
                );
                this.end_date = String(
                    moment(new Date()).add(-1, 'days').format('YYYY-MM-DD')
                );
                break;
            // Ultimos 3 dias
            case '3':
                this.start_date = String(
                    moment(new Date()).add(-3, 'days').format('YYYY-MM-DD')
                );
                this.end_date = String(moment(new Date()).format('YYYY-MM-DD'));
                break;
            // Últimos 7 dias
            case '7':
                this.start_date = String(
                    moment(new Date()).add(-7, 'days').format('YYYY-MM-DD')
                );
                this.end_date = String(moment(new Date()).format('YYYY-MM-DD'));
                break;
            // Tudo
            case '-1':
                this.start_date = '';
                this.end_date = '';
                break;
            default:
                break;
        }

        // Check dates before trying to refresh
        if (
            this.isValidDate(this.end_date) &&
            this.isValidDate(this.start_date)
        ) {
            this.refreshData();
        } else {
            console.error('Invalid date');
        }
    }

    // This function will be called when first value of date range is set
    onDateRangeChangeStart(event: MatDatepickerInputEvent<Date>): void {
        this.start_date = moment(
            event.value.toISOString(),
            moment.ISO_8601,
            true
        ).format('YYYY-MM-DD');
    }

    // This function will be called when the last date value from range is chosen.
    onDateRangeChangeEnd(event: MatDatepickerInputEvent<Date>): void {
        if (event.value) {
            this.end_date = moment(
                event.value.toISOString(),
                moment.ISO_8601,
                true
            ).format('YYYY-MM-DD');

            // Check dates before trying to refresh
            if (
                this.isValidDate(this.end_date) &&
                this.isValidDate(this.start_date)
            ) {
                this.refreshData();
            } else {
                console.error('Invalid date');
            }
        }
    }

    isValidDate(date_string: string): boolean {
        return !isNaN(new Date(date_string).getTime()) || isEmpty(date_string);
    }

    emailResolvedStatus(email: FilteredEmail): ResolvedStatusEnum {
        if (!email) {
            return ResolvedStatusEnum.UNRESOLVED;
        }

        if (email.status === EMAIL_STATUSES.RESOLVIDO_MANUALMENTE.description) {
            return ResolvedStatusEnum.RESOLVED;
        }

        if (email.resolved_by && email.resolved_by !== '') {
            return ResolvedStatusEnum.RESOLVEDPREV;
        }

        return ResolvedStatusEnum.UNRESOLVED;
    }

    wasEmailResolved(email: FilteredEmail): boolean {
        return (
            this.emailResolvedStatus(email) !== ResolvedStatusEnum.UNRESOLVED
        );
    }

    getEmailResolvedTextColor(email: FilteredEmail): string {
        switch (this.emailResolvedStatus(email)) {
            case ResolvedStatusEnum.RESOLVED:
                return 'text-orange-600';
            case ResolvedStatusEnum.RESOLVEDPREV:
                return 'text-orange-100';
            case ResolvedStatusEnum.UNRESOLVED:
                return 'text-gray-300';
            default:
                return 'text-gray-300';
        }
    }
}
