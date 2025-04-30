import { Route } from '@angular/router';
// import {
//     TransactionGetByQueueIdResolver,
//     TransactionsResolver
// } from 'app/modules/automations/queues/transactions/transactions.resolvers';
import { LayoutComponent } from 'app/layout/layout.component';
import { TransactionsComponent } from './transactions.component';
import { TransactionsResolver } from './transactions.resolvers';

export const TransactionRoutes: Route[] = [
    {
        path: 'transactions/:id',
        component: TransactionsComponent,
        resolve: {
            transactions: TransactionsResolver,
        }
        // children: [
        //     {
        //         path: '',
        //         pathMatch: 'full',
        //         component: TransactionsComponent,
        //         resolve: {
        //             librarys: TransactionsResolver,
        //         },
        //     },
        //     {
        //         path: ':id',
        //         component: TransactionsComponent,
        //         resolve: {
        //             library: TransactionGetByQueueIdResolver,
        //         },
        //     },
        // ],
    },


]