export interface SignatureResponse {
    signature: Signature;
    result: string;
    result_code: number;
}

export interface Signature {
    user_id: string;
    signature: string;
}

export interface SimpleMessage {
  message: string;
  result: string;
  result_code: number;
}