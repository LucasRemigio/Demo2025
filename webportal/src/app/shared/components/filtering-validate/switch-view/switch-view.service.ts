/* eslint-disable @typescript-eslint/member-ordering */
import { Injectable } from '@angular/core';
import { BehaviorSubject, Observable } from 'rxjs';

export enum ViewType {
    card = 'card',
    table = 'table',
}
@Injectable({
    providedIn: 'root',
})
export class SwitchViewService {
    private _selectedViewType = new BehaviorSubject<ViewType>(ViewType.card);
    selectedViewType$: Observable<ViewType> =
        this._selectedViewType.asObservable();

    private _isViewTypeVisible = new BehaviorSubject<boolean>(false);
    isViewTypeVisible$: Observable<boolean> =
        this._isViewTypeVisible.asObservable();

    constructor() {
        // Retrieve the view from localStorage if available
        const viewType = localStorage.getItem(
            'filteringValidateViewType'
        ) as ViewType | null;
        if (viewType === ViewType.card || viewType === ViewType.table) {
            this._selectedViewType.next(viewType);
        }
    }

    // Method to update the view type
    setViewType(view: ViewType): void {
        this._selectedViewType.next(view);
        localStorage.setItem('filteringValidateViewType', view);
    }

    setViewTypeVisibility(visible: boolean): void {
        this._isViewTypeVisible.next(visible);
    }
}
