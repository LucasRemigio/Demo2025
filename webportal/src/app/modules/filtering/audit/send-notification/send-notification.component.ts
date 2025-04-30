import {
    ChangeDetectorRef,
    Component,
    Inject,
    OnInit,
    ViewEncapsulation,
} from '@angular/core';
import { FormBuilder, FormGroup, Validators, Form } from '@angular/forms';
import { MAT_DIALOG_DATA, MatDialogRef } from '@angular/material/dialog';
import { runInThisContext } from 'vm';
import { Observable, of } from 'rxjs';
import { TranslocoService } from '@ngneat/transloco';
import { OrderService } from 'app/modules/order/order.service';
import { environment } from 'environments/environment';


@Component({
    selector: 'app-send-notification',
    templateUrl: './send-notification.component.html',
    encapsulation: ViewEncapsulation.None,
})
export class SendNotificationComponent implements OnInit {
    composeForm: FormGroup;
    flashMessage: 'success' | 'error' | null = null;
    flashMessageText: string;
    email: string;

    constructor(
        private _changeDetectorRef: ChangeDetectorRef,
        public matDialogRef: MatDialogRef<SendNotificationComponent>,
        private _formBuilder: FormBuilder,
        private readonly translocoService: TranslocoService,
        @Inject(MAT_DIALOG_DATA) public data: any
    ) {
        // alert(JSON.stringify(this.data));
    }

    ngOnInit(): void {
        // Create the form
        this.composeForm = this._formBuilder.group({
            to: [
                this.data.order.email_sender,
                [
                    Validators.required,
                    Validators.minLength(2),
                    Validators.maxLength(100),
                ],
            ],
            subject: [
                'Culligan | Validação Formulário',
                [
                    Validators.required,
                    Validators.minLength(2),
                    Validators.maxLength(100),
                ],
            ],
            body: [
                'Bom dia, \n\nVerificámos que ainda não confirmou os dados da sua encomenda.\nAssim que for mais oportuno, pedimos que o faça. Por favor utilize o seguinte <a href=\'' + environment.currrentBaseURL + '/order/confirmation/' + this.data.order.order_token + '\'>link</a> \n\n(Mensagem automática) \n\nCom os melhores cumprimentos,\nCulligan',
                [
                    Validators.required,
                    Validators.minLength(1),
                    Validators.maxLength(1000),
                ],
            ],
        });
    }

    // -----------------------------------------------------------------------------------------------------
    // @ Public methods
    // -----------------------------------------------------------------------------------------------------

    close(): void {
        // Close the dialog
        this.matDialogRef.close();
    }

    /**
     * Save and close
     */
    saveAndClose(): void {
        if (this.composeForm.controls['to'].errors) {
            return;
        }

        if (this.composeForm.controls['subject'].errors) {
            return;
        }

        if (this.composeForm.controls['body'].errors) {
            return;
        }

        const to = this.composeForm.controls['to'].value;
        const subject = this.composeForm.controls['subject'].value;
        const body = this.composeForm.controls['body'].value;

        // Close the dialog
        this.matDialogRef.close({
            email: to,
            subject: subject,
            body: body,
        });
    }

    /**
     * Show flash message
     */
    showFlashMessage(type: 'success' | 'error', textMsg: string): void {
        // Show the message
        this.flashMessage = type;
        this.flashMessageText = textMsg;

        // Mark for check
        this._changeDetectorRef.markForCheck();

        // Hide it after 3 seconds
        setTimeout(() => {
            this.flashMessage = null;
            this.flashMessageText = null;

            // Mark for check
            this._changeDetectorRef.markForCheck();
        }, 5000);
    }
}
