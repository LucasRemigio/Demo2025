import { ChangeDetectorRef, Component, Inject, OnInit, ViewEncapsulation } from '@angular/core';
import { FormGroup, FormBuilder, Validators } from '@angular/forms';
import { MAT_DIALOG_DATA, MatDialogRef } from '@angular/material/dialog';
import { TranslocoService } from '@ngneat/transloco';
import { Triggers } from '../triggers.types';

@Component({
    selector: 'edit-trigger',
    templateUrl: './edit-trigger.component.html',
    encapsulation: ViewEncapsulation.None,
})
export class EditTrigger implements OnInit {
    composeForm: FormGroup;
    triggerDetails: string;
    flashMessage: 'success' | 'error' | null = null;
    flashMessageText: string;
    trigger: Triggers;
    isLoading: boolean = false;
    daysOfWeek: any[] = [
        { id: 1, name: 'Sunday' },
        { id: 2, name: 'Monday' },
        { id: 3, name: 'Tuesday' },
        { id: 4, name: 'Wednesday' },
        { id: 5, name: 'Thursday' },
        { id: 6, name: 'Friday' },
        { id: 7, name: 'Saturday' }
    ];

    constructor(
        private _changeDetectorRef: ChangeDetectorRef,
        public matDialogRef: MatDialogRef<EditTrigger>,
        private _formBuilder: FormBuilder,
        private translocoService: TranslocoService,
        @Inject(MAT_DIALOG_DATA) public data: any
    ) {}
    ngOnInit(): void {
        // Parse the cron expression to initialize the form fields
        const cronParts = this.data.trigger.cron_expression.split(' ');
        let scheduleType = 'advanced';
        let dailyTime = { hour: 0, minute: 0 };
        let weeklySchedule = { dayOfWeek: [], hour: 0, minute: 0 };
        let monthlyDayOfMonth = { dayOfMonth: 1, hour: 0, minute: 0 };

        if (cronParts[3] === '*' && cronParts[5] === '?') {
            scheduleType = 'daily';
            dailyTime = { hour: parseInt(cronParts[2], 10), minute: parseInt(cronParts[1], 10) };
        } else if (cronParts[3] === '?' && cronParts[5].includes(',')) {
            scheduleType = 'weekly';
            weeklySchedule = {
                dayOfWeek: cronParts[5].split(',').map(day => parseInt(day, 10)),
                hour: parseInt(cronParts[2], 10),
                minute: parseInt(cronParts[1], 10),
            };
        } else if (cronParts[3] !== '*' && cronParts[5] === '?') {
            scheduleType = 'monthlyDayOfMonth';
            monthlyDayOfMonth = {
                dayOfMonth: parseInt(cronParts[3], 10),
                hour: parseInt(cronParts[2], 10),
                minute: parseInt(cronParts[1], 10),
            };
        }

        this.composeForm = this._formBuilder.group({
            name: [this.data.trigger.name, Validators.required],
            script_name: [this.data.trigger.script_name, [Validators.required]],
            scheduleType: [scheduleType, Validators.required],
            dailyTime: this._formBuilder.group({
                hour: [dailyTime.hour, [Validators.required, Validators.min(0), Validators.max(23)]],
                minute: [dailyTime.minute, [Validators.required, Validators.min(0), Validators.max(59)]],
            }),
            weeklySchedule: this._formBuilder.group({
                dayOfWeek: [weeklySchedule.dayOfWeek, Validators.required],
                hour: [weeklySchedule.hour, [Validators.required, Validators.min(0), Validators.max(23)]],
                minute: [weeklySchedule.minute, [Validators.required, Validators.min(0), Validators.max(59)]],
            }),
            monthlyDayOfMonth: this._formBuilder.group({
                dayOfMonth: [monthlyDayOfMonth.dayOfMonth, [Validators.required, Validators.min(1), Validators.max(31)]],
                hour: [monthlyDayOfMonth.hour, [Validators.required, Validators.min(0), Validators.max(23)]],
                minute: [monthlyDayOfMonth.minute, [Validators.required, Validators.min(0), Validators.max(59)]],
            }),
            advancedCronExpression: [this.data.trigger.cron_expression, Validators.required],
        });
    }

    generateCronExpression(): string {
        const scheduleType = this.composeForm.controls['scheduleType'].value;
        let cronExpression: string = '';

        switch (scheduleType) {
            case 'daily':
                const dailyTime = this.composeForm.get('dailyTime').value;
                cronExpression = `0 ${dailyTime.minute} ${dailyTime.hour} * * ?`;
                break;
            case 'weekly':
                const weeklySchedule = this.composeForm.get('weeklySchedule').value;
                const daysOfWeek = weeklySchedule.dayOfWeek.map(day => day).join(',');
                cronExpression = `0 ${weeklySchedule.minute} ${weeklySchedule.hour} ? * ${daysOfWeek}`;
                break;
            case 'monthlyDayOfMonth':
                const monthlyDayOfMonth = this.composeForm.get('monthlyDayOfMonth').value;
                cronExpression = `0 ${monthlyDayOfMonth.minute} ${monthlyDayOfMonth.hour} ${monthlyDayOfMonth.dayOfMonth} * ?`;
                break;
            case 'advanced':
                cronExpression = this.composeForm.controls['advancedCronExpression'].value;
                break;
            default:
                cronExpression = '';
        }

        return cronExpression;
    }

    onScheduleTypeChanged(): void {
        const scheduleType = this.composeForm.controls['scheduleType'].value;

        // Reset form controls based on the selected schedule type
        switch (scheduleType) {
            case 'daily':
                this.composeForm.get('dailyTime').reset({
                    hour: 0,
                    minute: 0
                });
                break;
            case 'weekly':
                this.composeForm.get('weeklySchedule').reset({
                    dayOfWeek: [],
                    hour: 0,
                    minute: 0
                });
                break;
            case 'monthlyDayOfMonth':
                this.composeForm.get('monthlyDayOfMonth').reset({
                    dayOfMonth: 1,
                    hour: 0,
                    minute: 0
                });
                break;
            case 'advanced':
                this.composeForm.get('advancedCronExpression').reset('');
                break;
            default:
                break;
        }
    }


    close(): void {
        // Close the dialog
        this.isLoading = false;
        this.matDialogRef.close();
    }

        /**
     * Save and close
     */
    saveAndClose(): void {

        const name = this.composeForm.controls['name'].value;
        // const cron_expression = this.composeForm.controls['cron_expression'].value;
        const script_name = this.generateCronExpression();

        let cron_expression = '';

        // Generate cron expression based on schedule type
        const scheduleType = this.composeForm.controls['scheduleType'].value;
        switch (scheduleType) {
            case 'daily':
                const dailyTime = this.composeForm.get('dailyTime').value;
                cron_expression = `0 ${dailyTime.minute} ${dailyTime.hour} * * ?`;
                break;
            case 'weekly':
                const weeklySchedule = this.composeForm.get('weeklySchedule').value;
                const daysOfWeek = weeklySchedule.dayOfWeek.join(',');
                cron_expression = `0 ${weeklySchedule.minute} ${weeklySchedule.hour} ? * ${daysOfWeek}`;
                break;
            case 'monthlyDayOfMonth':
                const monthlyDayOfMonth = this.composeForm.get('monthlyDayOfMonth').value;
                cron_expression = `0 ${monthlyDayOfMonth.minute} ${monthlyDayOfMonth.hour} ${monthlyDayOfMonth.dayOfMonth} * ?`;
                break;
            case 'advanced':
                cron_expression = this.composeForm.controls['advancedCronExpression'].value;
                break;
            default:
                break;
        }

        // Close the dialog
        this.matDialogRef.close({
            name: name,
            cron_expression: cron_expression,
            script_name: script_name,
        });

        this.isLoading = false;
    }

    /**
     * Show flash message
     */
    showFlashMessage(type: 'success' | 'error', textMsg: string): void {
        // Show the message
        this.flashMessage = type;
        this.flashMessageText = textMsg;

        // Mark for check
        this._changeDetectorRef.markForCheck();

        // Hide it after 3 seconds
        setTimeout(() => {
            this.flashMessage = null;
            this.flashMessageText = null;

            // Mark for check
            this._changeDetectorRef.markForCheck();
        }, 5000);
    }
}