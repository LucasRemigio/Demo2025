import { ChangeDetectorRef, Component, Input, OnInit, ViewEncapsulation } from '@angular/core';
import { MAT_DIALOG_DATA, MatDialog, MatDialogConfig, MatDialogRef } from '@angular/material/dialog';
import { Inject } from '@angular/core';
import { HttpClient, HttpHeaders, HttpParams } from '@angular/common/http';
import { environment } from 'environments/environment';
import { FormBuilder, FormArray, FormGroup, Validators } from '@angular/forms';
import { Auth2F, SendNewAuth2fCode, User, ValidateAuth2f } from 'app/core/user/user.types';
import { Observable, interval, of, throwError, timer } from 'rxjs';
import { TranslocoService } from '@ngneat/transloco';
import { AuthService } from 'app/core/auth/auth.service';
import { UserService } from 'app/core/user/user.service';
import { ActivatedRoute } from '@angular/router';
import { FuseConfirmationService } from '@fuse/services/confirmation';
import { stringify } from 'crypto-js/enc-base64';
import { takeWhile } from 'rxjs/operators';
import { AccountManagmentEntry } from 'app/modules/accounts-managment/accounts-managment.types';

@Component({
  selector: 'app-auth2f',
  templateUrl: './auth2f.component.html',
  styleUrls: ['./auth2f.component.scss']
})
export class Auth2FComponent implements OnInit {
  disabled: boolean = true;
  flashMessage: 'success' | 'error' | null = null;
  flashMessageText: string;
  auth2fIsActive: boolean;
  clicked : boolean = false;
  timeLeft= 30;
  selectedAccount: AccountManagmentEntry | null = null;
  composeForm = this._formBuilder.group({
    auth2f_clientInput: '',
  });
  interval: NodeJS.Timeout;

  constructor(
      public matDialogRef: MatDialogRef<Auth2FComponent>,
      private _formBuilder: FormBuilder,
      private _changeDetectorRef: ChangeDetectorRef,
      private _authService: AuthService,
      private _httpClient: HttpClient,
      private _matDialog: MatDialog,
      @Inject(MAT_DIALOG_DATA) public data: any
  ) { 

  }

  ngOnInit(): void {
    this.composeForm = this._formBuilder.group({
        auth2f_clientInput: ['', [Validators.required, Validators.minLength(6), Validators.maxLength(6)]],
    });
    

  }

    // -----------------------------------------------------------------------------------------------------
    // @ Public methods
    // -----------------------------------------------------------------------------------------------------
    
  close(): void {
  // Close the dialog
    this.matDialogRef.close();
  }

  saveAndClose(): void {
    if (this.composeForm.controls['auth2f_clientInput'].errors) {
        return;
    }

    let auth2f_clientInput = this.composeForm.controls['auth2f_clientInput'].value;

    // Close the dialog
    this.matDialogRef.close({
      token: auth2f_clientInput
    });
  }

  sendNewAuth2fCode(): void {
      this._authService
          .sendNewAuth2fCode(this.data.email)
          .subscribe(
            (response) => {
                if (response && response.result_code > 0) {
                    (this.showFlashMessage(
                        'success', 'success'
                        ) + this.data.email
                    );            
                } else {
                  this.showFlashMessage('error', 'Error sending a new code. Please try again.');
                }
                (err) => {
                    this.showFlashMessage('error', 'Try again later');
                }
            },
        )
      }
  
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

  blockButton(): void {
    setTimeout(() => {
      this.clicked = false;
    }, 30000);

  }

}
