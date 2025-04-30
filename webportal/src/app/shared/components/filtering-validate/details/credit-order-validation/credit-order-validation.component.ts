/* eslint-disable @typescript-eslint/naming-convention */
import { ChangeDetectorRef, Component, Input, OnInit } from '@angular/core';
import { MatDialog, MatDialogConfig } from '@angular/material/dialog';
import { Router, ActivatedRoute } from '@angular/router';
import { FuseMediaWatcherService } from '@fuse/services/media-watcher';
import { ClientsService } from 'app/modules/configurations/clients/clients.service';
import {
    PrimaveraOrder,
    PrimaveraInvoice,
    InvoiceTotal,
} from 'app/modules/configurations/clients/clients.types';
import { ViewClientPrimaveraInvoicesComponent } from 'app/modules/configurations/clients/ratings-table/view-client-primavera-invoices/view-client-primavera-invoices.component';
import { ViewClientPrimaveraOrdersComponent } from 'app/modules/configurations/clients/ratings-table/view-client-primavera-orders/view-client-primavera-orders.component';
import { OrderService } from 'app/shared/components/confirm-order-address/order.service';
import { FlashMessageService } from 'app/shared/components/flash-message/flash-message.service';
import { Subject } from 'rxjs';
import { takeUntil } from 'rxjs/operators';
import { OrderDTO } from '../details.types';

@Component({
    selector: 'app-credit-order-validation',
    templateUrl: './credit-order-validation.component.html',
    styleUrls: ['./credit-order-validation.component.scss'],
})
export class CreditOrderValidationComponent implements OnInit {
    @Input() order: OrderDTO;
    @Input() isLoadingClient: boolean;

    isLoading: boolean = false;
    isUpdatingCredit: boolean = false;

    orders: PrimaveraOrder[] = [];
    ordersTotal: number = 0;
    invoices: PrimaveraInvoice[] = [];
    invoicesTotal: InvoiceTotal;
    usedPlafound: number = 0;

    creditPercentage: number = 0;

    drawerMode: 'over' | 'side' = 'side';
    drawerOpened: boolean = true;

    private _unsubscribeAll: Subject<any> = new Subject<any>();

    constructor(
        private _clientService: ClientsService,
        private _cdr: ChangeDetectorRef,
        private _fuseMediaWatcherService: FuseMediaWatcherService,
        private _matDialog: MatDialog,
        private _orderService: OrderService,
        private _fms: FlashMessageService,
        private _router: Router,
        private _route: ActivatedRoute
    ) {}

    ngOnInit(): void {
        this.fetchClientOrders();

        // Subscribe to media changes
        this._fuseMediaWatcherService.onMediaChange$
            .pipe(takeUntil(this._unsubscribeAll))
            .subscribe(({ matchingAliases }) => {
                // Set the drawerMode and drawerOpened
                if (matchingAliases.includes('lg')) {
                    this.drawerMode = 'side';
                    this.drawerOpened = true;
                } else {
                    this.drawerMode = 'over';
                    this.drawerOpened = false;
                }

                // Mark for check
                this._cdr.markForCheck();
            });
    }

    fetchClientOrders(): void {
        this.isLoading = true;
        this._clientService
            .getClientPendingPrimaveraOrdersAndInvoices(this.order.client.code)
            .subscribe((response) => {
                if (response.result_code <= 0) {
                    if (response.result_code === -72) {
                        // primavera api error
                        this._fms.nterror(response.result);
                        this.isLoading = false;
                        return;
                    }

                    this._fms.error('error-server');
                    this.isLoading = false;
                    return;
                }
                const { orders, invoices, invoices_total, orders_total } =
                    response;
                this.orders = orders;
                this.ordersTotal = orders_total;
                this.invoices = invoices;
                this.invoicesTotal = invoices_total;
                this.usedPlafound =
                    this.ordersTotal + this.invoicesTotal.valor_pendente;
                this.isLoading = false;
                this.creditPercentage = this.getCreditUsagePercentage();
                this._cdr.markForCheck();
            });
    }

    // Add this method to your client-details.component.ts file
    getCreditUsagePercentage(): number {
        if (!this.order.client?.primavera_client?.plafoundCesce) {
            return 0;
        }

        // Calculate based on client's current debt vs total credit
        const currentDebt = this.usedPlafound || 0;
        const plafound =
            Number(this.order.client.primavera_client.plafoundCesce) || 0;

        if (plafound <= 0) {
            return 0;
        }

        const percentage = Math.min(
            Math.round((currentDebt / plafound) * 100),
            100
        );

        return percentage;
    }

    getColorByPercentage(percentage: number): string {
        if (percentage > 80) {
            return 'red';
        }

        if (percentage > 50) {
            return 'yellow';
        }

        return 'green';
    }
    approveCredit(): void {
        this.updateCredit(true);
    }

    rejectCredit(): void {
        this.updateCredit(false);
    }

    updateCredit(accepted: boolean): void {
        this.isUpdatingCredit = true;
        this._orderService.updateCredit(this.order.token, accepted).subscribe(
            (response) => {
                if (response.result_code <= 0) {
                    // handle error
                    this._fms.error('Order.credit-approved-error');
                    return;
                }

                const message = accepted
                    ? 'Order.credit-approved'
                    : 'Order.credit-rejected';

                this._fms.success(message);

                this._router.navigate(['..'], {
                    relativeTo: this._route,
                });
            },
            (error) => {
                // handle error
                this._fms.error('Order.credit-approved-error');
            },
            () => {
                // close the dialog
                this.isUpdatingCredit = false;
                this._cdr.markForCheck();
            }
        );
    }

    viewClientPastOrders(): void {
        const dialogConfig: MatDialogConfig = this.getDialogConfig();
        dialogConfig.data = {
            client: this.order.client,
        };

        const dialogRef = this._matDialog.open(
            ViewClientPrimaveraOrdersComponent,
            dialogConfig
        );

        dialogRef.afterClosed().subscribe((result) => {
            if (result) {
                // no action to do inside
            }
        });
    }

    viewClientInvoices(): void {
        const dialogConfig: MatDialogConfig = this.getDialogConfig();
        dialogConfig.data = {
            client: this.order.client,
        };

        const dialogRef = this._matDialog.open(
            ViewClientPrimaveraInvoicesComponent,
            dialogConfig
        );

        dialogRef.afterClosed().subscribe((result) => {
            if (result) {
                // no action to do inside
            }
        });
    }

    getDialogConfig(): MatDialogConfig {
        return {
            panelClass: 'no-padding-dialog',
            maxHeight: '80vh',
            height: '80vh',
            maxWidth: '80vw',
            width: '80vw',
        };
    }
}
