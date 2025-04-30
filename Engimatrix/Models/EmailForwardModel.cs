// // Copyright (c) 2024 Engibots. All rights reserved.

using System.Collections.Concurrent;
using System.Data;
using engimatrix.Config;
using engimatrix.Connector;
using engimatrix.Exceptions;
using engimatrix.ModelObjs;
using engimatrix.Utils;
using Engimatrix.ModelObjs;
using Engimatrix.Models;
using Smartsheet.Api.Models;
using Smartsheet.Api.OAuth;

using MimeKit.Utils;

using MimeKit;
using Engimatrix.Utils;
using System.Text;
using engimatrix.Views;

namespace engimatrix.Models
{
    public static class EmailForwardModel
    {
        public static List<EmailForwardItem> GetEmailForwardsByTokenOrId(string emailIdentifier, string execute_user)
        {
            Dictionary<string, string> dic = [];
            string whereClause = "";

            if (int.TryParse(emailIdentifier, out int emailId))
            {
                dic.Add("email_id", emailId.ToString());
                whereClause = "WHERE email_id = @email_id";
            }
            else
            {
                dic.Add("email_token", emailIdentifier);
                whereClause = "WHERE email_token = @email_token";
            }

            // Common query parts
            string query = "SELECT id, COALESCE(email_token, '') as email_token, COALESCE(email_id, 0) as email_id, " +
                           "forwarded_by, forwarded_to, forwarded_at, message " +
                           "FROM email_forward " + whereClause + " ORDER BY forwarded_at DESC";


            SqlExecuterItem response = SqlExecuter.ExecFunction(query, dic, execute_user, false, "GetEmailForwards");

            List<EmailForwardItem> forwards = [];

            // If no data was returned
            if (response == null || response.out_data.Count == 0)
            {
                // Return an empty list
                return forwards;
            }

            // Loop through the data and create EmailForward objects
            foreach (Dictionary<string, string> item in response.out_data)
            {
                int id = Int32.Parse(item["0"]);
                string email_token = item["1"];
                int? email_id = null;
                if (!String.IsNullOrEmpty(item["2"]))
                {
                    if (int.TryParse(item["2"], out int parsedEmailId))
                    {
                        email_id = parsedEmailId;
                    }
                }

                string forwarded_by = item["3"];
                string forwarded_to = item["4"];
                DateTime forwarded_at = DateTime.Parse(item["5"]);
                string? message = !string.IsNullOrEmpty(item["6"]) ? item["6"] : null;

                EmailForwardItem forward = new(id, email_token, email_id, forwarded_by, forwarded_to, forwarded_at, message);
                forwards.Add(forward);
            }

            return forwards;
        }

        public static EmailForwardItem SaveEmailForward(EmailForwardItem forward, string execute_user)
        {
            string? message = forward.message;
            if (!string.IsNullOrEmpty(message) && message.Length > 1000)
            {
                message = message[..1000];
            }

            Dictionary<string, string> dic = new()
            {
                // Add the token or null to the dictionary
                { "email_token", string.IsNullOrEmpty(forward.email_token) ? null : forward.email_token },
                // Add the ID or null
                { "email_id", forward.email_id.HasValue ? forward.email_id.Value.ToString() : (string)null },
                { "forwarded_by", forward.forwarded_by },
                { "forwarded_to", forward.forwarded_to },
                { "forwarded_at", forward.forwarded_at.ToString("yyyy-MM-dd HH:mm:ss") },
                { "message", message ?? "" }
            };


            string query = "INSERT INTO email_forward (email_token, email_id, forwarded_by, forwarded_to, forwarded_at, message) " +
                           "VALUES (IFNULL(@email_token, NULL), IFNULL(@email_id, NULL), @forwarded_by, @forwarded_to, @forwarded_at, NULLIF(@message, '')) ";


            SqlExecuter.ExecFunction(query, dic, execute_user, false, "SaveEmailForward");

            // Retrieve the inserted record's ID
            SqlExecuterItem response = SqlExecuter.ExecFunction("SELECT MAX(id) AS id FROM email_forward", [], execute_user, false, "GetInsertedEmailForwardId");

            if (response != null && response.out_data.Count > 0)
            {
                forward.id = Int32.Parse(response.out_data[0]["0"]);
            }

            return forward;
        }

        public static async Task FwdEmail(string emailToken, List<string> emailsToFwd, string? message, string executer_user)
        {
            // Remove duplicates
            emailsToFwd = emailsToFwd.Distinct().ToList();

            if (!string.IsNullOrEmpty(message) && message.Length > 1000)
            {
                message = message[..1000];
            }

            // Remove invalid addresses
            emailsToFwd.RemoveAll(email => !Util.IsValidInputEmail(email));

            try
            {
                await MasterFerro.FwdEmail(emailToken, emailsToFwd, message, executer_user);

            }
            catch (Exception e)
            {
                Log.Debug("Error in fwdEmail EmailForwardModel: " + e);
                throw;
            }

            foreach (string email_to in emailsToFwd)
            {
                int? id = null;
                string token = null;
                if (emailToken.Length < 10)
                {
                    // Id is set, token remains null
                    id = Int32.Parse(emailToken);
                }
                else
                {
                    // Token is set, id remains null
                    token = emailToken;
                }
                // Create an EmailForward object for each forwarded email
                EmailForwardItem forward = new()
                {
                    email_token = token,
                    email_id = id,
                    forwarded_by = executer_user,
                    forwarded_to = email_to,
                    forwarded_at = DateTime.UtcNow,
                    message = message
                };

                EmailForwardModel.SaveEmailForward(forward, executer_user);
            }
        }

        public static List<string> GetMostForwardedEmailRecipients(string execute_user)
        {
            Dictionary<string, string> dic = [];

            string query = "SELECT forwarded_to, COUNT(*) as forward_count " +
                "FROM email_forward " +
                "GROUP BY forwarded_to " +
                "ORDER BY forward_count DESC; ";

            SqlExecuterItem response = SqlExecuter.ExecFunction(query, dic, execute_user, false, "GetEmailForwardsByToken");

            List<string> forwards = [];

            // If no data was returned
            if (response == null || response.out_data.Count == 0)
            {
                // Return an empty list
                return forwards;
            }

            // Loop through the data and create EmailForward objects
            foreach (Dictionary<string, string> item in response.out_data)
            {
                forwards.Add(item["0"]);
            }

            return forwards;
        }
    }
}
