import { NgModule } from '@angular/core';
import { Route, RouterModule } from '@angular/router';
import { LogsComponent } from './logs.component';
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

const ReceiptsRoutes: Route[] = [
  {
      path     : '',
      component: LogsComponent
  }
];

@NgModule({
  declarations: [
    LogsComponent,
    
  ],
  imports: [
    RouterModule.forChild(ReceiptsRoutes),
    MatFormFieldModule,
    MatIconModule,
    MatInputModule,
    SharedModule,
    MatSelectModule,
    MatButtonModule,
    TranslocoModule,
    MatProgressBarModule,
    MatTooltipModule,
    GridModule

  ]
})
export class LogsModule { }


