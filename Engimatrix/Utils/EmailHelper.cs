// // Copyright (c) 2024 Engibots. All rights reserved.

using System.Globalization;
using System.Security.Principal;
using System.Text;
using System.Text.RegularExpressions;
using engimatrix.Config;
using engimatrix.Connector;
using Engimatrix.ModelObjs;
using Engimatrix.Models;
using MailKit;
using MailKit.Net.Imap;
using MimeKit;

namespace engimatrix.Utils
{
    public static class EmailHelper
    {
        public static readonly HashSet<string> allowedExtensions = new HashSet<string>(StringComparer.OrdinalIgnoreCase)
            {
                ".bmp", ".gif", ".jpg", ".jpeg", ".pdf", ".doc", ".docx",
                ".xls", ".xlsx", ".csv", ".zip"
            };

        public static BodyBuilder CheckAndAddAttachmentsToEmail(List<IFormFile>? attachments, BodyBuilder bodyBuilder)
        {
            // Attach files
            if (attachments == null || attachments.Count == 0)
            {
                return bodyBuilder;
            }

            // For instance, the Gmail attachment size limit is 25 MB, while the Outlook attachment size limit is 20 MB.
            const long maxMessageSize = 20 * 1024 * 1024; // 20MB maximum message size
            long totalSize = 0;

            foreach (IFormFile attachment in attachments)
            {
                // Validate file extension
                string extension = Path.GetExtension(attachment.FileName);
                if (!allowedExtensions.Contains(extension))
                {
                    throw new InvalidOperationException($"The file type {extension} is not allowed.");
                }

                // Verify total file size
                totalSize += attachment.Length;
                if (totalSize > maxMessageSize)
                {
                    throw new InvalidOperationException("The total size of the email exceeds the allowed limit.");
                }

                // Copy the file content into a byte array
                byte[] fileBytes;
                using (MemoryStream memoryStream = new MemoryStream())
                {
                    attachment.CopyTo(memoryStream);
                    fileBytes = memoryStream.ToArray();  // Store the file content in a byte array
                }

                // Append attachment to the message
                MimePart mimePart = new MimePart(attachment.ContentType)
                {
                    Content = new MimeContent(new MemoryStream(fileBytes), ContentEncoding.Default),  // Use the byte array in a new MemoryStream
                    ContentDisposition = new ContentDisposition(ContentDisposition.Attachment),
                    ContentTransferEncoding = ContentEncoding.Base64,
                    FileName = attachment.FileName
                };

                bodyBuilder.Attachments.Add(mimePart);
            }

            return bodyBuilder;
        }

        public static string ExtractEmailBodyFromMessage(MimeMessage message)
        {
            string emailBody = message.HtmlBody;

            if (string.IsNullOrEmpty(emailBody))
            {
                // No HTML Body found
                emailBody = message.TextBody;

                if (string.IsNullOrEmpty(emailBody))
                {
                    // No text body found as well. Return error
                    return string.Empty;
                }

                return emailBody;
            }

            emailBody = EmailHelper.getHtmlBodyWithImages(message);
            emailBody = EmailHelper.RemoveUnwantedTags(emailBody);

            return emailBody;
        }

        public static BodyBuilder AddAttachmentsToBodyBuilder(IEnumerable<MimeEntity> attachments, BodyBuilder bodyBuilder)
        {
            if (attachments == null)
            {
                return bodyBuilder;
            }

            foreach (MimeEntity mimeAttachment in attachments)
            {
                bodyBuilder.Attachments.Add(mimeAttachment);
            }

            return bodyBuilder;
        }

        public static string RemoveAllHtmlTags(string htmlBody)
        {
            htmlBody = Regex.Replace(htmlBody, @"<[^>]*>", Environment.NewLine);
            htmlBody = Regex.Replace(htmlBody, Environment.NewLine + Environment.NewLine, Environment.NewLine);
            htmlBody = Regex.Replace(htmlBody, @"[\r\n]+", Environment.NewLine);
            return htmlBody;
        }

        public static string GetEmailBetweenTriangleBrackets(string email)
        {
            // Regular expression to extract the email from "Name" <email@example.com>
            string emailPattern = "<([^>]+)>";

            Match match = Regex.Match(email, emailPattern, RegexOptions.None, TimeSpan.FromSeconds(1));
            if (match.Success)
            {
                email = match.Groups[1].Value; // Extracted email
            }

            return email;
        }

        public static string GetUserNameBetweenDoubleQuote(string email)
        {
            // Regular expression to extract the email from "Name" <email@example.com>
            string emailPattern = "\"([^\"]+)\"";

            Match match = Regex.Match(email, emailPattern, RegexOptions.None, TimeSpan.FromSeconds(1));
            if (match.Success)
            {
                email = match.Groups[1].Value; // Extracted email
            }

            return email;
        }

        /*
        *   This function retrieves the base64 of the images (found on the BodyParts part of the MimeMessage). The original content of the message
        *   on the HtmlBody only contains cid(id) of the image, which is a reference pointing to the BodyParts, where we can find the same cid
        *   associated with the true base64 of the image.
        */

        public static string getHtmlBodyWithImages(MimeMessage message)
        {
            string htmlBody = message.HtmlBody;

            foreach (MimeEntity bodyPart in message.BodyParts)
            {
                // Check if bodyPart is a base64 that we want
                if (bodyPart is not MimePart mimePart || mimePart.ContentTransferEncoding != MimeKit.ContentEncoding.Base64)
                {
                    continue;
                }

                string mediaType = mimePart.ContentType.MediaType;
                string mediaSubtype = mimePart.ContentType.MediaSubtype;

                // Only handle image types (e.g., png, jpeg, etc.) or application (that can also be an image)
                if (!mediaType.Equals("image", StringComparison.OrdinalIgnoreCase) && !mediaType.Equals("application", StringComparison.OrdinalIgnoreCase))
                {
                    continue;
                }

                // Extract the content ID (CID) and remove <> characters
                string contentId = mimePart.ContentId?.Trim('<', '>');

                if (string.IsNullOrEmpty(contentId))
                {
                    continue;
                }

                // Read the content of the body part into a byte array
                using (MemoryStream memoryStream = new MemoryStream())
                {
                    // Extract the base64 found on the content of BodyParts
                    mimePart.Content.DecodeTo(memoryStream);
                    byte[] imageBytes = memoryStream.ToArray();
                    string base64ImageData = Convert.ToBase64String(imageBytes);

                    // Construct the data URL with the base64-encoded image
                    string dataUrl = $"data:{mediaType}/{mediaSubtype};base64,{base64ImageData}";

                    // Replace the CID reference in the HTML body
                    string cidReference = $"cid:{contentId}";
                    htmlBody = htmlBody.Replace(cidReference, dataUrl);
                }
            }

            return htmlBody;
        }

        // Adjust the patterns as needed. The more we clean, the less goes to the database.
        public static string RemoveUnwantedTags(string html)
        {
            // Remove script, style, meta, link, and other unwanted tags, matching opening and closing unwanted tags
            string pattern = @"<\s*(script|style|meta|link|head|title)[^>]*>.*?<\s*/\s*\1\s*>";
            html = Regex.Replace(html, pattern, "", RegexOptions.IgnoreCase | RegexOptions.Singleline);

            // Remove other self-closing tags like meta, link
            pattern = @"<\s*(meta|link)[^>]*>";
            html = Regex.Replace(html, pattern, "", RegexOptions.IgnoreCase);

            return html;
        }

        // If isTokenOriginal is true, it searches for the original token that created the thread
        // Else, if false, it goes to check for the direct reply token
        // The direct reply token could be useful if, in the future, we want to make threads inside threads
        public static string CheckIfEmailIsAReply(MimeMessage message, bool isOriginalToken)
        {
            // Step 1: Check the References header for the token
            string token = LookForTokenInReferences(message, isOriginalToken);
            if (!string.IsNullOrEmpty(token))
            {
                return token;
            }

            // Step 2: If not found in References, check the headers
            token = LookForTokenInHeaders(message, isOriginalToken);
            if (!string.IsNullOrEmpty(token))
            {
                return token;
            }

            // Step 3: If not found in headers, check the HTML body
            token = LookForTokenInHTMLBody(message, isOriginalToken);
            if (!string.IsNullOrEmpty(token))
            {
                return token;
            }

            // If the token was not found in any location, return an empty string
            return string.Empty;
        }

        public static string LookForTokenInReferences(MimeMessage message, bool isOriginalToken)
        {
            // Check the References header for the token
            string tokenPrefix = isOriginalToken ? "original-token-" : "reply-token-";

            foreach (string reference in message.References)
            {
                if (!reference.Contains(tokenPrefix))
                {
                    continue;
                }

                // Extract the token from the reference
                int tokenStart = reference.IndexOf(tokenPrefix, StringComparison.InvariantCulture) + tokenPrefix.Length;
                int tokenEnd = reference.IndexOf('@', tokenStart);
                if (tokenEnd == -1)
                {
                    continue;
                }

                string extractedToken = reference.Substring(tokenStart, tokenEnd - tokenStart);
                return extractedToken;
            }

            return string.Empty;
        }

        public static string LookForTokenInHeaders(MimeMessage message, bool isOriginalToken)
        {
            string headerName = isOriginalToken ? "X-Original-Token" : "X-Reply-Token";
            string token = message.Headers[headerName];
            return string.IsNullOrEmpty(token) ? string.Empty : token;
        }

        public static (int startIndex, int endIndex) FindTokenIndices(string htmlBody, bool isOriginalToken)
        {
            string tokenStartTag = isOriginalToken ? "START_ORIG_MSG_ID:" : "START_REPLY_MSG_ID:";
            string tokenEndTag = isOriginalToken ? "END_ORIG_MSG_ID" : "END_REPLY_MSG_ID";

            // Find the start tag
            int startIndex = htmlBody.IndexOf(tokenStartTag, StringComparison.Ordinal);
            if (startIndex == -1)
            {
                return (-1, -1); // Token start tag not found
            }

            // Move the startIndex to the position after the start tag
            startIndex += tokenStartTag.Length;

            // Find the end tag after the start tag
            int endIndex = htmlBody.IndexOf(tokenEndTag, startIndex, StringComparison.Ordinal);
            if (endIndex == -1)
            {
                return (-1, -1); // Token end tag not found
            }

            return (startIndex, endIndex);
        }

        public static string LookForTokenInHTMLBody(MimeMessage message, bool isOriginalToken)
        {
            string htmlBody = message.HtmlBody;
            if (string.IsNullOrEmpty(htmlBody))
            {
                return string.Empty;
            }

            var (startIndex, endIndex) = FindTokenIndices(htmlBody, isOriginalToken);
            if (startIndex == -1 || endIndex == -1)
            {
                return string.Empty; // Token not found
            }

            // Extract the token from the body
            string token = htmlBody.Substring(startIndex, endIndex - startIndex).Trim();
            return token;
        }

        public static string RemoveTokenFromHTMLBody(string htmlBody, bool isOriginalToken)
        {
            if (string.IsNullOrEmpty(htmlBody))
            {
                return htmlBody;
            }

            var (startIndex, endIndex) = FindTokenIndices(htmlBody, isOriginalToken);
            if (startIndex == -1 || endIndex == -1)
            {
                return htmlBody; // Token not found
            }

            // Determine token start and end tags
            string tokenStartTag = isOriginalToken ? "START_ORIG_MSG_ID:" : "START_REPLY_MSG_ID:";
            string tokenEndTag = isOriginalToken ? "END_ORIG_MSG_ID" : "END_REPLY_MSG_ID";

            // Calculate the starting point for removal (including the start tag)
            int removalStartIndex = startIndex - tokenStartTag.Length;

            // Calculate the length of the token section to remove
            int removalLength = endIndex - removalStartIndex + tokenEndTag.Length;

            // Remove the token section from the HTML body
            return htmlBody.Remove(removalStartIndex, removalLength);
        }

        public static async Task DownloadEmailAndAttachmentsAsync(UniqueId emailId, string emailFolder, string downloadPath, string account)
        {
            using ImapClient client = await EmailServiceMailkit.GetAutenticatedImapClientAsync(account);

            try
            {
                // Access the desired folder
                IMailFolder folder = client.GetFolder(emailFolder);
                await folder.OpenAsync(FolderAccess.ReadOnly);

                MimeMessage message = await folder.GetMessageAsync(emailId);

                // Save the email content as .eml
                string emailFilePath = Path.Combine(downloadPath, $"{emailId}.eml");

                if (!Directory.Exists(downloadPath))
                {
                    Directory.CreateDirectory(downloadPath);
                }

                // Extract important details
                string from = message.From.ToString();
                string to = string.Join(", ", message.To.Select(x => x.ToString()));
                string subject = message.Subject;
                string date = message.Date.ToString();
                string bodyText = message.TextBody ?? message.HtmlBody ?? "No body content available";

                // Save the important details to a file
                using (StreamWriter writer = new StreamWriter(emailFilePath))
                {
                    await writer.WriteLineAsync($"From: {from}");
                    await writer.WriteLineAsync($"To: {to}");
                    await writer.WriteLineAsync($"Subject: {subject}");
                    await writer.WriteLineAsync($"Date: {date}");
                    await writer.WriteLineAsync("Body:");
                    await writer.WriteLineAsync(bodyText);
                }

                Log.Debug($"Email saved to {emailFilePath}");

                // Download email attachments
                IEnumerable<MimeEntity> attachments = message.Attachments;
                if (!attachments.Any())
                {
                    return;
                }

                string attachmentsFolder = Path.Combine(downloadPath, emailId.ToString());
                Directory.CreateDirectory(attachmentsFolder);

                foreach (MimeEntity attachment in attachments)
                {
                    if (attachment is MimePart mimePart)
                    {
                        string fileName = mimePart.FileName;

                        using (FileStream stream = File.Create(Path.Combine(attachmentsFolder, fileName)))
                        {
                            await mimePart.Content.DecodeToAsync(stream);
                        }

                        Log.Debug($"Attachment {fileName} saved to {attachmentsFolder}");
                    }
                }
            }
            catch (Exception ex)
            {
                Log.Debug(ex.ToString());
            }
            finally
            {
                await client.DisconnectAsync(true);
            }
        }

        public static void PopulateDatabaseWithRandomMimeMessages(int messageCount)
        {
            var mimeMessages = GenerateRandomMimeMessages(messageCount);

            foreach (var message in mimeMessages)
            {
                string htmlBody = EmailHelper.RemoveUnwantedTags(message.HtmlBody);

                string categoryToSave = CategoryConstants.CategoryCode.SPAM.ToString();
                string status = StatusConstants.StatusCode.AGUARDA_VALIDACAO.ToString();

                // Generate random token and confianca (confidence)
                string token = Guid.NewGuid().ToString();
                string confianca = new Random().Next(1, 100).ToString();

                string cc = message.Cc.ToString();
                string bcc = message.Bcc.ToString();

                // Create and save the email and filtered email records
                EmailItem email = new EmailItem("", message.From.ToString(), "randomAccount", cc, bcc, message.Subject, htmlBody, DateTime.Now);
                FilteredEmail filteredEmail = new FilteredEmail(email, categoryToSave, status, DateTime.Now, token, confianca, "0");

                // Save the email in the DB
                filteredEmail = FilteringModel.SaveFiltered(filteredEmail);

                // Save attachments, if any
                AttachmentModel.SaveEmailAttachments(message, filteredEmail.email.id);
            }
        }

        public static List<MimeMessage> GenerateRandomMimeMessages(int count)
        {
            var mimeMessages = new List<MimeMessage>();

            for (int i = 0; i < count; i++)
            {
                var message = new MimeMessage();

                // Set random email addresses
                message.From.Add(new MailboxAddress(GetRandomText(5), GetRandomEmail()));
                message.To.Add(new MailboxAddress(GetRandomText(5), GetRandomEmail()));
                message.Cc.Add(new MailboxAddress(GetRandomText(5), GetRandomEmail()));
                message.Bcc.Add(new MailboxAddress(GetRandomText(5), GetRandomEmail()));

                // Set subject and body
                message.Subject = $"Test Email {i + 1} - {GetRandomText(15)}";
                message.Body = new TextPart("html")
                {
                    Text = $"<html><body><p>{GetRandomText(50)}</p></body></html>"
                };

                // Set date
                message.Date = GetRandomDate();

                mimeMessages.Add(message);
            }

            return mimeMessages;
        }

        private static string GetRandomEmail()
        {
            var domains = new[] { "example.com", "test.com", "sample.org" };
            return $"user{new Random().Next(10000)}@{domains[new Random().Next(domains.Length)]}";
        }

        private static string GetRandomText(int length = 10)
        {
            const string chars = "ABCDEFGHIJKLMNOPQRSTUVWXYZabcdefghijklmnopqrstuvwxyz0123456789";
            return new string(Enumerable.Repeat(chars, length).Select(s => s[new Random().Next(s.Length)]).ToArray());
        }

        private static DateTime GetRandomDate()
        {
            return DateTime.Now.AddDays(-new Random().Next(0, 365));
        }

        public static string ExtractDomainFromEmail(string email)
        {
            // Extract the domain from the email address
            string domain = email.Split('@')[1];

            // Remove the last character until it is not a special character
            domain = RemoveLastSpecialCharactersFromString(domain);

            return domain;
        }

        public static string RemoveLastSpecialCharactersFromString(string input)
        {
            // Remove the last character until it is not a special character
            while (input.Length > 0 && (input[^1] == '>' || input[^1] == ' ' || input[^1] == '"'))
            {
                input = input[..^1];
            }

            return input.Trim();
        }
    }
}
