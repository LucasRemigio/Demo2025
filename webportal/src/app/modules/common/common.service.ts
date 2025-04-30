import { Injectable } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { BehaviorSubject, Observable, of, throwError } from 'rxjs';
import { tap } from 'rxjs/operators';
import {
    CardItem,
    Category,
} from './common.types';
import { environment } from 'environments/environment';
import { FuseSplashScreenService } from '@fuse/services/splash-screen';

@Injectable({
    providedIn: 'root',
})
export class CommonService {
    // Private
    private _categories: BehaviorSubject<Category[] | null> =
        new BehaviorSubject(null);

    /**
     * Constructor
     */
    constructor(
        private _httpClient: HttpClient,
    ) {
    }

    // -----------------------------------------------------------------------------------------------------
    // @ Accessors
    // -----------------------------------------------------------------------------------------------------

    /**
     * Getter for categories
     */
    get categories$(): Observable<Category[]> {
        return this._categories.asObservable();
    }

    // -----------------------------------------------------------------------------------------------------
    // @ Public methods
    // -----------------------------------------------------------------------------------------------------

    /**
     * Get categories
     */
    getCategories(): Observable<Category[]> {
        return this._httpClient
            .get<Category[]>(environment.currrentBaseURL + '/api/categories')
            .pipe(
                tap((response: any) => {
                    this._categories.next(response.categories);
                })
            );
    }
    
    

}
