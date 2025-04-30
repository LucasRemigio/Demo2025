import {
    ChangeDetectorRef,
    Component,
    Inject,
    OnInit,
    ViewEncapsulation,
} from '@angular/core';
import { FormBuilder, FormGroup, Validators } from '@angular/forms';
import { MAT_DIALOG_DATA, MatDialogRef } from '@angular/material/dialog';
import { TranslocoService } from '@ngneat/transloco';
import { ScriptsService } from '../../scripts/scripts.service';
import { TriggersService } from '../triggers.service';

@Component({
    selector: 'app-add-triggers',
    templateUrl: './add-triggers.component.html',
    encapsulation: ViewEncapsulation.None,
})
export class AddTriggers implements OnInit {
    composeForm: FormGroup;
    flashMessage: 'success' | 'error' | null = null;
    flashMessageText: string;
    scriptNames: string[] = [];
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
        // public matDialogRef: MatDialogRef<ImportAssets>,
        public matDialogRef: MatDialogRef<AddTriggers>,
        private _formBuilder: FormBuilder,
        private translocoService: TranslocoService,
        private scriptsService: ScriptsService,
        private triggersService: TriggersService,
        private fb: FormBuilder,
        @Inject(MAT_DIALOG_DATA) public data: any
    ) {}

    // ngOnInit(): void {
    //     this.fetchScriptNamesFromBE();

    //     // Create the form
    //     this.composeForm = this._formBuilder.group({
    //       name: [
    //         '',
    //         [Validators.minLength(1), Validators.maxLength(500)],
    //       ],
    //       cron_expression: [
    //         '',
    //         [Validators.minLength(1), Validators.maxLength(500)],
    //       ],
    //       script_name: [
    //         '',
    //         [Validators.required],
    //       ],
    //       cronJobScheduler: this._formBuilder.group({}) // Initialize empty for now
    //     });

    // }

    ngOnInit(): void {
        this.fetchScriptNamesFromBE();

        // Create the form
        this.composeForm = this._formBuilder.group({
          name: [
            '',
            [Validators.minLength(1), Validators.maxLength(500)],
          ],
        //   cron_expression: [
        //     '',
        //     [Validators.minLength(1), Validators.maxLength(500)],
        //   ],
          script_name: [
            '',
            [Validators.required],
          ],
          scheduleType: ['daily'], // Default to daily
          dailyTime: this._formBuilder.group({
            hour: [0, [Validators.required, Validators.min(0), Validators.max(23)]],
            minute: [0, [Validators.required, Validators.min(0), Validators.max(59)]],
          }),
          weeklySchedule: this._formBuilder.group({
            dayOfWeek: [[], Validators.required], // Use an array to select multiple days
            hour: [0, [Validators.required, Validators.min(0), Validators.max(23)]],
            minute: [0, [Validators.required, Validators.min(0), Validators.max(59)]],
          }),
          monthlyDayOfMonth: this._formBuilder.group({
            dayOfMonth: [1, [Validators.required, Validators.min(1), Validators.max(31)]],
            // monthInterval: [1, [Validators.required, Validators.min(1)]],
            hour: [0, [Validators.required, Validators.min(0), Validators.max(23)]],
            minute: [0, [Validators.required, Validators.min(0), Validators.max(59)]],
          }),
        //   monthlyDayOfWeek: this._formBuilder.group({
        //     dayOfWeek: [[], Validators.required],
        //     monthInterval: [1, [Validators.required, Validators.min(1)]],
        //     hour: [0, [Validators.required, Validators.min(0), Validators.max(23)]],
        //     minute: [0, [Validators.required, Validators.min(0), Validators.max(59)]],
        //   }),
          advancedCronExpression: ['', Validators.required],
          });
    }

    fetchScriptNamesFromBE(): void {
        this.isLoading = true;

        this.scriptsService.getScripts().subscribe(
            (response) => {
                this.isLoading = false;
                if (response && response.scripts_items) {
                    // Filter out scripts with status '2' and extract names
                    this.scriptNames = response.scripts_items
                        .filter(script => script.status !== '2')
                        .map(script => script.name);

                    // Trigger change detection
                    this._changeDetectorRef.markForCheck();
                } else {
                    console.error('Invalid response structure:', response);
                }
            },
            (err) => {
                this.isLoading = false;
                console.error('Error fetching scripts:', err);
                this.showFlashMessage(
                    'error',
                    this.translocoService.translate('error-loading-list', {})
                );

                this._changeDetectorRef.markForCheck();
            }
        );
    }

    // -----------------------------------------------------------------------------------------------------
    // @ Public methods
    // -----------------------------------------------------------------------------------------------------

    close(): void {
        // Close the dialog
        this.matDialogRef.close();
        this.isLoading = false;
    }

    onScheduleTypeChanged(): void {
        // Handle schedule type change here
        const selectedType = this.composeForm.get('scheduleType').value;
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
                const daysOfWeek = weeklySchedule.dayOfWeek.map(day => this.daysOfWeek[day].id).join(',');
                cronExpression = `0 ${weeklySchedule.minute} ${weeklySchedule.hour} ? * ${daysOfWeek}`;
                break;
            case 'monthlyDayOfMonth':
                const monthlyDayOfMonth = this.composeForm.get('monthlyDayOfMonth').value;
                cronExpression = `0 ${monthlyDayOfMonth.minute} ${monthlyDayOfMonth.hour} ${monthlyDayOfMonth.dayOfMonth} * ?`;
                break;
            case 'monthlyDayOfWeek':
                const monthlyDayOfWeek = this.composeForm.get('monthlyDayOfWeek').value;
                const monthDaysOfWeek = monthlyDayOfWeek.dayOfWeek.map(day => this.daysOfWeek[day].id).join(',');
                cronExpression = `0 ${monthlyDayOfWeek.minute} ${monthlyDayOfWeek.hour} * * ${monthDaysOfWeek}`;
                break;
            case 'advanced':
                cronExpression = this.composeForm.controls['advancedCronExpression'].value;
                break;
            default:
                cronExpression = '';
        }

        return cronExpression;
    }

    /**
     * Save and close
     */
    saveAndClose(): void {

        const name = this.composeForm.controls['name'].value;
        // const cron_expression = this.composeForm.controls['cron_expression'].value;
        const script_name = this.composeForm.controls['script_name'].value;

        // const cron_expression = this.composeForm.get('cron_expression').value;

        const cron_expression = this.generateCronExpression();

        // // Get the appropriate cron expression based on the schedule type
        // switch (scheduleType) {
        // case 'daily':
        //     cron_expression = this.composeForm.get('dailyTime').value;
        //     break;
        // case 'weekly':
        //     cron_expression = this.composeForm.get('weeklySchedule').value;
        //     break;
        // case 'monthlyDayOfMonth':
        //     cron_expression = this.composeForm.get('monthlyDayOfMonth').value;
        //     break;
        // case 'monthlyDayOfWeek':
        //     cron_expression = this.composeForm.get('monthlyDayOfWeek').value;
        //     break;
        // case 'advanced':
        //     cron_expression = this.composeForm.controls['advancedCronExpression'].value;
        //     break;
        // default:
        //     cron_expression = '';
        // }

        // Check if any field is empty
        if (!name) {
            // Show our desired error message
            this.showFlashMessage('error', this.translocoService.translate('Scripts.name-field-required', {}));
            return; // Prevent further execution
        }

        if (!cron_expression) {
            // Show our desired error message
            this.showFlashMessage('error', this.translocoService.translate('Scripts.expression-field-required', {}));
            return; // Prevent further execution
        }

        if (!script_name) {
            // Show our desired error message
            this.showFlashMessage('error', this.translocoService.translate('Scripts.script-name-required', {}));
            return; // Prevent further execution
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
