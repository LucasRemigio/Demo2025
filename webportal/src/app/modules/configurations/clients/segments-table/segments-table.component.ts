/* eslint-disable arrow-parens */
import {
    ChangeDetectorRef,
    Component,
    EventEmitter,
    Input,
    OnChanges,
    OnInit,
    Output,
    ViewEncapsulation,
} from '@angular/core';
import { MatDialog, MatDialogConfig } from '@angular/material/dialog';
import { TranslocoService } from '@ngneat/transloco';
import { FlashMessageService } from 'app/shared/components/flash-message/flash-message.service';
import { ClientsService } from '../clients.service';
import { Client } from '../clients.types';
import { PrimaveraSyncingService } from '../primavera-syncing.service';
import { ChangeClientSegmentComponent } from './change-client-segment/change-client-segment.component';

@Component({
    selector: 'app-segments-table',
    templateUrl: './segments-table.component.html',
    styleUrls: ['./segments-table.component.scss'],
    encapsulation: ViewEncapsulation.None,
})
export class SegmentsTableComponent implements OnInit, OnChanges {
    @Input() clients: Client[] = [];
    @Input() isLoading: boolean = true;
    @Output() refreshRequested = new EventEmitter<void>();

    filteredClients: Client[] = [];
    showOnlyPendingClients: boolean = false;
    isSyncingPrimavera: boolean = false;
    pendingClientsCount: number = 0;

    constructor(
        private _matDialog: MatDialog,
        private _changeDetectorRef: ChangeDetectorRef,
        private _transloco: TranslocoService,
        private _clientsService: ClientsService,
        private _flashMessageService: FlashMessageService,
        private _primaveraSyncingService: PrimaveraSyncingService
    ) {}

    ngOnInit(): void {}

    ngOnChanges(): void {
        this.refreshData();
    }

    refreshData(): void {
        if (!Array.isArray(this.clients)) {
            return;
        }

        this.pendingClientsCount = this.clients.filter(
            (client) => client.updated_at === null
        ).length;

        if (this.showOnlyPendingClients) {
            this.filteredClients = this.clients.filter(
                (client) => client.updated_at === null
            );
            return;
        }

        this.filteredClients = this.clients;
    }
    /**
     * Show flash message
     */

    syncClients(): void {
        if (this.isSyncingPrimavera) {
            return;
        }
        this.isSyncingPrimavera = true;
        this._primaveraSyncingService.syncClients().subscribe(
            (response) => {
                this.isSyncingPrimavera = false;
                if (response.result_code <= 0) {
                    this._flashMessageService.error(
                        'sync-clients-primavera-error'
                    );
                }

                if (response.statistics.total_syncs > 0) {
                    this._flashMessageService.ntsuccess(
                        response.statistics.total_syncs +
                            ' ' +
                            this._transloco.translate(
                                'sync-clients-primavera-success'
                            ) +
                            ' ' +
                            this._transloco.translate('in') +
                            ' ' +
                            response.statistics.time_elapsed_ms +
                            ' ms'
                    );
                } else {
                    this._flashMessageService.info(
                        'primavera-clients-already-up-to-date'
                    );
                }

                this.refreshRequested.emit();
            },
            (error) => {
                this.isSyncingPrimavera = false;
                this._flashMessageService.error('sync-clients-primavera-error');
            }
        );
    }

    changeClientSegment(client: Client): void {
        // open the dialog to edit the segment
        const dialogConfig: MatDialogConfig = {
            maxHeight: '80vh',
            minHeight: '200px',
            height: 'auto',
            maxWidth: '80vw',
            minWidth: '300px',
            width: 'auto',
            data: {
                client,
            },
        };

        const dialogRef = this._matDialog.open(
            ChangeClientSegmentComponent,
            dialogConfig
        );

        dialogRef.afterClosed().subscribe((result) => {
            if (result) {
                // Refresh data after successful create or update
                this._flashMessageService.success('change-segment-success');
                this.refreshRequested.emit();
            }
        });
    }

    public getSegmentTooltip(client: Client): string {
        if (client.updated_at === null) {
            return this._transloco.translate('segment-not-updated');
        }

        const formatedDateTime = new Date(client.updated_at).toLocaleString(
            'pt-PT',
            {
                day: '2-digit',
                month: '2-digit',
                year: 'numeric',
                hour: '2-digit',
                minute: '2-digit',
                second: '2-digit',
                hour12: false,
            }
        );

        const label =
            this._transloco.translate('segment-updated-at') +
            ' ' +
            formatedDateTime +
            ' ' +
            this._transloco.translate('by') +
            ' ' +
            client.updated_by;

        return label;
    }

    public getHeaderStyle(): any {
        return {
            'background-color': '#383838',
            // eslint-disable-next-line quote-props
            color: 'white',
            'font-weight': 'bold',
        };
    }

    public getRowClass(clientDataItem: any): { [key: string]: boolean } {
        const client: Client = clientDataItem.dataItem;

        if (client.updated_by === null || client.updated_by === '') {
            return {
                warning: true,
            };
        }

        return {};
    }

    public isSmallScreen(): boolean {
        return window.innerWidth < 768;
    }
}
