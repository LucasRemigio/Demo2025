using engimatrix.Utils;

namespace engimatrix.Views.Orquestration
{
    public class ScriptRequest
    {
        public class Add
        {
            public string name { get; set; }
            public string description { get; set; }
            public string main_file { get; set; }
            public string file_content { get; set; }

            public bool Validate()
            {
                if (!Util.IsValidInputString(this.name) || Util.IsValidInputString(this.main_file) || !Util.IsValidInputString(this.file_content))
                {
                    return false;
                }
                return true;
            }
        }

        public class Edit
        {
            public string id { get; set; }
            public string description { get; set; }
            public string main_file { get; set; }
            public string cron_job { get; set; }
            public string name { get; set; }
            public string file_content { get; set; }

            public bool Validate()
            {
                if (Util.IsValidInputString(this.id) || !Util.IsValidInputString(this.main_file) || !Util.IsValidInputEmail(this.cron_job))
                {
                    return false;
                }
                return true;
            }
        }
    }
}
