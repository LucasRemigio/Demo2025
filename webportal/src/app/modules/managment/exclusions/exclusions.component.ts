import {
    ChangeDetectionStrategy,
    ChangeDetectorRef,
    Component,
    OnDestroy,
    OnInit,
    ViewEncapsulation,
} from '@angular/core';
import { ActivatedRoute, Router } from '@angular/router';
import { takeUntil } from 'rxjs/operators';
import {
    Client_Address,
    Exclusions,
} from 'app/modules/managment/managment.types';
import { ManagmentService } from 'app/modules/managment/managment.service';
import { UserService } from 'app/core/user/user.service';
import { MatDialog, MatDialogRef } from '@angular/material/dialog';
import { GridComponent, PageChangeEvent } from '@progress/kendo-angular-grid';
import { FuseConfirmationService } from '@fuse/services/confirmation';
import { MatDialogConfig } from '@angular/material/dialog';
import { TranslocoService } from '@ngneat/transloco';
import { DomSanitizer, SafeStyle } from '@angular/platform-browser';
import { BreakpointObserver, Breakpoints } from '@angular/cdk/layout';

import { MomentDateAdapter } from '@angular/material-moment-adapter';
import {
    DateAdapter,
    MAT_DATE_FORMATS,
    MAT_DATE_LOCALE,
} from '@angular/material/core';
import { Subject } from 'rxjs';
import { PopUpInfo } from './pop-up-info/pop-up-info.component';

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
    selector: 'exclusions',
    templateUrl: './exclusions.component.html',
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
export class ExclusionsComponent implements OnInit, OnDestroy {
    flashMessage: 'success' | 'error' | null = null;
    flashMessageText: string;
    isAdmin: boolean = false;
    isLoading: boolean = false;

    private _unsubscribeAll: Subject<any> = new Subject<any>();

    /*
     * Constructor
     */
    constructor(
        private _activatedRoute: ActivatedRoute,
        private _changeDetectorRef: ChangeDetectorRef,
        private _router: Router,
        private _managmentService: ManagmentService,
        private _userService: UserService,
        private _matDialog: MatDialog,
        private _fuseConfirmationService: FuseConfirmationService,
        private translocoService: TranslocoService,
        private sanitizer: DomSanitizer,
        private breakpointObserver: BreakpointObserver
    ) {
        this.isAdmin = this._userService.isAdmin();
    }

    exclusions: Exclusions[] = [];

    // -----------------------------------------------------------------------------------------------------
    // @ Lifecycle hooks
    // -----------------------------------------------------------------------------------------------------

    /**
     * On init
     */
    ngOnInit(): void {
        // Get the categories

        // Get the clients
        this._managmentService.exclusions$
            .pipe(takeUntil(this._unsubscribeAll))
            .subscribe((exclusions: Exclusions[]) => {
                this.exclusions = exclusions;
                // Mark for check
                this._changeDetectorRef.markForCheck();
            });
    }

    /**
     * On destroy
     */
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
    isSmallScreen(): boolean {
        return window.innerWidth <= 768; 
    }

    openPopUpInfo(client?: Exclusions): void {
        var isToAdd: boolean = false;
        if (!client) {
            client = {
                id: '',
                client_email: '',
                client_id: '',
                client_vat: '',
            };
            isToAdd = true;
        }
        // Open the dialog
        const dialogConfig: MatDialogConfig = {
            maxHeight: '90vh',
            minWidth: this.breakpointObserver.isMatched(Breakpoints.Handset) ? '35vh' : '60vh',
            data: {
                client: client,
            },
        };

        const dialogRef = this._matDialog.open(PopUpInfo, dialogConfig);

        dialogRef.afterClosed().subscribe((result) => {
            if (result) {
                this.isLoading = true;
                client.client_email = result.client_email;
                client.client_id = result.client_id;
                client.client_vat = result.client_vat;

                if (isToAdd) {
                    this._managmentService
                        .addExclusion(client)
                        .subscribe((response) => {
                            this.isLoading = false;
                            if (response) {
                                this.showFlashMessage(
                                    'success',
                                    this.translocoService.translate(
                                        'Exclusions.exclusion-added',
                                        {}
                                    )
                                );
                                this._managmentService
                                    .getAllExcluisons()
                                    .subscribe((response: any) => {
                                        this.exclusions =
                                            response.exclusion_clients;
                                    });
                            } else {
                                this.showFlashMessage(
                                    'error',
                                    this.translocoService.translate(
                                        'Exclusions.exclusion-error',
                                        {}
                                    )
                                );
                            }
                            this._changeDetectorRef.markForCheck();
                        });
                } else {
                    this._managmentService
                        .editExclusion(client)
                        .subscribe((response) => {
                            this.isLoading = false;
                            if (response) {
                                this.showFlashMessage(
                                    'success',
                                    this.translocoService.translate(
                                        'Exclusions.exclusion-edited',
                                        {}
                                    )
                                );
                                this._managmentService
                                    .getAllExcluisons()
                                    .subscribe((response: any) => {
                                        this.exclusions =
                                            response.exclusion_clients;
                                    });
                            } else {
                                this.showFlashMessage(
                                    'error',
                                    this.translocoService.translate(
                                        'Exclusions.exclusion-edited-error',
                                        {}
                                    )
                                );
                            }
                            this._changeDetectorRef.markForCheck();
                        });
                }
            }
        });
    }

    remove(client: Exclusions) {
        // Open the confirmation dialog
        const confirmation = this._fuseConfirmationService.open({
            title: this.translocoService.translate(
                'Exclusions.delete-title',
                {}
            ),
            message: this.translocoService.translate(
                'Exclusions.delete-text',
                {}
            ),
            actions: {
                confirm: {
                    label: this.translocoService.translate('confirm', {}),
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
                this._managmentService
                    .removeExclusion(client)
                    .subscribe((response) => {
                        this.isLoading = false;
                        if (response) {
                            this.showFlashMessage(
                                'success',
                                this.translocoService.translate(
                                    'Exclusions.exclusion-deleted',
                                    {}
                                )
                            );

                            this._managmentService
                                .getAllExcluisons()
                                .subscribe((response: any) => {
                                    this.exclusions =
                                        response.exclusion_clients;
                                });
                        } else {
                            this.showFlashMessage(
                                'error',
                                this.translocoService.translate(
                                    'Exclusions.exclusion-error',
                                    {}
                                )
                            );
                        }
                        this._changeDetectorRef.markForCheck();
                    });

                this._changeDetectorRef.markForCheck();
            }
        });
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

    public colorCode(code: string): SafeStyle {
        let result;

        switch (code) {
            default:
                result = 'transparent';
                break;
        }

        return this.sanitizer.bypassSecurityTrustStyle(result);
    }

    public onPageChange(e: PageChangeEvent): void {
        // alert('pageChange' + JSON.stringify(e));
        // process the data manually
    }
}
