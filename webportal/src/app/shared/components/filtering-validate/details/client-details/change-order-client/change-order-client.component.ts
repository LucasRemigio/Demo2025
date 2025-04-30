/* eslint-disable @typescript-eslint/naming-convention */
import { ChangeDetectorRef, Component, Inject, OnInit } from '@angular/core';
import {
    AbstractControl,
    FormBuilder,
    FormGroup,
    ValidatorFn,
    Validators,
} from '@angular/forms';
import { MatDialogRef, MAT_DIALOG_DATA } from '@angular/material/dialog';
import { TranslocoService } from '@ngneat/transloco';
import { ClientsService } from 'app/modules/configurations/clients/clients.service';
import {
    Client,
    ClientsResponse,
} from 'app/modules/configurations/clients/clients.types';
import { OrderService } from 'app/shared/components/confirm-order-address/order.service';
import { FlashMessageService } from 'app/shared/components/flash-message/flash-message.service';
import { Subject } from 'rxjs';
import { debounceTime } from 'rxjs/operators';
import { ChangeClientData, ClientProductsUpdate } from '../../details.types';

@Component({
    selector: 'app-change-order-client',
    templateUrl: './change-order-client.component.html',
    styleUrls: ['./change-order-client.component.scss'],
})
export class ChangeOrderClientComponent implements OnInit {
    client?: Client;
    availableClients: Client[] = [];
    filteredClients: Client[] = [];

    selectedClient?: Client;
    searchTerm = new Subject<string>();
    orderToken: string = '';
    orderNif: number;

    isLoading: boolean = true;
    composeForm: FormGroup;

    flashMessage: 'success' | 'error' | null = null;
    flashMessageText: string;

    constructor(
        private _cdr: ChangeDetectorRef,
        private _transloco: TranslocoService,
        private _clientsService: ClientsService,
        private _ordersService: OrderService,
        private dialogRef: MatDialogRef<ChangeOrderClientComponent>,
        private _formBuilder: FormBuilder,
        private _fm: FlashMessageService,
        @Inject(MAT_DIALOG_DATA)
        public injectedClient: ChangeClientData
    ) {
        this.client = injectedClient.client;
        this.orderToken = injectedClient.orderToken;
        this.orderNif = injectedClient.orderNif;
    }

    ngOnInit(): void {
        this.isLoading = true;

        this.createForm();

        this.searchTerm.pipe(debounceTime(200)).subscribe((searchValue) => {
            this.searchClients(searchValue);
        });

        this.searchClients('');
    }
    createForm(): void {
        const clientName = this.client?.primavera_client?.nome || '';
        const clientCode = this.client?.code || '';
        const nif = this.orderNif || '';

        this.composeForm = this._formBuilder.group({
            client_name: [clientName, Validators.required],
            client_code: [clientCode, Validators.required],
            nif: [nif, [Validators.required, this.portugueseNifValidator()]],
        });
        this._cdr.markForCheck();
    }

    /**
     * Show flash message
     */
    showFlashMessage(type: 'success' | 'error', textMsg: string): void {
        // Show the message
        this.flashMessage = type;
        this.flashMessageText = textMsg;

        // Mark for check
        this._cdr.markForCheck();

        // Hide it after 3 seconds
        setTimeout(() => {
            this.flashMessage = null;
            this.flashMessageText = null;

            // Mark for check
            this._cdr.markForCheck();
        }, 5000);
    }

    getFormattedClientName(client?: Client): string {
        if (!client) {
            return '-';
        }

        return `${client.code} - ${client.primavera_client.nome}`;
    }

    close(): void {
        this.dialogRef.close();
    }

    onSearchChange(searchValue: string): void {
        this.searchTerm.next(searchValue);
    }

    searchClients(searchValue: string): void {
        this._clientsService.getClientsBySearch(searchValue).subscribe(
            (response: ClientsResponse) => {
                if (response.result_code <= 0) {
                    this._fm.error('error-loading-list');
                    this.isLoading = false;
                    return;
                }
                this.availableClients = response.clients;
                this.filteredClients = response.clients;

                this.isLoading = false;
            },
            (error) => {
                this.isLoading = false;
            }
        );
    }

    selectClient(client: Client): void {
        this.selectedClient = client;
        const name = this.getFormattedClientName(client);
        this.composeForm.get('client_name').patchValue(name);
        this.composeForm.get('client_code').patchValue(client.code);
        this.composeForm
            .get('nif')
            .patchValue(client.primavera_client.contribuinte);
    }

    get isFormValid(): boolean {
        // check first if compose form as even been touched, we dont want to open
        // the dialog already with errors
        if (!this.composeForm.touched) {
            return true;
        }

        this.composeForm.markAllAsTouched();
        return this.composeForm.valid;
    }

    saveChanges(): void {
        // validate form
        this.updateOrderClient();
    }

    updateOrderClient(): void {
        this.isLoading = true;
        // validate the form is valid
        if (!this.isFormValid) {
            this.isLoading = false;
            return;
        }

        const code = this.composeForm.get('client_code').value;
        const nif = this.composeForm.get('nif').value;

        this._ordersService.patchClient(this.orderToken, code, nif).subscribe(
            (result) => {
                this.isLoading = false;

                if (result.result_code <= 0) {
                    this.showFlashMessage(
                        'error',
                        this._transloco.translate('change-segment-error')
                    );
                    return;
                }

                const dialogCloseResponse: ClientProductsUpdate = {
                    client: this.selectedClient,
                    nif: nif,
                    products: result.products,
                    address_details: result.address_filling_details,
                    destination_details: result.destination_details,
                    logistic_rating: result.logistic_rating,
                };

                this.dialogRef.close(dialogCloseResponse);
            },
            (error) => {
                this.showFlashMessage(
                    'error',
                    this._transloco.translate('change-segment-error')
                );
                this.isLoading = false;
            }
        );
    }

    portugueseNifValidator(): ValidatorFn {
        return (control: AbstractControl): { [key: string]: any } | null => {
            const nif: number = control.value;
            if (!nif) {
                // If empty, let required validator handle it
                return null;
            }

            // Check if NIF is a number
            if (isNaN(nif)) {
                return { invalidNif: 'NIF must be a number' };
            }

            const nifString = nif.toString();
            // Check if NIF is exactly 9 digits
            if (!/^\d{9}$/.test(nifString)) {
                return { invalidNif: 'NIF must be exactly 9 digits' };
            }
            let total = 0;
            // Calculate weighted sum of the first 8 digits
            for (let i = 0; i < 8; i++) {
                total += parseInt(nifString.charAt(i), 10) * (9 - i);
            }
            const remainder = total % 11;
            const checkDigit = remainder < 2 ? 0 : 11 - remainder;
            if (parseInt(nifString.charAt(8), 10) !== checkDigit) {
                return { invalidNif: 'Invalid NIF check digit' };
            }
            return null; // NIF is valid
        };
    }
}
