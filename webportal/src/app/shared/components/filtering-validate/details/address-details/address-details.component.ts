import {
    AfterViewInit,
    ChangeDetectorRef,
    Component,
    EventEmitter,
    Input,
    OnDestroy,
    OnInit,
    Output,
    ViewChild,
} from '@angular/core';
import { AddressUpdateService } from 'app/shared/components/confirm-order-address/address-update.service';
import { ConfirmOrderAddressComponent } from 'app/shared/components/confirm-order-address/confirm-order-address.component';
import { CurrentAddress } from 'app/shared/components/confirm-order-address/confirm-order-address.types';
import { OrderService } from 'app/shared/components/confirm-order-address/order.service';
import { FlashMessageService } from 'app/shared/components/flash-message/flash-message.service';

@Component({
    selector: 'app-address-details',
    templateUrl: './address-details.component.html',
    styleUrls: ['./address-details.component.scss'],
})
export class AddressDetailsComponent
    implements OnInit, OnDestroy, AfterViewInit
{
    // The address can be edited in the child component, and we want that one,
    // or the default order one, handled by the parent component
    @Input() address: CurrentAddress;
    @Input() orderToken: string;
    @Input() isDisabled: boolean = false;
    @Input() isClient: boolean = false;
    @Input() clientCar: string;

    @Output() updatedAddress = new EventEmitter<CurrentAddress>();

    @ViewChild(ConfirmOrderAddressComponent)
    confirmOrderAddress!: ConfirmOrderAddressComponent;

    isOrderToSendToClient: boolean = false;
    isLoading: boolean = false;

    constructor(
        private _orderService: OrderService,
        private _flashMessageService: FlashMessageService,
        private _changeDetectorRef: ChangeDetectorRef,
        private _addressUpdateService: AddressUpdateService
    ) {}

    ngOnInit(): void {
        if (this.address.is_delivery && this.address.is_delivery === true) {
            this.isOrderToSendToClient = true;
        }
    }

    ngOnDestroy(): void {
        // the children component already send the event to save the address
        // so we do not need to send this event if it is not a delivery
        if (!this.isOrderToSendToClient) {
            this._addressUpdateService.emitAddressUpdate(this.address);
        }
    }

    ngAfterViewInit(): void {
        if (!this.confirmOrderAddress) {
            return;
        }

        this.confirmOrderAddress.loadingStateChanged.subscribe(
            (loading: boolean) => {
                this.isLoading = loading;
            }
        );
    }

    emitUpdatedAddress(event: CurrentAddress): void {
        this.updatedAddress.emit(event);
    }

    updateAddress(): void {
        if (this.isOrderToSendToClient) {
            // need to watch an inner loading because the func is async
            this.confirmOrderAddress.updateAddress(this.isOrderToSendToClient);
            return;
        }

        this.address.is_delivery = this.isOrderToSendToClient;
        this.isLoading = true;
        this._changeDetectorRef.detectChanges();

        this._orderService
            .updateOrderAddress(this.address, this.orderToken)
            .subscribe(
                (response) => {
                    if (!response || response.result_code < 0) {
                        this._flashMessageService.error(
                            'order-address-invalid-format'
                        );
                        this.isLoading = false;
                        return;
                    }

                    this._flashMessageService.success('order-address-updated');
                    this.updatedAddress.emit(this.address);

                    this._addressUpdateService.emitLogisticRatingUpdate(
                        response.logistic_rating
                    );
                    return;
                },
                (error) => {
                    this._flashMessageService.error(
                        'order-address-invalid-format'
                    );
                },
                () => {
                    this.isLoading = false;
                    this._changeDetectorRef.detectChanges();
                }
            );
    }
}
