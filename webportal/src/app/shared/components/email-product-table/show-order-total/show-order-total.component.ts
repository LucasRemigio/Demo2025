import {
    ChangeDetectorRef,
    Component,
    Input,
    OnChanges,
    OnInit,
} from '@angular/core';
import { OrderTotalItem } from '../products.types';

@Component({
    selector: 'app-show-order-total',
    templateUrl: './show-order-total.component.html',
    styleUrls: ['./show-order-total.component.scss'],
})
export class ShowOrderTotalComponent implements OnInit, OnChanges {
    @Input() orderTotal: OrderTotalItem;

    constructor(private _cdr: ChangeDetectorRef) {}

    ngOnInit(): void {}

    ngOnChanges(event: any): void {}

    get getOrderTax(): number {
        const tax =
            this.orderTotal.totalDiscountPlusTax -
            this.orderTotal.totalDiscount;

        return tax;
    }
}
