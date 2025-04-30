using engimatrix.ModelObjs.Orquestration;
using engimatrix.ResponseMessages;

namespace engimatrix.Views.Orquestration
{
    public class ScriptResponse
    {
        public class Get
        {
            public List<ScriptsItem> scripts_items { get; set; }
            public List<TriggersItem> triggers { get; set; }

            public string result { get; set; }
            public int result_code { get; set; }

            public Get(List<ScriptsItem> scriptsItems, List<TriggersItem> triggers, int result_code, string language)
            {
                this.scripts_items = scriptsItems;
                this.triggers = triggers;
                this.result_code = result_code;
                this.result = ResponseMessage.GetResponseMessage(result_code, language);
            }

            public Get(int result_code, string language)
            {
                this.result_code = result_code;
                this.result = ResponseMessage.GetResponseMessage(result_code, language);
            }
        }

    }
}
