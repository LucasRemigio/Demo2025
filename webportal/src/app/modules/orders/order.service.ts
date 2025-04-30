import { Injectable } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { BehaviorSubject, Observable, of, throwError } from 'rxjs';
import { catchError, map, switchMap, tap } from 'rxjs/operators';
import { cloneDeep } from 'lodash-es';
import {
    CardCategory,
    CardItem,
    Dashboard,
    Base64Csv,
    Email,
    FwdOrder,
    OrderSteps,
} from './order.types';
import { environment } from 'environments/environment';
import { FuseSplashScreenService } from '@fuse/services/splash-screen';

@Injectable({
    providedIn: 'root',
})
export class OrderService {
    // Private
    private _categories: BehaviorSubject<CardCategory[] | null> =
        new BehaviorSubject(null);

    private _librarys: BehaviorSubject<CardItem[] | null> = new BehaviorSubject(
        null
    );
    private _library: BehaviorSubject<CardItem | null> = new BehaviorSubject(
        null
    );

    private _sources: BehaviorSubject<CardItem[] | null> = new BehaviorSubject(
        null
    );

    private _dashboard: BehaviorSubject<Dashboard | null> = new BehaviorSubject(
        null
    );

    private _Base64: BehaviorSubject<Base64Csv | null> = new BehaviorSubject(
        null
    );

    private _operatorOrderSteps: BehaviorSubject<OrderSteps[] | null> =
        new BehaviorSubject(null);
    private _clientOrderSteps: BehaviorSubject<OrderSteps[] | null> =
        new BehaviorSubject(null);

    /**
     * Constructor
     */
    constructor(
        private _httpClient: HttpClient,
        private _fuseSplashScreenService: FuseSplashScreenService
    ) {
        this.getOrderSteps();
    }

    // -----------------------------------------------------------------------------------------------------
    // @ Accessors
    // -----------------------------------------------------------------------------------------------------

    /**
     * Getter for categories
     */
    get categories$(): Observable<CardCategory[]> {
        return this._categories.asObservable();
    }

    /**
     * Getter for Library
     */
    get librarys$(): Observable<CardItem[]> {
        return this._librarys.asObservable();
    }

    /**
     * Getter for Librarys
     */
    get library$(): Observable<CardItem> {
        return this._library.asObservable();
    }

    /**
     * Getter for OperatorOrderSteps
     */
    get operatorOrderSteps$(): Observable<OrderSteps[]> {
        return this._operatorOrderSteps.asObservable();
    }

    /**
     * Getter for ClientOrderSteps
     */
    get clientOrderSteps$(): Observable<OrderSteps[]> {
        return this._clientOrderSteps.asObservable();
    }

    /**
     * Getter for Sources
     */
    get sources$(): Observable<CardItem[]> {
        return this._sources.asObservable();
    }

    get dashboard$(): Observable<Dashboard> {
        return this._dashboard.asObservable();
    }

    // -----------------------------------------------------------------------------------------------------
    // @ Public methods
    // -----------------------------------------------------------------------------------------------------

    /**
     * Get categories
     */
    getCategories(): Observable<CardCategory[]> {
        return this._httpClient
            .get<CardCategory[]>('api/categories/getallcategories', {
                params: { id_department: '2' },
            })
            .pipe(
                tap((response: any) => {
                    this._categories.next(response.categories);
                })
            );
    }

    getOrderSteps() {
        this._httpClient
            .get<OrderSteps[]>('api/orders/getAllOrderSteps')
            .subscribe((response: any) => {
                this._operatorOrderSteps.next(response.operatorStepsItems);
                this._clientOrderSteps.next(response.clientOrderStepsItems);
            });
    }

    /**
     * Get Librarys
     */
    getLibrarys(): Observable<CardItem[]> {
        this._fuseSplashScreenService.show();
        return this._httpClient
            .get<CardItem[]>('api/orders/GetAllOrdersToBeTrated')
            .pipe(
                tap((response: any) => {
                    this._librarys.next(response.order);
                    this._fuseSplashScreenService.hide();
                })
            );
    }

    /**
     * Get Library by id
     */
    getLibraryById(id: string): Observable<CardItem> {
        this._fuseSplashScreenService.show();
        return this._httpClient
            .get<CardItem>('api/orders/getOrderById', {
                params: { id },
            })
            .pipe(
                tap((response: any) => {
                    const steps = cloneDeep(this._operatorOrderSteps);
                    var library: CardItem = response.order;

                    if (library) {
                        library.steps = steps.getValue();
                    }

                    this._library.next(library);
                    this._fuseSplashScreenService.hide();
                    return library;
                })
            );
    }

    edit(order: CardItem): Observable<any> {
        return this._httpClient.post('api/orders/editOrder', order).pipe(
            catchError(() => {
                return of(false);
            }),
            switchMap((response: any) => {
                if (response.result_code && Number(response.result_code) > 0) {
                    this._library.next(order);
                    return of(true);
                } else {
                    return of(false);
                }
            })
        );
    }

    reprocessing(client_id: string,  order_token: string, synchronize: string): Observable<any> {
        return this._httpClient.post('api/orders/reprocessing', {
            client_id: client_id,
            orderToken: order_token,
            synchronize: synchronize,
        }).pipe(
            catchError(() => {
                return of(false);
            }),
            switchMap((response: any) => {
                if (response && response.result_code && Number(response.result_code) > 0) {
                    return of(true);
                } else {
                    return of(false);
                }
            })
        );
    }

    /**
     * Get Library by token
     */
    getLibraryByToken(order_token: string): Observable<CardItem> {
        return this._httpClient
            .get<CardItem>('api/orders/getOrderByToken', {
                params: { order_token },
            })
            .pipe(
                tap((response: any) => {
                    this._fuseSplashScreenService.show();
                    const steps = cloneDeep(this._clientOrderSteps);
                    var library: CardItem = response.order;
                    if (library) {
                        library.steps = steps.getValue();
                    }

                    this._library.next(library);
                    this._fuseSplashScreenService.hide();
                    return library;
                })
            );
    }

    /**
     * Get Sources
     */
    getSources(start_date?: string, end_date?: string): Observable<CardItem[]> {
        this._fuseSplashScreenService.show();

        return this._httpClient
            .get<CardItem[]>('api/orders/getAllOrders', {
                params: { start_date: start_date, end_date: end_date },
            })
            .pipe(
                tap((response: any) => {
                    this._sources.next(response.order);
                    this._fuseSplashScreenService.hide();
                })
            );
    }

    sendNotifications(email: Email): Observable<any> {
        return this._httpClient
            .post('api/orders/sendNotifications', email)
            .pipe(
                switchMap((response: any) => {
                    return of(response);
                })
            );
    }

    fwdOrder(input: FwdOrder): Observable<any> {
        input.order.id = String(input.order.id);
        return this._httpClient.post('api/orders/fwdOrder', input).pipe(
            switchMap((response: any) => {
                return of(response);
            })
        );
    }

    getDashboard(start_date: string, end_date: string): Observable<Dashboard> {
        this._fuseSplashScreenService.show();

        return this._httpClient
            .get<Dashboard>('api/orders/getOrdersDashboard', {
                params: { start_date: start_date, end_date: end_date },
            })
            .pipe(
                tap((response: any) => {
                    this._dashboard.next(response.dashboard);
                    this._fuseSplashScreenService.hide();
                })
            );
    }

    getAllOrdersBase64Csv(
        start_date: string,
        end_date: string
    ): Observable<Base64Csv> {
        this._fuseSplashScreenService.show();

        return this._httpClient
            .get<Base64Csv>('api/orders/getAllOrdersCsv', {
                params: { start_date: start_date, end_date: end_date },
            })
            .pipe(
                tap((response: any) => {
                    this._Base64.next(response.Base64);
                    this._fuseSplashScreenService.hide();
                })
            );
    }

    getBase64Csv(start_date: string, end_date: string): Observable<Base64Csv> {
        this._fuseSplashScreenService.show();

        return this._httpClient
            .get<Base64Csv>('api/orders/getBase64Csv', {
                params: { start_date: start_date, end_date: end_date },
            })
            .pipe(
                tap((response: any) => {
                    this._Base64.next(response.Base64);
                    this._fuseSplashScreenService.hide();
                })
            );
    }
}
