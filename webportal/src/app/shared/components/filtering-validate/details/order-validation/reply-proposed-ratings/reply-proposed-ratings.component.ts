/* eslint-disable @typescript-eslint/naming-convention */
/* eslint-disable arrow-parens */
import { Component, Inject, OnChanges, OnInit } from '@angular/core';
import { FormArray, FormBuilder, FormGroup } from '@angular/forms';
import { MatDialogRef, MAT_DIALOG_DATA } from '@angular/material/dialog';
import { TranslocoService } from '@ngneat/transloco';
import { ClientCodeResponse } from 'app/modules/configurations/clients/clients.types';
import { OrderService } from 'app/shared/components/confirm-order-address/order.service';
import {
    OrderRatingChangeRequestDto,
    OrderRatingDTO,
    ReplyProposedRatings,
} from '../../details.types';
import { ProposeNewOrderRatingsComponent } from '../propose-new-order-ratings/propose-new-order-ratings.component';

@Component({
    selector: 'app-reply-proposed-ratings',
    templateUrl: './reply-proposed-ratings.component.html',
    styleUrls: ['./reply-proposed-ratings.component.scss'],
})
export class ReplyProposedRatingsComponent implements OnInit {
    isLoading: boolean = false;
    proposedRatings: OrderRatingChangeRequestDto[] = [];
    orderRatings: OrderRatingDTO[] = [];
    clientRatings: ClientCodeResponse[] = [];
    ratingFormGroup: FormGroup;

    flashMessage: 'success' | 'error' | null = null;
    flashMessageText: string;

    ratingsByType: Map<number, string> = new Map();

    constructor(
        private _matDialogRef: MatDialogRef<ProposeNewOrderRatingsComponent>,
        private _formBuilder: FormBuilder,
        private _transloco: TranslocoService,
        private _orderService: OrderService,
        @Inject(MAT_DIALOG_DATA)
        public data: {
            ratings: OrderRatingChangeRequestDto[];
            orderRatings: OrderRatingDTO[];
            clientRatings: ClientCodeResponse[];
        }
    ) {
        this.proposedRatings = data.ratings;
        this.orderRatings = data.orderRatings;
        this.clientRatings = data.clientRatings;
    }

    ngOnInit(): void {
        this.isLoading = true;

        this.orderRatings.forEach((rating) => {
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

        this.createForm();
        this.isLoading = false;
    }

    showFlashMessage(type: 'success' | 'error', text: string): void {
        this.flashMessage = type;
        this.flashMessageText = text;

        setTimeout(() => {
            this.flashMessage = null;
        }, 5000);
    }

    /** ========================================================================
     *  Public methods
     */

    currentRatingByType(type: number): string {
        const rating = this.ratingsByType.get(type);

        return rating;
    }

    createForm(): void {
        // separate the pending requests in order requests and client requests
        const orderRatingTypes = this.orderRatings.map(
            (rating) => rating.rating_type.id
        );
        const clientRatingTypes = this.clientRatings.map(
            (rating) => rating.rating_type.id
        );

        const clientProposedRatings = this.proposedRatings.filter((rating) =>
            clientRatingTypes.includes(rating.rating_type.id)
        );

        const orderProposedRatings = this.proposedRatings.filter((rating) =>
            orderRatingTypes.includes(rating.rating_type.id)
        );

        this.ratingFormGroup = this._formBuilder.group({
            ratings: this._formBuilder.array(
                orderProposedRatings.map((rating) =>
                    this._formBuilder.group({
                        rating: [rating.new_rating_discount.rating],
                        name: [
                            this._transloco.translate(
                                'Ratings.' + rating.rating_type.slug
                            ),
                        ],
                        ratingId: [rating.rating_type.id],
                        accepted: [null], // this will be updated to either true or false
                    })
                )
            ),
            clientRatings: this._formBuilder.array(
                clientProposedRatings.map((rating) =>
                    this._formBuilder.group({
                        rating: [rating.new_rating_discount.rating],
                        name: [
                            this._transloco.translate(
                                'Ratings.' + rating.rating_type.slug
                            ),
                        ],
                        ratingId: [rating.rating_type.id],
                        accepted: [null], // this will be updated to either true or false
                    })
                )
            ),
        });
    }

    close(): void {
        this._matDialogRef.close();
    }

    get isFormValid(): boolean {
        // for the form to be valid, all the accepted parameters must be filled in
        const ratingsArray = this.ratingFormGroup.get('ratings') as FormArray;
        const clientRatingsArray = this.ratingFormGroup.get(
            'clientRatings'
        ) as FormArray;

        return (
            ratingsArray.controls.every(
                (group: FormGroup) => group.get('accepted').value !== null
            ) &&
            clientRatingsArray.controls.every(
                (group: FormGroup) => group.get('accepted').value !== null
            )
        );
    }

    get isAnyRequestForOrder(): boolean {
        return this.orderRatings.some((rating) =>
            this.proposedRatings.some(
                (proposedRating) =>
                    proposedRating.rating_type.id === rating.rating_type.id
            )
        );
    }

    get isAnyRequestForClient(): boolean {
        return this.clientRatings.some((rating) =>
            this.proposedRatings.some(
                (proposedRating) =>
                    proposedRating.rating_type.id === rating.rating_type.id
            )
        );
    }

    getResponses(): ReplyProposedRatings[] {
        const mapRatingToResponse = (rating: {
            ratingId: number;
            accepted: boolean;
        }): ReplyProposedRatings => ({
            rating_type_id: rating.ratingId,
            is_accepted: String(rating.accepted).toLowerCase() === 'true',
        });

        const orderReplies =
            this.ratingFormGroup.value.ratings.map(mapRatingToResponse);
        const clientReplies =
            this.ratingFormGroup.value.clientRatings.map(mapRatingToResponse);

        return [...orderReplies, ...clientReplies];
    }

    saveChanges(): void {
        if (!this.isFormValid) {
            return;
        }

        this.isLoading = true;
        const orderToken = this.proposedRatings[0].order_token;

        const replies = this.getResponses();

        this.sendReplyResponse(replies, orderToken);
    }

    sendReplyResponse(
        replies: ReplyProposedRatings[],
        orderToken: string
    ): void {
        this._orderService.replyProposedRatings(replies, orderToken).subscribe(
            (response) => {
                if (response.result_code <= 0) {
                    this.showFlashMessage('error', response.result);
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
}
