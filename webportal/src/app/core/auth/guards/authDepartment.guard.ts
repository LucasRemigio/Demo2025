import { Injectable } from '@angular/core';
import {
    ActivatedRouteSnapshot,
    CanActivate,
    Router,
    RouterStateSnapshot,
    UrlTree,
} from '@angular/router';
import { Observable } from 'rxjs';
import { UserService } from 'app/core/user/user.service';

@Injectable({
    providedIn: 'root',
})
export class AuthDepartmentGuard implements CanActivate {
    constructor(private _userService: UserService, private router: Router) {}

    canActivate(
        route: ActivatedRouteSnapshot,
        state: RouterStateSnapshot
    ): Observable<boolean> | Promise<boolean> | boolean {
        const userDepartments: string[] = this._userService.getDepartments();
        const requiredDepartment = route.data['requiredDepartment'];

        if (!userDepartments.includes(requiredDepartment)) {
            // Redirect to unauthorized page
            this.router.navigate(['/unauthorized']);
            return false;
        }
        // Allow access if the user belongs to the required department
        return true;
    }

    canLoad(): boolean {
        // Implement similar logic as canActivate for lazy-loaded modules if needed
        return true;
    }
}
