import { Injectable } from '@angular/core';
import { HttpClient, HttpHeaders, HttpParams } from '@angular/common/http';
import { catchError, map, switchMap } from 'rxjs/operators';
import { Observable, of, throwError } from 'rxjs';
import { environment } from 'environments/environment';
import { Triggers } from './triggers.types';
import { Console } from 'console';
import { TranslocoService } from '@ngneat/transloco';
import { Router } from '@angular/router';

@Injectable({
  providedIn: 'root'
})
export class TriggersService {

  constructor(
    private _httpClient: HttpClient,
    private router: Router,
    private readonly translocoService: TranslocoService
  ) {

  }

  getTriggers(): Observable<any> {
    return this._httpClient.get(environment.currrentBaseURL + '/api/triggers/getTriggers').pipe(
      switchMap((response: any) => {
        return of(response);
      })
    );
  }

  getTriggersByScriptId(id: string): Observable<any> {
    let params;
    if (id) {
        params = new HttpParams().set('id', id.toString());
    }

    return this._httpClient.get(environment.currrentBaseURL + '/api/triggers/getTriggersByScriptId', {
        params: params,
    }).pipe(
          switchMap((response: any) => {
            return of(response);
        })
    );
  }

  addTrigger(
    name: string,
    cron_expression: string,
    script_name: string
  ): Observable<any> {
    return this._httpClient
        .post(environment.currrentBaseURL + '/api/triggers/addTrigger', {
            name: name,
            cron_expression: cron_expression,
            script_name: script_name,
        })
        .pipe(
            switchMap((response: any) => {
                return of(response);
            })
        );
  }

  editTrigger(
    id: string,
    name: string,
    cron_expression: string
  ): Observable<any> {
    return this._httpClient
        .post(environment.currrentBaseURL + '/api/triggers/editTrigger', {
          id: id,
          name: name,
          cron_expression: cron_expression,
        })
        .pipe(
            switchMap((response: any) => {
                return of(response);
            })
        );
  }

  deleteTrigger(id: string): Observable<any> {
    let params = new HttpParams();

    if (id) {
        params = params.set('id', id);
    }

    return this._httpClient
        .post(
            environment.currrentBaseURL + '/api/triggers/removeTrigger',
            null,
            {
                params: params,
            }
        )
        .pipe(
            switchMap((response: any) => {
                return of(response);
            })
        );
  }

  generateCronExpression(scheduleType: string, options: any): string {
    let cron_expression = '';

    switch (scheduleType) {
      case 'daily':
        cron_expression = `0 ${options.minute} ${options.hour} * * ?`;
        break;
      case 'weekly':
        cron_expression = `0 ${options.minute} ${options.hour} ? * ${options.dayOfWeek}`;
        break;
      // Add cases for other schedule types as needed (monthly, advanced, etc.)
      default:
        break;
    }

    return cron_expression;
  }
}
