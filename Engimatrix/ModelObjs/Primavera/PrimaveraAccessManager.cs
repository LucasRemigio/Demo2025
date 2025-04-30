// // Copyright (c) 2024 Engibots. All rights reserved.

using engimatrix.Utils;

namespace engimatrix.ModelObjs.Primavera;

public static class PrimaveraAccessManager
{
    private static readonly Dictionary<PrimaveraAccessType, PrimaveraAccessItem> Accounts
        = [];

    public static void AddOrUpdateAccess(PrimaveraAccessType type, PrimaveraAccessItem access)
    {
        Accounts[type] = access;
    }

    public static PrimaveraAccessItem? GetAccess(PrimaveraAccessType type)
    {
        return Accounts.TryGetValue(type, out PrimaveraAccessItem? access) ? access : null;
    }
}
