export interface Email {
    id: string;
    from: string;
    cc?: string | null;
    bcc?: string | null;
    to: string;
    subject: string;
    body: string;
    date: Date | string;
}

export interface Products {
    id_product?: string;
    description?: string;
    quantity?: string;
    price?: string;
    addressId?: string;
    is_selected?: boolean;
}
export interface Addresses {
    id: string;
    description?: string;
    is_selected?: boolean;
}

export interface Attachments {
    id_order?: string;
    file?: boolean;
}

export interface Category {
    id: number;
    title?: string;
    slug?: string;
}

export interface CardItem {
    id?: string;
    order_title?: string;
    order_slug?: string;
    order_description?: string;
    products: Products[];
    addresses: Addresses[];
    attachments: Attachments[];
    order_status?: string;
    order_category?: string;
    order_token?: string;
    order_internal_contact?: string;
    order_external_contact?: string;
    order_confirmedBy?: string;
    email_sender?: string;
    email_subject?: string;
    email_date?: string;
    email_content?: string;
    steps?: OrderSteps[];
    icon?: string;
    email_source: string;
    cancel_reason?: string;
    cancel_user?: string;
    cancel_category?: string;
}

export interface FwdOrder {
    order: CardItem;
    email_to: string;
    source_email: string;
}

export interface Combobox {
    id: string;
    description: string;
    addressId?: string;
    price?: string;
    flag?: boolean;
    quantity?: string;
}

export interface Dashboard {
    orders_count: string;
    orders_registered_count: string;
    orders_pending_confirmation_count: string;
    orders_manual_validation_count: string;
    orders_error_count: string;
    confidence_low_count: string;
    address_validation_count: string;
    products_validation_count: string;
    erp_no_data_count: string;
    pre_payment_count: string;
    exclusions_list_count: string;
    high_quantities_count: string;
    client_blocked: string;
    large_content: string;
    multiple_entries: string;
    client_report_count: string;
    client_approved_count: string;
    already_registered: string;
    manual_registered: string;
    waiting_pending_confirmation_count: string;
    waiting_manual_validation_count: string;
    reported_count: string;
    amounted_error_count: string;
    canceled_count: string;
    trust_history: Motnhs;
    orders_month_count: Motnhs;
    top_scoring: Scoring[];
    bottom_scoring: Scoring[];
    excel_base64: string;
    awaiting_customer_data: string;
    orders_to_validation_count: string;
}

export interface Base64Csv {
    csv_base64: string;
}

export interface Motnhs {
    january: string;
    february: string;
    march: string;
    april: string;
    may: string;
    june: string;
    july: string;
    august: string;
    september: string;
    october: string;
    november: string;
    december: string;
}

export interface Scoring {
    trust: string;
    orders_total: string;
    user: string;
}

export interface OrderSteps {
    order: string;
    title: string;
    subtitle: string;
    type: string;
}
