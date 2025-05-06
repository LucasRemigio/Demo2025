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
            body: '<p>Boa tarde,</p><p>Serve o presente para solicitar cotação para o seguinte material:</p><p>Ferro</p><ul><li>Tubo Ø50x2 mm (+/- 222 ml);</li><li>Tubo 200x100x8 mm (+/- 48 ml);</li><li>Tubo 100x100x5 mm (+/- 24 ml);</li><li>Tubo 60x60x4 mm (+/- 96 ml);</li><li>Tubo 100x50x2 mm (+/- 132 ml);</li><li>Tubo 120x60x2 mm (+/- 18 m);</li><li>Tubo 60x30x3 mm (+/- 48 ml);</li><li>Varão Ø10 mm (+/- 1818 ml);</li><li>Cantoneira 100x100x10 mm (60 ml);</li><li>Cantoneira 60x60x6 mm (6 ml);</li><li>Cantoneira 65x65x6 mm (132 ml); Caso não comercialize esta dimensão pode propor dimensão acima.</li><li>Cantoneira 20x20x3 mm (+/- 6 ml);</li><li>Barra 250x5 mm (+/- 12 ml);</li><li>Barra 200x8 mm (+/-1356 ml);</li><li>Barra 60x15 mm (+/- 48 ml);</li><li>Barra 50x10 mm (+/- 270 ml);</li><li>Barra 100x10 mm (+/- 12 ml);</li><li>Chapa 3000x1500x3 mm (1 un);</li><li>Chapa 3000x1500x2 mm galvanizada (1 un);</li><li>Chapa 3000x1500x1,5 mm galvanizada (1 un);</li></ul>',
        },
        {
            name: 2,
            body: '<p>Solicito cotação e disponibilidade de stock para:</p><p><strong>Obra ob240102</strong></p><ul><li>15 - Varão red. 12 liso</li><li>12 - Pranchetas 50*10</li><li>1 - UPN 200 c/6.00 mts</li></ul><p><strong>Obra _Rio Este</strong></p><ul><li>1 - UPN 120 c/7.00 mts</li><li>1 - HEB 140 c/3.50 mts</li><li>90 - Varão Redondo 20 liso</li><li>30 - Pranchetas 50*10</li></ul>',
        },
        {
            name: 3,
            body: '<p>Na sequencia desta solicitação, agradeço cotação para o seguinte:</p><p><strong>FERRO PRETO:</strong></p><ul><li>TUBO 100X60X3 MM –6 METROS;</li><li>TUBO 40X40x3 MM – 6 METROS;</li><li>CHAPA 3000X1500X4 MM– 1 UN;</li><li>CHAPA 3000X1500X5 MM– 1 UN;</li><li>CHAPA 3000X1500X10 MM– 1 UN.</li></ul>',
        },
        {
            name: 4,
            body: '<p>Bom dia,</p><p>Envio encomenda do seguinte material:</p><ul><li>1 perfil HEB 160</li><li>5 tubos 50x50x3mm preto</li></ul><p>Obrigado pela atenção.</p>',
        },
        {
            name: 5,
            body: '<p>Bom dia,</p><p>Venho por este meio encomendar o seguinte material:</p><p>Enc . 59</p><ul><li>Prancheta 25x5 = 3un</li><li>Prancheta 25x10 = 1</li></ul>',
        },
    ];

    quillModules = {
        toolbar: [
            ['bold', 'italic', 'underline'],
            ['blockquote', 'code-block'],
            [{ list: 'ordered' }, { list: 'bullet' }],
            [{ indent: '-1' }, { indent: '+1' }],
            [{ header: [1, 2, 3, 4, 5, 6, false] }],
            [{ color: [] }, { background: [] }],
            [{ align: [] }],
        ],
    };

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
                    Validators.maxLength(5000),
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
            console.log(this.emailForm);
            this.emailForm.markAllAsTouched();
            return;
        }
        // close the dialog with the text
        const emailContent = this.emailForm.get('body')?.value;
        this.matDialogRef.close(emailContent);
    }
}
