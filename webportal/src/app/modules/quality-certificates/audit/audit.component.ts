import { Component, OnInit } from '@angular/core';
import { EMAIL_CATEGORIES } from 'app/modules/filtering/filtering.types';

@Component({
    selector: 'app-qualitycertificate-audit',
    templateUrl: './audit.component.html',
    styleUrls: ['./audit.component.scss'],
})
export class AuditComponent implements OnInit {
    categoryList = EMAIL_CATEGORIES;
    constructor() {}

    ngOnInit(): void {}
}
