export interface Client_Address {
    entity_id: string;
    address_id: string;
    address: string;
    locality: string;
    zip_code: string;
    zip_locality: string;
    phone: string;
    mobile_phone: string;
    email: string;
    updated_at: string;
    name: string;
    token: string;
}

export interface FilesToUpload {
    controlName: string;
    fileName: string;
    fileExtension: string;
    fileData: string;
}

export interface Exclusions {
    id: string;
    client_id: string;
    client_email: string;
    client_vat: string;
}

export interface Holidays {
    day: string;
    month: string;
    description: string;
}
