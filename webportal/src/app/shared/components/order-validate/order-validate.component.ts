/* eslint-disable arrow-parens */
import {
    ChangeDetectorRef,
    Component,
    Input,
    OnDestroy,
    OnInit,
} from '@angular/core';
import { FormGroup, FormControl } from '@angular/forms';
import { MatDatepickerInputEvent } from '@angular/material/datepicker';
import { ActivatedRoute, Router } from '@angular/router';
import { FuseSplashScreenService } from '@fuse/services/splash-screen';
import { translate } from '@ngneat/transloco';
import { UserService } from 'app/core/user/user.service';
import { Category } from 'app/modules/common/common.types';
import { FilteringService } from 'app/modules/filtering/filtering.service';
import {
    Status,
    EMAIL_STATUSES,
    EMAIL_CATEGORIES,
} from 'app/modules/filtering/filtering.types';
import { StatusItem } from 'app/modules/orders/order.types';
import { environment } from 'environments/environment';
import moment from 'moment';
import { BehaviorSubject, Subject, combineLatest } from 'rxjs';
import { takeUntil } from 'rxjs/operators';
import { OrderService } from '../confirm-order-address/order.service';
import { OrderDTO } from '../filtering-validate/details/details.types';
import {
    ViewType,
    SwitchViewService,
} from '../filtering-validate/switch-view/switch-view.service';
import { FlashMessageService } from '../flash-message/flash-message.service';

@Component({
    selector: 'app-order-validate',
    templateUrl: './order-validate.component.html',
    styleUrls: ['./order-validate.component.scss'],
})
export class OrderValidateComponent implements OnInit, OnDestroy {
    @Input() isDraft: boolean = false;
    @Input() isPendingAdminApproval: boolean = false;
    @Input() isPendingCreditApproval: boolean = false;

    category: Category;
    refreshTimeMs: number = 10 * 1000; // 10 seconds

    isAdmin: boolean = false;
    isLoading: boolean = false;
    isUserSupervisor: boolean = false;

    startDate: Date;
    endDate: Date;
    dateRangeForm: FormGroup;
    selectedPreset: string = '0'; // Default to "Hoje" (today)

    activeViewOptions = [
        {
            view: 'pendingValidation',
            translationKey: 'Filtering.pending-validation',
            extraClasses: 'rounded-r',
        },
        {
            view: 'clientPending',
            translationKey: 'Filtering.client-pending',
            extraClasses: 'rounded-l',
        },
    ];
    activeView: 'pendingValidation' | 'clientPending' = 'pendingValidation';

    viewType: ViewType = ViewType.card;

    statusList = EMAIL_STATUSES;
    categoriesList = EMAIL_CATEGORIES;

    orders: OrderDTO[] = [];
    filteredOrders: OrderDTO[] = [];

    filters: {
        query$: BehaviorSubject<string>;
    } = {
        query$: new BehaviorSubject(''),
    };

    private refreshInterval: any;
    private _unsubscribeAll: Subject<any> = new Subject<any>();

    /**
     * Constructor
     */
    constructor(
        private _changeDetectorRef: ChangeDetectorRef,
        private _orderService: OrderService,
        private _userService: UserService,
        private _fuseSplashScreenService: FuseSplashScreenService,
        private _flashMessageService: FlashMessageService,
        private _switchViewService: SwitchViewService,
        private _route: ActivatedRoute,
        private _router: Router
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
        this.fillCategory();

        this.isUserSupervisor = this._userService.isSupervisor();

        this._fuseSplashScreenService.show();

        // Read date parameters from URL using snapshot (much simpler)
        const params = this._route.snapshot.queryParams;

        // Handle preset date selection
        if (params['preset']) {
            this.onSelectionChange(params['preset']);
        }

        // Get the data
        this.refreshData();

        // Automatically refresh data every x seconds
        this.refreshInterval = setInterval(() => {
            this.refreshData();
        }, environment.refreshTimeMs);

        // Filter the librarys
        combineLatest([this.filters.query$]).subscribe(([query]) => {});

        // Subscribe to the view type
        this._switchViewService.selectedViewType$
            .pipe(takeUntil(this._unsubscribeAll))
            .subscribe((view) => {
                this.viewType = view;
                this._changeDetectorRef.markForCheck();
            });

        this._switchViewService.setViewTypeVisibility(true);
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

        this._switchViewService.setViewTypeVisibility(false);
    }

    // -----------------------------------------------------------------------------------------------------
    // @ Public methods
    // -----------------------------------------------------------------------------------------------------

    fillCategory(): void {
        if (this.isPendingAdminApproval) {
            return;
        }

        if (this.isDraft) {
            this.category = EMAIL_CATEGORIES.COTACOES;
        } else {
            this.category = EMAIL_CATEGORIES.ENCOMENDAS;
        }
    }

    /**
     * Filter by search query
     *
     * @param query
     */
    filterByQuery(query: string): void {
        this.filters.query$.next(query);
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

    setActiveView(view: 'pendingValidation' | 'clientPending'): void {
        this.activeView = view;
        this.refreshData();
    }

    public getReasonClass(status: StatusItem): string {
        switch (status.id) {
            case EMAIL_STATUSES.AGUARDA_VALIDACAO.id:
                return 'text-blue-800 bg-blue-100 dark:text-blue-50 dark:bg-blue-500';
            case EMAIL_STATUSES.ERRO.id:
                return 'text-red-800 bg-red-100 dark:text-red-50 dark:bg-red-500';
            case EMAIL_STATUSES.PENDENTE_APROVACAO_ADMINISTRACAO.id:
                return 'text-gray-800 bg-gray-100 dark:text-gray-50 dark:bg-gray-500';
            case EMAIL_STATUSES.APROVADO_DIRECAO_COMERCIAL.id:
                return 'text-green-800 bg-green-100 dark:text-green-50 dark:bg-green-500';
            case EMAIL_STATUSES.PENDENTE_CONFIRMACAO_CLIENTE.id:
                return 'text-blue-800 bg-blue-100 dark:text-blue-50 dark:bg-blue-500';
            case EMAIL_STATUSES.PENDENTE_APROVACAO_CREDITO.id:
                return 'text-yellow-800 bg-yellow-100 dark:text-yellow-50 dark:bg-yellow-500';
            default:
                return '';
        }
    }

    public getStatusDescription(status: StatusItem): string {
        switch (status.id) {
            case EMAIL_STATUSES.ERRO.id:
                return translate('Filtering.error-processing');
            case EMAIL_STATUSES.PENDENTE_APROVACAO_ADMINISTRACAO.id:
                return translate('Filtering.pending-admin-approval');
            case EMAIL_STATUSES.APROVADO_DIRECAO_COMERCIAL.id:
                return translate('Filtering.approved-commercial-direction');
            case EMAIL_STATUSES.AGUARDA_VALIDACAO.id:
                return translate('Filtering.awaiting-validation');
            case EMAIL_STATUSES.PENDENTE_CONFIRMACAO_CLIENTE.id:
                return translate('Filtering.client-pending');
            case EMAIL_STATUSES.PENDENTE_APROVACAO_CREDITO.id:
                return translate('Filtering.pending-credit-approval');
            default:
                return '';
        }
    }

    isEmailPendingAdminApproval(status: StatusItem): boolean {
        // This function will make the operator not able to approve the order if the email is in certain state
        // but supervisor can always click on that button so we return false because the button will not be disabled
        if (this.isUserSupervisor) {
            return false;
        }

        const adminStatuses = [
            EMAIL_STATUSES.PENDENTE_APROVACAO_ADMINISTRACAO.id,
            EMAIL_STATUSES.PENDENTE_APROVACAO_CREDITO.id,
        ];

        return adminStatuses.includes(status.id);
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

        this.refreshData();
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

        this.refreshData();
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

    refreshData(): void {
        if (this.isLoading) {
            return;
        }

        this.isLoading = true;

        const request: Promise<any> = this.getRequest();

        request
            .then((response) => {
                // Combine both email lists
                this.orders = response.orders;
                this.filteredOrders = response.orders;
            })
            .catch((error) => {
                console.error(error);
                this._flashMessageService.error('error-loading-list');
            })
            .finally(() => {
                // Hide splash screen
                this._fuseSplashScreenService.hide();
                this.isLoading = false;
                // Mark for change detection
                this._changeDetectorRef.markForCheck();
            });
    }

    getRequest(): Promise<any> {
        let request: Promise<any>;

        if (this.isPendingAdminApproval || this.isPendingCreditApproval) {
            request = this._orderService
                .getOrdersToValidate(
                    this.startDate,
                    this.endDate,
                    null,
                    this.isPendingAdminApproval,
                    this.isPendingCreditApproval
                )
                .toPromise();

            return request;
        }

        if (this.isDraft && this.activeView === 'clientPending') {
            request = this._orderService
                .getOrdersPendingClientConfirmation(
                    this.startDate,
                    this.endDate
                )
                .toPromise();

            return request;
        }

        request = this._orderService
            .getOrdersToValidate(this.startDate, this.endDate, this.isDraft)
            .toPromise();

        return request;
    }
}
