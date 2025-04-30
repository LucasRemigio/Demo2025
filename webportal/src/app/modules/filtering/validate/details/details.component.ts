import {
    ChangeDetectionStrategy,
    ChangeDetectorRef,
    Component,
    ElementRef,
    Inject,
    OnDestroy,
    OnInit,
    ViewChild,
    ViewEncapsulation,
} from '@angular/core';
import { DatePipe, DOCUMENT } from '@angular/common';
import { MatTabGroup } from '@angular/material/tabs';
import { Subject } from 'rxjs';
import { takeUntil } from 'rxjs/operators';
import { FuseMediaWatcherService } from '@fuse/services/media-watcher';
import { BreakpointObserver } from '@angular/cdk/layout';
import { CardCategory } from 'app/modules/orders/order.types';
import { FilteringService } from 'app/modules/filtering/filtering.service';
import { FormArray, FormBuilder, FormGroup, Validators } from '@angular/forms';
import { MatDialog, MatDialogConfig } from '@angular/material/dialog';
import { ActivatedRoute, Router } from '@angular/router';
import { TranslocoService } from '@ngneat/transloco';
import { FuseSplashScreenService } from '@fuse/services/splash-screen';
import { FuseConfirmationService } from '@fuse/services/confirmation';
import { Attachments, Category } from 'app/modules/common/common.types';
import { waitForAsync } from '@angular/core/testing';
import { HttpStatusCode } from '@angular/common/http';
import { stat } from 'fs';
import { result } from 'lodash';
import { DomSanitizer, SafeHtml } from '@angular/platform-browser';
import {
    FilteredEmail,
    FilteredEmailDTOResponse,
} from 'app/shared/components/filtering-validate/details/details.types';
import { FlashMessageService } from 'app/shared/components/flash-message/flash-message.service';
import { EmailAttachment, EMAIL_CATEGORIES } from '../../filtering.types';

@Component({
    selector: 'app-validate-details',
    templateUrl: './details.component.html',
    styleUrls: ['./details.component.scss'],
    encapsulation: ViewEncapsulation.None,
    changeDetection: ChangeDetectionStrategy.OnPush,
    providers: [DatePipe],
})
export class ValidateCategoryDetailsComponent implements OnInit, OnDestroy {
    flashMessage: 'success' | 'error' | null = null;
    flashMessageText: string;
    categories: Category[];
    filtered: FilteredEmail;
    attachments: EmailAttachment[];
    currentStep: number = 0;
    drawerMode: 'over' | 'side' = 'side';
    drawerOpened: boolean = true;

    totalSteps: number = 3;
    steps = [
        {
            order: 0,
            title: this.translate('email'),
            subtitle: this.translate('preview-email'),
        },
        {
            order: 1,
            title: this.translate('attachments'),
            subtitle: this.translate('preview-attachments'),
        },
        {
            order: 2,
            title: this.translate('Order.category'),
            subtitle: this.translate('Filtering.categorize-email'),
        },
    ];
    shwoAllButtons: boolean = true;
    disabled: boolean = false;
    selectedCategory: string;
    ready: number = 0;

    composeForm: FormGroup = new FormGroup({});

    sanitizedEmailBody: SafeHtml;

    sum: number = 0;
    _formFieldService: any;
    isLoading: boolean;

    private _fuseConfirmationService: FuseConfirmationService;
    private _operatorService: any;

    private _unsubscribeAll: Subject<any> = new Subject<any>();

    /**
     * Constructor
     */
    constructor(
        @Inject(DOCUMENT) private _document: Document,
        private _filteringService: FilteringService,
        private _changeDetectorRef: ChangeDetectorRef,
        private _fuseMediaWatcherService: FuseMediaWatcherService,
        private _router: Router,
        private readonly translocoService: TranslocoService,
        private _fuseSplashScreenService: FuseSplashScreenService,
        private _route: ActivatedRoute,
        private _flashMessage: FlashMessageService
    ) {}

    translate(key: string, params?: object): string {
        return this.translocoService.translate(key, params || {});
    }

    // -----------------------------------------------------------------------------------------------------
    // @ Lifecycle hooks
    // -----------------------------------------------------------------------------------------------------

    /**
     * On init
     */
    ngOnInit(): void {
        this.ready = 0;

        // Get the email
        this._filteringService
            .getFilteredEmailForCategorization(
                this._route.snapshot.paramMap.get('id')
            )
            .subscribe((data: FilteredEmailDTOResponse) => {
                if (data.result_code <= 0) {
                    this._flashMessage.error('error-loading-list');
                    return;
                }
                // Get the email
                this.filtered = data.filteredEmail;
                // format cc and bcc

                this.ready++;
                this._changeDetectorRef.markForCheck();
                this.attachments = data.emailAttachments;

                // Get the categories
                this.fetchCategories();

                // Go to step
                this.goToStep(0);

                // Mark for check
                this._changeDetectorRef.markForCheck();
            });

        // Subscribe to media changes
        this._fuseMediaWatcherService.onMediaChange$
            .pipe(takeUntil(this._unsubscribeAll))
            .subscribe(({ matchingAliases }) => {
                // Set the drawerMode and drawerOpened
                if (matchingAliases.includes('lg')) {
                    this.drawerMode = 'side';
                    this.drawerOpened = true;
                } else {
                    this.drawerMode = 'over';
                    this.drawerOpened = false;
                }

                // Mark for check
                this._changeDetectorRef.markForCheck();
            });

        this._fuseSplashScreenService.hide();
    }

    fetchCategories(): void {
        this._filteringService
            .getCategories()
            .pipe(takeUntil(this._unsubscribeAll))
            .subscribe((categories: Category[]) => {
                // Remove the category error from the list
                const index = categories.findIndex(
                    (c) => c.title === EMAIL_CATEGORIES.ERRO.title
                );
                if (index > -1) {
                    categories.splice(index, 1);
                }

                // Get the categories
                this.categories = categories;

                this.ready++;

                // Mark for check
                this._changeDetectorRef.markForCheck();
            });
    }

    saveAndClose(): void {}

    getCategoryClass(slug: string): string {
        switch (slug) {
            case 'confianca':
                return 'text-blue-800 bg-blue-100 dark:text-blue-50 dark:bg-blue-500';
            case 'Error':
                return 'text-red-800 bg-red-100 dark:text-red-50 dark:bg-red-500';
            default:
                return '';
        }
    }
    /**
     * On destroy
     */
    ngOnDestroy(): void {
        // Unsubscribe from all subscriptions
        this._unsubscribeAll.next();
        this._unsubscribeAll.complete();
    }

    // -----------------------------------------------------------------------------------------------------
    // @ Public methods
    // -----------------------------------------------------------------------------------------------------

    onSelectedAddressChange(add: any): void {}

    /**
     * Go to given step
     *
     * @param step
     */
    goToStep(step: number): void {
        // Set the current step
        this.currentStep = step;

        // Go to the step
        // this.librarySteps.selectedIndex = this.currentStep;

        // Mark for check
        this._changeDetectorRef.markForCheck();
    }

    /**
     * Go to previous step
     */
    goToPreviousStep(): void {
        // Return if we already on the first step
        if (this.currentStep === 0) {
            return;
        }

        // Go to step
        this.goToStep(this.currentStep - 1);

        // Scroll the current step selector from sidenav into view
        this._scrollCurrentStepElementIntoView();
    }

    /**
     * Go to next step
     */
    goToNextStep(): void {
        // Return if we already on the last step
        if (this.currentStep === this.totalSteps - 1) {
            return;
        }

        // Go to step
        this.goToStep(this.currentStep + 1);

        // Scroll the current step selector from sidenav into view
        this._scrollCurrentStepElementIntoView();
    }

    trackByFn(index: number, item: any): any {
        return item.id || index;
    }

    categorize(category: string): void {
        if (this.selectedCategory) {
            this._filteringService
                .categorizeEmail(this.filtered.email.id, this.selectedCategory)
                .subscribe(
                    (response) => {
                        this._flashMessage.success('email-categorized-success');

                        this._router.navigate(['/filtering/validate']);
                    },
                    (error) => {
                        this._flashMessage.error('email-categorized-error');
                    }
                );
        } else {
            this._flashMessage.error('email-category-mandatory');
        }
    }

    // -----------------------------------------------------------------------------------------------------
    // @ Private methods
    // -----------------------------------------------------------------------------------------------------

    /**
     * Scrolls the current step element from
     * sidenav into the view. This only happens when
     * previous/next buttons pressed as we don't want
     * to change the scroll position of the sidebar
     * when the user actually clicks around the sidebar.
     *
     * @private
     */
    private _scrollCurrentStepElementIntoView(): void {
        // Wrap everything into setTimeout so we can make sure that the 'current-step' class points to correct element
        setTimeout(() => {
            // Get the current step element and scroll it into view
            const currentStepElement =
                this._document.getElementsByClassName('current-step')[0];
            if (currentStepElement) {
                currentStepElement.scrollIntoView({
                    behavior: 'smooth',
                    block: 'start',
                });
            }
        });
    }
}
