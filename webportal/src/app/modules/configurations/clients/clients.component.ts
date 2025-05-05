/* eslint-disable @typescript-eslint/naming-convention */
import {
    AfterViewInit,
    Component,
    OnInit,
    ViewChild,
    ViewEncapsulation,
} from '@angular/core';
import { MatTabGroup } from '@angular/material/tabs';
import { Router, ActivatedRoute } from '@angular/router';
import { FuseSplashScreenService } from '@fuse/services/splash-screen';
import { ClientsService } from './clients.service';
import { Client } from './clients.types';

@Component({
    selector: 'app-clients',
    templateUrl: './clients.component.html',
    styleUrls: ['./clients.component.scss'],
    encapsulation: ViewEncapsulation.None,
})
export class ClientsComponent implements OnInit, AfterViewInit {
    @ViewChild('tabGroup') tabGroup: MatTabGroup;

    clients: Client[];
    isLoading: boolean = true;

    // Define tab IDs
    readonly TAB_IDS = {
        SEGMENTS: 'segments',
        RATINGS: 'ratings',
    };

    constructor(
        private _clientsService: ClientsService,
        private _fuseSplashScreenService: FuseSplashScreenService,
        private _router: Router,
        private _route: ActivatedRoute
    ) {}

    ngOnInit(): void {
        this.refreshData(true);
    }

    ngAfterViewInit(): void {
        // Wait for the view to be initialized before selecting the tab
        this._route.queryParams.subscribe((params) => {
            const tab = params['tab'];
            if (tab) {
                // Navigate to the appropriate tab based on name
                this.selectTabByName(tab);
            }
        });
    }

    onRefreshRequested(): void {
        this.refreshData(false);
    }

    refreshData(isShowSplashScreen: boolean = false): void {
        this.isLoading = true;
        if (isShowSplashScreen) {
            this._fuseSplashScreenService.show();
        }

        this._clientsService.getAllClients(20).subscribe((data) => {
            if (!data || data.result_code <= 0) {
                console.error('Error fetching clients data');
                return;
            }

            this.clients = data.clients;
            this.isLoading = false;

            if (isShowSplashScreen) {
                this._fuseSplashScreenService.hide();
            }
        });
    }

    onTabChange(event: any): void {
        // Get tab index
        const tabIndex = event.index;
        // Use a lookup to convert index to tab ID
        const tabIds = [this.TAB_IDS.SEGMENTS, this.TAB_IDS.RATINGS];
        const tabId = tabIds[tabIndex] || '';

        this._router.navigate([], {
            relativeTo: this._route,
            queryParams: { tab: tabId },
            queryParamsHandling: 'merge',
        });
    }

    selectTabByName(tabId: string): void {
        if (!this.tabGroup) {
            return;
        }

        // Map tabId to index using indexOf
        const tabIds = [this.TAB_IDS.SEGMENTS, this.TAB_IDS.RATINGS];
        const index = tabIds.indexOf(tabId);

        if (index !== -1) {
            this.tabGroup.selectedIndex = index;
        }
    }
}
