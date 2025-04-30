/* eslint-disable @typescript-eslint/naming-convention */
import { BaseResponse } from 'app/modules/configurations/clients/clients.types';
import { TransportItem } from '../filtering-validate/details/details.types';

export interface TransportsResponse extends BaseResponse {
    transports: TransportItem[];
}

export interface CurrentAddress {
    is_delivery?: boolean;
    district_dd?: string;
    municipality_cc?: string;
    postal_code_cp4?: string;
    postal_code_cp3?: string;
    address?: string;
    city?: string;
    transport_id?: number;
    maps_address?: string;
    distance?: number;
    travel_time?: number;
}

export interface CurrentMapsAddress {
    maps_address?: string;
    distance?: number;
    travel_time?: number;
}

export interface CttDistrictsResponse extends BaseResponse {
    ctt_districts: CttDistrict[];
}
export interface CttMunicipalitiesResponse extends BaseResponse {
    ctt_municipalities: CttMunicipality[];
}

export interface CttDistrict {
    dd: string;
    name: string;
}

export interface CttMunicipality {
    cc: string;
    district: CttDistrict;
    name: string;
}
export interface CttPostalCodesResponse extends BaseResponse {
    ctt_postal_codes: CttPostalCode[];
}

export interface CttPostalCode {
    id: number;
    municipality: CttMunicipality;
    llll: string;
    localidade: string;
    art_cod: string;
    art_tipo: string;
    pri_prep: string;
    art_titulo: string;
    seg_prep: string;
    art_desig: string;
    art_local: string;
    troco: string;
    porta: string;
    cliente: string;
    cp4: string;
    cp3: string;
    cpalf: string;
}
export interface UpdateOrderAddressResponse {
    destination_details: DestinationDetails;
    logistic_rating: OrderRatingItem;
    result: string;
    result_code: number;
}

export interface DestinationDetails {
    destination: string;
    distance: number;
    duration: number;
}

export interface OrderRatingItem {
    order_token: string;
    rating_type_id: number;
    rating: string;
    updated_at: null;
    updated_by: null;
    created_at: null;
    created_by: null;
}

interface Row {
    elements: Element[];
}

interface Element {
    distance: Distance;
    duration: Distance;
    duration_in_traffic: Distance;
    status: string;
}

interface Distance {
    text: string;
    value: number;
}

export interface CttDistrictsResponse extends BaseResponse {
    ctt_districts: CttDistrict[];
}
export interface CttMunicipalitiesResponse extends BaseResponse {
    ctt_municipalities: CttMunicipality[];
}

export interface CttDistrict {
    dd: string;
    name: string;
}

export interface CttMunicipality {
    cc: string;
    district: CttDistrict;
    name: string;
}
export interface CttPostalCodesResponse extends BaseResponse {
    ctt_postal_codes: CttPostalCode[];
}

export interface CttPostalCode {
    id: number;
    municipality: CttMunicipality;
    llll: string;
    localidade: string;
    art_cod: string;
    art_tipo: string;
    pri_prep: string;
    art_titulo: string;
    seg_prep: string;
    art_desig: string;
    art_local: string;
    troco: string;
    porta: string;
    cliente: string;
    cp4: string;
    cp3: string;
    cpalf: string;
}
