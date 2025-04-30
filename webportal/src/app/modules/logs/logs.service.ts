import { Injectable } from '@angular/core';
import { HttpClient, HttpHeaders, HttpParams } from '@angular/common/http';
import { catchError, switchMap } from 'rxjs/operators';
import { Observable, of, throwError } from 'rxjs';
import { environment } from 'environments/environment';
import { UpdateReceiptEntry, LogsEntry, FileInput } from './logs.types';
import { Console } from 'console';

@Injectable({
  providedIn: 'root'
})
export class LogService {

  constructor(
    private _httpClient: HttpClient
  ) {

  }

  getLogs(emailFilter: string, ContextFilter: string): Observable<any> {
    let params;
    if (emailFilter) {
      params = new HttpParams().set('user_operation', emailFilter);
    }

    if(ContextFilter) {
      params = new HttpParams().set('operationContext', ContextFilter);
    }

    return this._httpClient.get(environment.currrentBaseURL + '/api/logs/getLogs', { params: params}).pipe(
      switchMap((response: any) => {
        return of(response);
      })
    );
  }

  

  
}
