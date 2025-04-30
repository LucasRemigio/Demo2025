/* eslint-disable @typescript-eslint/naming-convention */
import { HttpClient, HttpParams } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { GenericResponse } from 'app/modules/configurations/products/products.types';
import { OrderObservationsItem } from 'app/modules/orders/order.types';
import { environment } from 'environments/environment';
import moment from 'moment';
import { Observable } from 'rxjs';
import {
    ConfirmOrderResponse,
    GenerateInvoiceResponse,
    OrderDocumentsResponse,
    OrderListResponse,
    OrderProductsItemResponse,
    OrderResponse,
    OrderToValidateResponse,
} from '../email-product-table/products.types';
import {
    OrderRatingDTO,
    ProposeNewRating,
    ReplyProposedRatings,
} from '../filtering-validate/details/details.types';
import {
    CttDistrictsResponse,
    CttMunicipalitiesResponse,
    CttMunicipality,
    CttPostalCodesResponse,
    CurrentAddress,
    TransportsResponse,
    UpdateOrderAddressResponse,
} from './confirm-order-address.types';

@Injectable({
    providedIn: 'root',
})
export class OrderService {
    constructor(private _httpClient: HttpClient) {}

    /**
     * Update order address
     */
    updateOrderAddress(
        address: CurrentAddress,
        orderToken: string
    ): Observable<UpdateOrderAddressResponse> {
        return this._httpClient.put<UpdateOrderAddressResponse>(
            environment.currrentBaseURL +
                '/api/orders/' +
                orderToken +
                '/address',
            address
        );
    }

    /*
     * Cancel order
     */
    cancelOrder(orderToken: string, cancelReasonId: number): Observable<any> {
        return this._httpClient.put(
            environment.currrentBaseURL + '/api/orders/cancel/' + orderToken,
            {
                cancel_reason_id: cancelReasonId,
            }
        );
    }

    /*
     * Check if order is already confirmed or canceled
     */
    checkOrderStatus(filteredToken: string): Observable<any> {
        return this._httpClient.get(
            environment.currrentBaseURL +
                '/api/filtering/' +
                filteredToken +
                '/checkConfirmed'
        );
    }

    /*
     * Confirm Order
     */
    confirmOrder(orderToken: string): Observable<ConfirmOrderResponse> {
        return this._httpClient.put<ConfirmOrderResponse>(
            environment.currrentBaseURL + '/api/orders/confirm/' + orderToken,
            {}
        );
    }

    /*
     * Client adjudicate order
     */
    adjudicateOrder(orderToken: string): Observable<any> {
        return this._httpClient.put(
            environment.currrentBaseURL +
                '/api/orders/no-auth/adjudicate/' +
                orderToken,
            {}
        );
    }

    /*
     * Generate invoice
     */
    generateInvoice(orderToken: string): Observable<GenerateInvoiceResponse> {
        return this._httpClient.post<GenerateInvoiceResponse>(
            environment.currrentBaseURL + '/api/orders/invoice/' + orderToken,
            {}
        );
    }

    /*
     * Patch observations and contact
     */
    patchObservations(
        observations: OrderObservationsItem,
        orderToken: string
    ): Observable<GenericResponse> {
        return this._httpClient.patch<GenericResponse>(
            environment.currrentBaseURL +
                `/api/orders/${orderToken}/observations`,
            observations
        );
    }

    /*
     * Edit order's client code
     */
    patchClient(
        orderToken: string,
        clientCode: string,
        nif: string
    ): Observable<OrderProductsItemResponse> {
        const params = {
            client_code: clientCode,
            client_nif: nif,
        };
        return this._httpClient.patch<OrderProductsItemResponse>(
            environment.currrentBaseURL +
                '/api/orders/' +
                orderToken +
                '/client',
            params
        );
    }

    /*
     * Get all transports
     */
    getTransports(): Observable<TransportsResponse> {
        return this._httpClient.get<TransportsResponse>(
            environment.currrentBaseURL + '/api/transports'
        );
    }

    /*
     * Propose new ratings
     */
    proposeNewRatings(
        ratings: ProposeNewRating[],
        orderToken: string
    ): Observable<GenericResponse> {
        return this._httpClient.post<GenericResponse>(
            environment.currrentBaseURL +
                `/api/orders/ratings/change-requests/${orderToken}`,
            ratings
        );
    }

    replyProposedRatings(
        ratings: ReplyProposedRatings[],
        orderToken: string
    ): Observable<GenericResponse> {
        return this._httpClient.patch<GenericResponse>(
            environment.currrentBaseURL +
                `/api/orders/ratings/change-requests/${orderToken}`,
            ratings
        );
    }

    /*
     * Get the districts
     */
    getCttDistricts(): Observable<CttDistrictsResponse> {
        return this._httpClient.get<CttDistrictsResponse>(
            environment.currrentBaseURL + '/api/ctt/districts'
        );
    }

    /*
     * Get the municipalities
     */
    getCttMunicipalities(dd: string): Observable<CttMunicipalitiesResponse> {
        return this._httpClient.get<CttMunicipalitiesResponse>(
            environment.currrentBaseURL + '/api/ctt/municipalities?dd=' + dd
        );
    }

    /*
     * get the postal codes
     */
    getCttPostalCodes(
        dd: string,
        cc: string
    ): Observable<CttPostalCodesResponse> {
        return this._httpClient.get<CttPostalCodesResponse>(
            environment.currrentBaseURL +
                '/api/ctt/postal-codes?dd=' +
                dd +
                '&cc=' +
                cc
        );
    }

    /*
     * Get order documents
     */
    getDocuments(orderToken: string): Observable<OrderDocumentsResponse> {
        return this._httpClient.get<OrderDocumentsResponse>(
            environment.currrentBaseURL + '/api/orders/documents/' + orderToken
        );
    }

    /*
     * Patch big transport, road does not allow it
     */
    patchBigTransport(orderToken: string): Observable<any> {
        return this._httpClient.patch(
            environment.currrentBaseURL +
                '/api/orders/' +
                orderToken +
                '/big-transport',
            {}
        );
    }

    createEmptyOrder(isDraft: boolean): Observable<OrderResponse> {
        const params = {
            is_draft: isDraft,
        };
        return this._httpClient.post<OrderResponse>(
            environment.currrentBaseURL + '/api/orders/empty',
            params
        );
    }

    getOrderByToken(token: string): Observable<OrderResponse> {
        return this._httpClient.get<OrderResponse>(
            environment.currrentBaseURL + `/api/orders/${token}`,
            {}
        );
    }

    getOrderByTokenToValidate(
        token: string
    ): Observable<OrderToValidateResponse> {
        return this._httpClient.get<OrderToValidateResponse>(
            environment.currrentBaseURL + `/api/orders/${token}/validate`,
            {}
        );
    }

    patchStatus(
        orderToken: string,
        statusId: number
    ): Observable<GenericResponse> {
        const params = {
            status_id: statusId,
        };
        return this._httpClient.patch<GenericResponse>(
            environment.currrentBaseURL + `/api/orders/${orderToken}/status`,
            params
        );
    }

    getOrdersToValidate(
        startDate: Date,
        endDate: Date,
        isDraft?: boolean,
        isPendingAdminApproval?: boolean,
        isPendingCreditApproval?: boolean
    ): Observable<OrderListResponse> {
        let params = new HttpParams()
            .set('start_date', moment(startDate).format('YYYY-MM-DD'))
            .set('end_date', moment(endDate).format('YYYY-MM-DD'));

        // Only add parameters if they're not null
        if (isDraft !== null && isDraft !== undefined) {
            params = params.set('is_draft', isDraft.toString());
        }

        // Conditionally add optional parameters
        if (
            isPendingAdminApproval !== null &&
            isPendingAdminApproval !== undefined
        ) {
            params = params.set(
                'is_pending_approval',
                isPendingAdminApproval.toString()
            );
        }

        if (
            isPendingCreditApproval !== null &&
            isPendingCreditApproval !== undefined
        ) {
            params = params.set(
                'is_pending_credit',
                isPendingCreditApproval.toString()
            );
        }

        return this._httpClient.get<OrderListResponse>(
            environment.currrentBaseURL + '/api/orders/validate',
            { params }
        );
    }

    getOrdersPendingClientConfirmation(
        startDate: Date,
        endDate: Date
    ): Observable<OrderListResponse> {
        const params = {
            start_date: moment(startDate).format('YYYY-MM-DD'),
            end_date: moment(endDate).format('YYYY-MM-DD'),
        };
        return this._httpClient.get<OrderListResponse>(
            environment.currrentBaseURL + '/api/orders/validate/pending-client',
            { params }
        );
    }

    getAuditOrders(
        isDraft: boolean,
        startDate: Date,
        endDate: Date
    ): Observable<OrderListResponse> {
        const params = {
            is_draft: isDraft,
            start_date: moment(startDate).format('YYYY-MM-DD'),
            end_date: moment(endDate).format('YYYY-MM-DD'),
        };
        return this._httpClient.get<OrderListResponse>(
            environment.currrentBaseURL + '/api/orders/audit',
            { params }
        );
    }

    /*
     * Patch observations and contact
     */
    setPendingClientConfirmation(
        orderToken: string
    ): Observable<GenericResponse> {
        return this._httpClient.put<GenericResponse>(
            environment.currrentBaseURL +
                `/api/orders/${orderToken}/pending-client-confirmation`,
            {}
        );
    }

    updateCredit(
        orderToken: string,
        accepted: boolean
    ): Observable<GenericResponse> {
        // add http params to the request

        return this._httpClient.patch<GenericResponse>(
            environment.currrentBaseURL +
                `/api/orders/${orderToken}/update-credit`,
            { accepted }
        );
    }
}
