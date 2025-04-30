/* eslint-disable arrow-parens */
/* eslint-disable @typescript-eslint/naming-convention */
import {
    ChangeDetectorRef,
    Component,
    EventEmitter,
    Input,
    OnChanges,
    OnDestroy,
    OnInit,
    Output,
} from '@angular/core';
import { FormBuilder, FormGroup, Validators } from '@angular/forms';
import { MatSlideToggleChange } from '@angular/material/slide-toggle';
import { TransportItem } from '../filtering-validate/details/details.types';
import { FlashMessageService } from '../flash-message/flash-message.service';
import { AddressUpdateService } from './address-update.service';
import {
    CttDistrict,
    CttMunicipality,
    CttPostalCode,
    CurrentAddress,
    CurrentMapsAddress,
    DestinationDetails,
} from './confirm-order-address.types';
import { OrderService } from './order.service';

@Component({
    selector: 'app-confirm-order-address',
    templateUrl: './confirm-order-address.component.html',
    styleUrls: ['./confirm-order-address.component.scss'],
})
export class ConfirmOrderAddressComponent
    implements OnInit, OnDestroy, OnChanges
{
    @Input() address: CurrentAddress;
    @Input() orderToken: string;
    @Input() isDisabled: boolean = false;
    @Input() clientCar: string;
    @Input() isClient: boolean = false;

    @Output() addressChange = new EventEmitter<string>();
    @Output() postalCodeChange = new EventEmitter<string>();
    @Output() addressUpdated = new EventEmitter<CurrentAddress>();
    @Output() loadingStateChanged = new EventEmitter<boolean>();

    addressForm: FormGroup;
    transports: TransportItem[];
    selectedTransport: TransportItem = null;
    currentMapsAddress: CurrentMapsAddress | null = null;

    isLoading: boolean = false;

    addressPatternRegex =
        /^[a-zA-Z0-9\s,'\.\-áàâãäéèêëíìîïóòôõöúùûüçÁÀÂÃÄÉÈÊËÍÌÎÏÓÒÔÕÖÚÙÛÜÇ]{1,250}$/;
    postalCodeRegex = /^[0-9]{4}-[0-9]{3}$/;
    cityRegex = /^[a-zA-ZÀ-ÖØ-öø-ÿ'’\-.\s]{1,100}$/;

    districts: CttDistrict[];
    municipalities: CttMunicipality[];
    postalCodes: CttPostalCode[];
    uniqueCp4: string[];
    matchingCp3: string[];
    cp3sByCp4Map = new Map<string, string[]>();
    postalCodesMap = new Map<string, CttPostalCode[]>();
    availableStreets: string[];

    roadDoesNotHandleBigCar: boolean = false;

    constructor(
        private _orderService: OrderService,
        private _formBuilder: FormBuilder,
        private _changeDetectorRef: ChangeDetectorRef,
        private _flashMessageService: FlashMessageService,
        private _addressUpdateService: AddressUpdateService
    ) {}

    ngOnInit(): void {
        this._orderService.getCttDistricts().subscribe(
            (response) => {
                if (response.result_code < 1) {
                    this._flashMessageService.error('error-loading-districts');
                    return;
                }
                this.districts = response.ctt_districts;
                this._changeDetectorRef.markForCheck();
            },
            (error) => {
                this._flashMessageService.error('error-loading-districts');
            }
        );

        this.createForm();
        // it is needed to fill the address again as now, with the subscription to changes
        // they will be activating the events and fill the form correctly
        this.fillFormWithAddress(this.address);

        this.subscribeToChanges();
        this.fillCurrentMapsAddress();

        // Get the transport types
        this._orderService.getTransports().subscribe(
            (response) => {
                if (response.result_code < 1) {
                    this._flashMessageService.error('error-loading-transports');
                    return;
                }
                this.transports = response.transports;
                this._changeDetectorRef.markForCheck();
            },
            (error) => {
                this._flashMessageService.error('error-loading-transports');
            },
            () => {}
        );
    }

    ngOnChanges(): void {
        this.fillCurrentMapsAddress();
    }

    fillCurrentMapsAddress(): void {
        if (!this.address) {
            return;
        }

        // detect changes on the address input
        if (this.address.maps_address === undefined) {
            return;
        }

        const mapsAddress: CurrentMapsAddress = {
            maps_address: this.address.maps_address,
            distance: this.address.distance,
            travel_time: this.address.travel_time,
        };

        if (
            this.currentMapsAddress &&
            this.currentMapsAddress.maps_address === mapsAddress.maps_address &&
            this.currentMapsAddress.distance === mapsAddress.distance &&
            this.currentMapsAddress.travel_time === mapsAddress.travel_time
        ) {
            return;
        }

        this.currentMapsAddress = mapsAddress;
        this._changeDetectorRef.markForCheck();
    }

    subscribeToChanges(): void {
        this.addressForm
            .get('district_dd')
            ?.valueChanges.subscribe((selectedDd) => {
                this.onDistrictChanged(selectedDd);
            });

        this.addressForm
            .get('municipality_cc')
            ?.valueChanges.subscribe((selectedCc) => {
                this.onMunicipalityChanged(selectedCc);
            });

        this.addressForm
            .get('postal_code_cp4')
            ?.valueChanges.subscribe((selectedCp4) => {
                this.onPostalCodeCp4Changed(selectedCp4);
            });

        this.addressForm
            .get('postal_code_cp3')
            .valueChanges.subscribe((selectedCp3) => {
                this.onPostalCodeCp3Changed(selectedCp3);
            });

        this.addressForm
            .get('transport_id')
            ?.valueChanges.subscribe((selectedId) => {
                this.onTransportSelected(selectedId);
            });
    }

    async fillFormWithAddress(address: CurrentAddress): Promise<void> {
        if (!address) {
            return;
        }

        // for every field that is not null on the address, progressivly fill the form with them
        if (address.district_dd) {
            this.addressForm.get('district_dd').setValue(address.district_dd);

            await this.onDistrictChanged(address.district_dd);
        }

        if (address.municipality_cc) {
            this.addressForm
                .get('municipality_cc')
                .setValue(address.municipality_cc);

            await this.onMunicipalityChanged(address.municipality_cc);
        }

        if (address.postal_code_cp4) {
            this.addressForm
                .get('postal_code_cp4')
                .setValue(address.postal_code_cp4);

            this.onPostalCodeCp4Changed(address.postal_code_cp4);
        }

        // this can only be filled once the matching cp3 is filled
        if (address.postal_code_cp3) {
            this.addressForm
                .get('postal_code_cp3')
                .setValue(address.postal_code_cp3);
        }

        if (address.address) {
            this.addressForm.get('address').setValue(address.address);
        }
    }

    createForm(): void {
        // Initialize the form with validators
        this.addressForm = this._formBuilder.group({
            district_dd: [
                this.address?.district_dd || '',
                [Validators.required],
            ],
            municipality_cc: [
                this.address?.municipality_cc || '',
                [Validators.required],
            ],
            postal_code_cp4: [
                this.address?.postal_code_cp4 || '',
                [Validators.required, Validators.pattern(/^[0-9]{4}$/)],
            ],
            postal_code_cp3: [
                this.address?.postal_code_cp3 || '',
                [Validators.required, Validators.pattern(/^[0-9]{3}$/)],
            ],
            // This one is not used to send the request to the backend, only to show the city
            // that corresponds to the postal code address
            city: [
                this.address?.city || '',
                [Validators.required, Validators.pattern(this.cityRegex)],
            ],
            address: [
                this.address?.address || '',
                [
                    Validators.required,
                    Validators.pattern(this.addressPatternRegex),
                ],
            ],
            transport_id: [
                this.getValidOrderTransportId() || '',
                [Validators.required],
            ],
            big_car_allowed: [false],
        });

        // disable the whole address form if isDisabled
        if (this.isDisabled) {
            this.addressForm.disable();
        }
        // always disable the city field
        this.addressForm.get('city').disable();
    }

    getValidOrderTransportId(): number {
        if (this.address?.transport_id) {
            return this.address.transport_id;
        }
        if (!this.clientCar) {
            return null;
        }

        // check if the client car is a number
        const clientCarNumber = Number(this.clientCar);
        if (isNaN(clientCarNumber)) {
            return null;
        }

        return clientCarNumber;
    }

    get isShowToggleForBigCar(): boolean {
        const validCar = this.getValidOrderTransportId();
        if (!validCar) {
            return false;
        }

        // only show the toggle if the current car is 3
        if (validCar !== 3) {
            return false;
        }

        return true;
    }

    ngOnDestroy(): void {
        // Emit the latest form values on destroy
        const currentAddress = this.getCurrentAddress();

        this._addressUpdateService.emitAddressUpdate(currentAddress);
    }

    getCurrentAddress(): CurrentAddress {
        const currentAddress: CurrentAddress = {
            is_delivery: true,
            district_dd: this.addressForm.get('district_dd').value || null,
            municipality_cc:
                this.addressForm.get('municipality_cc').value || null,
            address: this.addressForm.get('address').value || null,
            postal_code_cp4:
                this.addressForm.get('postal_code_cp4').value || null,
            postal_code_cp3:
                this.addressForm.get('postal_code_cp3').value || null,
            city: this.addressForm.get('city').value || null,
            transport_id: this.addressForm.get('transport_id').value || null,
            maps_address: this.currentMapsAddress?.maps_address || null,
            distance: this.currentMapsAddress?.distance || null,
            travel_time: this.currentMapsAddress?.travel_time || null,
        };

        return currentAddress;
    }

    // Getter for form validity
    get isFormValid(): boolean {
        return this.addressForm.valid;
    }

    async updateOrderAddress(isDelivery: boolean = true): Promise<boolean> {
        const addressData = this.getValidAddress(isDelivery);

        return new Promise((resolve) => {
            this._orderService
                .updateOrderAddress(addressData, this.orderToken)
                .subscribe(
                    (response) => {
                        if (!response || response.result_code < 0) {
                            this._flashMessageService.error(
                                'order-address-invalid-format'
                            );
                            resolve(false);
                            return;
                        }

                        this._flashMessageService.success(
                            'order-address-updated'
                        );
                        resolve(true);
                        return;
                    },
                    (error) => {
                        this._flashMessageService.error(
                            'order-address-invalid-format'
                        );
                        resolve(false);
                    }
                );
        });
    }

    getValidAddress(isDelivery: boolean): CurrentAddress | null {
        let addressData: CurrentAddress;

        if (!isDelivery) {
            addressData = {
                is_delivery: false,
            };
            return;
        }

        if (this.addressForm.invalid) {
            this._flashMessageService.error('order-address-invalid-format');

            this.addressForm.markAllAsTouched();
            return null;
        }

        // Prepare the data
        addressData = this.getCurrentAddress();

        return addressData;
    }

    updateAddress(isDelivery: boolean = true): void {
        // check if there is any missing field
        if (this.addressForm.invalid) {
            this._flashMessageService.error('order-address-invalid-format');
            this.addressForm.markAllAsTouched();
            return;
        }

        const addressData = this.getValidAddress(isDelivery);

        if (!addressData) {
            this._flashMessageService.error('order-address-invalid-format');
            this.addressForm.markAllAsTouched();
            return;
        }

        this.isLoading = true;
        this.loadingStateChanged.emit(this.isLoading);
        this._orderService
            .updateOrderAddress(addressData, this.orderToken)
            .subscribe(
                (response) => {
                    if (!response || response.result_code < 0) {
                        this._flashMessageService.error(
                            'order-address-invalid-format'
                        );
                        this.isLoading = false;
                        return;
                    }

                    this._flashMessageService.success('order-address-updated');
                    this.addressUpdated.emit(addressData);
                    this._addressUpdateService.emitLogisticRatingUpdate(
                        response.logistic_rating
                    );
                    this.emitDestinationDetailsChanged(
                        response.destination_details
                    );
                    return;
                },
                (error) => {
                    this._flashMessageService.error(
                        'order-address-invalid-format'
                    );
                    this.isLoading = false;
                },
                () => {
                    this.isLoading = false;
                    this.loadingStateChanged.emit(this.isLoading);
                    this._changeDetectorRef.markForCheck();
                }
            );
    }

    emitDestinationDetailsChanged(destination: DestinationDetails): void {
        const currentMapsAddress: CurrentMapsAddress = {
            maps_address: destination.destination,
            distance: destination.distance,
            travel_time: destination.duration,
        };

        this._addressUpdateService.emitCurrentDestinationDetailsUpdate(
            currentMapsAddress
        );
    }

    onTransportSelected(selectedId: number): void {
        this.selectedTransport = this.transports.find(
            (transport) => transport.id === selectedId
        );
    }

    /* --------------------------------------
     * On change of District
     * --------------------------------------
     */

    async onDistrictChanged(selectedDd: string): Promise<void> {
        // first clean the municipality_cc
        this.addressForm.get('municipality_cc').setValue('');

        try {
            const response = await this._orderService
                .getCttMunicipalities(selectedDd)
                .toPromise();

            if (response.result_code < 1) {
                this._flashMessageService.error('error-loading-municipalities');
                return;
            }

            this.municipalities = response.ctt_municipalities;
            this._changeDetectorRef.markForCheck();
        } catch (error) {
            this._flashMessageService.error('error-loading-municipalities');
        }
    }

    /* --------------------------------------
     * On change of Municipality
     * --------------------------------------
     */

    async onMunicipalityChanged(selectedCc: string): Promise<void> {
        // selected cc coming empty means the user selected a new district which
        // invalidated the current municipalities, and for consequence the
        // postal codes
        this.cleanPostalCodes();

        if (!selectedCc || selectedCc === '') {
            return;
        }

        const dd = this.addressForm.get('district_dd').value;

        try {
            const response = await this._orderService
                .getCttPostalCodes(dd, selectedCc)
                .toPromise();

            if (response.result_code < 1) {
                this._flashMessageService.error('error-loading-postal-codes');
                return;
            }

            this.postalCodes = response.ctt_postal_codes;
            this.fillPostalCodes(this.postalCodes);
            this._changeDetectorRef.markForCheck();
        } catch (error) {
            this._flashMessageService.error('error-loading-postal-codes');
        }
    }

    cleanPostalCodes(): void {
        this.uniqueCp4 = [];
        this.cp3sByCp4Map.clear();
        this.postalCodesMap.clear();
        this.availableStreets = [];

        this.cleanCp4Forward();
    }

    cleanCp4Forward(): void {
        this.addressForm.get('postal_code_cp4').setValue('');
        this.cleanCp3Forward();
    }

    fillPostalCodes(postalCodes: CttPostalCode[]): void {
        // Reset everything
        this.uniqueCp4 = [];
        this.cp3sByCp4Map.clear();
        this.postalCodesMap.clear();

        // Loop over postalCodes once to build our maps and unique lists
        postalCodes.forEach((postalCode) => {
            const cp4 = postalCode.cp4;
            const cp3 = postalCode.cp3;
            const key = this.getPostalCodeKey(cp3, cp4);

            // Initialize map entry for cp4 if not present
            if (!this.cp3sByCp4Map.has(cp4)) {
                this.cp3sByCp4Map.set(cp4, []);
                this.uniqueCp4.push(cp4);
            }
            // Only add cp3 if it's not already present in the list for that cp4
            const cp3List = this.cp3sByCp4Map.get(cp4);
            if (!cp3List.includes(cp3)) {
                cp3List.push(cp3);
            }

            // Build the postal code map keyed by "cp4-cp3"
            if (!this.postalCodesMap.has(key)) {
                this.postalCodesMap.set(key, []);
            }
            this.postalCodesMap.get(key).push(postalCode);
        });

        // sort the cp4s list
        this.uniqueCp4.sort((a, b) => a.localeCompare(b));
    }

    getPostalCodeKey(cp3: string, cp4: string): string {
        return `${cp4}-${cp3}`;
    }

    /* --------------------------------------
     * On change of CP4
     * --------------------------------------
     */

    onPostalCodeCp4Changed(selectedCp4: string): void {
        if (!this.isCp4Valid(selectedCp4)) {
            return;
        }

        this.cleanCp3Forward();

        // remove the postal code cp4 errors
        this.addressForm.get('postal_code_cp4').setErrors(null);
        this._changeDetectorRef.markForCheck();

        // Update the available cp3 options based on the selected cp4
        this.matchingCp3 = this.cp3sByCp4Map.get(selectedCp4) || [];

        // sort the matching cp3s list
        this.matchingCp3.sort((a, b) => a.localeCompare(b));

        this._changeDetectorRef.markForCheck();
    }

    isCp4Valid(cp4: string): boolean {
        if (!cp4 || cp4 === '') {
            return false;
        }

        if (cp4.length < 4) {
            return false;
        }

        if (cp4.length > 4) {
            this.invalidateCp4();
            return false;
        }

        return true;
    }

    invalidateCp4(): void {
        this.addressForm.get('postal_code_cp4').setErrors({ invalid: true });
        this.cleanCp3Forward();
    }

    cleanCp3Forward(): void {
        this.addressForm.get('postal_code_cp3').setValue('');
        this.cleanCityForward();
    }

    /* --------------------------------------
     * On change of CP3
     * --------------------------------------
     */

    onPostalCodeCp3Changed(selectedCp3: string): void {
        if (!this.isCp3Valid(selectedCp3)) {
            return;
        }

        this.cleanCityForward();

        const selectedCp4 = this.addressForm.get('postal_code_cp4').value;
        const postalCodeKey = this.getPostalCodeKey(selectedCp3, selectedCp4);
        const postalCodeInfo = this.postalCodesMap.get(postalCodeKey);

        if (!postalCodeInfo || postalCodeInfo.length === 0) {
            console.error('Postal code not found for key ' + postalCodeKey);
            return;
        }

        const postalCode = postalCodeInfo[0];
        const locality = postalCode.cpalf + ', ' + postalCode.localidade;

        // Use the first matching postal code to set the city
        this.addressForm.get('city').setValue(locality);

        // populate available streets with the postal code info
        this.availableStreets = postalCodeInfo.map((pc) =>
            [pc.art_tipo, pc.pri_prep, pc.art_titulo, pc.seg_prep, pc.art_desig]
                .filter((part) => part && part.trim() !== '')
                .join(' ')
        );

        // order the streets
        this.availableStreets = this.availableStreets
            .filter((street) => street && street.trim() !== '')
            .sort((a, b) => a.localeCompare(b));
    }

    isCp3Valid(cp3: string): boolean {
        if (!cp3 || cp3 === '') {
            return false;
        }

        if (cp3.length < 3) {
            return false;
        }

        if (cp3.length > 3) {
            this.invalidateCp3();
            return false;
        }

        this.addressForm.get('postal_code_cp3').setErrors(null);
        return true;
    }

    invalidateCp3(): void {
        this.addressForm.get('postal_code_cp3').setErrors({ invalid: true });
        this.cleanCityForward();
    }

    cleanCityForward(): void {
        this.addressForm.get('city').setValue('');
        this.addressForm.get('address').setValue('');
        this.availableStreets = null;
    }

    onBigCarToggleChange(event: MatSlideToggleChange): void {
        this.roadDoesNotHandleBigCar = event.checked;
        this.patchBigCarAllowed(this.roadDoesNotHandleBigCar);
    }

    patchBigCarAllowed(isAllowed: boolean): void {
        if (!isAllowed) {
            return;
        }

        this._orderService.patchBigTransport(this.orderToken).subscribe(
            (response) => {
                if (!response || response.result_code < 0) {
                    this._flashMessageService.error(
                        'Order.big-transport-error'
                    );
                    return;
                }

                this._flashMessageService.success(
                    'Order.big-transport-success'
                );
                this.address.transport_id = 2;
                return;
            },
            (error) => {
                this._flashMessageService.error('Order.big-transport-error');
            }
        );
    }
}
