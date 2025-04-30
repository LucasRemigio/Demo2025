import { Component, OnInit } from '@angular/core';
import { EMAIL_CATEGORIES } from '../filtering.types';

@Component({
    selector: 'app-duplicates',
    templateUrl: './duplicates.component.html',
    styleUrls: ['./duplicates.component.scss'],
})
export class DuplicatesComponent implements OnInit {
    categoryList = EMAIL_CATEGORIES;

    constructor() {}

    ngOnInit(): void {}
}
