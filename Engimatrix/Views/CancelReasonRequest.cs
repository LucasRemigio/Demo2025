// // Copyright (c) 2024 Engibots. All rights reserved.

using engimatrix.ModelObjs;
using engimatrix.ResponseMessages;
using Microsoft.IdentityModel.Tokens;

namespace engimatrix.Views;

public class CancelReasonCreateRequest
{
    public string reason { get; set; }
    public string description { get; set; }

    public bool IsValid()
    {
        if (String.IsNullOrEmpty(reason))
        {
            return false;
        }

        if (reason.Length > 50 || reason.Length < 5)
        {
            return false;
        }

        if (String.IsNullOrEmpty(description))
        {
            return false;
        }

        if (description.Length > 255)
        {
            return false;
        }

        return true;
    }
}

public class CancelReasonUpdateRequest
{
    public string reason { get; set; }
    public string description { get; set; }

    public bool IsValid()
    {
        if (String.IsNullOrEmpty(reason))
        {
            return false;
        }

        if (reason.Length > 50 || reason.Length < 5)
        {
            return false;
        }

        if (String.IsNullOrEmpty(description))
        {
            return false;
        }

        if (description.Length > 255)
        {
            return false;
        }

        return true;
    }
}
