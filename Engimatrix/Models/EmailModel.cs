// // Copyright (c) 2024 Engibots. All rights reserved.

using engimatrix.ModelObjs;
using Engimatrix.ModelObjs;
using engimatrix.Models;
using engimatrix.Config;
using engimatrix.Utils;
using MimeKit;
using engimatrix.Exceptions;
using Engimatrix.Processes;

namespace Engimatrix.Models
{
    public class EmailModel
    {
        private static readonly HashSet<string> commonDomains = new(StringComparer.OrdinalIgnoreCase)
        {
            "gmail.com", "hotmail.com", "outlook.com", "yahoo.com", "sapo.pt", "live.com", "icloud.com"
        };

        public static EmailItem getEmail(string id, string execute_user)
        {
            Dictionary<string, string> dic = new Dictionary<string, string>();
            dic.Add("emailId", id);

            string query = "SELECT e.id, e.from, e.to, e.subject, e.body, e.date, e.cc, e.bcc FROM email e WHERE e.id = @emailId";

            SqlExecuterItem emailRec = SqlExecuter.ExecFunction(query, dic, execute_user, false, "GetSpecificFilteredEmail");

            if (!emailRec.operationResult || emailRec.out_data.Count <= 0)
            {
                throw new InvalidOperationException($"No email found with id: {id}");
            }

            Dictionary<string, string> emailRecDic = emailRec.out_data[0];

            EmailItem email = new EmailItem(emailRecDic["0"], emailRecDic["1"], emailRecDic["2"], emailRecDic["6"], emailRecDic["7"], emailRecDic["3"], emailRecDic["4"], DateTime.Parse(emailRecDic["5"]));

            return DecryptEmailItem(email);
        }

        public static List<EmailItem> GetAll(string executeUser)
        {
            string query = "SELECT e.id, e.from, e.to, e.subject, e.body, e.date FROM email e ORDER BY e.date DESC";

            List<EmailItem> emails = SqlExecuter.ExecuteFunction<EmailItem>(query, [], executeUser, false, "GetAllEmails");
            return emails;
        }

        public static EmailItem? CheckIfEmailAlreadyExists(MimeMessage message, string account, int minutes)
        {
            string subject = message.Subject.ToString();
            if (ConfigManager.isProduction)
            {
                string key = message.From.ToString();
                subject = Cryptography.Encrypt(subject, key);
                account = Cryptography.Encrypt(account, key);
            }

            Dictionary<string, string> dic = new()
            {
                { "@from", message.From.ToString() },
                { "@to", account },
                { "@subject", subject },
                { "@minutes", minutes.ToString() }
            };

            string query = "SELECT e.id, e.from, e.to, e.subject, e.body, e.date " +
                "FROM email e " +
                "WHERE e.from = @from " +
                "AND e.subject = @subject " +
                "AND e.date >= NOW() - INTERVAL @minutes MINUTE " +
                // We want to check if email is not in our name
                "AND e.to NOT LIKE CONCAT('%', @to, '%') " +
                "LIMIT 1";

            SqlExecuterItem emailRec = SqlExecuter.ExecFunction(query, dic, "system", false, "GetLatestEmailsBySpecificUser");

            // User has no emails, return immediatly an empty list
            if (emailRec.out_data.Count == 0)
            {
                return null;
            }

            Dictionary<string, string> dicRec = emailRec.out_data[0];
            EmailItem email = new(dicRec["0"], dicRec["1"], dicRec["2"], dicRec["3"], dicRec["4"], DateTime.Parse(dicRec["5"]));

            return DecryptEmailItem(email);
        }

        public static List<string> GetSpamBlacklistedEmails(string executeUser)
        {
            Dictionary<string, string> dic = [];
            dic.Add("@CategoryId", CategoryConstants.CategoryCode.SPAM.ToString());
            string query = @"SELECT DISTINCT e.from AS email_address 
                FROM email e 
                JOIN filtered_email fe ON e.id = fe.email
                WHERE fe.category = @CategoryId";

            SqlExecuterItem emailRec = SqlExecuter.ExecuteFunction(query, dic, executeUser, false, "GetSpamBlacklistedEmails");

            List<string> blacklistedEmails = [];
            foreach (Dictionary<string, string> record in emailRec.out_data)
            {
                blacklistedEmails.Add(record["email_address"]);
            }

            return blacklistedEmails;
        }

        public static List<string> GetSpamBlacklistedDomains(string executeUser)
        {
            List<string> emailAddresses = GetSpamBlacklistedEmails(executeUser);

            /* The emails can be in the following formats:
                "Lucas Remigio" <lucas.remigio@engibots.com>
                lucas.remigio@engibots.com
            */

            List<string> blacklistedDomains = [];
            foreach (string email in emailAddresses)
            {
                string domain = EmailHelper.ExtractDomainFromEmail(email);

                if (!blacklistedDomains.Contains(domain) && !string.IsNullOrEmpty(domain) && !commonDomains.Contains(domain))
                {
                    blacklistedDomains.Add(domain);
                }
            }

            return blacklistedDomains;
        }

        public static bool IsEmailBlacklisted(string email, string executeUser)
        {
            List<string> blacklistedDomains = GetSpamBlacklistedDomains(executeUser);

            string emailDomain = EmailHelper.ExtractDomainFromEmail(email);

            // Check if the email is in the list of blacklisted emails or domains
            if (blacklistedDomains.Contains(emailDomain))
            {
                Log.Debug($"Email domain {emailDomain} is blacklisted.");
                return true;
            }

            return false;
        }

        public static bool IsAnyEmailMessageFromBlacklisted(InternetAddressList from, string executeUser)
        {
            List<string> blacklistedDomains = GetSpamBlacklistedDomains(executeUser);

            foreach (InternetAddress address in from)
            {
                string emailDomain = EmailHelper.ExtractDomainFromEmail(address.ToString());

                // Check if the email is in the list of blacklisted emails or domains
                if (blacklistedDomains.Contains(emailDomain))
                {
                    Log.Debug($"Email domain {emailDomain} is blacklisted.");
                    return true;
                }
            }

            return false;
        }

        public static EmailItem DecryptEmailItem(EmailItem email)
        {
            if (ConfigManager.isProduction)
            {
                email.to = Cryptography.Decrypt(email.to, email.from);
                email.subject = Cryptography.Decrypt(email.subject, email.from);
                email.body = Cryptography.Decrypt(email.body, email.from);
            }

            return email;
        }

        public static async Task<int> CreateEmailAndSaveAttachments(EmailItem email, List<IFormFile> attachments, string execute_user)
        {
            Log.Debug($"Creating email with from {email.from} to {email.to} and subject {email.subject}");

            if (!ConfigManager.isProduction)
            {
                if (!email.to.Contains("remigio"))
                {
                    Log.Warning("Tried sending an email in dev env to a non-remigio address. This is not allowed.");
                    throw new InvalidOperationException("Email not allowed in dev env");
                }
            }

            int emailId = SaveEmail(email, execute_user);

            if (emailId == -1)
            {
                throw new InvalidOperationException("Error creating email");
            }

            if (attachments == null || attachments.Count == 0)
            {
                return emailId;
            }

            try
            {
                await SaveIFormAttachments(attachments, emailId, execute_user);
            }
            catch (System.Exception ex)
            {
                throw new AttachmentNotValidException(ex.Message);
            }

            return emailId;
        }

        public static int SaveEmail(EmailItem email, string execute_user)
        {
            // first clean up some variables, like trimming and removing extra spaces
            // they can be comma separated and have spaces between them
            email.to = email.to.Replace(" ", string.Empty);
            // for cc and bcc first we need to make sure they exist
            email.cc ??= string.Empty;
            email.bcc ??= string.Empty;
            email.cc = email.cc.Replace(" ", string.Empty);
            email.bcc = email.bcc.Replace(" ", string.Empty);

            // Handle email
            Dictionary<string, string> dic = new Dictionary<string, string>();
            string date = DateTime.Parse(email.date.ToString()).ToString("yyyy-MM-dd HH:mm:ss");
            if (ConfigManager.isProduction)
            {
                email.to = Cryptography.Encrypt(email.to, email.from);
                email.subject = Cryptography.Encrypt(email.subject, email.from);
                email.body = Cryptography.Encrypt(email.body, email.from);
            }
            dic.Add("from", email.from);
            dic.Add("to", email.to);
            dic.Add("subject", email.subject);
            dic.Add("body", email.body);
            dic.Add("date", date);
            dic.Add("cc", email.cc);
            dic.Add("bcc", email.bcc);

            string query = "INSERT INTO email (`from`, `to`, `cc`,`bcc`, `subject`, `body`, `date`) VALUES (@from, @to, @cc, @bcc, @subject, @body, @date)";
            SqlExecuterItem response = SqlExecuter.ExecFunction(query, dic, execute_user, false, "InsertEmailRecord");

            dic.Clear();
            if (!response.operationResult)
            {
                Log.Debug("Ocorreu um erro a guardar email");
                return -1;
            }

            // Get last id email created
            response = SqlExecuter.ExecFunction("SELECT max(id) as id FROM email", dic, execute_user, false, "GetInsertedEmailId");
            email.id = response.out_data[0]["0"];
            return Int32.Parse(email.id);
        }

        public static async Task SaveIFormAttachments(List<IFormFile> attachments, int emailId, string execute_user)
        {
            foreach (IFormFile attachment in attachments)
            {
                try
                {
                    // Store the file content in a byte array immediately
                    byte[] fileBytes;
                    using (MemoryStream memoryStream = new())
                    {
                        await attachment.CopyToAsync(memoryStream);
                        fileBytes = memoryStream.ToArray();  // Convert the stream to a byte array
                    }

                    // Process the byte array (this avoids re-reading the stream)
                    string b64content = Convert.ToBase64String(fileBytes);
                    string filesize = fileBytes.Length.ToString();
                    string fileName = attachment.FileName;

                    // Save the attachment using the byte array content
                    EmailAttachmentItem attachmentItem = new(string.Empty, emailId.ToString(), fileName, filesize, b64content);
                    AttachmentModel.saveAttachment(attachmentItem);

                    Log.Debug($"Attachment {fileName} saved.");
                }
                catch (System.Exception ex)
                {
                    throw new InvalidOperationException(ex.Message);
                }
            }
        }

        public static List<string> GetMailboxes()
        {
            return ConfigManager.MailboxAccount;
        }

        public static List<EmailItem> GetEmailsCreatedInPlatform(string startDate, string endDate, string execute_user)
        {
            List<EmailItem> emails = new List<EmailItem>();

            // In the table emails, we can check if the email originated from the platform by checking if the From field is
            // any of the configured mailbox accounts. Any reply on the platform goes to the reply table, so if the record
            // is on the email table and is from one of the configured users, it must have been originated from the platform

            List<string> configuredMailboxes = GetMailboxes();
            // Enclose mailbox params in quotes for SQL
            List<string> mailboxParams = configuredMailboxes.Select(mailbox => $"'{mailbox}'").ToList();

            Dictionary<string, string> dic = new Dictionary<string, string>();

            string query = "SELECT e.id, e.from, e.to, e.subject, e.date " +
                "FROM email e " +
               $"WHERE e.from IN ({String.Join(",", mailboxParams)}) ";

            if (!String.IsNullOrEmpty(startDate))
            {
                dic.Add("@start_date", startDate);
                query += "AND DATE(e.date) >= @start_date ";
            }
            if (!String.IsNullOrEmpty(endDate))
            {
                dic.Add("@end_date", endDate);
                query += "AND DATE(e.date) <= @end_date ";
            }

            query += $"ORDER BY e.date DESC";

            SqlExecuterItem result = SqlExecuter.ExecFunction(query, dic, execute_user, false, "SelectEmailsFromPlatform");

            if (!result.operationResult || result.out_data.Count <= 0)
            {
                // Empty list of emails
                return emails;
            }

            foreach (Dictionary<string, string> email in result.out_data)
            {
                EmailItem emailItem = new EmailItem(email["0"], email["1"], email["2"], email["3"], String.Empty, DateTime.Parse(email["4"]));

                if (ConfigManager.isProduction)
                {
                    emailItem.to = Cryptography.Decrypt(emailItem.to, emailItem.from);
                    emailItem.subject = Cryptography.Decrypt(emailItem.subject, emailItem.from);
                }

                emailItem.to = emailItem.to.Trim();

                emails.Add(emailItem);
            }

            return emails;
        }

        public static List<string> GetSentEmailAddressesList(string executeUser)
        {
            List<string> configuredMailboxes = GetMailboxes();

            Dictionary<string, string> dic = [];
            List<string> mailboxParams = configuredMailboxes.Select((mailbox, index) =>
            {
                string paramName = $"@mailbox{index}";
                dic.Add(paramName, mailbox);
                return paramName;
            }).ToList();

            string mailboxForQuery = string.Join(",", mailboxParams);

            string query = "SELECT e.to, e.from, COUNT(*) AS sentCount " +
                           "FROM email e " +
                           $"WHERE e.from IN ({mailboxForQuery}) " +
                           "GROUP BY e.to, e.from " +
                           "ORDER BY sentCount DESC";

            SqlExecuterItem response = SqlExecuter.ExecuteFunction(query, dic, executeUser, false, "GetSentEmailAddressesList");

            if (!response.operationResult || response.out_data.Count <= 0)
            {
                Log.Debug("GetSentEmailAddressesList: No emails found.");
                return [];
            }

            List<string> addressesList = response.out_data
                .Select(email =>
                {
                    string emailAddress = email["to"];

                    // Decrypt if in production.
                    if (ConfigManager.isProduction)
                    {
                        emailAddress = Cryptography.Decrypt(emailAddress, email["from"]);
                    }

                    // Extract the email if it contains angle brackets.
                    if (emailAddress.Contains('<'))
                    {
                        emailAddress = EmailHelper.GetEmailBetweenTriangleBrackets(emailAddress);
                    }

                    return emailAddress.Trim();
                })
                .Distinct()
                .ToList();

            return addressesList;
        }

        public static void PatchEmailDestinatary(string newDestinatary, string emailId, string execute_user)
        {
            Dictionary<string, string> dic = new Dictionary<string, string>();
            dic.Add("destinatary", newDestinatary);
            dic.Add("id", emailId);

            string query = "UPDATE email SET email.from = @destinatary WHERE email.id = @id";

            SqlExecuter.ExecFunction(query, dic, execute_user, true, "UpdateEmailFrom");
        }

        public static string GetReplyTemplate(string orderToken)
        {
            OrderItem? order = OrderModel.GetOrderByToken(orderToken, "system") ?? throw new InputNotValidException($"Order not found with token {orderToken}");

            string template = string.Empty;
            if (order.is_draft)
            {
                template = GetQuotationReplyTemplate(order.token);
            }
            else
            {
                template = GetOrderReplyTemplate(order.token);
            }

            return template;
        }

        public static string GetQuotationReplyTemplate(string orderToken)
        {
            OrderItem? order = OrderModel.GetOrderByToken(orderToken, "system") ?? throw new InputNotValidException($"Order not found with token {orderToken}");
            int categoryId = order.is_draft ? CategoryConstants.CategoryCode.COTACOES_ORCAMENTOS : CategoryConstants.CategoryCode.ENCOMENDAS;

            string template = ProcessOrders.CreateRequestValidationEmailBody(order.token, "pt", categoryId);

            return template;
        }

        public static string GetOrderReplyTemplate(string orderToken)
        {
            OrderItem? order = OrderModel.GetOrderByToken(orderToken, "system") ?? throw new InputNotValidException($"Order not found with token {orderToken}");

            int? status = FilteringModel.GetFilteredStatusFromEmailToken(order.email_token, "System");
            if (!status.HasValue)
            {
                throw new InvalidOperationException($"Status not found for email token {order.email_token}");
            }

            string template = ProcessOrders.CreateOrderConfirmationEmailBody(order, "pt", status.Value);
            return template;
        }

    }
}
