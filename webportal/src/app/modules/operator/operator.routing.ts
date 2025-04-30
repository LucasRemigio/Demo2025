import { Route } from '@angular/router';
import { OperatorComponent } from './operator.component';
import { OperatorsResolver } from './operator.resolvers';
import { DoesItHaveAccessGuard } from 'app/core/auth/guards/does-it-have-access.guard';

export const OperatorRoutes: Route[] = [
    {
        path: '',
        component: OperatorComponent,
        resolve: {
            operators: OperatorsResolver,
        },
        canActivate: [DoesItHaveAccessGuard],
    },
];
