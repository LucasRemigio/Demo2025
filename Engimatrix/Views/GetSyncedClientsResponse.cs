// // Copyright (c) 2024 Engibots. All rights reserved.

using engimatrix.ModelObjs;
using engimatrix.ResponseMessages;

namespace engimatrix.Views
{
    public class GetSyncedClientsResponse
    {
        public int synced_clients { get; set; }
        public string result { get; set; }
        public int result_code { get; set; }

        public GetSyncedClientsResponse(int synced_clients, int result_code, string language)
        {
            this.synced_clients = synced_clients;
            this.result_code = result_code;
            this.result = ResponseMessage.GetResponseMessage(result_code, language);
        }

        public GetSyncedClientsResponse(int result_code, string language)
        {
            this.result_code = result_code;
            this.result = ResponseMessage.GetResponseMessage(result_code, language);
        }
    }
}
