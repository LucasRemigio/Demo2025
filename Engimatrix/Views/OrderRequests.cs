// // Copyright (c) 2024 Engibots. All rights reserved.

using engimatrix.Config;
using engimatrix.ModelObjs;
using engimatrix.ResponseMessages;
using Microsoft.IdentityModel.Tokens;

namespace engimatrix.Views;

public class ChangeOrderStatusRequest
{
    public int status_id { get; set; }

    public bool IsValid()
    {
        if (status_id <= 0 || status_id > StatusConstants.ULTIMO_ID_VALIDO)
        {
            return false;
        }

        return true;
    }
}
