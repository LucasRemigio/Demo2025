/* eslint-disable @typescript-eslint/naming-convention */
import { Component, Inject, OnInit } from '@angular/core';
import { FormBuilder, FormGroup, Validators } from '@angular/forms';
import { MatDialogRef, MAT_DIALOG_DATA } from '@angular/material/dialog';
import { translate } from '@ngneat/transloco';
import { PlatformSetting } from '../platform-settings.types';

@Component({
    selector: 'app-edit-platform-setting',
    templateUrl: './edit-platform-setting.component.html',
    styleUrls: ['./edit-platform-setting.component.scss'],
})
export class EditPlatformSettingComponent implements OnInit {
    settingForm: FormGroup;
    minimumValue: number = 1;
    maximumValue: number = 30;

    constructor(
        private _formBuilder: FormBuilder,
        public dialogRef: MatDialogRef<EditPlatformSettingComponent>,
        @Inject(MAT_DIALOG_DATA) public data: { setting: PlatformSetting }
    ) {}

    ngOnInit(): void {
        const translatedKey = translate(this.data.setting.setting_key);
        // Initialize form with setting data
        this.settingForm = this._formBuilder.group({
            id: [{ value: this.data.setting.id, disabled: true }],
            setting_key: [{ value: translatedKey, disabled: true }],
            setting_value: [
                this.data.setting.setting_value,
                [
                    Validators.required,
                    Validators.pattern('^[0-9]+(\\.[0-9]+)?$'),
                    Validators.min(this.minimumValue),
                    Validators.max(this.maximumValue),
                ],
            ],
        });
    }

    /**
     * Close the dialog
     */
    close(): void {
        this.dialogRef.close();
    }

    /**
     * Save the changes
     */
    save(): void {
        if (this.settingForm.invalid) {
            return;
        }

        // Create a copy of the setting instead of modifying the original reference
        const updatedSetting: PlatformSetting = {
            ...this.data.setting,
            setting_value: String(this.settingForm.value.setting_value),
        };

        this.dialogRef.close(updatedSetting);
    }
}
