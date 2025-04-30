/* eslint-disable arrow-parens */
import {
    ChangeDetectorRef,
    Component,
    Inject,
    OnInit,
} from '@angular/core';
import { FormGroup, FormBuilder, Validators } from '@angular/forms';
import { MatDialogRef, MAT_DIALOG_DATA } from '@angular/material/dialog';
import { TranslocoService } from '@ngneat/transloco';
import { ClientsService } from '../../clients.service';
import {
    Client,
    RatingDiscount,
    RatingType,
} from '../../clients.types';
import { ChangeClientSegmentComponent } from '../../segments-table/change-client-segment/change-client-segment.component';

@Component({
    selector: 'app-edit-client-rating',
    templateUrl: './edit-client-rating.component.html',
    styleUrls: ['./edit-client-rating.component.scss'],
})
export class EditClientRatingComponent implements OnInit {
    isLoading = false;
    client: Client;
    ratingType: RatingType;
    ratings: RatingDiscount[] = [];
    ratingForm: FormGroup;

    flashMessage: 'success' | 'error' | null = null;
    flashMessageText: string;

    constructor(
        private _clientsService: ClientsService,
        private _fb: FormBuilder,
        private _changeDetectorRef: ChangeDetectorRef,
        private _transloco: TranslocoService,
        private dialogRef: MatDialogRef<ChangeClientSegmentComponent>,
        @Inject(MAT_DIALOG_DATA)
        public data: { client: Client; ratingType: RatingType }
    ) {
        this.client = data.client;
        this.ratingType = data.ratingType;
    }

    ngOnInit(): void {
        this.isLoading = true;

        this._clientsService.getRatingDiscounts().subscribe((response) => {
            this.isLoading = false;
            this.ratings = response.rating_discounts;

            const currentRatingDiscount = this.client.ratings.find(
                (rating) => rating.rating_type.id === this.ratingType.id
            ).rating_discount.rating;

            this.ratingForm = this._fb.group({
                rating: [currentRatingDiscount, Validators.required],
            });
        });
    }

    /**
     * Show flash message
     */
    showFlashMessage(type: 'success' | 'error', textMsg: string): void {
        // Show the message
        this.flashMessage = type;
        this.flashMessageText = textMsg;

        // Mark for check
        this._changeDetectorRef.markForCheck();

        // Hide it after 3 seconds
        setTimeout(() => {
            this.flashMessage = null;
            this.flashMessageText = null;

            // Mark for check
            this._changeDetectorRef.markForCheck();
        }, 5000);
    }

    close(): void {
        this.dialogRef.close();
    }

    save(): void {
        if (this.ratingForm.invalid) {
            return;
        }

        this.isLoading = true;

        this._clientsService
            .patchRating(
                this.client.code,
                this.ratingType.id,
                this.ratingForm.value.rating
            )
            .subscribe(
                (response) => {
                    this.isLoading = false;

                    if (response.result_code <= 0) {
                        this.showFlashMessage(
                            'error',
                            this._transloco.translate(
                                'edit-client-rating-error'
                            )
                        );
                        return;
                    }

                    this.dialogRef.close(true);
                },
                (error) => {
                    this.isLoading = false;
                    this.showFlashMessage(
                        'error',
                        this._transloco.translate('edit-client-rating-error')
                    );
                }
            );
    }

    getTitle(): string {
        const baseTitle = this._transloco.translate('client-edit-rating');
        const ratingName = this._transloco.translate(
            'Ratings.' + this.ratingType.slug
        );

        return `${baseTitle} - ${ratingName}`;
    }
}
