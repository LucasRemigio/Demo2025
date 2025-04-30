// // Copyright (c) 2024 Engibots. All rights reserved.

using Engimatrix.ModelObjs;
using engimatrix.Models;
using engimatrix.ModelObjs;
using engimatrix.Config;
using engimatrix.Utils;
using MimeKit;
using Engimatrix.Utils;

namespace Engimatrix.Models
{
    public class ReplyAttachmentModel
    {
        public static List<ReplyAttachmentItem> getAttachments(string execute_user, string replyToken)
        {
            //Returns attachment

            Dictionary<string, string> dic = new Dictionary<string, string>();
            dic.Add("reply_token", replyToken);

            List<Dictionary<string, string>> attachmentRec = SqlExecuter.ExecFunction("SELECT * FROM reply_attachment WHERE reply_token = @reply_token", dic, execute_user, false, "GetReplyEmailAttachments").out_data;
            List<ReplyAttachmentItem> replyAttachmentItems = new List<ReplyAttachmentItem>();
            foreach (var item in attachmentRec)
            {
                if (ConfigManager.isProduction)
                {
                    item["2"] = Cryptography.Decrypt(item["2"], item["1"]);
                    item["4"] = Cryptography.Decrypt(item["4"], item["1"]);
                }
                ReplyAttachmentItem attachment = new ReplyAttachmentItem(item["0"], item["1"], item["2"], item["3"], item["4"]);
                replyAttachmentItems.Add(attachment);
            }

            return replyAttachmentItems;
        }

        public static void saveAttachment(ReplyAttachmentItem attach, string execute_user)
        {
            Dictionary<string, string> dic = new Dictionary<string, string>();
            dic.Add("reply_token", attach.reply_token);
            if (ConfigManager.isProduction)
            {
                attach.name = Cryptography.Encrypt(attach.name, attach.reply_token);
                attach.file = Cryptography.Encrypt(attach.file, attach.reply_token);
            }
            dic.Add("name", attach.name);
            dic.Add("size", attach.size);
            dic.Add("file", attach.file);
            SqlExecuter.ExecFunction("INSERT INTO reply_attachment (`reply_token`, `name`, `size`, `file`) VALUES (@reply_token, @name, @size, @file)", dic, execute_user, false, "Saving file");
        }

        public static void saveAttachment(ReplyAttachmentItem attach)
        {
            saveAttachment(attach, "");
        }

        public static void SaveEmailAttachments(MimeMessage message, string replyToken)
        {
            Log.Debug($"Reply has {message.Attachments.Count()} attachments");

            // Many times, when the attachment exists in the attachments part of the message, it will also exist on
            // the body parts. To avoid repetition, we create a hash set to store the names of the files that have
            // already been saved.
            HashSet<string> savedFiles = [];

            if (!message.Attachments.Any())
            {
                Log.Debug($"Reply No attachments");
            }

            // Look for atachments on the atachments part
            foreach (MimeEntity attachment in message.Attachments)
            {
                string fileName = attachment.ContentDisposition?.FileName ?? attachment.ContentType.Name;
                Log.Debug($"Reply Attachment: Rendering {fileName} with extension {Path.GetExtension(fileName).ToLower()}");

                if (string.IsNullOrEmpty(fileName) || !Path.GetExtension(fileName).ToLower().Equals(".pdf"))
                {
                    Log.Debug("Reply Attachment is not a pdf.");
                    continue;
                }

                // Check if the file has already been saved
                if (savedFiles.Contains(fileName))
                {
                    Log.Debug($"Reply Skipping duplicate PDF: {fileName}");
                    continue;
                }

                Log.Debug("Reply Attachment: Is a pdf. Saving...");

                string b64content = attachment.ConvertToBase64String();
                string filesize = b64content.Length.ToString();

                Log.Debug($"Reply Attachment: Convertion to base64 succesfull - " +
                    $"filesize: {filesize} / b64content: {b64content[..20]}...");

                ReplyAttachmentItem item = new("", replyToken, fileName, filesize, b64content);
                ReplyAttachmentModel.saveAttachment(item);

                savedFiles.Add(fileName);
                Log.Debug($"Reply Attachment {fileName} saved.");
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
                if (!mimePart.ContentType.MediaType.Equals("application", StringComparison.OrdinalIgnoreCase) &&
                    !mimePart.ContentType.MediaSubtype.Equals("pdf", StringComparison.OrdinalIgnoreCase))
                {
                    Log.Debug("Reply No PDFs found in the current MIME part.");
                    continue;
                }

                string fileName = mimePart.ContentDisposition?.FileName ?? mimePart.ContentType.Name;
                Log.Debug($"Reply Found PDF: {fileName}");

                // Skip if the file name is null or does not have a ".pdf" extension
                if (string.IsNullOrEmpty(fileName) || !Path.GetExtension(fileName).ToLower().Equals(".pdf"))
                {
                    Log.Debug("Reply PDF file name is invalid or not a PDF.");
                    continue;
                }

                // Check if the file has already been saved
                if (savedFiles.Contains(fileName))
                {
                    Log.Debug($"Reply Skipping duplicate PDF: {fileName}");
                    continue;
                }

                Log.Debug("Reply PDF found in the body parts. Saving...");

                // Convert to base64 and save
                string b64content;
                using (var memoryStream = new MemoryStream())
                {
                    mimePart.Content.DecodeTo(memoryStream);
                    b64content = Convert.ToBase64String(memoryStream.ToArray());
                }
                string filesize = b64content.Length.ToString();
                Log.Debug($"Reply PDF conversion to base64 successful - filesize: {filesize} - content: {b64content[..20]}...");

                ReplyAttachmentItem item = new("", replyToken, fileName, filesize, b64content);
                ReplyAttachmentModel.saveAttachment(item);

                savedFiles.Add(fileName);
                Log.Debug($"Reply PDF {fileName} saved.");
            }
        }
    }
}
