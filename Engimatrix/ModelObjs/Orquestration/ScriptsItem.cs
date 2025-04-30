using System.Transactions;

namespace engimatrix.ModelObjs.Orquestration
{
    public class ScriptsItem
    {
        public string id { get; set; }
        public string name { get; set; }
        public string description { get; set; }
        public string cron_job { get; set; }
        public string status { get; set; }
        public string url_script { get; set; }
        public string main_file { get; set; }

        public string script_update_last_time { get; set; }
        public string version { get; set; }
        public string last_execution { get; set; }
        public string nextRun { get; set; }

        public List<TriggersItem> Triggers { get; set; }


        public ScriptsItem(string id, string name, string description, string cron_job, string status, string url_script, string main_file, string script_update_last_time, string version, string last_execution, string nextRun, List<TriggersItem> triggers)
        {
            this.id = id;
            this.name = name;
            this.description = description;
            this.cron_job = cron_job;
            this.status = status;
            this.url_script = url_script; 
            this.main_file= main_file;
            this.script_update_last_time = script_update_last_time;
            this.version = version;
            this.last_execution = last_execution;
            this.nextRun = nextRun;
            this.Triggers = triggers;

        }
        public ScriptsItem ToScriptsItem()
        {
            return new ScriptsItem(this.id, this.name, this.description, this.cron_job, this.status, this.url_script, this.main_file, this.script_update_last_time, this.version, this.last_execution, this.nextRun, this.Triggers);
        }
    }
}
