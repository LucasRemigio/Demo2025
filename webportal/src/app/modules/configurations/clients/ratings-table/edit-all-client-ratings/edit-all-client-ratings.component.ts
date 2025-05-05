/* eslint-disable arrow-parens */
/* eslint-disable @typescript-eslint/naming-convention */
import { ChangeDetectorRef, Component, Inject, OnInit } from '@angular/core';
import {
    FormGroup,
    FormBuilder,
    Validators,
    FormArray,
    AbstractControl,
} from '@angular/forms';
import { MatDialogRef, MAT_DIALOG_DATA } from '@angular/material/dialog';
import { TranslocoService } from '@ngneat/transloco';
import { ClientsService } from '../../clients.service';
import {
    RatingType,
    RatingDiscount,
    Client,
    ClientRatingDTO,
    UpdateClientRatingsRequest,
} from '../../clients.types';
import { ChangeClientSegmentComponent } from '../../segments-table/change-client-segment/change-client-segment.component';

@Component({
    selector: 'app-edit-all-client-ratings',
    templateUrl: './edit-all-client-ratings.component.html',
    styleUrls: ['./edit-all-client-ratings.component.scss'],
})
export class EditAllClientRatingsComponent implements OnInit {
    isLoading = false;
    client: Client;
    ratings: ClientRatingDTO[] = [];
    ratingDiscounts: RatingDiscount[] = [];
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
        public data: { client: Client; ratings: ClientRatingDTO[] }
    ) {
        this.client = data.client;
        this.ratings = data.ratings;
    }

    ngOnInit(): void {
        this.isLoading = true;
        this._clientsService.getRatingDiscounts().subscribe((response) => {
            this.isLoading = false;
            this.ratingDiscounts = response.rating_discounts;
        });

        // Initialize form with a global valid until date field
        this.ratingForm = this._fb.group({
            global_valid_until: [
                this.getDefaultValidUntil(),
                Validators.required,
            ],
            ratings: this._fb.array([]),
        });

        this.initRatingForm();
    }

    /**
     * Initialize form with all ratings from input
     */
    initRatingForm(): void {
        // Add each rating to the form array
        this.ratings.forEach((rating) => {
            this.addRatingToForm(rating);
        });
    }

    /**
     * Add a single rating to the form array
     */
    addRatingToForm(rating: ClientRatingDTO): void {
        // Get the current rating discount value
        const ratingGroup = this._fb.group({
            rating_type_id: [rating.rating_type.id, Validators.required],
            rating_type_name: [rating.rating_type.slug],
            rating: [rating.rating_discount?.rating || '', Validators.required],
            rating_valid_until: [
                this.ratingForm.get('global_valid_until').value,
            ],
            current_rating: [rating.rating_discount?.rating || ''],
            recommended_rating: [
                rating.recommended_rating_discount?.rating || '',
            ],
            selected: [false],
        });

        // Add to form array
        this.ratingsFormArray.push(ratingGroup);
    }

    /**
     * Get default valid until date (1 year from now)
     */
    getDefaultValidUntil(): Date {
        const date = new Date();
        date.setFullYear(date.getFullYear() + 1);
        return date;
    }

    /**
     * Get the ratings form array
     */
    get ratingsFormArray(): FormArray {
        return this.ratingForm.get('ratings') as FormArray;
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

    /**
     * Format date for API
     */
    formatDate(date: Date): string {
        if (!date) {
            return '';
        }
        const d = new Date(date);
        let month = '' + (d.getMonth() + 1);
        let day = '' + d.getDate();
        const year = d.getFullYear();

        if (month.length < 2) {
            month = '0' + month;
        }
        if (day.length < 2) {
            day = '0' + day;
        }

        return [year, month, day].join('-');
    }

    /**
     * When the global date changes, update all individual rating dates
     */
    onGlobalDateChange(event: any): void {
        const newDate = event.value;

        // Update all rating valid_until dates
        this.ratingsFormArray.controls.forEach((control) => {
            control.get('rating_valid_until').setValue(newDate);
        });
    }

    /**
     * Save all ratings with the global valid_until date
     */
    save(): void {
        if (this.ratingForm.invalid) {
            // Mark all controls as touched to show validation errors
            this.markFormGroupTouched(this.ratingForm);
            return;
        }

        this.isLoading = true;

        // Get the global valid until date
        const globalValidUntil = this.formatDate(
            this.ratingForm.get('global_valid_until').value
        );

        // Create update request from form data - only include selected ratings
        const updateRequest: UpdateClientRatingsRequest = {
            ratings: this.ratingsFormArray.controls
                .filter((control) => control.get('selected').value) // Only include selected ratings
                .map((control) => ({
                    rating_type_id: control.get('rating_type_id').value,
                    rating: control.get('rating').value,
                    rating_valid_until: globalValidUntil, // Use the same date for all ratings
                })),
        };

        // Call the API
        this._clientsService
            .updateAllClientRatings(this.client.code, updateRequest)
            .subscribe(
                (response) => {
                    this.dialogRef.close(true);
                },
                (error) => {
                    console.error('Error updating ratings:', error);
                    this.showFlashMessage(
                        'error',
                        this._transloco.translate('ratings-update-error')
                    );
                },
                () => {
                    this.isLoading = false;
                }
            );
    }

    get allRatingsSelected(): boolean {
        return this.ratingsFormArray.controls.every(
            (control) => control.get('selected').value
        );
    }

    get someRatingsSelected(): boolean {
        return this.ratingsFormArray.controls.some(
            (control) => control.get('selected').value
        );
    }

    toggleAllRatings(): void {
        const allSelected = this.allRatingsSelected;
        this.ratingsFormArray.controls.forEach((control) => {
            control.get('selected').setValue(!allSelected);
        });
    }

    /**
     * Automatically select the row checkbox when a rating is chosen
     */
    onRatingSelected(ratingControl: AbstractControl): void {
        // Set the 'selected' control to true
        ratingControl.get('selected').setValue(true);
    }

    /**
     * Get color based on rating letter - improved contrast version
     */
    getRatingColor(value: string): string {
        if (!value) {
            return '';
        }

        // Color mapping for letter ratings with better contrast
        switch (value.toUpperCase()) {
            case 'A':
                return '#166534'; // Green-800 - darker green for better contrast
            case 'B':
                return '#3f6212'; // Lime-800 - darker lime green
            case 'C':
                return '#854d0e'; // Amber-800 - darker amber for better contrast than yellow
            case 'D':
                return '#9a3412'; // Orange-800 - darker orange
            default:
                return '#b91c1c'; // Red-700 - strong red with good contrast
        }
    }

    /**
     * Get background color based on rating letter
     */
    getRatingBgColor(value: string): string {
        if (!value) {
            return 'rgb(243, 244, 246)'; // gray-100
        }

        // Color mapping with better contrast
        switch (value.toUpperCase()) {
            case 'A':
                return 'rgb(220, 252, 231)'; // green-100
            case 'B':
                return 'rgb(236, 252, 203)'; // lime-100
            case 'C':
                return 'rgb(254, 243, 199)'; // amber-100 - slightly adjusted for better contrast
            case 'D':
                return 'rgb(255, 237, 213)'; // orange-100
            default:
                return 'rgb(254, 226, 226)'; // red-100
        }
    }

    /**
     * Get Tailwind classes for rating with good contrast
     */
    getRatingClasses(value: string): string {
        if (!value) {
            return 'bg-gray-100 text-gray-600';
        }

        switch (value.toUpperCase()) {
            case 'A':
                return 'bg-green-100 text-green-800 border border-green-300'; // Green
            case 'B':
                return 'bg-lime-100 text-lime-800 border border-lime-300'; // Lime
            case 'C':
                return 'bg-amber-100 text-amber-800 border border-amber-300'; // Amber
            case 'D':
                return 'bg-indigo-100 text-indigo-800 border border-indigo-300'; // Orange
            default:
                return 'bg-red-100 text-red-700 border border-red-300'; // Red
        }
    }

    /**
     * Mark all controls in a form group as touched
     */
    private markFormGroupTouched(formGroup: FormGroup | FormArray): void {
        Object.keys(formGroup.controls).forEach((key) => {
            const control = formGroup.get(key);
            if (control instanceof FormGroup || control instanceof FormArray) {
                this.markFormGroupTouched(control);
            } else {
                control.markAsTouched();
            }
        });
    }
}
