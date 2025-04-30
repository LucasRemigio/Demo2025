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
using engimatrix.Hubs;

namespace engimatrix.Models
{
    public class ReplyInfo
    {
        public string User { get; set; }
        public DateTime Date { get; set; }

        public ReplyInfo(string user, DateTime date)
        {
            this.User = user;
            this.Date = date;
        }
    }

    public static class ReplyModel
    {
        // Dictionary of emailToken/id with user who are replying to an email at the moment
        private static ConcurrentDictionary<string, List<ReplyInfo>> replyingATMDic = new();

        public static List<ReplyItem> getReplies(string execute_user, string emailToken)
        {
            Dictionary<string, string> dic = new Dictionary<string, string>();
            dic.Add("email_token", emailToken);

            string query = "SELECT * FROM reply WHERE email_token = @email_token ORDER BY id DESC";
            SqlExecuterItem response = SqlExecuter.ExecFunction(query, dic, execute_user, false, "GetRepliesByToken");

            List<ReplyItem> replyEmails = new List<ReplyItem>();

            // Check if the response is null or doesn't have a valid DataTable
            if (response == null)
            {
                return replyEmails;
            }

            foreach (Dictionary<string, string> item in response.out_data)
            {
                int id = Int32.Parse(item["0"]);
                string email_token = item["1"];
                string reply_token = item["2"];
                string from = item["3"];
                string to = item["4"];
                string subject = item["5"];
                string body = item["6"];
                string date = item["7"];
                string replied_by = item["8"];
                string is_read = item["9"];


                if (ConfigManager.isProduction)
                {
                    from = Cryptography.Decrypt(from, reply_token);
                    to = Cryptography.Decrypt(to, reply_token);
                    subject = Cryptography.Decrypt(subject, reply_token);
                    body = Cryptography.Decrypt(body, reply_token);
                }

                // Get reply attachments
                List<ReplyAttachmentItem> attachments = ReplyAttachmentModel.getAttachments(execute_user, reply_token);

                ReplyItem reply = new ReplyItem
                {
                    id = id,
                    from = from,
                    to = to,
                    subject = subject,
                    body = body,
                    date = date,
                    email_token = email_token,
                    reply_token = reply_token,
                    attachments = attachments,
                    replied_by = replied_by,
                    is_read = is_read,
                };

                replyEmails.Add(reply);
            }

            return replyEmails;
        }

        public static ReplyItem getReply(string replyId, string execute_user)
        {
            Dictionary<string, string> dic = new Dictionary<string, string>();
            dic.Add("id", replyId);

            string query = "SELECT * FROM reply WHERE id = @id";
            SqlExecuterItem response = SqlExecuter.ExecFunction(query, dic, execute_user, false, "GetRepliesByToken");
            Dictionary<string, string> item = response.out_data[0];

            int id = Int32.Parse(item["0"]);
            string email_token = item["1"];
            string reply_token = item["2"];
            string from = item["3"];
            string to = item["4"];
            string subject = item["5"];
            string body = item["6"];
            string date = item["7"];
            string replied_by = item["8"];
            string is_read = item["9"];

            if (ConfigManager.isProduction)
            {
                from = Cryptography.Decrypt(from, reply_token);
                to = Cryptography.Decrypt(to, reply_token);
                subject = Cryptography.Decrypt(subject, reply_token);
                body = Cryptography.Decrypt(body, reply_token);
            }

            // Get reply attachments
            List<ReplyAttachmentItem> attachments = ReplyAttachmentModel.getAttachments(execute_user, reply_token);

            ReplyItem reply = new ReplyItem
            {
                id = id,
                from = from,
                to = to,
                subject = subject,
                body = body,
                date = date,
                email_token = email_token,
                reply_token = reply_token,
                attachments = attachments,
                replied_by = replied_by,
                is_read = is_read,
            };

            return reply;
        }

        public static (string, DateTime) GetRepliedInfoByEmailToken(string email_token, string execute_user)
        {
            Dictionary<string, string> dic = new Dictionary<string, string>();
            dic.Add("filteredToken", email_token);
            string query = "SELECT replied_by, date, reply_token " +
                "FROM reply " +
                "WHERE email_token = @filteredToken AND replied_by != '' " +
                "ORDER BY date DESC " +
                "LIMIT 1";
            SqlExecuterItem response = SqlExecuter.ExecFunction(query, dic, execute_user, false, "GetReplyInfoFromFilteredToken");

            if (response.out_data.Count == 0)
            {
                return (String.Empty, DateTime.MinValue);
            }

            Dictionary<string, string> replyRec = response.out_data[0];

            string repliedBy = replyRec["0"];
            string repliedAt = replyRec["1"];
            string replyToken = replyRec["2"];

            if (ConfigManager.isProduction)
            {
                // Decrypt replied_by with reply_token
                repliedBy = Cryptography.Decrypt(repliedBy, replyToken);
            }

            return (repliedBy, DateTime.Parse(repliedAt));
        }

        public static ReplyItem saveReply(ReplyItem replyEmail, string execute_user)
        {
            Dictionary<string, string> dic = new Dictionary<string, string>();
            string reply_token = replyEmail.reply_token;
            if (ConfigManager.isProduction)
            {
                replyEmail.from = Cryptography.Encrypt(replyEmail.from, reply_token);
                replyEmail.to = Cryptography.Encrypt(replyEmail.to, reply_token);
                replyEmail.subject = Cryptography.Encrypt(replyEmail.subject, reply_token);
                replyEmail.body = Cryptography.Encrypt(replyEmail.body, reply_token);
            }
            dic.Add("email_token", replyEmail.email_token);
            dic.Add("reply_token", replyEmail.reply_token);
            dic.Add("from", replyEmail.from);
            dic.Add("to", replyEmail.to);
            dic.Add("subject", replyEmail.subject);
            dic.Add("body", replyEmail.body);
            dic.Add("date", DateTime.Parse(replyEmail.date.ToString()).ToString("yyyy-MM-dd HH:mm:ss"));
            dic.Add("replied_by", replyEmail.replied_by);
            dic.Add("is_read", replyEmail.is_read);

            SqlExecuter.ExecFunction("INSERT INTO reply (`email_token`, `reply_token`, `from`, `to`, `subject`, `body`, `date`, `replied_by`, `is_read`) VALUES (@email_token, @reply_token, @from, @to, @subject, @body, @date, @replied_by, @is_read)", dic, execute_user, false, "InsertEmailRecord");
            dic.Clear();

            SqlExecuterItem responsive = SqlExecuter.ExecFunction("SELECT max(id) as id FROM reply;)", dic, execute_user, false, "GetInsertedEmailId");
            replyEmail.id = Int32.Parse(responsive.out_data[0]["0"]);
            return replyEmail;
        }

        public static ReplyItem saveReply(ReplyItem replyEmail)
        {
            return saveReply(replyEmail, "");
        }

        public static (bool isFirstResponder, ReplyInfo replyInfo) StartReplyConcurrency(string emailToken, string execute_user)
        {
            ReplyInfo newReplyInfo = new(execute_user, DateTime.UtcNow);
            Log.Debug($"StartReplyConcurrency: For email {execute_user} and token {emailToken}");

            // Get or create the list of reply infos for the given email token.
            if (!replyingATMDic.TryGetValue(emailToken, out List<ReplyInfo>? replyInfos) || replyInfos == null)
            {
                replyInfos = [];
                replyingATMDic[emailToken] = replyInfos;
            }

            // Determine if this is the first responder.
            bool isFirstResponder = replyInfos.Count == 0;

            // Add the new reply info.
            replyInfos.Add(newReplyInfo);
            Log.Debug(isFirstResponder
                ? "StartReplyConcurrency: Added new reply info to dictionary"
                : "StartReplyConcurrency: Added new reply info to existing list");

            // If someone else is replying, return one of their infos (other than the current user).
            ReplyInfo otherReply = replyInfos.FirstOrDefault(info => info.User != execute_user) ?? newReplyInfo;
            return (isFirstResponder, otherReply);
        }

        public static void StopReplyConcurrency(string emailToken, string execute_user)
        {
            if (!replyingATMDic.TryGetValue(emailToken, out List<ReplyInfo>? replyInfos))
            {
                throw new ConcurrencyEmailTokenNotFoundException($"Email token given not found: {emailToken}");
            }

            Log.Debug($"StopReplyConcurrency: For email {execute_user} and token {emailToken}");
            replyInfos.RemoveAll(info => info.User == execute_user);
        }
        public static async Task<string> generateResponseAI(string emailToken, bool isReplyToOriginal, string execute_user)
        {
            // Obtain necessary email info by filteredEmail.Token
            Dictionary<string, string> dic = new Dictionary<string, string>();
            dic.Add("emailToken", emailToken);

            string query =
                "SELECT e.id, e.from, e.to, e.subject, e.body, e.date, c.title " +
                "FROM email e " +
                "JOIN filtered_email f ON e.id = f.email " +
                "JOIN category c ON f.category = c.id " +
                "WHERE f.token = @emailToken";

            SqlExecuterItem emailResult = SqlExecuter.ExecFunction(query, dic, execute_user, false, "GetEmailByFilteredEmailToken");

            if (emailResult.out_data.Count == 0)
            {
                throw new EmailNotFoundException("The email with token " + emailToken + " was not found.");
            }

            Dictionary<string, string> emailRec = emailResult.out_data[0];

            if (ConfigManager.isProduction)
            {
                emailRec["2"] = Cryptography.Decrypt(emailRec["2"], emailRec["1"]);
                emailRec["3"] = Cryptography.Decrypt(emailRec["3"], emailRec["1"]);
                emailRec["4"] = Cryptography.Decrypt(emailRec["4"], emailRec["1"]);
            }

            EmailItem email = new EmailItem(emailRec["0"], emailRec["1"], emailRec["2"], emailRec["3"], emailRec["4"], DateTime.Parse(emailRec["5"]));
            string category = emailRec["6"];

            // Obtain attachment info
            List<EmailAttachmentItem> attachments = AttachmentModel.getAttachments(execute_user, email.id);

            // Encapsule all email data to send to openAI as a single string
            StringBuilder emailData = new StringBuilder();
            emailData.Append("************ INICIO INFORMAÇOES DO EMAIL ************\n\n");
            emailData.Append($"Cliente que enviou o e-mail (cumprimenta-o): {email.from} /\nAssunto do e-mail: {email.subject} /\n");
            emailData.Append($"Anexos do e-mail: {String.Join(", ", attachments.Select(attachment => attachment.name))} /\n");
            emailData.Append($"Categoria do e-mail: {category} /\n");
            emailData.Append($"Mensagem do cliente: {email.body}\n");

            if (!isReplyToOriginal)
            {
                List<ReplyItem> replies = ReplyModel.getReplies(execute_user, emailToken);
                emailData.Append("Respostas fornecidas: ");
                foreach (ReplyItem reply in replies)
                {
                    emailData.Append("**** Ínicio da próxima resposta *****");
                    emailData.Append($"Quem respondeu: {reply.from}\n");
                    emailData.Append($"Mensagem enviada: {reply.body} \n");
                    emailData.Append("**** Fim desta resposta *****");
                }
            }

            emailData.Append("\n\n************ FIM INFORMAÇOES DO EMAIL ************");

            return await OpenAI.AIGenerateResponseGivenEmail(emailData.ToString());
        }

        public static void SaveReplyAndAttachmentsToDB(MimeMessage message, string emailToken, string replyToken, string executer_user, bool isRead)
        {
            // 0 indicates is not read, 1 indicates is read
            string read = "0";
            if (isRead)
            {
                read = "1";
            }
            // Save the email in the database
            ReplyItem reply = new(
                -1, emailToken, replyToken, message.From.ToString(), message.To.ToString(), message.Subject, message.HtmlBody, message.Date.ToString(), read, executer_user
            );
            ReplyModel.saveReply(reply);

            // Save the attachments in the database
            ReplyAttachmentModel.SaveEmailAttachments(message, reply.reply_token);
        }

        public static void SaveReplyAndAttachmentsToDB(MimeMessage message, string emailToken, string replyToken, bool isRead)
        {
            SaveReplyAndAttachmentsToDB(message, emailToken, replyToken, "", isRead);
        }

        public static bool SetReplyToRead(string replyToken, string execute_user)
        {
            Dictionary<string, string> dic = new Dictionary<string, string>();
            dic.Add("replyToken", replyToken);

            SqlExecuterItem response = SqlExecuter.ExecFunction("UPDATE reply SET is_read = 1 WHERE reply_token = @replyToken", dic, execute_user, false, "UpdateReplyEmailToRead");

            return response.operationResult;
        }
    }
}
