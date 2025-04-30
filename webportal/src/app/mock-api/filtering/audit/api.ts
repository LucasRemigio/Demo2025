import { Injectable } from '@angular/core';
import { cloneDeep } from 'lodash-es';
import { FuseMockApiService } from '@fuse/lib/mock-api';
import {mockFilteredEmails} from './data';
@Injectable({
    providedIn: 'root'
})
export class FilteringMockApi
{
    private _filteredEmails: any = mockFilteredEmails;

    /**
     * Constructor
     */
    constructor(private _fuseMockApiService: FuseMockApiService)
    {
        // Register Mock API handlers
        this.registerHandlers();
    }

    // -----------------------------------------------------------------------------------------------------
    // @ Public methods
    // -----------------------------------------------------------------------------------------------------

    /**
     * Register Mock API handlers
     */
    registerHandlers(): void
    {
        // -----------------------------------------------------------------------------------------------------
        // @ Filtered Emails - GET
        // -----------------------------------------------------------------------------------------------------
        this._fuseMockApiService
            .onGet('mockapi/filtering/filtered', 1000)
            .reply(() =>{
                return [200, cloneDeep(this._filteredEmails)];
            });
    }

    // -----------------------------------------------------------------------------------------------------
    // @ Private methods
    // -----------------------------------------------------------------------------------------------------

    
}
