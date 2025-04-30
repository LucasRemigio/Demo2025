import { Component, Input, OnInit } from '@angular/core';
import { translate } from '@ngneat/transloco';
import { UserService } from 'app/core/user/user.service';
import { EMAIL_STATUSES } from 'app/modules/filtering/filtering.types';
import { StatusItem } from 'app/modules/orders/order.types';
import { OrderDTO } from '../../filtering-validate/details/details.types';

@Component({
    selector: 'app-order-validate-cards',
    templateUrl: './order-validate-cards.component.html',
    styleUrls: ['./order-validate-cards.component.scss'],
})
export class OrderValidateCardsComponent implements OnInit {
    @Input() orders: OrderDTO[] = [];
    @Input() isPendingApproval!: (status: StatusItem) => boolean;
    @Input() getReasonClass!: (status: StatusItem) => string;
    @Input() getStatusDescription!: (status: StatusItem) => string;

    isUserSupervisor: boolean = false;

    constructor(private _userService: UserService) {}

    ngOnInit(): void {
        this.isUserSupervisor = this._userService.isSupervisor();
    }
}
