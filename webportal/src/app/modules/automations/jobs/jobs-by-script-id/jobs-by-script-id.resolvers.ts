import { Injectable } from '@angular/core';
import { ActivatedRouteSnapshot, Resolve, RouterStateSnapshot } from '@angular/router';
import { Observable } from 'rxjs';
import { JobsService } from '../jobs.service';

@Injectable({
  providedIn: 'root'
})
export class JobsByScriptIdResolver implements Resolve<any> {
  constructor(private jobService: JobsService) {}

  resolve(route: ActivatedRouteSnapshot, state: RouterStateSnapshot): Observable<any> | Promise<any> | any {
    return this.jobService.getJobsByScriptId(route.paramMap.get('id'));
  }
}