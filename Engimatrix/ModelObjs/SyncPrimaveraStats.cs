// // Copyright (c) 2024 Engibots. All rights reserved.

using Engimatrix.ModelObjs;

namespace engimatrix.ModelObjs
{
    public class SyncPrimaveraStats(long timeElapsedMs, int totalSyncs)
    {
        public long time_elapsed_ms { get; set; } = timeElapsedMs;
        public int total_syncs { get; set; } = totalSyncs;
    }
}