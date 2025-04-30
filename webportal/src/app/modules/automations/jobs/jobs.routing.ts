import { Route } from '@angular/router';
import { JobsComponent } from './jobs.component';

export const JobsRoutes: Route[] = [
    {
        path: '',
        component: JobsComponent
    },
    {
        path: ':id',
        component: JobsComponent
    },
];
