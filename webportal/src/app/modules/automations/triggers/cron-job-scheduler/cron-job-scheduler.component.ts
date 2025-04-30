import { Component, EventEmitter, Output } from '@angular/core';
import { FormBuilder, FormGroup } from '@angular/forms';

@Component({
  selector: 'app-cron-job-scheduler',
  templateUrl: './cron-job-scheduler.component.html',
  styleUrls: ['./cron-job-scheduler.component.css']
})
export class CronJobSchedulerComponent {

  @Output() cronExpressionChanged = new EventEmitter<string>();

  cronJobForm: FormGroup;

  constructor(private fb: FormBuilder) {
    this.initForm();
  }

  initForm(): void {
    this.cronJobForm = this.fb.group({
      scheduleType: ['daily'], // Default to daily
      dailyTime: this.fb.group({
        hour: [0],
        minute: [0]
      }),
      weeklySchedule: this.fb.group({
        dayOfWeek: [[]], // Use an array to select multiple days
        time: ['']
      }),
      monthlyDayOfMonth: this.fb.group({
        dayOfMonth: [1],
        monthInterval: [1],
        time: ['']
      }),
      monthlyDayOfWeek: this.fb.group({
        dayOfWeek: [[]],
        monthInterval: [1],
        time: ['']
      }),
      advancedCronExpression: ['']
    });
  }

  emitCronExpression(): void {
    let cronExpression = '';

    switch (this.cronJobForm.get('scheduleType').value) {
      case 'daily':
        cronExpression = `0 ${this.cronJobForm.get('dailyTime.minute').value} ${this.cronJobForm.get('dailyTime.hour').value} * * ?`;
        break;
      case 'weekly':
        const daysOfWeek = this.cronJobForm.get('weeklySchedule.dayOfWeek').value.join(',');
        cronExpression = `0 ${this.cronJobForm.get('weeklySchedule.time').value} ? * ${daysOfWeek}`;
        break;
      case 'monthlyDayOfMonth':
        cronExpression = `0 ${this.cronJobForm.get('monthlyDayOfMonth.time').value} ${this.cronJobForm.get('monthlyDayOfMonth.dayOfMonth').value} 1/${this.cronJobForm.get('monthlyDayOfMonth.monthInterval').value} * ?`;
        break;
      case 'monthlyDayOfWeek':
        const daysOfWeekMonthly = this.cronJobForm.get('monthlyDayOfWeek.dayOfWeek').value.join(',');
        cronExpression = `0 ${this.cronJobForm.get('monthlyDayOfWeek.time').value} ? * ${daysOfWeekMonthly}#1`;
        break;
      case 'advanced':
        cronExpression = this.cronJobForm.get('advancedCronExpression').value;
        break;
      default:
        break;
    }

    this.cronExpressionChanged.emit(cronExpression);
  }
}