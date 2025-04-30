import { NgModule } from '@angular/core';
import { Route, RouterModule } from '@angular/router';
import { ScriptsComponent } from './scripts.component';
import { AddScript } from '../add-script/add-script.component';
import { MatFormFieldModule } from '@angular/material/form-field';
import { MatButtonModule } from '@angular/material/button';
import { MatIconModule } from '@angular/material/icon';
import { MatInputModule } from '@angular/material/input';
import { SharedModule } from 'app/shared/shared.module';
import { MatProgressBarModule } from '@angular/material/progress-bar';
import { MatTooltipModule } from '@angular/material/tooltip';
import { TranslocoModule } from '@ngneat/transloco';
import { PasswordStrengthBarModule } from '../../auth/password-strength-bar/password-strenght-bar.module';
import { MatSelectModule } from '@angular/material/select';
import { GridModule } from '@progress/kendo-angular-grid';
import { EditScript } from './edit-script/edit-script.component';

const ScriptsRoutes: Route[] = [
    {
        path: '',
        component: ScriptsComponent,
    },
];

@NgModule({
    declarations: [ScriptsComponent, EditScript, AddScript],
    imports: [
        RouterModule.forChild(ScriptsRoutes),
        MatFormFieldModule,
        MatIconModule,
        MatInputModule,
        SharedModule,
        MatButtonModule,
        TranslocoModule,
        MatProgressBarModule,
        MatTooltipModule,
        PasswordStrengthBarModule,
        MatSelectModule,
        GridModule,
    ],
})
export class ScriptsModule {}
