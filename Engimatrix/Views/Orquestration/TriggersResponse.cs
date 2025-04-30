// // Copyright (c) 2024 Engibots. All rights reserved.

using engimatrix.ModelObjs.Orquestration;
using engimatrix.ResponseMessages;

namespace engimatrix.Views.Orquestration
{
    public class TriggersResponse
    {
        public class GetTriggers
        {
            public List<TriggersItem> triggers { get; set; }
            public string result { get; set; }
            public int result_code { get; set; }

            public GetTriggers(List<TriggersItem> triggers, int result_code, string language)
            {
                this.triggers = triggers;
                this.result_code = result_code;
                this.result = ResponseMessage.GetResponseMessage(result_code, language);
            }

            // Constructor for errors and / o r empty results...
            public GetTriggers(int result_code, string language)
            {
                this.result_code = result_code;
                this.result = ResponseMessage.GetResponseMessage(result_code, language);
            }
        }
    }
}
