import { Injectable } from '@angular/core';
import {
    ActivatedRouteSnapshot,
    Resolve,
    Router,
    RouterStateSnapshot,
} from '@angular/router';
import { Observable, throwError } from 'rxjs';
import { catchError } from 'rxjs/operators';
import { Operators } from 'app/modules/operator/operator.types';
import { OperatorService } from 'app/modules/operator/operator.service';
import moment from 'moment';
@Injectable({
    providedIn: 'root',
})
export class OperatorsResolver implements Resolve<any> {
    /**
     * Constructor
     */
    constructor(private _operatorService: OperatorService) {}

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
    ): Observable<Operators[]> {
        return this._operatorService.getOperators('1');
    }
}
