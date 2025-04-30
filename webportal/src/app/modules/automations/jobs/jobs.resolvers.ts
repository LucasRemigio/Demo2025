import { Injectable } from "@angular/core";
import { ActivatedRouteSnapshot, Resolve, RouterStateSnapshot } from "@angular/router";
import { JobsService } from "./jobs.service";
import { Jobs } from "./jobs.types";
import { Observable } from "rxjs";
import moment from "moment";

@Injectable({
    providedIn: 'root',
})
export class JobsResolver implements Resolve<any> {
    constructor(private _jobService: JobsService) {}

    resolve(
        route: ActivatedRouteSnapshot,
        state: RouterStateSnapshot
    ): Observable<Jobs[]> {
        return this._jobService.getJobs();
    }
}
