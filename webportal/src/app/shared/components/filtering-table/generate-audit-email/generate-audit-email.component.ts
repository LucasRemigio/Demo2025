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
        console.log(emailContent);
        this.matDialogRef.close(emailContent);
    }
}
