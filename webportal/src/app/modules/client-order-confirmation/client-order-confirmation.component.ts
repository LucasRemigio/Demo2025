/* eslint-disable @typescript-eslint/naming-convention */
import { ChangeDetectorRef, Component, OnInit, ViewChild } from '@angular/core';
import { ActivatedRoute, Router } from '@angular/router';
import { FuseSplashScreenService } from '@fuse/services/splash-screen';
import { translate, TranslocoService } from '@ngneat/transloco';
import { ConfirmOrderAddressComponent } from 'app/shared/components/confirm-order-address/confirm-order-address.component';
import { CurrentAddress } from 'app/shared/components/confirm-order-address/confirm-order-address.types';
import { OrderService } from 'app/shared/components/confirm-order-address/order.service';
import { EmailProductTableComponent } from 'app/shared/components/email-product-table/email-product-table.component';
import {
    FilteredEmailDTOResponse,
    OrderDTO,
    OrderProductsDTO,
} from 'app/shared/components/filtering-validate/details/details.types';
import { FlashMessageService } from 'app/shared/components/flash-message/flash-message.service';
import { ClientsService } from '../configurations/clients/clients.service';
import { FilteringService } from '../filtering/filtering.service';
import { EMAIL_CATEGORIES, EMAIL_STATUSES } from '../filtering/filtering.types';
import { Client } from '../configurations/clients/clients.types';
@Component({
    selector: 'app-client-order-confirmation',
    templateUrl: './client-order-confirmation.component.html',
    styleUrls: ['./client-order-confirmation.component.scss'],
})
export class ClientOrderConfirmationComponent implements OnInit {
    @ViewChild(EmailProductTableComponent)
    productTable!: EmailProductTableComponent;

    @ViewChild(ConfirmOrderAddressComponent)
    confirmOrderAddress!: ConfirmOrderAddressComponent;

    token: string = '';
    isLoading: boolean = true;
    isLoadingClient: boolean = true;
    isSaving: boolean = false;
    statusList = EMAIL_STATUSES;
    categoryList = EMAIL_CATEGORIES;

    data: FilteredEmailDTOResponse;
    address: CurrentAddress;
    client: Client;

    orderProducts: OrderProductsDTO[] = [];
    order: OrderDTO;

    addressUpdatedSuccessfully: boolean = false;

    currentStep: number = 0;
    drawerMode: 'over' | 'side' = 'side';
    drawerOpened: boolean = true;
    isSidebarOpen: boolean = true;
    steps = [
        {
            order: 0,
            title: translate('order-address'),
            subtitle: translate('order-address-details'),
        },
        {
            order: 1,
            title: translate('Order.product'),
            subtitle: translate('Dashboard.product-validation'),
        },
    ];
    totalSteps: number = this.steps.length;

    flashMessage: 'success' | 'error' | null = null;
    flashMessageText: string | null = null;

    constructor(
        private _route: ActivatedRoute,
        private _filteringService: FilteringService,
        private _fuseSplashScreenService: FuseSplashScreenService,
        private _router: Router,
        private _activatedRoute: ActivatedRoute,
        private _changeDetectorRef: ChangeDetectorRef,
        private translocoService: TranslocoService,
        private _orderService: OrderService,
        private _fm: FlashMessageService,
        private _clientService: ClientsService
    ) {}

    ngOnInit(): void {
        this.isLoading = true;
        this._fuseSplashScreenService.show();

        // get the token from the url
        this.token = this._route.snapshot.paramMap.get('token') || '';

        this._filteringService.getOrderByTokenNoAuth(this.token).subscribe(
            (data: FilteredEmailDTOResponse) => {
                if (data.result_code < 0) {
                    // Handle error
                    this._fuseSplashScreenService.hide();
                    this.isLoading = false;
                    return;
                }
                // Get the email
                this.data = data;
                this.orderProducts = data.order.products;
                this.order = data.order;
                this.getCurrentAddress(this.order);

                this.fetchClient(this.order.client?.token);

                const step: string =
                    this._route.snapshot.queryParamMap.get('step');
                if (step) {
                    const stepNum = Number(step);
                    this.goToStep(stepNum - 1);
                }
            },
            (error) => {
                this.isLoading = false;
                this._fuseSplashScreenService.hide();
                this._changeDetectorRef.detectChanges();
                this._fm.error('error-loading-list');
            },
            () => {
                this.isLoading = false;
                this._fuseSplashScreenService.hide();
                this._changeDetectorRef.detectChanges();
            }
        );
    }

    fetchClient(clientToken?: string): void {
        this.isLoadingClient = true;
        if (!clientToken) {
            this.isLoadingClient = false;
            return;
        }

        this._clientService.getClientByTokenNoAuth(clientToken).subscribe(
            (response) => {
                this.client = response.client;
                this.isLoadingClient = false;
            },
            (error) => {
                this.isLoadingClient = false;
                this._fm.error('error-loading-list');
            },
            () => {
                this.isLoadingClient = false;
                this._changeDetectorRef.detectChanges();
            }
        );
    }

    getCurrentAddress(order: OrderDTO): void {
        let cp4 = '';
        let cp3 = '';

        if (
            order.postal_code !== null &&
            order.postal_code !== undefined &&
            order.postal_code.indexOf('-') !== -1
        ) {
            const postalCodes = order.postal_code.split('-');
            cp4 = postalCodes[0];
            cp3 = postalCodes[1];
        }

        this.address = {
            is_delivery: order.is_delivery,
            district_dd: order.district_dd,
            municipality_cc: order.municipality_cc,
            address: order.address,
            postal_code_cp4: cp4,
            postal_code_cp3: cp3,
            city: order.city,
            transport_id: order.transport?.id || 0,
        };
    }

    get isAddressValid(): boolean {
        if (!this.address.is_delivery) {
            return true;
        }

        // it is valid if district_dd and municipality_cc are not null
        if (
            this.address.district_dd !== null ||
            this.address.municipality_cc !== null
        ) {
            return true;
        }

        // but also if all of the other parameters are filled
        if (
            this.address.address !== null &&
            this.address.city !== null &&
            this.address.postal_code_cp4 !== null &&
            this.address.postal_code_cp3 !== null
        ) {
            return true;
        }

        return false;
    }

    get isEmailAwaitingValidation(): boolean {
        if (this.data.order.is_adjudicated) {
            return false;
        }

        if (
            this.data.order.status.id ===
            EMAIL_STATUSES.CONFIRMADO_POR_CLIENTE.id
        ) {
            return false;
        }

        return true;
    }

    get clientCar(): string {
        if (!this.client) {
            return '';
        }

        if (!this.client.primavera_client) {
            return '';
        }

        return this.client.primavera_client.carro;
    }

    get headerTitle(): string {
        return this.data.order.is_draft
            ? translate('Filtering.quote-request')
            : translate('Filtering.order-request');
    }

    get isMobile(): boolean {
        const windowSize = window.innerWidth;
        return windowSize < 768;
    }

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

    updateAddress(address: CurrentAddress): void {
        this.addressUpdatedSuccessfully = true;
        this.updateOrderAddress(address);
    }

    updateOrderAddress(address: CurrentAddress): void {
        this.order.is_delivery = address.is_delivery;
        this.order.address = address.address;
        this.order.postal_code =
            address.postal_code_cp4 + '-' + address.postal_code_cp3;
        this.order.city = address.city;
        this.order.municipality_cc = address.municipality_cc;
        this.order.district_dd = address.district_dd;
        // create transport if non existent
        if (!this.order.transport) {
            this.order.transport = {
                id: null,
                name: null,
                slug: null,
                description: null,
            };
        }
        this.order.transport.id = address.transport_id;
    }

    async confirmProducts(): Promise<void> {
        this.isSaving = true;

        let error: boolean = false;
        try {
            const products = this.productTable.localProducts;

            // Wait for updateProducts to complete
            const success = await this.productTable.updateProducts();

            if (!success) {
                // If the update failed, stop further processing
                // show error message
                this._fm.error('products-update-error');
                error = true;
                return;
            }
        } catch (exception) {
            return;
        } finally {
            if (error) {
                this.isSaving = false;
            }
        }

        this._orderService.adjudicateOrder(this.order.token).subscribe(
            (response) => {
                if (response.result_code < 0) {
                    this._fm.error('Order.created-error');
                    return;
                }
                this._fm.success('Order.created-success');
                setTimeout(() => {
                    this._router.navigate(['./success'], {
                        relativeTo: this._activatedRoute,
                    });
                    this.isSaving = false;
                }, 2000);
            },
            (exception) => {
                this.isSaving = false;
                this._fm.error('Order.created-error');
            }
        );
    }

    goToStep(step: number): void {
        if (this.currentStep === this.steps.length - 1) {
            return;
        }

        if (this.currentStep === 0 && !this.isAddressValid) {
            this._fm.error('order-address-invalid-format');
            return;
        }

        this.currentStep = step;

        // add the step to the url
        // save the step on the parameters of the url
        this._router.navigate([], {
            relativeTo: this._route,
            queryParams: { step: step + 1 },
            queryParamsHandling: 'merge', // preserves existing query parameters
        });
    }

    goToNextStep(): void {
        this.goToStep(this.currentStep + 1);
    }

    toggleSidebar(): void {
        this.isSidebarOpen = !this.isSidebarOpen;
    }
}
