// // Copyright (c) 2024 Engibots. All rights reserved.

using engimatrix.ModelObjs;
using engimatrix.Models;
using engimatrix.ResponseMessages;

namespace engimatrix.Views
{
    public class TransportListResponse : BaseResponse
    {
        public List<TransportItem>? transports { get; set; }

        public TransportListResponse(List<TransportItem> transports, int result_code, string language) : base(result_code, language)
        {
            this.transports = transports;
        }

        public TransportListResponse(int result_code, string language) : base(result_code, language)
        {
        }
    }

    public class TransportItemResponse : BaseResponse
    {
        public TransportItem? transport { get; set; }

        public TransportItemResponse(TransportItem transport, int result_code, string language) : base(result_code, language)
        {
            this.transport = transport;
        }

        public TransportItemResponse(int result_code, string language) : base(result_code, language)
        {
        }
    }
}
