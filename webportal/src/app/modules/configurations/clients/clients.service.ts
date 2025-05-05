/* eslint-disable arrow-parens */
/* eslint-disable @typescript-eslint/naming-convention */
import { HttpClient, HttpParams } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { environment } from 'environments/environment';
import { Observable, of, throwError } from 'rxjs';
import { catchError } from 'rxjs/operators';
import {
    GenericResponse,
    TimeElapsedResponse,
} from '../products/products.types';
import {
    ClientPrimaveraInvoicesResponse,
    ClientPrimaveraOrdersResponse,
    ClientPrimaveraPendingInvoiceOrdersResponse,
    ClientResponse,
    ClientsResponse,
    RatingDiscount,
    RatingDiscountsResponse,
    RatingTypesResponse,
    SegmentsResponse,
    SyncClientResponse,
    UpdateClientRatingsRequest,
} from './clients.types';
import { ClientsMockService } from './clients-mock.service';

@Injectable({
    providedIn: 'root',
})
export class ClientsService {
    constructor(
        private _httpClient: HttpClient,
        private _mock: ClientsMockService
    ) {}

    /**
     * Get All cancel reasons
     */
    getAllClients(limit?: number): Observable<ClientsResponse> {
        let params = new HttpParams();
        if (limit !== undefined) {
            params = params.append('limit', limit.toString());
        }

        return this._httpClient.get<ClientsResponse>(
            environment.currrentBaseURL + '/api/clients',
            { params }
        );
    }

    getClientsBySearch(search: string): Observable<ClientsResponse> {
        return this._httpClient.get<ClientsResponse>(
            environment.currrentBaseURL + '/api/clients/search?query=' + search
        );
    }

    getSegments(): Observable<SegmentsResponse> {
        return this._httpClient.get<SegmentsResponse>(
            environment.currrentBaseURL + '/api/segments'
        );
    }

    patchClientSegment(clientCode: string, segmentId: number): Observable<any> {
        return this._httpClient.patch<any>(
            environment.currrentBaseURL +
                '/api/clients/' +
                clientCode +
                '/segment',
            { segment_id: segmentId }
        );
    }

    getRatingTypes(): Observable<RatingTypesResponse> {
        return this._httpClient.get<RatingTypesResponse>(
            environment.currrentBaseURL + '/api/ratings/types'
        );
    }

    getRatingTypesByType(
        type: 'client' | 'order'
    ): Observable<RatingTypesResponse> {
        return this._httpClient.get<RatingTypesResponse>(
            environment.currrentBaseURL + '/api/ratings/types/' + type
        );
    }

    getClientRatingTypes(): Observable<RatingTypesResponse> {
        return this.getRatingTypesByType('client');
    }

    getOrderClientRatingTypes(): Observable<RatingTypesResponse> {
        return this.getRatingTypesByType('order');
    }

    getRatingDiscounts(): Observable<RatingDiscountsResponse> {
        return this._httpClient.get<RatingDiscountsResponse>(
            environment.currrentBaseURL + '/api/ratings/discounts'
        );
    }

    getClientPrimaveraOrders(
        clientCode: string,
        isOnlyPastMonth: boolean = false
    ): Observable<ClientPrimaveraOrdersResponse> {
        return of(this._mock.getMockClientOrders(clientCode, isOnlyPastMonth));
    }

    getClientPrimaveraInvoices(
        clientCode: string
    ): Observable<ClientPrimaveraInvoicesResponse> {
        return this._httpClient.get<ClientPrimaveraInvoicesResponse>(
            environment.currrentBaseURL +
                '/api/primavera-invoices/client/' +
                clientCode
        );
    }

    patchRating(
        clientCode: string,
        ratingTypeId: number,
        rating: string
    ): Observable<any> {
        return this._httpClient.patch<any>(
            environment.currrentBaseURL +
                '/api/clients' +
                '/ratings/' +
                clientCode +
                '/' +
                ratingTypeId,
            { rating }
        );
    }

    getClientByCode(code: string): Observable<ClientResponse> {
        return this._httpClient.get<ClientResponse>(
            environment.currrentBaseURL + '/api/clients/' + code
        );
    }

    getClientByTokenNoAuth(token: string): Observable<ClientResponse> {
        return this._httpClient.get<ClientResponse>(
            `${environment.currrentBaseURL}/api/clients/no-auth/${token}`
        );
    }

    getClientByCodeByOrderToken(
        code: string,
        orderToken: string
    ): Observable<ClientResponse> {
        return this._httpClient.get<ClientResponse>(
            `${environment.currrentBaseURL}/api/clients/${code}/${orderToken}`
        );
    }

    getClientPendingPrimaveraOrdersAndInvoices(
        code: string
    ): Observable<ClientPrimaveraPendingInvoiceOrdersResponse> {
        return this._httpClient.get<ClientPrimaveraPendingInvoiceOrdersResponse>(
            environment.currrentBaseURL +
                '/api/clients/' +
                code +
                '/rate-credit'
        );
    }

    updateAllClientRatings(
        clientCode: string,
        updateRequest: UpdateClientRatingsRequest
    ): Observable<any> {
        return this._httpClient
            .put<any>(
                environment.currrentBaseURL +
                    '/api/clients/ratings/' +
                    clientCode,
                updateRequest
            )
            .pipe(catchError((error) => throwError(error)));
    }
}
