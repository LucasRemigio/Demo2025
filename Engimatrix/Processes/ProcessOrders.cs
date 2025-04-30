// // Copyright (c) 2024 Engibots. All rights reserved.

using engimatrix.Config;
using engimatrix.Connector;
using engimatrix.ModelObjs;
using Engimatrix.ModelObjs;
using engimatrix.Models;
using engimatrix.Utils;
using Engimatrix.Models;
using engimatrix.Emails;
using System.Text;
using engimatrix.Views;
using System.Linq.Expressions;
using engimatrix.ModelObjs.Primavera;
using engimatrix.PricingAlgorithm;
using Newtonsoft.Json.Linq;

namespace Engimatrix.Processes;

public static class ProcessOrders
{
    private static DateTime lastLogTime = DateTime.Now;

    private static void LogActiveProcessWarning(int minutesBetweenLogs)
    {
        DateTime currentTime = DateTime.Now;

        if (DateTime.Now.Subtract(lastLogTime).TotalMinutes < minutesBetweenLogs)
        {
            return;
        }

        Log.Warning("Process to extract products from pending emails is currently ACTIVE!");
        lastLogTime = currentTime;
    }

    public static async Task CreateOrderFromPendingRequests()
    {
        LogActiveProcessWarning(10);

        List<FilteredEmail> filteredEmailsToExtractProducts = FilteringModel.GetRequestsToValidateAllDetails("System", StatusConstants.StatusCode.A_PROCESSAR);

        if (filteredEmailsToExtractProducts.Count == 0)
        {
            return;
        }

        Log.Info($"CreateOrderFromPendingRequests: {filteredEmailsToExtractProducts.Count} emails pending to process their products");

        foreach (FilteredEmail filteredEmail in filteredEmailsToExtractProducts)
        {
            OrderItem? order = null;
            try
            {
                Log.Debug($"CreateOrderFromPendingRequests: Extracting products from {filteredEmail.email.from} with subject {filteredEmail.email.subject} - Token: {filteredEmail.token}");

                /*
                *    PROCESS EMAIL!
                */
                order = await ProcessEmailAndCreateOrderAsync(filteredEmail);

                // change status to processed
                int status = StatusConstants.StatusCode.AGUARDA_VALIDACAO;
                FilteringModel.ChangeEmailStatusUnvalidated(filteredEmail.token, status.ToString(), "System");
                OrderModel.PatchStatus(order.token, status, "System");
            }
            catch (Exception ex)
            {
                Log.Error($"An error occurred while extracting products from email {filteredEmail.token}: {ex}");
                // save with error status
                int errorStatus = StatusConstants.StatusCode.ERRO;
                FilteringModel.ChangeEmailStatus(filteredEmail.token, errorStatus.ToString(), "System");
                if (!string.IsNullOrEmpty(order?.token))
                {
                    OrderModel.PatchStatus(order.token, errorStatus, "System");
                }

            }

            try
            {
                /*
                *    SEND EMAIL!
                */
                if (!await IsOrderValidToSendQuotationEmail(filteredEmail, order))
                {
                    Log.Error($"CreateOrderFromPendingRequests: Order is not valid to send quotation email for email {filteredEmail.token} with subject {filteredEmail.email.subject} and sender {filteredEmail.email.from}. Not sending the email");
                    continue;
                }

                // await SendValidationEmailToPendingRequest(filteredEmail);
            }
            catch (Exception ex)
            {
                Log.Error($"An error occurred while sending confirmation email to {filteredEmail.email.from}: {ex}");
            }
        }
    }

    private static async Task<bool> IsOrderValidToSendQuotationEmail(FilteredEmail filteredEmail, OrderItem? order)
    {
        if (order is null)
        {
            Log.Debug($"IsOrderValidToSendQuotationEmail: Order is null for email {filteredEmail.token} with subject {filteredEmail.email.subject} and sender {filteredEmail.email.from}");
            return false;
        }

        if (string.IsNullOrEmpty(order.client_code))
        {
            Log.Debug($"IsOrderValidToSendQuotationEmail: Order client code is null for email {filteredEmail.token} with subject {filteredEmail.email.subject} and sender {filteredEmail.email.from}");
            return false;
        }

        // verify the matched client has the same email domain as the email from
        string emailDomain = filteredEmail.email.from.Split('@')[1];
        MFPrimaveraClientItem? primaveraClient = await PrimaveraClientModel.GetPrimaveraClient(order.client_code);

        if (primaveraClient is null)
        {
            Log.Debug($"IsOrderValidToSendQuotationEmail: Primavera client is null for email {filteredEmail.token} with subject {filteredEmail.email.subject} and sender {filteredEmail.email.from}");
            return false;
        }

        if (string.IsNullOrEmpty(primaveraClient.Email))
        {
            Log.Debug($"IsOrderValidToSendQuotationEmail: Primavera client email is null for email {filteredEmail.token} with subject {filteredEmail.email.subject} and sender {filteredEmail.email.from}");
            return false;
        }

        if (!primaveraClient.Email.Contains(emailDomain))
        {
            Log.Debug($"IsOrderValidToSendQuotationEmail: Primavera client email {primaveraClient.Email} does not contain the original email domain {emailDomain}");
            return false;
        }

        return true;
    }

    private static async Task<OrderItem> ProcessEmailAndCreateOrderAsync(FilteredEmail filteredEmail)
    {
        int categoryId = Int32.Parse(filteredEmail.category);
        bool isDraft = categoryId == CategoryConstants.CategoryCode.COTACOES_ORCAMENTOS;

        // Reads attachments and email body and gives a body with all the attachments and images content
        string emailBody = await ProcessEmailContentAsync(filteredEmail);

        // This gives the original email body without all the tags, so just simple plain text without any aditional info
        string cleanSimpleEmailBody = CleanEmailBodyText(filteredEmail.email.body);
        filteredEmail.email.body = cleanSimpleEmailBody;

        string orderToken = Guid.NewGuid().ToString();
        OrderItemBuilder orderBuilder = new OrderItemBuilder()
            .SetToken(orderToken)
            .SetEmailToken(filteredEmail.token)
            .SetCreatedBy("Sistema")
            .SetIsDelivery(true)
            .SetStatusId(StatusConstants.StatusCode.A_PROCESSAR)
            .SetIsDraft(isDraft);

        // Identify the client
        Log.Debug($"ProcessEmailAndCreateOrderAsync: Identifying client: {filteredEmail.email.from} - {filteredEmail.email.subject}");


        string clientCode = await ClientIdentifier.IdentifyClient(filteredEmail);

        Log.Debug($"ProcessEmailAndCreateOrderAsync: Client identified: {clientCode}");
        orderBuilder = AddClientCodeToOrderBuilderIfValid(orderBuilder, clientCode);

        OrderItem order = orderBuilder.Build();
        OrderModel.CreateOrder(order, "System");

        // Extract the products from the email and associate to order
        Log.Debug($"ProcessEmailAndCreateOrderAsync: Extracting products from email: {filteredEmail.email.from} - {filteredEmail.email.subject}");
        List<ProdutoComparado> extractedProducts = await ProductExtractor.ExtractProductsFromEmailOpenAI(emailBody);
        Log.Debug($"ProcessEmailAndCreateOrderAsync: Products extracted: {extractedProducts.Count}");
        decimal total = PricingCalculator.CalculateQuotePriceAndSaveProducts(extractedProducts, order);
        Log.Debug($"ProcessEmailAndCreateOrderAsync: Order created with total: {total} Eur.");
        return order;
    }

    /// <summary>
    /// Processes email content by extracting text from the email body, embedded images and attachments
    /// </summary>
    /// <param name="filteredEmail">The filtered email to process</param>
    /// <returns>Combined text content from the email body and all visual elements</returns>
    private static async Task<string> ProcessEmailContentAsync(FilteredEmail filteredEmail)
    {
        // 1. Process the basic email text content
        string cleanedEmailBody = CleanEmailBodyText(filteredEmail.email.body);
        StringBuilder contentBuilder = new(cleanedEmailBody);

        // 2. Process embedded images from email body
        await ProcessEmbeddedImagesAsync(filteredEmail, contentBuilder);

        // 3. Process attachments (PDFs, images, etc.)
        await ProcessEmailAttachmentsAsync(filteredEmail, contentBuilder);

        return contentBuilder.ToString();
    }

    /// <summary>
    /// Cleans up HTML from email body text
    /// </summary>
    private static string CleanEmailBodyText(string emailBody)
    {
        string cleanText = EmailHelper.RemoveAllHtmlTags(emailBody);
        return cleanText.Replace("&nbsp;", " ");
    }

    /// <summary>
    /// Processes embedded images in the email body and extracts their text content
    /// </summary>
    private static async Task ProcessEmbeddedImagesAsync(FilteredEmail filteredEmail, StringBuilder contentBuilder)
    {
        Log.Debug($"ProcessEmbeddedImagesAsync: Processing embedded images for {filteredEmail.email.from} - {filteredEmail.email.subject}");

        (string imagesContent, int imagesCount) = await AttachmentReader.ReadImagesFromEmailBody(filteredEmail.email.body);

        if (string.IsNullOrEmpty(imagesContent))
        {
            Log.Debug("ProcessEmbeddedImagesAsync: No embedded images found or no text could be extracted from them");
            return;
        }

        Log.Debug($"ProcessEmbeddedImagesAsync: Processed {imagesCount} embedded images from email body");
        contentBuilder.Append(imagesContent);
    }

    /// <summary>
    /// Processes email attachments and extracts their text content
    /// </summary>
    private static async Task ProcessEmailAttachmentsAsync(FilteredEmail filteredEmail, StringBuilder contentBuilder)
    {
        List<EmailAttachmentItem> attachments = AttachmentModel.getAttachments("System", filteredEmail.email.id);

        if (attachments.Count == 0)
        {
            Log.Debug("ProcessEmailAttachmentsAsync: No attachments found in email");
            return;
        }

        Log.Debug($"ProcessEmailAttachmentsAsync: Processing {attachments.Count} email attachments");
        (string content, int pagesCount) = await AttachmentReader.ReadEmailAttachmentsListAsync(attachments);

        if (string.IsNullOrEmpty(content))
        {
            Log.Debug("ProcessEmailAttachmentsAsync: No text could be extracted from attachments");
            return;
        }

        Log.Debug($"ProcessEmailAttachmentsAsync: Processed {pagesCount} pages from email attachments");
        contentBuilder.Append(content);
    }

    public static OrderItemBuilder AddClientCodeToOrderBuilderIfValid(OrderItemBuilder orderBuilder, string clientCode)
    {
        if (string.IsNullOrEmpty(clientCode))
        {
            return orderBuilder;
        }

        // make sure the client code exists by fetching the primavara clients dictionary. if it has a result, lets go
        MFPrimaveraClientItem? primaveraClient = PrimaveraClientModel.GetPrimaveraClient(clientCode).Result;
        if (primaveraClient == null)
        {
            Log.Warning($"Client code {clientCode} not found in Primavera");
            return orderBuilder;
        }

        // string is not null and primavera client indeed exists
        return orderBuilder.SetClientCode(clientCode);
    }

    public static async Task SendValidationEmailToPendingRequest(FilteredEmail filteredEmail)
    {
        int categoryId = Int32.Parse(filteredEmail.category);
        string categoryName = CategoryConstants.GetCategoryNameForEmailByCode(categoryId);

        Log.Debug($"Sending confirmation email to {filteredEmail.email.from} on category: {categoryName}");

        // Send email to client
        string validationEmailBody = CreateRequestValidationEmailBody(filteredEmail.token, "pt", categoryId);
        ReplyRequest replyReq = new()
        {
            response = validationEmailBody,
            attachments = null,
            isReplyToOriginalEmail = true,
            cc = "",
            bcc = ""
        };
        // Only uncomment this line when the quotation goes directly to the client, so that he can confirm
        // without an operator checking first if everything is right
        // await MasterFerro.ReplyToEmailAsync(filteredEmail.email.id, replyReq, ConfigManager.SystemEmail, false);

        Log.Debug("Confirmation email sent to " + filteredEmail.email.from);
    }

    public static async Task SendEmailToOrdersConfirmed()
    {
        List<OrderItem> confirmedOrders = OrderModel.GetOrdersConfirmedByOperatorOrClient("System");

        if (confirmedOrders.Count == 0)
        {
            return;
        }

        Log.Info($"SendEmailToOrdersConfirmed: {confirmedOrders.Count} emails pending to send confirmation email");

        foreach (OrderItem order in confirmedOrders)
        {
            if (order.email_token == null)
            {
                Log.Error($"SendEmailToOrdersConfirmed: Order {order.token} does not have a filtered email");
                continue;
            }

            // create the order document
            try
            {
                // if the request was a quotation, we must convert it and set is_draft to 1
                // but if the request as already an order, 
                Log.Debug("SendEmailToOrdersConfirmed: Order is draft: " + order.is_draft);
                if (order.is_draft)
                {
                    Log.Debug("SendEmailToOrdersConfirmed: Order is draft, creating order document from quotation");
                    // in case it was a quotation, we must create the order document from the quotation
                    await OrderPrimaveraDocumentModel.CreateOrderDocumentsFromQuotation(order.token, "System");
                }
                else
                {
                    Log.Debug("SendEmailToOrdersConfirmed: Order is not draft, creating order document");
                    // in case it was an order and it was confirmed by anyone, having it being an operator or the client, we must create the order document
                    // which did not have any quotation before
                    await OrderPrimaveraDocumentModel.CreateOrderDocuments(order.token, "System");
                }
            }
            catch (Exception ex)
            {
                Log.Error($"An error occurred while creating order document for order {order.token}: {ex}");
                // change the email status
                int status = StatusConstants.StatusCode.ERRO_ENVIAR_PRIMAVERA;
                OrderModel.PatchStatus(order.token, status, "System");
                continue;
            }

            try
            {
                /*
                * SEND EMAIL!
                */
                // await SendConfirmationEmailToConfirmedOrder(order);
            }
            catch (Exception ex)
            {
                Log.Error($"An error occurred while sending confirmation email to {order.token} with email token {order.email_token}: {ex}");
            }

            try
            {
                // change the email status
                int status = StatusConstants.StatusCode.ENVIADO_PARA_PRIMAVERA;
                OrderModel.PatchStatus(order.token, status, "System");

                Log.Info($"Changed email {order.email_token} and order {order.token} to status {StatusConverter.Convert(status)}");
            }
            catch (Exception ex)
            {
                Log.Error($"An error occurred while changing order status for order {order.token}: {ex}");
            }

        }
    }

    public static async Task SendConfirmationEmailToConfirmedOrder(OrderItem order)
    {
        FilteredEmail filteredEmail = FilteringModel.getFilteredEmail("System", order.email_token, true);
        if (string.IsNullOrEmpty(filteredEmail.token))
        {
            Log.Error($"SendConfirmationEmailToConfirmedOrders: Filtered email not found for order {order.token}");
            return;
        }

        Log.Debug($"SendEmailToOrdersConfirmed: Sending confirmation email to {filteredEmail.email.from} with subject {filteredEmail.email.subject} - Token: {filteredEmail.token}");

        int statusId = StatusConverter.Convert(filteredEmail.status);

        string confirmationEmailBody = CreateOrderConfirmationEmailBody(order, "pt", statusId);

        // Send email to client
        ReplyRequest replyReq = new()
        {
            response = confirmationEmailBody,
            attachments = null,
            isReplyToOriginalEmail = true,
            cc = "",
            bcc = ""
        };

        // await MasterFerro.ReplyToEmailAsync(filteredEmail.email.id, replyReq, ConfigManager.SystemEmail, false);

        Log.Debug("Confirmation email sent to " + filteredEmail.email.from);
    }



    public static string CreateOrderConfirmationEmailBody(OrderItem order, string lang, int statusId)
    {
        StringBuilder result = new();

        bool isOperator = statusId == StatusConstants.StatusCode.CONFIRMADO_POR_OPERADOR;
        string orderAddress = GetFormattedAddress(order);

        if (lang != "pt")
        {
            if (string.IsNullOrEmpty(orderAddress))
            {
                orderAddress = "Client picks up at Masterferro";
            }
            result.Append("<p>Dear Customer,</p>");
            result.Append("<br/><br/>");
            result.Append(isOperator
                ? "<p>We are pleased to inform you that your order has been confirmed by one of our operators.</p>"
                : "<p>Thank you for your confirmation.</p>");
            result.Append("<p>Your order has been registered.</p>");
            result.Append("<h3>Order Summary:</h3>");
            result.Append("<p><b>Delivery Address:</b> " + orderAddress + "</p>");
            result.Append("<p><b>Products:</b></p>");
            result.Append("<table border='1' style=\"font-family: Arial, sans-serif; padding: 10px;\">");
            result.Append("<tr><th>Product</th><th>Quantity</th></tr>");
            result.Append(GetProductTableContent(order));
            result.Append("</table>");
            result.Append("<br/><br/>");
            result.Append("<p>Thank you for your order!</p>");
            result.Append("<p>The Masterferro team wishes you a wonderful day!</p>");
        }
        else
        {
            if (string.IsNullOrEmpty(orderAddress))
            {
                orderAddress = "Cliente levanta na Masterferro";
            }
            result.Append("<p>Estimado Cliente,</p>");
            result.Append("<br/><br/>");
            result.Append(isOperator
                ? "<p>Temos o prazer de informar que a sua encomenda foi confirmada por um dos nossos operadores.</p>"
                : "<p>Obrigado pela sua confirmação.</p>");
            result.Append("<p>Informamos que a sua encomenda se encontra registada.</p>");
            result.Append("<h3>Resumo da Encomenda:</h3>");
            result.Append("<p><b>Morada de Entrega:</b> " + orderAddress + "</p>");
            result.Append("<p><b>Produtos:</b></p>");
            result.Append("<table border='1' style=\"font-family: Arial, sans-serif; padding: 10px;\">");
            result.Append("<tr><th>Produto</th><th>Quantidade</th></tr>");
            result.Append(GetProductTableContent(order));
            result.Append("</table>");
            result.Append("<br/><br/>");
            result.Append("<p>Agradecemos a sua encomenda!</p>");
            result.Append("<p>A sua equipa da Masterferro deseja-lhe a continuação de um excelente dia!</p>");
        }

        string signature = SignatureModel.GetDefaultFormattedSignature(string.Empty);
        result.Append("<br>");
        result.Append(signature);

        return result.ToString();
    }

    private static string GetFormattedAddress(OrderItem order)
    {
        if (string.IsNullOrEmpty(order.address))
        {
            return string.Empty;
        }

        return $"{order.address}, {order.postal_code} {order.locality}, {order.district} {order.city}";
    }

    private static string GetProductTableContent(OrderItem order)
    {
        List<OrderProductItem> orderProducts = OrderProductModel.GetOrderProducts(order.token, "System");

        List<string> productIds = [.. orderProducts.Select(p => p.product_catalog_id.ToString())];
        List<ProductCatalogItem> orderCatalogs = ProductCatalogModel.GetProductCatalogsByIds(productIds, false, "System");
        Dictionary<int, ProductCatalogItem> orderCatalogsDic = orderCatalogs.ToDictionary(p => p.id);

        List<ProductUnitItem> productUnits = ProductUnitModel.GetProductUnits("System");
        Dictionary<int, ProductUnitItem> productUnitsDic = productUnits.ToDictionary(p => p.id);

        StringBuilder result = new();
        foreach (OrderProductItem p in orderProducts)
        {
            if (!orderCatalogsDic.TryGetValue(p.product_catalog_id, out ProductCatalogItem? productCatalog))
            {
                Log.Error($"Product catalog not found for product id {p.product_catalog_id} in template of CreateOrderConfirmationEmailBody");
                continue;
            }
            if (!productUnitsDic.TryGetValue(p.product_unit_id, out ProductUnitItem? productUnit))
            {
                Log.Error($"Product unit not found for product id {p.product_unit_id} in template of CreateOrderConfirmationEmailBody");
                continue;
            }

            result.Append("<tr>");
            result.Append($"<td> {productCatalog.description_full} </td>");
            result.Append($"<td> {p.quantity} {productUnit.abbreviation} </td>");
            result.Append("</tr>");
        }

        return result.ToString();
    }


    public static string CreateRequestValidationEmailBody(string orderToken, string language, int categoryId)
    {
        string baseUrl = ConfigManager.ClientEndpoint().TrimEnd('/');
        string confirmationPath = "order/confirmation";
        string fullUrl = $"{baseUrl}/{confirmationPath}/{orderToken}";

        // Define type-specific words
        bool isCategoryOrder = categoryId == CategoryConstants.CategoryCode.ENCOMENDAS;
        string typeWordEn = isCategoryOrder ? "order" : "quotation";
        string typeWordPt = isCategoryOrder ? "encomenda" : "cotação";

        StringBuilder result = new();

        if (language != "pt")
        {
            result.Append("<p>Dear Customer,</p>");
            result.Append("<br>");
            result.Append($"<p>Thank you for your {typeWordEn}!</p>");
            result.Append("<br>");
            result.Append($"<p>We are optimizing our {typeWordEn} request process via email. Therefore, we kindly ask you to confirm your {typeWordEn} by clicking on this <a href='{fullUrl}'>link</a>.</p>");
            result.Append("<br>");
            result.Append("<p>We appreciate your cooperation and look forward to assisting you with your request!</p>");
            result.Append("<br>");
            result.Append("<p>The Masterferro team wishes you a wonderful day!</p>");
            result.Append("<br/>");
        }
        else
        {
            result.Append("<p>Estimado Cliente,</p>");
            result.Append("<br>");
            result.Append($"<p>Agradecemos o seu pedido de {typeWordPt}!</p>");
            result.Append("<br>");
            result.Append($"<p>Estamos a otimizar o nosso processo de solicitação de {typeWordPt} por e-mail. Por isso, solicitamos que confirme o seu pedido clicando neste <a href='{fullUrl}'>link</a>.</p>");
            result.Append("<br>");
            result.Append("<p>Agradecemos a sua colaboração e estamos à disposição para ajudar com a sua solicitação!</p>");
            result.Append("<br>");
            result.Append("<p>A sua equipa da Masterferro deseja-lhe um excelente dia!</p>");
            result.Append("<br/>");
        }

        return result.ToString();
    }

    public static string GetMasterFerroLogo()
    {
        return "\r\n<div style='max-width: 200px;'>\r\n" +
        $"   <img width=\"200\" width=\"100\" style='width: 100px; max-width:150px; height: auto; display: block;' src='data:image/png;base64,{ConfigManager.Logo}' alt='Logo' />\r\n " +
        "</div>\r\n";
    }
}
