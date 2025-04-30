/* eslint-disable quote-props */
import { Component, Input, OnChanges, OnInit } from '@angular/core';
import { TranslocoService } from '@ngneat/transloco';
import { ProductConversionDTO } from 'app/shared/components/email-product-table/products.types';
import { FlashMessageService } from 'app/shared/components/flash-message/flash-message.service';
import { PrimaveraSyncingService } from '../../clients/primavera-syncing.service';

@Component({
    selector: 'app-conversions-table',
    templateUrl: './conversions-table.component.html',
    styleUrls: ['./conversions-table.component.scss'],
})
export class ConversionsTableComponent implements OnInit, OnChanges {
    @Input() conversions: ProductConversionDTO[] = [];
    isSyncingConversions: boolean = false;

    constructor(
        private _primaveraSyncingService: PrimaveraSyncingService,
        private _flashMessage: FlashMessageService,
        private _transloco: TranslocoService
    ) {}

    ngOnInit(): void {}

    ngOnChanges(): void {}

    syncPrimaveraConversions(): void {
        this.isSyncingConversions = true;

        this._primaveraSyncingService.syncProductConversions().subscribe(
            (response) => {
                if (response.result_code <= 0) {
                    this._flashMessage.error(
                        'sync-product-conversions-primavera-error'
                    );
                }

                if (response.statistics.total_syncs > 0) {
                    const message =
                        response.statistics.total_syncs +
                        ' ' +
                        this._transloco.translate(
                            'sync-product-conversions-primavera-success'
                        ) +
                        ' ' +
                        this._transloco.translate('in') +
                        ' ' +
                        response.statistics.time_elapsed_ms +
                        ' ms';
                    this._flashMessage.ntsuccess(message);

                    // emit update only if anything changed
                } else {
                    this._flashMessage.info(
                        'primavera-product-conversions-already-up-to-date'
                    );
                }
            },
            (error) => {
                this._flashMessage.error(
                    'sync-product-conversions-primavera-error'
                );
                return null;
            },
            () => {
                this.isSyncingConversions = false;
            }
        );
    }
    public isSmallScreen(): boolean {
        return window.innerWidth < 768;
    }

    public getHeaderStyle(): any {
        return {
            'background-color': '#383838',
            color: 'white',
            'font-weight': 'bold',
            height: 'auto',
            'vertical-align': 'middle',
            'text-align': 'center',
            'max-width': '150px',
        };
    }
}
