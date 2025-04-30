import { Injectable } from '@angular/core';
import { ActivatedRouteSnapshot, Resolve, RouterStateSnapshot } from '@angular/router';
import { Observable } from 'rxjs';
import { TriggersService } from '../triggers.service';

@Injectable({
  providedIn: 'root'
})
export class TriggersByScriptIdResolver implements Resolve<any> {
  constructor(private triggerService: TriggersService) {}

  resolve(route: ActivatedRouteSnapshot, state: RouterStateSnapshot): Observable<any> | Promise<any> | any {
    return this.triggerService.getTriggersByScriptId(route.paramMap.get('id'));
  }
}