import { Component, OnInit } from '@angular/core';
import { translate } from '@ngneat/transloco';
import { SwitchViewService, ViewType } from './switch-view.service';

@Component({
    selector: 'app-switch-view',
    templateUrl: './switch-view.component.html',
    styleUrls: ['./switch-view.component.scss'],
})
export class SwitchViewComponent implements OnInit {
    viewOptions = [
        {
            value: 'card',
            icon: 'heroicons_solid:credit-card',
            extraClasses: 'rounded-l-xl',
            tooltip: translate('Filtering.cards-view'),
        },
        {
            value: 'table',
            icon: 'heroicons_solid:table',
            extraClasses: 'rounded-r-xl',
            tooltip: translate('Filtering.table-view'),
        },
    ];

    // this reads the subscribable view type from the service
    viewType$ = this._switchViewService.selectedViewType$;

    constructor(private _switchViewService: SwitchViewService) {}

    ngOnInit(): void {}

    // This changes the subscribable view type of the service
    setViewType(view: ViewType): void {
        this._switchViewService.setViewType(view);
    }
}
