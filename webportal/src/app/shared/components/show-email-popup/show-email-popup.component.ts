/* eslint-disable arrow-parens */
import { DatePipe } from '@angular/common';
import {
    Component,
    Inject,
    Input,
    OnInit,
    Optional,
    ViewChild,
} from '@angular/core';
import { MatDialogRef, MAT_DIALOG_DATA } from '@angular/material/dialog';
import { MatDrawer } from '@angular/material/sidenav';
import { translate } from '@ngneat/transloco';
import { Email } from 'app/modules/common/common.types';
import { EmailAttachment } from 'app/modules/filtering/filtering.types';

@Component({
    selector: 'app-show-email-popup',
    templateUrl: './show-email-popup.component.html',
    styleUrls: ['./show-email-popup.component.scss'],
    providers: [DatePipe],
})
export class ShowEmailPopupComponent implements OnInit {
    @Input() email: Email;
    @Input() attachments: EmailAttachment[];

    @ViewChild('drawer') drawer: MatDrawer;
    drawerMode: 'over' | 'side' = 'side';
    drawerOpened: boolean = true;
    panels: any[] = [];
    selectedPanel: string = 'emailPanel';
    showPdfPanel: boolean = false;

    constructor(
        @Optional()
        @Inject(MAT_DIALOG_DATA)
        public data: { email: Email; attachments: EmailAttachment[] },
        @Optional() private _matDialogRef: MatDialogRef<ShowEmailPopupComponent>
    ) {
        if (data) {
            this.email = data.email;
            this.attachments = data.attachments;
        }
    }

    ngOnInit(): void {
        this.panels = [
            {
                id: 'emailPanel',
                icon: 'heroicons_outline:mail',
                title: translate('Email.preview'),
            },
            {
                id: 'attachmentPanel',
                icon: 'heroicons_outline:paper-clip',
                title: translate('Email.attachments'),
            },
        ];
    }

    togglePdfPanel(): void {
        // Alterna entre a exibição do painel de PDF
        this.showPdfPanel = !this.showPdfPanel;

        // Fecha o drawer no modo 'over'
        if (this.drawerMode === 'over') {
            this.drawer.close();
        }
    }

    goToPanel(panel: string): void {
        this.selectedPanel = panel;

        // Close the drawer on 'over' mode
        if (this.drawerMode === 'over') {
            this.drawer.close();
        }
    }

    getPanelInfo(id: string): any {
        return this.panels.find((panel) => panel.id === id);
    }

    close(): void {
        // Close the dialog
        this._matDialogRef.close();
    }
}
