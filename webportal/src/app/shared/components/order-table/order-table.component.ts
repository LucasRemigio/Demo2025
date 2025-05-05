/* eslint-disable quote-props */
/* eslint-disable arrow-parens */
import {
    ChangeDetectionStrategy,
    ChangeDetectorRef,
    Component,
    Input,
    OnDestroy,
    OnInit,
    ViewEncapsulation,
} from '@angular/core';
import { FormGroup, FormControl } from '@angular/forms';
import { MomentDateAdapter } from '@angular/material-moment-adapter';
import {
    DateAdapter,
    MAT_DATE_LOCALE,
    MAT_DATE_FORMATS,
} from '@angular/material/core';
import { MatDatepickerInputEvent } from '@angular/material/datepicker';
import { MatDialog, MatDialogConfig } from '@angular/material/dialog';
import { MatSelectChange } from '@angular/material/select';
import { MatSlideToggleChange } from '@angular/material/slide-toggle';
import { DomSanitizer, SafeStyle } from '@angular/platform-browser';
import { ActivatedRoute, Router } from '@angular/router';
import { FuseSplashScreenService } from '@fuse/services/splash-screen';
import { translate, TranslocoService } from '@ngneat/transloco';
import { PageChangeEvent } from '@progress/kendo-angular-grid';
import { CompositeFilterDescriptor } from '@progress/kendo-data-query';
import { UserService } from 'app/core/user/user.service';
import { Category } from 'app/modules/common/common.types';
import {
    EmailStatusUpdateWs,
    EMAIL_CATEGORIES,
    EMAIL_STATUSES,
    Status,
} from 'app/modules/filtering/filtering.types';
import { PendingClientConfirmationEnum } from 'app/modules/orders/order.types';
import moment from 'moment';
import { BehaviorSubject, Subject } from 'rxjs';
import { OrderService } from '../confirm-order-address/order.service';
import { AuditHubService } from '../filtering-table/audit-hub.service';
import { EmailActionsService } from '../filtering-table/email-actions.service';
import {
    FilteredEmail,
    Order,
    OrderDTO,
    ResolvedStatus,
} from '../filtering-validate/details/details.types';
import { FlashMessageService } from '../flash-message/flash-message.service';
import { GenericConfirmationPopupComponent } from '../generic-confirmation-popup/generic-confirmation-popup.component';

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
    selector: 'app-order-table',
    templateUrl: './order-table.component.html',
    styleUrls: ['./order-table.component.scss'],
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
export class OrderTableComponent implements OnInit, OnDestroy {
    @Input() isDraft: boolean = true;

    category: Category;

    isAdmin: boolean = false;
    isLoading: boolean = false;
    filteredEmailData: FilteredEmail;

    ordersMapByToken: Map<string, OrderDTO> = new Map<string, OrderDTO>();

    orders: OrderDTO[];
    filteredOrders: OrderDTO[];
    selectedOrder: OrderDTO;
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

    startDate: Date;
    endDate: Date;
    selectedPreset: string = '0'; // Default to "Hoje" (Today)
    dateRangeForm: FormGroup;
    statusId: number = 0;
    localStorageName: string = 'order-audit-filter';
    isLoadingOrderStatusChange: boolean = false;

    state: any = {
        skip: 0,
        take: 50,
        sort: [],
    };
    savedFilter: CompositeFilterDescriptor | null = null;

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
        private _cdr: ChangeDetectorRef,
        private _userService: UserService,
        private translocoService: TranslocoService,
        private sanitizer: DomSanitizer,
        private _fuseSplashScreenService: FuseSplashScreenService,
        private _fms: FlashMessageService,
        private _auditHubWs: AuditHubService,
        private _orderService: OrderService,
        private _emailActionsService: EmailActionsService,
        private _matDialog: MatDialog,
        private _router: Router,
        private _route: ActivatedRoute
    ) {
        this.isAdmin = this._userService.isAdmin();

        const today = new Date();
        this.startDate = today;
        this.endDate = today;

        this.dateRangeForm = new FormGroup({
            start: new FormControl(this.startDate),
            end: new FormControl(this.endDate),
        });
    }

    // -----------------------------------------------------------------------------------------------------
    // @ Lifecycle hooks
    // -----------------------------------------------------------------------------------------------------

    /**
     * On init
     */
    ngOnInit(): void {
        // get the stored filter from cache
        this.category = this.isDraft
            ? EMAIL_CATEGORIES.COTACOES
            : EMAIL_CATEGORIES.ENCOMENDAS;

        this._fuseSplashScreenService.show();
        this.savedFilter = this.getLocalStorageFilters();

        this.subscribeToChanges();

        // Read date parameters from URL using snapshot (much simpler)
        const params = this._route.snapshot.queryParams;

        // Handle preset date selection
        if (params['preset']) {
            this.onSelectionChange(params['preset']);
        }

        this.fetchOrders();
    }

    fetchOrders(): void {
        this.isLoading = true;
        this._orderService
            .getAuditOrders(this.isDraft, this.startDate, this.endDate)
            .subscribe(
                (response) => {
                    if (response.result_code <= 0) {
                        this._fms.error('error-loading-list');
                        return;
                    }
                    this.orders = response.orders;
                    this.filteredOrders = response.orders;
                    this.ordersMapByToken = this.mapEmailsByToken(
                        response.orders
                    );
                },
                (error) => {
                    this._fms.error('error-loading-list');
                },
                () => {
                    this._fuseSplashScreenService.hide();
                    this.isLoading = false;
                    this._cdr.markForCheck();
                }
            );
    }

    subscribeToChanges(): void {
        this._auditHubWs.messageReceived$.subscribe((message) => {
            const payload: EmailStatusUpdateWs = JSON.parse(message);
            // get the filtered email
            const order: OrderDTO = this.ordersMapByToken.get(
                payload.email_token
            );
            // update the status
            this._cdr.detectChanges();
        });
    }

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
            this._fms.warning('warning-saved-filters');
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

        this.fetchOrders();
    }

    previewReplies(emailToken: string): void {
        this._emailActionsService.previewReplies(emailToken).subscribe(() => {
            this.fetchOrders();
        });
    }

    previewEmail(token: string, isToChangeCategory: boolean = false): void {
        this._emailActionsService
            .previewEmail(token, isToChangeCategory)
            .subscribe(
                (result: any) => {
                    if (!result || !isToChangeCategory) {
                        return;
                    }

                    this._emailActionsService
                        .categorize(this.filteredEmailData, result.categoryId)
                        .subscribe(() => {});
                },
                (error) => {},
                () => {
                    this.fetchOrders();
                }
            );
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
                this.fetchOrders();
            },
            error: () => {
                this.isLoading = false;
            },
        });
    }

    mapEmailsByToken(orders: OrderDTO[]): Map<string, OrderDTO> {
        const map = new Map<string, OrderDTO>();
        orders.forEach((email) => {
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
        const emailsLength = this.orders.length;

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
        return this.state.skip < this.orders.length;
    }

    onSelectionChange(event: string): void {
        this.selectedPreset = event;
        const today = new Date();
        const yesterday = moment(today).subtract(1, 'days').toDate();

        switch (event) {
            // Hoje (today)
            case '0':
                this.startDate = today;
                this.endDate = today;
                break;
            // Ontem e Hoje (yesterday and today)
            case '1':
                this.startDate = yesterday;
                this.endDate = today;
                break;
            // Ontem (yesterday)
            case '2':
                this.startDate = yesterday;
                this.endDate = yesterday;
                break;
            // Últimos 3 dias (last 3 days)
            case '3':
                this.startDate = moment(today).subtract(3, 'days').toDate();
                this.endDate = today;
                break;
            // Últimos 7 dias (last 7 days)
            case '7':
                this.startDate = moment(today).subtract(7, 'days').toDate();
                this.endDate = today;
                break;
            // Tudo (all)
            case '-1':
                this.startDate = new Date(0);
                this.endDate = today;
                break;
            default:
                break;
        }

        // Update the form with the new values
        this.dateRangeForm.patchValue({
            start: this.startDate,
            end: this.endDate,
        });

        this.updateUrlParams();

        this.fetchOrders();
    }

    updateUrlParams(): void {
        // Create the query params object
        const queryParams: any = {
            preset: this.selectedPreset,
        };

        // Update the URL with the new parameters without navigating
        this._router.navigate([], {
            relativeTo: this._route,
            queryParams: queryParams,
            queryParamsHandling: 'merge', // Keep other query params
            replaceUrl: true, // Don't add to browser history
        });
    }

    // This function will be called when first value of date range is set
    onDateRangeChangeStart(event: MatDatepickerInputEvent<Date>): void {
        this.startDate = event.value;
    }

    // This function will be called when the last date value from range is chosen.
    onDateRangeChangeEnd(event: MatDatepickerInputEvent<Date>): void {
        if (!event.value) {
            return;
        }

        this.endDate = event.value;

        this.fetchOrders();
    }

    get headerStyle(): { [key: string]: string } {
        return {
            'background-color': '#383838',
            color: 'white',
            'font-weight': 'bold',
        };
    }

    getPendingClientColor(order: OrderDTO): string {
        const status: PendingClientConfirmationEnum =
            this.getOrderPendingClientStatus(order);

        switch (status) {
            case PendingClientConfirmationEnum.CONFIRMED:
                return 'text-green-600';
            case PendingClientConfirmationEnum.PENDING:
                return 'text-orange-600';
            case PendingClientConfirmationEnum.NOT_PENDING:
                return 'text-gray-300';
            default:
                return 'text-gray-300';
        }
    }

    getOrderPendingClientStatus(
        order: OrderDTO
    ): PendingClientConfirmationEnum {
        if (order.is_adjudicated) {
            return PendingClientConfirmationEnum.CONFIRMED;
        }

        if (
            order.status.id === EMAIL_STATUSES.PENDENTE_CONFIRMACAO_CLIENTE.id
        ) {
            return PendingClientConfirmationEnum.PENDING;
        }

        return PendingClientConfirmationEnum.NOT_PENDING;
    }

    getPendingOrderPendingClientMatTooltip(order: OrderDTO): string {
        const status: PendingClientConfirmationEnum =
            this.getOrderPendingClientStatus(order);

        switch (status) {
            case PendingClientConfirmationEnum.CONFIRMED:
                return translate('client-adjudicated');
            case PendingClientConfirmationEnum.PENDING:
                return translate('pending-client-confirmation');
            case PendingClientConfirmationEnum.NOT_PENDING:
                return translate('pending-validation');
            default:
                return translate('pending-validation');
        }
    }

    /* --------------------------------------------------------------------------
     *   Manually Resolved
     * --------------------------------------------------------------------------
     */

    manuallyResolve(order: OrderDTO): void {
        this.isLoadingOrderStatusChange = true;
        this._cdr.detectChanges();

        this._orderService.toggleResolved(order.token).subscribe(
            (response: any) => {
                if (response.result_code < 0) {
                    this._fms.error('email-status-change-error');
                    return;
                }

                this.setResolvedStatus(order);
                this.fetchOrders();

                this._fms.success('email-status-change-success');
            },
            (error) => {
                this._fms.error('email-status-change-error');
            },
            () => {
                this.isLoadingOrderStatusChange = false;
                // change the order status in place
                this._cdr.markForCheck();
            }
        );
    }

    setResolvedStatus(order: OrderDTO): void {
        // Update the order status directly on the reference
        if (this.isOrderResolved(order)) {
            // clear the fields
            order.resolved_by = null;
            order.resolved_at = null;
            return;
        }

        // fill in the fields
        order.resolved_by = this._userService.getLoggedUserEmail() || 'System';
        order.resolved_at = new Date();
    }

    isOrderResolved(order: OrderDTO): boolean {
        if (!order) {
            return false;
        }

        if (order.resolved_by && order.resolved_by !== '') {
            return true;
        }

        return false;
    }

    getOrderResolvedTextColor(order: OrderDTO): string {
        if (this.isOrderResolved(order)) {
            return 'text-orange-600';
        }

        return 'text-gray-300';
    }
}
