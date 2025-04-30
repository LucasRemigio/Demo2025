import { NgModule } from '@angular/core';
import { RouterModule } from '@angular/router';
import { MatButtonModule } from '@angular/material/button';
import { MatFormFieldModule } from '@angular/material/form-field';
import { MatIconModule } from '@angular/material/icon';
import { MatInputModule } from '@angular/material/input';
import { MatProgressBarModule } from '@angular/material/progress-bar';
import { MatSelectModule } from '@angular/material/select';
import { MatSidenavModule } from '@angular/material/sidenav';
import { MatSlideToggleModule } from '@angular/material/slide-toggle';
import { MatTooltipModule } from '@angular/material/tooltip';
import { FuseFindByKeyPipeModule } from '@fuse/pipes/find-by-key';
import { SharedModule } from 'app/shared/shared.module';
import { OperatorRoutes } from 'app/modules/operator/operator.routing';
import { GridModule } from '@progress/kendo-angular-grid';
import { TranslocoModule } from '@ngneat/transloco';
import { MatTabsModule } from '@angular/material/tabs';
import { MatDatepickerModule } from '@angular/material/datepicker';
import { PdfViewerModule } from 'ng2-pdf-viewer';
import { ChartsModule } from '@progress/kendo-angular-charts';
import { LanguagesModule } from 'app/layout/common/languages/languages.module';
import { RecaptchaModule } from 'ng-recaptcha';
import { OperatorComponent } from './operator.component';
import { PopUpInfo } from './utils/pop-up-info/pop-up-info.component';

@NgModule({
    declarations: [OperatorComponent, PopUpInfo],
    imports: [
        RouterModule.forChild(OperatorRoutes),
        LanguagesModule,
        MatButtonModule,
        MatFormFieldModule,
        MatIconModule,
        MatInputModule,
        MatProgressBarModule,
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
    ],
})
export class OperatorModule {}
