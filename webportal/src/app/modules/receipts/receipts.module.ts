import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';
import { Route, RouterModule } from '@angular/router';
import { StatisticsComponent } from './statistics/statistics.component';
import { AuditComponent } from './audit/audit.component';
import { ValidateComponent } from './validate/validate.component';
import { FilteringTableModule } from 'app/shared/components/filtering-table/filtering-table.module';
import { FilteringValidateModule } from 'app/shared/components/filtering-validate/filtering-validate.module';
import { ValidateDetailsComponent } from 'app/shared/components/filtering-validate/details/details.component';
import { SharedModule } from 'app/shared/shared.module';

const routes: Route[] = [
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
];

@NgModule({
    declarations: [AuditComponent, ValidateComponent, StatisticsComponent],
    imports: [
        RouterModule.forChild(routes),
        CommonModule,
        FilteringTableModule,
        FilteringValidateModule,
        SharedModule,
    ],
})
export class ReceiptsModule {}
