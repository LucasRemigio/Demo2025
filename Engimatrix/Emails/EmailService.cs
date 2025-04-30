// // Copyright (c) 2024 Engibots. All rights reserved.

using System.Diagnostics.CodeAnalysis;
using System.Net;
using System.Net.Mail;
using MySqlConnector;
using Newtonsoft.Json;
using engimatrix.Config;
using engimatrix.Connector;
using engimatrix.Exceptions;
using engimatrix.ModelObjs;
using engimatrix.Models;
using engimatrix.RepositoryRecords;
using engimatrix.Utils;
using static engimatrix.Emails.EmailSubjectJSON;
using MimeKit;
using NLog;

namespace engimatrix.Emails
{
    public static class EmailService
    {
        private static Dictionary<EmailLangKey, EmailLangValue> emailTemplates = new Dictionary<EmailLangKey, EmailLangValue>(new EmailTemplatesCustomComp());
        private static System.Threading.Timer checkEmails;
        private static string argsSeparator = "||";
        private static bool executingEmailSender = false;
        private static string logoMasterFerroBase64 = "";

        private class EmailTemplatesCustomComp : IEqualityComparer<EmailLangKey>
        {
            public bool Equals(EmailLangKey x, EmailLangKey y)
            {
                return x.Equals(y);
            }

            public int GetHashCode([DisallowNull] EmailLangKey obj)
            {
                return obj.GetHashCode();
            }
        }

        public static void StartEmailService()
        {
            Log.Debug("********************************************");
            Log.Debug("***   Loading email templates messages   ***");

            // Generate the logo
            string imagePath = "./Emails/logo/masterferro_logo.png";
            byte[] imageBytes = File.ReadAllBytes(imagePath);
            logoMasterFerroBase64 = "data:image/png;base64," + Convert.ToBase64String(imageBytes);

            // read email body html templates
            DirectoryInfo d = new("Emails/templates");
            foreach (var bodyEmail in d.GetFiles("*.html"))
            {
                Log.Debug("Email body template file found: " + bodyEmail.FullName);

                using StreamReader readerEmailMessages = new(bodyEmail.FullName);
                string emailMsg = readerEmailMessages.ReadToEnd();
                readerEmailMessages.Close();

                string[] fileNameParts = Path.GetFileNameWithoutExtension(bodyEmail.FullName).Split('-');

                if (fileNameParts.Length != 2)
                {
                    Log.Error("Wrong name for file (should be emailtemplate-lang.html) " + bodyEmail.FullName);
                    continue;
                }

                emailTemplates.Add(new EmailLangKey(fileNameParts[0], fileNameParts[1]), new EmailLangValue(emailMsg));
            }

            // read email subjects from json mapping file
            foreach (var emailSubjects in d.GetFiles("*.json"))
            {
                Log.Debug("Email subjects file found: " + emailSubjects.FullName);

                using StreamReader readerSubjects = new(emailSubjects.FullName);
                string jsonString = readerSubjects.ReadToEnd();
                readerSubjects.Close();
                EmailSubjectJSONArray fileResult = JsonConvert.DeserializeObject<EmailSubjectJSONArray>(jsonString);

                string email = Path.GetFileNameWithoutExtension(emailSubjects.FullName);

                foreach (EmailSubjectJSONObject obj in fileResult.Languages)
                {
                    if (!emailTemplates.ContainsKey(new EmailLangKey(email, obj.Language)))
                    {
                        Log.Error("There exists a subject mapping without body html template - " + email);
                    }

                    emailTemplates[new EmailLangKey(email, obj.Language)].Subject = obj.Subject;
                }
            }

            // timer to check periodically emails to send
            checkEmails = new System.Threading.Timer(x =>

            {
                ExecuteEmailCheck();
            }, null, new TimeSpan(00, 00, 00), new TimeSpan(00, 01, 00));
        }

        private static async Task ExecuteEmailCheck()
        {
            if (executingEmailSender)
            {
                return;
            }

            executingEmailSender = true;

            try
            {
                List<EmailDBRecord> records = new List<EmailDBRecord>();

                using (var connSQL = new MySqlConnection(SqlConn.GetConnectionBuilder()))
                {
                    connSQL.Open();
                    using (var command = connSQL.CreateCommand())
                    {
                        command.CommandText = "SELECT COUNT(*) FROM notification_email WHERE last_send IS NULL";
                        int emailsToSend = (int)(long)command.ExecuteScalar();

                        if (emailsToSend > 0)
                        {
                            command.CommandText = "SELECT * FROM notification_email WHERE last_send IS NULL";
                            using (MySqlDataReader rdr = command.ExecuteReader())
                            {
                                while (rdr.Read())
                                {
                                    int emailId = rdr.GetInt32(0);
                                    string sendTo = rdr.GetString(1);
                                    string subject = rdr.GetString(2);
                                    string email_template = rdr.GetString(3);
                                    string email_args = rdr.GetString(4);
                                    if (ConfigManager.isProduction)
                                    {
                                        email_args = Cryptography.Decrypt(email_args);
                                    }
                                    string email_lang = rdr.GetString(5);
                                    DateTime last_send = rdr[6] == DBNull.Value ? default(DateTime) : rdr.GetDateTime(6);
                                    int retries = rdr.GetInt32(7);

                                    records.Add(new EmailDBRecord(emailId, sendTo, subject, email_template, email_args, email_lang, last_send, retries));
                                }
                            }
                        }
                    }

                    foreach (EmailDBRecord rec in records)
                    {
                        string language = rec.EmailLanguage.Split("-")[0];

                        string template = GetEmailBody(language, rec.EmailTemplate);
                        rec.EmailArgs += argsSeparator + logoMasterFerroBase64;
                        string email_body = ReplaceBodyArgs(template, rec.EmailArgs);
                        string email_subject = GetEmailSubject(language, rec.EmailTemplate);

                        try
                        {
                            MimeMessage message = new MimeMessage();
                            message.From.Add(MailboxAddress.Parse(ConfigManager.SystemEmail));
                            message.To.Add(MailboxAddress.Parse(rec.SendTo));

                            message.Subject = rec.Subject;

                            var bodyBuilder = new BodyBuilder
                            {
                                HtmlBody = email_body
                            };

                            message.Body = bodyBuilder.ToMessageBody();

                            // Send the email to the destinatary
                            using MailKit.Net.Smtp.SmtpClient client = new MailKit.Net.Smtp.SmtpClient();
                            await client.ConnectAsync(ConfigManager.EmailServer, Int32.Parse(ConfigManager.SMTPPort), true);
                            await client.AuthenticateAsync(ConfigManager.SystemEmail, ConfigManager.SystemPassword);
                            await client.SendAsync(message);
                            await client.DisconnectAsync(true);
                        }
                        catch (Exception e)
                        {
                            Log.Error("Error sending email - " + e);

                            using (var command = connSQL.CreateCommand())
                            {
                                command.CommandText = "UPDATE notification_email SET retries=@retries WHERE id_notification_email=\'" + rec.EmailId + "\';";

                                command.Parameters.AddWithValue("@retries", rec.Retries++);

                                if (command.ExecuteNonQuery() < 0)
                                {
                                    throw new DBWriteException("Unable to update notification_email error - " + rec.EmailId);
                                }
                            }
                        }
                        finally
                        {
                            using (var command = connSQL.CreateCommand())
                            {
                                command.CommandText = "UPDATE notification_email SET last_send=@last_send, retries=@retries WHERE id_notification_email=\'" + rec.EmailId + "\';";

                                command.Parameters.AddWithValue("@last_send", DateTime.UtcNow);
                                command.Parameters.AddWithValue("@retries", 0);

                                if (command.ExecuteNonQuery() < 0)
                                {
                                    throw new DBWriteException("Unable to update notification_email success - " + rec.EmailId);
                                }
                            }
                        }
                    }
                }
            }
            catch (Exception e)
            {
                Log.Debug("ERROR - " + e.Message);
            }
            finally
            {
                executingEmailSender = false;
            }
        }

        private static void SendEmailSMTP(EmailDBRecord rec, string emailBody)
        {
            var fromAddress = new MailAddress(ConfigManager.NotificationEmailUsername());
            var toAddress = new MailAddress(rec.SendTo);

            var smtp = new SmtpClient
            {
                Host = ConfigManager.NotificationEmailHostname(),
                Port = Int32.Parse(ConfigManager.NotificationEmailPort()),
                EnableSsl = true,
                UseDefaultCredentials = false,
                DeliveryMethod = SmtpDeliveryMethod.Network,
                Credentials = new NetworkCredential(ConfigManager.NotificationEmailUsername(), ConfigManager.NotificationEmailPassword()),
                Timeout = 20000
            };

            using (var message = new MailMessage(fromAddress, toAddress)
            {
                Subject = rec.Subject,
                IsBodyHtml = true,
                Body = emailBody
            })
            {
                smtp.Send(message);
                smtp.Dispose();
            }
        }

        private static string ReplaceBodyArgs(string body, string args)
        {
            string[] argParts = args.Split(argsSeparator);
            for (int i = 0; i < argParts.Length; i++)
            {
                int currentArgs = i + 1;
                body = body.Replace("[ARG_" + currentArgs + "]", ReplaceEscapeHtmlChars(argParts[i]));
            }

            return body;
        }

        private static string ReplaceEscapeHtmlChars(string input)
        {
            return input.Replace("&", "&amp;").Replace("<", "&lt;").Replace(">", "&gt;").Replace("\"", "&quot;").Replace("'", "&#39;");
        }

        private static string GetEmailSubject(string language, string emailTemplate)
        {
            if (!emailTemplates.ContainsKey(new EmailLangKey(emailTemplate, language)))
            {
                return string.Empty;
            }

            return emailTemplates[new EmailLangKey(emailTemplate, language)].Subject;
        }

        private static string GetEmailBody(string language, string emailTemplate)
        {
            EmailLangKey key = new EmailLangKey(emailTemplate, language);

            if (!emailTemplates.ContainsKey(key))
            {
                return string.Empty;
            }

            string template = emailTemplates[new EmailLangKey(emailTemplate, language)].Body;

            return template;
        }

        private static void AddEmailEntry(string sendTo, string subject, string emailTemplate, string emailArgs, string emailLang)
        {
            using (var connSQL = new MySqlConnection(SqlConn.GetConnectionBuilder()))
            {
                connSQL.Open();
                using (var command = connSQL.CreateCommand())
                {
                    command.CommandText = "INSERT INTO notification_email (send_to, subject, email_template, email_args, email_lang)  VALUES " +
                    "(@send_to, @subject, @email_template, @email_args, @email_lang)";

                    command.Parameters.AddWithValue("@send_to", sendTo);
                    command.Parameters.AddWithValue("@subject", subject);
                    command.Parameters.AddWithValue("@email_template", emailTemplate);
                    if (ConfigManager.isProduction)
                    {
                        emailArgs = Cryptography.Encrypt(emailArgs);
                    }
                    command.Parameters.AddWithValue("@email_args", emailArgs);
                    command.Parameters.AddWithValue("@email_lang", emailLang);

                    if (command.ExecuteNonQuery() < 0)
                    {
                        throw new DBWriteException("Unable to register signup for email - " + sendTo);
                    }
                }
            }

            ExecuteEmailCheck();
        }

        public static void SendSignUpEmail(string regToken, string userName, string email, string language)
        {
            string emailTemplate = "signup";
            string args = userName + argsSeparator + ConfigManager.ClientEndpoint() + "\tochange";
            string subject = GetEmailSubject(language, emailTemplate);
            AddEmailEntry(email, subject, emailTemplate, args, language);
        }

        public static void SendCredentialsEmail(string userName, string email, string password, string language)
        {
            string emailTemplate = "send_credentials";
            string args = userName + argsSeparator + email + argsSeparator + password + argsSeparator + ConfigManager.ClientEndpoint() + argsSeparator + ConfigManager.companyName;
            string subject = GetEmailSubject(language, emailTemplate);
            AddEmailEntry(email, subject, emailTemplate, args, language);
        }

        internal static void SendResetPasswordEmail(string userName, string userEmail, string token, DateTime expirationDate, string language)
        {
            string emailTemplate = "reset_password";
            ConfigManager.ClientEndpoint();
            string args = userName + argsSeparator + ConfigManager.ClientEndpoint() + "/reset-password;email=" + userEmail + ";token=" + token + argsSeparator +
                expirationDate.ToString() + argsSeparator + ConfigManager.companyName + argsSeparator + ConfigManager.ClientEndpoint();
            string subject = GetEmailSubject(language, emailTemplate);
            AddEmailEntry(userEmail, subject, emailTemplate, args, language);
        }

        internal static void ResetPasswordConfirmationEmail(string userName, string userEmail, string language)
        {
            string emailTemplate = "reset_password_confirmation";
            string args = userName + argsSeparator + ConfigManager.companyName + argsSeparator + ConfigManager.ClientEndpoint();
            string subject = GetEmailSubject(language, emailTemplate);
            AddEmailEntry(userEmail, subject, emailTemplate, args, language);
        }

        internal static void SendAuth2FToClient(string email, string auth2f_code, string language)
        {
            // language = "en";
            string emailTemplate = "send_auth2f_code";
            string args = email + argsSeparator + auth2f_code + argsSeparator + ConfigManager.ClientEndpoint() + argsSeparator + ConfigManager.companyName;
            string subject = GetEmailSubject(language, emailTemplate);
            AddEmailEntry(email, subject, emailTemplate, args, language);
        }

        internal static void SendFormResponse(string userOperation, string operation, string language)
        {
            //language = "en";
            string emailTemplate = "send_form_request";
            string args = userOperation + argsSeparator + operation + argsSeparator + ConfigManager.companyName;
            string subject = GetEmailSubject(language, emailTemplate);
            List<UserItem> allUsers = UserModel.GetUsers(string.Empty, string.Empty);
            foreach (UserItem user in allUsers)
            {
                if (user.role.ToLower().Equals("administrator"))
                {
                    AddEmailEntry(user.email, "Form Response | " + userOperation, emailTemplate, args, language);
                }
            }
        }

        internal static async Task NotificateClientAsync(string email, string subject, string body, string language, string sourceEmail)
        {
            try
            {
                string singnature = $" <img src='data:image/png;base64,{ConfigManager.Logo}' alt='hrb' />";
                string newBody = "";

                foreach (string var in body.Split("\n"))
                {
                    newBody += "<p>" + var + "</p>";
                };
                newBody = newBody + "<br/>" + singnature;

                List<string> toRecipients = new List<string>();
                toRecipients = new List<string> { email };

                // CC Recipients
                List<string> ccRecipients = new List<string> { ConfigManager.ccConfirmations };

                if (ConfigManager.sendAutomaticallyDrafts)
                {
                    //await MSGraphAPI.CreateAndSendEmailAsync(toRecipients, ccRecipients, subject, newBody, sourceEmail);
                }
            }
            catch (Exception e)
            {
                Log.Error("Something went wrong: " + e.ToString());
            }
        }

        internal static void SendTokenToClient(string email, string subject, string body, string language)
        {
            //language = "en";
            string emailTemplate = "send_notifications";
            string args = email + argsSeparator + body + argsSeparator + ConfigManager.ClientEndpoint() + argsSeparator + ConfigManager.companyName;
            AddEmailEntry(email, subject, emailTemplate, args, language);
        }
    }
}
