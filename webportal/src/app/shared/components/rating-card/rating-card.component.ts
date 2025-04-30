/* eslint-disable arrow-parens */
import { Component, Input, OnInit } from '@angular/core';
import { TranslocoService } from '@ngneat/transloco';
import { BaseRatingDTO } from 'app/modules/configurations/clients/clients.types';
import {
    OrderRatingChangeRequestDto,
    OrderRatingDTO,
} from '../filtering-validate/details/details.types';

@Component({
    selector: 'app-rating-card',
    templateUrl: './rating-card.component.html',
    styleUrls: ['./rating-card.component.scss'],
})
export class RatingCardComponent implements OnInit {
    @Input() ratings: BaseRatingDTO[];
    @Input() ratingRequests: OrderRatingChangeRequestDto[];

    pendingRatingRequests: OrderRatingChangeRequestDto[] = [];
    hasAnyProposedRating: boolean = false;

    constructor(private _transloco: TranslocoService) {}

    ngOnInit(): void {
        if (!this.ratingRequests) {
            return;
        }

        this.pendingRatingRequests = this.ratingRequests.filter(
            (req) => req.status === 'pending'
        );
        this.hasAnyProposedRating = this.ratings.some((rating) =>
            this.getLastPendingRequest(rating.rating_type.id)
        );
    }

    /*
     * Getters
     */

    getLastPendingRequest(ratingTypeId: number): OrderRatingChangeRequestDto {
        if (!this.ratingRequests) {
            return;
        }

        const request = this.pendingRatingRequests.find(
            (req) => req.rating_type.id === ratingTypeId
        );

        if (!request) {
            return;
        }

        return request;
    }

    getLastVerifiedRequest(ratingTypeId: number): OrderRatingChangeRequestDto {
        if (!this.ratingRequests) {
            return;
        }

        const request = this.ratingRequests.find(
            (req) =>
                req.rating_type.id === ratingTypeId &&
                (req.status === 'accepted' || req.status === 'rejected')
        );

        if (!request) {
            return;
        }

        return request;
    }

    /*
     * Mat tooltips
     */

    getWhoVerifiedRating(ratingTypeId: number): string {
        const rating = this.getLastVerifiedRequest(ratingTypeId);
        if (!rating) {
            return;
        }

        const repliedByStart =
            this._transloco.translate('rating-verified-by') +
            ' ' +
            rating.verified_by;

        const formatedDate = new Date(rating.verified_at);

        const repliedAtEnd =
            this._transloco.translate('at') +
            ' ' +
            formatedDate.toLocaleString('pt-PT');

        return repliedByStart + ' ' + repliedAtEnd;
    }

    getWhoRequestedRating(rating: OrderRatingChangeRequestDto): string {
        const proposedByStart =
            this._transloco.translate('new-rating-proposed-by') +
            ' ' +
            rating.requested_by;

        const formatedDate = new Date(rating.requested_at);

        const proposedAtEnd =
            this._transloco.translate('at') +
            ' ' +
            formatedDate.toLocaleString('pt-PT');

        return proposedByStart + ' ' + proposedAtEnd;
    }
}
