import { HttpClient } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { environment } from 'environments/environment';
import { Observable, throwError } from 'rxjs';
import { catchError, map } from 'rxjs/operators';
import {
    PlatformSetting,
    PlatformSettingsResponse,
    UpdateAppSetting,
} from './platform-settings.types';

@Injectable({
    providedIn: 'root',
})
export class PlatformSettingsService {
    constructor(private _httpClient: HttpClient) {}

    /**
     * Get All platform settings
     */
    getAllPlatformSettings(): Observable<PlatformSettingsResponse> {
        return this._httpClient
            .get<PlatformSettingsResponse>(
                environment.currrentBaseURL + '/api/app-settings'
            )
            .pipe(
                map(this.handleResponse),
                catchError(this.handleError('Error fetching platform settings'))
            );
    }

    /**
     * Update platform settings
     */
    updatePlatformSettings(
        settings: UpdateAppSetting
    ): Observable<PlatformSettingsResponse> {
        return this._httpClient
            .patch<PlatformSettingsResponse>(
                environment.currrentBaseURL +
                    '/api/app-settings/' +
                    settings.id,
                {
                    value: settings.setting_value,
                }
            )
            .pipe(
                map(this.handleResponse),
                catchError(this.handleError('Error updating platform settings'))
            );
    }

    /**
     * Handle API response and check for error codes
     */
    private handleResponse(
        response: PlatformSettingsResponse
    ): PlatformSettingsResponse {
        // If result code is negative or zero, treat it as an error
        if (response.result_code <= 0) {
            throw new Error(`API Error: ${response.result || 'Unknown error'}`);
        }
        return response;
    }

    /**
     * Create a function that will handle HTTP errors consistently
     */
    private handleError(defaultMessage: string) {
        return (error: any): Observable<never> => {
            // Handle both HTTP errors and our custom errors
            const errorMessage = error.message || defaultMessage;
            return throwError(errorMessage);
        };
    }
}
