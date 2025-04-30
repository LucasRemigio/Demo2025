/* eslint-disable @typescript-eslint/explicit-function-return-type */
/* eslint-disable arrow-parens */
import { Route } from '@angular/router';
import { AuthGuard } from 'app/core/auth/guards/auth.guard';
import { NoAuthGuard } from 'app/core/auth/guards/noAuth.guard';
import { LayoutComponent } from 'app/layout/layout.component';
import { InitialDataResolver } from 'app/app.resolvers';
import { AuthAdminGuard } from './core/auth/guards/authAdmin.guard';
import { AuthDepartmentGuard } from './core/auth/guards/authDepartment.guard';
import { UnauthorizedComponent } from './modules/unauthorized/unauthorized.component';
import { UnauthorizedModule } from './modules/unauthorized/unauthorized.module';
import { AuthSupervisorGuard } from './core/auth/guards/authSupervisor.guard';

// @formatter:off
// tslint:disable:max-line-length
export const appRoutes: Route[] = [
    // Redirect root URL to filtering/audit
    {
        path: '',
        redirectTo: 'filtering/audit',
        pathMatch: 'full',
    },
    {
        path: 'signed-in-redirect',
        canActivate: [AuthAdminGuard],
        component: LayoutComponent,
        pathMatch: 'full',
    },

    // Auth routes for guests
    {
        path: '',
        canActivate: [NoAuthGuard],
        canActivateChild: [NoAuthGuard],
        component: LayoutComponent,
        data: {
            layout: 'empty',
        },
        children: [
            {
                path: 'forgot-password',
                loadChildren: () =>
                    import(
                        'app/modules/auth/forgot-password/forgot-password.module'
                    ).then((m) => m.AuthForgotPasswordModule),
            },
            {
                path: 'sign-in',
                loadChildren: () =>
                    import('app/modules/auth/sign-in/sign-in.module').then(
                        (m) => m.AuthSignInModule
                    ),
            },
            {
                path: 'reset-password',
                loadChildren: () =>
                    import(
                        'app/modules/auth/reset-password/reset-password.module'
                    ).then((m) => m.AuthResetPasswordModule),
            },
            {
                path: 'reset-password/:email/:token',
                loadChildren: () =>
                    import(
                        'app/modules/auth/reset-password/reset-password.module'
                    ).then((m) => m.AuthResetPasswordModule),
            },
        ],
    },

    // Auth routes for authenticated users
    {
        path: '',
        canActivate: [AuthGuard],
        canActivateChild: [AuthGuard],
        component: LayoutComponent,
        data: {
            layout: 'empty',
        },
        children: [
            {
                path: 'sign-out',
                loadChildren: () =>
                    import('app/modules/auth/sign-out/sign-out.module').then(
                        (m) => m.AuthSignOutModule
                    ),
            },
        ],
    },

    // No login routes but with default layout
    {
        path: '',
        component: LayoutComponent,
        resolve: {
            initialData: InitialDataResolver,
        },
        data: {
            layout: 'empty',
        },
        children: [
            {
                path: 'order/confirmation',
                loadChildren: () =>
                    import(
                        'app/modules/client-order-confirmation/client-order-confirmation.module'
                    ).then((m) => m.ClientOrderConfirmationModule),
            },
        ],
    },

    // App routes
    {
        path: '',
        canActivate: [AuthGuard],
        canActivateChild: [AuthGuard],
        component: LayoutComponent,
        resolve: {
            initialData: InitialDataResolver,
        },
        children: [
            // users
            {
                path: 'filtering',
                loadChildren: () =>
                    import('app/modules/filtering/filtering.module').then(
                        (m) => m.FilteringModule
                    ),
            },
            {
                path: 'communications',
                loadChildren: () =>
                    import(
                        'app/modules/communications/communications.module'
                    ).then((m) => m.CommunicationsModule),
            },
            {
                path: 'orders',
                canActivate: [AuthDepartmentGuard],
                data: { requiredDepartment: 'orders' },
                loadChildren: () =>
                    import('app/modules/orders/orders.module').then(
                        (m) => m.OrdersModule
                    ),
            },
            {
                path: 'quotes_budgets',
                canActivate: [AuthDepartmentGuard],
                data: { requiredDepartment: 'quotations' },
                loadChildren: () =>
                    import(
                        'app/modules/quotes_budgets/quotes_budgets.module'
                    ).then((m) => m.QuotesBudgetsModule),
            },
            {
                path: 'receipts',
                canActivate: [AuthDepartmentGuard],
                data: { requiredDepartment: 'receipts' },
                loadChildren: () =>
                    import('app/modules/receipts/receipts.module').then(
                        (m) => m.ReceiptsModule
                    ),
            },
            {
                path: 'certificates',
                canActivate: [AuthDepartmentGuard],
                data: { requiredDepartment: 'certificates' },
                loadChildren: () =>
                    import(
                        'app/modules/quality-certificates/quality-certificates.module'
                    ).then((m) => m.QualityCertificatesModule),
            },
            {
                path: 'accounts-managment',
                canActivate: [AuthAdminGuard],
                loadChildren: () =>
                    import(
                        'app/modules/accounts-managment/accounts-managment.module'
                    ).then((m) => m.AccountsManagmentModule),
            },
            {
                path: 'configurations',
                canActivate: [AuthAdminGuard],
                loadChildren: () =>
                    import(
                        'app/modules/configurations/configurations.module'
                    ).then((m) => m.ConfigurationsModule),
            },
            {
                path: 'awaiting-approval',
                canActivate: [AuthSupervisorGuard],
                loadChildren: () =>
                    import(
                        'app/modules/awaiting-approval/awaiting-approval.module'
                    ).then((m) => m.AwaitingApprovalModule),
            },

            /*
                Orchestration
            */
            {
                path: 'jobs',
                loadChildren: () =>
                    import('app/modules/automations/jobs/jobs.module').then(
                        (m) => m.JobsModule
                    ),
                canActivate: [AuthAdminGuard],
            },
            {
                path: 'scripts',
                loadChildren: () =>
                    import(
                        'app/modules/automations/scripts/scripts.component.module'
                    ).then((m) => m.ScriptsModule),
                canActivate: [AuthAdminGuard],
            },
            {
                path: 'assets',
                loadChildren: () =>
                    import(
                        'app/modules/automations/assets/assets.component.module'
                    ).then((m) => m.AssetsModule),
                canActivate: [AuthAdminGuard],
            },
            {
                path: 'queues',
                loadChildren: () =>
                    import(
                        'app/modules/automations/queues/queues.component.module'
                    ).then((m) => m.QueuesModule),
                canActivate: [AuthAdminGuard],
            },
            {
                path: 'triggers',
                loadChildren: () =>
                    import(
                        'app/modules/automations/triggers/triggers.module'
                    ).then((m) => m.TriggersModule),
                canActivate: [AuthAdminGuard],
            },
            {
                path: 'transactions/:id',
                loadChildren: () =>
                    import(
                        'app/modules/automations/queues/transactions/transactions.component.module'
                    ).then((m) => m.TransactionsModule),
                canActivate: [AuthAdminGuard],
            },
            {
                path: 'unauthorized',
                loadChildren: () =>
                    import('app/modules/unauthorized/unauthorized.module').then(
                        (m) => m.UnauthorizedModule
                    ),
            },

            // admins
            // {path: 'accounts-managment', loadChildren: () => import('app/modules/accounts-managment/accounts-managment.module').then(m => m.AccountsManagmentModule)},
            // {
            //     path: 'logs',
            //     loadChildren: () =>
            //         import('app/modules/logs/logs.module').then(
            //             (m) => m.LogsModule
            //         ),
            // },
            // {path: 'settings-managment', loadChildren: () => import('app/modules/settings-managment/settings-managment.module').then(m => m.SettingsManagmentModule)},
        ],
    },

    // Catch-all for unmatched routes or 404s
    // If not logged in it goes to login page
    {
        path: '**',
        redirectTo: 'filtering/audit',
    },
];
