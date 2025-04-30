// // Copyright (c) 2024 Engibots. All rights reserved.

using System.Drawing;
using System.Drawing.Imaging;
using System.Text;
using System.Text.Json;
using HtmlAgilityPack;
using iTextSharp.text.pdf;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using engimatrix.Config;
using engimatrix.Connector;
using engimatrix.ModelObjs;
using engimatrix.Models;
using engimatrix.Utils;
using engimatrix.Views;
using Exception = System.Exception;


namespace engimatrix.Processes
{
    public static class Hrb
    {
        public static async Task<JObject> ProcessLocalPdfAsync(string pdfFilePath)
        {
            JObject jsonRes = null;
            try
            {
                if (!File.Exists(pdfFilePath))
                {
                    Console.WriteLine("PDF file not found.");
                    return jsonRes;
                }

                // Create a temporary folder to store intermediate files
                string tempFolder = Path.Combine(Path.GetTempPath(), Guid.NewGuid().ToString());
                Directory.CreateDirectory(tempFolder);

                // Convert PDF to images
                AzureOCR.ConvertPdfToImages(pdfFilePath, tempFolder);

                // Get JPEG files from the temp folder
                string[] jpegFiles = Directory.GetFiles(tempFolder, "*.jpg");

                // StringBuilder to  concatenate the text from all pages
                StringBuilder concatenatedText = new StringBuilder();

                foreach (string filePath in jpegFiles)
                {
                    Console.WriteLine("Reading image - " + filePath);
                    string imageText = await AzureOCR.ExtractTextUsingReadAPI(filePath);

                    

                    if (string.IsNullOrEmpty(imageText))
                    {
                        continue;
                    }

                    // Append extracted text to the concatenated text
                    concatenatedText.AppendLine(imageText);
                }

                //string funcaoEstagiario = "Função do estagiário: Técnico Recursos Humanos trainee";

                //concatenatedText.AppendLine(funcaoEstagiario);

                File.WriteAllText("documento.txt", concatenatedText.ToString());

                // Process concatenated text
                var results = await OpenAI.AIParseCsvTextAsync(concatenatedText.ToString());
                JObject result1 = (JObject)results["result1"];
                JObject result2 = (JObject)results["result2"];
                Console.WriteLine("Image AI result 1:");
                Console.WriteLine(Util.PrintWithoutBase64Field(result1));
                Console.WriteLine("Image AI result 2:");
                Console.WriteLine(Util.PrintWithoutBase64Field(result2));

                // Combine text from result1 and result2
                string combinedText = Util.PrintWithoutBase64Field(result1) + Util.PrintWithoutBase64Field(result2);

                // Write combined text to a text file
                string filePathTxt = @"C:\Users\Ana\Documents\textoResultJson.txt";
                File.WriteAllText(filePathTxt, combinedText);

                Console.WriteLine("Text written to: " + filePathTxt);

                // Delete temporary folder and its contents
                Directory.Delete(tempFolder, true);
            }
            catch (Exception e)
            {
                Console.WriteLine("Error processing PDF: " + e.ToString());
            }

            return jsonRes;
        }

        private static bool IsCSVFormat(string text)
        {
            // check if it contains comma-separated value (?) ask André
            return text.Contains(",") && text.Contains("\n");
        }

    }
}
