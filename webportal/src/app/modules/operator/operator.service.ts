import { Injectable } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { BehaviorSubject, Observable, of, throwError } from 'rxjs';
import { catchError, map, switchMap, tap } from 'rxjs/operators';
import { cloneDeep } from 'lodash-es';

import { environment } from 'environments/environment';
import { FuseSplashScreenService } from '@fuse/services/splash-screen';
import { Operators } from './operator.types';

@Injectable({
    providedIn: 'root',
})
export class OperatorService {
    // Private

    private _operators: BehaviorSubject<Operators[] | null> =
        new BehaviorSubject(null);

    /**
     * Constructor
     */
    constructor(
        private _httpClient: HttpClient,
        private _fuseSplashScreenService: FuseSplashScreenService
    ) {}

    // -----------------------------------------------------------------------------------------------------
    // @ Accessors
    // -----------------------------------------------------------------------------------------------------

    /**
     * Getter for Sources
     */
    get operators$(): Observable<Operators[]> {
        return this._operators.asObservable();
    }

    // -----------------------------------------------------------------------------------------------------
    // @ Public methods
    // -----------------------------------------------------------------------------------------------------

    /**
     * Get Sources
     */
    getOperators(operator_area: string): Observable<Operators[]> {
        this._fuseSplashScreenService.show();

        return this._httpClient
            .get<Operators[]>('api/operator/getAllOperators', {
                params: { operator_area: operator_area },
            })
            .pipe(
                tap((response: any) => {
                    this._operators.next(response.operators);
                    this._fuseSplashScreenService.hide();
                })
            );
    }

    add(operator: Operators): Observable<any> {
        return this._httpClient.post('api/operator/addOperator', operator).pipe(
            catchError(() => {
                this._fuseSplashScreenService.hide();
                return of(false);
            }),
            switchMap((response: any) => {
                if (response.result_code && Number(response.result_code) > 0) {
                    return of(true);
                } else {
                    return of(false);
                }
            })
        );
    }

    edit(operator: Operators): Observable<any> {
        operator.id = String(operator.id);
        return this._httpClient
            .post('api/operator/editOperator', operator)
            .pipe(
                catchError(() => {
                    return of(false);
                }),
                switchMap((response: any) => {
                    if (
                        response.result_code &&
                        Number(response.result_code) > 0
                    ) {
                        return of(true);
                    } else {
                        return of(false);
                    }
                })
            );
    }

    remove(operator: Operators): Observable<any> {
        operator.id = String(operator.id);
        return this._httpClient
            .post('api/operator/removeOperator', operator)
            .pipe(
                catchError(() => {
                    return of(false);
                }),
                switchMap((response: any) => {
                    if (
                        response.result_code &&
                        Number(response.result_code) > 0
                    ) {
                        return of(true);
                    } else {
                        return of(false);
                    }
                })
            );
    }
}
