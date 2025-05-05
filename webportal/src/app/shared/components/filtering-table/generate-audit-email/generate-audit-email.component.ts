import { Component, OnInit } from '@angular/core';
import { FormBuilder, FormGroup, Validators } from '@angular/forms';
import { MatDialogRef } from '@angular/material/dialog';
import { connectableObservableDescriptor } from 'rxjs/internal/observable/ConnectableObservable';

@Component({
    selector: 'app-generate-audit-email',
    templateUrl: './generate-audit-email.component.html',
    styleUrls: ['./generate-audit-email.component.scss'],
})
export class GenerateAuditEmailComponent implements OnInit {
    emailForm: FormGroup;

    emailTemplates: any[] = [
        {
            name: 1,
            body: 'Boa tarde,\n\nServe o presente para solicitar cotação para o seguinte material:\n\nFerro\n- Tubo Ø50x2 mm (+/- 222 ml);\n- Tubo 200x100x8 mm (+/- 48 ml);\n- Tubo 100x100x5 mm (+/- 24 ml);\n- Tubo 60x60x4 mm (+/- 96 ml);\n- Tubo 100x50x2 mm (+/- 132 ml);\n- Tubo 120x60x2 mm (+/- 18 m);\n- Tubo 60x30x3 mm (+/- 48 ml);\n- Varão Ø10 mm (+/- 1818 ml);\n- Cantoneira 100x100x10 mm (60 ml);\n- Cantoneira 60x60x6 mm (6 ml);\n- Cantoneira 65x65x6 mm (132 ml); Caso não comercialize esta dimensão pode propor dimensão acima.\n- Cantoneira 20x20x3 mm (+/- 6 ml);\n- Barra 250x5 mm (+/- 12 ml);\n- Barra 200x8 mm (+/-1356 ml);\n- Barra 60x15 mm (+/- 48 ml);\n- Barra 50x10 mm (+/- 270 ml);\n- Barra 100x10 mm (+/- 12 ml);\n- Chapa 3000x1500x3 mm (1 un);\n- Chapa 3000x1500x2 mm galvanizada (1 un);\n- Chapa 3000x1500x1,5 mm galvanizada (1 un);',
        },
        {
            name: 2,
            body: 'Solicito cotação e disponibilidade de stock para:\n\nObra ob240102\n15 - Varão red. 12 liso\n12 - Pranchetas 50*10\n1 - UPN 200 c/6.00 mts\n\nObra _Rio Este\n1 - UPN 120 c/7.00 mts\n1 - HEB 140 c/3.50 mts\n90 - Varão Redondo 20 liso\n30 - Pranchetas 50*10',
        },
        {
            name: 3,
            body: 'Na sequencia desta solicitação, agradeço cotação para o seguinte:\nFERRO PRETO:\n- TUBO 100X60X3 MM –6 METROS;\n- TUBO 40X40x3 MM – 6 METROS;\n- CHAPA 3000X1500X4 MM– 1 UN;\n- CHAPA 3000X1500X5 MM– 1 UN;\n- CHAPA 3000X1500X10 MM– 1 UN.',
        },
        {
            name: 4,
            body: 'Bom dia,\n\nEnvio encomenda do seguinte material:\n• 1 perfil HEB 160\n• 5 tubos 50x50x3mm preto\nObrigado pela atenção.',
        },
        {
            name: 5,
            body: 'Bom dia, \nVenho por este meio encomendar o seguinte material:\n\nEnc . 59\n-Prancheta 25x5 = 3un\n-Prancheta 25x10 = 1',
        },
    ];

    constructor(
        public matDialogRef: MatDialogRef<GenerateAuditEmailComponent>,
        private _formBuilder: FormBuilder
    ) {}

    ngOnInit(): void {
        // create the form with the email content
        this.emailForm = this._formBuilder.group({
            body: [
                '',
                [
                    Validators.required,
                    Validators.minLength(10),
                    Validators.maxLength(500),
                ],
            ],
        });
    }

    selectTemplate(templateBody: string): void {
        this.emailForm.get('body').setValue(templateBody);
    }

    close(): void {
        this.matDialogRef.close();
    }

    sendEmail(): void {
        if (this.emailForm.invalid) {
            this.emailForm.markAllAsTouched();
            return;
        }
        // close the dialog with the text
        const emailContent = this.emailForm.get('body')?.value;
        this.matDialogRef.close(emailContent);
    }
}
