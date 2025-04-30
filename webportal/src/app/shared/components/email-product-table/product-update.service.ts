/* eslint-disable @typescript-eslint/member-ordering */
import { EventEmitter, Injectable } from '@angular/core';
import { BehaviorSubject } from 'rxjs';
import {
    OrderProductsDTO,
    OrderRatingDTO,
} from '../filtering-validate/details/details.types';
import { OrderTotalItem } from './products.types';

@Injectable({
    providedIn: 'root',
})
export class ProductUpdateService {
    productsUpdated = new EventEmitter();
    orderTotalUpdated = new EventEmitter();

    private orderTotalSubject = new BehaviorSubject<OrderTotalItem>(null);
    orderTotal$ = this.orderTotalSubject.asObservable();

    private ratingsSubject = new BehaviorSubject<OrderRatingDTO[]>([]);
    ratings$ = this.ratingsSubject.asObservable();

    constructor() {}

    emitProductUpdate(products: OrderProductsDTO[]): void {
        this.productsUpdated.emit(products);
    }

    updateRatings(ratings: OrderRatingDTO[]): void {
        this.ratingsSubject.next(ratings);
    }

    updateOrderTotal(orderTotal: OrderTotalItem): void {
        this.orderTotalUpdated.emit(orderTotal);
        this.orderTotalSubject.next(orderTotal);
    }
}
