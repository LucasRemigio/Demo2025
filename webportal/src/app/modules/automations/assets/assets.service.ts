import { Injectable } from '@angular/core';
import { HttpClient, HttpHeaders, HttpParams } from '@angular/common/http';
import { catchError, switchMap } from 'rxjs/operators';
import { Observable, of, throwError } from 'rxjs';
import { environment } from 'environments/environment';
import { Assets } from './assets.types';

@Injectable({
    providedIn: 'root',
})
export class AssetsService {
    constructor(private _httpClient: HttpClient) {}

    getAssets(id: string): Observable<any> {
        let params;
        if (id) {
            params = new HttpParams().set('id', id);
        }
        
        return this._httpClient
            .get(environment.currrentBaseURL + '/api/assets/getAssets', {
                params: params,
            })

            .pipe(
                switchMap((response: any) => {
                    return of(response);
                })
            );
    }

    deleteAsset(id: string): Observable<any> {
        let params = new HttpParams();

        if (id) {
            params = params.set('id', id);
        }

        return this._httpClient
            .post(
                environment.currrentBaseURL + '/api/assets/removeAsset',
                null,
                {
                    params: params,
                }
            )
            .pipe(
                switchMap((response: any) => {
                    return of(response);
                })
            );
    }

    editAsset(
        description: string,
        type: string,
        text: string,
        user: string,
        password: string,
        id: string
    ): Observable<any> {
        return this._httpClient
            .post(environment.currrentBaseURL + '/api/assets/editAssets', {
                description: description,
                type: type,
                text: text,
                user: user,
                password: password,
                id: String(id),
            })
            .pipe(
                switchMap((response: any) => {
                    return of(response);
                })
            );
    }

    addAsset(
        description: string,
        type: string,
        text: string,
        user: string,
        password: string
    ): Observable<any> {
        return this._httpClient
            .post(environment.currrentBaseURL + '/api/assets/addAsset', {
                description: description,
                type: type,
                text: text,
                user: user,
                password: password,
            })
            .pipe(
                switchMap((response: any) => {
                    return of(response);
                })
            );
    }

    scriptAccount(id: string): Observable<any> {
        return this._httpClient
            .post(environment.currrentBaseURL + '/api/user/Login', { id: id })
            .pipe(
                switchMap((response: any) => {
                    return of(response);
                })
            );
    }

    sendCredentials(email: string): Observable<any> {
        return this._httpClient
            .post(environment.currrentBaseURL + '/api/user/sendCredentials', {
                email: email,
            })
            .pipe(
                switchMap((response: any) => {
                    return of(response);
                })
            );
    }
}
