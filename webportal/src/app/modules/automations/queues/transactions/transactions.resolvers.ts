import { Injectable } from '@angular/core';
import {
    ActivatedRouteSnapshot,
    Resolve,
    Router,
    RouterStateSnapshot,
} from '@angular/router';
import { Observable, throwError } from 'rxjs';
import { catchError } from 'rxjs/operators';
import {
    Transactions
} from 'app/modules/automations/queues/transactions/transactions.types';
import { TransactionsService } from 'app/modules/automations/queues/transactions/transactions.service';
import moment from 'moment';

@Injectable({
    providedIn: 'root',
})
export class TransactionsResolver implements Resolve<any> {
    /**
     * Constructor
     */
    constructor(private _router: Router, private _transactionsService: TransactionsService) {}

    // -----------------------------------------------------------------------------------------------------
    // @ Public methods
    // -----------------------------------------------------------------------------------------------------

    /**
     * Resolver
     *
     * @param route
     * @param state
     */
    resolve(
        route: ActivatedRouteSnapshot,
        state: RouterStateSnapshot
    ): Observable<Transactions[]> {
        return this._transactionsService.getTransactionsByQueueId(route.paramMap.get('id')).pipe(
            // Error here means the requested task is not available
            catchError((error) => {
                // Get the parent url
                const parentUrl = state.url.split('/').slice(0, -1).join('/');

                // Navigate to there
                this._router.navigateByUrl(parentUrl);

                // Throw an error
                return throwError(error);
            })
        );
    }
}