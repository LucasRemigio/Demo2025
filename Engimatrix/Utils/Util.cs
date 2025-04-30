// // Copyright (c) 2024 Engibots. All rights reserved.

using System.Data;
using System.Diagnostics;
using System.Drawing;
using System.Drawing.Imaging;
using System.Text;
using System.Text.RegularExpressions;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using engimatrix.Config;
using engimatrix.Models;
using iTextSharp.text.pdf;
using iTextSharp.text;
using iTextSharp.text.pdf.parser;

namespace engimatrix.Utils
{
    public static class Util
    {
        private const int MaxInputStringLenght = 100;
        public static Dictionary<string, string> client_address { get; set; } = new Dictionary<string, string>();

        private static readonly char[] separators = ['.', ','];

        public static bool IsValidInputLenght(string inputStr)
        {
            if (inputStr.Length > MaxInputStringLenght)
            {
                return false;
            }

            return true;
        }

        public static bool IsValidInputString(string inputStr)
        {
            if (string.IsNullOrEmpty(inputStr) || !IsValidInputLenght(inputStr))
            {
                return false;
            }

            return true;
        }

        public static bool IsValidInputEmail(string email)
        {
            if (!IsValidInputString(email))
            {
                return false;
            }

            if (email.Trim().EndsWith("."))
            {
                return false;
            }
            try
            {
                var addr = new System.Net.Mail.MailAddress(email);
                return addr.Address == email;
            }
            catch
            {
                return false;
            }
        }

        public static bool HasValidData<T>(List<T> data, string debugMessage)
        {
            if (data == null || data.Count == 0)
            {
                Log.Debug(debugMessage);
                return false;
            }
            return true;
        }

        /// <summary>
        /// Calculates the optimal chunk size for dividing a given amount into manageable chunks,
        /// ensuring the chunk size does not exceed a specified maximum.
        /// </summary>
        /// <returns>
        /// The optimal chunk size that evenly distributes the total amount across chunks
        /// without exceeding the specified maximum chunk size.
        /// </returns>
        public static int GetOptimalChunkSize(int maxChunkSize, int amountToDivide)
        {
            // Step 1: Calculate the number of chunks needed
            int numberOfChunks = (int)Math.Ceiling((double)amountToDivide / maxChunkSize);
            // Calculate how many chunks are needed if we use maxChunkSize for each chunk.
            // Example: totalProducts = 801, maxChunkSize = 400
            // numberOfChunks = Math.Ceiling(801 / 400) = 3

            // Step 2: Calculate the optimal chunk size
            int optimalChunkSize = (int)Math.Ceiling((double)amountToDivide / numberOfChunks);
            // Calculate the optimal chunk size to evenly distribute the products across the chunks.
            // Example: totalProducts = 801, numberOfChunks = 3
            // optimalChunkSize = Math.Ceiling(801 / 3) = 267

            // Step 3: Choose the smaller of maxChunkSize or optimalChunkSize
            int chunkSize = Math.Min(maxChunkSize, optimalChunkSize);
            // Ensure the chunk size does not exceed the maximum allowed chunk size.
            // Example: maxChunkSize = 400, optimalChunkSize = 267
            // chunkSize = Math.Min(400, 267) = 267

            return chunkSize;
        }

        // Extract the integer part from a decimal string
        public static string ExtractIntegerPartFromString(string input)
        {
            // Split by '.' or ',' to handle decimal numbers
            string[] parts = input.Split(separators, StringSplitOptions.RemoveEmptyEntries);

            // Try parsing the integer part before the decimal point
            if (parts.Length > 0 && int.TryParse(parts[0], out _))
            {
                return parts[0].Trim();
            }

            return null; // Return null if no valid integer part found
        }

        public static DateTime AddBusinessDays(DateTime startDate, int businessDaysToAdd)
        {
            List<DateTime> portugalHollidays = HolidaysModel.GetHolidays(DateTime.Now.Year);
            List<DateTime> portugalHollidaysNextYear = HolidaysModel.GetHolidays(DateTime.Now.Year + 1);
            portugalHollidays.Concat(portugalHollidaysNextYear);

            DateTime newDate = startDate.AddDays(businessDaysToAdd);
            while (newDate.DayOfWeek == DayOfWeek.Saturday || newDate.DayOfWeek == DayOfWeek.Sunday || portugalHollidays.Contains(newDate) || portugalHollidaysNextYear.Contains(newDate))
            {
                newDate = newDate.AddDays(1);
            }

            return newDate;
        }

        // Function to transform a plural word into singular (ex: barras => barra, varoes => varao)
        public static string Singularize(string word)
        {
            if (word.EndsWith('s'))
            {
                return word[..^1];
            }

            return word;
        }

        public static void KillAllProcess(string[] processes)
        {
            foreach (string proc in processes)
            {
                KillProcessByName(proc);
            }
        }

        private static void KillProcessByName(string processName)
        {
            foreach (var process in Process.GetProcessesByName(processName))
            {
                process.Kill();
            }
        }

        public static string ExtractReasonPart(string emailBodyContent)
        {
            const string startTag = "*Start_Reason*: ";
            const string endTag = " *End_Reason*";

            int startIndex = emailBodyContent.IndexOf(startTag, StringComparison.Ordinal);
            int endIndex = emailBodyContent.IndexOf(endTag, StringComparison.Ordinal);

            if (startIndex != -1 && endIndex != -1 && startIndex < endIndex)
            {
                return emailBodyContent.Substring(startIndex + startTag.Length, endIndex - startIndex - startTag.Length);
            }
            else
            {
                return string.Empty;
            }
        }

        public static string ReadPDF(string path)
        {
            PdfReader reader = new PdfReader(path);
            string text = string.Empty;
            for (int page = 1; page <= reader.NumberOfPages; page++)
            {
                text += PdfTextExtractor.GetTextFromPage(reader, page);
            }
            reader.Close();
            return text;
        }

        public static Boolean SavePDFPages(string path, string fileName)
        {
            PdfReader reader = new PdfReader(path);

            Document document = new Document();

            try
            {
                string[] files = Directory.GetFiles(Environment.CurrentDirectory + "/DocumentProcessing/PDFs");
                foreach (var file in files)
                {
                    File.Delete(file);
                }

                if (reader.NumberOfPages > 0)
                {
                    // Here it saves all pages as individual PDF files, instead of a single one with all pages
                    for (int i = 1; i <= reader.NumberOfPages; i++)
                    {
                        PdfCopy pdfCopyFicha = new PdfCopy(document, new FileStream(System.IO.Path.Combine(Environment.CurrentDirectory + "/DocumentProcessing/PDFs", string.Format(fileName.Replace(".pdf", "") + "_{0}.pdf", i)), FileMode.Create));
                        document.Open();
                        pdfCopyFicha.AddPage(pdfCopyFicha.GetImportedPage(reader, i));
                    }
                }
            }
            catch (System.Exception e)
            {
                Log.Info("SavePDFPages - something went wrong with the pdf file  - " + " - Detail - " + e);
            }
            finally
            {
                document.Close();
                reader.Close();
            }

            return true;
        }

        public static string ReturnValueFromPDF(string text, string prevWord, bool flag)
        {
            Boolean founded = false;
            string[] strArray = text.Split("\n");

            if (flag)
            {
                foreach (string str in strArray)
                {
                    string[] months = new string[] { "Janeiro", "Fevereiro", "Março", "Abril", "Maio", "Junho", "Julho", "Agosto", "Setembro", "Outubro", "Novembro", "Dezembro" };
                    foreach (string month in months)
                    {
                        if (str.ToLower().Contains(month.ToLower()))
                            return month;
                    }
                }
            }

            foreach (string str in strArray)
            {
                if (str.ToLower().Contains(prevWord.ToLower()))
                    foreach (string word in str.Split(" "))
                    {
                        if (founded)
                            return word;

                        if (word.ToLower() == prevWord.ToLower())
                            founded = true;
                    }
            }

            return null;
        }

        public static string ToUTF8(this string text)
        {
            return Encoding.UTF8.GetString(Encoding.Default.GetBytes(text));
        }

        public static int LevenshteinDistance(string str1, string str2)
        {
            int len1 = str1.Length;
            int len2 = str2.Length;

            int[,] dp = new int[len1 + 1, len2 + 1];

            for (int i = 0; i <= len1; i++)
                dp[i, 0] = i;

            for (int j = 0; j <= len2; j++)
                dp[0, j] = j;

            for (int i = 1; i <= len1; i++)
            {
                for (int j = 1; j <= len2; j++)
                {
                    int substitutionCost = (str1[i - 1] == str2[j - 1]) ? 0 : 1;
                    dp[i, j] = Math.Min(Math.Min(dp[i - 1, j] + 1, dp[i, j - 1] + 1), dp[i - 1, j - 1] + substitutionCost);
                }
            }

            return dp[len1, len2];
        }

        public static void ConvertJpegToPng(string jpegFilePath, string pngFilePath)
        {
            using (Bitmap jpegImage = new Bitmap(jpegFilePath))
            {
                // Cria uma imagem em branco com o mesmo tamanho da imagem JPEG
                using (Bitmap pngImage = new Bitmap(jpegImage.Width, jpegImage.Height, PixelFormat.Format32bppArgb))
                {
                    // Desenha a imagem JPEG na imagem PNG em branco
                    using (Graphics g = Graphics.FromImage(pngImage))
                    {
                        g.DrawImage(jpegImage, 0, 0, jpegImage.Width, jpegImage.Height);
                    }

                    // Salva a imagem PNG
                    pngImage.Save(pngFilePath, ImageFormat.Png);
                }
            }
        }

        public static void ConvertPngToJpeg(string pngFilePath, string jpegFilePath)
        {
            using (Bitmap pngImage = new Bitmap(pngFilePath))
            {
                // Cria uma imagem em branco com o mesmo tamanho da imagem JPEG
                using (Bitmap jpegImage = new Bitmap(pngImage.Width, pngImage.Height, PixelFormat.Format32bppArgb))
                {
                    // Desenha a imagem PNG na imagem JPEG em branco
                    using (Graphics g = Graphics.FromImage(pngImage))
                    {
                        g.DrawImage(jpegImage, 0, 0, pngImage.Width, pngImage.Height);
                    }

                    // Salva a imagem PNG
                    pngImage.Save(jpegFilePath, ImageFormat.Jpeg);
                }
            }
        }

        public static string QrCodeInvoiceConverter(string qrCodeText)
        {
            string out_str = String.Empty;
            Dictionary<string, string> dic = new Dictionary<string, string>
            {
             {"A", "NIF do emitente"}, {"B", "NIF do adquirente"}, {"C", "País do adquirente"}, {"D", "Tipo de documento"}, {"E", "Estado do documento"}, {"F", "Data do documento"}, {"G", "Identificação única do documento"}, {"H", "ATCUD"}, {"I1", "Espaço fiscal"}, {"I2", "Base tributável isenta de IVA"}, {"I3", "Base tributável de IVA à taxa reduzida"}, {"I4", "Total de IVA à taxa reduzida"}, {"I5", "Base tributável de IVA à taxa intermédia"}, {"I6", "Total de IVA à taxa intermédia"}, {"I7", "Base tributável de IVA à taxa normal"}, {"I8", "Total de IVA à taxa normal"}, {"J1", "Espaço fiscal"}, {"J2", "Base tributável isenta de IVA"}, {"J3", "Base tributável de IVA à taxa reduzida"}, {"J4", "Total de IVA à taxa reduzida"}, {"J5", "Base tributável de IVA à taxa intermédia"}, {"J6", "Total de IVA à taxa intermédia"}, {"J7", "Base tributável de IVA à taxa normal"}, {"J8", "Total de IVA à taxa normal"}, {"K1", "Espaço fiscal"}, {"K2", "Base tributável isenta de IVA"}, {"K3", "Base tributável de IVA à taxa reduzida"}, {"K4", "Total de IVA à taxa reduzida"}, {"K5", "Base tributável de IVA à taxa intermédia"}, {"K6", "Total de IVA à taxa intermédia"}, {"K7", "Base tributável de IVA à taxa normal"}, {"K8", "Total de IVA à taxa normal"}, {"L", "Não sujeito / não tributável em IVA / outras situações"}, {"M", "Imposto do Selo"}, {"N", "Total de impostos"}, {"O", "Total do documento com impostos"}, {"P", "Retenções na fonte"}, {"Q", "4 caracteres do Hash"}, {"R", "Nº do certificado"}, {"S", "Outras informações"}
            };

            foreach (string item in qrCodeText.Split("*"))
            {
                string key = item.Split(":").First();
                out_str += item.Replace(key, dic[key]) + ";";
            }
            return out_str;
        }

        public static bool SaveNewRecordInvoiceProcessed(string filePath, string invoice, string date, string due_date, string supplier_tax, string supplier_name, string supplier_contact, string supplier_address, string supplier_zip_code, string purchase_order, string amount, string vat_value, string discounts)
        {
            DataTable dt = ReadCSVFileIntoDt(filePath);

            DataRow row = dt.NewRow();
            row["invoice"] = invoice;
            row["date"] = date;
            row["due_date"] = due_date;
            row["supplier_tax"] = supplier_tax;
            row["supplier_name"] = supplier_name;
            row["supplier_contact"] = supplier_contact;
            row["supplier_address"] = supplier_address;
            row["supplier_zip_code"] = supplier_zip_code;
            row["purchase_order"] = purchase_order;
            row["amount"] = amount;
            row["vat_value"] = vat_value;
            row["discounts"] = discounts;
            dt.Rows.Add(row);

            SaveDtAsCSV(filePath, dt);

            return true;
        }

        private static DataTable ReadCSVFileIntoDt(string filePath)
        {
            DataTable tabela = new DataTable();
            try
            {
                using (StreamReader reader = new StreamReader(filePath))
                {
                    string linha = reader.ReadLine();
                    string[] colunas = linha.Split(';');
                    foreach (string coluna in colunas)
                    {
                        tabela.Columns.Add(coluna);
                    }

                    while (!reader.EndOfStream)
                    {
                        linha = reader.ReadLine();
                        string[] valores = linha.Split(';');
                        DataRow novaLinha = tabela.NewRow();
                        novaLinha.ItemArray = valores;
                        tabela.Rows.Add(novaLinha);
                    }
                }
            }
            catch (System.Exception ex)
            {
                Console.WriteLine("Erro ao ler arquivo CSV: " + ex.Message);
            }
            return tabela;
        }

        private static void SaveDtAsCSV(string filePath, DataTable dt)
        {
            try
            {
                string columns = String.Empty;
                foreach (DataColumn coluna in dt.Columns)
                    columns += ";" + coluna.ColumnName;

                using (StreamWriter writer = new StreamWriter(filePath))
                {
                    // Escreva o cabeçalho das colunas
                    writer.WriteLine(columns);

                    // Escreva cada linha de dados
                    foreach (DataRow linha in dt.Rows)
                    {
                        writer.WriteLine(string.Join(";", linha.ItemArray));
                    }
                }

                Console.WriteLine("DataTable salvo como CSV com sucesso!");
            }
            catch (System.Exception ex)
            {
                Console.WriteLine("Erro ao salvar DataTable como CSV: " + ex.Message);
            }
        }

        public static string ReplaceJsonNullValuesToEmptyString(string json)
        {
            json = Regex.Replace(json, @""":\s*""null""", @""": """"");
            return json;
        }

        public static string PrintWithoutBase64Field(JObject jsonObj)
        {
            JObject jsonResultCopy = jsonObj.DeepClone() as JObject;

            // Remove o campo base64 da cópia
            if (jsonResultCopy["base64"] != null)
            {
                jsonResultCopy.Remove("base64");
            }

            return jsonResultCopy.ToString();
        }

        public static string ImageToBase64(string imagePath)
        {
            try
            {
                byte[] imageBytes = File.ReadAllBytes(imagePath);
                string base64Image = Convert.ToBase64String(imageBytes);

                return base64Image;
            }
            catch (Exception ex)
            {
                Log.Error($"Error converting image to base64: {ex.Message}");
                return string.Empty;
            }
        }

        public static string PrintWithoutAttachment(JObject jsonObj)
        {
            JObject jsonResultCopy = jsonObj.DeepClone() as JObject;

            // Remove o campo base64 da cópia
            if (jsonResultCopy["attachments"] != null)
            {
                jsonResultCopy.Remove("attachments");
            }

            return jsonResultCopy.ToString();
        }

        public static string InjectNewHTML(string htmlContent, string contentToAdd, string htmlTag)
        {
            string pattern = @"<" + htmlTag + "[^>]*>";

            Match match = Regex.Match(htmlContent, pattern);
            if (match.Success)
            {
                // Adiciona o conteúdo após a tag <body>
                string newHtmlContent = Regex.Replace(htmlContent, pattern, match.Value + contentToAdd);
                return newHtmlContent;
            }
            else
            {
                Console.WriteLine("Tag <body> não encontrada no arquivo.");
            }
            return String.Empty;
        }

        public static string DecodeBase64(string base64String)
        {
            byte[] data = Convert.FromBase64String(base64String);

            return System.Text.Encoding.UTF8.GetString(data);
        }

        public static string EncodeBase64(string str)
        {
            byte[] bytes = System.Text.Encoding.UTF8.GetBytes(str);

            return Convert.ToBase64String(bytes);
        }

        public static void GetJsonFileChanges()
        {
            string originalFilePath = System.IO.Path.Combine(Environment.CurrentDirectory, "data_emails_v1.json");
            string newFilePath = System.IO.Path.Combine(Environment.CurrentDirectory, "data_emails_packs.json");

            // Check if the files exist
            if (File.Exists(originalFilePath) && File.Exists(newFilePath))
            {
                // Read the contents of the files
                string originalFileContent = File.ReadAllText(originalFilePath);
                string newFileContent = File.ReadAllText(newFilePath);

                // Convert the file contents to JSON objects
                List<Dictionary<string, object>> emailDataOrig = JsonConvert.DeserializeObject<List<Dictionary<string, object>>>(originalFileContent);
                List<Dictionary<string, object>> emailDataNew = JsonConvert.DeserializeObject<List<Dictionary<string, object>>>(newFileContent);
                JObject jsonOriginal = null;
                JObject jsonNew = null;
                foreach (Dictionary<string, object> item in emailDataOrig)
                {
                    foreach (KeyValuePair<string, object> i in item)
                    {
                        foreach (Dictionary<string, object> item2 in emailDataNew)
                        {
                            foreach (KeyValuePair<string, object> i2 in item2)
                            {
                                if (i.Key == i2.Key)
                                {
                                    jsonOriginal = JObject.Parse(i.Value.ToString());
                                    jsonNew = JObject.Parse(i2.Value.ToString());

                                    // Compare the JSON objects
                                    var diff = new JTokenEqualityComparer().Equals(jsonOriginal, jsonNew);

                                    if (diff)
                                    {
                                        Console.WriteLine("The JSON files are identical.");
                                    }
                                    else
                                    {
                                        Console.WriteLine("The JSON files have differences.");
                                        Console.WriteLine("Differences found:");
                                        FindDifferencesBetweenJsons(jsonOriginal, jsonNew);
                                    }

                                    break;
                                }
                            }
                        }
                    }
                }
            }
            else
            {
                Console.WriteLine("One or both files were not found.");
            }
        }

        private static void FindDifferencesBetweenJsons(JToken original, JToken newToken, string path = "")
        {
            if (!JToken.DeepEquals(original, newToken))
            {
                if (original.ToString() != newToken.ToString())
                {
                    Console.WriteLine($"- Differences found:");
                    Console.WriteLine($"    Original: {original}");
                    Console.WriteLine("-----");
                    Console.WriteLine($"    New: {newToken}");
                    return;
                }
            }

            if (original.Type == JTokenType.Object)
            {
                var originalProps = original.Children<JProperty>();
                var newProps = newToken.Children<JProperty>();

                foreach (var prop in originalProps)
                {
                    var newProp = newProps.FirstOrDefault(p => p.Name == prop.Name);
                    FindDifferencesBetweenJsons(prop.Value, newProp?.Value, $"{path}.{prop.Name}");
                }
            }
            else if (original.Type == JTokenType.Array)
            {
                var originalArray = (JArray)original;
                var newArray = (JArray)newToken;

                for (int i = 0; i < originalArray.Count || i < newArray.Count; i++)
                {
                    FindDifferencesBetweenJsons(originalArray.ElementAtOrDefault(i), newArray.ElementAtOrDefault(i), $"{path}[{i}]");
                }
            }
        }

        public static async Task<T> Retry<T>(Func<Task<T>> operation, int maxRetries = 2)
        {
            int delaySeconds = ConfigManager.TimeoutRetry();
            for (int retryCount = 0; retryCount < maxRetries; retryCount++)
            {
                try
                {
                    T result = await operation.Invoke();

                    if (result != null)
                    {
                        return result;
                    }
                }
                catch (Exception ex)
                {
                    Log.Error($"Error in operation. Retry {retryCount + 1}/{maxRetries}. Error: {ex}");
                    await Task.Delay(TimeSpan.FromSeconds(delaySeconds));
                }
            }

            Log.Error($"Max retry attempts reached.");
            return default;
        }

        public static int GetDaysLeft(DateTime date)
        {
            return (date - DateTime.UtcNow).Days;
        }

        public static string RemovingAnyOldReasonMsg(string originalText)
        {
            string startString = "<p>*Start_Reason*: ";
            string endString = " *End_Reason*</p>";

            int startIndex = originalText.IndexOf(startString);
            while (startIndex >= 0)
            {
                int endIndex = originalText.IndexOf(endString, startIndex);
                if (endIndex >= 0)
                {
                    string removedText = originalText.Substring(startIndex, endIndex - startIndex + endString.Length);
                    originalText = originalText.Replace(removedText, "");
                    startIndex = originalText.IndexOf(startString, startIndex);
                }
                else
                {
                    break;
                }
            }

            return originalText;
        }

        public static string RemovingAnyOldTrackCode(string originalText)
        {
            string startString = "<p class=\"TEXT ONLY\" style=\"width:0; overflow:hidden; float:left; display:none\">START_ORIG_MSG_ID:";
            string endString = " END_ORIG_MSG_ID </p>";

            int startIndex = originalText.IndexOf(startString);
            while (startIndex >= 0)
            {
                int endIndex = originalText.IndexOf(endString, startIndex);
                if (endIndex >= 0)
                {
                    string removedText = originalText.Substring(startIndex, endIndex - startIndex + endString.Length);
                    originalText = originalText.Replace(removedText, "");
                    startIndex = originalText.IndexOf(startString, startIndex);
                }
                else
                {
                    break;
                }
            }

            return originalText;
        }
        public static bool IsValidNif(string nif)
    {
            // Verifica se o NIF é nulo, vazio ou não possui 9 dígitos
            if (string.IsNullOrWhiteSpace(nif) || nif.Length != 9 || !nif.All(char.IsDigit)) { return false; }

            int total = 0;
            // Percorre os primeiros 8 dígitos e calcula o somatório ponderado
            for (int i = 0; i < 8; i++)
            {
                int digit = nif[i] - '0';
                total += digit * (9 - i);
            }

            // Calcula o dígito de controlo
            int remainder = total % 11;
            int checkDigit = remainder < 2 ? 0 : 11 - remainder;

            // Compara o dígito de controlo calculado com o nono dígito
            return (nif[8] - '0') == checkDigit;
        }
    }

}
