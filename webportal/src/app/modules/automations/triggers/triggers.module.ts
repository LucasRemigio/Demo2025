import { NgModule } from '@angular/core';
import { Route, RouterModule } from '@angular/router';
import { TriggersComponent } from './triggers.component';
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
import { EditTrigger } from './edit-trigger/edit-trigger.component';
import { AddTriggers } from './add-triggers/add-triggers.component';
import { MatDialog, MatDialogModule } from '@angular/material/dialog';
import { TriggersRoutes } from './triggers.routing';


@NgModule({
  declarations: [
    TriggersComponent,
    EditTrigger,
    AddTriggers,
  ],
  
  imports: [
    RouterModule.forChild(TriggersRoutes),
    MatFormFieldModule,
    MatIconModule,
    MatInputModule,
    SharedModule,
    MatSelectModule,
    MatButtonModule,
    MatDialogModule,
    TranslocoModule,
    MatProgressBarModule,
    MatTooltipModule,
    GridModule
  ]
})
export class TriggersModule { }


