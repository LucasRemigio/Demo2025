/* eslint-disable arrow-parens */
import {
    ChangeDetectorRef,
    Component,
    EventEmitter,
    Input,
    OnChanges,
    OnInit,
    Output,
    OnDestroy,
} from '@angular/core';
import { translate } from '@ngneat/transloco';
import { Subject } from 'rxjs';
import { debounceTime, distinctUntilChanged, takeUntil } from 'rxjs/operators';
import {
    OrderProductsDTO,
    ProductCatalogDTO,
} from '../../filtering-validate/details/details.types';
import { FlashMessageService } from '../../flash-message/flash-message.service';
import { ProductsService } from '../products.service';
import {
    ProductCatalogDto,
    ProductConversionDTO,
    ProductUnit,
} from '../products.types';

@Component({
    selector: 'app-quote-product-table',
    templateUrl: './quote-product-table.component.html',
    styleUrls: ['./quote-product-table.component.scss'],
})
export class QuoteProductTableComponent
    implements OnInit, OnChanges, OnDestroy
{
    @Input() products: OrderProductsDTO[] = [];
    @Input() productUnits: ProductUnit[] = [];
    @Input() isDisabled: boolean = false;
    @Input() isToRemoveHeader: boolean = false;
    @Input() isClient: boolean = false;

    @Output() removeProduct = new EventEmitter<number>();
    @Output() updateProduct = new EventEmitter<OrderProductsDTO>();

    previousFlashMessages: Map<number, number> = new Map();
    maxMessagesPerNumber: number = 3; // Maximum number of messages to show for each product

    // this is to debounce the quantity change event, so that we do not emit too many events in a short time.
    private quantityChangeSubject = new Subject<OrderProductsDTO>();
    private _unsubscribeAll: Subject<any> = new Subject<any>();

    constructor(
        private _changeDetectorRef: ChangeDetectorRef,
        private _productsService: ProductsService,
        private _flashMessage: FlashMessageService
    ) {}

    ngOnInit(): void {
        // Set up the debounced quantity change handler
        this.quantityChangeSubject
            .pipe(
                debounceTime(500), // Wait 500ms after the last event before emitting
                distinctUntilChanged(
                    (prev, curr) =>
                        prev.id === curr.id && prev.quantity === curr.quantity
                ),
                takeUntil(this._unsubscribeAll)
            )
            .subscribe((product) => {
                this.updateProduct.emit(product);
            });
    }

    ngOnChanges(): void {}

    ngOnDestroy(): void {
        // Unsubscribe from all subscriptions
        this._unsubscribeAll.next();
        this._unsubscribeAll.complete();
    }

    formatPrice(price: number): string {
        return price != null ? `${price.toFixed(2)}€` : '';
    }

    formatMeters(meters: number): string {
        if (!meters) {
            return '';
        }

        const formatedMeters = parseFloat(meters.toFixed(2)).toString();

        return `${formatedMeters}m`;
    }

    getProductQuantityInOtherUnit(product: OrderProductsDTO): string {
        // this should display the quantity in the other unit, as per the conversions list
        const currentUnit = product.product_unit.abbreviation;
        const quantity = product.quantity || 0;

        const baseStock = `${quantity} ${currentUnit}`;

        if (!quantity) {
            return baseStock;
        }

        const productConversions = product.product_catalog.product_conversions;

        if (!productConversions) {
            return baseStock;
        }

        const otherUnitConversion = productConversions.find(
            (conversion) => conversion.origin_unit.abbreviation === currentUnit
        );

        if (!otherUnitConversion) {
            return baseStock;
        }

        const convertedQuantity = quantity * otherUnitConversion.rate;
        return `${convertedQuantity.toFixed(2)} ${
            otherUnitConversion.end_unit.abbreviation
        }`;
    }

    getQuantityInBaseUnit(product: OrderProductsDTO): number {
        const productBaseUnit = product.product_catalog.unit;
        const quantity = product.quantity || 0;
        const conversion = this.getUnitConversionRate(product, productBaseUnit);
        return quantity * conversion.rate;
    }

    getProductTotalPrice(product: OrderProductsDTO): string {
        // for this, we must get the quantity of the product in the base unit.
        // to that, we round up the price_discount to 3 digits and multiply by the quantity
        let quantity = product.quantity || 0;

        if (
            product.product_unit.abbreviation !== product.product_catalog.unit
        ) {
            quantity = this.getQuantityInBaseUnit(product);
        }

        const priceDiscount = product.price_discount;
        const totalPrice = priceDiscount * quantity;

        return this.formatPrice(totalPrice);
    }

    formatStock(product: OrderProductsDTO): string {
        const stock = this._productsService.formatStock(
            product.product_catalog
        );

        return stock;
    }

    getProductConversions(product: ProductCatalogDto): ProductConversionDTO[] {
        const conversions = product.product_conversions || [];
        return conversions;
    }

    onUnitChange(product: OrderProductsDTO): void {
        // emit event for upper component to update the products price and meter rate
        product.quantity = 1;
        // remove from flash messages
        this.previousFlashMessages.delete(product.id);

        this.updateProduct.emit(product);
    }

    onQuantityChange(product: OrderProductsDTO): void {
        const currentQuantity = Number(product.quantity);
        if (currentQuantity < 0 || isNaN(currentQuantity)) {
            setTimeout(() => {
                product.quantity = 1;
                this._changeDetectorRef.markForCheck();
            });
            return;
        }

        // quotations do not have a limit to the quantity given, as it is just a quote
        // The client will never have access to the stock as well, so even if it is an order, as a client
        // we should not limit the quantity to the stock.
        if (this.isClient) {
            return;
        }

        // now i want to check if the user is putting more stock than what we have.
        // if so, we should show a warning message to the user.
        // for that, we need to convert the quantity to the original product unit
        // and compare it with the stock_current

        // Check if quantity exceeds available stock
        const isExceedingStock = this.isExceedingAvailableStock(product);

        // Show warning only if stock is exceeded and hasn't been shown yet
        if (isExceedingStock) {
            this.flashMessageInvalidStock(product);
        }

        // Always emit the update event
        this.quantityChangeSubject.next({ ...product });
    }

    /**
     * Check if the product quantity exceeds available stock
     */
    isExceedingAvailableStock(product: OrderProductsDTO): boolean {
        const catalogUnit = product.product_catalog.unit.toUpperCase().trim();
        const productUnit = product.product_unit.abbreviation
            .toUpperCase()
            .trim();
        const maxStock = product.product_catalog.stock_current;

        // Direct comparison if units match
        if (catalogUnit === productUnit) {
            return product.quantity > maxStock;
        }

        // Need conversion for different units
        const conversion = this.getUnitConversionRate(product, catalogUnit);
        if (!conversion) {
            return false; // Can't determine if exceeding stock without conversion
        }

        // Compare with converted quantity
        const convertedQuantity = product.quantity * conversion.rate;
        return convertedQuantity > maxStock;
    }

    getUnitConversionRate(
        product: OrderProductsDTO,
        catalogUnit: string
    ): ProductConversionDTO | null {
        const conversions = this.getProductConversions(product.product_catalog);

        if (!conversions.length) {
            return null;
        }

        const conversion = conversions.find(
            (conv) =>
                conv.origin_unit.abbreviation ===
                    product.product_unit.abbreviation &&
                conv.end_unit.abbreviation === catalogUnit
        );

        if (!conversion) {
            return null;
        }

        return conversion;
    }

    flashMessageInvalidStock(product: OrderProductsDTO): void {
        const currentCount = this.previousFlashMessages.get(product.id) || 0;
        if (currentCount >= this.maxMessagesPerNumber) {
            return;
        }

        let message = translate('stock-exceeded');
        message += ': ' + product.product_catalog.description;

        this._flashMessage.ntwarning(message);
        this.previousFlashMessages.set(product.id, currentCount + 1);
    }

    isPriceEmpty(product: OrderProductsDTO): boolean {
        if (
            product.price_discount === null ||
            product.price_discount === undefined
        ) {
            return;
        }

        return product.price_discount <= 0;
    }

    // Get the available units for the given product
    getAvailableUnits(product: OrderProductsDTO): ProductUnit[] {
        const conversions = this.getProductConversions(product.product_catalog);
        const availableUnits: ProductUnit[] = [];

        // Map through the conversions to get available units
        conversions.forEach((conversion) => {
            availableUnits.push(conversion.origin_unit);
        });

        const defaultUnit = this.productUnits.find(
            (unit) => unit.id === product.product_unit.id
        );

        // add default unit if conversion list does not have it
        if (this.productUnits) {
            if (!availableUnits.find((unit) => unit.id === defaultUnit.id)) {
                availableUnits.push(defaultUnit);
            }
        }

        this._changeDetectorRef.markForCheck();

        return availableUnits;
    }

    /* -------------------------------
     * GETTERS
     * ------------------------------- */

    get highConfidenceProducts(): OrderProductsDTO[] {
        return this.products.filter((product) => product.confidence >= 100);
    }

    get lowConfidenceProducts(): OrderProductsDTO[] {
        return this.products.filter((product) => product.confidence < 100);
    }
}
