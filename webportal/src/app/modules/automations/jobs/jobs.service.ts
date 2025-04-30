import { Injectable } from '@angular/core';
import { HttpClient, HttpHeaders, HttpParams } from '@angular/common/http';
import { catchError, map, switchMap } from 'rxjs/operators';
import { Observable, of, throwError } from 'rxjs';
import { environment } from 'environments/environment';
import {JobsEntry, FileInput } from './jobs.types';
import { Console } from 'console';
import { TranslocoService } from '@ngneat/transloco';
import { Router } from '@angular/router';

@Injectable({
  providedIn: 'root'
})
export class JobsService {

  constructor(
    private _httpClient: HttpClient,
    private router: Router,
    private readonly translocoService: TranslocoService
  ) {

  }

  getJobs(): Observable<any> {
    // let params;
    // if (emailFilter) {
    //   params = new HttpParams().set('user_operation', emailFilter);
    // }

    // if(ContextFilter) {
    //   params = new HttpParams().set('operationContext', ContextFilter);
    // }

    return this._httpClient.get(environment.currrentBaseURL + '/api/jobs/getJobs').pipe(
      switchMap((response: any) => {
        return of(response);
      })
    );
  }

  getJobsByScriptId(id: string): Observable<any> {
    let params;
    if (id) {
        params = new HttpParams().set('id', id.toString());
    }

    return this._httpClient.get(environment.currrentBaseURL + '/api/jobs/getJobsByScriptId', {
        params: params,
    }).pipe(
          switchMap((response: any) => {
            return of(response);
        })
    );
}

  StartScripts(id: string): Observable<any> {
    const params = id ? new HttpParams().set('id', id) : undefined;
  
    return this._httpClient
    .post(environment.currrentBaseURL + '/api/scripts/startScript', { id }, { params })
      .pipe(
        switchMap((response: any) => {
          return of(response);
        })
      );
  }
}
