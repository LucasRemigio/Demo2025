import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';
import { HttpClientModule } from '@angular/common/http';
import { MatButtonModule } from '@angular/material/button';
import { MatIconModule } from '@angular/material/icon';
import { MatProgressBarModule } from '@angular/material/progress-bar';
import { MatTooltipModule } from '@angular/material/tooltip';
import { TranslocoModule } from '@ngneat/transloco';
import { GridModule } from '@progress/kendo-angular-grid';
import { PlatformSettingsService } from './platform-settings.service';
import { PlatformComponent } from './platform.component';
import { EditPlatformSettingComponent } from './edit-platform-setting/edit-platform-setting.component';
import { EditPlatformSettingModule } from './edit-platform-setting/edit-platform-setting.module';

@NgModule({
    declarations: [PlatformComponent],
    imports: [
        CommonModule,
        MatProgressBarModule,
        MatIconModule,
        MatButtonModule,
        MatTooltipModule,
        GridModule,
        TranslocoModule,
        EditPlatformSettingModule,
    ],
    exports: [PlatformComponent],
})
export class PlatformModule {}
