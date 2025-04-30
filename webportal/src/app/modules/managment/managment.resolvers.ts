import { Injectable } from '@angular/core';
import {
    ActivatedRouteSnapshot,
    Resolve,
    Router,
    RouterStateSnapshot,
} from '@angular/router';
import { Observable, throwError } from 'rxjs';
import { catchError } from 'rxjs/operators';
import { Client_Address } from 'app/modules/managment/managment.types';
import { ManagmentService } from 'app/modules/managment/managment.service';
import moment from 'moment';
@Injectable({
    providedIn: 'root',
})
export class ClientsResolver implements Resolve<any> {
    /**
     * Constructor
     */
    constructor(private _managmentService: ManagmentService) {}

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
    ): Observable<Client_Address[]> {
        return this._managmentService.getAlClientsAddresses();
    }
}

@Injectable({
    providedIn: 'root',
})
export class ClientAddressResolver implements Resolve<any> {
    constructor(
        private _router: Router,
        private _managmentService: ManagmentService
    ) {}

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
    ): Observable<any> {
        return this._managmentService
            .getAllAddressesByToken(route.paramMap.get('id'))
            .pipe(
                // Error here means the requested task is not available
                catchError((error) => {
                    // Log the error
                    console.error(error);

                    // Get the parent url
                    const parentUrl = state.url
                        .split('/')
                        .slice(0, -1)
                        .join('/');

                    // Navigate to there
                    this._router.navigateByUrl(parentUrl);

                    // Throw an error
                    return throwError(error);
                })
            );
    }
}

@Injectable({
    providedIn: 'root',
})
export class ClientExclusionsResolver implements Resolve<any> {
    constructor(
        private _router: Router,
        private _managmentService: ManagmentService
    ) {}

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
    ): Observable<any> {
        return this._managmentService.getAllExcluisons();
    }
}

@Injectable({
    providedIn: 'root',
})
export class HolidaysResolver implements Resolve<any> {
    constructor(
        private _router: Router,
        private _managmentService: ManagmentService
    ) {}

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
    ): Observable<any> {
        return this._managmentService.getAllHolidays();
    }
}
