using engimatrix.ResponseMessages;
using System.Collections.Generic;
using engimatrix.ModelObjs.Orquestration;

namespace engimatrix.Views.Orquestration
{
    public class GetScriptsResponse
    {
        public List<ScriptsItem> scripts_items { get; set; }
        public string result { get; set; }
        public int result_code { get; set; }

        public GetScriptsResponse(List<ScriptsItem> scriptsItems, int result_code, string language)
        {
            this.scripts_items = scriptsItems;
            this.result_code = result_code;
            this.result = ResponseMessage.GetResponseMessage(result_code, language);
        }

        public GetScriptsResponse(int result_code, string language)
        {
            this.result_code = result_code;
            this.result = ResponseMessage.GetResponseMessage(result_code, language);
        }
    }
}
