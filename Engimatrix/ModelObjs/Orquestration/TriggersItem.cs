// // Copyright (c) 2024 Engibots. All rights reserved.

namespace engimatrix.ModelObjs.Orquestration
{
    public class TriggersItem
    {
        public int id { get; set; }
        public string name { get; set; }
        public string cron_expression { get; set; }
        public string script_name { get; set; }

        public TriggersItem(int id, string name, string cron_expression, string script_name)
        {
            this.id = id;
            this.name = name;
            this.cron_expression = cron_expression;
            this.script_name = script_name;
        }

        public TriggersItem ToItem()
        {
            return new TriggersItem(this.id, this.name, this.cron_expression, this.script_name);
        }
    }
}
