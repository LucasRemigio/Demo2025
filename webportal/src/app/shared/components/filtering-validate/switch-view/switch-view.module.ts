import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';
import { SwitchViewComponent } from './switch-view.component';
import { MatIconModule } from '@angular/material/icon';
import { MatTooltipModule } from '@angular/material/tooltip';
import { TranslocoModule } from '@ngneat/transloco';

@NgModule({
    declarations: [SwitchViewComponent],
    imports: [CommonModule, TranslocoModule, MatTooltipModule, MatIconModule],
    exports: [SwitchViewComponent],
})
export class SwitchViewModule {}
