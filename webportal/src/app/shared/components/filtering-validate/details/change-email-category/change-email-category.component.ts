import { Component, Input, OnInit } from '@angular/core';
import { Router, ActivatedRoute } from '@angular/router';
import { Category } from 'app/modules/common/common.types';
import { FilteringService } from 'app/modules/filtering/filtering.service';
import { FlashMessageService } from 'app/shared/components/flash-message/flash-message.service';

@Component({
    selector: 'app-change-email-category',
    templateUrl: './change-email-category.component.html',
    styleUrls: ['./change-email-category.component.scss'],
})
export class ChangeEmailCategoryComponent implements OnInit {
    @Input() emailId: string;
    @Input() categories: Category[];
    selectedCategory: string;

    constructor(
        private _filteringService: FilteringService,
        private _flashMessageService: FlashMessageService,
        private _router: Router,
        private _activatedRoute: ActivatedRoute
    ) {}

    ngOnInit(): void {}

    categorize(): void {
        if (!this.selectedCategory) {
            this._flashMessageService.error('email-category-mandatory');
            return;
        }

        this._filteringService
            .categorizeEmail(this.emailId, this.selectedCategory)
            .subscribe(
                (response) => {
                    if (response.result_code <= 0) {
                        this._flashMessageService.error(
                            'email-categorized-error'
                        );
                        return;
                    }

                    this._flashMessageService.success(
                        'email-categorized-success'
                    );

                    this._router.navigate(['../../validate'], {
                        relativeTo: this._activatedRoute,
                    });
                },
                (error) => {
                    this._flashMessageService.error('email-categorized-error');
                }
            );
    }
}
