import {
    ChangeDetectorRef,
    Component,
    OnDestroy,
    OnInit,
    ViewEncapsulation,
} from '@angular/core';
import { ActivatedRoute, Router } from '@angular/router';
import { Subject } from 'rxjs';
import { takeUntil } from 'rxjs/operators';
import { FuseMediaWatcherService } from '@fuse/services/media-watcher';
import {
    FuseNavigationService,
    FuseVerticalNavigationComponent,
} from '@fuse/components/navigation';
import { Navigation } from 'app/core/navigation/navigation.types';
import { NavigationService } from 'app/core/navigation/navigation.service';
import { User } from 'app/core/user/user.types';
import { UserService } from 'app/core/user/user.service';
import { ShowEmailPopupServiceService } from 'app/layout/common/show-email/show-email-popup-service.service';
import { MatDialog, MatDialogConfig } from '@angular/material/dialog';
import { ShowEmailPopupComponent } from 'app/shared/components/show-email-popup/show-email-popup.component';
import { translate } from '@ngneat/transloco';
import { EmailPopupState } from 'app/shared/components/filtering-validate/details/details.types';
import { SwitchViewService } from 'app/shared/components/filtering-validate/switch-view/switch-view.service';

@Component({
    selector: 'classy-layout',
    templateUrl: './classy.component.html',
    encapsulation: ViewEncapsulation.None,
})
export class ClassyLayoutComponent implements OnInit, OnDestroy {
    isScreenSmall: boolean = true;
    navigation: Navigation;
    user: User;

    showEmailPopup: boolean = false;

    previewEmailLabel: string = translate('preview-email');
    createQuotationLabel: string = translate('quote-create');

    isViewTypeVisible: boolean = false;

    private _unsubscribeAll: Subject<any> = new Subject<any>();

    /**
     * Constructor
     */
    constructor(
        private _navigationService: NavigationService,
        private _userService: UserService,
        private _fuseMediaWatcherService: FuseMediaWatcherService,
        private _fuseNavigationService: FuseNavigationService,
        private _showEmailPopupService: ShowEmailPopupServiceService,
        private _switchViewService: SwitchViewService,
        private _cdr: ChangeDetectorRef
    ) {}

    // -----------------------------------------------------------------------------------------------------
    // @ Accessors
    // -----------------------------------------------------------------------------------------------------

    /**
     * Getter for current year
     */
    get currentYear(): number {
        return new Date().getFullYear();
    }

    // -----------------------------------------------------------------------------------------------------
    // @ Lifecycle hooks
    // -----------------------------------------------------------------------------------------------------

    /**
     * On init
     */
    ngOnInit(): void {
        this.toggleNavigation('mainNavigation');
        // Subscribe to navigation data
        this._navigationService.navigation$
            .pipe(takeUntil(this._unsubscribeAll))
            .subscribe((navigation: Navigation) => {
                this.navigation = navigation;
            });

        // Subscribe to the user service
        this._userService.user$
            .pipe(takeUntil(this._unsubscribeAll))
            .subscribe((user: User) => {
                this.user = user;
            });

        // Subscribe to media changes
        this._fuseMediaWatcherService.onMediaChange$
            .pipe(takeUntil(this._unsubscribeAll))
            .subscribe(({ matchingAliases }) => {
                // Check if the screen is small
                this.isScreenSmall = !matchingAliases.includes('md');
            });

        this._switchViewService.isViewTypeVisible$
            .pipe(takeUntil(this._unsubscribeAll))
            .subscribe((visible) => {
                this.isViewTypeVisible = visible;
                this._cdr.detectChanges();
            });

        this._showEmailPopupService.showEmailButton$
            .pipe(takeUntil(this._unsubscribeAll))
            .subscribe((show) => {
                this.showEmailPopup = show;
                this._cdr.detectChanges();
            });
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

    /**
     * Toggle navigation
     *
     * @param name
     */
    toggleNavigation(name: string): void {
        // Get the navigation
        const navigation =
            this._fuseNavigationService.getComponent<FuseVerticalNavigationComponent>(
                name
            );
        if (navigation) {
            // Toggle the opened status
            navigation.toggle();
        }
    }

    openEmailPopup(): void {
        // open the mat-drawer state with the service
        const currentState = this._showEmailPopupService.isDrawerOpen();
        this._showEmailPopupService.toggleDrawer(!currentState);
    }
}
