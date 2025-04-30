import { Component, OnInit, ViewEncapsulation } from '@angular/core';
import { FuseSplashScreenService } from '@fuse/services/splash-screen';
import { ClientsService } from './clients.service';
import { Client } from './clients.types';

@Component({
    selector: 'app-clients',
    templateUrl: './clients.component.html',
    styleUrls: ['./clients.component.scss'],
    encapsulation: ViewEncapsulation.None,
})
export class ClientsComponent implements OnInit {
    clients: Client[];
    isLoading: boolean = true;

    constructor(
        private _clientsService: ClientsService,
        private _fuseSplashScreenService: FuseSplashScreenService
    ) {}

    ngOnInit(): void {
        this.refreshData(true);
    }

    onRefreshRequested(): void {
        this.refreshData(false);
    }

    refreshData(isShowSplashScreen: boolean = false): void {
        this.isLoading = true;
        if (isShowSplashScreen) {
            this._fuseSplashScreenService.show();
        }

        this._clientsService.getAllClients().subscribe((data) => {
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
}
