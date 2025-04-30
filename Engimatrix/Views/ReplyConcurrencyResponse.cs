// // Copyright (c) 2024 Engibots. All rights reserved.

using engimatrix.ModelObjs;
using engimatrix.Models;
using engimatrix.ResponseMessages;

namespace engimatrix.Views
{
    public class ReplyConcurrencyResponse
    {
        public bool CanReply { get; set; } = true;
        public engimatrix.Models.ReplyInfo? ReplyInfo { get; set; }
        public string result { get; set; }
        public int result_code { get; set; }

        public ReplyConcurrencyResponse(bool canReply, engimatrix.Models.ReplyInfo replyInfo, int result_code, string language)
        {
            this.CanReply = canReply;
            this.ReplyInfo = replyInfo;
            this.result_code = result_code;
            this.result = ResponseMessage.GetResponseMessage(result_code, language);
        }

        public ReplyConcurrencyResponse(int result_code, string language)
        {
            this.result_code = result_code;
            this.result = ResponseMessage.GetResponseMessage(result_code, language);
        }
    }
}
