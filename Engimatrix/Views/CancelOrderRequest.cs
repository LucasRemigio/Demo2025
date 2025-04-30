// // Copyright (c) 2024 Engibots. All rights reserved.

using engimatrix.ModelObjs;
using engimatrix.ResponseMessages;
using Microsoft.IdentityModel.Tokens;

namespace engimatrix.Views;

public class CancelOrderRequest
{
    public int cancel_reason_id { get; set; }

    public bool IsValid()
    {
        if (cancel_reason_id <= 0)
        {
            return false;
        }

        return true;
    }
}
