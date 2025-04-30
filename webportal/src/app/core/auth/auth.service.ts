import { Injectable } from '@angular/core';
import { HttpClient, HttpHeaders, HttpParams } from '@angular/common/http';
import { Observable, of, throwError } from 'rxjs';
import { catchError, switchMap } from 'rxjs/operators';
import { AuthUtils } from 'app/core/auth/auth.utils';
import { UserService } from 'app/core/user/user.service';
import { environment } from 'environments/environment';
import {
    Auth2F,
    SendNewAuth2fCode,
    User,
    ValidateAuth2f,
} from 'app/core/user/user.types';

@Injectable()
export class AuthService {
    private _authenticated: boolean = false;
    private _2FAuthenticated: boolean = false;

    /**
     * Constructor
     */
    constructor(
        private _httpClient: HttpClient,
        private _userService: UserService
    ) {}

    // -----------------------------------------------------------------------------------------------------
    // @ Accessors
    // -----------------------------------------------------------------------------------------------------

    /**
     * Setter & getter for access token
     */
    set accessToken(token: string) {
        localStorage.setItem('accessToken', token);
    }

    get accessToken(): string {
        return localStorage.getItem('accessToken') ?? '';
    }

    set codeA2F(code: string) {
        localStorage.setItem('codeA2F', code);
    }

    get codeA2F(): string {
        return localStorage.getItem('codeA2F') ?? '';
    }

    // -----------------------------------------------------------------------------------------------------
    // @ Public methods
    // -----------------------------------------------------------------------------------------------------

    /**
     * Forgot password
     *
     * @param email
     */
    forgotPassword(email: string): Observable<any> {
        return this._httpClient.post(
            environment.currrentBaseURL + '/api/auth/forgotPassword',
            { email: email }
        );
    }

    /**
     * Reset password
     *
     * @param password
     */
    resetPassword(
        email: string,
        token: string,
        password: string
    ): Observable<any> {
        // passar para base64
        return this._httpClient.post(
            environment.currrentBaseURL + '/api/auth/resetPassword',
            { email: email, token: token, password: password }
        );
    }

    /**
     * Sign in
     *
     * @param credentials
     */
    signIn(
        credentials: { email: string; password: string },
        useAuth2f: boolean
    ): Observable<any> {
        // Throw error, if the user is already logged in
        if (this._authenticated) {
            return throwError('User is already logged in.');
        }

        return this._httpClient
            .post(environment.currrentBaseURL + '/api/auth/signIn', credentials)
            .pipe(
                switchMap((response: any) => {
                    if (
                        response.result_code &&
                        Number(response.result_code) > 0 &&
                        response.token
                    ) {
                        // Store the access token in the local storage
                        this.accessToken = response.token;
                        useAuth2f = response.useAuth2f;

                        // Set the authenticated flag to true
                        this._authenticated = true;

                        // Store the user on the user service
                        const userLog: User = {
                            name: response.user,
                            email: response.email,
                            role: response.role,
                            departments: response.departments,
                        };

                        this._userService.user = userLog;

                        if (!response.useAuth2f) {
                            this._2FAuthenticated = true;
                        }
                    }

                    // Return a new observable with the response
                    return of(response);
                })
            );
    }

    validateAuth2FCode(auth2f_clientInput: ValidateAuth2f): Observable<any> {
        return this._httpClient
            .post(
                environment.currrentBaseURL + '/api/auth/validateAuth2fcode',
                auth2f_clientInput
            )
            .pipe(
                switchMap((response: any) => {
                    if (response.result_code == 1) {
                        this.codeA2F = auth2f_clientInput.auth2f_code;
                        this._2FAuthenticated = true;
                        return of(response);
                    }
                    return of(response);
                })
            );
    }

    /**
     * Send New Auth2f Code
     *
     *
     */
    sendNewAuth2fCode(email: string): Observable<any> {
        return this._httpClient
            .post(environment.currrentBaseURL + '/api/auth/sendNewAuth2fCode', {
                email: email,
            })
            .pipe(
                switchMap((response: any) => {
                    return of(response);
                })
                // )
            );
    }

    enableAuth2f(useAuth2f: string): Observable<any> {
        return this._httpClient
            .post(environment.currrentBaseURL + '/api/auth/sendNewAuth2fCode', {
                useAuth2f: useAuth2f,
            })
            .pipe(
                switchMap((response: any) => {
                    return of(response);
                })
            );
    }

    /**
     * Sign in using the access token
     */
    signInUsingToken(): Observable<any> {
        // Renew token
        return this._httpClient
            .post(
                environment.currrentBaseURL + '/api/auth/refreshAccessToken',
                {
                    access_token: this.accessToken,
                    a2f_code: this.codeA2F,
                }
            )
            .pipe(
                catchError(() => {
                    this.signOut();
                    return of(false);
                }),
                switchMap((response: any) => {
                    if (!response.token) {
                        return of(false);
                    }

                    // Store the access token in the local storage
                    this.accessToken = response.token;

                    // Set the authenticated flag to true
                    this._authenticated = true;
                    this._2FAuthenticated = true;

                    // Store the user on the user service
                    const userLog: User = {
                        name: response.user,
                        email: response.email,
                        role: response.role,
                        departments: response.departments,
                    };

                    this._userService.user = userLog;

                    // Return true
                    return of(true);
                })
            );
    }

    /**
     * Sign out
     */
    signOut(): Observable<any> {
        // Remove the access token from the local storage
        localStorage.clear();

        // Set the authenticated flag to false
        this._authenticated = false;
        this._2FAuthenticated = false;

        // Return the observable
        return of(true);
    }

    /**
     * Sign up
     *
     * @param user
     */
    signUp(user: { name: string; email: string; password }): Observable<any> {
        return this._httpClient.post(
            environment.currrentBaseURL + '/api/auth/signUp',
            user
        );
    }

    /**
     * Unlock session
     *
     * @param credentials
     */
    unlockSession(credentials: {
        email: string;
        password: string;
    }): Observable<any> {
        return this._httpClient.post('api/auth/unlock-session', credentials);
    }

    /**
     * Check the authentication status
     */
    check(): Observable<boolean> {
        // Check if the user is logged in
        if (this._authenticated && this._2FAuthenticated) {
            return of(true);
        }

        // Check the access token availability
        if (!this.accessToken) {
            localStorage.removeItem('accessToken');
            return of(false);
        }

        // Check the access token expire date
        if (AuthUtils.isTokenExpired(this.accessToken)) {
            localStorage.removeItem('accessToken');
            return of(false);
        }

        return this.signInUsingToken();
    }
}
