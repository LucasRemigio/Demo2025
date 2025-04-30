using engimatrix.ModelObjs.Orquestration;
using engimatrix.ResponseMessages;

namespace engimatrix.Views.Orquestration
{
    public class RemoveScriptResponse
    {
        public List<ScriptsItem> scripts_items { get; set; }
        public string result { get; set; }
        public int result_code { get; set; }
        public RemoveScriptResponse(List<ScriptsItem> scriptsItems, int result_code, string language)
        {
            this.scripts_items = scriptsItems;
            this.result_code = result_code;
            this.result = ResponseMessage.GetResponseMessage(result_code, language);
        }
        public RemoveScriptResponse(int result_code, string language)
        {
            this.result_code = result_code;
            this.result = ResponseMessage.GetResponseMessage(result_code, language);
        }
    }
}
