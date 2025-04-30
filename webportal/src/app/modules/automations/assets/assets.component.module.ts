import { NgModule } from '@angular/core';
import { Route, RouterModule } from '@angular/router';
import { AssetsComponent } from './assets.component';
import { AddAssets } from '../add-assets/add-assets.component';
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
import { EditAsset } from './edit-asset/edit-asset.component';

const AssetsRoutes: Route[] = [
    {
        path: '',
        component: AssetsComponent,
    },
];

@NgModule({
    declarations: [AssetsComponent, EditAsset, AddAssets],
    imports: [
        RouterModule.forChild(AssetsRoutes),
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
export class AssetsModule {}
