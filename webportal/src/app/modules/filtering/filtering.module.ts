import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';
import { AuditComponent } from './audit/audit.component';
import { Route, RouterModule } from '@angular/router';
import { ValidateComponent } from './validate/validate.component';
import { StatisticsComponent } from './statistics/statistics.component';
import { TranslocoModule } from '@ngneat/transloco';
import { MatFormFieldModule } from '@angular/material/form-field';
import { MatButtonModule } from '@angular/material/button';
import { MatIconModule } from '@angular/material/icon';
import { MatInputModule } from '@angular/material/input';
import { SharedModule } from 'app/shared/shared.module';
import { MatProgressBarModule } from '@angular/material/progress-bar';
import { MatTooltipModule } from '@angular/material/tooltip';
import { MatSelectModule } from '@angular/material/select';
import { LanguagesModule } from 'app/layout/common/languages/languages.module';
import { MatSidenavModule } from '@angular/material/sidenav';
import { MatSlideToggleModule } from '@angular/material/slide-toggle';
import { FuseFindByKeyPipeModule } from '@fuse/pipes/find-by-key';
import { MatDatepickerModule } from '@angular/material/datepicker';
import { MatTabsModule } from '@angular/material/tabs';
import { ChartsModule } from '@progress/kendo-angular-charts';
import {
    DropDownListModule,
    DropDownsModule,
} from '@progress/kendo-angular-dropdowns';
import { GridModule } from '@progress/kendo-angular-grid';
import { RecaptchaModule } from 'ng-recaptcha';
import { PdfViewerModule } from 'ng2-pdf-viewer';
import { MatBadgeModule } from '@angular/material/badge';
import { MatProgressSpinnerModule } from '@angular/material/progress-spinner';
import { FilteringTableModule } from 'app/shared/components/filtering-table/filtering-table.module';
import { FilteringValidateModule } from 'app/shared/components/filtering-validate/filtering-validate.module';
import { DuplicatesComponent } from './duplicates/duplicates.component';
import { SpamComponent } from './spam/spam.component';
import { ValidateCategoryDetailsComponent } from './validate/details/details.component';

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
        path: 'duplicates',
        component: DuplicatesComponent,
    },
    {
        path: 'spam',
        component: SpamComponent,
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
                component: ValidateCategoryDetailsComponent,
            },
        ],
    },
];

@NgModule({
    declarations: [
        AuditComponent,
        ValidateComponent,
        ValidateCategoryDetailsComponent,
        StatisticsComponent,
        DuplicatesComponent,
        SpamComponent,
    ],
    imports: [
        RouterModule.forChild(Routes),
        LanguagesModule,
        MatButtonModule,
        MatFormFieldModule,
        MatIconModule,
        MatInputModule,
        MatProgressBarModule,
        MatProgressSpinnerModule,
        MatSelectModule,
        MatSidenavModule,
        MatSlideToggleModule,
        MatTooltipModule,
        FuseFindByKeyPipeModule,
        SharedModule,
        MatTabsModule,
        MatDatepickerModule,
        GridModule,
        TranslocoModule,
        PdfViewerModule,
        ChartsModule,
        RecaptchaModule,
        DropDownsModule,
        DropDownListModule,
        CommonModule,
        MatBadgeModule,
        FilteringTableModule,
        FilteringValidateModule,
    ],
})
export class FilteringModule {}
