import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';
import { FlashMessageComponent } from './flash-message.component';
import { FuseAlertModule } from '@fuse/components/alert';

@NgModule({
    declarations: [FlashMessageComponent],
    imports: [CommonModule, FuseAlertModule],
    exports: [FlashMessageComponent],
})
export class FlashMessageModule {}
