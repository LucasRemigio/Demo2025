// // Copyright (c) 2024 Engibots. All rights reserved.

using MySqlConnector;
using engimatrix.Connector;
using engimatrix.Exceptions;
using engimatrix.ModelObjs;
using engimatrix.Utils;
using System.Globalization;
using System.Reflection;

namespace engimatrix.Models
{
    public class SqlExecuter
    {
        public static SqlExecuterItem ExecFunction(string query, Dictionary<string, string> dic, string executer_user, bool isToRecord, string operation_context)
        {
            Dictionary<string, string> out_dic = new Dictionary<string, string>();
            string out_query = query;

            // Execute Query
            using (var connSQL = new MySqlConnection(SqlConn.GetConnectionBuilder()))
            {
                SqlExecuterItem output = new SqlExecuterItem();
                output.out_data = new List<Dictionary<string, string>>();
                connSQL.Open();
                using (var command = connSQL.CreateCommand())
                {
                    command.CommandText = query;

                    foreach (KeyValuePair<string, string> item in dic)
                    {
                        command.Parameters.AddWithValue(item.Key, item.Value);
                        out_query = out_query.Replace(item.Key, String.IsNullOrEmpty(item.Value) ? null : item.Value.ToLower().Trim().Equals("true") ? "1" : item.Value.ToLower().Trim().Equals("false") ? "0" : item.Value);
                    }

                    try
                    {
                        MySqlDataReader rdr = null;

                        // Executa comando SQL
                        if (out_query.Split(" ")[0].ToUpper(CultureInfo.InvariantCulture) == "SELECT" || out_query.Split(" ")[0].ToUpper(CultureInfo.InvariantCulture) == "WITH")
                        {
                            using (rdr = command.ExecuteReader())
                            {
                                // Enquanto houver registos a adicionar
                                while (rdr.Read())
                                {
                                    int contador = 0;
                                    out_dic = new Dictionary<string, string>();

                                    // Enquanto houver campos para adicionar
                                    while (contador < rdr.FieldCount)
                                    {
                                        out_dic.Add(contador.ToString(), rdr[contador].ToString());
                                        contador++;
                                    }

                                    output.out_data.Add(out_dic);
                                }
                            }
                        }
                        else
                        {
                            if (command.ExecuteNonQuery() < 0)
                                throw new DBWriteException("Unable to make this operation");
                        }

                        output.operationResult = true;
                    }
                    catch (Exception error)
                    {
                        Log.Error(error.Message.ToString());
                        output.operationResult = false;
                        output.out_data = null;
                    }
                }

                // Insert into de Logs
                if (isToRecord)
                {
                    using (var command = connSQL.CreateCommand())
                    {
                        command.CommandText = "INSERT INTO logs (operation,user_operation,operation_state,date_time, operation_context) VALUES (@operation,@user_operation,@Operation_State,@datetime,@operation_context)";
                        command.Parameters.AddWithValue("@Operation_State", output.operationResult ? "Success" : "Error");
                        command.Parameters.AddWithValue("@datetime", DateTime.UtcNow);
                        command.Parameters.AddWithValue("@operation", out_query);
                        command.Parameters.AddWithValue("@user_operation", executer_user);
                        command.Parameters.AddWithValue("@operation_context", operation_context);
                        command.ExecuteNonQuery();
                    }
                }

                if (output.operationResult == false)
                {
                    throw new DBWriteException("Error");
                }

                return output;
            }
        }

        public static SqlExecuterItem ExecuteFunction(string query, Dictionary<string, string> dic, string executer_user, bool isToRecord, string operation_context)
        {
            string out_query = query;
            SqlExecuterItem output = new()
            {
                out_data = []
            };

            // Open database connection and prepare SQL command for execution
            using MySqlConnection connSQL = new(SqlConn.GetConnectionBuilder());
            connSQL.Open();
            using MySqlCommand command = connSQL.CreateCommand();
            command.CommandText = query;

            // Add values to the command execution, and replace placeholders in `out_query` for readability in logs
            foreach (KeyValuePair<string, string> item in dic)
            {
                // Convert boolean strings to flags
                string? processedValue = item.Value?.ToLower().Trim() switch
                {
                    "true" => "1",
                    "false" => "0",
                    _ => item.Value
                };

                // Add parameter to the command
                command.Parameters.AddWithValue(item.Key, processedValue);

                out_query = out_query.Replace(item.Key, processedValue);
            }

            try
            {
                // Read function
                if (out_query.StartsWith("SELECT", StringComparison.OrdinalIgnoreCase) || out_query.StartsWith("WITH", StringComparison.OrdinalIgnoreCase))
                {
                    // Execute the query and retrieve results
                    using MySqlDataReader rdr = command.ExecuteReader();

                    // Loop through each row in the result set
                    while (rdr.Read())
                    {
                        Dictionary<string, string> out_dic = [];

                        // Loop through each column in the row
                        for (int i = 0; i < rdr.FieldCount; i++)
                        {
                            out_dic[rdr.GetName(i)] = rdr[i].ToString();
                        }

                        output.out_data.Add(out_dic);
                    }
                }
                // For non-SELECT queries, check if rows were affected
                else
                {
                    int rowsAffected = command.ExecuteNonQuery();
                    if (rowsAffected < 0)
                    {
                        throw new DBWriteException("Unable to make this operation, no row affected");
                    }
                    output.rowsAffected = rowsAffected;
                }

                output.operationResult = true;
            }
            catch (Exception error)
            {
                Log.Error(error.Message.ToString());
                output.operationResult = false;
                output.out_data = null;
            }

            // If `isToRecord` is true, insert an operation log entry
            if (isToRecord)
            {
                using MySqlCommand logCommand = connSQL.CreateCommand();
                logCommand.CommandText = "INSERT INTO logs (operation,user_operation,operation_state,date_time, operation_context) VALUES (@operation,@user_operation,@Operation_State,@datetime,@operation_context)";

                logCommand.Parameters.AddWithValue("@Operation_State", output.operationResult ? "Success" : "Error");
                logCommand.Parameters.AddWithValue("@datetime", DateTime.UtcNow);
                logCommand.Parameters.AddWithValue("@operation", out_query);
                logCommand.Parameters.AddWithValue("@user_operation", executer_user);
                logCommand.Parameters.AddWithValue("@operation_context", operation_context);

                logCommand.ExecuteNonQuery();
            }

            if (output.operationResult == false)
            {
                throw new DBWriteException("Error");
            }

            return output;
        }

        public static List<T> ExecuteFunction<T>(
            string query,
            Dictionary<string, string> dic,
            string executerUser,
            bool isToRecord,
            string operationContext
        ) where T : new()
        {
            List<T> results = [];
            string out_query = query;

            using MySqlConnection connSQL = new(SqlConn.GetConnectionBuilder());
            connSQL.Open();
            using MySqlCommand command = connSQL.CreateCommand();
            command.CommandText = query;

            foreach (KeyValuePair<string, string> item in dic)
            {
                string? processedValue = item.Value?.ToLower().Trim() switch
                {
                    "true" => "1",
                    "false" => "0",
                    _ => item.Value
                };

                command.Parameters.AddWithValue(item.Key, processedValue);
                out_query = out_query.Replace(item.Key, processedValue);
            }

            try
            {
                if (out_query.StartsWith("SELECT", StringComparison.OrdinalIgnoreCase) ||
                    out_query.StartsWith("WITH", StringComparison.OrdinalIgnoreCase))
                {
                    using MySqlDataReader rdr = command.ExecuteReader();
                    while (rdr.Read())
                    {
                        // TODO: Save the datatypes on the first cycle and then use the saved ones instead of always computing the required datatype
                        T item = new();
                        for (int i = 0; i < rdr.FieldCount; i++)
                        {
                            string columnName = rdr.GetName(i);
                            PropertyInfo? property = typeof(T).GetProperty(columnName);
                            // Skip if property not found or value is DBNull
                            if (property == null || Equals(rdr[columnName], DBNull.Value))
                            {
                                continue;
                            }

                            try
                            {
                                // Get property type (and handle nullable types)
                                Type propertyType = property.PropertyType;
                                Type underlyingType = Nullable.GetUnderlyingType(propertyType) ?? propertyType;

                                // Convert value to the correct underlying type
                                object convertedValue = Convert.ChangeType(rdr[columnName], underlyingType);
                                property.SetValue(item, convertedValue);
                            }
                            catch (InvalidCastException)
                            {
                                // Log conversion error but continue with other properties
                                Log.Warning($"Could not convert '{columnName}' value to {property.PropertyType.Name}");
                            }
                        }
                        results.Add(item);
                    }
                }
                else if (command.ExecuteNonQuery() < 0)
                {
                    throw new DBWriteException("Unable to make this operation");
                }
            }
            catch (Exception error)
            {
                Log.Error(error.Message);
                throw;
            }

            // Optionally record the operation log if needed...
            if (isToRecord)
            {
                using MySqlCommand logCommand = connSQL.CreateCommand();
                logCommand.CommandText = "INSERT INTO logs (operation, user_operation, operation_state, date_time, operation_context) " +
                                         "VALUES (@operation, @user_operation, @Operation_State, @datetime, @operation_context)";
                logCommand.Parameters.AddWithValue("@Operation_State", results.Count != 0 ? "Success" : "Error");
                logCommand.Parameters.AddWithValue("@datetime", DateTime.UtcNow);
                logCommand.Parameters.AddWithValue("@operation", out_query);
                logCommand.Parameters.AddWithValue("@user_operation", executerUser);
                logCommand.Parameters.AddWithValue("@operation_context", operationContext);
                logCommand.ExecuteNonQuery();
            }

            return results;
        }

    }
}
