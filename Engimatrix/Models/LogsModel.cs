// // Copyright (c) 2024 Engibots. All rights reserved.

using engimatrix.ModelObjs;
using engimatrix.RepositoryRecords;

namespace engimatrix.Models
{
    public class LogsModel
    {
        public static List<LogsItem> GetLogs(string user_operation)
        {
            List<LogsItem> result = new List<LogsItem>();

            if (!string.IsNullOrEmpty(user_operation))
            {
                Dictionary<string, string> dic = new Dictionary<string, string>();
                dic.Add("@email", "%" + user_operation + "%");

                SqlExecuterItem responsive = SqlExecuter.ExecFunction("SELECT COUNT(*) FROM logs WHERE user_operation LIKE @email", dic, user_operation, false, "GetUsers");

                string usersFound1 = responsive.out_data[0]["0"];
                int usersFound = Int32.Parse(usersFound1);

                dic = new Dictionary<string, string>();
                dic.Add("@email", "%" + user_operation + "%");

                SqlExecuterItem responsive2 = SqlExecuter.ExecFunction("SELECT COUNT(*) FROM logs WHERE operation_context LIKE @email", dic, user_operation, false, "GetUsers");

                string contextFound1 = responsive2.out_data[0]["0"];
                int contextFound = Int32.Parse(contextFound1);

                if (usersFound == 0)
                {
                    dic = new Dictionary<string, string>();
                    dic.Add("@email", "%" + user_operation + "%");
                    LogsDBRecord receiptRec = null;

                    SqlExecuterItem responsive1 = SqlExecuter.ExecFunction("SELECT * FROM logs WHERE operation_context LIKE @email order by id_log desc TOP 150", dic, user_operation, false, "GetUsers");

                    foreach (Dictionary<string, string> item in responsive1.out_data)
                    {
                        string id = item["0"];
                        string operation = item["1"];
                        string user_operation1 = item["2"];
                        string state = item["3"];
                        string operation_context = item["5"];
                        string date_time = item["4"];

                        receiptRec = new LogsDBRecord(id, operation, user_operation1, state, date_time, operation_context);
                        result.Add(receiptRec.ToLogsItem());
                    }
                }
                if (usersFound != 0)
                {
                    dic = new Dictionary<string, string>();
                    dic.Add("@email", "%" + user_operation + "%");

                    SqlExecuterItem responsive3 = SqlExecuter.ExecFunction("SELECT * FROM logs WHERE user_operation LIKE @email order by id_log desc", dic, user_operation, false, "GetUsers");
                    LogsDBRecord receiptRec = null;

                    foreach (Dictionary<string, string> item in responsive3.out_data)
                    {
                        string id = item["0"];
                        string operation = item["1"];
                        string user_operation1 = item["2"];
                        string state = item["3"];
                        string operation_context = item["5"];
                        string date_time = item["4"];

                        receiptRec = new LogsDBRecord(id, operation, user_operation1, state, date_time, operation_context);
                        result.Add(receiptRec.ToLogsItem());
                    }
                }

                if (usersFound == 0 || contextFound == 0)
                {
                    dic = new Dictionary<string, string>();
                    dic.Add("@email", "%" + user_operation + "%");

                    SqlExecuterItem responsive3 = SqlExecuter.ExecFunction("SELECT * FROM logs WHERE date_time LIKE @email order by id_log desc", dic, user_operation, false, "GetUsers");
                    LogsDBRecord receiptRec = null;

                    foreach (Dictionary<string, string> item in responsive3.out_data)
                    {
                        string id = item["0"];
                        string operation = item["1"];
                        string user_operation1 = item["2"];
                        string state = item["3"];
                        string operation_context = item["5"];
                        string date_time = item["4"];

                        receiptRec = new LogsDBRecord(id, operation, user_operation1, state, date_time, operation_context);
                        result.Add(receiptRec.ToLogsItem());
                    }
                }
            }
            else
            {
                Dictionary<string, string> dic = new Dictionary<string, string>();
                SqlExecuterItem responsive3 = SqlExecuter.ExecFunction("SELECT * FROM logs order by id_log desc", dic, user_operation, false, "GetClients");
                LogsDBRecord receiptRec = null;

                foreach (Dictionary<string, string> item in responsive3.out_data)
                {
                    string id = item["0"];
                    string operation = item["1"];
                    string user_operation1 = item["2"];
                    string state = item["3"];
                    string operation_context = item["5"];
                    string date_time = item["4"];

                    receiptRec = new LogsDBRecord(id, operation, user_operation1, state, date_time, operation_context);
                    result.Add(receiptRec.ToLogsItem());
                }
            }
            return result;
        }
    }
}
