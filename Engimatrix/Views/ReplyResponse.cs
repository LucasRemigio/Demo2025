// // Copyright (c) 2024 Engibots. All rights reserved.

using Engimatrix.ModelObjs;
using engimatrix.ResponseMessages;
using engimatrix.ModelObjs;

namespace Engimatrix.Views
{
    public class ReplyResponse
    {
        public class GetReplyResponse
        {
            public List<ReplyItem> replies { get; set; } = new List<ReplyItem>();
            public string result { get; set; }
            public int result_code { get; set; }

            public GetReplyResponse(List<ReplyItem> replies, int result_code, string language)
            {
                this.replies = replies;
                this.result_code = result_code;
                this.result = ResponseMessage.GetResponseMessage(result_code, language);
            }

            public GetReplyResponse(int result_code, string language)
            {
                this.result_code = result_code;
                this.result = ResponseMessage.GetResponseMessage(result_code, language);
            }
        }
    }
}
