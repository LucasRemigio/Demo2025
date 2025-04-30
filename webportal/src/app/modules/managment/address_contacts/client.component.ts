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
import { Client_Address } from 'app/modules/managment/managment.types';
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
import { ImportFile } from './import-file/import-file.component';
import { PopUpAddressInfo } from './pop-up-address-info/pop-up-address-info.component';

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
    selector: 'client',
    templateUrl: './client.component.html',
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
export class ClientComponent implements OnInit, OnDestroy {
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


    clients: Client_Address[] = [];

    // -----------------------------------------------------------------------------------------------------
    // @ Lifecycle hooks
    // -----------------------------------------------------------------------------------------------------

    /**
     * On init
     */
    ngOnInit(): void {
        // Get the categories

        // Get the clients
        this._managmentService.clients$
            .pipe(takeUntil(this._unsubscribeAll))
            .subscribe((clients: Client_Address[]) => {
                this.clients = clients;
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

    /**
     * Track by function for ngFor loops
     *
     * @param index
     * @param item
     */
    trackByFn(index: number, item: any): any {
        return item.id || index;
    }

    isSmallScreen(): boolean { 
        return window.innerWidth <= 768; 
    }
    
    importFile(): void {

        // Open the dialog
        const dialogConfig: MatDialogConfig = {
            maxHeight: '50vh',
            minWidth: this.breakpointObserver.isMatched(Breakpoints.Handset) ? '20vh' : '60vh'
            
          };

        const dialogRef = this._matDialog.open(ImportFile, dialogConfig);

        dialogRef.afterClosed().subscribe((result) => {
            if (result) {
                this.isLoading = true;
                this.showFlashMessage(
                    'success',
                    this.translocoService.translate('Clients.file-sent', {})
                );
                this._managmentService
                    .add(result.file_content)
                    .subscribe((response) => {
                        this._managmentService
                            .getAlClientsAddresses()
                            .subscribe((response: any) => {
                                if (response) {
                                    this.refreshData();

                                    this.isLoading = false;
                                } else {
                                    this.refreshData();
                                    this.showFlashMessage(
                                        'error',
                                        this.translocoService.translate(
                                            'Clients.file-sent-error',
                                            {}
                                        )
                                    );
                                    this.isLoading = false;
                                }
                            });

                        this._changeDetectorRef.markForCheck();
                    });
            } else {
                this.isLoading = false;
            }
        });
    }

    openPopUpInfo(client: Client_Address): void {
        this._managmentService
            .getAllAddressesByToken(client.token)
            .subscribe((response: any) => {
                if (response) {
                    const dialogConfig: MatDialogConfig = {
                        maxHeight: '90vh',
                        minWidth: this.breakpointObserver.isMatched(Breakpoints.Handset) ? '10vh' : '65vh',
                        data: { addresses: response.addresses },
                      };

                    const dialogRef = this._matDialog.open(
                        PopUpAddressInfo,
                        dialogConfig
                    );

                    dialogRef.afterClosed().subscribe((result) => {
                        if (result) {
                            this.isLoading = true;
                            this._managmentService
                                .edit(result)
                                .subscribe((response) => {
                                    this.isLoading = false;
                                    if (response) {
                                        this.refreshData();
                                        this.showFlashMessage(
                                            'success',
                                            this.translocoService.translate(
                                                'Clients.data-updated',
                                                {}
                                            )
                                        );
                                    } else {
                                        this.refreshData();
                                        this.showFlashMessage(
                                            'error',
                                            this.translocoService.translate(
                                                'Clients.data-update-error',
                                                {}
                                            )
                                        );
                                    }
                                    this._changeDetectorRef.markForCheck();
                                });
                        }
                    });
                } else {
                    this.showFlashMessage(
                        'error',
                        this.translocoService.translate(
                            'Clients.get-address-error',
                            {}
                        )
                    );
                }
                this._changeDetectorRef.markForCheck();
            });
    }

    resendMail(client: Client_Address): void {
        this.isLoading = true;
        this._managmentService
            .getAllAddressesByToken(client.token)
            .subscribe((response: any) => {
                this.isLoading = false;
                if (response) {
                    this._managmentService
                        .resendConfirmationEmail({
                            addresses: response.addresses,
                        })
                        .subscribe((response: any) => {
                            this.isLoading = false;
                            if (response) {
                                this.refreshData();
                                this.showFlashMessage(
                                    'success',
                                    this.translocoService.translate(
                                        'email-sent-success', {}
                                    )
                                );
                            } else {
                                this.refreshData();
                                this.showFlashMessage(
                                    'error',
                                    this.translocoService.translate(
                                        'email-sent-error',
                                        {}
                                    )
                                );
                            }
                            this._changeDetectorRef.markForCheck();
                        });
                } else {
                    this.showFlashMessage(
                        'error',
                        this.translocoService.translate(
                            'email-sent-error',
                            {}
                        )
                    );
                }
                this._changeDetectorRef.markForCheck();
            });

        // Open the dialog
    }

    sendAllClientsNotification(): void {
        this.isLoading = true;
        this._managmentService
            .sendAllClientsNotification()
            .subscribe((response: any) => {
                this.isLoading = false;
                if (response) {
                    this.refreshData();
                    this.showFlashMessage(
                        'success',
                        this.translocoService.translate(
                            'Clients.emails-sent',
                            {}
                        )
                    );
                } else {
                    this.refreshData();
                    this.showFlashMessage(
                        'error',
                        this.translocoService.translate(
                            'Clients.emails-sent-error',
                            {}
                        )
                    );
                }
                this._changeDetectorRef.markForCheck();
            });
    }

    refreshData(): void {
        this.isLoading = true;
        this._managmentService
            .getAlClientsAddresses()
            .subscribe((response: any) => {
                this.isLoading = false;
                if (response) {
                    this.clients = response.clients;
                }
                this._changeDetectorRef.markForCheck();
            });
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
