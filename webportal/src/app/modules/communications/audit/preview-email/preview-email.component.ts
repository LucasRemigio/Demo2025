import {
    Component,
    OnInit,
    Inject,
    ViewChild,
    OnDestroy,
    HostListener,
    ViewEncapsulation,
} from '@angular/core';
import { MatDialogRef, MAT_DIALOG_DATA } from '@angular/material/dialog';
import { TranslocoService } from '@ngneat/transloco';
import { MatDrawer } from '@angular/material/sidenav';
import { FuseSplashScreenService } from '@fuse/services/splash-screen';
import {
    EmailResponse,
    FilteringPopupData,
} from 'app/modules/filtering/filtering.types';
import { DomSanitizer, SafeHtml } from '@angular/platform-browser';
import 'pdfjs-dist/build/pdf.worker.entry';
import DOMPurify from 'dompurify';

@Component({
    selector: 'communications-preview-email',
    templateUrl: './preview-email.component.html',
    styleUrls: ['./preview-email.component.scss'],
    encapsulation: ViewEncapsulation.None,
    styles: [
        `
            .panel-title {
                padding-left: 2rem;
                padding-top: 2rem;
                padding-bottom: 2rem;
                box-sizing: border-box;
            }

            pdf-viewer {
                ::ng-deep {
                    .ng2-pdf-viewer-container {
                        height: -webkit-fill-available;
                        width: -webkit-fill-available;
                    }
                }
            }

            .drawer-content {
                width: 50rem;
            }

            @media (min-width: 320px) and (max-width: 960px) {
                .drawer-content {
                    width: auto;
                }
            }
        `,
    ],
})
export class PreviewCommunicationsEmailComponent implements OnInit {
    @ViewChild('drawer') drawer: MatDrawer;

    noAttachments: boolean;

    drawerMode: 'over' | 'side' = 'side';
    drawerOpened: boolean = true;
    panels: any[] = [];
    selectedPanel: string = 'emailPanel';
    showPdfPanel: boolean = false;

    sanitizedEmailBody: SafeHtml;

    constructor(
        public matDialogRef: MatDialogRef<PreviewCommunicationsEmailComponent>,
        private translocoService: TranslocoService,
        private _fuseSplashScreen: FuseSplashScreenService,
        private sanitizer: DomSanitizer,

        @Inject(MAT_DIALOG_DATA) public data: EmailResponse
    ) {}

    @HostListener('window:beforeunload', ['$event'])
    translate(key: string, params?: object): string {
        return this.translocoService.translate(key, params || {});
    }

    ngOnInit(): void {
        this._fuseSplashScreen.show();
        // Corrigindo o operador de atribuição para o operador de igualdade
        this.noAttachments = this.data.attachments.length === 0;

        const rawEmailHtml = this.data.email.body;
        // Sanitize the HTML content to make it safe for rendering
        this.sanitizedEmailBody =
            this.sanitizer.bypassSecurityTrustHtml(rawEmailHtml);

        this.panels = [
            {
                id: 'emailPanel',
                icon: 'heroicons_outline:mail',
                title: this.translate('Email.preview'),
            },
            {
                id: 'attachmentPanel',
                icon: 'heroicons_outline:paper-clip',
                title: this.translate('Email.attachments'),
            },
        ];

        this._fuseSplashScreen.hide();
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
        this.matDialogRef.close();
    }

    saveAndClose(): void {
        this.matDialogRef.close();
    }
}
