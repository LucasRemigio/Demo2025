// // Copyright (c) 2024 Engibots. All rights reserved.

using engimatrix.Config;
using engimatrix.Connector;
using engimatrix.Exceptions;
using engimatrix.ModelObjs.Primavera;
using engimatrix.Utils;

namespace engimatrix.Models;

public static class PrimaveraClientModel
{
    private static Dictionary<string, MFPrimaveraClientItem> cachedPrimaveraClients = [];
    private static DateTime cacheExpirationDate = DateTime.MinValue;

    public static bool IsCacheValid()
    {
        if (cachedPrimaveraClients.Count <= 0)
        {
            return false;
        }

        if (DateTime.Now > cacheExpirationDate)
        {
            return false;
        }

        return true;
    }

    public static void InvalidateCache()
    {
        cachedPrimaveraClients.Clear();
        cacheExpirationDate = DateTime.MinValue;
    }

    public static async Task<Dictionary<string, MFPrimaveraClientItem>> GetPrimaveraClients()
    {
        if (IsCacheValid())
        {
            return cachedPrimaveraClients;
        }

        PrimaveraListResponseItem<MFPrimaveraClientItem> primaveraClients = await Primavera.GetListAsync<MFPrimaveraClientItem>(
            ConfigManager.PrimaveraUrls.MFClientes,
            999999,
            0,
            "1=1"
        );

        if (primaveraClients.IsError)
        {
            throw new PrimaveraApiErrorException(primaveraClients.Message!);
        }

        // Primavera, even though it might throw an error and not return anything, an
        // object on the list is always created, so we must check for 1
        if (primaveraClients.Data.Count <= 1)
        {
            throw new ResourceEmptyException("No clients found in Primavera");
        }

        // cache the primavera clients
        foreach (MFPrimaveraClientItem primaveraClient in primaveraClients.Data)
        {
            if (primaveraClient.Cliente == null)
            {
                continue;
            }

            cachedPrimaveraClients[primaveraClient.Cliente] = primaveraClient;
        }

        cacheExpirationDate = DateTime.Now.AddMinutes(30);

        return cachedPrimaveraClients;
    }

    public static async Task<MFPrimaveraClientItem?> GetPrimaveraClient(string clientCode)
    {
        Dictionary<string, MFPrimaveraClientItem> primaveraClients = await GetPrimaveraClients();

        if (primaveraClients.TryGetValue(clientCode, out var client))
        {
            return client;
        }

        Log.Debug($"PrimaveraClient with client code {clientCode} not found");
        return null;
    }
}
