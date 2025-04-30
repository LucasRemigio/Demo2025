import { Route } from '@angular/router';
import { TriggersComponent } from './triggers.component';

export const TriggersRoutes: Route[] = [
    {
        path: '',
        component: TriggersComponent,
    },
    {
        path: ':id',
        component: TriggersComponent
    },
];

