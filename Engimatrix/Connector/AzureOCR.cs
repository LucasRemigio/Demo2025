// // Copyright (c) 2024 Engibots. All rights reserved.

using System.Diagnostics;
using System.Net.Http.Headers;
using System.Text;
using Newtonsoft.Json.Linq;
using engimatrix.Config;
using engimatrix.Utils;

namespace engimatrix.Connector;
public static class AzureOCR
{
    public static void ConvertPdfToImages(string pdfFilePath, string outputFolderPath)
    {
        string ghostscriptPath = Directory.GetCurrentDirectory() + "/gs/gs10.01.1/bin/gswin32c.exe";
        string outputFileName = "page_%03d.jpg"; // Output file name pattern, where %03d represents the page number

        string outputPath = Path.Combine(outputFolderPath, outputFileName);

        ProcessStartInfo startInfo = new()
        {
            FileName = ghostscriptPath,
            Arguments = string.Format("-sDEVICE=jpeg -o \"{0}\" -r300 \"{1}\"", outputPath, pdfFilePath),
            RedirectStandardOutput = true,
            RedirectStandardError = true,
            UseShellExecute = false,
            CreateNoWindow = true,
        };

        using Process process = Process.Start(startInfo);

        process.WaitForExit();
        if (process.ExitCode != 0)
        {
            string errorMessage = process.StandardError.ReadToEnd();
            Console.WriteLine($"Error converting PDF to images: {errorMessage}");
            return;
        }
    }

    private static byte[] GetByteArrayOfImage(string imageFilePath)
    {
        // Open a read-only file stream for the specified file.
        using FileStream fileStream =
            new(imageFilePath, FileMode.Open, FileAccess.Read);
        // Read the file's contents into a byte array.
        BinaryReader binaryReader = new BinaryReader(fileStream);
        return binaryReader.ReadBytes((int)fileStream.Length);
    }

    public static async Task<string> ExtractTextUsingReadAPI(string imageFilePath)
    {
        try
        {
            HttpResponseMessage response;
            HttpClient client = new();

            string key = ConfigManager.AzureOCRKey;
            string endpoint = ConfigManager.AzureOCREndpoint;

            // request headers
            client.DefaultRequestHeaders.Add("Ocp-Apim-Subscription-Key", key);
            string url = endpoint + "vision/v3.1/read/analyze";

            // operationLocation stores the URI of the second REST API method,
            // returned by the first REST API method.
            string operationLocation;

            byte[] dataBytes = GetByteArrayOfImage(imageFilePath);
            using (ByteArrayContent content = new(dataBytes))
            {
                content.Headers.ContentType = new MediaTypeHeaderValue("application/octet-stream");

                // first method
                response = await client.PostAsync(url, content);
            }

            if (response.IsSuccessStatusCode)
            {
                operationLocation = response.Headers.GetValues("Operation-Location").FirstOrDefault();
            }
            else
            {
                string errorContent = await response.Content.ReadAsStringAsync();
                Log.Error("Azure OCR error - " + JToken.Parse(errorContent).ToString());
                return string.Empty;
            }

            string contentString;
            int i = 0;
            do
            {
                Thread.Sleep(1000);

                // second method
                response = await client.GetAsync(operationLocation);
                contentString = await response.Content.ReadAsStringAsync();
                ++i;
            }
            while (i < 30 && !contentString.Contains("\"status\":\"succeeded\"", StringComparison.OrdinalIgnoreCase));

            if (i == 30 && !contentString.Contains("\"status\":\"succeeded\"", StringComparison.OrdinalIgnoreCase))
            {
                Log.Error("Azure OCR timeout! - ");
                return string.Empty;
            }

            JObject json = JObject.Parse(contentString);
            StringBuilder result = new();

            JArray readResults = (JArray)json["analyzeResult"]["readResults"];
            foreach (JToken readResult in readResults)
            {
                JArray lines = (JArray)readResult["lines"];
                foreach (JToken line in lines)
                {
                    string text = (string)line["text"];
                    result.AppendLine(text);
                }
            }

            return result.ToString();
        }
        catch (Exception e)
        {
            Log.Error("Azure OCR general error! - " + e.ToString());
            return string.Empty;
        }
    }
}
