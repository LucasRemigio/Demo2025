// // Copyright (c) 2024 Engibots. All rights reserved.

using MySqlConnector;

namespace engimatrix.Connector
{
    public static class SqlConn
    {
        private static MySqlConnectionStringBuilder builder = null;

        public static void ConfigureConnection(
            string sqlServer,
            int sqlPort,
            string sqlDatabase,
            string sqlUser,
            string sqlPassowrd)
        {
            builder = new MySqlConnectionStringBuilder
            {
                Server = sqlServer,
                Port = (uint)sqlPort,
                Database = sqlDatabase,
                UserID = sqlUser,
                Password = sqlPassowrd,
                SslMode = MySqlSslMode.Preferred
            };
        }

        public static string GetConnectionBuilder()
        {
            if (builder != null)
            {
                return builder.ConnectionString;
            }

            return String.Empty;
        }
    }
}
