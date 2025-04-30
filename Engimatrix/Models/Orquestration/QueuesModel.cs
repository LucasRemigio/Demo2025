// // Copyright (c) 2024 Engibots. All rights reserved.

using engimatrix.Config;
using engimatrix.ModelObjs;
using engimatrix.ModelObjs.Orquestration;
using engimatrix.Utils;
using engimatrix.Views.Orquestration;
using static engimatrix.Views.Orquestration.TransactionsRequest;
using System.Globalization;

namespace engimatrix.Models.Orquestration
{
    public static class QueuesModel
    {
        public static List<QueuesItem> GetQueues(string language, string user_operation)
        {
            List<QueuesItem> result = [];

            Dictionary<string, string> param = [];

            SqlExecuterItem responsive = SqlExecuter.ExecFunction(
                "SELECT q.id, q.name, q.description, q.autoRetry, q.numberRetry, q.status_id, (SELECT s.name FROM script s WHERE s.id = q.id_script) AS script_name," +
                "(SELECT COUNT(*) FROM transactions t WHERE status_id = 3 AND t.queue_id = q.id) AS successCount, " +
                "(SELECT COUNT(*) FROM transactions t WHERE status_id = 4 AND t.queue_id = q.id) AS insuccessCount " +
                "FROM queues_script q WHERE q.status_id = 1",
                param, user_operation, true, "Get All Queues");

            QueuesItem rec = null;

            foreach (Dictionary<string, string> item in responsive.out_data)
            {
                int id = Convert.ToInt32(item["0"]);
                string name = item["1"];
                string description = item["2"];
                string autoRetry = item["3"];
                int numberRetry = Convert.ToInt32(item["4"]);
                int status_id = Convert.ToInt32(item["5"]);
                string script_name = item["6"];
                string time = "0";
                string successCount = item["7"];
                string insuccessCount = item["8"];
                string sysException = "0";
                string busException = "0";

                if (ConfigManager.isProduction)
                {
                    description = Cryptography.Decrypt(description, ConfigManager.generalKey);

                }

                rec = new QueuesItem(id, name, description, autoRetry, numberRetry, status_id, script_name, time, successCount, insuccessCount, sysException, busException, getTransactionByQueueId(id.ToString(), language, user_operation));

                result.Add(rec.ToItem());
            }

            return result;
        }

        public static bool remove(string id, string user_operation)
        {
            Dictionary<string, string> param = new()
            {
                { "@id", id }
            };

            SqlExecuter.ExecFunction("UPDATE queues_script SET status_id = 2 WHERE id = @id", param, user_operation, true, "Update Queue");

            Log.Debug("Queue " + id + " removed successfully.");

            return true;
        }

        public static bool edit(QueuesRequest.Edit input, string user_operation)
        {

            if (ConfigManager.isProduction)
            {
                input.description = Cryptography.Encrypt(input.description, ConfigManager.generalKey);
            }

            Dictionary<string, string> param = new()
            {
                { "@id", input.id },
                { "@name", input.name },
                { "@description", input.description },
                { "@autoRetry", input.autoRetry ? "1" : "0" },
                { "@numberRetry", input.numberRetry.ToString() }
            };

            SqlExecuter.ExecFunction("UPDATE queues_script SET name = @name, description = @description, autoRetry = @autoRetry, numberRetry= @numberRetry WHERE id = @id", param, user_operation, true, "Edit Queue");

            Log.Debug("Queue " + input.name + " edited successfully.");

            return true;
        }

        public static bool Add(QueuesRequest.Add input, string user_operation)
        {

            if (ConfigManager.isProduction)
            {
                input.description = Cryptography.Encrypt(input.description, ConfigManager.generalKey);
            }

            Dictionary<string, string> param = new()
            {
                { "@name", input.name },
                { "@description", input.description },
                { "@autoRetry", input.autoRetry ? "1" : "0" },
                { "@numberRetry", input.numberRetry.ToString() },
                { "@status_id", StatusConstants.StatusCode.ATIVO.ToString() },
                { "@script_name", input.script_name }
            };

            SqlExecuter.ExecFunction("INSERT INTO queues_script (name, description, autoRetry, numberRetry, status_id, id_script) VALUES (@name, @description, @autoRetry, @numberRetry, @status_id, (SELECT id FROM script WHERE name = @script_name))", param, user_operation, false, "Add Queue");

            Log.Debug("Queue " + input.name + " added successfully.");

            return true;
        }

        public static List<TransactionsItem> getTransactionByQueueId(string queueId, string language, string user_operation)
        {
            List<TransactionsItem> result = [];

            Dictionary<string, string> param = new()
            {
                { "@queue_id", queueId },
                { "@status_id", Config.StatusConstants.StatusCode.APAGADO.ToString() },
                { "@lang", language }
            };

            SqlExecuterItem responsive = SqlExecuter.ExecFunction("SELECT t.id, GetLabelDescription(@lang, s.description) as status, t.reference, t.started, t.ended, t.exception, t.queue_id, t.input_data, t.output_data, q.numberRetry FROM transactions t JOIN queues_script q ON t.queue_id = q.ID JOIN status s ON t.status_id = s.id WHERE queue_id = @queue_id and t.status_id not in(@status_id) ORDER BY t.id DESC", param, user_operation, false, "Get Transaction by Queue");
            TransactionsItem rec = null;

            foreach (Dictionary<string, string> item in responsive.out_data)
            {
                int id = Convert.ToInt32(item["0"]);
                string status = item["1"];
                string reference = item["2"];
                string started = item["3"];
                string ended = item["4"];
                string exception = item["5"];
                int queue_id = Convert.ToInt32(item["6"]);
                string input_data = item["7"];
                string output_data = item["8"];
                int numberRetry = Convert.ToInt32(item["9"]);

                if (ConfigManager.isProduction)
                {
                    input_data = Cryptography.Decrypt(input_data, ConfigManager.generalKey);
                    output_data = Cryptography.Decrypt(output_data, ConfigManager.generalKey);
                }

                rec = new TransactionsItem(id, status, reference, started, ended, exception, queue_id, input_data, output_data, numberRetry);
                result.Add(rec.ToItem());
            }

            return result;
        }

        public static List<TransactionsItem> getTransactionByQueueName(string queue_name, string language, string user_operation)
        {
            List<TransactionsItem> result = [];

            Dictionary<string, string> param = new()
            {
                { "@queue_name", queue_name },
                { "@lang", language },
                { "@status_id", Config.StatusConstants.StatusCode.NOVO.ToString() }
            };

            ScriptsItem script = ScriptsModel.GetScriptByQueueName(queue_name, user_operation, language);

            SqlExecuterItem responsive = SqlExecuter.ExecFunction("SELECT t.id, GetLabelDescription(@lang, s.description) as status, t.reference, t.started, t.ended, t.exception, t.queue_id, t.input_data, t.output_data, q.numberRetry FROM transactions t JOIN queues_script q ON t.queue_id = q.ID JOIN status s ON t.status_id = s.id WHERE q.name = @queue_name AND t.status_id = @status_id", param, user_operation, false, "Get Transaction by Queue Name");

            TransactionsItem rec = null;

            foreach (Dictionary<string, string> item in responsive.out_data)
            {
                int id = Convert.ToInt32(item["0"]);
                string status = item["1"];
                string reference = item["2"];
                string started = item["3"];
                string ended = item["4"];
                string exception = item["5"];
                int queue_id = Convert.ToInt32(item["6"]);
                string input_data = item["7"];
                string output_data = item["8"];
                int numberRetry = Convert.ToInt32(item["9"]);

                if (ConfigManager.isProduction)
                {
                    input_data = Cryptography.Decrypt(input_data, ConfigManager.generalKey);
                    output_data = Cryptography.Decrypt(output_data, ConfigManager.generalKey);
                }

                rec = new TransactionsItem(id, status, reference, started, ended, exception, queue_id, input_data, output_data, numberRetry, script.name);
                result.Add(rec.ToItem());
            }

            return result;
        }

        public static bool removeTransaction(string id, string user_operation)
        {

            Dictionary<string, string> param = new()
            {
                { "@ID", id },
                { "@newStatusCode", Config.StatusConstants.StatusCode.APAGADO.ToString() }
            };

            SqlExecuter.ExecFunction("UPDATE transactions SET status_id = @newStatusCode WHERE id = @ID", param, user_operation, true, "Remove Transaction");

            Log.Debug("Transaction " + id + " removed successfully.");

            return true;
        }

        public static bool removeTransactionsWithStatusNew(string user_operation)
        {

            Dictionary<string, string> param = new()
            {
                { "@status_id", "9" },
                { "@newStatusCode", StatusConstants.StatusCode.APAGADO.ToString() }
            };

            SqlExecuter.ExecFunction("UPDATE transactions SET status_id = @newStatusCode WHERE status_id = @status_id", param, user_operation, true, "Remove Transaction");

            Log.Debug("Transactions with status NEW removed successfully.");

            return true;
        }

        public static bool editTransaction(EditTransaction input, string user_operation, string language)
        {
            if (ConfigManager.isProduction)
            {
                input.output_data = Cryptography.Encrypt(input.output_data, ConfigManager.generalKey);
            }

            Dictionary<string, string> param = new()
            {
                { "@ID", input.id.ToString() },
                { "@status_id", input.status_id },
                { "@started", input.started },
                { "@ended", input.ended },
                { "@exception", input.exception },
                { "@output_data", input.output_data }
            };

            string query = "UPDATE transactions SET status_id = ifnull(@status_id, status_id), started = ifnull(@started, started), ended = ifnull(@ended, ended), exception = ifnull(@exception, exception), output_data = ifnull(@output_data, output_data) WHERE ID = @ID";

            SqlExecuter.ExecFunction(query, param, user_operation, true, "Edit Transaction");

            Log.Debug("Transaction " + input.id + " edited successfully.");

            return true;
        }

        private static string ConvertToDateTime(string dateTimeString)
        {
            DateTime dateTime = DateTime.ParseExact(dateTimeString, "M/d/yyyy h:mm:ss tt", CultureInfo.InvariantCulture);
            return dateTime.ToString("yyyy-MM-dd HH:mm:ss", CultureInfo.InvariantCulture);
        }

        public static (bool, int) AddTransaction(AddTransaction input, string user_operation)
        {
            int transaction_id = -1;

            if (ConfigManager.isProduction)
            {
                input.input_data = Cryptography.Encrypt(input.input_data, ConfigManager.generalKey);
            }

            Dictionary<string, string> param = new()
            {
                { "@status_id", Config.StatusConstants.StatusCode.NOVO.ToString() },
                { "@queue_id", input.queue_id },
                { "@input_data", input.input_data },
                { "@reference", "Bitcoin Process" }
            };

            SqlExecuter.ExecFunction("INSERT INTO transactions(queue_id, status_id, input_data, reference) VALUES (@queue_id, @status_id, @input_data, @reference)", param, user_operation, false, "Add Transaction");

            // Retrieve the last inserted ID
            SqlExecuterItem responsive = SqlExecuter.ExecFunction("SELECT * FROM transactions ORDER BY ID DESC LIMIT 1", param, user_operation, false, "Add Transaction");

            foreach (Dictionary<string, string> item in responsive.out_data)
            {
                transaction_id = Convert.ToInt32(item["0"]);
            }

            Log.Debug("Transaction added successfully to queue" + input.queue_id);

            return (true, transaction_id);
        }
    }
}


