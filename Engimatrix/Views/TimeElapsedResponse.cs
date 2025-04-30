// // Copyright (c) 2024 Engibots. All rights reserved.

using engimatrix.ModelObjs;
using engimatrix.Models;
using engimatrix.ResponseMessages;

namespace engimatrix.Views
{
    public class TimeElapsedResponse : BaseResponse
    {
        public long time_elapsed_ms { get; set; }

        public TimeElapsedResponse(long timeElapsedMs, int result_code, string language) : base(result_code, language)
        {
            time_elapsed_ms = timeElapsedMs;
        }

        public TimeElapsedResponse(int result_code, string language) : base(result_code, language)
        {
        }
    }
}
