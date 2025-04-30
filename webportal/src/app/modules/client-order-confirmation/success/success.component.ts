import { Component, OnInit } from '@angular/core';
import { ActivatedRoute, Router } from '@angular/router';
import { FuseSplashScreenService } from '@fuse/services/splash-screen';
import { OrderService } from 'app/shared/components/confirm-order-address/order.service';

@Component({
    selector: 'app-success',
    templateUrl: './success.component.html',
    styleUrls: ['./success.component.scss'],
})
export class SuccessComponent implements OnInit {
    token: string = '';

    constructor(
        private _orderService: OrderService,
        private _route: ActivatedRoute,
        private _router: Router,
        private _fuseSplashScreenService: FuseSplashScreenService
    ) {}

    ngOnInit(): void {
        this._fuseSplashScreenService.show();
        // get the token from the url
        this.token = this._route.snapshot.paramMap.get('token') || '';

        this._orderService
            .checkOrderStatus(this.token)
            .subscribe((response) => {
                this._fuseSplashScreenService.hide();
                if (response.result_code < 0) {
                    this.goBack();
                    // handle error
                    return;
                }

                if (response.is_order_checked) {
                    // then its ok and the user can stay in this page
                    return;
                }

                this.goBack();
            });
    }

    goBack(): void {
        // if not checked, he must go back to confirm it. he cant see success message
        this._router.navigate(['..'], {
            relativeTo: this._route,
        });
    }
}
