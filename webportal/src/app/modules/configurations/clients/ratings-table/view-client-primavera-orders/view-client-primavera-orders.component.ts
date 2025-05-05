import {
    ChangeDetectorRef,
    Component,
    Inject,
    Input,
    OnChanges,
    OnInit,
    Optional,
    ViewEncapsulation,
} from '@angular/core';
import { MatDialogRef, MAT_DIALOG_DATA } from '@angular/material/dialog';
import { ClientsService } from '../../clients.service';
import { Client, PrimaveraOrder } from '../../clients.types';

@Component({
    selector: 'app-view-client-primavera-orders',
    templateUrl: './view-client-primavera-orders.component.html',
    styleUrls: ['./view-client-primavera-orders.component.scss'],
    encapsulation: ViewEncapsulation.None,
})
export class ViewClientPrimaveraOrdersComponent implements OnInit, OnChanges {
    @Input() orders: PrimaveraOrder[] = [];
    @Input() clientTotal: number = 0;
    client: Client;
    isPopup: boolean = true;
    isLoading: boolean = false;

    constructor(
        private _clientsService: ClientsService,
        private _cdr: ChangeDetectorRef,

        @Optional()
        private _matDialogRef: MatDialogRef<ViewClientPrimaveraOrdersComponent>,
        @Optional()
        @Inject(MAT_DIALOG_DATA)
        public data: { client: Client }
    ) {
        if (data) {
            this.client = data.client;
        } else {
            this.isPopup = false;
        }
    }

    ngOnInit(): void {
        if (this.client && this.isPopup) {
            this.fetchOrders();
        }
    }

    ngOnChanges(): void {
        if (this.client && this.isPopup) {
            this.fetchOrders();
        }
    }

    fetchOrders(): void {
        this.isLoading = true;
        this._clientsService
            .getClientPrimaveraOrders(this.client.code)
            .subscribe(
                (response) => {
                    console.log(response);
                    this.orders = response.primavera_orders;
                    console.log(this.orders);
                    this.clientTotal = response.total;
                },
                (error) => {},
                () => {
                    this.isLoading = false;
                    this._cdr.markForCheck();
                }
            );
    }

    close(): void {
        this._matDialogRef.close();
    }
}
