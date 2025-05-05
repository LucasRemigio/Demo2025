/* eslint-disable arrow-parens */
/* eslint-disable quote-props */
import {
    ChangeDetectorRef,
    Component,
    EventEmitter,
    Input,
    OnChanges,
    OnInit,
    Output,
    SimpleChanges,
    ViewEncapsulation,
} from '@angular/core';
import { MatDialog, MatDialogConfig } from '@angular/material/dialog';
import { DomSanitizer, SafeStyle } from '@angular/platform-browser';
import { TranslocoService } from '@ngneat/transloco';
import { FlashMessageService } from 'app/shared/components/flash-message/flash-message.service';
import { ClientsService } from '../clients.service';
import {
    Client,
    ClientCodeResponse,
    ClientRatingDTO,
    RatingSlugs,
    RatingType,
    SyncingRatingStates,
    SyncPrimaveraStatsResponse,
} from '../clients.types';
import { PrimaveraSyncingService } from '../primavera-syncing.service';
import { EditAllClientRatingsComponent } from './edit-all-client-ratings/edit-all-client-ratings.component';
import { EditClientRatingComponent } from './edit-client-rating/edit-client-rating.component';
import { ViewClientPrimaveraInvoicesComponent } from './view-client-primavera-invoices/view-client-primavera-invoices.component';
import { ViewClientPrimaveraOrdersComponent } from './view-client-primavera-orders/view-client-primavera-orders.component';

@Component({
    selector: 'app-ratings-table',
    templateUrl: './ratings-table.component.html',
    styleUrls: ['./ratings-table.component.scss'],
    encapsulation: ViewEncapsulation.None,
})
export class RatingsTableComponent implements OnInit, OnChanges {
    @Input() clients: Client[] = [];
    @Input() isLoading: boolean = true;
    @Output() refreshRequested = new EventEmitter<void>();

    isLoadingRatingTypes: boolean = true;
    ratingTypes: RatingType[] = [];
    clientRatingsMap: Map<string, Map<number, ClientRatingDTO>> = new Map();
    maxPossibleScore: number = 0;

    isSyncingRatings: SyncingRatingStates = {
        credit: false,
        paymentCompliance: false,
        historicalVolume: false,
    };

    constructor(
        private _clientsService: ClientsService,
        private _transloco: TranslocoService,
        private _matDialog: MatDialog,
        private _changeDetectorRef: ChangeDetectorRef,
        private sanitizer: DomSanitizer,
        private _flashMessageService: FlashMessageService,
        private _primaveraSyncingService: PrimaveraSyncingService
    ) {}

    ngOnInit(): void {
        this.isLoadingRatingTypes = true;
        this._clientsService.getClientRatingTypes().subscribe((response) => {
            this.ratingTypes = response.rating_types;

            // sum the weights of the ratings to get the max possible score
            this.maxPossibleScore =
                this.ratingTypes.reduce(
                    (accumulator, current) => accumulator + current.weight,
                    0
                ) * 100;

            this.isLoadingRatingTypes = false;
        });
    }

    ngOnChanges(changes: SimpleChanges): void {
        if (!changes['clients'] || !this.clients || this.clients.length <= 0) {
            return;
        }

        this.populateClientRatingsMap();
    }

    /*
     *               PUBLIC METHODS
     */

    populateClientRatingsMap(): void {
        this.clients.forEach((client) => {
            if (!client.ratings || client.ratings.length <= 0) {
                return;
            }

            const ratingsMap: Map<number, ClientRatingDTO> = new Map();

            client.ratings.forEach((rating) => {
                ratingsMap.set(rating.rating_type.id, rating);

                // This makes it possible to filter and sort each rating on the table
                // By assigning a new field to the client object with the rating id
                client[`rating_${rating.rating_type.id}`] =
                    rating.rating_discount.rating || null;
            });

            this.clientRatingsMap.set(client.code, ratingsMap);
        });

    }

    // Method to retrieve the rating for a specific client and rating type id
    getRatingByClientCodeAndType(
        clientCode: string,
        ratingTypeId: number
    ): ClientRatingDTO | undefined {
        const clientRatings = this.clientRatingsMap.get(clientCode);
        return clientRatings ? clientRatings.get(ratingTypeId) : undefined;
    }

    editRating(client: Client, ratingSlug: RatingSlugs): void {
        // Find the rating type based on the slug
        const ratingType = this.getRatingTypeBySlug(ratingSlug);

        if (!ratingType) {
            console.error('Rating type not found');
            return;
        }

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
                ratingType,
            },
        };

        const dialogRef = this._matDialog.open(
            EditClientRatingComponent,
            dialogConfig
        );

        dialogRef.afterClosed().subscribe((result) => {
            if (result) {
                this._flashMessageService.success('edit-client-rating-success');

                this.refreshRequested.emit();
            }
        });
    }

    editAllRatings(client: Client): void {
        // Find the rating type based on the slug

        // open the dialog to edit the segment
        const dialogConfig: MatDialogConfig = {
            maxHeight: '80vh',
            minHeight: '200px',
            height: 'auto',
            maxWidth: '80vw',
            minWidth: '500px',
            width: 'auto',
            data: {
                client,
                ratings: client.ratings,
            },
        };

        const dialogRef = this._matDialog.open(
            EditAllClientRatingsComponent,
            dialogConfig
        );

        dialogRef.afterClosed().subscribe((result) => {
            if (result) {
                this._flashMessageService.success('edit-client-rating-success');

                this.refreshRequested.emit();
            }
        });
    }

    async syncAllRatings(): Promise<void> {
        try {
            await Promise.all([
                this.syncRating('credit'),
                this.syncRating('paymentCompliance'),
                this.syncRating('historicalVolume'),
            ]);

            this.refreshRequested.emit();
        } catch (error) {
            console.error('Error syncing ratings', error);
        }
    }

    async syncRating(
        type: 'credit' | 'paymentCompliance' | 'historicalVolume'
    ): Promise<SyncPrimaveraStatsResponse | null> {
        // Set syncing state
        this.isSyncingRatings[type] = true;

        // Define a mapping of types to service methods
        const syncMethods = {
            credit: this._primaveraSyncingService.syncRatingCredits(),
            paymentCompliance:
                this._primaveraSyncingService.syncRatingPaymentCompliance(),
            historicalVolume:
                this._primaveraSyncingService.syncHistoricalVolumeRating(),
        };

        // Define a mapping for translations
        const translationKeys = {
            credit: {
                success: 'sync-rating-credit-primavera-success',
                error: 'sync-rating-credit-primavera-error',
                noSync: 'primavera-credit-rating-already-up-to-date',
            },
            paymentCompliance: {
                success: 'sync-rating-payment-compliance-primavera-success',
                error: 'sync-rating-payment-compliance-primavera-error',
                noSync: 'primavera-payment-compliance-rating-already-up-to-date',
            },
            historicalVolume: {
                success: 'sync-rating-historical-volume-primavera-success',
                error: 'sync-rating-historical-volume-primavera-error',
                noSync: 'primavera-historical-volume-rating-already-up-to-date',
            },
        };

        try {
            // Execute the correct sync method
            const response: SyncPrimaveraStatsResponse = await syncMethods[
                type
            ].toPromise();

            if (response.result_code <= 0) {
                this._flashMessageService.error(translationKeys[type].error);
                return null;
            }

            if (response.statistics.total_syncs > 0) {
                const message =
                    response.statistics.total_syncs +
                    ' ' +
                    this._transloco.translate(translationKeys[type].success) +
                    ' ' +
                    this._transloco.translate('in') +
                    ' ' +
                    response.statistics.time_elapsed_ms +
                    ' ms';
                this._flashMessageService.ntsuccess(message);
            } else {
                this._flashMessageService.info(translationKeys[type].noSync);
            }

            return response;
        } catch (error) {
            this._flashMessageService.error(translationKeys[type].error);
            return null;
        } finally {
            this.isSyncingRatings[type] = false;
        }
    }

    viewClientPastOrders(client: Client): void {
        const dialogConfig: MatDialogConfig = {
            maxHeight: '80vh',
            minHeight: '200px',
            height: 'auto',
            maxWidth: '80vw',
            minWidth: '60vw',
            width: 'auto',
            data: {
                client,
            },
        };

        const dialogRef = this._matDialog.open(
            ViewClientPrimaveraOrdersComponent,
            dialogConfig
        );

        dialogRef.afterClosed().subscribe((result) => {
            if (result) {
                // no action to do inside
            }
        });
    }

    viewClientInvoices(client: Client): void {
        const dialogConfig: MatDialogConfig = {
            panelClass: 'custom-dialog',
            maxHeight: '80vh',
            minHeight: '200px',
            height: 'auto',
            maxWidth: '80vw',
            minWidth: '60vw',
            width: 'auto',
            data: {
                client,
            },
        };

        const dialogRef = this._matDialog.open(
            ViewClientPrimaveraInvoicesComponent,
            dialogConfig
        );

        dialogRef.afterClosed().subscribe((result) => {
            if (result) {
                // no action to do inside
            }
        });
    }

    getRatingTypeBySlug(slug: RatingSlugs): RatingType | undefined {
        return this.ratingTypes.find((rating) => rating.slug === slug);
    }

    get isAnyRatingLoading(): boolean {
        return (
            this.isSyncingRatings.credit ||
            this.isSyncingRatings.paymentCompliance ||
            this.isSyncingRatings.historicalVolume
        );
    }

    public getRatingTooltip(code: string, ratingTypeId: number): string {
        const rating: ClientRatingDTO = this.getRatingByClientCodeAndType(
            code,
            ratingTypeId
        );

        if (rating.updated_at === null) {
            return this._transloco.translate('rating-not-updated');
        }

        const formatedDateTime = new Date(rating.updated_at).toLocaleString(
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

        let label: string = '';

        if (rating.rating_valid_until) {
            const formatedValidUntil = new Date(
                rating.rating_valid_until
            ).toLocaleString('pt-PT', {
                day: '2-digit',
                month: '2-digit',
                year: 'numeric',
            });

            label +=
                this._transloco.translate('rating-valid-until') +
                ' ' +
                formatedValidUntil +
                ' \n';
        }

        label +=
            this._transloco.translate('rating-updated-at') +
            ' ' +
            formatedDateTime +
            ' ' +
            this._transloco.translate('by') +
            ' ' +
            rating.updated_by;

        return label;
    }

    public getRatingColor(clientCode: string, ratingTypeId: number): SafeStyle {
        const rating: ClientRatingDTO = this.getRatingByClientCodeAndType(
            clientCode,
            ratingTypeId
        );

        let color = 'transparent';

        if (rating.updated_at === null) {
            color = '#fbd38d';
        }

        return this.sanitizer.bypassSecurityTrustStyle(color);
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

    public isSmallScreen(): boolean {
        return window.innerWidth < 768;
    }
}
