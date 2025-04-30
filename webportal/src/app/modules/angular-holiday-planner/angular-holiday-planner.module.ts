import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';
import { CalendarViewComponent } from './calendar-view.component';
import { DateFormatPipe } from './date-format.pipe';



@NgModule({
    declarations: [
        CalendarViewComponent,
        DateFormatPipe
    ],
    imports: [
        CommonModule
    ],
    exports: [
        CalendarViewComponent
    ]
})
export class AngularHolidayPlannerModule { }
