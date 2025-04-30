/* eslint-disable @typescript-eslint/naming-convention */
import { HttpClient } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { environment } from 'environments/environment';
import { Observable } from 'rxjs';
import { TimeElapsedResponse } from '../products/products.types';
import {
    SyncClientResponse,
    SyncPrimaveraStatsResponse,
} from './clients.types';

@Injectable({
    providedIn: 'root',
})
export class PrimaveraSyncingService {
    constructor(private _httpClient: HttpClient) {}

    syncClients(): Observable<SyncPrimaveraStatsResponse> {
        return this._httpClient.post<SyncPrimaveraStatsResponse>(
            environment.currrentBaseURL + '/api/clients/sync-primavera',
            {}
        );
    }

    syncRating(
        type: 'credit' | 'payment-compliance' | 'historical-volume'
    ): Observable<SyncPrimaveraStatsResponse> {
        const endpoint = `/api/clients/ratings/sync-primavera/${type}`;
        return this._httpClient.post<SyncPrimaveraStatsResponse>(
            environment.currrentBaseURL + endpoint,
            {}
        );
    }

    syncRatingCredits(): Observable<SyncPrimaveraStatsResponse> {
        return this.syncRating('credit');
    }

    syncRatingPaymentCompliance(): Observable<SyncPrimaveraStatsResponse> {
        return this.syncRating('payment-compliance');
    }

    syncHistoricalVolumeRating(): Observable<SyncPrimaveraStatsResponse> {
        return this.syncRating('historical-volume');
    }

    syncProductsCatalogs(): Observable<SyncPrimaveraStatsResponse> {
        return this._httpClient.post<SyncPrimaveraStatsResponse>(
            environment.currrentBaseURL +
                '/api/products/catalogs/sync-primavera',
            {}
        );
    }

    syncProductConversions(): Observable<SyncPrimaveraStatsResponse> {
        return this._httpClient.post<SyncPrimaveraStatsResponse>(
            environment.currrentBaseURL +
                '/api/products/conversions/sync-primavera',
            {}
        );
    }
}
