import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';
import { Route, RouterModule } from '@angular/router';
import { AuditComponent } from './audit/audit.component';
import { FilteringTableModule } from 'app/shared/components/filtering-table/filtering-table.module';
import { SharedModule } from '../../shared/shared.module';

const Routes: Route[] = [
    {
        path: 'audit',
        component: AuditComponent,
    },
];

@NgModule({
    declarations: [AuditComponent],
    imports: [
        RouterModule.forChild(Routes),
        CommonModule,
        FilteringTableModule,
        SharedModule,
    ],
})
export class QualityCertificatesModule {}
