/* eslint-disable quote-props */
import { Component, Input, OnChanges, OnInit } from '@angular/core';
import { DomSanitizer, SafeStyle } from '@angular/platform-browser';
import { translate } from '@ngneat/transloco';
import { PageChangeEvent } from '@progress/kendo-angular-grid';
import { CompositeFilterDescriptor } from '@progress/kendo-data-query';
import { UserService } from 'app/core/user/user.service';
import {
    EMAIL_CATEGORIES,
    EMAIL_STATUSES,
} from 'app/modules/filtering/filtering.types';
import { StatusItem } from 'app/modules/orders/order.types';
import { OrderDTO } from '../../filtering-validate/details/details.types';
import { FlashMessageService } from '../../flash-message/flash-message.service';

@Component({
    selector: 'app-order-validate-table',
    templateUrl: './order-validate-table.component.html',
    styleUrls: ['./order-validate-table.component.scss'],
})
export class OrderValidateTableComponent implements OnInit, OnChanges {
    @Input() orders: OrderDTO[] = [];
    @Input() isPendingApproval!: (status: StatusItem) => boolean;
    @Input() getReasonClass!: (status: StatusItem) => string;
    @Input() getStatusDescription!: (status: StatusItem) => string;

    filteredOrders: OrderDTO[] = [];
    isUserSupervisor: boolean = false;

    state: any = {
        skip: 0,
        take: 50,
        sort: [],
    };
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
    savedFilter: CompositeFilterDescriptor | null = null;

    localStorageName: string = 'validate-filter';

    constructor(
        private _fms: FlashMessageService,
        private sanitizer: DomSanitizer,
        private _userService: UserService
    ) {}

    /* ====================================
     *             LIFECYCLE METHODS
     * ==================================== */

    ngOnInit(): void {
        this.isUserSupervisor = this._userService.isSupervisor();

        this.filteredOrders = this.orders;

        this.savedFilter = this.getLocalStorageFilters();
    }

    ngOnChanges(): void {
        this.filteredOrders = this.orders;
    }

    /* ====================================
     *             PUBLIC METHODS
     * ==================================== */

    isSmallScreen(): boolean {
        return window.innerWidth < 768;
    }

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

    public onPageChange(e: PageChangeEvent): void {
        // Skip is the number of records to skip from the table
        this.state.skip = e.skip;
        // Take is the amount of records shown at each table page
        this.state.take = e.take;
    }

    public colorCode(code: string): SafeStyle {
        let result: string;

        switch (code) {
            case translate('OrderStatus.contact-requested', {}):
                result = '#ffe6cc';
                break;

            default:
                result = 'transparent';
                break;
        }

        return this.sanitizer.bypassSecurityTrustStyle(result);
    }

    public getHeaderStyle(): any {
        return {
            'background-color': '#383838',
            color: 'white',
            'font-weight': 'bold',
            height: 'auto',
            'vertical-align': 'middle',
            'text-align': 'center',
            'max-width': '150px',
        };
    }

    getOrderTypeDescription(order: OrderDTO): string {
        if (order.is_draft) {
            return EMAIL_CATEGORIES.COTACOES.title;
        }

        return EMAIL_CATEGORIES.ENCOMENDAS.title;
    }
}
