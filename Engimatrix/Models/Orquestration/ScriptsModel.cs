using engimatrix.ModelObjs.Orquestration;
using System.Diagnostics;
using Quartz;
using Quartz.Impl;
using Quartz.Impl.Matchers;
using engimatrix.Utils;
using System.IO.Compression;
using engimatrix.Config;
using static engimatrix.Views.Orquestration.ScriptRequest;
using static engimatrix.Models.Orquestration.TriggersModel;
using System.Text;
using engimatrix.ModelObjs;
namespace engimatrix.Models.Orquestration
{
    public class ScriptsModel
    {
        private static IScheduler scheduler;

        private static bool isFirstRun = true;
        private static bool isFirstRunTriggerRemove = true;

        public static void InitializeTimer()
        {
            try
            {
                scheduler = new StdSchedulerFactory().GetScheduler().Result;
                scheduler.Start();

                Timer timer = new(UpdateSettings, null, TimeSpan.Zero, TimeSpan.FromSeconds(20));
            }
            catch (Exception ex)
            {
                Log.Error($"Error initializing timer: {ex}");
            }
        }

        private static void UpdateSettings(object state)
        {
            try
            {
                List<(string ScriptId, string CronExpression)> scriptConfigurations = SearchCronExpressions();

                UpdateTriggers(scriptConfigurations);
            }
            catch (Exception ex)
            {
                Log.Error($"Error updating settings: {ex}");
            }
        }

        private static void UpdateTriggers(List<(string ScriptId, string CronExpression)> scriptConfigurations)
        {
            try
            {
                var existingTriggers = scheduler.GetTriggerKeys(GroupMatcher<TriggerKey>.AnyGroup()).Result;

                foreach (var triggerKey in existingTriggers)
                {
                    scheduler.UnscheduleJob(triggerKey).Wait();

                    if (isFirstRunTriggerRemove)
                    {
                        Log.Debug($"Trigger removed: {triggerKey}");
                        isFirstRunTriggerRemove = false; // Setting the flag to false after the first run so we don't have multiple prints 
                    }
                }

                foreach (var (scriptId, cronExpression) in scriptConfigurations)
                {
                    ScheduleNewTrigger(scriptId, cronExpression);
                }
            }
            catch (Exception ex)
            {
                Log.Error($"Error updating triggers: {ex}");
            }
        }

        private static void ScheduleNewTrigger(string scriptId, string cronExpression)
        {
            try
            {
                IJobDetail job = JobBuilder.Create<CheckScriptsJob>()
                    .UsingJobData("scriptId", scriptId)
                    .Build();

                ITrigger trigger = TriggerBuilder.Create()
                    .WithCronSchedule(cronExpression)
                    .Build();

                scheduler.ScheduleJob(job, trigger);

                if (isFirstRun)
                {
                    Log.Debug($"New Scheduled Trigger - Script ID: {scriptId}, Cron Expression: {cronExpression}");
                    isFirstRun = false; // Setting the flag to false after the first run so we don't have multiple prints 
                }
            }
            catch (Exception ex)
            {
                Log.Error($"Error scheduling new trigger: {ex}");
            }
        }

        private static List<(string ScriptId, string CronExpression)> SearchCronExpressions()
        {
            try
            {
                List<(string ScriptId, string CronExpression)> scriptConfigurations = [];

                Dictionary<string, string> dic = [];
                SqlExecuterItem scriptsResponse = SqlExecuter.ExecFunction("SELECT id FROM script WHERE status NOT IN (2)", dic, "system", false, "Crono");

                // Collect valid script IDs
                HashSet<string> validScriptIds = [];
                foreach (Dictionary<string, string> item in scriptsResponse.out_data)
                {
                    validScriptIds.Add(item["0"]);
                }

                // If no valid script IDs, return empty list
                if (validScriptIds.Count == 0)
                {
                    return scriptConfigurations;
                }

                // Query triggers based on valid script IDs
                SqlExecuterItem triggersResponse = SqlExecuter.ExecFunction("SELECT script_id, cron_expression FROM triggers", dic, "system", false, "Crono");

                foreach (Dictionary<string, string> item in triggersResponse.out_data)
                {
                    string scriptId = item["0"];
                    string cronExpression = item["1"];

                    // Add to configurations if the script ID is valid
                    if (validScriptIds.Contains(scriptId))
                    {
                        scriptConfigurations.Add((scriptId, cronExpression));
                    }
                }

                return scriptConfigurations;
            }
            catch (Exception ex)
            {
                Log.Error($"Error searching cron expressions: {ex}");
                return [];
            }
        }

        public class CheckScriptsJob : IJob
        {
            public Task Execute(IJobExecutionContext context)
            {
                try
                {
                    string scriptId = context.JobDetail.JobDataMap.GetString("scriptId");
                    StartScript(scriptId, "system", ConfigManager.defaultLanguage);

                    return Task.CompletedTask;
                }
                catch (Exception ex)
                {
                    Log.Error($"Error executing job: {ex}");
                    return Task.CompletedTask;
                }
            }
        }

        public static List<ScriptsItem> GetScripts(string executer_user, string lang)
        {
            List<ScriptsItem> result = [];

            Dictionary<string, string> dic = new()
           {
               { "@lang", lang },
               { "@status", Config.StatusConstants.StatusCode.INATIVO.ToString() }
           };

            SqlExecuterItem responsive = SqlExecuter.ExecFunction("SELECT s.id, s.name, s.description, s.cron_job, s.status , sv.url_script, sv.main_file, sv.date_time, sv.version, (SELECT j.date_time from job j where j.id_script = s.id ORDER BY STR_TO_DATE(j.date_time, '%d/%m/%Y %H:%i:%s') DESC LIMIT 1 ) as 'last_execution' FROM script s INNER JOIN script_version sv ON s.id = sv.id_script INNER JOIN  status ss ON s.status = ss.id WHERE sv.is_current_version = 1  and s.status not in (@status);", dic, executer_user, false, "GetAllScripts");

            foreach (Dictionary<string, string> item in responsive.out_data)
            {
                string id_script = item["0"];
                string name = item["1"];
                string description = item["2"];
                string cron_job = item["3"];
                string status = item["4"];
                string url_script = item["5"];
                string main_file = item["6"];
                string script_update_last_time = item["7"];
                string version = item["8"];
                string last_execution = item["9"];
                string nextRun = CronTriggerManager.GetNextExecutionForScript(id_script, lang, executer_user);

                List<TriggersItem> triggers = GetTriggersByScriptId(id_script, lang, executer_user);

                ScriptsItem scriptsRec = new(id_script, name, description, cron_job, status, url_script, main_file, script_update_last_time, version, last_execution, nextRun, triggers);

                result.Add(scriptsRec);
            }

            return result;
        }

        public static ScriptsItem GetScriptById(string id, string executer_user, string lang)
        {
            ScriptsItem result = null;

            Dictionary<string, string> dic = new()
           {
               { "@id", id },
               { "@lang", lang }
           };
            string query = "SELECT s.id, s.name, s.description, s.cron_job, ss.description as 'status' , " +
                         "sv.url_script, sv.main_file, sv.date_time, sv.version, " +
                         "(SELECT j.date_time from job j where j.id_script = s.id ORDER BY STR_TO_DATE(j.date_time, '%d/%m/%Y %H:%i:%s') DESC LIMIT 1 ) as 'last_execution' " +
                         "FROM script s " +
                         "INNER JOIN script_version sv ON s.id = sv.id_script " +
                         "INNER JOIN status ss ON s.status = ss.id " +
                         "WHERE sv.is_current_version = 1 AND s.id = @id;";

            SqlExecuterItem responsive = SqlExecuter.ExecFunction(query, dic, executer_user, false, "GetScriptsById");

            foreach (Dictionary<string, string> item in responsive.out_data)
            {
                string id_script = item["0"];
                string name = item["1"];
                string description = item["2"];
                string cron_job = item["3"];
                string status = item["4"];
                string url_script = item["5"];
                string main_file = item["6"];
                string script_update_last_time = item["7"];
                string version = item["8"];
                string last_execution = item["9"];
                string nextRun = CronTriggerManager.GetNextExecutionForScript(id_script, lang, executer_user);

                List<TriggersItem> triggers = GetTriggersByScriptId(id_script, lang, executer_user);

                result = new ScriptsItem(id_script, name, description, cron_job, status, url_script, main_file, script_update_last_time, version, last_execution, nextRun, triggers);
            }
            return result;
        }

        public static ScriptsItem GetScriptByQueueName(string queue_name, string executer_user, string lang)
        {
            ScriptsItem result = null;

            Dictionary<string, string> dic = new()
           {
               { "@queue_name", queue_name },
               { "@lang", lang }
           };
            SqlExecuterItem responsive = SqlExecuter.ExecFunction("SELECT s.id, s.name, s.description, s.cron_job, GetLabelDescription(@lang, ss.description) as 'status' , sv.url_script, sv.main_file, sv.date_time, sv.version, (SELECT j.date_time from job j where j.id_script = s.id ORDER BY j.date_time DESC LIMIT 1 ) as 'last_execution' FROM script s INNER JOIN script_version sv ON s.id = sv.id_script INNER JOIN  status ss ON s.status = ss.id INNER JOIN queues_script q ON q.id_script = s.id WHERE sv.is_current_version = 1 AND q.name = @queue_name;", dic, executer_user, false, "GetScriptsById");

            foreach (Dictionary<string, string> item in responsive.out_data)
            {
                string id_script = item["0"];
                string name = item["1"];
                string description = item["2"];
                string cron_job = item["3"];
                string status = item["4"];
                string url_script = item["5"];
                string main_file = item["6"];
                string script_update_last_time = item["7"];
                string version = item["8"];
                string last_execution = item["9"];
                string nextRun = CronTriggerManager.GetNextExecutionForScript(id_script, lang, executer_user);

                List<TriggersItem> triggers = GetTriggersByScriptId(id_script, lang, executer_user);

                result = new ScriptsItem(id_script, name, description, cron_job, status, url_script, main_file, script_update_last_time, version, last_execution, nextRun, triggers);
            }
            return result;
        }

        public static List<ScriptsItem> StartScriptOLD(string id, string executer_user, string language)
        {
            DateTime currentDateTime = DateTime.Now;
            string date_time = currentDateTime.ToString("dd/MM/yyyy HH:mm:ss");

            Stopwatch stopwatch = new();
            stopwatch.Start();
            List<ScriptsItem> result = [];
            List<string> job_details = [];
            ScriptsItem scriptsRec = null;
            try
            {
                job_details.Add("Python process started");
                ScriptsItem script = GetScriptById(id, executer_user, language);
                try
                {
                    // Paths to files and directories
                    string scriptDirectory = Path.GetDirectoryName(script.url_script);

                    // Verify if the file exists before trying to access it
                    if (!File.Exists(script.url_script))
                    {
                        job_details.Add($"The file {script.url_script} was not found.");
                        throw new FileNotFoundException($"File not found: {script.url_script}");
                    }

                    // Configuration for the Python process
                    ProcessStartInfo start = new()
                    {
                        FileName = "python",
                        Arguments = $"\"{script.url_script}\"",
                        UseShellExecute = false,
                        RedirectStandardOutput = true,
                        RedirectStandardError = true,
                        //WorkingDirectory = scriptDirectory
                    };

                    // Execution of the Python process
                    using (Process process = new())
                    {
                        process.StartInfo = start;
                        process.Start();

                        // Read standard output and error in separate threads to avoid blocking
                        Task<string> outputTask = process.StandardOutput.ReadToEndAsync();
                        Task<string> errorTask = process.StandardError.ReadToEndAsync();

                        // Wait until both tasks are completed
                        Task.WaitAll(outputTask, errorTask);

                        // Display standard output and error
                        string output = outputTask.Result;
                        string error = errorTask.Result;

                        Log.Debug(output);
                        Log.Debug(error);

                        process.WaitForExit();

                        job_details.Add("The Python process was completed successfully.");
                        stopwatch.Stop();
                        job_details.Add($"Execution time: {stopwatch.Elapsed}");

                        string jobDetailsString = string.Join(",", job_details);

                        job_details.Add("Script execution successful.");

                        JobsModel.InsertJobDetails(script.id, executer_user, Config.StatusConstants.StatusCode.SUCESSO.ToString(), date_time, job_details);
                    }
                }
                catch (Exception ex)
                {
                    if (scriptsRec == null)
                    {
                        var errorMessage = $"Failed to run the Script. Error: {ex.Message}";
                        Log.Error(errorMessage);
                        job_details.Add(errorMessage);
                        JobsModel.InsertJobDetails(script.id, executer_user, Config.StatusConstants.StatusCode.ERRO.ToString(), date_time, job_details);
                    }
                }
            }
            catch (Exception ex)
            {
                if (scriptsRec != null)
                {
                    var errorMessage = $"Script Not Found. Error: {ex.Message}";
                    Log.Error(errorMessage);
                    job_details.Add(errorMessage);
                    JobsModel.InsertJobDetails(scriptsRec.id, executer_user, Config.StatusConstants.StatusCode.ERRO.ToString(), date_time, job_details);
                }
            }

            return GetScripts(executer_user, language);
        }

        public static List<ScriptsItem> StartScript(string id, string executer_user, string language)
        {
            DateTime currentDateTime = DateTime.Now;
            string date_time = currentDateTime.ToString("dd/MM/yyyy HH:mm:ss");

            Stopwatch stopwatch = new();
            stopwatch.Start();
            List<ScriptsItem> result = [];
            List<string> job_details = [];
            ScriptsItem scriptsRec = null;

            try
            {
                Log.Debug("Starting Script execution");
                job_details.Add("Script execution started");
                ScriptsItem script = GetScriptById(id, executer_user, language);

                Log.Debug("Script retrieved: " + script.name);

                // Ensure activate.bat is executed before proceeding
                string scriptDirectory = Path.GetDirectoryName(script.url_script);
                string activateBatPath = Path.Combine(scriptDirectory, ".venv", "Scripts", "activate.bat");

                // Execute activate.bat to set environment variables
                RunCommand(activateBatPath);

                Log.Debug("Execution of Script " + script.name + " has started.");

                try
                {
                    // Verify if the file exists before trying to access it
                    if (!File.Exists(script.url_script))
                    {
                        job_details.Add($"The file {script.url_script} was not found.");
                        throw new FileNotFoundException($"File not found: {script.url_script}");
                    }

                    string pythonExePath = Path.Combine(scriptDirectory, ".venv//Scripts//python.exe");
                    string arguments = $"\"{Path.GetFileName(script.url_script)}\"";
                    string workingDirectory = scriptDirectory;

                    var (output, error) = RunCommand(pythonExePath, arguments, workingDirectory);

                    // Display standard output and error

                    if (!string.IsNullOrEmpty(error))
                    {
                        job_details.Add("********* ERRORS ***********\r\n" + error + "\r\n*****************");
                    }
                    job_details.Add(output);
                    stopwatch.Stop();

                    string jobDetailsString = string.Join(",", job_details);

                    job_details.Add("Script execution ended.");

                    //// Update status based on presence of errors
                    int status = string.IsNullOrEmpty(error) ? Config.StatusConstants.StatusCode.SUCESSO : Config.StatusConstants.StatusCode.ERRO;

                    JobsModel.InsertJobDetails(script.id, executer_user, status.ToString(), date_time, job_details);

                    if (status == StatusConstants.StatusCode.SUCESSO)
                    {
                        Log.Debug("Execution of Script " + script.name + " finished with Success.");
                    }
                    else if (status == StatusConstants.StatusCode.ERRO)
                    {
                        Log.Debug("Execution of Script " + script.name + " finished with Insuccess.");
                    }
                }
                catch (Exception ex)
                {
                    if (scriptsRec == null)
                    {
                        var errorMessage = $"Failed to run the Script. Error: {ex.Message}";
                        Log.Error(errorMessage);
                        job_details.Add(errorMessage);
                        JobsModel.InsertJobDetails(script.id, executer_user, Config.StatusConstants.StatusCode.ERRO.ToString(), date_time, job_details);
                    }
                    throw;
                }
            }
            catch (Exception ex)
            {
                if (scriptsRec != null)
                {
                    var errorMessage = $"Script Not Found. Error: {ex.Message}";
                    Log.Error(errorMessage);
                    job_details.Add(errorMessage);
                    JobsModel.InsertJobDetails(scriptsRec.id, executer_user, Config.StatusConstants.StatusCode.ERRO.ToString(), date_time, job_details);
                }
                throw;
            }

            return GetScripts(executer_user, language);
        }

        public static void EditScript(Edit input, string executer_user)
        {
            Log.Debug("Updating Script.");

            Dictionary<string, string> dic = new()
           {
               { "@description", input.description },
               { "@cron_job", input.cron_job },
               { "@id", input.id }
           };

            SqlExecuter.ExecFunction("UPDATE script SET description=@description, cron_job=@cron_job WHERE id=@id", dic, executer_user, true, "EditScripts");

            if (!string.IsNullOrEmpty(input.file_content))
            {
                string mainFolderPath = "Scripts";

                string temp_file_name = input.name.Replace(" ", "_");
                // Save base64 into zip file
                byte[] zipBytes = Convert.FromBase64String(input.file_content);
                string zipFilePath = Path.Combine(mainFolderPath, Path.GetFileNameWithoutExtension(temp_file_name) + ".zip");
                File.WriteAllBytes(zipFilePath, zipBytes);

                InstallNewScript(zipFilePath, input.main_file, true, input.name);
                Log.Debug("Script " + input.id + " version updated successfully.");
            }
        }

        public static void UpdateScriptStatus(string id, string executer_user, string language)
        {
            Dictionary<string, string> param = new()
            {
                { "@id", id },
                { "@lang", language },
                { "@status", Config.StatusConstants.StatusCode.A_PROCESSAR.ToString() }
            };

            SqlExecuter.ExecFunction("UPDATE script SET status = 2 WHERE id = @id", param, executer_user, true, "UpdateScriptStatus");

            Log.Debug("Status of Script " + id + " updated to Processing.");

        }

        public static void RemoveScript(string id, string executer_user)
        {
            Dictionary<string, string> dic = new()
           {
               { "@status", Config.StatusConstants.StatusCode.INATIVO.ToString() },
               { "@id", id }
           };

            SqlExecuter.ExecFunction("UPDATE script SET status=@status WHERE id=@id", dic, executer_user, true, "removeScript");

            Log.Debug("Status of Script" + id + " updated to Inactive.");

        }

        public static string GetNextExecution(string cronExpression)
        {
            try
            {
                var cron = new CronExpression(cronExpression);

                // Get the local time zone
                var localTimeZone = TimeZoneInfo.Local;
                var currentTime = DateTimeOffset.UtcNow;

                // Convert the current time to local time
                var currentLocalTime = TimeZoneInfo.ConvertTime(currentTime, localTimeZone);

                // Calculate the next valid time after the current local time
                var nextRun = cron.GetNextValidTimeAfter(currentLocalTime);

                // Manually add the time zone offset to the nextRun time
                var nextRunWithOffset = nextRun?.Add(localTimeZone.GetUtcOffset(nextRun.Value.DateTime));

                // Convert the next run time with offset to string
                return nextRunWithOffset?.ToString("yyyy-MM-dd HH:mm:ss") ?? "Error Converting";
            }
            catch (Exception ex)
            {
                Log.Error($"Error converting cron expression: {ex.Message}");
                return "Error converting cron expression";
            }
        }

        public static class CronTriggerManager
        {
            public static string GetNextExecutionForScript(string scriptId, string language, string executer_user)
            {
                List<TriggersItem> triggers = GetTriggersByScriptId(scriptId, language, executer_user);
                if (triggers == null || triggers.Count == 0)
                {
                    return "No triggers found for the script.";
                }

                DateTimeOffset? nextExecution = null;

                foreach (var trigger in triggers)
                {
                    try
                    {
                        var cron = new CronExpression(trigger.cron_expression);
                        var currentLocalTime = TimeZoneInfo.ConvertTime(DateTimeOffset.UtcNow, TimeZoneInfo.Local);
                        var nextRun = cron.GetNextValidTimeAfter(currentLocalTime);

                        if (nextRun.HasValue)
                        {
                            if (!nextExecution.HasValue || nextRun.Value < nextExecution.Value)
                            {
                                nextExecution = nextRun;
                            }
                        }
                        else
                        {
                            Log.Error($"Error converting cron expression for trigger {trigger.name}: Invalid cron expression");
                        }
                    }
                    catch (Exception ex)
                    {
                        Log.Error($"Error converting cron expression for trigger {trigger.name}: {ex.Message}");
                    }
                }

                if (nextExecution.HasValue)
                {
                    var nextRunWithOffset = nextExecution.Value.Add(TimeZoneInfo.Local.GetUtcOffset(nextExecution.Value.DateTime));
                    return nextRunWithOffset.ToString("yyyy-MM-dd HH:mm:ss");
                }
                else
                {
                    return "No valid next execution time found";
                }
            }
        }

        public static string InstallNewScript(string script_zip_path, string main_file, bool newversion, string script_name)
        {

            string mainFolderPath = "Scripts";
            string zipFilePath = script_zip_path;

            string extractionPath = Path.Combine(mainFolderPath, Path.GetFileNameWithoutExtension(zipFilePath) + Guid.NewGuid().ToString());
            string venvPath = Path.Combine(extractionPath, ".venv");
            string requirementsPath = Path.Combine(extractionPath, "requirements.txt");
            string projectScriptPath = Path.Combine(extractionPath, Path.GetFileNameWithoutExtension(main_file) + ".py");
            string pythonScriptPath = Path.Combine(mainFolderPath, "Install New Script.py");

            try
            {
                if (Directory.Exists(extractionPath))
                {
                    Directory.Delete(extractionPath, true);
                }
                Thread.Sleep(2009);

                // Extract ZIP
                ZipFile.ExtractToDirectory(zipFilePath, extractionPath);

                //Validate requirements file
                string[] files = Directory.GetFiles(extractionPath, "*.txt");
                int requirementsCount = files.Count(nome => Path.GetFileNameWithoutExtension(nome).ToLower() == Path.GetFileNameWithoutExtension(requirementsPath.ToLower()));

                if (requirementsCount == 0)
                {
                    throw new FileNotFoundException("No requirements file found");
                }
                else if (requirementsCount > 1)
                {
                    throw new InvalidOperationException("Found " + requirementsCount.ToString() + " requirements files");
                }

                //Validate main file
                files = Directory.GetFiles(extractionPath, "*.py");
                int mainFileCount = files.Count(nome => Path.GetFileName(nome).Equals(main_file, StringComparison.OrdinalIgnoreCase));

                if (mainFileCount == 0)
                {
                    throw new InvalidOperationException("No main file found");
                }
                else if (mainFileCount > 1)
                {
                    throw new InvalidOperationException("Found " + mainFileCount.ToString() + " main files");
                }

                //Configure logging events
                string[] main_file_code = File.ReadAllLines(files.First(nome => Path.GetFileName(nome).Equals(main_file, StringComparison.OrdinalIgnoreCase)));

                for (int index = 0; index < main_file_code.Length; index++)
                {
                    string line = main_file_code[index];
                    if (line.Contains("import logging"))
                    {
                        //Check if configuration was already injected
                        string config = "import logging;import sys;logging.basicConfig(level=logging.DEBUG, handlers=[logging.StreamHandler(sys.stdout)]);error_handler = logging.StreamHandler(sys.stderr);error_handler.setLevel(logging.ERROR);logging.getLogger().addHandler(error_handler)";
                        if (main_file_code.Count(line => line.Contains(config)) == 0)
                        {
                            line = line.Replace("import logging", config);
                            main_file_code[index] = line;
                            File.WriteAllLines(files.First(nome => Path.GetFileName(nome).Equals(main_file, StringComparison.OrdinalIgnoreCase)), main_file_code);
                        }
                        break;
                    }
                }

                venvPath = Path.Combine(extractionPath, ".venv");

                string pythonPath = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.ProgramFiles), "Python", "Python312", "python.exe");

                // Create a virtual environment
                var (output, error) = RunCommand1(pythonPath, $"-m venv {venvPath}");

                if (!string.IsNullOrEmpty(error))
                {
                    Log.Error($"Error creating virtual environment: {error}");
                    throw new InvalidOperationException($"Error creating virtual environment: {error}");
                }

                // Read the existing content of activate.bat
                string activateBatPath = Path.Combine(venvPath, "Scripts", "activate.bat");
                string existingContent = File.ReadAllText(activateBatPath);

                // Append the new environment variable setting to the existing content
                string newContent = $@"{existingContent}
                @echo off
                set ""engiToken={ConfigManager.engimatrixInternalApiKey}""";

                File.WriteAllText(activateBatPath, newContent);

                // Activate virtual environment
                RunCommand(activateBatPath);

                if (!string.IsNullOrEmpty(error))
                {
                    Log.Error($"Error creating virtual environment: {error}");
                }

                Log.Debug("Installing dependencies... ");

                // Install python dependencies
                var (_, _) = RunCommand1(Path.Combine(venvPath, "Scripts", "pip.exe"), $"install -r {requirementsPath} --verbose");

                if (!string.IsNullOrEmpty(error))
                {
                    Log.Error($"Error installing dependencies: {error}");
                }
                else
                {
                    Log.Error($"Dependencies installed successfully:\n{output}");
                }

                if (newversion == true)
                {
                    UpdateScriptVersion(script_name, projectScriptPath, Path.GetFileNameWithoutExtension(main_file) + ".py", "system");
                }

                return projectScriptPath;
            }
            catch (Exception ex)
            {
                Log.Error($"An error occurred: {ex.Message}");
                throw;
            }

        }

        public static (string output, string error) RunCommand1(string command, string arguments)
        {
            var processStartInfo = new ProcessStartInfo
            {
                FileName = command,
                Arguments = arguments,
                RedirectStandardOutput = true,
                RedirectStandardError = true,
                UseShellExecute = false,
                CreateNoWindow = true
            };

            using (var process = Process.Start(processStartInfo))
            {

                StringBuilder outputBuilder = new();
                StringBuilder errorBuilder = new();

                process.OutputDataReceived += (_, e) =>
                {
                    if (e.Data != null)
                    {
                        outputBuilder.AppendLine(e.Data);
                    }
                };

                process.ErrorDataReceived += (_, e) =>
                {
                    if (e.Data != null)
                    {
                        errorBuilder.AppendLine(e.Data);
                    }
                };

                process.BeginOutputReadLine();
                process.BeginErrorReadLine();

                process.WaitForExit(); // Aguarda o processo terminar

                return (outputBuilder.ToString(), errorBuilder.ToString());
            }
        }

        public static void AddScript(Add script, string executer_user)
        {

            string mainFolderPath = "Scripts";

            string temp_file_name = script.name.Replace(" ", "_");
            // Save base64 into zip file
            byte[] zipBytes = Convert.FromBase64String(script.file_content);
            string zipFilePath = Path.Combine(mainFolderPath, Path.GetFileNameWithoutExtension(temp_file_name) + ".zip");
            File.WriteAllBytes(zipFilePath, zipBytes);

            string projectScriptPath = InstallNewScript(zipFilePath, script.main_file, false, script.name);

            Dictionary<string, string> dic = new()
            {
                { "@name", script.name },
                { "@description", script.description },
                { "@status_id", Config.StatusConstants.StatusCode.ATIVO.ToString() },
                { "@url_script", projectScriptPath },
                { "@main_file", Path.GetFileNameWithoutExtension(script.main_file) + ".py" }
            };
            try
            {
                SqlExecuter.ExecFunction("INSERT INTO script (name, description, status) VALUES (@name,@description,@status_id);", dic, executer_user, true, "InsertScript");
                SqlExecuter.ExecFunction("INSERT INTO script_version (id_script, url_script, main_file, version) VALUES ((SELECT id FROM script WHERE name = @name ), @url_script, @main_file, (SELECT IFNULL(MAX(v.version), 0) + 1 FROM script_version v WHERE v.id_script = (SELECT s.id FROM script s WHERE s.name = @name)));", dic, executer_user, false, "InsertScriptVersion");
                Log.Debug("Script " + script.name + " added with success.");
            }
            catch (Exception)
            {
                UpdateScriptVersion(Path.GetFileNameWithoutExtension(zipFilePath), projectScriptPath, Path.GetFileNameWithoutExtension(script.main_file) + ".py", executer_user);
            }
        }

        private static (string Output, string Error) RunCommand(string fileName, string arguments = null, string workingDirectory = null)
        {
            var processStartInfo = new ProcessStartInfo
            {
                FileName = fileName,
                Arguments = arguments,
                RedirectStandardOutput = true,
                RedirectStandardError = true,
                UseShellExecute = false,
                CreateNoWindow = true,
                WorkingDirectory = workingDirectory
            };

            using (var process = new Process { StartInfo = processStartInfo })
            {
                process.Start();

                // Leia a saída padrão e o erro em threads separadas para evitar bloqueios
                Task<string> errorTask = process.StandardError.ReadToEndAsync();
                Task<string> outputTask = process.StandardOutput.ReadToEndAsync();

                processStartInfo.EnvironmentVariables.Add("MY_APP_LOG_LEVEL", "DEBUG");

                // Aguarde até que ambas as tarefas sejam concluídas
                Task.WaitAll(outputTask, errorTask);

                // Exiba a saída padrão e o erro
                Log.Debug(outputTask.Result);
                Log.Debug(errorTask.Result);

                // Display the error if it has thrown an error
                if (!string.IsNullOrEmpty(errorTask.Result))
                {
                    Log.Error(errorTask.Result);
                }

                process.WaitForExit();
                return (Output: outputTask.Result, Error: errorTask.Result);
            }
        }

        private static void UpdateScriptVersion(string script_name, string url_script, string main_file, string executer_user)
        {
            Log.Debug("Updating the DB with the new version for script " + script_name);

            Dictionary<string, string> dic = new()
           {
               { "@name", script_name },
               { "@url_script", url_script },
               { "@main_file", main_file }
           };

            // Retrieve the 'id_script' from the 'script' table based on the provided script name
            string scriptIdQuery = "SELECT id FROM script WHERE name = @name";
            SqlExecuterItem scriptIdResult = SqlExecuter.ExecFunction(scriptIdQuery, dic, executer_user, true, "GetScriptId");

            // Check if script id was found
            if (scriptIdResult.operationResult && scriptIdResult.out_data.Count > 0)
            {
                string scriptId = scriptIdResult.out_data[0]["0"]; // Assuming the id is retrieved as the first field

                // Add 'id_script' parameter to the dictionary
                dic.Add("@id_script", scriptId);

                // Update 'is_current_version' column in 'script_version' table
                SqlExecuter.ExecFunction("UPDATE script_version SET is_current_version = 0 WHERE id_script = @id_script", dic, executer_user, true, "Update Script Version");

                // Insert new row into 'script_version' table
                SqlExecuter.ExecFunction("INSERT INTO script_version (id_script, url_script, main_file, version) VALUES (@id_script, @url_script, @main_file, (SELECT IFNULL(MAX(v.version), 0) + 1 FROM script_version v WHERE v.id_script = @id_script))", dic, executer_user, false, "InsertScriptVersion");

                Log.Debug("Script " + script_name + " version updated successfully.");
            }
            else
            {
                Log.Error("Error: Script ID not found for the provided script name: " + script_name);
            }

        }
    }
}
