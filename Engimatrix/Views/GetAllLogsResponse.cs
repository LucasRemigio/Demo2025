// // Copyright (c) 2024 Engibots. All rights reserved.

using engimatrix.ModelObjs;
using engimatrix.ResponseMessages;

namespace engimatrix.Views
{
    public class GetAllLogsResponse
    {
        public List<GetAllLogsItem> GetAllLogs_items { get; set; }
        public string result { get; set; }
        public int result_code { get; set; }

        public GetAllLogsResponse(List<GetAllLogsItem> GetAllLogsItems, int result_code, string language)
        {
            this.GetAllLogs_items = GetAllLogsItems;
            this.result_code = result_code;
            this.result = ResponseMessage.GetResponseMessage(result_code, language);
        }

        public GetAllLogsResponse(int result_code, string language)
        {
            this.result_code = result_code;
            this.result = ResponseMessage.GetResponseMessage(result_code, language);
        }
    }
}
