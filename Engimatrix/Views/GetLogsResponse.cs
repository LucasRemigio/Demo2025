// // Copyright (c) 2024 Engibots. All rights reserved.

using engimatrix.ModelObjs;
using engimatrix.ResponseMessages;

namespace engimatrix.Views
{
    public class GetLogsResponse
    {
        public List<LogsItem> logs_items { get; set; }
        public string result { get; set; }
        public int result_code { get; set; }

        public GetLogsResponse(List<LogsItem> logsItems, int result_code, string language)
        {
            this.logs_items = logsItems;
            this.result_code = result_code;
            this.result = ResponseMessage.GetResponseMessage(result_code, language);
        }

        public GetLogsResponse(int result_code, string language)
        {
            this.result_code = result_code;
            this.result = ResponseMessage.GetResponseMessage(result_code, language);
        }
    }
}
