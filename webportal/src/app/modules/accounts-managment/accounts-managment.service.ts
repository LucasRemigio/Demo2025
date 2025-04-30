import { Injectable } from '@angular/core';
import { HttpClient, HttpHeaders, HttpParams } from '@angular/common/http';
import { catchError, switchMap } from 'rxjs/operators';
import { Observable, of, throwError } from 'rxjs';
import { environment } from 'environments/environment';
import { UpdateAccountEntry, AccountEntry } from './accounts-managment.types';

@Injectable({
    providedIn: 'root',
})
export class AccountsManagmentService {
    constructor(private _httpClient: HttpClient) {}

    getAccounts(emailFilter: string): Observable<any> {
        let params;
        if (emailFilter) {
            params = new HttpParams().set('emailFilter', emailFilter);
        }
        return this._httpClient
            .get(environment.currrentBaseURL + '/api/user/getUsers', {
                params: params,
            })
            .pipe(
                switchMap((response: any) => {
                    return of(response);
                })
            );
    }

    addAccount(user: AccountEntry): Observable<any> {
        return this._httpClient
            .post(environment.currrentBaseURL + '/api/user/addUser', user)
            .pipe(
                switchMap((response: any) => {
                    return of(response);
                })
            );
    }

    deleteAccount(email: string): Observable<any> {
        return this._httpClient
            .post(environment.currrentBaseURL + '/api/user/removeUser', {
                email: email,
            })
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

    updateAccount(user: UpdateAccountEntry): Observable<any> {
        return this._httpClient
            .post(environment.currrentBaseURL + '/api/user/updateUser', user)
            .pipe(
                switchMap((response: any) => {
                    return of(response);
                })
            );
    }

    departments(): Observable<any> {
        return this._httpClient
        .get(environment.currrentBaseURL + '/api/user/departments', {})
        .pipe(
            switchMap((response: any) => {
                return of(response);
            })
        );
    }


    getUserDepartments(userEmail : string):  Observable<any> {
        const params = new HttpParams().set('userEmail', userEmail)
        return this._httpClient
            .get(environment.currrentBaseURL + '/api/user/departments', {params})
    }

    getDepartments(): Observable<any> {
        return this._httpClient
            .get<String[]>(environment.currrentBaseURL + '/api/user/departments')
            
    }
}
