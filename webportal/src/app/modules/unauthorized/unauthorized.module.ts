import { NgModule } from '@angular/core';
import { Route, RouterModule } from '@angular/router';
import { UnauthorizedComponent } from './unauthorized.component';
import { CommonModule } from '@angular/common';
import { TranslocoModule } from '@ngneat/transloco';

const unauthorizedRoutes: Route[] = [
    {
        path: '',
        component: UnauthorizedComponent,
    },
];

@NgModule({
    declarations: [UnauthorizedComponent],
    imports: [
        RouterModule.forChild(unauthorizedRoutes),
        CommonModule,
        TranslocoModule,
    ],
})
export class UnauthorizedModule {}
