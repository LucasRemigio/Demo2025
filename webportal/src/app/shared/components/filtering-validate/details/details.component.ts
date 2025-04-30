/* eslint-disable arrow-parens */
/* eslint-disable @typescript-eslint/naming-convention */
import {
    ChangeDetectionStrategy,
    ChangeDetectorRef,
    Component,
    Inject,
    Input,
    OnDestroy,
    OnInit,
    ViewEncapsulation,
} from '@angular/core';
import { DatePipe, DOCUMENT, Location } from '@angular/common';
import { Subject } from 'rxjs';
import { takeUntil } from 'rxjs/operators';
import { FuseMediaWatcherService } from '@fuse/services/media-watcher';
import { FilteringService } from 'app/modules/filtering/filtering.service';
import { ActivatedRoute, Router } from '@angular/router';
import { TranslocoService } from '@ngneat/transloco';
import { FuseSplashScreenService } from '@fuse/services/splash-screen';
import {
    EMAIL_STATUSES,
    EmailAttachment,
    EMAIL_CATEGORIES,
} from 'app/modules/filtering/filtering.types';
import { Category } from 'app/modules/common/common.types';
import {
    ClientProductsUpdate,
    FilteredEmail,
    FilteredEmailDTOResponse,
    OrderDTO,
    OrderProductsDTO,
} from './details.types';
import { ClientsService } from 'app/modules/configurations/clients/clients.service';
import { ProductUpdateService } from '../../email-product-table/product-update.service';
import {
    CurrentAddress,
    CurrentMapsAddress,
    OrderRatingItem,
} from '../../confirm-order-address/confirm-order-address.types';
import { AddressUpdateService } from '../../confirm-order-address/address-update.service';
import { FlashMessageService } from '../../flash-message/flash-message.service';
import {
    OrderDocument,
    OrderProductItem,
    OrderToValidateResponse,
    ProductUnit,
    ProductUnitsResponse,
    Step,
} from '../../email-product-table/products.types';
import { ProductsService } from '../../email-product-table/products.service';
import { UserService } from 'app/core/user/user.service';
import { OrderObservationsItem } from 'app/modules/orders/order.types';
import { ShowEmailPopupServiceService } from 'app/layout/common/show-email/show-email-popup-service.service';
import { OrderService } from '../../confirm-order-address/order.service';
import {
    MatDialog,
    MatDialogConfig,
    MatDialogRef,
} from '@angular/material/dialog';
import { SelectCancelReasonComponent } from '../select-cancel-reason/select-cancel-reason.component';

@Component({
    selector: 'app-validate-details',
    templateUrl: './details.component.html',
    styleUrls: ['./details.component.scss'],
    styles: [
        /* language=SCSS */
        `
            .products-managment-grid {
                grid-template-columns: 70% 20% 10%;
            }
        `,
    ],
    providers: [DatePipe],
    encapsulation: ViewEncapsulation.None,
    changeDetection: ChangeDetectionStrategy.OnPush,
})
export class ValidateDetailsComponent implements OnInit, OnDestroy {
    @Input() isCreateNew: boolean = false;
    @Input() isDraft: boolean = true;

    flashMessage: 'success' | 'error' | null = null;
    flashMessageText: string;
    isToDisableForms: boolean = false;
    isPendingClientApproval: boolean = false;

    categories: Category[];
    filtered: FilteredEmail;
    attachments: EmailAttachment[];
    productUnits: ProductUnit[] = [];

    currentStep: number = 0;
    drawerMode: 'over' | 'side' = 'side';
    drawerOpened: boolean = true;
    isEmailDrawerOpened: boolean = false;

    statusList = EMAIL_STATUSES;
    categoryList = EMAIL_CATEGORIES;

    steps: Step[] = [];
    fixedSteps = {
        email: 0,
        attachments: 1,
        client: 2,
        address: 3,
        products: 4,
        documents: 5,
    };

    shwoAllButtons: boolean = true;
    disabled: boolean = false;

    order: OrderDTO;
    currentProducts?: OrderProductsDTO[] = [];
    currentAddress?: CurrentAddress = null;
    currentObservations?: OrderObservationsItem;

    sum: number = 0;
    _formFieldService: any;
    isLoading: boolean;
    isLoadingClient: boolean;

    private _unsubscribeAll: Subject<any> = new Subject<any>();

    /**
     * Constructor
     */
    constructor(
        @Inject(DOCUMENT) private _document: Document,
        private _filteringService: FilteringService,
        private _cdr: ChangeDetectorRef,
        private _fuseMediaWatcherService: FuseMediaWatcherService,
        private readonly translocoService: TranslocoService,
        private _fuseSplashScreenService: FuseSplashScreenService,
        private _route: ActivatedRoute,
        private _clientService: ClientsService,
        private _productUpdateService: ProductUpdateService,
        private _addressUpdateService: AddressUpdateService,
        private _productsService: ProductsService,
        private _userService: UserService,
        private _fms: FlashMessageService,
        private _showEmailPopupService: ShowEmailPopupServiceService,
        private _router: Router,
        private _orderService: OrderService,
        private _activatedRoute: ActivatedRoute,
        private _matDialog: MatDialog,
        private _location: Location
    ) {}

    translate(key: string): string {
        return this.translocoService.translate(key);
    }

    // -----------------------------------------------------------------------------------------------------
    // @ Lifecycle hooks
    // -----------------------------------------------------------------------------------------------------

    /**
     * On init
     */
    async ngOnInit(): Promise<void> {
        this.isLoading = true;
        if (this.isCreateNew) {
            await this.handleCreateNew();
        } else {
            await this.fetchOrder();
        }

        if (!this.isUserAllowedOnThisPage(this.order)) {
            this._fms.warning('Order.not-allowed-to-view-in-current-state');
            this.goBack();
            return;
        }

        this.initSteps(this.isCreateNew);

        const step: string = this._route.snapshot.queryParamMap.get('step');
        if (step) {
            const stepNum = Number(step);
            this.goToStep(stepNum - 1);
        }

        this.isLoading = false;
        this._fuseSplashScreenService.hide();
        this._cdr.markForCheck();

        this.fetchProductUnits();
        this.subscribeToListeningEvents();
    }

    isUserAllowedOnThisPage(order: OrderDTO): boolean {
        const isUserSupervisor = this._userService.isSupervisor();

        const adminStatuses = [
            EMAIL_STATUSES.PENDENTE_APROVACAO_ADMINISTRACAO.id,
            EMAIL_STATUSES.PENDENTE_APROVACAO_CREDITO.id,
        ];

        if (!isUserSupervisor && adminStatuses.includes(order.status.id)) {
            return false;
        }

        return true;
    }

    async fetchOrder(): Promise<void> {
        const token = this._route.snapshot.paramMap.get('id');
        const response = await this.fetchOrderByToken(token);

        // Get the email
        if (response.result_code <= 0) {
            this.isLoading = false;
            this._fms.error('error-loading-list');
            return;
        }

        // Get the email
        this.filtered = response.filtered_email;
        this.attachments = response.email_attachments;
        this.order = response.order;
        // format cc and bcc

        if (this.filtered) {
            this._showEmailPopupService.showEmailButton();
        }

        this.isToDisableForms =
            this._userService.isSupervisor() &&
            this.isOrderPendingAdminApproval;

        this.isPendingClientApproval = this.isEmailPendingClientApproval();
    }

    async handleCreateNew(): Promise<void> {
        // Check if there is a token on the URL to fetch that order.
        const token: string = this._route.snapshot.queryParamMap.get('token');
        try {
            if (token) {
                await this.fetchOrderByToken(token);
            } else {
                await this.createEmptyOrder(this.isDraft);
            }
        } catch (error) {
            console.error('Error creating/fetching order:', error);
            return;
        }
    }

    fetchOrderByToken(token: string): Promise<OrderToValidateResponse> {
        return new Promise((resolve, reject) => {
            this._orderService.getOrderByTokenToValidate(token).subscribe(
                (response) => {
                    if (response.result_code <= 0) {
                        // Handle error appropriately.
                        reject(new Error('Failed to fetch order.'));
                        return;
                    }
                    this.order = response.order;
                    // If the order has a client, fetch it.
                    if (this.order.client) {
                        this.fetchClient();
                    }

                    // Complete callback
                    resolve(response);
                },
                (error) => {
                    reject(error);
                },
                () => {
                    this._cdr.markForCheck();
                }
            );
        });
    }

    createEmptyOrder(isDraft: boolean): Promise<void> {
        return new Promise((resolve, reject) => {
            this._orderService.createEmptyOrder(isDraft).subscribe(
                (response) => {
                    if (response.result_code <= 0) {
                        // Handle error appropriately.
                        reject(new Error('Failed to create empty order.'));
                        return;
                    }
                    this.order = response.order;

                    // Update URL with the order token.
                    this._router.navigate([], {
                        relativeTo: this._route,
                        queryParams: { token: this.order.token },
                        queryParamsHandling: 'merge',
                    });
                },
                (error) => {
                    reject(error);
                },
                () => {
                    // Complete callback
                    this.isLoading = false;
                    this._cdr.markForCheck();
                    resolve();
                }
            );
        });
    }

    saveAndClose(): void {}

    getCategoryClass(slug: string): string {
        switch (slug) {
            case 'confianca':
                return 'text-blue-800 bg-blue-100 dark:text-blue-50 dark:bg-blue-500';
            case 'Error':
                return 'text-red-800 bg-red-100 dark:text-red-50 dark:bg-red-500';
            default:
                return '';
        }
    }
    /**
     * On destroy
     */
    ngOnDestroy(): void {
        // Unsubscribe from all subscriptions
        this._unsubscribeAll.next();
        this._unsubscribeAll.complete();

        this._productsService.clearCache();

        this._showEmailPopupService.closeDrawer();
        this._showEmailPopupService.hideEmailButton();
    }

    // -----------------------------------------------------------------------------------------------------
    // @ Public methods
    // -----------------------------------------------------------------------------------------------------

    isEmailPendingClientApproval(): boolean {
        return (
            this.order.status.id ===
                EMAIL_STATUSES.PENDENTE_CONFIRMACAO_CLIENTE.id &&
            this.order.is_adjudicated === false
        );
    }

    get isOrderPendingCreditApproval(): boolean {
        return (
            this.order.status.id ===
            EMAIL_STATUSES.PENDENTE_APROVACAO_CREDITO.id
        );
    }

    get isOrderPendingAdminApproval(): boolean {
        return (
            this.order.status.id ===
            EMAIL_STATUSES.PENDENTE_APROVACAO_ADMINISTRACAO.id
        );
    }

    initSteps(isNewOrder: boolean): void {
        this.steps = []; // Reset steps

        if (this.filtered) {
            // For existing orders, add the email and attachments steps.
            this.steps.push({
                order: this.fixedSteps.email,
                type: 'email',
                title: this.translate('email'),
                subtitle: this.translate('preview-email'),
            });

            this.steps.push({
                order: this.fixedSteps.attachments,
                type: 'attachments',
                title: this.translate('attachments'),
                subtitle: this.translate('preview-attachments'),
            });
        } else {
            // remove 2 steps from each fixed step
            this.fixedSteps.client = 0;
            this.fixedSteps.address = 1;
            this.fixedSteps.products = 2;
            this.fixedSteps.documents = 3;
        }

        // Add common steps for both scenarios.
        this.steps.push({
            order: this.fixedSteps.client,
            type: 'client',
            title: this.translate('Clients.Client'),
            subtitle: this.translate('Dashboard.client-confirmation'),
        });

        this.steps.push({
            order: this.fixedSteps.address,
            type: 'address',
            title: this.translate('order-address'),
            subtitle: this.translate('order-address-details'),
        });

        this.steps.push({
            order: this.fixedSteps.products,
            type: 'products',
            title: this.translate('Order.product'),
            subtitle: this.translate('Dashboard.product-validation'),
        });

        if (
            !this.isPendingClientApproval &&
            !this.isOrderPendingAdminApproval &&
            this.order.is_draft
        ) {
            this.steps.push({
                order: this.fixedSteps.documents,
                type: 'documents',
                title: this.translate('Order.documents'),
                subtitle: this.translate('Order.preview-order-documents'),
            });
        }
    }

    fetchClient(): void {
        this.isLoadingClient = true;
        this._clientService
            .getClientByCodeByOrderToken(
                this.order.client.code,
                this.order.token
            )
            .subscribe((response) => {
                this.order.client = response.client;
                this.isLoadingClient = false;
                this._cdr.markForCheck();
            });
    }

    subscribeToListeningEvents(): void {
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

        this._productUpdateService.productsUpdated
            .pipe(takeUntil(this._unsubscribeAll))
            .subscribe((updatedProducts) => {
                this.currentProducts = updatedProducts;
                console.log('updated products', updatedProducts);
            });

        this._productUpdateService.orderTotalUpdated
            .pipe(takeUntil(this._unsubscribeAll))
            .subscribe((orderTotal) => {
                this.order.order_total = orderTotal;
            });

        this._addressUpdateService.addressUpdated
            .pipe(takeUntil(this._unsubscribeAll))
            .subscribe((updatedAddress) => {
                this.currentAddress = updatedAddress;
            });

        this._addressUpdateService.logisticRatingUpdated
            .pipe(takeUntil(this._unsubscribeAll))
            .subscribe((rating) => {
                this.updateLogisticRating(rating);
            });

        this._addressUpdateService.destinationDetailsUpdated
            .pipe(takeUntil(this._unsubscribeAll))
            .subscribe((destinationDetails) => {
                this.updateCurrentDestinationDetails(destinationDetails);
            });

        this._productUpdateService.ratings$
            .pipe(takeUntil(this._unsubscribeAll))
            .subscribe((ratings) => {
                if (!ratings || ratings.length === 0) {
                    return;
                }
                this.order.ratings = ratings;
            });

        // Subscribe to drawer state
        this._showEmailPopupService.drawerOpen$
            .pipe(takeUntil(this._unsubscribeAll))
            .subscribe((open) => {
                this.isEmailDrawerOpened = open;
                this.drawerOpened = !open;
                this._cdr.markForCheck();
            });
    }

    fetchProductUnits(): void {
        this._productsService.getProductUnits().subscribe(
            (response: ProductUnitsResponse) => {
                if (response.result_code <= 0) {
                    this._fms.error('error-loading-list');
                    return;
                }

                this.productUnits = response.product_units;
            },
            (error) => {
                this._fms.error('error-loading-list');
            }
        );
    }

    updateClient(event: ClientProductsUpdate): void {
        // update client to new one
        this.order.client = event.client;

        // update order products and invalidate current products
        // create map for updates products
        const updatedProducts = new Map<number, OrderProductItem>();
        event.products.forEach((product) => {
            updatedProducts.set(product.id, product);
        });

        // map productunits
        const productUnitsMap = new Map<number, ProductUnit>();
        this.productUnits.forEach((unit) => {
            productUnitsMap.set(unit.id, unit);
        });

        this.order.products.map((product) => {
            const updatedProduct = updatedProducts.get(product.id);
            if (!updatedProduct) {
                return product;
            }
            product.calculated_price = updatedProduct.calculated_price;
            product.quantity = updatedProduct.quantity;

            // update product size
            const productSize = productUnitsMap.get(
                updatedProduct.product_unit_id
            );
            if (!productSize) {
                return product;
            }
            product.product_unit = productSize;
        });

        if (event.address_details) {
            const currentAddress: CurrentAddress = {
                is_delivery: this.order.is_delivery,
                address: event.address_details.address,
                municipality_cc: event.address_details.municipality_cc,
                district_dd: event.address_details.district_dd,
                postal_code_cp4: event.address_details.cp4,
                postal_code_cp3: event.address_details.cp3,
                city: event.address_details.city,
                transport_id:
                    this.order.transport?.id ??
                    event.address_details.transport_id,
            };
            const currentMapsAddress: CurrentMapsAddress = {
                distance: event.destination_details.distance,
                travel_time: event.destination_details.duration,
                maps_address: event.destination_details.destination,
            };

            this.updateAddress(currentAddress);
            this.updateCurrentDestinationDetails(currentMapsAddress);
        } else {
            this.cleanCurrentAddress();
        }

        this.currentProducts = [];
        this._cdr.markForCheck();
    }

    updateAddress(event: CurrentAddress): void {
        this.currentAddress = event;
        this.updateOrderAddress(this.currentAddress);
        this._cdr.markForCheck();
    }

    updateLogisticRating(event: OrderRatingItem): void {
        // find the corresponding rating from the order
        const index = this.order.ratings.findIndex(
            (r) => r.rating_type.id === event.rating_type_id
        );

        if (index <= -1) {
            console.error('Rating not found');
            return;
        }

        this.order.ratings[index].rating_discount.rating = event.rating;
    }

    updateCurrentDestinationDetails(event: CurrentMapsAddress): void {
        this.order.distance = event.distance;
        this.order.travel_time = event.travel_time;
        this.order.maps_address = event.maps_address;

        this.currentAddress.distance = event.distance;
        this.currentAddress.travel_time = event.travel_time;
        this.currentAddress.maps_address = event.maps_address;
    }

    invoiceGenerated(event: OrderDocument): void {
        if (!this.order.is_draft) {
            this.goBack();
            return;
        }

        this.goToNextStep();

        // This timeout is needed to ensure the email-products component destroys effectively,
        // sending the products to this upper component, and only then we lock the product prices
        // after we received the products from the destruction of that component
        setTimeout(() => {
            this.lockProductPrices();
        }, 500);
    }

    lockProductPrices(): void {
        this.currentProducts.forEach((product) => {
            product.price_locked_at = new Date();
            product.is_price_locked = true;
        });

        this.order.products.forEach((product) => {
            product.price_locked_at = new Date();
            product.is_price_locked = true;
        });
    }

    updateObservations(event: OrderObservationsItem): void {
        this.currentObservations = event;
        this._cdr.markForCheck();
    }

    /**
     * Go to given step
     *
     * @param step
     */
    goToStep(step: number): void {
        const clientStep = this.fixedSteps.client;
        if (
            this.currentStep <= clientStep &&
            !this.order.client &&
            step > clientStep
        ) {
            // Show an error message to the user
            this._fms.warning('Order.select-client-before-proceeding');
            step = clientStep;
        }

        const addressStep = this.fixedSteps.address;
        if (
            this.currentStep <= addressStep &&
            !this.isAddressValid() &&
            step > addressStep
        ) {
            // Show an error message to the user
            this._fms.warning('Order.fill-address-before-proceeding');
            step = addressStep;
        }

        // Set the current step
        this.currentStep = step;

        // add the step to the url
        // save the step on the parameters of the url
        this._router.navigate([], {
            relativeTo: this._route,
            queryParams: { step: step + 1 },
            queryParamsHandling: 'merge', // preserves existing query parameters
        });

        // Go to the step
        // this.librarySteps.selectedIndex = this.currentStep;

        // Mark for check
        this._cdr.markForCheck();
    }

    /**
     * Go to previous step
     */
    goToPreviousStep(): void {
        // Return if we already on the first step
        if (this.currentStep === 0) {
            return;
        }

        // Go to step
        this.goToStep(this.currentStep - 1);

        // Scroll the current step selector from sidenav into view
        this._scrollCurrentStepElementIntoView();
    }

    /**
     * Go to next step
     */
    goToNextStep(): void {
        if (!this.isClientStepValid()) {
            // Show an error message to the user
            this._fms.warning('Order.select-client-before-proceeding');
            return;
        }

        if (!this.isAddressStepValid()) {
            // Show an error message to the user
            this._fms.warning('Order.fill-address-before-proceeding');
            return;
        }

        // Return if we already on the last step
        if (this.currentStep === this.totalSteps - 1) {
            return;
        }

        // Go to step
        this.goToStep(this.currentStep + 1);

        // Scroll the current step selector from sidenav into view
        this._scrollCurrentStepElementIntoView();
    }

    goBack(): void {
        const url = this.backValidateUrl;
        this._router.navigate([url], {
            relativeTo: this._activatedRoute,
        });
    }

    get backValidateUrl(): string {
        // when we open this screen for validation, the url is either
        // quotes_budgets/validate/{token}
        // orders/validate/{token}
        // awaiting-approval/rating/{token}
        // if that is the case, we want to go back to the previous screen
        // But if it is for creation, the url is
        // quotes_budgets/create?token={token}
        // orders/create?token={token}
        // And we want to go back to quotes_budgets/validate or orders/validate

        if (this.isCreateNew) {
            return '../validate';
        }

        return '..';
    }

    get totalSteps(): number {
        return this.steps.length;
    }

    get orderNif(): number | null {
        if (this.order.client_nif) {
            return this.order.client_nif;
        }

        // in case the algorithm matches a client, we want to default to that nif
        if (this.order.client?.primavera_client?.contribuinte) {
            const primaveraNif: string =
                this.order.client.primavera_client.contribuinte;
            return Number(primaveraNif);
        }

        return null;
    }

    get clientCar(): string | null {
        if (!this.order.client) {
            return null;
        }

        if (!this.order.client.primavera_client) {
            return null;
        }

        return this.order.client.primavera_client.carro;
    }

    trackByFn(index: number, item: any): any {
        return item.id || index;
    }

    get validCurrentProducts(): OrderProductsDTO[] {
        // if current products not null, send them. else send order products
        return this.currentProducts.length > 0
            ? this.currentProducts
            : this.order.products;
    }

    get validCurrentAddress(): CurrentAddress {
        // for now we dont send any address, so just send the updated one
        if (this.currentAddress) {
            return this.currentAddress;
        }

        let cp4 = '';
        let cp3 = '';
        if (
            this.order.postal_code &&
            this.order.postal_code.indexOf('-') > -1
        ) {
            const postalCodes = this.order.postal_code.split('-');
            cp4 = postalCodes[0];
            cp3 = postalCodes[1];
        }

        const address: CurrentAddress = {
            is_delivery: this.order.is_delivery,
            address: this.order.address ?? null,
            municipality_cc: this.order.municipality_cc ?? null,
            district_dd: this.order.district_dd ?? null,
            postal_code_cp4: cp4 ?? null,
            postal_code_cp3: cp3 ?? null,
            city: this.order.city ?? null,
            transport_id: this.order.transport?.id ?? null,
            maps_address: this.order.maps_address ?? null,
            distance: this.order.distance ?? null,
            travel_time: this.order.travel_time ?? null,
        };

        return address;
    }

    cleanCurrentAddress(): void {
        this.currentAddress = null;
        this.order.address = null;
        this.order.postal_code = null;
        this.order.city = null;
        this.order.municipality_cc = null;
        this.order.district_dd = null;
        this.order.transport = null;
        this.order.maps_address = null;
        this.order.distance = null;
        this.order.travel_time = null;
    }

    get validCurrentObservations(): OrderObservationsItem {
        if (this.currentObservations) {
            return this.currentObservations;
        }

        const observations: OrderObservationsItem = {
            observations: this.order.observations ?? null,
            contact: this.order.contact ?? null,
        };

        return observations;
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
        this.order.maps_address = address.maps_address;
        this.order.distance = address.distance;
        this.order.travel_time = address.travel_time;
    }

    cancelOrder(): void {
        // Open popup to select the cancel reason
        const dialogConfig: MatDialogConfig = {
            maxHeight: '50vh',
            minWidth: '40vh',
            data: {
                orderToken: this.order.token,
            },
        };

        const dialogRef = this._matDialog.open(
            SelectCancelReasonComponent,
            dialogConfig
        );

        dialogRef.afterClosed().subscribe((selectedCancelReasonId) => {
            if (!selectedCancelReasonId) {
                // do nothing
                return;
            }

            this._fms.success('Order.success-cancel');

            this._router.navigate(['../../validate'], {
                relativeTo: this._activatedRoute,
            });
        });
    }

    // -----------------------------------------------------------------------------------------------------
    // @ Private methods
    // -----------------------------------------------------------------------------------------------------

    /**
     * Scrolls the current step element from
     * sidenav into the view. This only happens when
     * previous/next buttons pressed as we don't want
     * to change the scroll position of the sidebar
     * when the user actually clicks around the sidebar.
     *
     * @private
     */
    private _scrollCurrentStepElementIntoView(): void {
        // Wrap everything into setTimeout so we can make sure that the 'current-step' class points to correct element
        setTimeout(() => {
            // Get the current step element and scroll it into view
            const currentStepElement =
                this._document.getElementsByClassName('current-step')[0];
            if (currentStepElement) {
                currentStepElement.scrollIntoView({
                    behavior: 'smooth',
                    block: 'start',
                });
            }
        });
    }

    private isClientStepValid(): boolean {
        if (this.currentStep === 2 && !this.order.client) {
            return false;
        }

        return true;
    }

    private isAddressStepValid(): boolean {
        if (this.currentStep === 3 && !this.isAddressValid()) {
            return false;
        }
        return true;
    }

    private isAddressValid(): boolean {
        if (!this.order.is_delivery) {
            return true;
        }

        // not delivery
        if (
            !this.order.address ||
            !this.order.postal_code ||
            !this.order.city ||
            !this.order.transport?.id
        ) {
            return false;
        }

        return true;
    }
}
