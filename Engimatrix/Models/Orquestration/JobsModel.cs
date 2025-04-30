using engimatrix.ModelObjs.Orquestration;
using engimatrix.Utils;
using engimatrix.ModelObjs;

namespace engimatrix.Models.Orquestration
{
    public class JobsModel
    {
        public static List<JobsItem> GetJobs(string lang, string user_operation)
        {
            List<JobsItem> result = [];

            if (!string.IsNullOrEmpty(user_operation))
            {
                Dictionary<string, string> dic = new()
               {
                   { "@lang", lang }
               };
                string query = "SELECT j.id, j.id_script, script.name, j.user_operation, j.date_time, j.job_details, " +
                "s.description as status " +
                "FROM job j " +
                "INNER JOIN script on script.id = j.id_script " +
                "JOIN status s ON s.id = j.status " +
                "ORDER BY j.id DESC, STR_TO_DATE(j.date_time, '%m/%d/%Y %H:%i:%s') DESC ";

                SqlExecuterItem responsive = SqlExecuter.ExecFunction(query, dic, user_operation, false, "GetJobs");
                JobsItem job = null;

                foreach (Dictionary<string, string> item in responsive.out_data)
                {
                    string id = item["0"];
                    string script_id = item["1"];
                    string script_name = item["2"];
                    string user_operation_job = item["3"];
                    string date_time = item["4"];
                    string job_Details = item["5"];
                    string status = item["6"];

                    job = new JobsItem(id, script_id, script_name, user_operation_job, status, date_time, job_Details);
                    result.Add(job);
                }
            }
            return result;
        }

        public static List<JobsItem> GetJobsByScriptId(string scriptId, string lang, string user_operation)
        {
            List<JobsItem> result = [];

            Dictionary<string, string> dic = new()
           {
               { "@id_script", scriptId },
               { "@lang", lang }
           };

            string query = "SELECT j.id, j.id_script, script.name, j.user_operation, j.date_time, j.job_details, " +
                "s.description as status " +
                "FROM job j " +
                "INNER JOIN script on script.id = j.id_script " +
                "JOIN status s ON s.id = j.status " +
                "WHERE j.id_script = @id_script " +
                "ORDER BY j.id DESC, STR_TO_DATE(j.date_time, '%m/%d/%Y %H:%i:%s') DESC ";

            SqlExecuterItem responsive = SqlExecuter.ExecFunction(query, dic, user_operation, false, "GetJobs");
            JobsItem job = null;

            foreach (Dictionary<string, string> item in responsive.out_data)
            {
                string id = item["0"];
                string script_id = item["1"];
                string script_name = item["2"];
                string user_operation_job = item["3"];
                string date_time = item["4"];
                string job_Details = item["5"];
                string status = item["6"];

                job = new JobsItem(id, script_id, script_name, user_operation_job, status, date_time, job_Details);
                result.Add(job);
            }

            return result;
        }

        public static void InsertJobDetails(string id_script, string executer_user, string status, string date_time, List<string> job_details)
        {
            Dictionary<string, string> dic = new()
           {
               { "@id_script", id_script },
               { "@user_operation", executer_user },
               { "@date_time", date_time },
               { "@job_details", string.Join(",", job_details) },
               { "@status", status }
           };

            SqlExecuterItem responsive = SqlExecuter.ExecFunction("INSERT INTO job ( id_script, user_operation,  date_time, job_details, status) VALUES " +
                "( @id_script, @user_operation, @date_time, @job_details, @status)", dic, executer_user, true, "InsertJobDetails");
        }

        public static bool InsertJobDetailsUsingQueueName(string queue_name, string executer_user, string status, string date_time, List<string> job_details, string lang)
        {
            ScriptsItem script = ScriptsModel.GetScriptByQueueName(queue_name, executer_user, lang);

            Dictionary<string, string> dic = new()
           {
               { "@id_script", script.id },
               { "@user_operation", executer_user },
               { "@date_time", date_time },
               { "@job_details", string.Join(",", job_details) },
               { "@status", status }
           };

            SqlExecuterItem responsive = SqlExecuter.ExecFunction("INSERT INTO job ( id_script, user_operation,  date_time, job_details, status) VALUES " +
                "( @id_script, @user_operation, @date_time, @job_details, @status)", dic, executer_user, true, "InsertJobDetails");

            Log.Debug("Job inserted successfully for script " + script.name);

            return true;
        }
    }
}
