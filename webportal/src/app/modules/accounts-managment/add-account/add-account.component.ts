import {
    AfterViewInit,
    Component,
    OnInit,
    ViewChild,
    ViewEncapsulation,
} from '@angular/core';
import {
    FormBuilder,
    FormGroup,
    Validators,
    Form,
    FormArray,
} from '@angular/forms';
import { MatDialogRef } from '@angular/material/dialog';
import { SharedModule } from 'app/shared/shared.module';
import { TranslocoService } from '@ngneat/transloco';
import { Combobox } from 'app/modules/common/common.types';
import { RoleManagerComponent } from '../role-manager/role-manager.component';

@Component({
    selector: 'app-add-account',
    templateUrl: './add-account.component.html',
    encapsulation: ViewEncapsulation.None,
})
export class AddAccountComponent implements OnInit, AfterViewInit {
    @ViewChild(RoleManagerComponent) roleManager: RoleManagerComponent;

    composeForm: FormGroup;
    passPatternError: string = this._sharedModule.getPasswordValidatorError();

    roles: Combobox[] = [
        { id: '3', description: this.translocoService.translate('user', {}) },
        { id: '2', description: this.translocoService.translate('admin', {}) },
        {
            id: '4',
            description: this.translocoService.translate('supervisor', {}),
        },
    ];

    constructor(
        public matDialogRef: MatDialogRef<AddAccountComponent>,
        private _formBuilder: FormBuilder,
        private _sharedModule: SharedModule,
        private readonly translocoService: TranslocoService
    ) {}

    ngOnInit(): void {
        // Create the form
        this.composeForm = this._formBuilder.group({
            role: ['', [Validators.required]],
            name: ['', [Validators.required]],
            email: ['', [Validators.email, Validators.required]],
            password: [
                '',
                [
                    Validators.required,
                    Validators.pattern(
                        this._sharedModule.getPasswordValidatorPattern()
                    ),
                ],
            ],
        });
    }

    ngAfterViewInit(): void {
        // when admin, lock department roles
        this.composeForm
            .get('role')
            .valueChanges.subscribe((selectedRoleId) => {
                if (selectedRoleId == '2') {
                    this.roleManager.adminSelect();
                } else {
                    this.roleManager.userSelect();
                }
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
        if (this.composeForm.invalid) {
            return;
        }
        let role = this.composeForm.controls['role'].value;
        let name = this.composeForm.controls['name'].value;
        let email = this.composeForm.controls['email'].value;
        let password = this.composeForm.controls['password'].value;
        let departments = this.roleManager.getOptions();

        // Close the dialog
        this.matDialogRef.close({
            name: name,
            email: email,
            password: password,
            role: role,
            departments: departments,
        });
    }

    generateNewPass(): void {
        this.composeForm.controls['password'].setValue(
            this._sharedModule.generateNewPass()
        );
    }
}
