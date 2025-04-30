// // Copyright (c) 2024 Engibots. All rights reserved.

using engimatrix.ModelObjs;
using engimatrix.ResponseMessages;

namespace engimatrix.Views;

public class CancelReasonResponseList
{
    public List<CancelReasonItem> cancel_reasons { get; set; }
    public string result { get; set; }
    public int result_code { get; set; }

    public CancelReasonResponseList(List<CancelReasonItem> cancel_reasons, int result_code, string language)
    {
        this.cancel_reasons = cancel_reasons;
        this.result_code = result_code;
        this.result = ResponseMessage.GetResponseMessage(result_code, language);
    }

    public CancelReasonResponseList(int result_code, string language)
    {
        this.result_code = result_code;
        this.result = ResponseMessage.GetResponseMessage(result_code, language);
    }
}

public class CancelReasonResponse
{
    public CancelReasonItem cancel_reason { get; set; }
    public string result { get; set; }
    public int result_code { get; set; }

    public CancelReasonResponse(CancelReasonItem cancel_reason, int result_code, string language)
    {
        this.cancel_reason = cancel_reason;
        this.result_code = result_code;
        this.result = ResponseMessage.GetResponseMessage(result_code, language);
    }

    public CancelReasonResponse(int result_code, string language)
    {
        this.result_code = result_code;
        this.result = ResponseMessage.GetResponseMessage(result_code, language);
    }
}

public class CancelReasonStatusUpdateRequest
{
    public bool is_active { get; set; }

    public bool IsValid()
    {
        return true;
    }
}
