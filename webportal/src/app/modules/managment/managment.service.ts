import { Injectable } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { BehaviorSubject, Observable, of, throwError } from 'rxjs';
import { catchError, map, switchMap, tap } from 'rxjs/operators';
import { cloneDeep } from 'lodash-es';

import { environment } from 'environments/environment';
import { FuseSplashScreenService } from '@fuse/services/splash-screen';
import { Client_Address, Exclusions, Holidays } from './managment.types';

@Injectable({
    providedIn: 'root',
})
export class ManagmentService {
    // Private

    private _clients: BehaviorSubject<Client_Address[] | null> =
        new BehaviorSubject(null);

    private _client_addresses: BehaviorSubject<Client_Address[] | null> =
        new BehaviorSubject(null);

    private _exclusions: BehaviorSubject<Exclusions[] | null> =
        new BehaviorSubject(null);

    private _holidays: BehaviorSubject<Holidays[] | null> = new BehaviorSubject(
        null
    );

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
    get clients$(): Observable<Client_Address[]> {
        return this._clients.asObservable();
    }

    get client_addresses$(): Observable<Client_Address[]> {
        return this._client_addresses.asObservable();
    }

    get exclusions$(): Observable<Exclusions[]> {
        return this._exclusions.asObservable();
    }

    get holidays$(): Observable<Holidays[]> {
        return this._holidays.asObservable();
    }

    // -----------------------------------------------------------------------------------------------------
    // @ Public methods
    // -----------------------------------------------------------------------------------------------------

    /**
     * Get Sources
     */
    getAlClientsAddresses(): Observable<Client_Address[]> {
        this._fuseSplashScreenService.show();

        return this._httpClient
            .get<Client_Address[]>('api/client/getAlClientsAddresses')
            .pipe(
                tap((response: any) => {
                    this._clients.next(response.clients);
                    this._fuseSplashScreenService.hide();
                })
            );
    }

    getAllAddressesByToken(token: string): Observable<Client_Address[]> {
        this._fuseSplashScreenService.show();

        return this._httpClient
            .get<Client_Address[]>('api/client/getAllAddressesByToken', {
                params: { token: String(token) },
            })
            .pipe(
                tap((response: any) => {
                    this._client_addresses.next(response.addresses);
                    this._fuseSplashScreenService.hide();
                })
            );
    }

    edit(client_addresses): Observable<any> {
        return this._httpClient
            .post('api/client/editClientAddressess', client_addresses)
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

    add(json_content: string): Observable<any> {
        return this._httpClient
            .post('api/client/addClientAddressess', {
                json_content: json_content,
            })
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

    remove(address: Client_Address): Observable<any> {
        return this._httpClient
            .post('api/client/removeClientAddress', address)
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

    resendConfirmationEmail(client_addresses): Observable<any> {
        return this._httpClient
            .post('api/client/ResendConfirmationEmail', client_addresses)
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

    sendAllClientsNotification(): Observable<any> {
        return this._httpClient
            .post('api/client/SendAllClientsNotification', {})
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

    getAllExcluisons(): Observable<Exclusions[]> {
        this._fuseSplashScreenService.show();

        return this._httpClient
            .get<Exclusions[]>('api/client/getAllExclusionsClients')
            .pipe(
                tap((response: any) => {
                    this._exclusions.next(response.exclusion_clients);
                    this._fuseSplashScreenService.hide();
                })
            );
    }

    editExclusion(exclusion: Exclusions): Observable<any> {
        return this._httpClient
            .post('api/client/editExclusionClient', exclusion)
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

    addExclusion(exclusion: Exclusions): Observable<any> {
        return this._httpClient
            .post('api/client/addExclusionClient', exclusion)
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

    removeExclusion(exclusion: Exclusions): Observable<any> {
        return this._httpClient
            .post('api/client/removeExclusionClient', exclusion)
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

    getAllHolidays(): Observable<Holidays[]> {
        this._fuseSplashScreenService.show();

        return this._httpClient
            .get<Holidays[]>('api/holidays/getAllHolidays')
            .pipe(
                tap((response: any) => {
                    this._holidays.next(response.holidays);
                    this._fuseSplashScreenService.hide();
                })
            );
    }

    addHolidays(holiday: Holidays): Observable<any> {
        return this._httpClient.post('api/holidays/AddHoliday', holiday).pipe(
            catchError(() => {
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

    removeHoliday(holiday: Holidays): Observable<any> {
        return this._httpClient
            .post('api/holidays/RemoveHoliday', holiday)
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
