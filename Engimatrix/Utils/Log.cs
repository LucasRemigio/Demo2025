// // Copyright (c) 2024 Engibots. All rights reserved.

using System.Reflection;
using System.Text;

namespace engimatrix.Utils
{
    public static class Log
    {
        private static readonly string DatetimeFormat = "yyyy-MM-dd HH:mm:ss.fff";
        private static string Filename;
        private static readonly object locker = new Object();
        public static string logsFolder = string.Empty;

        static Log()
        {
            Init();
        }

        public static void Init()
        {
            if (!Directory.Exists("Logs"))
            {
                Directory.CreateDirectory("Logs");
            }

            logsFolder = Path.Combine(Directory.GetCurrentDirectory(), "Logs", DateTime.UtcNow.ToString("yyyyMMdd") + "/");
            Directory.CreateDirectory(logsFolder);

            Filename = Path.Combine(logsFolder, Assembly.GetExecutingAssembly().GetName().Name + "_" + DateTime.UtcNow.ToString("yyyyMMdd") + ".log");

            Console.WriteLine("Log file name - " + Filename);

            // Log file header line
            string logHeader = Filename + " is created.";
            if (!File.Exists(Filename))
            {
                // Usar 'using' para garantir que o FileStream seja fechado após a criação.
                using (var stream = File.Create(Filename)) { }
                WriteLine(DateTime.UtcNow.ToString(DatetimeFormat) + " " + logHeader, false);
            }
        }

        public static void Debug(string text)
        {
            lock (locker)
            {
                WriteFormattedLog(LogLevel.DEBUG, text);
            }
        }

        public static void Error(string text)
        {
            lock (locker)
            {
                WriteFormattedLog(LogLevel.ERROR, text);
            }
        }

        public static void Fatal(string text)
        {
            lock (locker)
            {
                WriteFormattedLog(LogLevel.FATAL, text);
            }
        }

        public static void Info(string text)
        {
            lock (locker)
            {
                WriteFormattedLog(LogLevel.INFO, text);
            }
        }

        public static void Trace(string text)
        {
            lock (locker)
            {
                WriteFormattedLog(LogLevel.TRACE, text);
            }
        }

        public static void Warning(string text)
        {
            lock (locker)
            {
                WriteFormattedLog(LogLevel.WARNING, text);
            }
        }

        private static void CheckForDailyLogRotation()
        {
            string currentLogFolder = Path.Combine(Directory.GetCurrentDirectory(), "Logs", DateTime.UtcNow.ToString("yyyyMMdd") + "/");
            string currentFilename = Path.Combine(currentLogFolder, Assembly.GetExecutingAssembly().GetName().Name + "_" + DateTime.UtcNow.ToString("yyyyMMdd") + ".log");

            if (currentFilename != Filename)
            {
                lock (locker)
                {
                    Init(); // Reinitialize the logging system, which will create a new log file if necessary - one per day
                }
            }
        }

        private static void WriteFormattedLog(LogLevel level, string text)
        {
            // Here you would get your LogLevel from your configuration.
            // For the example, let's assume all levels are enabled.
            LogLevel currentLevel = LogLevel.TRACE;

            if (currentLevel > level)
            {
                return;
            }

            CheckForDailyLogRotation();

            string pretext = DateTime.UtcNow.ToString(DatetimeFormat) + " [" + level + "]   ";

            WriteLine(pretext + text);
        }

        private static void WriteLine(string text, bool append = true)
        {
            try
            {
                Console.WriteLine(text);

                using (StreamWriter writer = new StreamWriter(Filename, append, Encoding.UTF8))
                {
                    if (!string.IsNullOrEmpty(text))
                    {
                        writer.WriteLine(text);
                    }
                }
            }
            catch (Exception ex)
            {
                // Handle the exception according to your needs
                Console.WriteLine("An error occurred while writing to the log file: " + ex.Message);
            }
        }

        [Flags]
        public enum LogLevel
        {
            TRACE,
            DEBUG,
            INFO,
            WARNING,
            ERROR,
            FATAL
        }
    }
}
