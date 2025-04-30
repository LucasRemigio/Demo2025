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
import { JobsEntry } from './jobs.types';
import { fuseAnimations } from '@fuse/animations';
import { of } from 'rxjs';
import { MatSelectChange } from '@angular/material/select';
import { MatDialog } from '@angular/material/dialog';
import { JobsService } from './jobs.service';
import { Jobs } from './jobs.types';
import { FuseConfirmationService } from '@fuse/services/confirmation';
import { UserService } from 'app/core/user/user.service';
import { TranslocoService } from '@ngneat/transloco';
import { Renderer2 } from '@angular/core';
import { PopUpInfo } from './pop-up-info/pop-up-info.component';
import { ActivatedRoute } from '@angular/router';
import { FlashMessageService } from 'app/shared/components/flash-message/flash-message.service';
@Component({
    selector: 'app-jobs',
    templateUrl: './jobs.component.html',
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
export class JobsComponent implements OnInit {
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
    private _unsubscribeAll: Subject<any> = new Subject<any>();

    public gridData: Jobs[] = [];

    jobs$: Observable<JobsEntry[]>;
    private jobsArr: JobsEntry[] = [];
    private jobsArrInitial: JobsEntry[] = [];
    constructor(
        private _changeDetectorRef: ChangeDetectorRef,
        private _formBuilder: FormBuilder,
        private _matDialog: MatDialog,
        private _jobsService: JobsService,
        private _userService: UserService,
        private readonly translocoService: TranslocoService,
        private _fuseConfirmationService: FuseConfirmationService,
        private renderer: Renderer2,
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
                this.getJobsByScriptId(params['id']);
            } else {
                // This component was loaded from the 'jobs' route
                if (this.isAdmin) {
                    this.updateJobs();
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

    searchScripts(Id: string) {
        let searchTerm = '@';

        if (Id) {
            this.StartScripts(Id);
        } else {
            this.updateJobs();
        }
    }

    private StartScripts(id: string) {
        this._fm.success('Scripts.start-script-success');
        this.isLoading = true;

        this._jobsService.StartScripts(id).subscribe(
            (response) => {
                this.isLoading = false;

                if (response && response.jobs_items) {
                    this.jobsArr = response.jobs_items;
                    this.jobs$ = of(this.jobsArr);
                    this.gridData = response.jobs_items;
                }

                this.updateJobs();
                this._fm.success('Scripts.run-script-finish');
                this._changeDetectorRef.markForCheck();
            },
            (err) => {
                this.isLoading = false;
                this._fm.error('Scripts.start-script-error');

                this._changeDetectorRef.markForCheck();
            }
        );
    }

    closeDetalhes(dataItem: any): void {
        dataItem.showDetalhes = false;
    }

    // -----------------------------------------------------------------------------------------------------
    // @ Public methods
    // -----------------------------------------------------------------------------------------------------

    private updateJobs() {
        this.isLoading = true;

        this._jobsService.getJobs().subscribe(
            (response) => {
                this.isLoading = false;

                if (response.result_code <= 0) {
                    this._fm.error('error-loading-list');
                    return;
                }

                if (response && response.jobs_items) {
                    this.jobsArr = response.jobs_items;
                    this.jobs$ = of(this.jobsArr);
                    this.gridData = response.jobs_items;
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

    getJobsByScriptId(id: string): void {
        if (!id) {
            return;
        }

        this.isLoading = true; // Set loading indicator
        this._jobsService.getJobsByScriptId(id).subscribe(
            (response) => {
                if (response.result_code <= 0) {
                    this._fm.error('error-loading-list');
                    return;
                }

                if (response && response.jobs_items) {
                    this.jobsArr = response.jobs_items;
                    this.jobs$ = of(this.jobsArr);
                    this.gridData = response.jobs_items;
                }
                this.isLoading = false;

                this._changeDetectorRef.markForCheck();
            },
            (err) => {
                this.isLoading = false;
                this._fm.error('error-loading-list');

                this._changeDetectorRef.markForCheck();
            }
        );
    }

    toggleDetalhes(dataItem: Jobs): void {
        // Assuming that your openPopUpInfo method is available in the same component
        this.openPopUpInfo(dataItem);
    }

    openPopUpInfo(jobs?: Jobs): void {
        var isToAdd: boolean = !jobs;

        if (!jobs) {
            jobs = {
                id: '',
                script_id: '',
                script_name: '',
                user_operation: '',
                date_time: '',
                state_operation: '',
                job_details: '',
            };
            isToAdd = true;
        }

        // Open the dialog
        const dialogConfig: MatDialogConfig = {
            maxHeight: '80vh',
            minWidth: '70vh',
            data: {
                jobs: jobs,
            },
        };

        const dialogRef = this._matDialog.open(PopUpInfo, dialogConfig);

        dialogRef.afterClosed().subscribe((result) => {
            this.isLoading = false;

            if (result) {
                this.isLoading = true;
                jobs.job_details = result.type;
            }
        });
    }

    statusFilterChange(event: MatSelectChange) {
        this.currentStatusFilter = event.value;
        this.updateFilter();
    }

    updateFilter() {
        if (this.currentStatusFilter === 'All') {
            this.jobsArr = this.jobsArrInitial;
        } else {
            this.jobsArr = this.jobsArrInitial.filter(
                (item) => item.state === this.currentStatusFilter
            );
        }
        this.jobs$ = of(this.jobsArr);
    }

    /**
     * Toggle account details
     *
     * @param accountEmail
     */

    searchJobs(): void {
        let searchTerm =
            (this.searchInputControl.value, this.searchInputControl1.value);

        if (searchTerm) {
            this.updateJobs();
        } else {
            this.updateJobs();
        }
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
