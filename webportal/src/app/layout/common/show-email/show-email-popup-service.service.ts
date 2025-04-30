/* eslint-disable @typescript-eslint/member-ordering */
import { Injectable } from '@angular/core';
import { EmailAttachment } from 'app/modules/filtering/filtering.types';
import {
    Email,
    EmailPopupState,
} from 'app/shared/components/filtering-validate/details/details.types';
import { BehaviorSubject, Observable } from 'rxjs';

import { Subject } from 'rxjs/internal/Subject';

@Injectable({
    providedIn: 'root',
})
export class ShowEmailPopupServiceService {
    private _drawerOpenSubject = new BehaviorSubject<boolean>(false);
    drawerOpen$: Observable<boolean> = this._drawerOpenSubject.asObservable();

    private _showEmailButton = new Subject<boolean>();
    showEmailButton$: Observable<boolean> =
        this._showEmailButton.asObservable();

    constructor() {}

    /**
     * Toggle the drawer open/closed state
     */
    toggleDrawer(open?: boolean): void {
        const newState =
            open !== undefined ? open : !this._drawerOpenSubject.getValue();
        this._drawerOpenSubject.next(newState);
    }

    /**
     * Get current drawer state
     */

    isDrawerOpen(): boolean {
        return this._drawerOpenSubject.getValue();
    }
    /**
     * Close drawer and clear all data
     */
    closeDrawer(): void {
        this.toggleDrawer(false);
    }

    // Show email button
    showEmailButton(): void {
        this._showEmailButton.next(true);
    }

    hideEmailButton(): void {
        this._showEmailButton.next(false);
    }
}
