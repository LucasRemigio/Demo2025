import { EventEmitter, Injectable } from '@angular/core';
import {
    CurrentAddress,
    CurrentMapsAddress,
    OrderRatingItem,
} from './confirm-order-address.types';
import { Subject } from 'rxjs';

@Injectable({
    providedIn: 'root',
})
export class AddressUpdateService {
    addressUpdated = new Subject<CurrentAddress>();
    logisticRatingUpdated = new Subject<OrderRatingItem>();
    destinationDetailsUpdated = new Subject<CurrentMapsAddress>();

    constructor() {}

    emitAddressUpdate(address: CurrentAddress): void {
        this.addressUpdated.next(address);
    }

    emitLogisticRatingUpdate(rating: OrderRatingItem): void {
        this.logisticRatingUpdated.next(rating);
    }

    emitCurrentDestinationDetailsUpdate(
        destinationDetails: CurrentMapsAddress
    ): void {
        this.destinationDetailsUpdated.next(destinationDetails);
    }
}
