/* eslint-disable arrow-parens */
/* eslint-disable @typescript-eslint/naming-convention */
import {
    ChangeDetectionStrategy,
    ChangeDetectorRef,
    Component,
    OnInit,
    ViewEncapsulation,
    OnDestroy,
} from '@angular/core';
import { FormControl, FormGroup } from '@angular/forms';
import { fuseAnimations } from '@fuse/animations';
import { Subject } from 'rxjs';
import { UserService } from 'app/core/user/user.service';
import {
    CategoryBaseUnit,
    SeriesLabelsContentArgs,
} from '@progress/kendo-angular-charts';
import moment from 'moment';
import { MatDatepickerInputEvent } from '@angular/material/datepicker';
import { MomentDateAdapter } from '@angular/material-moment-adapter';
import {
    DateAdapter,
    MAT_DATE_FORMATS,
    MAT_DATE_LOCALE,
} from '@angular/material/core';

import { saveAs } from 'file-saver';
import { FilteringService } from '../filtering.service';
import { Statistics, StatisticsResponse } from '../filtering.types';
import { isEmpty } from 'lodash';
import { FlashMessageService } from 'app/shared/components/flash-message/flash-message.service';
import { Category } from 'app/modules/common/common.types';
import { TranslocoService } from '@ngneat/transloco';

export const MY_FORMATS = {
    parse: {
        dateInput: 'LL',
    },
    display: {
        dateInput: 'YYYY-MM-DD',
        monthYearLabel: 'YYYY',
        dateA11yLabel: 'LL',
        monthYearA11yLabel: 'YYYY',
    },
};

@Component({
    selector: 'app-order-statistics',
    templateUrl: './statistics.component.html',
    styleUrls: ['./statistics.component.scss'],
    encapsulation: ViewEncapsulation.None,
    changeDetection: ChangeDetectionStrategy.Default,
    animations: fuseAnimations,
    providers: [
        {
            provide: DateAdapter,
            useClass: MomentDateAdapter,
            deps: [MAT_DATE_LOCALE],
        },

        { provide: MAT_DATE_FORMATS, useValue: MY_FORMATS },
    ],
})
export class StatisticsComponent implements OnInit, OnDestroy {
    securityForm: FormGroup;

    categories: Category[];
    selectedCategoryId: number = 0;

    isNoData: boolean = false;
    isLoading: boolean = false;
    isInitialLoading: boolean = false;

    _unsubscribeAll: Subject<any> = new Subject<any>();
    isAdmin: boolean = false;
    loading: boolean = false;
    dashboard: Statistics;
    index: number = 1;
    pieChartData: any[];
    histogramData: any[];
    resolvedData: any[];
    dateRangeForm: FormGroup;
    start_date = String(moment(new Date(2023, 0, 1)).format('YYYY-MM-DD'));
    end_date = String(moment(new Date()).format('YYYY-MM-DD'));

    dateOptions = [
        { value: '-1', label: 'Dashboard.today' },
        { value: '3', label: 'Dashboard.last3days' },
        { value: '7', label: 'Dashboard.last7days' },
        { value: '30', label: 'Dashboard.last30Days' },
        { value: '60', label: 'Dashboard.last60Days' },
        { value: '90', label: 'Dashboard.last90Days' },
        { value: '120', label: 'Dashboard.last120Days' },
        { value: '0', label: 'Dashboard.tudo' },
    ];

    categoryMapping = {
        encomendas: 'orders',
        pedidos: 'quotations',
        comprovativos: 'receipts',
        outros: 'others',
        erro: 'errors',
        duplicados: 'duplicates',
        certificados: 'certificates',
        spam: 'spams',
    };

    constructor(
        private _changeDetectorRef: ChangeDetectorRef,
        private _userService: UserService,
        private _filteringService: FilteringService,
        private _flashMessageService: FlashMessageService,
        private _translocoService: TranslocoService
    ) {
        this.isAdmin = this._userService.isAdmin();
        this.labelContent = this.labelContent.bind(this);

        this.dateRangeForm = new FormGroup({
            start: new FormControl(),
            end: new FormControl(),
        });
    }

    ngOnInit(): void {
        this.fetchCategories();
        this.fetchData(true);
    }

    ngOnDestroy(): void {
        // Unsubscribe from all subscriptions
        this._unsubscribeAll.next();
        this._unsubscribeAll.complete();
    }

    public labelContent(e: SeriesLabelsContentArgs): string {
        return e.category;
    }

    fetchCategories(): void {
        this._filteringService.getCategories().subscribe(
            (categories) => {
                this.categories = categories;
            },
            (error) => {
                this._flashMessageService.error('error-loading-categories');
            }
        );
    }

    fetchData(isFirstLoad: boolean = false): void {
        this.isLoading = true;
        if (isFirstLoad) {
            this.isInitialLoading = true;
        }
        this._filteringService
            .getStatistics(
                this.start_date,
                this.end_date,
                this.selectedCategoryId
            )
            .subscribe(
                (response: StatisticsResponse) => {
                    if (response.result_code < 0) {
                        this._flashMessageService.error(
                            'error-loading-statistics'
                        );
                        this.isNoData = true;
                        return;
                    }

                    const stats = response.statistics;
                    if (this.isResponseEmpty(stats)) {
                        this._flashMessageService.info(
                            'Home-Dashboard.No-data'
                        );
                        this.isNoData = true;
                        return;
                    }
                    this.isNoData = false;
                    this.dashboard = stats;
                    this.generateDashboard();
                    this._changeDetectorRef.markForCheck();
                },
                (error) => {
                    this.isNoData = true;
                    this._flashMessageService.error('error-loading-statistics');
                },
                () => {
                    this.isLoading = false;
                    if (isFirstLoad) {
                        this.isInitialLoading = false;
                    }
                }
            );
    }

    generateDashboard(): void {
        this.loading = true;
        const currentCategory = this.categories.find(
            (c) => c.id === this.selectedCategoryId
        );

        if (!this.selectedCategoryId) {
            this.pieChartData = this.getCategoryPieChartData();
        } else {
            this.pieChartData =
                this.getSelectedCategoryPieChartData(currentCategory);
        }

        if (!this.selectedCategoryId) {
            this.resolvedData = this.getResolvedPieChartData();
        } else {
            this.resolvedData = this.getResolvedPieChartDataByCategory();
        }

        this.histogramData = [
            {
                category: this._translocoService
                    .translate<string>('Filtering.total-to-validate')
                    .replace(':', '')
                    .trim(),
                value: this.dashboard.manual,
                color: '#4b08e7',
            },
            {
                category: this._translocoService.translate(
                    'Filtering.error-processing'
                ),
                value: this.dashboard.error,
                color: '#ca0f02',
            },
            {
                category: this._translocoService.translate(
                    'Filtering.low-confidence'
                ),
                value: this.dashboard.lowConfidence,
                color: '#4dabf3',
            },
        ];

        this.loading = false;
    }

    isSmallScreen(): boolean {
        return window.innerWidth <= 768;
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

    onCategoryChange(categoryId: number): void {
        this.selectedCategoryId = categoryId;
        this.fetchData();
    }

    onSelectionChange(event: any): void {
        const today = moment(new Date()).format('YYYY-MM-DD');
        let start_date: string;
        const end_date: string = today;

        switch (event.value) {
            case '-1':
                start_date = today;
                break;
            case '0':
                start_date = String(
                    moment(new Date(2023, 0, 1)).format('YYYY-MM-DD')
                );
                break;
            default:
                start_date = String(
                    moment(new Date())
                        .add(-event.value, 'days')
                        .format('YYYY-MM-DD')
                );
        }

        //update end date to tomorrow, since its last X  days
        this.start_date = start_date;
        this.end_date = end_date;

        this.fetchData();
    }

    isResponseEmpty(stats: Statistics): boolean {
        return stats.total === 0;
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
            const end_date = this.end_date;
            const start_date = this.start_date;

            // Check dates before trying to refresh
            if (this.isValidDate(end_date) && this.isValidDate(start_date)) {
                this.start_date = start_date;
                this.end_date = end_date;
                this.fetchData();
            } else {
                console.error('Invalid date');
            }
        }
    }

    isValidDate(date_string: string): boolean {
        return !isNaN(new Date(date_string).getTime()) || isEmpty(date_string);
    }

    private saveFile(base64String: string): void {
        const byteCharacters = atob(base64String);
        const byteNumbers = new Array(byteCharacters.length);

        for (let i = 0; i < byteCharacters.length; i += 1) {
            byteNumbers[i] = byteCharacters.charCodeAt(i);
        }

        const byteArray = new Uint8Array(byteNumbers);
        const blob = new Blob([byteArray], {
            type: 'application/vnd.openxmlformats-officedocument.spreadsheetml.sheet',
        });

        const fileName = 'dashboard.csv';

        saveAs(blob, fileName);
    }

    private getResolvedPieChartDataByCategory(): any[] {
        return [
            {
                kind: this._translocoService.translate('Dashboard.resolved'),
                share: this.dashboard.resolved,
                color: '#34DC23',
            },
            {
                kind: this._translocoService.translate(
                    'Dashboard.non-resolved'
                ),
                share: this.dashboard.unresolved,
                color: '#FF9800',
            },
        ];
    }

    private getResolvedPieChartData(): any[] {
        const unresolvedMinusQuotations =
            this.dashboard.unresolved - this.dashboard.unresolved_quotations;
        return [
            {
                kind: this._translocoService.translate('Dashboard.resolved'),
                share: this.dashboard.resolved,
                color: '#24DB00',
            },
            {
                kind: this._translocoService.translate(
                    'Dashboard.non-resolved'
                ),
                share: unresolvedMinusQuotations,
                color: '#DBA600',
            },
            {
                kind: this._translocoService.translate(
                    'Dashboard.unresolved-quotations'
                ),
                share: this.dashboard.unresolved_quotations,
                color: '#DCDC00',
            },
        ];
    }

    private getCategoryPieChartData(): any[] {
        return [
            {
                kind: this._translocoService.translate('Category.encomendas'),
                share: this.dashboard.orders,
                color: '#2EDB09',
            },
            {
                kind: this._translocoService.translate('Category.pedidos'),
                share: this.dashboard.quotations,
                color: '#096FDB',
            },
            {
                kind: this._translocoService.translate(
                    'Category.comprovativos'
                ),
                share: this.dashboard.receipts,
                color: '#DB8E08',
            },
            { kind: 'Outros', share: this.dashboard.others, color: '#B0B0B0' },
        ];
    }

    private getSelectedCategoryPieChartData(currentCategory: any): any[] {
        const currentCategoryTotal =
            this.dashboard[this.categoryMapping[currentCategory.slug]];

        const remaining =
            this.dashboard.total_only_dates - currentCategoryTotal;

        return [
            {
                kind: currentCategory.title,
                share: currentCategoryTotal,
                color: '#2EDB09',
            },
            {
                kind: this._translocoService.translate('remaining'),
                share: remaining,
                color: '#B0B0B0',
            },
        ];
    }
}
