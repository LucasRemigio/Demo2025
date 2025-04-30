using engimatrix.ModelObjs.Orquestration;
using engimatrix.Utils;
using static engimatrix.Views.Orquestration.TriggersRequest;
using engimatrix.ModelObjs;

namespace engimatrix.Models.Orquestration
{
    public static class TriggersModel
    {
        public static List<TriggersItem> GetTriggers(string language, string user_operation)
        {
            List<TriggersItem> result = [];

            Dictionary<string, string> param = new()
            {
                { "@lang", language }
            };

            SqlExecuterItem responsive = SqlExecuter.ExecFunction(
                "SELECT t.id, t.name, t.cron_expression, (SELECT s.name FROM script s WHERE s.id = t.script_id) AS script_name " +
                "FROM triggers t",
                param, user_operation, true, "Get All Triggers");

            TriggersItem? trigger = null;

            foreach (Dictionary<string, string> item in responsive.out_data)
            {
                int id = Convert.ToInt32(item["0"]);
                string name = item["1"];
                string cron_expression = item["2"];
                string script_name = item["3"];

                trigger = new TriggersItem(id, name, cron_expression, script_name);
                result.Add(trigger.ToItem());
            }

            return result;
        }

        public static List<TriggersItem> GetTriggersByScriptId(string scriptId, string language, string executer_user)
        {
            List<TriggersItem> result = [];

            Dictionary<string, string> param = new()
            {
                { "@script_id", scriptId },
                { "@lang", language }
            };

            SqlExecuterItem responsive = SqlExecuter.ExecFunction("SELECT t.id, t.name, t.cron_expression, (SELECT s.name FROM script s WHERE s.id = t.script_id) AS script_name FROM triggers t WHERE t.script_id = @script_id", param, executer_user, false, "Get Triggers By Script Id");
            TriggersItem? trigger = null;

            foreach (Dictionary<string, string> item in responsive.out_data)
            {
                int id = Convert.ToInt32(item["0"]);
                string name = item["1"];
                string cron_expression = item["2"];
                string script_name = item["3"];

                trigger = new TriggersItem(id, name, cron_expression, script_name);
                result.Add(trigger.ToItem());
            }

            return result;
        }

        public static void AddTrigger(AddTrigger input, string executer_user)
        {
            Dictionary<string, string> param = new()
            {
                { "@name", input.name },
                { "@cron_expression", input.cron_expression },
                { "@script_name", input.script_name }
            };

            SqlExecuter.ExecFunction("INSERT INTO triggers (name, cron_expression, script_id) VALUES (@name, @cron_expression, (SELECT id FROM script WHERE name = @script_name))", param, executer_user, true, "Add Trigger");

            Log.Debug("Trigger " + input.name + " updated successfully.");

            ScriptsModel.InitializeTimer();

        }


        public static void EditTrigger(EditTrigger input, string executer_user)
        {
            Dictionary<string, string> param = new()
            {
                { "@id", input.id },
                { "@cron_expression", input.cron_expression }
            };

            // Use a subquery to get the script_id based on the script_name
            string query = "UPDATE triggers SET cron_expression = @cron_expression WHERE id = @id";

            SqlExecuter.ExecFunction(query, param, executer_user, true, "Edit Trigger");

            Log.Debug("Trigger " + input.name + " updated successfully.");

            ScriptsModel.InitializeTimer();
        }

        public static void RemoveTrigger(string triggerId, string executer_user)
        {
            Dictionary<string, string> param = new()
            {
                { "@id", triggerId }
            };

            SqlExecuter.ExecFunction("DELETE FROM triggers WHERE id = @id", param, executer_user, true, "Remove Trigger");

            Log.Debug($"Trigger {triggerId} removed successfully.");

            ScriptsModel.InitializeTimer();
        }
    }
}
