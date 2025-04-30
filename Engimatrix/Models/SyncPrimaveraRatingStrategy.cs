// // Copyright (c) 2024 Engibots. All rights reserved.

using engimatrix.ModelObjs;
namespace engimatrix.Models;

public interface ISyncPrimaveraStrategy
{
    Task<SyncPrimaveraStats> Execute(string executerUser);
}

public class SyncPrimaveraCreditStrategy : ISyncPrimaveraStrategy
{
    public async Task<SyncPrimaveraStats> Execute(string executerUser)
    {
        return await SyncPrimaveraRatingsModel.SyncPrimaveraCreditRating(executerUser);
    }
}

public class SyncPrimaveraPaymentComplianceStrategy : ISyncPrimaveraStrategy
{
    public async Task<SyncPrimaveraStats> Execute(string executerUser)
    {
        return await SyncPrimaveraRatingsModel.SyncPrimaveraPaymentCompliances(executerUser);
    }
}

public class SyncPrimaveraHistoricalVolumeStrategy : ISyncPrimaveraStrategy
{
    public async Task<SyncPrimaveraStats> Execute(string executerUser)
    {
        return await SyncPrimaveraRatingsModel.SyncPrimaveraHistoricalVolumeRating(executerUser);
    }
}
