import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';
import { ComposeComponent } from './compose/compose.component';
import { AuditComponent } from './audit/audit.component';
import { Route, RouterModule } from '@angular/router';
import { FormsModule, ReactiveFormsModule } from '@angular/forms';
import { TranslocoModule } from '@ngneat/transloco';
import { MatFormFieldModule } from '@angular/material/form-field';
import { MatIconModule } from '@angular/material/icon';
import { MatInputModule } from '@angular/material/input';
import { MatButtonModule } from '@angular/material/button';
import { QuillModule } from 'ngx-quill';
import { MatSelectModule } from '@angular/material/select';
import { MatProgressBarModule } from '@angular/material/progress-bar';
import { MatTooltipModule } from '@angular/material/tooltip';
import { MatDatepickerModule } from '@angular/material/datepicker';
import { MatNativeDateModule } from '@angular/material/core';
import { GridModule, PagerModule } from '@progress/kendo-angular-grid';
import { DropDownsModule } from '@progress/kendo-angular-dropdowns';
import { PreviewCommunicationsEmailComponent } from './audit/preview-email/preview-email.component';
import { MatSidenavModule } from '@angular/material/sidenav';
import { PdfViewerModule } from 'ng2-pdf-viewer';
import { MatAutocompleteModule } from '@angular/material/autocomplete';
import { EmailReplyFormModule } from 'app/shared/components/email-reply-form/email-reply-form.module';
import { EmailFormComponent } from './compose/email-form/email-form.component';
import { EmailFormModule } from './compose/email-form/email-form.module';

const routes: Route[] = [
    {
        path: 'compose',
        component: ComposeComponent,
    },
    {
        path: 'audit',
        component: AuditComponent,
    },
];

@NgModule({
    declarations: [
        ComposeComponent,
        AuditComponent,
        PreviewCommunicationsEmailComponent,
    ],
    imports: [
        RouterModule.forChild(routes),
        CommonModule,
        FormsModule,
        ReactiveFormsModule,
        TranslocoModule,
        MatFormFieldModule,
        MatIconModule,
        MatInputModule,
        MatButtonModule,
        MatSelectModule,
        QuillModule.forRoot(),
        MatProgressBarModule,
        MatTooltipModule,
        MatDatepickerModule,
        MatNativeDateModule,
        GridModule,
        PagerModule,
        DropDownsModule,
        MatSidenavModule,
        PdfViewerModule,
        MatAutocompleteModule,
        EmailReplyFormModule,
        EmailFormModule,
    ],
})
export class CommunicationsModule {}
