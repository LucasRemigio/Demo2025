import { Component, OnInit, ViewEncapsulation } from '@angular/core';
import {
    FormArray,
    FormBuilder,
    FormGroup,
    Validators,
} from '@angular/forms';
import { MAT_DIALOG_DATA, MatDialogRef } from '@angular/material/dialog';
import {
    Inject,
} from '@angular/core';
import {
    Combobox,
} from 'app/modules/order/order.types';

@Component({
    selector: 'audit.elem-action',
    templateUrl: './elem-action.component.html',
    encapsulation: ViewEncapsulation.None,
})
export class ElementActionComponent implements OnInit {
    composeForm: FormGroup;
    addresses: Combobox[] = [{ id: '-1', description: 'Sem moradas no ERP' }];
    products: Combobox[] = [
        { id: '-1', description: 'Sem produtos identificados' },
    ];

    constructor(
        public matDialogRef: MatDialogRef<ElementActionComponent>,
        private _formBuilder: FormBuilder,
        @Inject(MAT_DIALOG_DATA) public data: any
    ) {
        // alert(JSON.stringify(this.data));
        // alert(this._formBuilder.array[this.data.order.products]);
    }

    ngOnInit(): void {
        this.composeForm = this._formBuilder.group({
            id: this.data.order.id,
            // disabled: true,
            products: this._formBuilder.array([]),
            addresses: this._formBuilder.array([]),
            cancel_reason: ['', Validators.required]
        });

        if (this.data.order.products.length > 0) this.products = [];
        if (this.data.order.addresses.length > 0) this.addresses = [];

        var addGlasses: boolean = false;
        var addWater: boolean = false;
        var addMachine: boolean = false;
        var addBotlles: boolean = false;

        for (const field of this.data.order.products) {
            const validators = [];
            validators.push(Validators.required);
            this.products.push({
                id: field.id_product,
                description: field.description,
            });

            const newFormGroup = this._formBuilder.group({
                id_product: [
                    field.id_product,
                    // disabled: true

                    [
                        Validators.required,
                        Validators.minLength(2),
                        Validators.maxLength(100),
                    ],
                ],
                description: [
                    field.description,
                    [
                        Validators.required,
                        Validators.minLength(2),
                        Validators.maxLength(100),
                    ],
                ],
                quantity: [
                    field.quantity,
                    [
                        Validators.required,
                        Validators.minLength(1),
                        Validators.maxLength(100),
                    ],
                ],
                price: [
                    field.price,
                    [
                        Validators.required,
                        Validators.minLength(2),
                        Validators.maxLength(100),
                    ],
                ],
            });

            // IF ALREADY ADD GLASSES
            if (
                field.description.toLowerCase().includes('copos') &&
                !addGlasses
            ) {
                (this.composeForm.get('products') as FormArray).push(
                    newFormGroup
                );
                addGlasses = true;
            }

            // IF ALREADY ADD WATER BOTTLE
            if (
                field.description.toLowerCase().includes('garrafão') &&
                !addWater
            ) {
                (this.composeForm.get('products') as FormArray).push(
                    newFormGroup
                );
                addWater = true;
            }

            // IF ALREADY ADD  BOTLTE OF WATER
            if (field.description.toLowerCase().includes('água fonte viva')) {
                (this.composeForm.get('products') as FormArray).push(
                    newFormGroup
                );
            }

            // IF ALREADY ADD  BOTLTE OF WATER Tetra Pack
            if (field.description.toLowerCase().includes('tetrapack')) {
                (this.composeForm.get('products') as FormArray).push(
                    newFormGroup
                );
            }

            // IF ALREADY ADD MACHINE
            if (
                field.description.toLowerCase().includes('máquina') &&
                !addMachine
            ) {
                (this.composeForm.get('products') as FormArray).push(
                    newFormGroup
                );
                addMachine = true;
            }
        }

        for (const field of this.data.order.addresses) {
            this.addresses.push({
                id: field.id,
                description: field.description,
            });
        }

        const newFormGroup = this._formBuilder.group({
            id: [
                this.addresses[0].id,
                [
                    Validators.required,
                    Validators.minLength(2),
                    Validators.maxLength(100),
                ],
            ],
            description: [
                this.addresses[0].description,
                [
                    Validators.required,
                    Validators.minLength(2),
                    Validators.maxLength(100),
                ],
            ],
        });

        // Add the phase form group to the Phase form array
        (this.composeForm.get('addresses') as FormArray).push(newFormGroup);
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
    // saveAndClose(): void {
    //     const complaint = this.composeForm.controls['complaint'].value;

    //     // Close the dialog
    //     this.matDialogRef.close({complaint: complaint });
    // }
}
