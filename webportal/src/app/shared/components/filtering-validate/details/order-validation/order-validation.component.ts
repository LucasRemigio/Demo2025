/* eslint-disable arrow-parens */
import { Location } from '@angular/common';
import {
    ChangeDetectorRef,
    Component,
    EventEmitter,
    Input,
    OnChanges,
    OnDestroy,
    OnInit,
    Output,
    ViewChild,
} from '@angular/core';
import { FormBuilder, FormGroup, Validators } from '@angular/forms';
import { MatDialog, MatDialogConfig } from '@angular/material/dialog';
import { Router, ActivatedRoute } from '@angular/router';
import { translate, TranslocoService } from '@ngneat/transloco';
import { GenericResponse } from 'app/modules/configurations/products/products.types';
import { FilteringService } from 'app/modules/filtering/filtering.service';
import { EMAIL_STATUSES } from 'app/modules/filtering/filtering.types';
import { OrderObservationsItem } from 'app/modules/orders/order.types';
import { OrderService } from 'app/shared/components/confirm-order-address/order.service';
import { EmailProductTableComponent } from 'app/shared/components/email-product-table/email-product-table.component';
import {
    ConfirmOrderResponse,
    GenerateInvoiceResponse,
    OrderDocument,
} from 'app/shared/components/email-product-table/products.types';
import { FlashMessageService } from 'app/shared/components/flash-message/flash-message.service';
import { GenericConfirmationPopupComponent } from 'app/shared/components/generic-confirmation-popup/generic-confirmation-popup.component';
import { SelectCancelReasonComponent } from '../../select-cancel-reason/select-cancel-reason.component';
import {
    OrderProductsDTO,
    OrderDTO,
    OrderRatingChangeRequestDto,
} from '../details.types';
import { ProposeNewOrderRatingsComponent } from './propose-new-order-ratings/propose-new-order-ratings.component';
import { ReplyProposedRatingsComponent } from './reply-proposed-ratings/reply-proposed-ratings.component';

@Component({
    selector: 'app-order-validation',
    templateUrl: './order-validation.component.html',
    styleUrls: ['./order-validation.component.scss'],
})
export class OrderValidationComponent implements OnInit, OnChanges, OnDestroy {
    @Input() order: OrderDTO;
    @Input() isDisabled: boolean = false;
    @Input() isPendingClientApproval: boolean = false;

    // The orderProducts are the products that are associated with the order
    // Or the current editing products from the products table
    @Input() orderProducts: OrderProductsDTO[];
    @Input() currentObservations: OrderObservationsItem;

    // the porpuse of this is to save the current changes if the user changes steps
    @Output() updateCurrentObservations =
        new EventEmitter<OrderObservationsItem>();
    @Output() invoiceGenerated = new EventEmitter<OrderDocument>();

    @ViewChild(EmailProductTableComponent)
    productTable!: EmailProductTableComponent;

    isLoading: boolean;
    isConfirmingOrder: boolean;
    alreadyValidatedOrderSteps: boolean = false;
    isOrderValid: boolean = false;
    ratingRequests: OrderRatingChangeRequestDto[];

    observationsForm: FormGroup;
    maxObservationsLength = 1000;
    maxContactLength = 255;

    constructor(
        private _fms: FlashMessageService,
        private _orderService: OrderService,
        private _router: Router,
        private _activatedRoute: ActivatedRoute,
        private _matDialog: MatDialog,
        private translocoService: TranslocoService,
        private _detectorRef: ChangeDetectorRef,
        private _formBuilder: FormBuilder,
        private _location: Location
    ) {}

    ngOnInit(): void {
        this.isOrderValid = this.validateOrder();

        this.ratingRequests = this.order.rating_change_requests;

        this.observationsForm = this._formBuilder.group({
            observations: [
                this.currentObservations.observations,
                [Validators.maxLength(this.maxObservationsLength)],
            ],
            contact: [
                this.currentObservations.contact,
                [Validators.maxLength(this.maxContactLength)],
            ],
        });

        if (this.isDisabled) {
            this.observationsForm.disable();
        }
    }

    ngOnChanges(): void {}

    ngOnDestroy(): void {
        this.updateCurrentObservations.emit({
            observations: this.observationsForm.get('observations').value,
            contact: this.observationsForm.get('contact').value,
        });
    }

    /* ==============================================================================
     *    Public Methods
     * ==============================================================================
     */

    /* --------------------------------------------------------------------------
     *   Confirm Order
     * --------------------------------------------------------------------------
     */

    async confirmProducts(): Promise<void> {
        this.isConfirmingOrder = true;
        this.isLoading = true;
        let error: boolean = false;
        try {
            this.isOrderValid = this.validateOrder();
            if (!this.isOrderValid) {
                return;
            }
            // Wait for updateProducts to complete
            const success = await this.productTable.updateProducts();

            if (!success) {
                // If the update failed, stop further processing
                // show error message
                this._fms.error('products-update-error');
                error = true;
                return;
            }
        } catch (exception) {
            return;
        } finally {
            if (error) {
                this.isLoading = false;
                this.isConfirmingOrder = false;
            }
        }

        const confirmationMessage =
            this.order.is_draft === true
                ? 'Order.confirm-quotation'
                : 'Order.confirm-order';

        // Confirmation popup before proceeding
        const dialogConfig: MatDialogConfig = {
            maxHeight: '50vh',
            minWidth: '40vh',
            data: {
                title: this.translocoService.translate('confirmation-required'),
                message:
                    this.translocoService.translate(confirmationMessage) + '?',
            },
        };

        const dialogRef = this._matDialog.open(
            GenericConfirmationPopupComponent,
            dialogConfig
        );

        dialogRef.afterClosed().subscribe((confirmed) => {
            if (confirmed) {
                this.generateInvoice();
                return;
            }

            this.isConfirmingOrder = false;
            this.isLoading = false;
            this._detectorRef.markForCheck();
        });
    }

    async generateInvoice(): Promise<void> {
        const observationsResponse = await this.patchObservations();

        if (observationsResponse.result_code < 0) {
            this._fms.error('Order.observations-error');
            return;
        }

        // if it is indeed an order, we must confirm it as this is our last step
        if (!this.order.is_draft) {
            const orderResponse = await this.confirmOrder();
            if (orderResponse.result_code < 0) {
                this._fms.error('Order.confirm-error');
                return;
            }
            this._fms.success('Order.confirm-success');
            this.invoiceGenerated.emit(orderResponse.document);
        } else {
            const quoteInvoiceResponse = await this.generateQuoteInvoice();
            if (quoteInvoiceResponse.result_code < 0) {
                this._fms.error('generate-document-error');
                return;
            }

            this._fms.success('generate-document-success');
            this.invoiceGenerated.emit(quoteInvoiceResponse.document);
        }
    }

    async patchObservations(): Promise<GenericResponse> {
        if (!this.observationsForm.valid) {
            this._fms.error('Order.observations-invalid');
            return;
        }

        const observations: OrderObservationsItem = {
            observations: this.observationsForm.get('observations').value,
            contact: this.observationsForm.get('contact').value,
        };

        try {
            const response = await this._orderService
                .patchObservations(observations, this.order.token)
                .toPromise();
            return response;
        } catch (exception) {
            this._fms.error('Order.observations-error');
        }
    }

    async confirmOrder(): Promise<ConfirmOrderResponse> {
        try {
            const response = await this._orderService
                .confirmOrder(this.order.token)
                .toPromise();

            return response;
        } catch (error) {
            this._fms.error('Order.confirm-error');
        }
    }

    async generateQuoteInvoice(): Promise<GenerateInvoiceResponse> {
        try {
            const response = await this._orderService
                .generateInvoice(this.order.token)
                .toPromise();

            return response;
        } catch (error) {
            this._fms.error('generate-document-error');
        }
    }

    /* --------------------------------------------------------------------------
     *   Manually Resolved
     * --------------------------------------------------------------------------
     */

    confirmManuallyResolve(): void {
        // first open the confirmation popup to make sure it was not a miss-click
        const dialogConfig: MatDialogConfig = {
            maxHeight: '50vh',
            minWidth: '40vh',
            data: {
                title: this.translocoService.translate('confirmation-required'),
                message: this.translocoService.translate(
                    'Email.confirm-mark-as-resolved'
                ),
            },
        };

        const dialogRef = this._matDialog.open(
            GenericConfirmationPopupComponent,
            dialogConfig
        );

        dialogRef.afterClosed().subscribe((confirmed) => {
            if (!confirmed) {
                // do nothing
                return;
            }
            // Change the status of the email to resolved
            this.manuallyResolve();
        });
    }

    manuallyResolve(): void {
        const statusId = EMAIL_STATUSES.RESOLVIDO_MANUALMENTE.id;

        this._orderService.patchStatus(this.order.token, statusId).subscribe(
            (response: any) => {
                if (response.result_code < 0) {
                    this._fms.error('email-status-change-error');
                    return;
                }
                this._fms.success('email-status-change-success');

                this._router.navigate(['../../validate'], {
                    relativeTo: this._activatedRoute,
                });
            },
            (error) => {
                this._fms.error('email-status-change-error');
            }
        );
    }

    /* --------------------------------------------------------------------------
     *   Cancel Order
     * --------------------------------------------------------------------------
     */

    cancelOrder(): void {
        // Open popup to select the cancel reason
        const dialogConfig: MatDialogConfig = {
            maxHeight: '50vh',
            minWidth: '40vh',
            data: {
                orderToken: this.order.token,
            },
        };

        const dialogRef = this._matDialog.open(
            SelectCancelReasonComponent,
            dialogConfig
        );

        dialogRef.afterClosed().subscribe((selectedCancelReasonId) => {
            if (!selectedCancelReasonId) {
                // do nothing
                return;
            }

            this._fms.success('Order.success-cancel');

            this._router.navigate(['../../validate'], {
                relativeTo: this._activatedRoute,
            });
        });
    }

    /* --------------------------------------------------------------------------
     *   Propose new ratings
     * --------------------------------------------------------------------------
     */

    proposeNewRatings(): void {
        // open popup with current order ratings for the operator to change
        const dialogConfig: MatDialogConfig = {
            maxHeight: '90vh',
            minHeight: '80vh',
            height: 'auto',
            maxWidth: '80vw',
            minWidth: '40vw',
            width: 'auto',
            data: {
                orderRatings: this.order.ratings,
                clientRatings: this.order.client.ratings,
            },
        };

        const dialogRef = this._matDialog.open(
            ProposeNewOrderRatingsComponent,
            dialogConfig
        );

        dialogRef.afterClosed().subscribe((newRatings) => {
            if (!newRatings) {
                // do nothing
                return;
            }

            this._fms.success('email-status-change-success');
            this._router.navigate(['../../validate'], {
                relativeTo: this._activatedRoute,
            });
        });
    }

    replyProposedRatings(): void {
        // we should only send the pending ones
        const pendingRequests = this.ratingRequests.filter(
            (req) => req.status === 'pending'
        );

        // there should be a popup where he will accept or reject each of the proposed rating
        const dialogConfig: MatDialogConfig = {
            maxHeight: '90vh',
            minHeight: '40vh',
            height: 'auto',
            maxWidth: '80vw',
            minWidth: '40vw',
            width: 'auto',
            data: {
                ratings: pendingRequests,
                orderRatings: this.order.ratings,
                clientRatings: this.order.client.ratings,
            },
        };

        const dialogRef = this._matDialog.open(
            ReplyProposedRatingsComponent,
            dialogConfig
        );

        dialogRef.afterClosed().subscribe((newRatings) => {
            if (newRatings) {
                // go to back page
                this._fms.success('reply-new-proposed-rating-success');
                this._router.navigate(['../../validate'], {
                    relativeTo: this._activatedRoute,
                });
            }
        });
    }

    async confirmClientAdjudicated(): Promise<void> {
        this.isConfirmingOrder = true;

        let error: boolean = false;
        try {
            // Wait for updateProducts to complete
            const success = await this.productTable.updateProducts();

            if (!success) {
                // If the update failed, stop further processing
                // show error message
                this._fms.error('products-update-error');
                error = true;
                return;
            }
        } catch (exception) {
            return;
        } finally {
            if (error) {
                this.isConfirmingOrder = false;
            }
        }

        this._orderService.adjudicateOrder(this.order.token).subscribe(
            (response) => {
                if (response.result_code < 0) {
                    this._fms.error('Order.created-error');
                    return;
                }
                this._fms.success('Order.created-success');
                setTimeout(() => {
                    this._router.navigate(['../../validate'], {
                        relativeTo: this._activatedRoute,
                    });
                    this.isConfirmingOrder = false;
                }, 2000);
            },
            (exception) => {
                this.isConfirmingOrder = false;
                this._fms.error('Order.created-error');
            }
        );
    }

    /* --------------------------------------------------------------------------
     *   Public Methods
     * --------------------------------------------------------------------------
     */

    validateOrder(): boolean {
        if (this.alreadyValidatedOrderSteps) {
            return this.isOrderValid;
        }
        this.alreadyValidatedOrderSteps = true;
        // for the order to be valid, it needs to have an associated client
        // and the address filled in, in case the is_delivery flag is setup
        // if not set, we need to make sure the operator went through that stage
        // also, if it is to confirm
        if (!this.order.client) {
            this._fms.error('order-no-client');
            return false;
        }

        if (!this.order.client.code) {
            this._fms.error('order-no-client');
            return false;
        }

        if (this.order.is_delivery) {
            if (!this.order.address) {
                this._fms.error('order-no-address');
                return false;
            }

            if (this.order.address === '') {
                this._fms.error('order-no-address');
                return false;
            }
        }

        return true;
    }

    get isFormValid(): boolean {
        return this.observationsForm.valid;
    }

    errorMessage(type: string): string {
        if (!type) {
            return '';
        }

        if (type !== 'observations' && type !== 'contact') {
            return '';
        }

        let length = 0;

        switch (type) {
            case 'observations':
                length = this.maxObservationsLength;
                break;
            case 'contact':
                length = this.maxContactLength;
                break;
            default:
                return '';
        }

        const message =
            translate(type) +
            ' ' +
            translate('x-must-be-lower-than') +
            ' ' +
            length +
            ' ' +
            translate('characters');

        return message;
    }
}
