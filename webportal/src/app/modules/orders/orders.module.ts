import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';
import { Route, RouterModule } from '@angular/router';
import { StatisticsComponent } from './statistics/statistics.component';
import { AuditComponent } from './audit/audit.component';
import { ValidateComponent } from './validate/validate.component';
import { FilteringTableModule } from 'app/shared/components/filtering-table/filtering-table.module';
import { FilteringValidateModule } from 'app/shared/components/filtering-validate/filtering-validate.module';
import { ValidateDetailsComponent } from 'app/shared/components/filtering-validate/details/details.component';
import { SharedModule } from '../../shared/shared.module';
import { CreateNewComponent } from './create-new/create-new.component';
import { OrderValidateModule } from 'app/shared/components/order-validate/order-validate.module';
import { OrderTableModule } from 'app/shared/components/order-table/order-table.module';

const Routes: Route[] = [
    {
        path: 'statistics',
        component: StatisticsComponent,
    },
    {
        path: 'audit',
        component: AuditComponent,
    },
    {
        path: 'validate',
        children: [
            {
                path: '',
                pathMatch: 'full',
                component: ValidateComponent,
            },
            {
                path: ':id',
                component: ValidateDetailsComponent,
            },
        ],
    },
    {
        path: 'create',
        component: CreateNewComponent,
    },
];

@NgModule({
    declarations: [
        AuditComponent,
        ValidateComponent,
        StatisticsComponent,
        CreateNewComponent,
    ],
    imports: [
        RouterModule.forChild(Routes),
        CommonModule,
        OrderValidateModule,
        FilteringTableModule,
        FilteringValidateModule,
        SharedModule,
        OrderTableModule,
    ],
})
export class OrdersModule {}
