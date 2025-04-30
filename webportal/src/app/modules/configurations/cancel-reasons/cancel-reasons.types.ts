export interface CancelReasonsResponse {
    cancel_reasons: CancelReason[];
    result: string;
    result_code: number;
}

export interface CancelReason {
    id: number;
    reason: string;
    slug: string;
    description: string;
    is_active: boolean;
    created_at: string;
    updated_at: string;
}

export interface UpdateCancelReason {
    id: number;
    reason: string;
    description: string;
}

export interface CreateCancelReason {
    reason: string;
    description: string;
}

export interface ChangeCancelReasonActiveStatus {
    id: number;
    is_active: boolean;
}
