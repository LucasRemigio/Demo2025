// // Copyright (c) 2024 Engibots. All rights reserved.

using engimatrix.ModelObjs;
using engimatrix.Models;
using engimatrix.ResponseMessages;

namespace engimatrix.Views
{
    public class SyncPrimaveraStatsResponse : BaseResponse
    {
        public SyncPrimaveraStats statistics { get; set; }

        public SyncPrimaveraStatsResponse(SyncPrimaveraStats statistics, int result_code, string language) : base(result_code, language)
        {
            this.statistics = statistics;
        }

        public SyncPrimaveraStatsResponse(int result_code, string language) : base(result_code, language)
        {
        }
    }
}
