// // Copyright (c) 2024 Engibots. All rights reserved.

using engimatrix.Utils;
using Engimatrix.ModelObjs;

namespace engimatrix.Views;
public class PlatformSettingUpdateRequest
{
    public string value { get; set; }

    public bool IsValid()
    {
        if (!Util.IsValidInputString(value))
        {
            return false;
        }

        return true;
    }
}
