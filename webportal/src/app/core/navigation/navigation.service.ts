/* eslint-disable arrow-parens */
import { Injectable } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { Observable, ReplaySubject } from 'rxjs';
import { Navigation } from 'app/core/navigation/navigation.types';
import { TranslocoService } from '@ngneat/transloco';
import { FuseNavigationItem } from '@fuse/components/navigation';
import { UserService } from '../user/user.service';
import { MatDialog } from '@angular/material/dialog';

@Injectable({
    providedIn: 'root',
})
export class NavigationService {
    labelOrder: string = this.translocoService.translate('Order.orders', {});
    labelRequests: string = this.translocoService.translate(
        'NavBar.requests',
        {}
    );
    labelReceipts: string = this.translocoService.translate(
        'NavBar.receipts',
        {}
    );
    labelCredentials: string = this.translocoService.translate(
        'NavBar.credentials',
        {}
    );
    labelSettings: string = this.translocoService.translate(
        'NavBar.settings',
        {}
    );
    labelLogs: string = this.translocoService.translate('Logs.logs', {});
    labelDashboards: string = this.translocoService.translate(
        'Order.dashboard',
        {}
    );
    labelAudit: string = this.translocoService.translate('Order.audit', {});
    labelValidate: string = this.translocoService.translate(
        'Order.for-validate',
        {}
    );
    labelFiltering: string = this.translocoService.translate(
        'NavBar.filtering',
        {}
    );
    labelCommunications: string = this.translocoService.translate(
        'NavBar.communications',
        {}
    );
    labelCompose: string = this.translocoService.translate(
        'NavBar.compose',
        {}
    );
    labelCertificates: string = this.translocoService.translate(
        'NavBar.certificates',
        {}
    );
    labelDuplicates: string = this.translocoService.translate(
        'Filtering.duplicates',
        {}
    );
    labelConfigurations: string = this.translocoService.translate(
        'NavBar.configurations',
        {}
    );
    labelCancelReasons: string = this.translocoService.translate(
        'Cancel-Reason.cancel-reason',
        {}
    );
    labelClients: string = this.translocoService.translate('clients', {});
    labelProducts: string = this.translocoService.translate(
        'Order.products',
        {}
    );
    labelSpam: string = this.translocoService.translate('Category.spam', {});
    labelAwaitingApproval: string = this.translocoService.translate(
        'Filtering.awaiting-approval'
    );
    labelProposedRatings: string =
        this.translocoService.translate('proposed-ratings');
    labelPendingCredits: string =
        this.translocoService.translate('pending-credits');
    labelCreate: string = this.translocoService.translate('create-new', {});
    labelPlatform: string = this.translocoService.translate('platform');

    private _navigation: ReplaySubject<Navigation> =
        new ReplaySubject<Navigation>(1);

    /**
     * Constructor
     */
    constructor(private readonly translocoService: TranslocoService) {
        const initialNav: Navigation = {
            compact: [],
            default: [],
            futuristic: [],
            horizontal: [],
        };
        this._navigation.next(initialNav);
    }

    // -----------------------------------------------------------------------------------------------------
    // @ Accessors
    // -----------------------------------------------------------------------------------------------------

    /**
     * Getter for navigation
     */
    get navigation$(): Observable<Navigation> {
        return this._navigation.asObservable();
    }

    // -----------------------------------------------------------------------------------------------------
    // @ Public methods
    // -----------------------------------------------------------------------------------------------------

    /**
     * Get all navigation data
     */
    get(userService: UserService): Observable<Navigation> {
        const itemArr: FuseNavigationItem[] = [];
        const isAdmin = userService.isAdmin();
        const isSupervisor = userService.isSupervisor();
        const userDepartments = userService.getDepartments();

        const nav: Navigation = {
            compact: itemArr,
            default: itemArr,
            futuristic: itemArr,
            horizontal: itemArr,
        };

        const communicationsDashboard: FuseNavigationItem = {
            orderNumber: 1,
            id: 'communications',
            title: this.labelCommunications,
            type: 'collapsable',
            icon: 'heroicons_outline:inbox',
            children: [
                {
                    id: 'communications.compose',
                    title: this.labelCompose,
                    type: 'basic',
                    link: '/communications/compose',
                },
                {
                    id: 'communications.audit',
                    title: this.labelAudit,
                    type: 'basic',
                    link: '/communications/audit',
                },
            ],
        };

        const filtering: FuseNavigationItem = {
            orderNumber: 2,
            id: 'filtering',
            title: this.labelFiltering,
            type: 'collapsable',
            icon: 'heroicons_outline:beaker',
            children: [
                {
                    id: 'filtering.statistics',
                    title: this.labelDashboards,
                    type: 'basic',
                    link: '/filtering/statistics',
                },
                {
                    id: 'filtering.validate',
                    title: this.labelValidate,
                    type: 'basic',
                    link: '/filtering/validate',
                },
                {
                    id: 'filtering.duplicates',
                    title: this.labelDuplicates,
                    type: 'basic',
                    link: '/filtering/duplicates',
                },
                {
                    id: 'filtering.spam',
                    title: this.labelSpam,
                    type: 'basic',
                    link: '/filtering/spam',
                },
                {
                    id: 'filtering.audit',
                    title: this.labelAudit,
                    type: 'basic',
                    link: '/filtering/audit',
                },
            ],
        };
        const ordersDashboard: FuseNavigationItem = {
            orderNumber: 3,
            id: 'orders',
            title: this.labelOrder,
            type: 'collapsable',
            icon: 'heroicons_outline:clipboard',
            children: [
                //{
                //     id: 'orders.statistics',
                //     title: this.labelDashboards,
                //     type: 'basic',
                //     link: '/orders/statistics',
                // },
                {
                    id: 'orders.create',
                    title: this.labelCreate,
                    type: 'basic',
                    link: '/orders/create',
                },
                {
                    id: 'orders.library',
                    title: this.labelValidate,
                    type: 'basic',
                    link: '/orders/validate',
                },
                {
                    id: 'orders.sources',
                    title: this.labelAudit,
                    type: 'basic',
                    link: '/orders/audit',
                },
            ],
        };
        const quotesAndBudgetsDashboard: FuseNavigationItem = {
            orderNumber: 4,
            id: 'quotes_budgets',
            title: this.labelRequests,
            type: 'collapsable',
            icon: 'heroicons_outline:currency-euro',
            children: [
                // {
                //     id: 'quotes_budgets.statistics',
                //     title: this.labelDashboards,
                //     type: 'basic',
                //     link: '/quotes_budgets/statistics',
                // },
                {
                    id: 'quotes_budgets.create',
                    title: this.labelCreate,
                    type: 'basic',
                    link: '/quotes_budgets/create',
                },
                {
                    id: 'quotes_budgets.validate',
                    title: this.labelValidate,
                    type: 'basic',
                    link: '/quotes_budgets/validate',
                },
                {
                    id: 'quotes_budgets.audit',
                    title: this.labelAudit,
                    type: 'basic',
                    link: '/quotes_budgets/audit',
                },
            ],
        };

        const receiptsDashboard: FuseNavigationItem = {
            orderNumber: 5,
            id: 'receipts',
            title: this.labelReceipts,
            type: 'collapsable',
            icon: 'heroicons_outline:document-currency-euro',
            children: [
                {
                    id: 'receipts.audit',
                    title: this.labelAudit,
                    type: 'basic',
                    link: '/receipts/audit',
                },
            ],
        };

        const qualityCertificatesDashboard: FuseNavigationItem = {
            orderNumber: 6,
            id: 'certificates',
            title: this.labelCertificates,
            type: 'collapsable',
            icon: 'heroicons_outline:newspaper',
            children: [
                {
                    id: 'certificates.audit',
                    title: this.labelAudit,
                    type: 'basic',
                    link: '/certificates/audit',
                },
            ],
        };

        const awaitingApproval: FuseNavigationItem = {
            orderNumber: 7,
            id: 'awaiting_approval',
            title: this.labelAwaitingApproval,
            type: 'collapsable',
            icon: 'heroicons_outline:clock',
            children: [
                {
                    id: 'awaiting_approval.ratings',
                    title: this.labelProposedRatings,
                    type: 'basic',
                    link: '/awaiting-approval/ratings',
                },
                {
                    id: 'awaiting_approval.credits',
                    title: this.labelPendingCredits,
                    type: 'basic',
                    link: '/awaiting-approval/credits',
                },
            ],
        };

        const configurationsDashboard: FuseNavigationItem = {
            orderNumber: 8,
            id: 'configurations',
            title: this.labelConfigurations,
            type: 'collapsable',
            icon: 'heroicons_outline:cog',
            children: [
                {
                    id: 'configurations.clients',
                    title: this.labelClients,
                    type: 'basic',
                    link: '/configurations/clients',
                    orderNumber: 2,
                },
                {
                    id: 'configurations.products',
                    title: this.labelProducts,
                    type: 'basic',
                    link: '/configurations/products',
                    orderNumber: 3,
                },
                {
                    id: 'configurations.cancel-reasons',
                    title: this.labelCancelReasons,
                    type: 'basic',
                    link: '/configurations/cancel-reasons',
                    orderNumber: 4,
                },
            ],
        };

        const platformConfigurations: FuseNavigationItem = {
            id: 'configurations.platform',
            title: this.labelPlatform,
            type: 'basic',
            link: '/configurations/platform',
            orderNumber: 1,
        };

        const scriptsNav: FuseNavigationItem = {
            orderNumber: 9,
            id: 'scripts',
            title: this.translocoService.translate('Scripts.automatisms', {}),
            type: 'collapsable',
            icon: 'heroicons_outline:play',
            children: [
                {
                    id: 'scripts',
                    title: this.translocoService.translate(
                        'Scripts.processes',
                        {}
                    ),
                    type: 'basic',
                    link: '/scripts',
                },
                {
                    id: 'jobs',
                    title: this.translocoService.translate(
                        'Scripts.executions',
                        {}
                    ),
                    type: 'basic',
                    link: '/jobs',
                },
                {
                    id: 'assets',
                    title: this.translocoService.translate(
                        'Scripts.assets',
                        {}
                    ),
                    type: 'basic',
                    link: '/assets',
                },
                {
                    id: 'queues',
                    title: this.translocoService.translate(
                        'Scripts.queues',
                        {}
                    ),
                    type: 'basic',
                    link: '/queues',
                },
                {
                    id: 'triggers',
                    title: this.translocoService.translate(
                        'Scripts.triggers',
                        {}
                    ),
                    type: 'basic',
                    link: '/triggers',
                },
            ],
        };

        const accountsManagmnet: FuseNavigationItem = {
            orderNumber: 10,
            id: 'accounts-managment',
            title: this.labelCredentials,
            type: 'basic',
            icon: 'heroicons_outline:user-group',
            link: '/accounts-managment',
        };

        // Mapping between departments and navigation items
        const departmentNavMap = {
            filtering: filtering, // Assuming 'comercial' corresponds to 'filtering'
            orders: ordersDashboard,
            quotations: quotesAndBudgetsDashboard,
            receipts: receiptsDashboard,
            communications: communicationsDashboard,
            certificates: qualityCertificatesDashboard,
            configurations: configurationsDashboard,
        };

        // First add the communications tab, if the user has access to it
        if (userDepartments.includes('communications')) {
            itemArr.push(departmentNavMap['communications']);
        }

        // Loop through the user's departments and push the corresponding items into the array
        userDepartments.forEach((department) => {
            if (
                department === 'communications' ||
                department === 'configurations'
            ) {
                return;
            }

            if (departmentNavMap[department]) {
                itemArr.push(departmentNavMap[department]);
            }
        });

        if (isAdmin) {
            itemArr.push(accountsManagmnet);
            itemArr.push(scriptsNav);

            // add the platform configurations tab
            configurationsDashboard.children.push(platformConfigurations);
            // sort the configurations tab children by orderNumber
            configurationsDashboard.children.sort(
                (a, b) => a.orderNumber - b.orderNumber
            );
        }

        if (isSupervisor) {
            itemArr.push(awaitingApproval);
        }

        // First add the communications tab, if the user has access to it
        if (userDepartments.includes('configurations')) {
            itemArr.push(departmentNavMap['configurations']);
        }

        itemArr.sort((a, b) => a.orderNumber - b.orderNumber);

        this._navigation.next(nav);
        return this._navigation.asObservable();
    }
}
