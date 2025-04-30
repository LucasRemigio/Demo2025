import { Injectable } from '@angular/core';
import { HttpClient, HttpHeaders, HttpParams } from '@angular/common/http';
import { catchError, finalize, switchMap } from 'rxjs/operators';
import { Observable, of, throwError } from 'rxjs';
import { environment } from 'environments/environment';
import { Script } from './scripts.types';
import { FuseSplashScreenService } from '@fuse/services/splash-screen';

@Injectable({
    providedIn: 'root',
})
export class ScriptsService {
    constructor(
        private _httpClient: HttpClient,
        private _fuseSplashScreenService: FuseSplashScreenService
    ){

    }

    getScripts(id?: string): Observable<any> {
        let params;
        if (id) {
            params = new HttpParams().set('id', id);
        }
        return this._httpClient
            .get(environment.currrentBaseURL + '/api/scripts/getScripts', {
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

    deleteAccount(id: string): Observable<any> {
        let params = new HttpParams();

        if (id) {
            params = params.set('id', id);
        }

        return this._httpClient
            .post(
                environment.currrentBaseURL + '/api/scripts/removeScript',
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

    editScript(
        name: string,
        file_content: string,
        description: string,
        main_file: string,
        cron_job: string,
        id: string
    ): Observable<any> {
        this._fuseSplashScreenService.show();
        return this._httpClient
            .post(environment.currrentBaseURL + '/api/scripts/editScript', {
                description: description,
                name: name,
                file_content: file_content,
                main_file: main_file,
                cron_job: cron_job,
                id: String(id),
            })
            .pipe(
                switchMap((response: any) => {
                    return of(response);
                }),
                finalize(() => {
                    this._fuseSplashScreenService.hide();
                })
            );
    }

    addScript(
        name: string,
        main_file: string,
        description: string,
        file_content: string
    ): Observable<any> {
        this._fuseSplashScreenService.show();
        return this._httpClient
            .post(environment.currrentBaseURL + '/api/scripts/addScript', {
                name: name,
                main_file: main_file,
                description: description,
                file_content: file_content,
            })
            .pipe(
                switchMap((response: any) => {
                    return of(response);
                }),
                finalize(() => {
                    this._fuseSplashScreenService.hide();
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
