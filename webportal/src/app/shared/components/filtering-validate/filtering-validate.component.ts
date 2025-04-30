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
import { MatSelectChange } from '@angular/material/select';
import { MatSlideToggleChange } from '@angular/material/slide-toggle';
import { BehaviorSubject, combineLatest, Subject } from 'rxjs';
import { CardCategory } from 'app/modules/orders/order.types';
import { UserService } from 'app/core/user/user.service';
import {
    EMAIL_CATEGORIES,
    EMAIL_STATUSES,
    Status,
} from 'app/modules/filtering/filtering.types';
import { FilteringService } from 'app/modules/filtering/filtering.service';
import { Category } from 'app/modules/common/common.types';
import { FuseSplashScreenService } from '@fuse/services/splash-screen';
import { environment } from 'environments/environment';
import { FlashMessageService } from '../flash-message/flash-message.service';
import { FilteredEmail } from './details/details.types';
import { translate } from '@ngneat/transloco';
import { FormControl, FormGroup } from '@angular/forms';
import {
    DateAdapter,
    MAT_DATE_FORMATS,
    MAT_DATE_LOCALE,
} from '@angular/material/core';
import { MomentDateAdapter } from '@angular/material-moment-adapter';
import { MY_FORMATS } from '../filtering-table/filtering-table.component';
import moment from 'moment';
import { MatDatepickerInputEvent } from '@angular/material/datepicker';
import { SwitchViewService, ViewType } from './switch-view/switch-view.service';
import { takeUntil } from 'rxjs/operators';

@Component({
    selector: 'shared-filtering-validate',
    templateUrl: './filtering-validate.component.html',
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
export class FilteringValidateComponent implements OnInit, OnDestroy {
    @Input() status: Status;
    @Input() category: Category = null;

    refreshTimeMs: number = 10 * 1000; // 10 seconds

    flashMessage: 'success' | 'error' | null = null;
    flashMessageText: string;
    isAdmin: boolean = false;
    isLoading: boolean = false;
    isUserSupervisor: boolean = false;

    startDate: Date;
    endDate: Date;
    dateRangeForm: FormGroup;

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

    categories: CardCategory[] = [];
    emails: FilteredEmail[] = [];
    filteredEmails: FilteredEmail[] = [];
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
        private _filteringService: FilteringService,
        private _userService: UserService,
        private _fuseSplashScreenService: FuseSplashScreenService,
        private _flashMessageService: FlashMessageService,
        private _switchViewService: SwitchViewService
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
        this.isUserSupervisor = this._userService.isSupervisor();

        // Get the categories
        this._filteringService
            .getCategories()
            .subscribe((categories: CardCategory[]) => {
                this.categories = categories;
                // Mark for check
                this._changeDetectorRef.markForCheck();
            });

        if (!this.category) {
            this.category = { id: 0 };
        }

        this._fuseSplashScreenService.show();

        // Get the data
        this.refreshData();

        // Automatically refresh data every x seconds
        this.refreshInterval = setInterval(() => {
            this.refreshData();
        }, environment.refreshTimeMs);

        // Filter the librarys
        combineLatest([this.filters.query$]).subscribe(([query]) => {
            // Reset the filtered librarys
            this.filteredEmails = this.emails;

            // Filter by search query
            if (query !== '') {
                this.filterEmailsByQuery(query);
            }
        });

        // Subscribe to the view type
        this._switchViewService.selectedViewType$
            .pipe(takeUntil(this._unsubscribeAll))
            .subscribe((view) => {
                this.viewType = view;
                this._changeDetectorRef.markForCheck();
            });

        this._switchViewService.setViewTypeVisibility(true);
    }

    filterEmailsByQuery(query: string): void {
        this.filteredEmails = this.filteredEmails.filter(
            (course) =>
                course.email.from.toLowerCase().includes(query.toLowerCase()) ||
                course.email.subject.toLowerCase().includes(query.toLowerCase())
        );
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
        this.isLoading = true;
        this.refreshData();
    }

    public getReasonClass(status: string): string {
        switch (status) {
            case EMAIL_STATUSES.AGUARDA_VALIDACAO.description:
                return 'text-blue-800 bg-blue-100 dark:text-blue-50 dark:bg-blue-500';
            case EMAIL_STATUSES.ERRO.description:
                return 'text-red-800 bg-red-100 dark:text-red-50 dark:bg-red-500';
            case EMAIL_STATUSES.PENDENTE_APROVACAO_ADMINISTRACAO.description:
                return 'text-gray-800 bg-gray-100 dark:text-gray-50 dark:bg-gray-500';
            case EMAIL_STATUSES.APROVADO_DIRECAO_COMERCIAL.description:
                return 'text-green-800 bg-green-100 dark:text-green-50 dark:bg-green-500';
            case EMAIL_STATUSES.PENDENTE_CONFIRMACAO_CLIENTE.description:
                return 'text-blue-800 bg-blue-100 dark:text-blue-50 dark:bg-blue-500';
            case EMAIL_STATUSES.PENDENTE_APROVACAO_CREDITO.description:
                return 'text-yellow-800 bg-yellow-100 dark:text-yellow-50 dark:bg-yellow-500';
            default:
                return '';
        }
    }

    public getStatusDescription(status: string): string {
        switch (status) {
            case EMAIL_STATUSES.ERRO.description:
                return translate('Filtering.error-processing');
            case EMAIL_STATUSES.PENDENTE_APROVACAO_ADMINISTRACAO.description:
                return translate('Filtering.pending-admin-approval');
            case EMAIL_STATUSES.APROVADO_DIRECAO_COMERCIAL.description:
                return translate('Filtering.approved-commercial-direction');
            case EMAIL_STATUSES.AGUARDA_VALIDACAO.description:
                return translate('Filtering.awaiting-approval');
            case EMAIL_STATUSES.PENDENTE_CONFIRMACAO_CLIENTE.description:
                return translate('Filtering.client-pending');
            default:
                return '';
        }
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

        this.refreshData();
    }

    refreshData(): void {
        let firstRequest: Promise<FilteredEmail[]>;

        if (this.category.id !== 0) {
            if (
                this.category.id === this.categoriesList.COTACOES.id &&
                this.activeView === 'clientPending'
            ) {
                firstRequest = this._filteringService
                    .getToValidadePendingClient(this.startDate, this.endDate)
                    .toPromise();
            } else {
                firstRequest = this._filteringService
                    .getToValidadeOrders(
                        this.category.id,
                        this.startDate,
                        this.endDate
                    )
                    .toPromise();
            }
        } else {
            // Create the first promise for the initial request
            firstRequest = this._filteringService
                .getToValidade(
                    this.status.id,
                    this.category.id,
                    this.startDate,
                    this.endDate
                )
                .toPromise();
        }

        const secondRequest = this.getSecondRequest();

        // Use Promise.all to wait for both requests to complete
        Promise.all([firstRequest, secondRequest])
            .then(([emails, otherEmails]) => {
                // Combine both email lists
                this.emails = [...emails, ...otherEmails];
                this.filteredEmails = this.emails;

                // Sort the emails by date
                this.emails.sort(
                    (a, b) =>
                        new Date(a.email.date).getTime() -
                        new Date(b.email.date).getTime()
                );

                // Mark for change detection
                this.isLoading = false;
                this._changeDetectorRef.markForCheck();

                // Hide splash screen
                this._fuseSplashScreenService.hide();
            })
            .catch((error) => {
                console.error(error);
                this._flashMessageService.error('error-loading-list');
            });
    }

    getSecondRequest(): Promise<FilteredEmail[]> {
        // Create the second promise for the additional request if the status is ERRO
        let secondRequest: Promise<FilteredEmail[]>;

        // this is the case when we are on the screen of filtering audit, where
        // we only want the emails with the category of error
        if (this.category.id === 0) {
            // Return an empty array wrapped in a promise if no request is needed
            return Promise.resolve([]);
        }

        if (
            this.category.id === this.categoriesList.COTACOES.id &&
            this.activeView === 'clientPending'
        ) {
            return Promise.resolve([]);
        }

        if (this.status.id === this.statusList.ERRO.id) {
            secondRequest = this._filteringService
                .getToValidade(
                    EMAIL_STATUSES.AGUARDA_VALIDACAO.id,
                    EMAIL_CATEGORIES.OUTROS.id,
                    this.startDate,
                    this.endDate
                )
                .toPromise();
        } else {
            // Return an empty array wrapped in a promise if no request is needed
            return Promise.resolve([]);
        }

        return secondRequest;
    }
}
