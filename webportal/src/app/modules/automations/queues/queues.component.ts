import {
    Component,
    OnInit,
    ViewEncapsulation,
    ChangeDetectorRef,
    ChangeDetectionStrategy,
    OnDestroy,
} from '@angular/core';
import { FormControl, FormGroup } from '@angular/forms';
import { Observable, Subject } from 'rxjs';
import { Queues } from './queues.types';
import { MatDialogConfig } from '@angular/material/dialog';
import { fuseAnimations } from '@fuse/animations';
import { MatDialog } from '@angular/material/dialog';
import { QueuesService } from './queues.service';
import { FuseConfirmationService } from '@fuse/services/confirmation';
import { TranslocoService } from '@ngneat/transloco';
import { AddQueues } from '../add-queues/add-queues.component';
import { EditQueues } from '../edit-queues/edit-queues.component';
import { Router } from '@angular/router';
import { FlashMessageService } from 'app/shared/components/flash-message/flash-message.service';

@Component({
    selector: 'app-queues',
    templateUrl: './queues.component.html',
    styles: [
        /* language=SCSS */
        `
            .queues-grid {
                grid-template-columns: 20% 16% 16% 16% 16% 16%;
            }
        `,
    ],
    encapsulation: ViewEncapsulation.None,
    changeDetection: ChangeDetectionStrategy.OnPush,
    animations: fuseAnimations,
})
export class QueuesComponent implements OnInit, OnDestroy {
    searchInputControl: FormControl = new FormControl();
    isLoading: boolean = false;
    selectedQueue: Queues | null = null;
    selectedQueueForm: FormGroup;
    _unsubscribeAll: Subject<any> = new Subject<any>();
    searchEmailLabel: string = this.translocoService.translate(
        'search-by-email',
        {}
    );

    public gridData: Queues[] = [];
    filteredSources: Queues[];

    filters: {
        // categorySlug$: BehaviorSubject<string>;
        // query$: BehaviorSubject<string>;
        // hideCompleted$: BehaviorSubject<boolean>;
    } = {
        // categorySlug$: new BehaviorSubject('all'),
        // query$: new BehaviorSubject(''),
        // hideCompleted$: new BehaviorSubject(false),
    };

    queues$: Observable<Queues[]>;
    private queuesArr: Queues[] = [];
    // public scriptNames: string[] = [];
    length: number;

    constructor(
        private _changeDetectorRef: ChangeDetectorRef,
        private _matDialog: MatDialog,
        private _queueService: QueuesService,
        private _fuseConfirmationService: FuseConfirmationService,
        private router: Router,
        private readonly translocoService: TranslocoService,
        private _fm: FlashMessageService
    ) {}

    ngOnInit(): void {
        // Create the selected account form
        this.updateQueues();
    }

    expandedItemId: string | null = null;

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

    updateQueues(emailFilter?: string) {
        // alert("getQueues");
        this.isLoading = true;
        this._queueService.getQueues(emailFilter).subscribe(
            (response) => {
                if (response.result_code <= 0) {
                    this._fm.error('error-loading-list');
                    return;
                }

                this.isLoading = false;
                if (response && response.queues) {
                    // this.queuesArr = response.queues;
                    // this.queues$ = of(this.queuesArr);
                    this.gridData = response.queues;
                }
                this._changeDetectorRef.markForCheck();
            },
            (err) => {
                // alert(JSON.stringify(err));
                this.isLoading = false;
                this._fm.error('error-loading-list');

                this._changeDetectorRef.markForCheck();
            }
        );
    }

    deleteQueue(id: string) {
        // Open the confirmation dialog
        const confirmation = this._fuseConfirmationService.open({
            title: this.translocoService.translate('Scripts.queue-delete', {}),
            message: this.translocoService.translate(
                'Scripts.queue-delete-message',
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
                this._queueService.deleteQueue(id).subscribe(
                    (response) => {
                        if (response.result_code <= 0) {
                            this._fm.error('Scripts.queue-deleted-error');
                            return;
                        }

                        this.isLoading = false;

                        if (response && response.result_code > 0) {
                            this._fm.success('Scripts.queue-deleted');
                        } else {
                            this._fm.error('Scripts.queue-deleted-error');
                        }

                        this.updateQueues();
                    },
                    (err) => {
                        this.isLoading = false;
                        this._fm.error('Scripts.queue-deleted-error');
                    }
                );
            }
        });
    }

    viewTransactions(id: String): void {
        this.router
            .navigate(['/transactions', id])
            .then((success) => {})
            .catch((error) => {});
    }

    addQueue(): void {
        // Open the dialog
        const dialogConfig: MatDialogConfig = {
            maxHeight: '100vh',
            minWidth: '60vh',
        };

        const dialogRef = this._matDialog.open(AddQueues, dialogConfig);

        dialogRef.afterClosed().subscribe((result) => {
            if (result) {
                this.isLoading = true;
                this._queueService
                    .addQueue(
                        result.name,
                        result.description,
                        result.autoRetry,
                        result.numberRetry,
                        result.script_name
                    )
                    .subscribe(
                        (response) => {
                            if (response.result_code <= 0) {
                                this._fm.error('Scripts.queue-added-error');
                                return;
                            }

                            this.isLoading = false;

                            if (response && response.result_code > 0) {
                                this._fm.success('Scripts.queue-added');
                                this.updateQueues();
                            } else {
                                this._fm.error('Scripts.queue-added-error');
                            }
                        },
                        (err) => {
                            this.isLoading = false;
                            this._fm.error('Scripts.queue-added-error');

                            this._changeDetectorRef.markForCheck();
                        }
                    );
            }
        });
    }

    editQueue(queue: Queues): void {
        // Open the dialog
        const dialogConfig: MatDialogConfig = {
            maxHeight: '100vh',
            minWidth: '60vh',
            data: {
                queue: queue,
            },
        };

        const dialogRef = this._matDialog.open(EditQueues, dialogConfig);

        dialogRef.afterClosed().subscribe((result) => {
            if (result) {
                // alert(JSON.stringify(result));
                this.isLoading = true;
                queue.name = result.name;
                queue.description = result.description;
                queue.autoRetry = result.autoRetry as boolean;
                queue.numberRetry = result.numberRetry;

                this._queueService
                    .editQueue(
                        queue.name,
                        result.description,
                        result.autoRetry,
                        result.numberRetry,
                        queue.id
                    )
                    .subscribe((response) => {
                        this.isLoading = false;
                        if (response) {
                            this._fm.success('Scripts.queue-edit-success');
                            this.updateQueues();
                        } else {
                            this.isLoading = false;
                            this._fm.error('Scripts.queue-edit-error');
                        }
                        this._changeDetectorRef.markForCheck();
                    });
            }
        });
    }

    /**
     * Close the details
     */
    closeDetails(): void {
        this.selectedQueue = null;
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
