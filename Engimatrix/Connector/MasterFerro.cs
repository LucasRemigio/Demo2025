// // Copyright (c) 2024 Engibots. All rights reserved.

using MailKit.Net.Smtp;
using MailKit.Net.Imap;
using MailKit.Search;
using MimeKit;
using MailKit;
using Engimatrix.Models;
using Engimatrix.ModelObjs;
using Microsoft.IdentityModel.Tokens;
using engimatrix.Config;
using engimatrix.Utils;
using engimatrix.ModelObjs;
using engimatrix.Models;
using System.Text.RegularExpressions;
using engimatrix.Views;
using engimatrix.PricingAlgorithm;
using Microsoft.AspNetCore.Diagnostics;
using engimatrix.Exceptions;

namespace engimatrix.Connector;

public static class MasterFerro
{
    public static async Task CategorizeFolderAsync(string account, string folderName)
    {
        using ImapClient client = await EmailServiceMailkit.GetAutenticatedImapClientAsync(account);
        IMailFolder folder = client.GetFolder(folderName);

        try
        {
            folder.Open(FolderAccess.ReadOnly); // Open folder for reading

            int timeFrame = ConfigManager.timespanForEmailRetrieval; // 20 minutes
            IList<MailKit.UniqueId> uids = await GetRecentEmailsAsync(folder, timeFrame);

            if (uids.Count <= 0)
            {
                return; // No message to fetch
            }

            Log.Debug($"{account} - folder has {uids.Count} message(s) in the last {timeFrame / 60} minutes");

            foreach (MailKit.UniqueId uid in uids)
            {
                await ProcessEmailAsync(folder, uid, account, folderName);
            }
        }
        catch (Exception ex)
        {
            Log.Error($"{account} - An error occurred during folder processing: {ex}");
        }
        finally
        {
            await folder.CloseAsync();
            await client.DisconnectAsync(true);
        }
    }

    // Helper methods for better modularity

    public static async Task<IList<MailKit.UniqueId>> GetRecentEmailsAsync(IMailFolder folder, int timeFrame)
    {
        SearchQuery searchQuery = SearchQuery.YoungerThan(timeFrame);
        IList<MailKit.UniqueId> uids = await folder.SearchAsync(searchQuery);
        return uids;
    }

    private static async Task ProcessEmailAsync(IMailFolder folder, MailKit.UniqueId uid, string account, string folderName)
    {
        MimeMessage message = folder.GetMessage(uid);
        string token = Guid.NewGuid().ToString();

        Log.Debug("---------------------------------");
        Log.Debug($"Filtering email with subject {message.Subject} from {message.From}");

        try
        {
            if (EmailModel.IsAnyEmailMessageFromBlacklisted(message.From, account))
            {
                Log.Debug($"Email from {message.From} is blacklisted. Ignoring email.");
                await HandleSpamEmailAsync(message, folderName, uid, account, token);
                return;
            }

            // Check for email coming from another employee
            if (IsEmailFromConfiguredAddress(message.From))
            {
                Log.Debug($"Address the email originated from {message.From} with subject {message.Subject} is in ConfigManager");
                await HandleDuplicateEmailAsync(message, folderName, uid, account, token);
                return;
            }

            // check if email is a reply. If it has a traceable token, that it must be a reply
            string emailToken = EmailHelper.CheckIfEmailIsAReply(message, true);
            if (!string.IsNullOrEmpty(emailToken))
            {
                Log.Debug($"Email from {message.From} with subject {message.Subject} is a reply");
                await HandleReplyEmailAsync(message, emailToken, folderName, uid, account, token);
                return;
            }

            // Check for duplicate email from the same client
            EmailItem? emailRecentListFromSamePerson = EmailModel.CheckIfEmailAlreadyExists(message, account, ConfigManager.delayMinutesBetweenRepeatedEmailChecks);
            if (emailRecentListFromSamePerson is not null)
            {
                Log.Debug($"Found a repeated email from {emailRecentListFromSamePerson.from} with subject {emailRecentListFromSamePerson.subject}");
                await HandleDuplicateEmailAsync(message, folderName, uid, account, token);
                return;
            }

            await CategorizeAndProcessEmailAsync(message, folderName, uid, account, token);
        }
        catch (Exception ex)
        {
            Log.Debug("An error ocurred processing or saving filtered email " + ex);
            await HandleEmailProcessingErrorAsync(message, uid, account, token);
        }
    }

    private static bool IsEmailFromConfiguredAddress(InternetAddressList from)
    {
        string? fromAddress = from.Mailboxes.FirstOrDefault()?.Address;

        if (fromAddress == null)
        {
            return false;
        }

        if (ConfigManager.MailboxAccount.Contains(fromAddress))
        {
            return true;
        }

        return false;
    }

    private static async Task HandleDuplicateEmailAsync(MimeMessage message, string folderName, MailKit.UniqueId uid, string account, string token)
    {
        await HandleCategorizedEmailAsync(message, folderName, uid, account, token,
            CategoryConstants.CategoryCode.DUPLICADOS);
    }

    private static async Task HandleSpamEmailAsync(MimeMessage message, string folderName, MailKit.UniqueId uid, string account, string token)
    {
        await HandleCategorizedEmailAsync(message, folderName, uid, account, token,
            CategoryConstants.CategoryCode.SPAM);
    }

    private static async Task HandleCategorizedEmailAsync(MimeMessage message, string folderName, MailKit.UniqueId uid,
        string account, string token, int categoryId)
    {
        await UpdateEmailAsync(token, message, folderName, uid, account);
        await MoveEmailByTokenAsync(folderName, ConfigManager.SpamFolder, token, account);

        string status = StatusConstants.StatusCode.TRIAGEM_REALIZADA.ToString();

        FilteredEmail filteredEmail = CreateFilteredEmail(message, account, categoryId.ToString(), status, token, "100", "0");
        SaveFilteredEmailAndAttachments(filteredEmail, message);

        Log.Debug($"Email {uid} processed as {categoryId}.");
    }

    private static async Task HandleReplyEmailAsync(MimeMessage message, string emailToken, string folderName, MailKit.UniqueId uid, string account, string token)
    {
        string emailContent = EmailHelper.ExtractEmailBodyFromMessage(message);

        if (string.IsNullOrEmpty(emailContent))
        {
            Log.Debug($"Reply with id {uid} has no body");
            throw new ResourceEmptyException($"Reply with id {uid} has no body");
        }

        string filteredCategory = FilteringModel.getFilteredCategoryFromEmailToken(emailToken);
        BodyBuilder bodyBuilder = new()
        {
            HtmlBody = emailContent,
            TextBody = message.TextBody,
        };

        // Save the attachments so the new bodybuilder gets them too
        bodyBuilder = EmailHelper.AddAttachmentsToBodyBuilder(message.Attachments, bodyBuilder);
        message.Body = bodyBuilder.ToMessageBody();

        // save the reply to the database
        ReplyModel.SaveReplyAndAttachmentsToDB(message, emailToken, token, false);
        Log.Debug($"Reply email {uid} saved successfully.");

        if (string.IsNullOrEmpty(filteredCategory))
        {
            Log.Debug($"Original token not found on reply - {emailToken}");
            // Look for the token and replace it with a new one
            emailContent = ReplaceCurrentToken(emailContent, token);
        }

        Log.Debug("Filtered category: " + filteredCategory);
        if (filteredCategory.Equals(CategoryConstants.CategoryCode.COTACOES_ORCAMENTOS.ToString(), StringComparison.OrdinalIgnoreCase))
        {
            Log.Debug("Checking if client adjudicated the quote");
            if (await CheckIfClientAdjudicatedQuotation(message, account, emailToken))
            {
                if (!FilteringModel.IsFilteredEmailAlreadyConfirmed(emailToken, account))
                {
                    await HandleClientAdjudicatedEmailAsync(message, account, folderName, uid, emailToken);
                    Log.Debug("The client adjudicated the quote, and the order was not confirmed before");
                    return;
                }

                Log.Debug("The client adjudicated, but the order was already confirmed before");
            }
        }

        string destinationFolder = EmailCategorizer.GetFolderByCategoryId(filteredCategory);
        await MoveEmailAsync(folderName, destinationFolder, uid, account);

        // Mark the original email as to be resolved again
        FilteringModel.ChangeEmailStatus(emailToken, StatusConstants.StatusCode.AGUARDA_VALIDACAO.ToString(), "system");
    }

    // this function returns a bool of true if the client adjudicated the quote
    private static async Task<bool> CheckIfClientAdjudicatedQuotation(MimeMessage message, string account, string filteredToken)
    {
        FilteredEmail filteredEmail = FilteringModel.getFilteredEmail(account, filteredToken, true);
        if (filteredEmail.IsEmpty())
        {
            Log.Debug("No filtered email found for token " + filteredToken);
            return false;
        }

        // we need to verify if the client (or the operator) had already confirmed the order

        string emailContent = EmailHelper.ExtractEmailBodyFromMessage(message);

        CheckClientAdjudicated? clientAdjudicated = await OpenAI.CheckIfClientAdjudicated(emailContent);

        if (clientAdjudicated is null)
        {
            Log.Debug("No client adjudicated found in the email");
            return false;
        }

        // even if the client adjudicated, the act of asking for a retification must be sent back to the operator
        if (!string.IsNullOrEmpty(clientAdjudicated.pediu_retificacao) && clientAdjudicated.pediu_retificacao.Equals("sim", StringComparison.OrdinalIgnoreCase))
        {
            Log.Debug("Client requested a retification");
            return false;
        }

        if (!string.IsNullOrEmpty(clientAdjudicated.cliente_adjudicou) && clientAdjudicated.cliente_adjudicou.Equals("sim", StringComparison.OrdinalIgnoreCase))
        {
            Log.Debug("Client adjudicated the quote");
            // in this case, we must follow a different flow.
            // The order must be set to confirmed by client and then we must create the order on the primavera
            // we must also send an email to the client with the order details
            return true;
        }

        Log.Debug("Client did not adjudicate the quote");
        return false;
    }

    private static async Task HandleClientAdjudicatedEmailAsync(MimeMessage message, string account, string folderName, MailKit.UniqueId uid, string filteredToken)
    {
        // we must set the filtered email state as confirmed by client
        // move the email to the orders folder
        string filteredCategory = CategoryConstants.CategoryCode.ENCOMENDAS.ToString();
        string destinationFolder = EmailCategorizer.GetFolderByCategoryId(filteredCategory);
        await MoveEmailAsync(folderName, destinationFolder, uid, account);

        // Mark the original email as to be resolved again
        string status = StatusConstants.StatusCode.CONFIRMADO_POR_CLIENTE.ToString();
        FilteringModel.ChangeEmailStatus(filteredToken, status, message.From.ToString());

        OrderItem? orderItem = OrderModel.GetOrderByEmailToken(filteredToken, account);

        if (orderItem is null)
        {
            Log.Debug("No order found for token " + filteredToken);
            throw new ResourceEmptyException("No order found for token " + filteredToken);
        }

        Log.Debug("Client adjudicated email processed successfully");
    }

    private static string ReplaceCurrentToken(string emailBody, string newToken)
    {
        (int startIdx, int endIdx) = EmailHelper.FindTokenIndices(emailBody, true);

        if (startIdx == -1 || endIdx == -1)
        {
            Log.Debug("Token not found in the email content.");
            return emailBody;
        }

        // Replace the token by reconstructing the email content
        emailBody = string.Concat(emailBody.AsSpan(0, startIdx + 1), newToken, emailBody.AsSpan(endIdx));
        return emailBody;
    }

    public static async Task CategorizeAndProcessEmailAsync(MimeMessage message, string folderName, MailKit.UniqueId uid, string account, string token)
    {
        try
        {
            OpenAiEmailCategorization? categorization = await OpenAI.ProcessMoveEmailCategoryOpenAI(message);
            if (categorization is null)
            {
                Log.Debug("Open AI response came null, creating default categorization...");
                categorization = new OpenAiEmailCategorizationBuilder().SetConfianca("0").SetCategoria("").Build();
            }
            Log.Debug("Categoria: " + categorization.categoria + " / Confiança: " + categorization.confianca);

            // In case the OpenAI gives an error, we need to send to the error folder. By giving the confidence of 0, the next function will get the Error folder
            if (string.IsNullOrEmpty(categorization.confianca)) { categorization.confianca = "0"; }

            // Check if the email is a forward and update the sender if necessary
            UpdateOriginalSenderIfForward(message, categorization.email_remetente, categorization.confianca_reencaminhamento);

            (string folderToSave, int categoryToSave) = EmailCategorizer.GetFolderNameAndCategoryConstantFromCategory(categorization.categoria, categorization.confianca);

            int status = GetStatusFromCategoryAndFolder(categoryToSave, folderToSave);

            await UpdateEmailAsync(token, message, folderName, uid, account);
            await MoveEmailByTokenAsync(folderName, folderToSave, token, account);

            FilteredEmail filteredEmail = CreateFilteredEmail(message, account, categoryToSave.ToString(), status.ToString(), token, categorization.confianca, "0");
            SaveFilteredEmailAndAttachments(filteredEmail, message);
        }
        catch (Exception ex)
        {
            throw new Exception($"Error categorizing email with uid {uid}, subject: {message.Subject}, from: {message.From} with message: {ex}");
        }
    }

    public static int GetStatusFromCategoryAndFolder(int category, string folderToSave)
    {
        int status = StatusConstants.StatusCode.TRIAGEM_REALIZADA;

        if (category == CategoryConstants.CategoryCode.COTACOES_ORCAMENTOS ||
            category == CategoryConstants.CategoryCode.ENCOMENDAS)
        {
            // status = StatusConstants.StatusCode.AGUARDA_VALIDACAO;

            // WARNING!: Status should be A_PROCESSAR !!ONLY!! if we want to extract products from the email
            status = StatusConstants.StatusCode.A_PROCESSAR;
            return status;
        }

        if (category == CategoryConstants.CategoryCode.OUTROS)
        {
            status = StatusConstants.StatusCode.AGUARDA_VALIDACAO;
            return status;
        }

        if (folderToSave == ConfigManager.ValidateFolder)
        {
            status = StatusConstants.StatusCode.ERRO;
            return status;
        }

        return status;
    }

    private static void UpdateOriginalSenderIfForward(MimeMessage message, string? originalSender, string? fwdConfianca)
    {
        if (string.IsNullOrEmpty(originalSender) || string.IsNullOrEmpty(fwdConfianca))
        {
            return; // No forward information available
        }

        if (int.TryParse(fwdConfianca, out int confidence) && confidence <= ConfigManager.theresholdConfiancaAI)
        {
            return; // Not enough confidence to update the sender
        }

        string parsedOriginalSender = EmailHelper.GetEmailBetweenTriangleBrackets(originalSender);

        if (string.IsNullOrEmpty(parsedOriginalSender) || !Util.IsValidInputEmail(parsedOriginalSender))
        {
            return; // Invalid email address
        }

        string messageFromEmail = EmailHelper.GetEmailBetweenTriangleBrackets(message.From.ToString());

        if (messageFromEmail != parsedOriginalSender)
        {
            message.From.Clear();
            message.From.Add(MailboxAddress.Parse(parsedOriginalSender));
            Log.Debug($"Updated original sender to: {parsedOriginalSender}");
        }
    }

    public static FilteredEmail CreateFilteredEmail(MimeMessage message, string account, string category, string status, string token, string confidence, string isValidated)
    {
        string emailBody = EmailHelper.ExtractEmailBodyFromMessage(message);
        if (string.IsNullOrEmpty(emailBody))
        {
            Log.Debug($"Email from {message.From} with subject {message.Subject} has no body");
        }

        EmailItem emailItem = new(
            "",
            message.From.ToString(),
            account,
            message.Cc.ToString(),
            message.Bcc.ToString(),
            message.Subject,
            emailBody,
            DateTime.Parse(message.Date.ToString())
        );

        return new FilteredEmail(emailItem, category, status, DateTime.Now, token, confidence, isValidated);
    }

    private static async Task HandleEmailProcessingErrorAsync(MimeMessage message, MailKit.UniqueId uid, string account, string token)
    {
        Log.Error($"Handling email processing error for uid: {uid}, from: {message.From} with subject: {message.Subject}");

        (string folderToSave, int categoryToSave) = EmailCategorizer.GetFolderNameAndCategoryConstantFromCategory(string.Empty, "0");
        await UpdateEmailAsync(token, message, ConfigManager.InboxFolder, uid, account);
        await MoveEmailByTokenAsync(ConfigManager.InboxFolder, folderToSave, token, account);

        string status = StatusConstants.StatusCode.ERRO.ToString();
        FilteredEmail filteredEmail = CreateFilteredEmail(message, account, categoryToSave.ToString(), status, token, "0", "0");
        SaveFilteredEmailAndAttachments(filteredEmail, message);
    }

    public static void SaveFilteredEmailAndAttachments(FilteredEmail filteredEmail, MimeMessage message)
    {
        filteredEmail = FilteringModel.SaveFiltered(filteredEmail);
        try
        {
            AttachmentModel.SaveEmailAttachments(message, filteredEmail.email.id);
        }
        catch (Exception ex)
        {
            Log.Debug($"Error saving attachments for email {filteredEmail.email.id}: {ex}");
        }

        string categoryName = CategoryConstants.GetCategoryNameByCode(int.Parse(filteredEmail.category));
        string statusName = StatusConverter.Convert(int.Parse(filteredEmail.status));
        Log.Debug($"Email {filteredEmail.email.id} saved successfully to category {filteredEmail.category} - {categoryName} and status {filteredEmail.status} - {statusName}");
    }

    public static async Task UpdateEmailAsync(string uniqueId, MimeMessage email, string folderName, MailKit.UniqueId emailIdToUpdate, string account)
    {
        string uniqueIdHTML = "<p class=\"TEXT ONLY\" style=\"width:0; overflow:hidden;float:left; display:none\">START_ORIG_MSG_ID:" + uniqueId + " END_ORIG_MSG_ID </p>";

        using ImapClient client = await EmailServiceMailkit.GetAutenticatedImapClientAsync(account);
        try
        {
            IMailFolder folder = client.GetFolder(folderName);
            await folder.OpenAsync(FolderAccess.ReadWrite);

            MimeMessage existingMessage = await folder.GetMessageAsync(emailIdToUpdate);

            TextPart? htmlPart = existingMessage.BodyParts.OfType<TextPart>().FirstOrDefault(part => part.IsHtml);
            if (htmlPart != null)
            {
                string updatedHtml = htmlPart.Text + uniqueIdHTML;
                htmlPart.Text = updatedHtml;
            }
            else if (existingMessage.TextBody != null)
            {
                string updatedHtml = "<p>" + existingMessage.TextBody + "</p>" + uniqueIdHTML;
                TextPart newHtmlPart = new TextPart("html") { Text = updatedHtml };
                existingMessage.Body = new Multipart("alternative")
                {
                    new TextPart("plain") { Text = existingMessage.TextBody },
                    newHtmlPart
                };
            }
            else
            {
                // If no HTML or Text part exists, create a new HTML part with the unique ID HTML
                TextPart newHtmlPart = new TextPart("html") { Text = uniqueIdHTML };
                existingMessage.Body = newHtmlPart;
            }

            await folder.ReplaceAsync(emailIdToUpdate, existingMessage);

            await folder.CloseAsync();
        }
        catch (Exception ex)
        {
            Log.Error($"Error updating email: {ex.Message}");
        }
        finally
        {
            await client.DisconnectAsync(true);
        }
    }

    public static async Task FwdEmail(string emailTokenOrId, List<string> emailToList, string? message, string executeUser)
    {
        EmailItem email;
        // Id
        if (emailTokenOrId.Length < 10)
        {
            email = EmailModel.getEmail(emailTokenOrId, executeUser);
        }
        // Token
        else
        {
            email = FilteringModel.getFilteredEmail(executeUser, emailTokenOrId, true).email;
        }

        if (email.IsEmpty())
        {
            Log.Debug($"FwdEmail: no email found with token/id {emailTokenOrId}");
            return;
        }

        // construct a new message
        MimeMessage mimeMessage = new MimeMessage();
        mimeMessage.From.Add(MailboxAddress.Parse(ConfigManager.SystemEmail));
        mimeMessage.ReplyTo.Add(MailboxAddress.Parse(email.from));
        mimeMessage.Subject = "FWD: " + email.subject;

        // Add all email addresses in the emailToList to the "To" field
        foreach (string emailTo in emailToList)
        {
            mimeMessage.To.Add(new MailboxAddress(emailTo, emailTo));
        }

        Log.Debug($"Starting foward with subject {mimeMessage.Subject} from {mimeMessage.From} to {mimeMessage.To} and message {message}");

        // now to create our body...

        BodyBuilder builder = new();
        // add original info to the email body

        string senderEmail = email.from;
        if (senderEmail.Contains('<'))
        {
            // get the email address between the brackets
            senderEmail = senderEmail.Replace('<', '(');
            senderEmail = senderEmail.Replace('>', ')');
        }

        string cleanedHtmlBody = "";
        if (!string.IsNullOrEmpty(message))
        {
            message = message.Replace("\n", "<br>");
            cleanedHtmlBody += "<p>" + message + "</p>";
        }
        cleanedHtmlBody += "<br/><br/><br/><hr/>";
        cleanedHtmlBody += "<p>------------- Original Message -------------</p>";
        cleanedHtmlBody += "<p><strong>Date:</strong> " + email.date + "</p>";
        cleanedHtmlBody += "<p><strong>From:</strong> " + senderEmail + "</p>";
        cleanedHtmlBody += "<p><strong>To:</strong> " + email.to + "</p>";
        cleanedHtmlBody += "<p><strong>Subject:</strong> " + email.subject + "</p><br/><br/>";

        cleanedHtmlBody += EmailHelper.RemoveTokenFromHTMLBody(email.body, true);

        cleanedHtmlBody = Regex.Replace(cleanedHtmlBody, @"<p([^>]*)>", "<p style='margin: 0; padding: 0;'>");

        builder.HtmlBody = cleanedHtmlBody;

        string plainTextBody = "";
        if (!string.IsNullOrEmpty(message))
        {
            plainTextBody += message + "\n\n";
        }
        // the same for the plaintextbody
        plainTextBody += "\n\n------------- Original Message -------------\n";
        plainTextBody += "Date: " + email.date + "\n";
        plainTextBody += "From: " + email.from + "\n";
        plainTextBody += "To: " + email.to + "\n";
        plainTextBody += "Subject: " + email.subject + "\n\n\n";

        plainTextBody += EmailHelper.RemoveAllHtmlTags(cleanedHtmlBody);

        builder.TextBody = plainTextBody;

        // Get the email attachments
        List<EmailAttachmentItem> attachments = AttachmentModel.getAttachments(executeUser, email.id);

        // Attach each file
        if (attachments.Count >= 1)
        {
            foreach (EmailAttachmentItem attachment in attachments)
            {
                // parse attachment file base64 content to byte array
                byte[] fileBytes = Convert.FromBase64String(attachment.file);
                builder.Attachments.Add(attachment.name, fileBytes);
            }
        }

        mimeMessage.Body = builder.ToMessageBody();

        using SmtpClient client = await EmailServiceMailkit.GetAutenticatedSmtpClientAsync(ConfigManager.SystemEmail);
        try
        {
            client.Send(mimeMessage);
            Log.Debug($"Email forwarded succesfully with subject {mimeMessage.Subject} from {mimeMessage.From} to {mimeMessage.To} and message {message}");

            // mark original email as resolved
            string filteredToken = FilteringModel.GetFilteredTokenByEmailId(email.id, executeUser);
            FilteringModel.ChangeEmailStatus(filteredToken, StatusConstants.StatusCode.RESOLVIDO_MANUALMENTE.ToString(), executeUser);
        }
        catch (Exception e)
        {
            Log.Debug($"Error in fwdEmail Masterferro sending subject {mimeMessage.Subject} to {mimeMessage.To}: " + e);
            throw;
        }
        finally
        {
            client.Disconnect(true);
        }
    }

    public static async Task SendEmailAsync(EmailRequest email, List<IFormFile>? attachments, string account)
    {
        Log.Debug($"Sending email from {account} to {email.to} with subject {email.subject}");
        MimeMessage message = new MimeMessage();
        message.From.Add(MailboxAddress.Parse(account));
        message.Subject = email.subject;

        AddRecipients(email.to, message.To);
        AddRecipients(email.cc, message.Cc);
        AddRecipients(email.bcc, message.Bcc);

        BodyBuilder bodyBuilder = new()
        {
            HtmlBody = email.body,
            TextBody = EmailHelper.RemoveAllHtmlTags(email.body)
        };

        bodyBuilder = EmailHelper.CheckAndAddAttachmentsToEmail(attachments, bodyBuilder);

        message.Body = bodyBuilder.ToMessageBody();

        using SmtpClient client = await EmailServiceMailkit.GetAutenticatedSmtpClientAsync(account);
        try
        {
            await client.SendAsync(message);
            Log.Debug($"Sent email with subject {message.Subject} to {message.To}");
        }
        catch (Exception ex)
        {
            Log.Debug("An error occurred sending email async " + ex);
            throw;
        }
        finally
        {
            await client.DisconnectAsync(true);
        }
    }

    private static void AddRecipients(string? addresses, InternetAddressList target)
    {
        if (string.IsNullOrWhiteSpace(addresses))
        {
            return;
        }

        string[] addressArray = addresses.Split(',', StringSplitOptions.RemoveEmptyEntries);
        foreach (string addr in addressArray)
        {
            // Remove espaços extras e valida se não está vazia
            string cleanedAddress = addr.Trim();
            if (!string.IsNullOrWhiteSpace(cleanedAddress))
            {
                target.Add(MailboxAddress.Parse(cleanedAddress));
            }
        }
    }

    public static async Task ReplyToEmailAsync(string emailId, ReplyRequest replyReq, string executerUser, bool isMarkResolved)
    {
        // Retrieve email data and token
        (EmailItem emailData, string filteredToken) = GetEmailDataAndTokenAsync(emailId, replyReq, executerUser);

        // Create the email reply message
        MimeMessage message = ConstructReplyMessage(emailData, replyReq, filteredToken);

        // Send the reply and save it in the Sent folder
        await SendReplyAndSaveAsync(message, executerUser, filteredToken, isMarkResolved);

        Log.Debug($"Reply sent successfully to {message.To}, sent by {message.From} with token {filteredToken}.");
    }

    private static (EmailItem, string) GetEmailDataAndTokenAsync(string emailId, ReplyRequest replyReq, string executerUser)
    {
        if (replyReq.isReplyToOriginalEmail)
        {
            FilteredEmail filteredEmail = FilteringModel.getFilteredEmail(executerUser, emailId, false)
                ?? throw new InvalidOperationException("Failed to retrieve the token for the original email.");

            EmailItem emailData = EmailModel.getEmail(emailId, executerUser)
                ?? throw new InvalidOperationException("The email could not be found.");

            return (emailData, filteredEmail.token);
        }

        // is reply to another reply
        ReplyItem replyItem = ReplyModel.getReply(emailId, executerUser)
                ?? throw new InvalidOperationException("The email could not be found.");

        ReplyModel.SetReplyToRead(replyItem.reply_token, executerUser);

        EmailItem emailReplyData = new(
            replyItem.id.ToString(),
            replyItem.from,
            replyItem.to,
            replyItem.subject,
            replyItem.body,
            DateTime.Parse(replyItem.date)
        );

        return (emailReplyData, replyItem.email_token);
    }

    private static MimeMessage ConstructReplyMessage(EmailItem emailData, ReplyRequest replyReq, string filteredToken)
    {
        MimeMessage message = new();

        message.From.Add(MailboxAddress.Parse(ConfigManager.SystemEmail));

        // if is not reply to original (is reply to reply),
        // check if there is any email in the destinatary field (the user edited the email)
        if (!string.IsNullOrEmpty(replyReq.destinatary))
        {
            AddEmailAddresses(message.To, replyReq.destinatary);
        }

        // if message.To is empty (either the previous function tried to insert an invalid address
        // or simply it is meant to reply to the original sender), add the original from
        if (message.To.Count == 0)
        {
            AddEmailAddresses(message.To, emailData.from);
        }

        AddEmailAddresses(message.Cc, replyReq.cc);
        AddEmailAddresses(message.Bcc, replyReq.bcc);

        string replyToken = Guid.NewGuid().ToString();
        AddReplyHeaders(message, replyToken, filteredToken, emailData.id);

        message.Subject = FormatSubject(emailData.subject);
        message.Body = CreateEmailBody(emailData, replyReq, replyToken, filteredToken);

        return message;
    }

    private static void AddEmailAddresses(InternetAddressList addressList, string? addresses)
    {
        if (string.IsNullOrEmpty(addresses) || addresses == null)
        {
            return;
        }

        foreach (string address in addresses.Split(','))
        {
            if (!MailboxAddress.TryParse(address, out MailboxAddress mailboxAdd))
            {
                Log.Debug($"Invalid mailbox address: {address}");
                continue;
            }

            string email = address.Contains('<') ? EmailHelper.GetEmailBetweenTriangleBrackets(address) : address.Trim();

            if (!Util.IsValidInputEmail(email))
            {
                Log.Debug($"Invalid email address: {email}");
                continue;
            }

            addressList.Add(mailboxAdd);
        }
    }

    private static void AddReplyHeaders(MimeMessage message, string replyToken, string filteredToken, string originalId)
    {
        message.Headers.Add("X-Reply-Token", replyToken);
        message.Headers.Add("X-Original-Token", filteredToken);

        if (!string.IsNullOrEmpty(originalId))
        {
            message.InReplyTo = originalId;
            message.References.Add($"<reply-token-{replyToken}@engibots.com>");
            message.References.Add($"<original-token-{filteredToken}@engibots.com>");
        }
    }

    private static string FormatSubject(string originalSubject)
    {
        return originalSubject.StartsWith("Re:", StringComparison.OrdinalIgnoreCase) ? originalSubject : $"Re: {originalSubject}";
    }

    private static MimeEntity CreateEmailBody(EmailItem emailData, ReplyRequest replyReq, string replyToken, string filteredToken)
    {
        string sanitizedBody = SanitizeEmailBody(emailData.body);
        string htmlBody = ConstructHtmlBody(replyReq.response, emailData, sanitizedBody, replyToken, filteredToken);

        // Remove <p> tags that directly enclose <br> and keep only the <br>
        htmlBody = Regex.Replace(htmlBody, @"<p>\s*<br>\s*</p>", "<br>", RegexOptions.IgnoreCase);

        // Remove all the padding and margins from the p tags
        htmlBody = Regex.Replace(htmlBody, @"<p([^>]*)>", "<p style='margin: 0; padding: 0;'>");

        BodyBuilder bodyBuilder = new BodyBuilder
        {
            TextBody = $"{replyReq.response}\n------------- Original Message -------------\n{emailData.date}, {emailData.from}:\n\n{emailData.body}",
            HtmlBody = htmlBody
        };

        // Check attachments and add them to message Attachments
        bodyBuilder = EmailHelper.CheckAndAddAttachmentsToEmail(replyReq.attachments, bodyBuilder);

        return bodyBuilder.ToMessageBody();
    }

    private static string SanitizeEmailBody(string originalBody)
    {
        int bodyStart = originalBody.IndexOf("<body", StringComparison.Ordinal);
        int bodyEnd = bodyStart != -1 ? originalBody.IndexOf('>', bodyStart) + 1 : -1;
        int bodyClose = originalBody.IndexOf("</body>", StringComparison.Ordinal);

        if (bodyStart == -1 || bodyEnd == -1 || bodyClose == -1)
        {
            return originalBody;
        }

        string bodyAttributes = originalBody[bodyStart..bodyEnd].Replace("<body", "").Replace(">", "").Trim();
        return $"<div {bodyAttributes}>{originalBody[bodyEnd..bodyClose]}</div>";
    }

    private static string ConstructHtmlBody(string response, EmailItem emailData, string sanitizedBody, string replyToken, string filteredToken)
    {
        return $@"
        <html>
            <body>
                <p>{response}</p>
                <hr/>
                <p>------------- Original Message -------------</p>
                <p><strong>Date:</strong> {emailData.date}</p>
                <p><strong>From:</strong> {emailData.from}</p>
                {sanitizedBody}
                <p style='display:none'>START_REPLY_MSG_ID: {replyToken} END_REPLY_MSG_ID</p>
                <p style='display:none'>START_ORIG_MSG_ID: {filteredToken} END_ORIG_MSG_ID</p>
            </body>
        </html>";
    }

    private static async Task SendReplyAndSaveAsync(MimeMessage message, string executerUser, string filteredToken, bool isMarkResolved)
    {
        try
        {
            await SendReplyAndSaveOnSentFolder(message, ConfigManager.SystemEmail);
        }
        catch (Exception ex)
        {
            Log.Debug($"Failed to send email: {ex.Message}");
            throw;
        }

        message.From.Clear();
        message.From.Add(MailboxAddress.Parse(executerUser));

        ReplyModel.SaveReplyAndAttachmentsToDB(message, filteredToken, message.Headers["X-Reply-Token"], executerUser, true);

        // mark original email as resolved
        if (isMarkResolved)
        {
            FilteringModel.ChangeEmailStatus(filteredToken, StatusConstants.StatusCode.RESOLVIDO_MANUALMENTE.ToString(), executerUser);
        }

        Log.Debug($"Reply sent from {ConfigManager.SystemEmail}, sender: {message.From}, to: {message.To}");
    }

    public static async Task SendReplyAndSaveOnSentFolder(MimeMessage message, string account)
    {
        // Send the reply email to the destinatary
        using SmtpClient client = await EmailServiceMailkit.GetAutenticatedSmtpClientAsync(account);
        try
        {
            await client.SendAsync(message);
            Log.Debug("Reply sent successfully");
        }
        catch (Exception ex)
        {
            Log.Error($"Error sending email: {ex.Message}");
            throw;
        }
        finally
        {
            await client.DisconnectAsync(true);
        }

        // Save the email in the Sent Folder

        using ImapClient imapClient = await EmailServiceMailkit.GetAutenticatedImapClientAsync(account);
        try
        {
            IMailFolder sentFolder = await imapClient.GetFolderAsync(ConfigManager.SentFolder);
            await sentFolder.OpenAsync(FolderAccess.ReadWrite);
            await sentFolder.AppendAsync(message);

            await sentFolder.CloseAsync();
        }
        catch (Exception ex)
        {
            Log.Error($"Error saving email to Sent folder: {ex.Message}");
            throw;
        }
        finally
        {
            await imapClient.DisconnectAsync(true);
        }
    }

    public static async Task MoveEmailAsync(string sourceFolderName, string destinationFolderName, MailKit.UniqueId emailIdToMove, string account)
    {
        using ImapClient client = await EmailServiceMailkit.GetAutenticatedImapClientAsync(account);
        try
        {
            client.Inbox.Open(FolderAccess.ReadOnly);

            // Select the source folder
            IMailFolder sourceFolder = await client.GetFolderAsync(sourceFolderName);
            await sourceFolder.OpenAsync(FolderAccess.ReadWrite);

            // Select the destination folder
            IMailFolder destinationFolder = await client.GetFolderAsync(destinationFolderName);
            if (!destinationFolder.Exists)
            {
                Log.Debug($"Destination folder {destinationFolderName} does not exist.");
                await sourceFolder.CloseAsync();
                return;
            }

            // Move the message
            await sourceFolder.MoveToAsync(emailIdToMove, destinationFolder);

            Log.Debug($"Email moved successfully from {sourceFolderName} to {destinationFolderName}");

            await sourceFolder.CloseAsync();
        }
        catch (Exception ex)
        {
            Log.Debug($"An error occurred while moving the email: {ex.Message}");
        }
        finally
        {
            await client.DisconnectAsync(true);
        }
    }

    public static async Task MoveEmailByTokenAsync(string sourceFolderName, string destinationFolderName, string htmlToken, string account)
    {
        // Stop from moving from folder A to folder A, wasting resources
        if (string.Equals(sourceFolderName, destinationFolderName, StringComparison.OrdinalIgnoreCase))
        {
            Log.Debug($"Email with token {htmlToken} is already in the destination folder {destinationFolderName}");
            return;
        }

        using ImapClient client = await EmailServiceMailkit.GetAutenticatedImapClientAsync(account);
        try
        {
            Log.Debug($"Moving email with token {htmlToken} from {sourceFolderName} to {destinationFolderName}");
            // Open the source folder
            IMailFolder sourceFolder = await client.GetFolderAsync(sourceFolderName);
            await sourceFolder.OpenAsync(FolderAccess.ReadWrite);

            // Open the destination folder
            IMailFolder destinationFolder = await client.GetFolderAsync(destinationFolderName);
            if (!destinationFolder.Exists)
            {
                Log.Debug($"Destination folder {destinationFolderName} does not exist.");
                await sourceFolder.CloseAsync();
                return;
            }

            // Search for the email containing the HTML token
            TextSearchQuery searchQuery = SearchQuery.BodyContains(htmlToken);
            IList<MailKit.UniqueId> uids = await sourceFolder.SearchAsync(searchQuery);

            if (uids.Count == 0)
            {
                Log.Debug($"No emails found with the specified token {htmlToken} in mailbox {account}");
                await sourceFolder.CloseAsync();
                return;
            }

            // Move the email
            foreach (MailKit.UniqueId uid in uids)
            {
                await sourceFolder.MoveToAsync(uid, destinationFolder);
                Log.Debug($"Email with Token {uid} moved successfully to folder {destinationFolderName}");
            }

            await sourceFolder.CloseAsync();
        }
        catch (Exception ex)
        {
            Log.Error($"An error occurred moving the email by token: {ex.Message}");

            // Log all available folders for the account
            try
            {
                Log.Debug("Listing folders inside INBOX.Robot:");

                // Get and open the INBOX.Robot folder
                IMailFolder robotFolder = await client.GetFolderAsync("INBOX.Robot");

                // Log subfolders within INBOX.Robot
                await LogFoldersAsync(robotFolder, "");
            }
            catch (Exception folderEx)
            {
                Log.Error($"An error occurred while listing folders: {folderEx.Message}");
            }
        }
        finally
        {
            await client.DisconnectAsync(true);
        }
    }

    private static async Task LogFoldersAsync(IMailFolder folder, string indent)
    {
        Log.Debug($"{indent}- {folder.FullName}");
        await folder.OpenAsync(FolderAccess.ReadOnly);

        IList<IMailFolder> emailFolders = await folder.GetSubfoldersAsync(false);

        foreach (IMailFolder subfolder in emailFolders)
        {
            await LogFoldersAsync(subfolder, indent + "  ");
        }
    }
}
