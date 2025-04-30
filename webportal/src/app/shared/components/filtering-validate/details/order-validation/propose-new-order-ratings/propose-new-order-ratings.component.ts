/* eslint-disable @typescript-eslint/naming-convention */
/* eslint-disable arrow-parens */
import { Component, Inject, OnInit } from '@angular/core';
import { FormBuilder, FormGroup } from '@angular/forms';
import { MatDialogRef, MAT_DIALOG_DATA } from '@angular/material/dialog';
import { TranslocoService } from '@ngneat/transloco';
import { ClientsService } from 'app/modules/configurations/clients/clients.service';
import {
    ClientRatingDTO,
    RatingDiscount,
} from 'app/modules/configurations/clients/clients.types';
import { GenericResponse } from 'app/modules/configurations/products/products.types';
import { OrderService } from 'app/shared/components/confirm-order-address/order.service';
import { OrderRatingDTO, ProposeNewRating } from '../../details.types';

@Component({
    selector: 'app-propose-new-order-ratings',
    templateUrl: './propose-new-order-ratings.component.html',
    styleUrls: ['./propose-new-order-ratings.component.scss'],
})
export class ProposeNewOrderRatingsComponent implements OnInit {
    isLoading: boolean = false;
    ratings: OrderRatingDTO[] = [];
    clientRatings: ClientRatingDTO[] = [];
    ratingDiscounts: RatingDiscount[] = [];
    ratingFormGroup: FormGroup;

    flashMessage: 'success' | 'error' | null = null;
    flashMessageText: string;

    ratingsByType: Map<number, string> = new Map();

    constructor(
        private _matDialogRef: MatDialogRef<ProposeNewOrderRatingsComponent>,
        private _formBuilder: FormBuilder,
        private _transloco: TranslocoService,
        private _clientsService: ClientsService,
        private _orderService: OrderService,
        @Inject(MAT_DIALOG_DATA)
        public data: {
            orderRatings: OrderRatingDTO[];
            clientRatings: ClientRatingDTO[];
        }
    ) {
        this.ratings = data.orderRatings;
        this.clientRatings = data.clientRatings;
    }

    ngOnInit(): void {
        this.isLoading = true;

        this._clientsService.getRatingDiscounts().subscribe(
            (response) => {
                this.ratingDiscounts = response.rating_discounts;
                this.createForm();
            },
            (error) => {
                this.showFlashMessage('error', error.error.message);
            },
            () => {
                this.isLoading = false;
            }
        );

        this.ratings.forEach((rating) => {
            this.ratingsByType.set(
                rating.rating_type.id,
                rating.rating_discount.rating
            );
        });

        this.clientRatings.forEach((rating) => {
            this.ratingsByType.set(
                rating.rating_type.id,
                rating.rating_discount.rating
            );
        });
    }

    showFlashMessage(type: 'success' | 'error', text: string): void {
        this.flashMessage = type;
        this.flashMessageText = text;

        setTimeout(() => {
            this.flashMessage = null;
        }, 5000);
    }

    /* ==============================================================================
     *   PUBLIC METHODS
     * ==============================================================================
     */

    createForm(): void {
        this.ratingFormGroup = this._formBuilder.group({
            ratings: this._formBuilder.array(
                this.ratings.map((rating) =>
                    this._formBuilder.group({
                        name: [
                            this._transloco.translate(
                                'Ratings.' + rating.rating_type.slug
                            ),
                        ],
                        newRating: [rating.rating_discount.rating],
                        ratingTypeId: [rating.rating_type.id],
                    })
                )
            ),
            clientRatings: this._formBuilder.array(
                this.clientRatings.map((rating) =>
                    this._formBuilder.group({
                        name: [
                            this._transloco.translate(
                                'Ratings.' + rating.rating_type.slug
                            ),
                        ],
                        newRating: [rating.rating_discount.rating],
                        ratingTypeId: [rating.rating_type.id],
                    })
                )
            ),
        });
    }

    currentRatingByType(type: number): string {
        const rating = this.ratingsByType.get(type);

        return rating;
    }

    changedRatings(): ProposeNewRating[] {
        // Map both order and client ratings to the same format
        const mapRating = (rating: {
            ratingTypeId: number;
            newRating: string;
        }): ProposeNewRating => ({
            rating_type_id: rating.ratingTypeId,
            new_rating: rating.newRating,
        });

        const orderRatings = this.ratingFormGroup.value.ratings.map(mapRating);
        const clientRatings =
            this.ratingFormGroup.value.clientRatings.map(mapRating);

        const allRatings = [...orderRatings, ...clientRatings];

        return allRatings.filter((rating: ProposeNewRating) => {
            const originalRating = this.ratingsByType.get(
                rating.rating_type_id
            );

            return originalRating !== rating.new_rating;
        });
    }

    saveChanges(): void {
        if (!this.isFormValid) {
            return;
        }

        this.isLoading = true;
        const orderToken = this.ratings[0].order_token;

        const newRatings = this.changedRatings();

        this.sendRatingsRequest(newRatings, orderToken);
    }

    sendRatingsRequest(
        newRatings: ProposeNewRating[],
        orderToken: string
    ): void {
        this._orderService.proposeNewRatings(newRatings, orderToken).subscribe(
            (response) => {
                if (response.result_code <= 0) {
                    this.handleRatingError(response);
                    return;
                }

                // close dialog with boolean value to indicate success
                this._matDialogRef.close(true);
            },
            (error) => {
                this.showFlashMessage('error', error.message);
            },
            () => {
                this.isLoading = false;
            }
        );
    }

    handleRatingError(response: GenericResponse): void {
        let message: string;
        // item already exists
        if (response.result_code === -74) {
            message = 'propose-new-ratings-already-exists';
        } else {
            message = 'propose-new-ratings-error';
        }

        this.showFlashMessage('error', this._transloco.translate(message));
        return;
    }

    get isFormValid(): boolean {
        // for the form to be valid, there must be at least 1 rating that is different from the current one
        const currentRatings = this.ratings.map(
            (rating) => rating.rating_discount.rating
        );
        const currentClientRatings = this.clientRatings.map(
            (rating) => rating.rating_discount.rating
        );

        const newOrderRatings = this.ratingFormGroup.value.ratings.map(
            (rating: { newRating: string }) => rating.newRating
        );
        const newClientRatings = this.ratingFormGroup.value.clientRatings.map(
            (rating: { newRating: string }) => rating.newRating
        );

        const isAnyRateDifferent = newOrderRatings.some(
            (newRating: string, index: number) =>
                newRating !== currentRatings[index]
        );

        const isAnyClientRateDifferent = newClientRatings.some(
            (newRating: string, index: number) =>
                newRating !== currentClientRatings[index]
        );

        if (!isAnyRateDifferent && !isAnyClientRateDifferent) {
            return false;
        }

        return this.ratingFormGroup.valid && this.ratingFormGroup.dirty;
    }

    close(): void {
        this._matDialogRef.close();
    }
}
