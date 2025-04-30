// // Copyright (c) 2024 Engibots. All rights reserved.

using engimatrix.ModelObjs;
using Engimatrix.ModelObjs;
using engimatrix.Models;
using Microsoft.Graph.Models;
using engimatrix.Utils;
using engimatrix.Config;
using MimeKit;
using Engimatrix.Utils;
using System.Security.Cryptography;
using NLog.Targets;

namespace Engimatrix.Models
{
    public class AttachmentModel
    {
        public static List<EmailAttachmentItem> getAttachments(string execute_user, string id)
        {
            //Returns attachment
            Dictionary<string, string> dic = new Dictionary<string, string>();
            dic.Add("id", id);

            string query = "SELECT id, email, name, size, file FROM attachment WHERE email = @id";

            SqlExecuterItem response = SqlExecuter.ExecFunction(query, dic, execute_user, false, "GetEmailAttachments");

            List<EmailAttachmentItem> emailAttachmentItems = new List<EmailAttachmentItem>();
            if (!response.operationResult)
            {
                // Error finding attachments for id
                Log.Debug("Error retrieving attachments for id " + id);
                return emailAttachmentItems; // Empty list of attachments
            }

            if (response.out_data.Count == 0)
            {
                // No attachment found on given id
                return emailAttachmentItems; // Empty list of attachments
            }

            foreach (Dictionary<string, string> item in response.out_data)
            {
                EmailAttachmentItem attachment = new EmailAttachmentItem(item["0"], item["1"], item["2"], item["3"], item["4"]);
                if (ConfigManager.isProduction)
                {
                    attachment.name = Cryptography.Decrypt(attachment.name, attachment.email);
                    attachment.file = Cryptography.Decrypt(attachment.file, attachment.email);
                }

                emailAttachmentItems.Add(attachment);
            }

            return emailAttachmentItems;
        }

        public static List<EmailAttachmentItem> GetAllAttachments(string execute_user)
        {
            //Returns attachment
            Dictionary<string, string> dic = new Dictionary<string, string>();

            string query = "SELECT a.id, a.email, a.name, a.size, e.subject " +
                "FROM attachment a " +
                "JOIN email e ON e.id = a.email";

            SqlExecuterItem response = SqlExecuter.ExecFunction(query, dic, execute_user, false, "GetEmailAttachments");

            List<EmailAttachmentItem> emailAttachmentItems = [];
            if (!response.operationResult)
            {
                // Error finding attachments for id
                Log.Debug("Error retrieving attachments for id ");
                return emailAttachmentItems; // Empty list of attachments
            }

            if (response.out_data.Count == 0)
            {
                // No attachment found on given id
                return emailAttachmentItems; // Empty list of attachments
            }

            foreach (Dictionary<string, string> item in response.out_data)
            {
                EmailAttachmentItem attachment = new(item["0"], item["1"], item["2"], item["3"], item["4"]);
                if (ConfigManager.isProduction)
                {
                    try
                    {
                        Path.GetExtension(attachment.name);
                    }
                    catch (ArgumentException ex)
                    {
                        attachment.name = Cryptography.Decrypt(attachment.name, attachment.email);
                        attachment.file = Cryptography.Decrypt(attachment.file, attachment.email);
                    }
                }

                emailAttachmentItems.Add(attachment);
            }

            return emailAttachmentItems;
        }

        public static void saveAttachment(EmailAttachmentItem attach, string execute_user)
        {
            Dictionary<string, string> dic = new Dictionary<string, string>();
            if (ConfigManager.isProduction)
            {
                attach.name = Cryptography.Encrypt(attach.name, attach.email);
                attach.file = Cryptography.Encrypt(attach.file, attach.email);
            }
            dic.Add("email", attach.email);
            dic.Add("name", attach.name);
            dic.Add("size", attach.size);
            dic.Add("file", attach.file);
            SqlExecuter.ExecFunction("INSERT INTO attachment (`email`, `name`, `size`, `file`) VALUES (@email, @name, @size, @file)", dic, execute_user, false, "Saving file");
        }

        public static void saveAttachment(EmailAttachmentItem attach)
        {
            saveAttachment(attach, "");
        }

        public static void SaveEmailAttachments(MimeMessage message, string filteredEmailId)
        {
            Log.Debug($"Email has {message.Attachments.Count()} attachments");

            // Many times, when the attachment exists in the attachments part of the message, it will also exist on
            // the body parts. To avoid repetition, we create a hash set to store the names of the files that have
            // already been saved.
            HashSet<string> savedFiles = [];

            if (!message.Attachments.Any())
            {
                Log.Debug($"No attachments");
            }

            // Look for atachments on the atachments part
            foreach (MimeEntity attachment in message.Attachments)
            {
                string fileName = attachment.ContentDisposition?.FileName ?? attachment.ContentType.Name;
                Log.Debug($"Attachment: Rendering {fileName} with extension {Path.GetExtension(fileName).ToLower()}");

                // download all allowed extensions 
                if (string.IsNullOrEmpty(fileName))
                {
                    Log.Debug("Attachment is not a valid format.");
                    continue;
                }

                // Check if the file has already been saved
                if (savedFiles.Contains(fileName))
                {
                    Log.Debug($"Skipping duplicate PDF: {fileName}");
                    continue;
                }

                Log.Debug("Attachment: Is a valid extension. Saving...");

                string b64content = attachment.ConvertToBase64String();
                string filesize = b64content.Length.ToString();

                string previewContent = b64content.Length > 20 ? b64content[..20] : b64content;
                Log.Debug($"Attachment: Convertion to base64 succesfull - " +
                    $"filesize: {filesize} / b64content: {previewContent}...");

                EmailAttachmentItem item = new("", filteredEmailId, fileName, filesize, b64content);
                AttachmentModel.saveAttachment(item);

                savedFiles.Add(fileName);
                Log.Debug($"Attachment {fileName} saved.");
            }

            // Process body parts (to handle inline or unconventional attachments)
            foreach (MimeEntity part in message.BodyParts)
            {
                // if not mime part, skip this content
                if (part is not MimePart mimePart)
                {
                    continue;
                }

                // Skip if not a PDF
                // Here we do not allow all the extensions allowed because we do not want to save the signature images
                // or any added extras
                if (!mimePart.ContentType.MediaType.Equals("application", StringComparison.OrdinalIgnoreCase) &&
                    !mimePart.ContentType.MediaSubtype.Equals("pdf", StringComparison.OrdinalIgnoreCase))
                {
                    Log.Debug($"Current {mimePart.ContentType.MediaSubtype} is not PDF in the current MIME part.");
                    continue;
                }

                string fileName = mimePart.ContentDisposition?.FileName ?? mimePart.ContentType.Name;
                Log.Debug($"Found PDF: {fileName}");

                // Skip if the file name is null or does not have a ".pdf" extension
                if (string.IsNullOrEmpty(fileName) ||
                    !Path.GetExtension(fileName).ToLower().Equals(".pdf", StringComparison.OrdinalIgnoreCase))
                {
                    Log.Debug("PDF file name is invalid or not a PDF.");
                    continue;
                }

                // Check if the file has already been saved
                if (savedFiles.Contains(fileName))
                {
                    Log.Debug($"Skipping duplicate PDF: {fileName}");
                    continue;
                }

                Log.Debug("PDF found in the body parts. Saving...");

                // Convert to base64 and save
                string b64content;
                using (var memoryStream = new MemoryStream())
                {
                    mimePart.Content.DecodeTo(memoryStream);
                    b64content = Convert.ToBase64String(memoryStream.ToArray());
                }
                string filesize = b64content.Length.ToString();

                string previewContent = b64content.Length > 20 ? b64content[..20] : b64content;
                Log.Debug($"PDF conversion to base64 successful - filesize: {filesize} - content: {previewContent}...");

                EmailAttachmentItem item = new("", filteredEmailId, fileName, filesize, b64content);
                AttachmentModel.saveAttachment(item);

                savedFiles.Add(fileName);
                Log.Debug($"PDF {fileName} saved.");
            }
        }

        public static string GetFileContentHash(byte[] content)
        {
            return Convert.ToBase64String(SHA256.HashData(content));
        }
    }
}
