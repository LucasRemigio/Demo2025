import { HttpClient, HttpParams } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { environment } from 'environments/environment';
import { Observable } from 'rxjs';
import {
    CancelReasonsResponse,
    ChangeCancelReasonActiveStatus,
    CreateCancelReason,
    UpdateCancelReason,
} from './cancel-reasons.types';

@Injectable({
    providedIn: 'root',
})
export class CancelReasonService {
    constructor(private _httpClient: HttpClient) {}

    /**
     * Get All cancel reasons
     */
    getAllCancelReasons(): Observable<CancelReasonsResponse> {
        return this._httpClient.get<CancelReasonsResponse>(
            environment.currrentBaseURL + '/api/cancelReasons'
        );
    }

    /*
     * Get Active cancel reasons
     */
    getActiveCancelReasons(): Observable<CancelReasonsResponse> {
        const params = new HttpParams().set('isActive', 'true');

        return this._httpClient.get<CancelReasonsResponse>(
            environment.currrentBaseURL + '/api/cancelReasons',
            { params }
        );
    }

    /**
     * Update cancel reason
     */
    updateCancelReason(cancelReason: UpdateCancelReason): Observable<any> {
        return this._httpClient.put<any>(
            environment.currrentBaseURL +
                '/api/cancelReasons/' +
                cancelReason.id,
            cancelReason
        );
    }

    /**
     * Create cancel reason
     */
    createCancelReason(cancelReason: CreateCancelReason): Observable<any> {
        return this._httpClient.post<any>(
            environment.currrentBaseURL + '/api/cancelReasons/',
            cancelReason
        );
    }

    /**
     * Change cancel reason active status
     */
    changeCancelReasonActiveStatus(
        cancelReason: ChangeCancelReasonActiveStatus
    ): Observable<any> {
        return this._httpClient.patch<any>(
            environment.currrentBaseURL +
                '/api/cancelReasons/' +
                cancelReason.id,
            cancelReason
        );
    }
}
