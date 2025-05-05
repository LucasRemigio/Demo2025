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
            body: 'Solicito cotação e disponibilidade de stock para:\n\nObra ob240102\n15 - Varão red. 12 liso\n12 - Pranchetas 50*10\n1 - UPN 200 c/6.00 mts\n\nObra _Rio Este\n1 - UPN 120 c/7.00 mts\n1 - HEB 140 c/3.50 mts\n90 - Varão Redondo 20 liso\n30 - Pranchetas 50*10',
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
