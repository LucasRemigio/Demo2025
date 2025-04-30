/* eslint-disable @typescript-eslint/no-shadow */
/* eslint-disable arrow-parens */
/* eslint-disable @typescript-eslint/naming-convention */
import {
    ChangeDetectorRef,
    Component,
    EventEmitter,
    Input,
    OnChanges,
    OnDestroy,
    OnInit,
    Output,
    SimpleChanges,
} from '@angular/core';
import { ProductsService } from './products.service';
import { TranslocoService } from '@ngneat/transloco';
import {
    OrderProduct,
    OrderProductsDTO,
} from '../filtering-validate/details/details.types';
import { Subject } from 'rxjs';
import { debounceTime } from 'rxjs/internal/operators/debounceTime';
import {
    OrderTotalItem,
    ProductCatalogDto,
    ProductCatalogResponse,
    ProductConversionDTO,
    ProductUnit,
    ProductUnitsResponse,
    ProductUpdateResponse,
} from './products.types';
import { ProductUpdateService } from './product-update.service';
import { FlashMessageService } from '../flash-message/flash-message.service';

@Component({
    selector: 'app-email-product-table',
    templateUrl: './email-product-table.component.html',
    styleUrls: ['./email-product-table.component.scss'],
})
export class EmailProductTableComponent
    implements OnInit, OnChanges, OnDestroy
{
    @Input() products: OrderProductsDTO[] = [];
    @Input() orderTotal: OrderTotalItem;
    @Input() orderToken: string = '';
    @Input() isDisabled: boolean = false;
    @Input() isClient: boolean = false;

    @Output() productsUpdated: EventEmitter<OrderProductsDTO[]> =
        new EventEmitter<OrderProductsDTO[]>();

    // Deactivate quotation calculation button while loading
    isUpdatingProducts: boolean = false;

    // Searching and filtering
    filteredCatalogProducts: ProductCatalogDto[] = [];
    selectedCatalogName: string = '';
    searchTerm = new Subject<string>();
    catalogProducts: ProductCatalogDto[] = [];
    showAddProduct = false;
    lastInsertedId: number = 0;

    productUnits: ProductUnit[] = [];
    unitsMap: Map<string, ProductUnit> = new Map();

    originalProducts: OrderProductsDTO[] = []; // Store a copy of the original products for later checking of changes
    localProducts: OrderProductsDTO[] = []; // Local copy of products
    flashMessage: 'success' | 'error' | null = null;
    flashMessageText: string;
    loadingReadyCount = 0;

    constructor(
        private _changeDetectorRef: ChangeDetectorRef,
        private translocoService: TranslocoService,
        private _productsService: ProductsService,
        private _productUpdateService: ProductUpdateService,
        private _flashMessageService: FlashMessageService
    ) {
        this.searchTerm.pipe(debounceTime(200)).subscribe((searchValue) => {
            this.searchProducts(searchValue);
        });
    }

    /*
     * @Lifecycle methods
     */

    ngOnInit(): void {
        this.originalProducts = JSON.parse(JSON.stringify(this.products));

        this.fetchData();

        this.filteredCatalogProducts = [...this.catalogProducts];
    }

    fetchData(): void {
        this.searchProducts('');

        this._productsService.getProductUnits().subscribe(
            (response: ProductUnitsResponse) => {
                if (response.result_code <= 0) {
                    this._flashMessageService.error('error-loading-list');
                    return;
                }
                this.productUnits = response.product_units;
                this.unitsMap = this.mapUnitsByAbbreviation(this.productUnits);

                this.loadingReadyCount++;
                this._changeDetectorRef.markForCheck();
            },
            (error) => {
                this._flashMessageService.error('error-loading-list');
            }
        );

        this._productUpdateService.orderTotalUpdated.subscribe(
            (orderTotal: OrderTotalItem) => {
                this.orderTotal = orderTotal;
                this._changeDetectorRef.markForCheck();
            }
        );
    }

    ngOnChanges(changes: SimpleChanges): void {
        if (changes.products && changes.products.currentValue) {
            // Create a deep copy of the products array
            this.localProducts = JSON.parse(JSON.stringify(this.products));
        }
    }

    ngOnDestroy(): void {
        this._productUpdateService.emitProductUpdate(this.localProducts);
    }

    /* --------------------------------------------------------------------------------------
     *        Public methods
     * --------------------------------------------------------------------------------------
     */

    searchProducts(searchValue: string): void {
        // operator has access to all products. even the ones with no stock, but the client only
        // has access to the ones with stock
        this._productsService
            .getCatalogProductsBySearchNoAuth(searchValue, this.isClient)
            .subscribe((response) => {
                this.catalogProducts = response.product_catalogs;
                this.filteredCatalogProducts = response.product_catalogs;
                this._changeDetectorRef.detectChanges();
            });
    }

    mapUnitsByAbbreviation(units: ProductUnit[]): Map<string, ProductUnit> {
        const tempUnitsMap: Map<string, ProductUnit> = new Map();

        units.forEach((unit) => {
            tempUnitsMap.set(unit.abbreviation, unit);
        });

        return tempUnitsMap;
    }

    /* ======================================
     * SEARCH AND FILTERING PRODUCTS
     * ======================================
     */

    onSearchChange(searchValue: string): void {
        this.searchTerm.next(searchValue);
    }

    /* ======================================
     * ADD AND UPDATE EVENTS
     * ======================================
     */

    toggleAddProduct(): void {
        this.showAddProduct = !this.showAddProduct;
    }

    async addProductByCatalog(
        selectedCatalog: ProductCatalogDto
    ): Promise<void> {
        const newProduct = this.createNewProduct(selectedCatalog);
        this.localProducts.push(newProduct);

        // update the product for instant price and meter rate calculation
        const success = await this.updateProducts();
        if (!success) {
            this._flashMessageService.error('products-update-error');

            // remove the product from local products
            this.localProducts = this.localProducts.filter(
                (product) => product.id !== newProduct.id
            );
        }

        // Clear the input and reset the filtered list
        this.selectedCatalogName = '';
        this.showAddProduct = false;
        this._changeDetectorRef.markForCheck();

        // clear the filtering search
        this.filteredCatalogProducts = this.catalogProducts;
    }

    createNewProduct(product: ProductCatalogDto): OrderProductsDTO {
        const defaultProductSize = this.getProductDefaultUnit(product);
        const maxProductId = Math.max(
            ...this.localProducts.map((productMax) => productMax.id),
            0
        );
        this.lastInsertedId = maxProductId + 1;

        const newProduct = {
            // id must be the max id + 1
            id: this.lastInsertedId,
            order_token: this.orderToken,
            product_catalog: product,
            // ensure to create a new defaultProductSize reference in memory,
            // do not share the same memory location of product size and change
            // all at the same time. By using the spread operate, we mitigate that
            product_unit: { ...defaultProductSize },
            quantity: 1,
            calculated_price: null,
            price_discount: null,
            confidence: 100,
            is_instant_match: false,
            is_manual_insert: true,
            price_locked_at: null,
            is_price_locked: false,
        };

        return newProduct;
    }

    getProductDefaultUnit(product: ProductCatalogDto): ProductUnit {
        const fallbackProductUnit: ProductUnit = this.unitsMap.get(
            product.unit
        );
        // The intention is that the default product unit should be UN (Unit), but only if the conversion has
        // the UN unit. If not, we should get the first unit available for that product
        const conversions = product.product_conversions;
        if (!conversions) {
            return fallbackProductUnit;
        }

        const unConversion = conversions.find(
            (conversion) => conversion.end_unit.abbreviation === 'UN'
        );

        return unConversion?.end_unit ?? fallbackProductUnit;
    }

    removeProduct(productId: number): void {
        this.localProducts = this.localProducts.filter(
            (product) => product.id !== productId
        );

        // update the list on the backend
        this.onUpdatedProducts();
    }

    onUpdatedProducts(): void {
        // update the products
        const success = this.updateProducts();
        if (!success) {
            this._flashMessageService.error('products-update-error');
            return;
        }
    }

    /* ======================================
     * GETTERS AND FORMATTERS
     * ======================================
     */

    formatStock(product: ProductCatalogDto): string {
        const baseStock = `${product.stock_current} ${product.unit}`;

        return baseStock;
    }

    get isProductTableValid(): boolean {
        return this.localProducts.every((product) => product.quantity > 0);
    }

    /* ======================================
     * UPDATE PRODUCTS
     * ======================================
     */

    async updateProducts(): Promise<boolean> {
        // Call updatePromise and wait for it to resolve
        try {
            const success = await this.updatePromise();
            return success;
        } catch (error) {
            return false;
        }
    }

    async updatePromise(): Promise<boolean> {
        if (this.localProducts.length === 0) {
            this._flashMessageService.error('products-empty-list');
            return Promise.resolve(false);
        }

        this.isUpdatingProducts = true;

        const updatedProducts = this.convertToOrderProducts();

        try {
            const response = await this.updateOrderProducts(updatedProducts);
            if (
                response.order_total !== undefined &&
                response.order_total !== null
            ) {
                this.orderTotal = response.order_total;
                this._productUpdateService.orderTotalUpdated.emit(
                    response.order_total
                );
            }
            return this.handleUpdateSuccess(response);
        } catch (e) {
            return this.handleUpdateError();
        }
    }

    private convertToOrderProducts(): OrderProduct[] {
        return this.localProducts.map((product) => {
            const productUnit = this.productUnits.find(
                (unit) =>
                    unit.abbreviation === product.product_unit.abbreviation
            );

            const orderProduct: OrderProduct = {
                id: product.id,
                order_token: product.order_token,
                product_catalog_id: product.product_catalog.id,
                product_unit_id: productUnit?.id ?? 0, // Handle undefined unit
                quantity: product.quantity,
                confidence: 100,
                is_instant_match: false,
                is_manual_insert: true,
            };

            return orderProduct;
        });
    }

    private updateOrderProducts(
        updatedProducts: OrderProduct[]
    ): Promise<ProductUpdateResponse> {
        const noAuth = this.isClient;
        return new Promise((resolve, reject) => {
            this._productsService
                .updateOrderProducts(this.orderToken, updatedProducts, noAuth)
                .subscribe(
                    (response) => resolve(response),
                    () => reject()
                );
        });
    }

    private handleUpdateSuccess(response: ProductUpdateResponse): boolean {
        if (!response || response.result_code < 0) {
            this._flashMessageService.error(response.result);
            this.isUpdatingProducts = false;
            return false;
        }

        this.updateLocalProducts(response);
        this._productUpdateService.updateRatings(response.order_ratings);

        this.isUpdatingProducts = false;
        this._changeDetectorRef.markForCheck();
        return true;
    }

    private updateLocalProducts(response: ProductUpdateResponse): void {
        const localProductsMap = new Map<number, OrderProductsDTO>();
        this.localProducts.forEach((product) => {
            localProductsMap.set(product.id, product);
        });

        response.products.forEach((product) => {
            const localProduct = localProductsMap.get(product.id);

            if (!localProduct) {
                // this means we got one product from the backend that was a creation, and now
                // we need to update our product with that id
                const newProduct = this.localProducts.find(
                    (prod) => prod.id === this.lastInsertedId
                );
                newProduct.id = product.id;
                newProduct.calculated_price = product.calculated_price;
                newProduct.price_discount = product.price_discount;

                localProductsMap.set(product.id, newProduct);
                return;
            }

            localProduct.calculated_price = product.calculated_price;
            localProduct.price_discount = product.price_discount;
        });

        this.products = JSON.parse(JSON.stringify(this.localProducts)); // Create a deep copy
    }

    private handleUpdateError(): boolean {
        this._flashMessageService.error('products-update-error');
        this.isUpdatingProducts = false;
        this._changeDetectorRef.markForCheck();
        return false;
    }
}
