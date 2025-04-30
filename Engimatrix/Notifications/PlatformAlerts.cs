// // Copyright (c) 2024 Engibots. All rights reserved.

namespace engimatrix.Notifications
{
    using System;
    using MySqlConnector;
    using engimatrix.Config;
    using engimatrix.Connector;
    using engimatrix.Exceptions;
    using engimatrix.Utils;

    public static class PlatformAlerts
    {
        private const string CRITICALALERT = "CRITICAL";
        private const string NORMALALERT = "NORMAL";

        public static void CreatePlatformAlert(string message)
        {
            string platformMsg = "[Platform Alert!] - " + ConfigManager.nodeName + " - " + message;

            try
            {
            }
            catch (Exception e)
            {
                Log.Error(e.ToString());
            }

            Log.Warning(platformMsg);

            try
            {
                SendAlertToBD(message, NORMALALERT);
            }
            catch (Exception e)
            {
                Log.Error(e.ToString());
            }
        }

        public static void CreateCriticalPlatformAlert(string message)
        {
            string platformMsg = "[Platform Critical Alert!] - " + ConfigManager.nodeName + " - " + message;

            try
            {
            }
            catch (Exception e)
            {
                Log.Error(e.ToString());
            }

            Log.Error(platformMsg);

            try
            {
                SendAlertToBD(message, CRITICALALERT);
            }
            catch (Exception e)
            {
                Log.Error(e.ToString());
            }
        }

        private static void SendAlertToBD(string message, string type)
        {
            using (var connSQL = new MySqlConnection(SqlConn.GetConnectionBuilder()))
            {
                connSQL.Open();
                using (var command = connSQL.CreateCommand())
                {
                    command.CommandText = "INSERT INTO alert (message, type, node, timestamp)  VALUES " +
                    "(@message, @type, @node, @timestamp)";

                    command.Parameters.AddWithValue("@message", message);
                    command.Parameters.AddWithValue("@type", type);
                    command.Parameters.AddWithValue("@node", ConfigManager.nodeName);
                    command.Parameters.AddWithValue("@timestamp", DateTime.UtcNow);

                    if (command.ExecuteNonQuery() < 0)
                    {
                        throw new DBWriteException("Unable to update alert table");
                    }
                }
            }
        }

        internal class Message
        {
            public string Text { get; set; }
        }
    }
}
