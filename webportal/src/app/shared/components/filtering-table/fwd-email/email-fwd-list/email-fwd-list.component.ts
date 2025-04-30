import { Component, Input, OnInit } from '@angular/core';
import { EmailForward } from 'app/modules/filtering/filtering.types';

@Component({
    selector: 'app-email-fwd-list',
    templateUrl: './email-fwd-list.component.html',
    styleUrls: ['./email-fwd-list.component.scss'],
})
export class EmailFwdListComponent implements OnInit {
    @Input() emailForwardList: EmailForward[];

    constructor() {}

    ngOnInit(): void {}

    closeForwardedEmailInfo(): void {
        this.emailForwardList = null;
    }
}
