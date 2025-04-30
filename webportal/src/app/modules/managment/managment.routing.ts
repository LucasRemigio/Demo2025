import { Route } from '@angular/router';
import { OrderComponent } from 'app/modules/order/order.component';
import { OrderListComponent } from 'app/modules/order/utils/list/list.component';
import { OrderDetailsComponent } from 'app/modules/order/utils/details/details.component';

import { LayoutComponent } from 'app/layout/layout.component';
import { ClientComponent } from './address_contacts/client.component';
import {
    ClientExclusionsResolver,
    ClientsResolver,
    HolidaysResolver,
} from './managment.resolvers';
import { ExclusionsComponent } from './exclusions/exclusions.component';
import { HolidaysComponent } from './holidays/holidays.component';

export const ClientRoutes: Route[] = [
    {
        path: 'exclusions',
        component: ExclusionsComponent,
        resolve: {
            exclusions: ClientExclusionsResolver,
        },
    },
    {
        path: 'clients',
        component: ClientComponent,
        resolve: {
            clients: ClientsResolver,
        },
    },
    {
        path: 'holidays',
        component: HolidaysComponent,
        resolve: {
            holidays: HolidaysResolver,
        },
    },
];
