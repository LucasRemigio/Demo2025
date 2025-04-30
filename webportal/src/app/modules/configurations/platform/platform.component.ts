/* eslint-disable arrow-parens */
/* eslint-disable @typescript-eslint/naming-convention */
/* eslint-disable quote-props */
import { ChangeDetectorRef, Component, OnInit } from '@angular/core';
import { MatDialog, MatDialogConfig } from '@angular/material/dialog';
import { FuseSplashScreenService } from '@fuse/services/splash-screen';
import { FlashMessageService } from 'app/shared/components/flash-message/flash-message.service';
import { finalize } from 'rxjs/operators';
import { EditPlatformSettingComponent } from './edit-platform-setting/edit-platform-setting.component';
import { PlatformSettingsService } from './platform-settings.service';
import { PlatformSetting, UpdateAppSetting } from './platform-settings.types';

@Component({
    selector: 'app-platform',
    templateUrl: './platform.component.html',
    styleUrls: ['./platform.component.scss'],
})
export class PlatformComponent implements OnInit {
    settings: PlatformSetting[];
    isLoading: boolean = false;

    constructor(
        private _platformSettingsService: PlatformSettingsService,
        private _fms: FlashMessageService,
        private _fuseSplashScreen: FuseSplashScreenService,
        private _cdr: ChangeDetectorRef,
        private _matDialog: MatDialog
    ) {}

    ngOnInit(): void {
        this.fetch();
    }

    fetch(): void {
        this._fuseSplashScreen.show();
        this.isLoading = true;
        this._platformSettingsService
            .getAllPlatformSettings()
            .pipe(
                finalize(() => {
                    this.isLoading = false;
                    this._fuseSplashScreen.hide();
                })
            )
            .subscribe(
                (response) => {
                    this.settings = response.platform_settings;
                },
                (error) => {
                    console.error('Error fetching platform settings:', error);
                    this._fms.error('error-loading-list');
                    // Here you could also display an error notification to the user
                }
            );
    }

    openSettingsDialog(setting?: PlatformSetting): void {
        // Configure dialog with dynamic title and behavior
        const dialogConfig: MatDialogConfig = {
            maxHeight: '60vh',
            minHeight: '20vh',
            height: 'auto',
            maxWidth: '60vw',
            minWidth: '30vw',
            width: 'auto',
            data: {
                setting: setting || null, // Pass null for "create" mode
            },
        };

        // Open the dialog dynamically based on mode
        const dialogRef = this._matDialog.open(
            EditPlatformSettingComponent,
            dialogConfig
        );

        dialogRef.afterClosed().subscribe((result: PlatformSetting) => {
            if (!result) {
                return;
            }

            const updateSetting: UpdateAppSetting = {
                id: result.id,
                setting_value: result.setting_value,
            };

            // Call your service to update the setting
            this._platformSettingsService
                .updatePlatformSettings(updateSetting)
                .subscribe(
                    (response) => {
                        // Update the local data
                        const updatingSetting = this.settings.find(
                            (s) => s.id === result.id
                        );
                        updatingSetting.setting_value = result.setting_value;
                        this._fms.success('setting-update-success');
                    },
                    (error) => {
                        this._fms.error('setting-update-error');
                    }
                );
        });
    }

    public getHeaderStyle(): any {
        return {
            'background-color': '#383838',
            color: 'white',
            'font-weight': 'bold',
        };
    }

    public isSmallScreen(): boolean {
        return window.innerWidth < 768;
    }

    public editSetting(setting: PlatformSetting): void {
        this.openSettingsDialog(setting);
    }
}
