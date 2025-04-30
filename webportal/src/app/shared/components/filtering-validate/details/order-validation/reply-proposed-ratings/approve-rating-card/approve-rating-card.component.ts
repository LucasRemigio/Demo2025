import { Component, Input, OnInit } from '@angular/core';
import { FormGroup } from '@angular/forms';

@Component({
    selector: 'app-approve-rating-card',
    templateUrl: './approve-rating-card.component.html',
    styleUrls: ['./approve-rating-card.component.scss'],
})
export class ApproveRatingCardComponent implements OnInit {
    @Input() ratingGroup: FormGroup;
    @Input() currentRating: string;

    constructor() {}

    ngOnInit(): void {}
}
