// // Copyright (c) 2024 Engibots. All rights reserved.

using engimatrix.Config;
using engimatrix.Connector;
using engimatrix.Exceptions;
using engimatrix.ModelObjs;
using engimatrix.ModelObjs.Primavera;
using engimatrix.Utils;
using PuppeteerSharp;

namespace engimatrix.Models;

public static class OrderPrimaveraDocumentModel
{
    public static readonly string baseQuery = @"SELECT id, order_token, name, type,
            series, number, created_at, created_by, invoice_html
            FROM order_primavera_document ";

    public static async Task Create(OrderPrimaveraDocumentItem doc, string executeUser)
    {
        if (!IsDocValid(doc))
        {
            throw new ArgumentException("Invalid document");
        }

        bool exists = OrderModel.OrderExists(doc.order_token, executeUser);
        if (!exists)
        {
            throw new NotFoundException("Order not found");
        }

        doc.invoice_html = await ConvertHtmlToPdfBase64(doc.invoice_html);

        if (ConfigManager.isProduction)
        {
            doc.name = Cryptography.Encrypt(doc.name, doc.order_token);
            doc.type = Cryptography.Encrypt(doc.type, doc.order_token);
            doc.series = Cryptography.Encrypt(doc.series, doc.order_token);
            doc.number = Cryptography.Encrypt(doc.number, doc.order_token);
            doc.invoice_html = Cryptography.Encrypt(doc.invoice_html, doc.order_token);
        }

        Dictionary<string, string> dic = new()
        {
            { "@OrderToken", doc.order_token },
            { "@DocName", doc.name },
            { "@DocType", doc.type },
            { "@DocSeries", doc.series },
            { "@DocNumber", doc.number },
            { "@CreatedBy", executeUser },
            { "@Invoice", doc.invoice_html }
        };

        string docQuery = $@"
            INSERT INTO order_primavera_document (order_token, name, type, series, number, created_by, invoice_html)
            VALUES (@OrderToken, @DocName, @DocType, @DocSeries, @DocNumber, @CreatedBy, @Invoice)
        ";

        SqlExecuterItem response = SqlExecuter.ExecuteFunction(docQuery, dic, executeUser, false, "CreateOrderPrimaveraDocument");

        if (!response.operationResult)
        {
            throw new DatabaseException($"Failed to create order_primavera_document");
        }
    }

    public static List<OrderPrimaveraDocumentItem> GetOrderDocs(string orderToken, string executeUser)
    {
        Dictionary<string, string> dic = [];
        dic.Add("@OrderToken", orderToken);

        string orderQuery = $"{baseQuery} WHERE order_token = @OrderToken";

        List<OrderPrimaveraDocumentItem> docs = SqlExecuter.ExecuteFunction<OrderPrimaveraDocumentItem>(orderQuery, dic, executeUser, false, "GetOrderDocs");

        if (ConfigManager.isProduction)
        {
            foreach (OrderPrimaveraDocumentItem doc in docs)
            {
                doc.name = Cryptography.Decrypt(doc.name, doc.order_token);
                doc.type = Cryptography.Decrypt(doc.type, doc.order_token);
                doc.series = Cryptography.Decrypt(doc.series, doc.order_token);
                doc.number = Cryptography.Decrypt(doc.number, doc.order_token);
                doc.invoice_html = Cryptography.Decrypt(doc.invoice_html, doc.order_token);
            }
        }

        return docs;
    }

    public static OrderPrimaveraDocumentItem? GetLastOrderDoc(string orderToken, string executeUser)
    {
        Dictionary<string, string> dic = [];
        dic.Add("@OrderToken", orderToken);

        string orderQuery = $"{baseQuery} WHERE order_token = @OrderToken ORDER BY created_at DESC LIMIT 1";

        List<OrderPrimaveraDocumentItem> docs = SqlExecuter.ExecuteFunction<OrderPrimaveraDocumentItem>(orderQuery, dic, executeUser, false, "GetLastOrderDoc");

        if (docs.Count == 0)
        {
            return null;
        }

        OrderPrimaveraDocumentItem doc = docs.First();
        if (ConfigManager.isProduction)
        {
            doc.name = Cryptography.Decrypt(doc.name, doc.order_token);
            doc.type = Cryptography.Decrypt(doc.type, doc.order_token);
            doc.series = Cryptography.Decrypt(doc.series, doc.order_token);
            doc.number = Cryptography.Decrypt(doc.number, doc.order_token);
            doc.invoice_html = Cryptography.Decrypt(doc.invoice_html, doc.order_token);
        }

        return doc;
    }

    public static bool Delete(int id, string orderToken, string executeUser)
    // Here the orderToken is redundant, but it is to make sure that we are deleting the right quotation
    {
        if (id <= 0 || string.IsNullOrEmpty(orderToken))
        {
            throw new ArgumentException("Invalid id or order token");
        }

        Dictionary<string, string> dic = [];
        dic.Add("@Id", id.ToString());
        dic.Add("@OrderToken", orderToken);

        string query = "DELETE FROM order_primavera_document WHERE id = @Id AND order_token = @OrderToken";

        SqlExecuterItem response = SqlExecuter.ExecuteFunction(query, dic, executeUser, true, "DeleteOrderPrimaveraDocument");

        return response.rowsAffected > 0;
    }

    public static async Task<string> ConvertHtmlToPdfBase64(string html)
    {
        if (string.IsNullOrWhiteSpace(html))
        {
            Log.Debug($"received Html is empty");
            return string.Empty;
        }

        byte[] pdfBytes = await ConvertHtmlToPdfByteArray(html);
        return Convert.ToBase64String(pdfBytes);
    }

    public static async Task<byte[]> ConvertHtmlToPdfByteArray(string htmlContent)
    {
        Log.Info("Converting HTML to PDF...");

        // Download the browser revision if needed
        await new BrowserFetcher().DownloadAsync();

        // Launch the browser in headless mode
        using IBrowser browser = await Puppeteer.LaunchAsync(new LaunchOptions { Headless = true });
        using IPage page = await browser.NewPageAsync();

        // Set the HTML content on the page
        await page.SetContentAsync(htmlContent);

        // Define PDF options
        PdfOptions pdfOptions = new()
        {
            Format = PuppeteerSharp.Media.PaperFormat.A4,
            PrintBackground = true,
            MarginOptions = new PuppeteerSharp.Media.MarginOptions
            {
                Top = "20px",
                Right = "20px",
                Bottom = "50px",
                Left = "20px"
            },
            DisplayHeaderFooter = false,
            Scale = 0.7m
        };

        // Generate the PDF and return it as a byte array
        byte[] pdfBytes = await page.PdfDataAsync(pdfOptions);
        Log.Info("HTML converted to PDF successfully");
        return pdfBytes;
    }

    public static bool IsDocValid(OrderPrimaveraDocumentItem doc)
    {
        Dictionary<string, string> fields = new()
            {
                { "OrderToken", doc.order_token },
                { "Name", doc.name },
                { "Type", doc.type },
                { "Series", doc.series },
                { "Number", doc.number },
                { "InvoiceHtml", doc.invoice_html }
            };

        foreach (KeyValuePair<string, string> field in fields)
        {
            if (string.IsNullOrEmpty(field.Value))
            {
                Log.Debug($"Invalid document. Field {field.Key} is empty");
                return false;
            }
        }

        return true;
    }
}
