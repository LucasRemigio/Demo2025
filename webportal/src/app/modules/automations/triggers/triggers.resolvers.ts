import { Injectable } from '@angular/core';
import {
    ActivatedRouteSnapshot,
    Resolve,
    Router,
    RouterStateSnapshot,
} from '@angular/router';
import { Observable, throwError } from 'rxjs';
import { catchError } from 'rxjs/operators';
import moment from 'moment';
import { TriggersService } from './triggers.service';
import { Triggers } from './triggers.types';

@Injectable({
    providedIn: 'root',
})
export class TriggersResolver implements Resolve<any> {
    /**
     * Constructor
     */
    constructor(private _router: Router, private _triggersService: TriggersService) {}

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
    ): Observable<Triggers[]> {

        return this._triggersService.getTriggers().pipe(
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