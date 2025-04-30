import { Component, OnInit } from '@angular/core';
import { ActivatedRoute } from '@angular/router';

@Component({
    selector: 'app-confirmation',
    templateUrl: './confirmation.component.html',
    styleUrls: ['./confirmation.component.scss'],
})
export class ValidateConfirmationComponent implements OnInit {
    token: string;
    constructor(private route: ActivatedRoute) {}

    ngOnInit(): void {
        // Get it from url
        this.token = this.route.snapshot.paramMap.get('token') || '';

        // get the products
        // show them on the table
    }
}
