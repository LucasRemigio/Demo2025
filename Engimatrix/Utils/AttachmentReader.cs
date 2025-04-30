// // Copyright (c) 2024 Engibots. All rights reserved.

using System.Globalization;
using System.Text;
using System.Text.RegularExpressions;
using engimatrix.Connector;
using Engimatrix.ModelObjs;

namespace engimatrix.Utils;
public static partial class AttachmentReader
{

    public static async Task<(string content, int pagesCount)> ReadImagesFromEmailBody(string emailBody)
    {
        // The body will be HTML with images in base64 format. Extract them and read them
        // using regex to find base64 images in the email body
        string pattern = @"<img[^>]+src=""data:image/(?<type>[^;]+);base64,(?<data>[^""]+)""[^>]*>";
        MatchCollection matches = Regex.Matches(emailBody, pattern, RegexOptions.IgnoreCase);

        if (matches.Count == 0)
        {
            Log.Debug("No embedded images found in the email body");
            return (string.Empty, 0);
        }

        StringBuilder combinedContent = new();
        int totalPages = 0;

        foreach (Match match in matches)
        {
            // Extract the base64 data and image type
            string base64Data = match.Groups["data"].Value;
            string imageType = match.Groups["type"].Value;

            // make sure the image is at least 50x50 pixels and maximum 10000x10000 pixels
            Log.Debug($"Found embedded {imageType} image in email body");
            if (!IsImageBase64Valid(match) || !IsImageTypeValid(imageType))
            {
                Log.Debug($"Invalid image dimensions or type for embedded {imageType} image in email body");
                continue;
            }

            // Process this image using our existing method for image attachments
            string fileName = $"embedded_{Guid.NewGuid()}.{imageType}";
            (string imageContent, int imagePages) = await ReadImageAsync(base64Data, fileName);

            if (!string.IsNullOrEmpty(imageContent))
            {
                combinedContent.Append(imageContent);
                combinedContent.AppendLine(); // Add line break between different images
                totalPages += imagePages;
            }
        }

        return (combinedContent.ToString(), totalPages);
    }

    public static bool IsImageTypeValid(string imageType)
    {
        if (string.IsNullOrEmpty(imageType))
        {
            return false;
        }

        // Check if the base64 data is valid and the image type is supported
        string[] supportedTypes = { "jpeg", "png", "gif", "bmp", "tiff" };

        if (!supportedTypes.Contains(imageType))
        {
            Log.Debug($"Unsupported image type: {imageType}");
            return false;
        }

        return true;
    }

    /// <summary>
    /// Validates that an embedded image has acceptable dimensions
    /// </summary>
    /// <param name="imageRegexMatch">The regex match containing image data</param>
    /// <returns>True if the image dimensions are valid</returns>
    public static bool IsImageBase64Valid(Match imageRegexMatch)
    {
        // Return false if there's no match data
        if (string.IsNullOrEmpty(imageRegexMatch.Value))
        {
            return false;
        }

        // Extract width and height attributes from the img tag
        Match dimensionsMatch = ImageSizeRegex().Match(imageRegexMatch.Value);

        // If no dimensions found, can't validate
        if (!dimensionsMatch.Success)
        {
            Log.Debug("Image dimensions not found in img tag");
            return false;
        }

        // Extract dimension values
        string widthStr = dimensionsMatch.Groups["width"].Value;
        string heightStr = dimensionsMatch.Groups["height"].Value;

        // Parse dimensions
        bool validWidth = int.TryParse(widthStr, out int width);
        bool validHeight = int.TryParse(heightStr, out int height);

        // If parsing failed
        if (!validWidth || !validHeight)
        {
            Log.Debug("Failed to parse image dimensions");
            return false;
        }

        // Check if dimensions are within acceptable range (50x50 to 10000x10000)
        bool isValidSize = width >= 50 && height >= 50 && width <= 10000 && height <= 10000;

        if (!isValidSize)
        {
            Log.Debug($"Image dimensions are not valid: {width}x{height}");
            return false;
        }

        Log.Debug($"Image dimensions are valid: {width}x{height}");
        return true;
    }


    public static async Task<(string content, int pagesCount)> ReadEmailAttachmentsListAsync(List<EmailAttachmentItem> attachment)
    {
        StringBuilder pdfContent = new();
        int pdfPagesCount = 0;
        foreach (EmailAttachmentItem att in attachment)
        {
            (string content, int pagesCount) = await ReadEmailAttachmentsAsync(att);
            pdfPagesCount += pagesCount;

            if (string.IsNullOrEmpty(content))
            {
                continue;
            }

            pdfContent.Append(content);
            pdfContent.AppendLine(); // Add line break between different attachments
        }

        return (pdfContent.ToString(), pdfPagesCount);
    }

    public static async Task<(string content, int pagesCount)> ReadEmailAttachmentsAsync(EmailAttachmentItem attachment)
    {
        string defaultContent = string.Empty;
        int defaultPagesCount = 0;

        // Check if the attachment is a PDF
        if (IsPdfAttachment(attachment.name))
        {
            return await ReadPdfFileAsync(attachment.file, attachment.name);
        }
        // Check if the attachment is an image
        else if (IsImageAttachment(attachment.name))
        {
            return await ReadImageAsync(attachment.file, attachment.name);
        }

        // Not a supported file type
        return (defaultContent, defaultPagesCount);
    }

    private static bool IsPdfAttachment(string attachName)
    {
        return attachName.EndsWith(".pdf", StringComparison.OrdinalIgnoreCase);
    }

    private static bool IsImageAttachment(string attachName)
    {
        string[] supportedExtensions = [".jpg", ".jpeg", ".png", ".gif", ".bmp", ".tiff", ".tif"];
        return supportedExtensions.Any(ext => attachName.EndsWith(ext, StringComparison.OrdinalIgnoreCase));
    }

    private static async Task<(string content, int pagesCount)> ReadPdfFileAsync(string base64, string fileName)
    {
        string defaultContent = string.Empty;
        int defaultPagesCount = 0;

        // Create a temporary folder for processing
        string tempFolder = CreateTemporaryFolder();

        try
        {
            // Save the attachment to disk
            string attachmentFilePath = SaveFileToDisk(base64, tempFolder, fileName);

            // Convert PDF to images and get the image files
            string[] jpegFiles = ConvertPdfToImages(attachmentFilePath, tempFolder);

            // Check if the PDF has too many pages
            if (jpegFiles.Length > 7)
            {
                Log.Warning($"PDF has too many pages ({jpegFiles.Length}), skipping");
                return (defaultContent, defaultPagesCount);
            }

            // Extract text from all images
            string extractedText = await ExtractTextFromImagesAsync(jpegFiles);

            return (extractedText, jpegFiles.Length);
        }
        catch (Exception e)
        {
            Log.Error("Error processing PDF attachment - " + e.ToString());
            return (defaultContent, defaultPagesCount);
        }
        finally
        {
            CleanupTemporaryFolder(tempFolder);
        }
    }


    private static async Task<(string content, int pagesCount)> ReadImageAsync(string base64) => await ReadImageAsync(base64, null);
    private static async Task<(string content, int pagesCount)> ReadImageAsync(string base64, string? fileName)
    {
        string tempFolder = CreateTemporaryFolder();

        try
        {
            // Save the image to disk
            string imageFilePath = SaveFileToDisk(base64, tempFolder, fileName);

            // Extract text from the image directly using OCR
            Log.Info($"Reading image attachment - {imageFilePath}");
            string extractedText = await AzureOCR.ExtractTextUsingReadAPI(imageFilePath);

            return (extractedText, 1); // Image counts as 1 page
        }
        catch (Exception e)
        {
            Log.Error("Error processing image attachment - " + e.ToString());
            return (string.Empty, 0);
        }
        finally
        {
            CleanupTemporaryFolder(tempFolder);
        }
    }

    private static string CreateTemporaryFolder()
    {
        string tempFolder = Path.Combine(Log.logsFolder, "temp_attachments");
        if (!Directory.Exists(tempFolder))
        {
            Directory.CreateDirectory(tempFolder);
        }
        return tempFolder;
    }

    private static string SaveFileToDisk(string base64, string folder, string? fileName)
    {
        byte[] fileBytes = Convert.FromBase64String(base64);

        string newFileName = fileName ?? Guid.NewGuid().ToString() + ".jpg";
        string filePath = Path.Combine(folder, newFileName);

        File.WriteAllBytes(filePath, fileBytes);
        Log.Debug($"Saved attachment to {filePath}");

        return filePath;
    }

    private static string[] ConvertPdfToImages(string pdfFilePath, string outputFolder)
    {
        Log.Info("Converting PDF to images...");
        AzureOCR.ConvertPdfToImages(pdfFilePath, outputFolder);

        string[] jpegFiles = Directory.GetFiles(outputFolder, "*.jpg");
        Log.Info($"Split PDF into {jpegFiles.Length} images");

        return jpegFiles;
    }

    private static async Task<string> ExtractTextFromImagesAsync(string[] imageFiles)
    {
        StringBuilder pdfContent = new();

        foreach (string filePath in imageFiles)
        {
            Log.Info("Reading image - " + filePath);
            string imageText = await AzureOCR.ExtractTextUsingReadAPI(filePath);

            if (string.IsNullOrEmpty(imageText)) { continue; }

            Log.Debug("Image text: " + imageText);
            pdfContent.Append(imageText);
        }

        return pdfContent.ToString();
    }

    private static void CleanupTemporaryFolder(string folder)
    {
        if (!Directory.Exists(folder)) { return; }

        try
        {
            Directory.Delete(folder, true);
            Log.Debug($"Cleaned up temporary folder: {folder}");
        }
        catch (Exception ex)
        {
            Log.Warning($"Failed to clean up temporary folder: {ex.Message}");
        }
    }


    [GeneratedRegex(@"width=""(?<width>\d+)"" height=""(?<height>\d+)""")]
    private static partial Regex ImageSizeRegex();
}
