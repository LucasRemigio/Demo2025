// // Copyright (c) 2024 Engibots. All rights reserved.

using engimatrix.ModelObjs;
using engimatrix.ResponseMessages;

namespace engimatrix.Views
{
    public class GetUsersResponse
    {
        public List<UserItem> user_items { get; set; } = new List<UserItem>();
        public string result { get; set; }
        public int result_code { get; set; }

        public GetUsersResponse(List<UserItem> userItems, int result_code, string language)
        {
            this.user_items = userItems;
            this.result_code = result_code;
            this.result = ResponseMessage.GetResponseMessage(result_code, language);
        }

        public GetUsersResponse(int result_code, string language)
        {
            this.result_code = result_code;
            this.result = ResponseMessage.GetResponseMessage(result_code, language);
        }
    }
}
