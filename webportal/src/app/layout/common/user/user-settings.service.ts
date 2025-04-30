import { Injectable } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { BehaviorSubject, Observable, of, ReplaySubject } from 'rxjs';
import { environment } from 'environments/environment';

@Injectable({
    providedIn: 'root',
})
export class UserSettingsService {
    /**
     * Constructor
     */
    constructor(private _httpClient: HttpClient) {}

    // -----------------------------------------------------------------------------------------------------
    // @ Public methods
    // -----------------------------------------------------------------------------------------------------

    getSignature(): any {
        return this._httpClient.get(
            environment.currrentBaseURL + '/api/signature'
        );
    }

    patchSignature(signature: string): any {
        return this._httpClient.patch(
            environment.currrentBaseURL + '/api/signature',
            { signature },
            {
                headers: { 'content-type': 'application/json; charset=utf-8' },
            }
        );
    }

    getTemplateSignature(): any {
        return this._httpClient.get(
            environment.currrentBaseURL + '/api/signature/template'
        );
    }

    getChristmasGreeting(): Observable<any> {
        return this._httpClient.get(
            environment.currrentBaseURL + '/api/signature/christmas'
        );
    }
}
