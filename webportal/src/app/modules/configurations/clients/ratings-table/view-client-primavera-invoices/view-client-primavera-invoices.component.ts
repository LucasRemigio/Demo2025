import {
    Component,
    Inject,
    Input,
    OnChanges,
    OnInit,
    Optional,
} from '@angular/core';
import { MatDialogRef, MAT_DIALOG_DATA } from '@angular/material/dialog';
import { ClientsService } from '../../clients.service';
import {
    AveragePaymentTime,
    Client,
    InvoiceTotal,
    PrimaveraInvoice,
} from '../../clients.types';

@Component({
    selector: 'app-view-client-primavera-invoices',
    templateUrl: './view-client-primavera-invoices.component.html',
    styleUrls: ['./view-client-primavera-invoices.component.scss'],
})
export class ViewClientPrimaveraInvoicesComponent implements OnInit, OnChanges {
    @Input() invoices: PrimaveraInvoice[];
    @Input() invoicesTotal: InvoiceTotal;
    averagePaymentTime?: AveragePaymentTime;
    client: Client;
    isLoading: boolean = false;
    isPopup: boolean = true;

    constructor(
        private _clientsService: ClientsService,
        @Optional()
        private _matDialogRef: MatDialogRef<ViewClientPrimaveraInvoicesComponent>,
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
            this.fetchPrimaveraInvoices();
        }
    }

    ngOnChanges(): void {
        if (this.client && this.isPopup) {
            this.fetchPrimaveraInvoices();
        }
    }

    fetchPrimaveraInvoices(): void {
        this.isLoading = true;

        this._clientsService
            .getClientPrimaveraInvoices(this.client.code)
            .subscribe(
                (response) => {
                    this.invoices = response.primavera_invoices;
                    this.averagePaymentTime = response.average_payment_time;
                    this.invoicesTotal = response.invoices_total;
                },
                (error) => {},
                () => {
                    this.isLoading = false;
                }
            );
    }

    close(): void {
        this._matDialogRef.close();
    }
}
