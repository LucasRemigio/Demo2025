import {
    Component,
    OnInit,
    ViewEncapsulation,
    ChangeDetectorRef,
    ChangeDetectionStrategy,
} from '@angular/core';
import { FormBuilder, FormControl, FormGroup } from '@angular/forms';
import { Observable, Subject } from 'rxjs';
import { MatDialogConfig } from '@angular/material/dialog';
import { Triggers } from './triggers.types';
import { fuseAnimations } from '@fuse/animations';
import { of } from 'rxjs';
import { takeUntil } from 'rxjs/operators';
import { MatSelectChange } from '@angular/material/select';
import { MatDialog } from '@angular/material/dialog';
import { TriggersService } from './triggers.service';
import { FuseConfirmationService } from '@fuse/services/confirmation';
import { UserService } from 'app/core/user/user.service';
import { TranslocoService } from '@ngneat/transloco';
import { EditTrigger } from './edit-trigger/edit-trigger.component';
import { ActivatedRoute } from '@angular/router';
import { AddTriggers } from './add-triggers/add-triggers.component';
import { FlashMessageService } from 'app/shared/components/flash-message/flash-message.service';
@Component({
    selector: 'app-triggers',
    templateUrl: './triggers.component.html',
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
export class TriggersComponent implements OnInit {
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
    isAdmin: boolean = false;
    _unsubscribeAll: Subject<any> = new Subject<any>();

    gridData: Triggers[] = [];

    triggers$: Observable<Triggers[]>;
    private triggersArr: Triggers[] = [];
    private triggersArrInitial: Triggers[] = [];
    constructor(
        private _changeDetectorRef: ChangeDetectorRef,
        private _formBuilder: FormBuilder,
        private _matDialog: MatDialog,
        private _triggersService: TriggersService,
        private _userService: UserService,
        private readonly translocoService: TranslocoService,
        private _fuseConfirmationService: FuseConfirmationService,
        private route: ActivatedRoute,
        private _fm: FlashMessageService
    ) {
        this.isAdmin = this._userService.isAdmin();
    }

    ngOnInit(): void {
        // Create the selected account form
        this.selectedAccountForm = this._formBuilder.group({
            name: [''],
            password: [''],
        });

        this.route.params.subscribe((params) => {
            if (params['id']) {
                // This component was loaded from the 'jobs/:id' route
                this.getTriggersByScriptId(params['id']);
            } else {
                // This component was loaded from the 'jobs' route
                if (this.isAdmin) {
                    this.updateTriggers();
                }
            }
        });
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

    closeDetalhes(dataItem: any): void {
        dataItem.showDetalhes = false;
    }

    // -----------------------------------------------------------------------------------------------------
    // @ Public methods
    // -----------------------------------------------------------------------------------------------------

    updateTriggers() {
        this.isLoading = true;

        this._triggersService.getTriggers().subscribe(
            (response) => {
                this.isLoading = false;

                if (response && response.triggers) {
                    this.triggersArr = response.triggers;
                    this.triggers$ = of(this.triggersArr);
                    this.gridData = response.triggers;
                }

                this._changeDetectorRef.markForCheck();
            },
            (err) => {
                this.isLoading = false;
                this._fm.error('error-loading-list');

                this._changeDetectorRef.markForCheck();
            }
        );
    }

    getTriggersByScriptId(id: string): void {
        if (!id) {
            return;
        }

        this.isLoading = true;
        this._triggersService
            .getTriggersByScriptId(id)
            .pipe(takeUntil(this._unsubscribeAll))
            .subscribe(
                (response) => {
                    this.isLoading = false;

                    if (response && response.triggers) {
                        this.triggersArr = response.triggers;
                        this.triggers$ = of(this.triggersArr);
                        this.gridData = response.triggers;
                    }

                    this._changeDetectorRef.markForCheck();
                },
                (err) => {
                    this.isLoading = false;
                    this._fm.error('error-loading-list');

                    this._changeDetectorRef.markForCheck();
                }
            );
    }

    getScheduleType(cronExpression: string): string {
        const parts = cronExpression.split(' ');

        // Check for daily schedules
        if (
            parts[4] === '*' &&
            parts[5] === '?' &&
            (parts[3] === '*' || parts[3] === '?')
        ) {
            const hour = parseInt(parts[2], 10); // Extract hour
            const minute = parseInt(parts[1], 10); // Extract minute
            const period = hour >= 12 ? 'PM' : 'AM'; // Determine AM/PM

            // Convert hour to 12-hour format
            const formattedHour = hour % 12 === 0 ? 12 : hour % 12;

            // Format the time string
            return `Daily At ${formattedHour
                .toString()
                .padStart(2, '0')}:${minute
                .toString()
                .padStart(2, '0')} ${period}`;
        }

        // Check for weekly schedules
        if (
            (parts[3] === '?' && !isNaN(Number(parts[5]))) ||
            (parts[3] === '?' && parts[5].includes(','))
        ) {
            const daysOfWeek = parts[5]
                .split(',')
                .map((day) => this.getDayName(parseInt(day, 10)));

            if (daysOfWeek.length > 0) {
                const hour = parseInt(parts[2], 10); // Extract hour
                const minute = parseInt(parts[1], 10); // Extract minute
                const period = hour >= 12 ? 'PM' : 'AM'; // Determine AM/PM

                // Convert hour to 12-hour format
                const formattedHour = hour % 12 === 0 ? 12 : hour % 12;

                // Format the time string
                return `Weekly on ${daysOfWeek.join(', ')} At ${formattedHour
                    .toString()
                    .padStart(2, '0')}:${minute
                    .toString()
                    .padStart(2, '0')} ${period}`;
            }
        }

        // Check for monthly schedules (day of month)
        if (parts[5] === '?' && parts[3] !== '*') {
            const dayOfMonth = parseInt(parts[3], 10);
            const hour = parseInt(parts[2], 10); // Extract hour
            const minute = parseInt(parts[1], 10); // Extract minute
            const period = hour >= 12 ? 'PM' : 'AM'; // Determine AM/PM

            // Convert hour to 12-hour format
            const formattedHour = hour % 12 === 0 ? 12 : hour % 12;
            return `Monthly on day ${dayOfMonth} At ${formattedHour
                .toString()
                .padStart(2, '0')}:${minute
                .toString()
                .padStart(2, '0')} ${period}`;
        }
    }

    // Helper function to get day name from day index
    getDayName(dayIndex: number): string {
        const daysOfWeek = ['Sun', 'Mon', 'Tue', 'Wed', 'Thu', 'Fri', 'Sat'];
        return daysOfWeek[dayIndex - 1] || '';
    }

    toggleDetalhes(dataItem: Triggers): void {
        this.editTrigger(dataItem);
    }

    addTrigger(): void {
        // Open the dialog
        const dialogConfig: MatDialogConfig = {
            maxHeight: '100vh',
            minWidth: '60vh',
        };

        const dialogRef = this._matDialog.open(AddTriggers, dialogConfig);

        dialogRef.afterClosed().subscribe((result) => {
            if (result) {
                this.isLoading = false;

                this._triggersService
                    .addTrigger(
                        result.name,
                        result.cron_expression,
                        result.script_name
                    )
                    .subscribe(
                        (response) => {
                            // alert(response);
                            this.isLoading = false;

                            if (response && response.result_code > 0) {
                                this._fm.success(
                                    this.translocoService.translate(
                                        'Scripts.trigger-add-success',
                                        {}
                                    )
                                );
                                this.updateTriggers();
                            } else {
                                this._fm.error('Scripts.trigger-add-error');
                            }
                        },
                        (err) => {
                            this.isLoading = false;
                            this._fm.error('Scripts.trigger-add-error');

                            this._changeDetectorRef.markForCheck();
                        }
                    );
            } else {
                this.isLoading = false;
            }
        });
    }

    editTrigger(trigger: Triggers): void {
        // Open the dialog
        const dialogConfig: MatDialogConfig = {
            maxHeight: '100vh',
            minWidth: '60vh',
            data: {
                trigger: trigger,
            },
        };

        const dialogRef = this._matDialog.open(EditTrigger, dialogConfig);

        dialogRef.afterClosed().subscribe((result) => {
            if (result) {
                this.isLoading = false;

                this._triggersService
                    .editTrigger(
                        trigger.id.toString(),
                        trigger.name,
                        result.cron_expression
                    )
                    .subscribe(
                        (response) => {
                            // alert(response);
                            this.isLoading = false;

                            if (response && response.result_code > 0) {
                                this._fm.success(
                                    this.translocoService.translate(
                                        'Scripts.trigger-edit-success',
                                        {}
                                    )
                                );
                                this.updateTriggers();
                            } else {
                                this._fm.error('Scripts.trigger-edit-error');
                            }
                        },
                        (err) => {
                            this.isLoading = false;
                            this._fm.error('Scripts.trigger-edit-error');

                            this._changeDetectorRef.markForCheck();
                        }
                    );
            } else {
                this.isLoading = false;
            }
        });
    }

    deleteTrigger(id: string) {
        // Open the confirmation dialog
        const confirmation = this._fuseConfirmationService.open({
            title: this.translocoService.translate(
                'Scripts.trigger-delete',
                {}
            ),
            message: this.translocoService.translate(
                'Scripts.trigger-delete-text',
                {}
            ),
            actions: {
                confirm: {
                    label: this.translocoService.translate('delete', {}),
                },

                cancel: {
                    label: this.translocoService.translate('cancel', {}),
                },
            },
        });

        // Subscribe to the confirmation dialog closed action
        confirmation.afterClosed().subscribe((result) => {
            // If the confirm button pressed...
            if (result === 'confirmed') {
                this._triggersService.deleteTrigger(id).subscribe(
                    (response) => {
                        this.isLoading = false;

                        if (response && response.result_code > 0) {
                            this._fm.success('Scripts.trigger-delete-success');
                        } else {
                            this._fm.error('Scripts.trigger-delete-error');
                        }

                        this.updateTriggers();
                    },
                    (err) => {
                        this.isLoading = false;
                        this._fm.error('Scripts.trigger-delete-error');
                    }
                );
            }
        });
    }

    statusFilterChange(event: MatSelectChange) {
        this.currentStatusFilter = event.value;
        this.updateFilter();
    }

    updateFilter() {
        if (this.currentStatusFilter === 'All') {
            this.triggersArr = this.triggersArrInitial;
        } else {
            this.triggersArr = this.triggersArrInitial.filter(
                (item) => item.script_name === this.currentStatusFilter
            );
        }
        this.triggers$ = of(this.triggersArr);
    }

    /**
     * Track by function for ngFor loops
     *
     * @param index
     * @param item
     */
    trackById(index: number, item: any): any {
        return item.id || index;
    }
}
