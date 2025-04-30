// // Copyright (c) 2024 Engibots. All rights reserved.

using engimatrix.ModelObjs;
using engimatrix.Models;
using engimatrix.ResponseMessages;

namespace engimatrix.Views
{
    public class SegmentListResponse
    {
        public List<SegmentItem> segments { get; set; } 
        public string result { get; set; }
        public int result_code { get; set; }

        public SegmentListResponse(List<SegmentItem> segments, int result_code, string language)
        {
            this.segments = segments;
            this.result_code = result_code;
            this.result = ResponseMessage.GetResponseMessage(result_code, language);
        }

        public SegmentListResponse(int result_code, string language)
        {
            this.segments = [];
            this.result_code = result_code;
            this.result = ResponseMessage.GetResponseMessage(result_code, language);
        }
    }

    public class SegmentItemResponse
    {
        public SegmentItem segment { get; set; } 
        public string result { get; set; }
        public int result_code { get; set; }

        public SegmentItemResponse(SegmentItem segment, int result_code, string language)
        {
            this.segment = segment;
            this.result_code = result_code;
            this.result = ResponseMessage.GetResponseMessage(result_code, language);
        }

        public SegmentItemResponse(int result_code, string language)
        {
            this.segment = new();
            this.result_code = result_code;
            this.result = ResponseMessage.GetResponseMessage(result_code, language);
        }
    }
}
