/* eslint-disable @typescript-eslint/naming-convention */
import {
    Component,
    EventEmitter,
    Input,
    OnChanges,
    OnInit,
    Output,
} from '@angular/core';
import { FormGroup, FormBuilder, Validators } from '@angular/forms';
import { MatDialog, MatDialogConfig } from '@angular/material/dialog';
import { Client } from 'app/modules/configurations/clients/clients.types';
import { AddressUpdateService } from 'app/shared/components/confirm-order-address/address-update.service';
import {
    CurrentMapsAddress,
    DestinationDetails,
} from 'app/shared/components/confirm-order-address/confirm-order-address.types';
import { FlashMessageService } from 'app/shared/components/flash-message/flash-message.service';
import {
    ChangeClientData,
    ClientProductsUpdate,
    OrderRatingChangeRequestDto,
} from '../details.types';
import { ChangeOrderClientComponent } from './change-order-client/change-order-client.component';

@Component({
    selector: 'app-client-details',
    templateUrl: './client-details.component.html',
    styleUrls: ['./client-details.component.scss'],
})
export class ClientDetailsComponent implements OnInit, OnChanges {
    @Input() client?: Client | null = null;
    @Input() orderToken: string | null;
    @Input() orderNif: number | null;
    @Input() ratingChangeRequests: OrderRatingChangeRequestDto[] = [];
    @Input() isDisabled: boolean = false;

    // The client is nullable, meaning the order can not have a client associated
    // To check that, we need to check if the client is null because it does not exist,
    // or because it is still loading
    @Input() isLoadingClient: boolean;

    @Output() clientChange = new EventEmitter<ClientProductsUpdate>();

    // This variable is used for quick access to the client code + client name outside this component
    clientName: string = '';

    composeForm: FormGroup;

    constructor(
        private _formBuilder: FormBuilder,
        private _matDialog: MatDialog,
        private _flashMessageService: FlashMessageService,
        private _addressUpdateService: AddressUpdateService
    ) {}

    ngOnInit(): void {}

    ngOnChanges(): void {
        // only create form if the change was the client input
        this.createForm();
    }

    /* ==============================================================================
     *   PUBLIC METHODS
     * ==============================================================================
     */

    editClient(): void {
        const data: ChangeClientData = {
            client: this.client,
            orderToken: this.orderToken,
            orderNif: this.orderNif,
        };

        // open popup for edit
        const dialogConfig: MatDialogConfig = {
            maxHeight: '50vh',
            minHeight: '30vh',
            height: 'auto',
            maxWidth: '80vw',
            minWidth: '40vw',
            width: 'auto',
            data: data,
        };

        const dialogRef = this._matDialog.open(
            ChangeOrderClientComponent,
            dialogConfig
        );

        dialogRef.afterClosed().subscribe((result: ClientProductsUpdate) => {
            if (result) {
                // Refresh data after successful create or update
                this._flashMessageService.success('edit-client-success');

                const selectedClient = result.client;
                // change the client to the one that was edited
                const clientName = `${selectedClient.code} - ${selectedClient.primavera_client.nome}`;
                this.clientName = clientName;

                this.client = selectedClient;
                this.clientChange.emit(result);

                if (result.destination_details) {
                    this._addressUpdateService.emitLogisticRatingUpdate(
                        result.logistic_rating
                    );
                }

                this.orderNif = result.nif;

                this.composeForm.get('client_name')?.setValue(clientName);
                this.composeForm.get('nif')?.setValue(result.nif);
                this.composeForm.markAsPristine();
            }
        });
    }

    emitDestinationDetailsChanged(destination: DestinationDetails): void {
        const currentMapsAddress: CurrentMapsAddress = {
            maps_address: destination.destination,
            distance: destination.distance,
            travel_time: destination.duration,
        };

        this._addressUpdateService.emitCurrentDestinationDetailsUpdate(
            currentMapsAddress
        );
    }

    createForm(): void {
        this.clientName = '';
        if (this.client && !this.isLoadingClient) {
            const clientCode = this.client?.code;
            const clientName = this.client?.primavera_client.nome;
            this.clientName = `${clientCode} - ${clientName}`;
        }

        this.composeForm = this._formBuilder.group({
            // eslint-disable-next-line @typescript-eslint/naming-convention
            client_name: [
                { value: this.clientName, disabled: true },
                Validators.required,
            ],
            nif: [
                {
                    value: this.orderNif || '',
                    disabled: true,
                },
                Validators.required,
            ],
        });
    }

    touchClientField(): void {
        const clientNameControl = this.composeForm.get('client_name');

        if (!clientNameControl) {
            return;
        }

        clientNameControl.enable();
        clientNameControl.markAsTouched();

        if (!clientNameControl.invalid) {
            return;
        }

        this._flashMessageService.error('client-is-mandatory');
    }
}
