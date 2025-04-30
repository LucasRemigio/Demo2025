import { Component, OnInit } from '@angular/core';
import { EMAIL_CATEGORIES } from '../filtering.types';

@Component({
    selector: 'app-spam',
    templateUrl: './spam.component.html',
    styleUrls: ['./spam.component.scss'],
})
export class SpamComponent implements OnInit {
    categoryList = EMAIL_CATEGORIES;

    constructor() {}

    ngOnInit(): void {}
}
