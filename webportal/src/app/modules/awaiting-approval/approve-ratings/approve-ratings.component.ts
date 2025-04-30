import { Component, OnInit } from '@angular/core';
import { EMAIL_CATEGORIES, EMAIL_STATUSES } from 'app/modules/filtering/filtering.types';

@Component({
    selector: 'app-approve-ratings',
    templateUrl: './approve-ratings.component.html',
    styleUrls: ['./approve-ratings.component.scss'],
})
export class ApproveRatingsComponent implements OnInit {
    statusList = EMAIL_STATUSES;
    categoryList = EMAIL_CATEGORIES;
    constructor() {}

    ngOnInit(): void {}
}
