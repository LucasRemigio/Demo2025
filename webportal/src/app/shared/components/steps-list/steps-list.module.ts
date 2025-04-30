import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';
import { StepsListComponent } from './steps-list.component';
import { MatIconModule } from '@angular/material/icon';
import { MatTabsModule } from '@angular/material/tabs';

@NgModule({
    declarations: [StepsListComponent],
    imports: [CommonModule, MatIconModule, MatTabsModule],
    exports: [StepsListComponent],
})
export class StepsListModule {}
