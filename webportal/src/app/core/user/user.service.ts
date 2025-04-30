import { Injectable } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { BehaviorSubject, Observable, of, ReplaySubject } from 'rxjs';
import { map, tap } from 'rxjs/operators';
import { User, Department } from 'app/core/user/user.types';
import { Password } from 'app/core/user/user.types';
import { catchError, switchMap } from 'rxjs/operators';
import { environment } from 'environments/environment';

@Injectable({
    providedIn: 'root',
})
export class UserService {
    private _user: ReplaySubject<User> = new ReplaySubject<User>(1);
    private _currentUser: User;
    private _isAdmin: boolean = false;
    private _isSupervisor: boolean = false;
    private _departments: string[] = [];

    private _userPages: BehaviorSubject<string[] | null> = new BehaviorSubject(
        null
    );

    /**
     * Constructor
     */
    constructor(private _httpClient: HttpClient) {}

    // -----------------------------------------------------------------------------------------------------
    // @ Accessors
    // -----------------------------------------------------------------------------------------------------

    /**
     * Setter & getter for user
     *
     * @param value
     */
    set user(value: User) {
        if (value && value.role == 'ADMIN') {
            this._isAdmin = true;
            this._isSupervisor = true;
        } else if (value && value.role === 'SUPERVISOR') {
            this._isSupervisor = true;
            this._isAdmin = false;
        } else {
            this._isAdmin = false;
            this._isSupervisor = false;
        }

        this._departments = value.departments.map(
            (department) => department.name
        );

        this._user.next(value);
        this._currentUser = value;
    }

    set userPages(pages: string[]) {
        this._userPages.next(pages);
    }

    get user$(): Observable<User> {
        return this._user.asObservable();
    }

    get userPages$(): Observable<string[]> {
        return this._userPages.asObservable();
    }

    // -----------------------------------------------------------------------------------------------------
    // @ Public methods
    // -----------------------------------------------------------------------------------------------------

    /**
     * Get the current logged in user data
     */
    get(): Observable<User> {
        return this._user.asObservable();
    }

    getLoggedUserEmail(): string {
        return this._currentUser.email;
    }

    getLoggedUserName(): string {
        return this._currentUser.name;
    }

    /**
     * Update the user
     *
     * @param user
     */
    update(user: User): Observable<any> {
        this._user.next(user);
        return this._user.asObservable();
    }

    isAdmin(): boolean {
        return this._isAdmin;
    }

    isSupervisor(): boolean {
        return this._isSupervisor;
    }

    getDepartments(): string[] {
        return this._departments;
    }

    updatePass(NewPassword: Password): Observable<any> {
        return this._httpClient
            .post(
                environment.currrentBaseURL + '/api/user/updatePass',
                NewPassword
            )
            .pipe(
                switchMap((response: any) => {
                    return of(response);
                })
            );
    }
}
