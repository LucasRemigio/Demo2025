import { ChangeDetectorRef, Component, OnInit } from '@angular/core';

import { PageChangeEvent } from '@progress/kendo-angular-grid';
import { MatDialog, MatDialogConfig } from '@angular/material/dialog';
import moment from 'moment';
import { MatDatepickerInputEvent } from '@angular/material/datepicker';
import { FormControl, FormGroup } from '@angular/forms';
import { isEmpty } from 'lodash';
import {
    EmailListResponse,
} from 'app/modules/filtering/filtering.types';
import { FilteringService } from 'app/modules/filtering/filtering.service';
import { TranslocoService } from '@ngneat/transloco';
import { FwdEmailComponent } from 'app/shared/components/filtering-table/fwd-email/fwd-email.component';
import { PreviewCommunicationsEmailComponent } from './preview-email/preview-email.component';
import { Email } from 'app/modules/common/common.types';
import { MomentDateAdapter } from '@angular/material-moment-adapter';
import {
    DateAdapter,
    MAT_DATE_LOCALE,
    MAT_DATE_FORMATS,
} from '@angular/material/core';


export const MY_FORMATS = {
    parse: {
        dateInput: 'LL',
    },
    display: {
        dateInput: 'DD-MM-YYYY',
        monthYearLabel: 'YYYY',
        dateA11yLabel: 'LL',
        monthYearA11yLabel: 'YYYY',
    },
};

@Component({
    selector: 'app-audit',
    templateUrl: './audit.component.html',
    styleUrls: ['./audit.component.scss'],
    providers: [
        {
            provide: DateAdapter,
            useClass: MomentDateAdapter,
            deps: [MAT_DATE_LOCALE],
        },

        { provide: MAT_DATE_FORMATS, useValue: MY_FORMATS },
    ],
})
export class AuditComponent implements OnInit {
    flashMessage: 'success' | 'error' | null = null;
    flashMessageText: string;

    emails: any[] = [];
    isLoading: boolean = false;

    startDate: string;
    endDate: string;
    dateRangeForm: FormGroup;

    state: any = {
        skip: 0,
        take: 10,
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
    ];
    constructor(
        private _changeDetectorRef: ChangeDetectorRef,
        private _filteringService: FilteringService,
        private _matDialog: MatDialog,
        private translocoService: TranslocoService
    ) {
        this.startDate = String(moment(new Date()).format('YYYY-MM-DD'));
        this.endDate = String(
            moment(new Date()).add(1, 'day').format('YYYY-MM-DD')
        );

        this.dateRangeForm = new FormGroup({
            start: new FormControl(this.startDate),
            end: new FormControl(this.endDate),
        });
    }

    ngOnInit(): void {
        this._filteringService
            .getEmailsSentOnPlatform(this.startDate, this.endDate)
            .subscribe((response: EmailListResponse) => {
                this.emails = response.emails;
            });
    }

    isSmallScreen(): boolean {
        return window.innerWidth <= 768;
    }

    trackByFn(index: number, item: any): any {
        return item.id || index;
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

    previewEmail(id: string): void {
        this._filteringService
            .getEmailDetails(id)
            .subscribe((response: any) => {
                if (response && response.result_code >= 0) {
                    // Abre o diálogo
                    const dialogConfig: MatDialogConfig = {
                        maxHeight: '160vh',
                        maxWidth: '100vw',
                        data: response,
                    };
                    const dialogRef = this._matDialog.open(
                        PreviewCommunicationsEmailComponent,
                        dialogConfig
                    );

                    dialogRef.afterClosed().subscribe((result) => {
                        if (result) {
                            this.refreshData();
                        } else {
                            this.refreshData();
                        }
                    });
                }
            });
    }

    public onPageChange(e: PageChangeEvent): void {
        // alert('pageChange' + JSON.stringify(e));
        // process the data manually
    }

    fwdEmail(email: Email): void {
        // Open the dialog
        const dialogConfig: MatDialogConfig = {
            maxHeight: '90vh',
            minWidth: '50vh',
            data: {
                token: email.id,
                isForwarded: true,
            },
        };

        const dialogRef = this._matDialog.open(FwdEmailComponent, dialogConfig);

        dialogRef.afterClosed().subscribe((result) => {
            if (result) {
                this.isLoading = true;

                this._filteringService
                    .fwdEmail(email.id, result.insertedEmailsToFwd)
                    .subscribe(
                        (response) => {
                            this.isLoading = false;
                            if (!response || response.result_code <= 0) {
                                this.showFlashMessage(
                                    'error',
                                    this.translocoService.translate(
                                        'Order.fwd-order-error',
                                        {}
                                    )
                                );
                                return;
                            }

                            this.showFlashMessage(
                                'success',
                                this.translocoService.translate(
                                    'Order.fwd-order-success',
                                    {}
                                )
                            );

                            this.refreshData();
                        },
                        (err) => {
                            this.isLoading = false;
                            this.showFlashMessage(
                                'error',
                                this.translocoService.translate(
                                    'Order.fwd-order-error',
                                    {}
                                )
                            );
                        }
                    );
            }
        });
    }

    refreshData(): void {
        this.isLoading = true;
        this._filteringService
            .getEmailsSentOnPlatform(this.startDate, this.endDate)
            .subscribe((response: EmailListResponse) => {
                this.emails = response.emails;
            });
        this.isLoading = false;
    }

    onSelectionChange(event: string): void {
        switch (event) {
            // Hoje
            case '0':
                this.startDate = String(
                    moment(new Date()).format('YYYY-MM-DD')
                );
                this.endDate = String(moment(new Date()).format('YYYY-MM-DD'));
                break;
            // Ontem e Hoje
            case '1':
                this.startDate = String(
                    moment(new Date()).add(-1, 'days').format('YYYY-MM-DD')
                );
                this.endDate = String(moment(new Date()).format('YYYY-MM-DD'));
                break;
            // Ontem
            case '2':
                this.startDate = String(
                    moment(new Date()).add(-1, 'days').format('YYYY-MM-DD')
                );
                this.endDate = String(
                    moment(new Date()).add(-1, 'days').format('YYYY-MM-DD')
                );
                break;
            // Ultimos 3 dias
            case '3':
                this.startDate = String(
                    moment(new Date()).add(-3, 'days').format('YYYY-MM-DD')
                );
                this.endDate = String(moment(new Date()).format('YYYY-MM-DD'));
                break;
            // Últimos 7 dias
            case '7':
                this.startDate = String(
                    moment(new Date()).add(-7, 'days').format('YYYY-MM-DD')
                );
                this.endDate = String(moment(new Date()).format('YYYY-MM-DD'));
                break;
            // Tudo
            case '-1':
                this.startDate = '';
                this.endDate = '';
                break;
            default:
                break;
        }

        // Check dates before trying to refresh
        if (
            this.isValidDate(this.endDate) &&
            this.isValidDate(this.startDate)
        ) {
            this.refreshData();
        } else {
            console.error('Invalid date');
        }
    }

    // This function will be called when first value of date range is set
    onDateRangeChangeStart(event: MatDatepickerInputEvent<Date>): void {
        this.startDate = moment(
            event.value.toISOString(),
            moment.ISO_8601,
            true
        ).format('YYYY-MM-DD');
    }

    // This function will be called when the last date value from range is chosen.
    onDateRangeChangeEnd(event: MatDatepickerInputEvent<Date>): void {
        if (event.value) {
            this.endDate = moment(
                event.value.toISOString(),
                moment.ISO_8601,
                true
            ).format('YYYY-MM-DD');

            // Check dates before trying to refresh
            if (
                this.isValidDate(this.endDate) &&
                this.isValidDate(this.startDate)
            ) {
                this.refreshData();
            } else {
                console.error('Invalid date');
            }
        }
    }

    isValidDate(dateString: string): any {
        return !isNaN(new Date(dateString).getTime()) || isEmpty(dateString);
    }
}
