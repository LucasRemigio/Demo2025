import { NgModule } from '@angular/core';
import { Route, RouterModule } from '@angular/router';
import { JobsComponent } from './jobs.component';
import { MatFormFieldModule } from '@angular/material/form-field';
import { MatButtonModule } from '@angular/material/button';
import { MatIconModule } from '@angular/material/icon';
import { MatInputModule } from '@angular/material/input';
import { SharedModule } from 'app/shared/shared.module';
import { MatProgressBarModule } from '@angular/material/progress-bar';
import { MatTooltipModule } from '@angular/material/tooltip';
import { MatSelectModule } from '@angular/material/select';
import { TranslocoModule } from '@ngneat/transloco';
import { GridModule } from '@progress/kendo-angular-grid';
import { FilterableSettings } from '@progress/kendo-angular-grid';
import { PopUpInfo } from './pop-up-info/pop-up-info.component';
import { JobsRoutes } from './jobs.routing';
import { CommonModule } from '@angular/common';
import { BrowserModule } from '@angular/platform-browser';

@NgModule({
  declarations: [
    JobsComponent,
    PopUpInfo
    
  ],
  imports: [
    RouterModule.forChild(JobsRoutes),
    CommonModule,
    MatFormFieldModule,
    MatIconModule,
    MatInputModule,
    MatSelectModule,
    MatButtonModule,
    TranslocoModule,
    MatProgressBarModule,
    MatTooltipModule,
    GridModule
  ]
})
export class JobsModule { }


