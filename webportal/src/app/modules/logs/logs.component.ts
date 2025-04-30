import {
    Component,
    OnInit,
    ViewEncapsulation,
    ChangeDetectorRef,
    ChangeDetectionStrategy,
} from '@angular/core';
import {
    FormBuilder,
    FormControl,
    FormGroup,
    Validators,
} from '@angular/forms';
import { merge, Observable, Subject } from 'rxjs';
import { LogsEntry, UpdateReceiptEntry, FileInput } from './logs.types';
import { fuseAnimations } from '@fuse/animations';
import { of } from 'rxjs';
import { tap } from 'rxjs/operators';
import { MatSelectChange } from '@angular/material/select';
import { MatDialog } from '@angular/material/dialog';
import { LogService } from './logs.service';
import { Product } from './logs.types';
import { FuseConfirmationService } from '@fuse/services/confirmation';
import { UserService } from 'app/core/user/user.service';
import { TranslocoService } from '@ngneat/transloco';
import dayjs from 'dayjs';

@Component({
    selector: 'app-logs',
    templateUrl: './logs.component.html',
    styles: [
        /* language=SCSS */
        `
            .admin-receipts-grid {
                grid-template-columns: 20% 16% 16% 16% 16% 16%;
            }

            .employee-receipts-grid {
                grid-template-columns: 25% 25% 25% 25%;
            }
        `,
    ],
    encapsulation: ViewEncapsulation.None,
    changeDetection: ChangeDetectionStrategy.OnPush,
    animations: fuseAnimations,
})
export class LogsComponent implements OnInit {
    searchInputControl: FormControl = new FormControl();
    searchInputControl1: FormControl = new FormControl();
    statusFilter: String[] = [
        'All',
        'GetUsers',
        'Negotiations',
        'On Hold',
        'On Boarding',
        'Rejected',
        'Inactive',
    ];
    currentStatusFilter: String = 'All';
    isLoading: boolean = false;

    selectedAccountForm: FormGroup;
    flashMessage: 'success' | 'error' | null = null;
    flashMessageText: string;
    isAdmin: boolean = false;
    private _unsubscribeAll: Subject<any> = new Subject<any>();

    public gridData: Product[] = [];

    logs$: Observable<LogsEntry[]>;
    private logsArr: LogsEntry[] = [];
    private logsArrInitial: LogsEntry[] = [];
    constructor(
        private _changeDetectorRef: ChangeDetectorRef,
        private _formBuilder: FormBuilder,
        private _matDialog: MatDialog,
        private _logsService: LogService,
        private _userService: UserService,
        private readonly translocoService: TranslocoService,
        private _fuseConfirmationService: FuseConfirmationService
    ) {
        this.isAdmin = this._userService.isAdmin();
    }

    ngOnInit(): void {
        // Create the selected account form
        this.selectedAccountForm = this._formBuilder.group({
            name: [''],
            password: [''],
        });

        if (this.isAdmin) {
            this.updateLogs();
        }
    }

    ngOnDestroy(): void {
        // Unsubscribe from all subscriptions
        this._unsubscribeAll.next();
        this._unsubscribeAll.complete();
    }

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

    // -----------------------------------------------------------------------------------------------------
    // @ Public methods
    // -----------------------------------------------------------------------------------------------------

    private updateLogs(emailFilter?: string, ContextFilter?: string) {
        this.isLoading = true;

        this._logsService.getLogs(emailFilter, ContextFilter).subscribe(
            (response) => {
                this.isLoading = false;

                if (response && response.logs_items) {
                    this.logsArr = response.logs_items;
                    this.logs$ = of(this.logsArr);
                    this.gridData = response.logs_items;
                }

                this._changeDetectorRef.markForCheck();
            },
            (err) => {
                this.isLoading = false;
                this.showFlashMessage(
                    'error',
                    this.translocoService.translate('error-loading-list', {})
                );

                this._changeDetectorRef.markForCheck();
            }
        );
    }

    statusFilterChange(event: MatSelectChange) {
        this.currentStatusFilter = event.value;
        this.updateFilter();
    }

    updateFilter() {
        if (this.currentStatusFilter === 'All') {
            this.logsArr = this.logsArrInitial;
        } else {
            this.logsArr = this.logsArrInitial.filter(
                (item) => item.state === this.currentStatusFilter
            );
        }
        this.logs$ = of(this.logsArr);
    }

    /**
     * Toggle account details
     *
     * @param accountEmail
     */

    searchLogs(): void {
        let searchTerm =
            (this.searchInputControl.value, this.searchInputControl1.value);

        if (searchTerm) {
            this.updateLogs(searchTerm);
        } else {
            this.updateLogs();
        }
    }

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
        }, 10000);
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
}
