import { Injectable } from '@angular/core';
import { HttpClient, HttpHeaders, HttpParams } from '@angular/common/http';
import { catchError, switchMap } from 'rxjs/operators';
import { Observable, of, throwError } from 'rxjs';
import { environment } from 'environments/environment';
import { Queues } from './queues.types';

@Injectable({
    providedIn: 'root',
})
export class QueuesService {
    constructor(private _httpClient: HttpClient) {}

    getQueues(id: string): Observable<any> {
        let params;
        if (id) {
            params = new HttpParams().set('id', id);
        }

        return this._httpClient
            .get(environment.currrentBaseURL + '/api/queues/getQueues', {
                params: params,
            })

            .pipe(
                switchMap((response: any) => {
                    return of(response);
                })
            );
    }

    StartScripts(id: string): Observable<any> {
        const params = id ? new HttpParams().set('id', id) : undefined;

        return this._httpClient
            .post(
                environment.currrentBaseURL + '/api/scripts/startScript',
                { id },
                { params }
            )
            .pipe(
                switchMap((response: any) => {
                    return of(response);
                })
            );
    }

    deleteQueue(id: string): Observable<any> {
        let params = new HttpParams();

        if (id) {
            params = params.set('id', id);
        }

        return this._httpClient
            .post(
                environment.currrentBaseURL + '/api/queues/removeQueue',
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

    editQueue(
        name: string,
        description: string,
        autoRetry: boolean,
        numberRetry: number,
        id: string,
    ): Observable<any> {
        return this._httpClient
            .post(environment.currrentBaseURL + '/api/queues/editQueue', {
                name: name,
                description: description,
                autoRetry: autoRetry,
                numberRetry: numberRetry,
                id: String(id),
            })
            .pipe(
                switchMap((response: any) => {
                    return of(response);
                })
            );
    }

    addQueue(
        name: string,
        description: string,
        autoRetry: boolean,
        numberRetry: number,
        script_name: string
    ): Observable<any> {
        return this._httpClient
            .post(environment.currrentBaseURL + '/api/queues/addQueue', {
                name: name,
                description: description,
                autoRetry: autoRetry,
                numberRetry: numberRetry,
                script_name: script_name
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

    /**
     * Track by function for ngFor loops
     *
     * @param index
     * @param item
     */
    trackByFn(index: number, item: any): any {
        return item.id || index;
    }
}