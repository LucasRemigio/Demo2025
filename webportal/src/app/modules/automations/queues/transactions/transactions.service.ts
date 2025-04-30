import { Injectable } from '@angular/core';
import { HttpClient, HttpHeaders, HttpParams } from '@angular/common/http';
import { catchError, switchMap } from 'rxjs/operators';
import { Observable, of, throwError } from 'rxjs';
import { environment } from 'environments/environment';
import { Transactions } from '../transactions/transactions.types';

@Injectable({
    providedIn: 'root',
})
export class TransactionsService {
    constructor(private _httpClient: HttpClient) {}

    getTransactionsByQueueId(id: string): Observable<any>{
        let params;
        if (id) {
            params = new HttpParams().set('id', id.toString());
        }

        return this._httpClient
            .get(environment.currrentBaseURL + '/api/queues/getTransactionByQueueId', {
                params: params,
            })

            .pipe(
                switchMap((response: any) => {
                    return of(response);
                })
            );

    }

    addTransaction(inputData: any): Observable<any> {
        return this._httpClient.post(environment.currrentBaseURL + '/api/queues/addTransaction', inputData)
            .pipe(
                switchMap((response: any) => {
                    return response;
                })
            );
    }

    removeTransaction(id: string): Observable<any> {
        const params = new HttpParams().set('id', id);
        return this._httpClient.post(environment.currrentBaseURL + '/api/queues/removeTransaction', null, { params: params });
            // .pipe(
            //     switchMap((response: any) => {
            //         return response;
            //     })
            // );
    }

    editTransaction(
        id: string,
        status_id: string,
        started: string,
        ended: string,
        exception: string,
        output_data: string
      ): Observable<any> {
        return this._httpClient.post(environment.currrentBaseURL + '/api/queues/editTransaction', {
          id: id,
          status_id: status_id,
          started: started,
          ended: ended,
          exception: exception,
          output_data: output_data
        });
      }

}