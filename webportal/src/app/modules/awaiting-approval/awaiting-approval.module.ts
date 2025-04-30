import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';
import { ApproveRatingsComponent } from './approve-ratings/approve-ratings.component';
import { Route, RouterModule } from '@angular/router';
import { FilteringValidateModule } from 'app/shared/components/filtering-validate/filtering-validate.module';
import { SharedModule } from 'app/shared/shared.module';
import { ValidateDetailsComponent } from 'app/shared/components/filtering-validate/details/details.component';
import { OrderValidateModule } from 'app/shared/components/order-validate/order-validate.module';
import { ApproveCreditComponent } from './approve-credit/approve-credit.component';

const routes: Route[] = [
    {
        path: 'ratings',
        children: [
            {
                path: '',
                pathMatch: 'full',
                component: ApproveRatingsComponent,
            },
            {
                path: ':id',
                component: ValidateDetailsComponent,
            },
        ],
    },
    {
        path: 'credits',
        children: [
            {
                path: '',
                pathMatch: 'full',
                component: ApproveCreditComponent,
            },
            {
                path: ':id',
                component: ValidateDetailsComponent,
            },
        ],
    },
];

@NgModule({
    declarations: [ApproveRatingsComponent, ApproveCreditComponent],
    imports: [
        RouterModule.forChild(routes),
        CommonModule,
        SharedModule,
        FilteringValidateModule,
        OrderValidateModule,
    ],
})
export class AwaitingApprovalModule {}
