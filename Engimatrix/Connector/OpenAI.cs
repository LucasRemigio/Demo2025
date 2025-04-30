// // Copyright (c) 2024 Engibots. All rights reserved.

using System.Globalization;
using System.Text;
using Azure;
using Azure.AI.OpenAI;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using engimatrix.Config;
using engimatrix.Utils;
using System.Text.RegularExpressions;
using MimeKit;
using MailKit;
using engimatrix.ModelObjs;
using Engimatrix.ModelObjs;
using System.Runtime.CompilerServices;
using engimatrix.ModelObjs.Primavera;

namespace engimatrix.Connector;

public static class OpenAI
{
    private static int productsValidationThreshold = 20;
    private static OpenAIClient clientOpenAI = null;
    private static int MaxProductsTokens = 10_000;

    public static async void SetupAiContext()
    {
        clientOpenAI = new(new Uri(ConfigManager.endpointOpenAI), new AzureKeyCredential(ConfigManager.apiKeyOpenAI));
    }

    public static async Task<string> TestAvailability()
    {
        if (clientOpenAI == null)
        {
            SetupAiContext();
        }
        string prompt = "Hello, are you alive?";

        try
        {
            ChatCompletionsOptions chatCompletionsOptions = new()
            {
                DeploymentName = "gpt-4o-mini",
                Messages =
                {
                    new ChatRequestUserMessage(prompt),
                },
                MaxTokens = 1000,
                Temperature = 0.0f,
                NucleusSamplingFactor = 1.0f,
                FrequencyPenalty = 0,
                PresencePenalty = 0
            };

            Response<ChatCompletions> response = await clientOpenAI!.GetChatCompletionsAsync(chatCompletionsOptions);

            if (response.Value == null || response.Value.Choices.Count == 0)
            {
                return string.Empty;
            }

            string? result = response.Value.Choices[0]?.Message?.Content;
            if (string.IsNullOrEmpty(result))
            {
                return string.Empty;
            }
            return result;
        }
        catch (Exception ex)
        {
            Log.Error($"Error retrieving response from the OpenAI API: {ex.Message}");
            return string.Empty;
        }
    }

    public static async Task<string> AIGenerateResponseGivenEmail(string emailData)
    {
        if (clientOpenAI == null)
        {
            SetupAiContext();
        }

        emailData = EmailHelper.RemoveAllHtmlTags(emailData);

        // Read Json struct file
        string json = File.ReadAllText("Config/JsonGenerateResponse.dat");

        // Read Json struct file
        string rules = File.ReadAllText("Config/RulesGenerateResponse.dat");

        string systemMessage = "És o funcionário de uma empresa que vende ferro e que vai responder a um email de um cliente, de seguida vais receber a estrutura de JSON a devolver, depois as regras e de seguida o email. São agora " + DateTime.Now.ToString("HH:mm");

        if (emailData.Length > 7500)
        {
            emailData = emailData.Substring(0, 7500);
        }

        try
        {
            var chatCompletionsOptions = new ChatCompletionsOptions
            {
                DeploymentName = "gpt-4o-mini",
                Messages =
            {
            new ChatRequestSystemMessage(systemMessage),
            new ChatRequestUserMessage($"Regras: {rules}"),
            new ChatRequestSystemMessage($"Json: {json}"),
            new ChatRequestUserMessage($"Email : {emailData}"),
            },
                MaxTokens = 3000,
                Temperature = 0.0f,
                NucleusSamplingFactor = 1.0f,
                FrequencyPenalty = 0,
                PresencePenalty = 0
            };

            var response = await clientOpenAI.GetChatCompletionsAsync(chatCompletionsOptions);

            if (response.Value == null || response.Value.Choices.Count <= 0)
            {
                return string.Empty;
            }

            string result = response.Value.Choices[0]?.Message?.Content;

            Log.Debug("Resposta por IA " + result);

            result = ExtractOnlyJSONAndReplaceNullWithEmptyString(result);

            JObject parsedResult = JObject.Parse(result);
            string resposta = parsedResult["resposta"].ToString();

            // OpenAI puts [Seu nome] at the end, so we want to remove it
            resposta = Regex.Replace(resposta, @"\[.*?\]", "");
            // Also remove the trailing \r\n at the end of the message
            resposta = resposta.TrimEnd('\r', '\n');

            return resposta;
        }
        catch (System.Exception ex)
        {
            Log.Error($"Error retrieving response from the OpenAI API: {ex.Message}");
        }

        return string.Empty;
    }

    public static async Task<OpenAiEmailCategorization?> ProcessMoveEmailCategoryOpenAI(MimeMessage message)
    {
        // Extract the email body, it being plain or html. If html, clean it
        string emailBody = message.TextBody;
        if (string.IsNullOrEmpty(emailBody))
        {
            emailBody = message.HtmlBody;
            if (string.IsNullOrEmpty(emailBody))
            {
                // No message to read, just return
                Log.Error("OpenAI ProcessMoveEmailCategory: The email has no body!");
                return null;
            }
            emailBody = EmailHelper.RemoveAllHtmlTags(emailBody);
        }

        // Append the subject to the email body
        string subject = message.Subject ?? "No Subject";
        emailBody = $"Subject: {subject}\n\n{emailBody}";

        if (clientOpenAI == null)
        {
            SetupAiContext();
        }

        string rulesFileName = "RulesMoveEmailCategory.dat";
        string rules = File.ReadAllText($"Config/{rulesFileName}");
        string json = File.ReadAllText("Config/JsonMoveEmailCategory.dat");

        string systemMessage = "És um analisador de emails. De seguida, vais receber as regras a seguir a estrutura Json e o conteudo do e-mail e o respetivo assunto.";

        if (emailBody.Length > 7500)
        {
            emailBody = emailBody.Substring(0, 7500);
        }

        try
        {
            var chatCompletionsOptions = new ChatCompletionsOptions
            {
                DeploymentName = "gpt-4o-mini",
                Messages =
            {
            new ChatRequestSystemMessage(systemMessage),
            new ChatRequestUserMessage($"Regras: {rules}"),
            new ChatRequestSystemMessage($"Json: {json}"),
            new ChatRequestUserMessage($"Email : {emailBody}"),
            },
                MaxTokens = 1000,
                Temperature = 0.0f,
                NucleusSamplingFactor = 1.0f,
                FrequencyPenalty = 0,
                PresencePenalty = 0
            };

            Response<ChatCompletions> response = await clientOpenAI!.GetChatCompletionsAsync(chatCompletionsOptions);

            if (response.Value == null || response.Value.Choices.Count <= 0)
            {
                Log.Debug("ProcessMoveEmailCategoryOpenAI: No response was given");
                return null;
            }

            string? result = response.Value.Choices[0]?.Message?.Content;

            if (result == null || string.IsNullOrEmpty(result))
            {
                Log.Debug("ProcessMoveEmailCategoryOpenAI: Response given is invalid");
                return null;
            }

            Log.Debug("Categorização por IA " + result);

            result = ExtractOnlyJSONAndReplaceNullWithEmptyString(result);

            JObject parsedResult = JObject.Parse(result);
            string confianca = parsedResult["confianca"]?.ToString() ?? "0";
            string category = parsedResult["categoria"]?.ToString() ?? string.Empty;
            string originalSender = parsedResult["email_remetente"]?.ToString() ?? string.Empty;
            string fwdConfianca = parsedResult["confianca_reencaminhamento"]?.ToString() ?? "0";
            string categoryJustification = parsedResult["justificacao"]?.ToString() ?? string.Empty;


            if (string.IsNullOrEmpty(confianca))
            {
                confianca = "0";
            }
            if (string.IsNullOrEmpty(category))
            {
                category = CategoryConstants.CategoryTitle.ERRO;
            }
            /* Chamada á função
             * ProcessReleasePrompt
             * message, result, json, rulesFileName, systemMessage, emailUId, emailBox, account
             */

            OpenAiEmailCategorization openAiEmailCategorization = new OpenAiEmailCategorizationBuilder()
               .SetConfianca(confianca)
               .SetCategoria(category)
               .SetJustificacao(categoryJustification)
               .SetEmailRemetente(originalSender)
               .SetConfiancaReencaminhamento(fwdConfianca)
               .Build();

            return openAiEmailCategorization;
        }
        catch (System.Exception ex)
        {
            Log.Error($"Error retrieving response from the OpenAI API: {ex}");
        }

        return null;
    }

    public static async Task<CheckClientAdjudicated?> CheckIfClientAdjudicated(string emailBody)
    {
        if (clientOpenAI == null) { SetupAiContext(); }

        if (emailBody.Length > 7500)
        {
            emailBody = emailBody[..7500];
        }

        try
        {
            string systemMessage = "És um analisador de emails. De seguida, vais receber as regras a seguir, a estrutura Json a ser devolvida, e o conteudo do e-mail";

            string rules = File.ReadAllText("Config/RulesCheckClientAdjudicated.dat");
            string json = File.ReadAllText("Config/JsonCheckClientAdjudicated.dat");

            ChatCompletionsOptions chatCompletionsOptions = new()
            {
                DeploymentName = "gpt-4o-mini",
                Messages =
                {
                    new ChatRequestSystemMessage(systemMessage),
                    new ChatRequestUserMessage($"Regras: {rules}"),
                    new ChatRequestSystemMessage($"Json: {json}"),
                    new ChatRequestUserMessage($"Email : {emailBody}"),
                },
                MaxTokens = 4000,
                Temperature = 0.0f,
                NucleusSamplingFactor = 1.0f,
                FrequencyPenalty = 0,
                PresencePenalty = 0
            };

            Response<ChatCompletions> response = await clientOpenAI!.GetChatCompletionsAsync(chatCompletionsOptions);

            if (response.Value == null || response.Value.Choices.Count == 0)
            {
                Log.Debug("No response from OpenAI");
                return null;
            }

            string? result = response.Value.Choices[0]?.Message?.Content;
            if (string.IsNullOrEmpty(result))
            {
                Log.Debug("No content on response message from OpenAI");
                return null;
            }

            result = ExtractOnlyJSONAndReplaceNullWithEmptyString(result);

            JObject parsedResult = JObject.Parse(result);
            CheckClientAdjudicated clientAdjudicated = parsedResult.ToObject<CheckClientAdjudicated>();

            return clientAdjudicated;
        }
        catch (Exception ex)
        {
            Log.Error($"Error retrieving response from the OpenAI API: {ex.Message}");
            return null;
        }
    }

    public static async Task<List<ProdutoRaw>> ExtractEntitiesFromEmail(string emailBody)
    {
        if (clientOpenAI == null)
        {
            SetupAiContext();
        }

        try
        {
            string basePath = Path.Combine("Config", "ExtractEntitiesFromEmail");
            string rulesPath = Path.Combine(basePath, "rules.dat");
            string jsonPath = Path.Combine(basePath, "json.dat");

            string rules = File.ReadAllText(rulesPath);
            string json = File.ReadAllText(jsonPath);

            string systemMessage = "És um analisador de emails. De seguida, vais receber as regras a seguir, a estrutura Json a ser devolvida, e o conteudo do e-mail";
            emailBody = EmailHelper.RemoveAllHtmlTags(emailBody);

            if (emailBody.Length > 7500)
            {
                emailBody = emailBody.Substring(0, 7500);
            }

            ChatCompletionsOptions chatCompletionsOptions = new()
            {
                DeploymentName = "gpt-4o-mini",
                Messages =
                {
                    new ChatRequestSystemMessage(systemMessage),
                    new ChatRequestUserMessage($"Regras: {rules}"),
                    new ChatRequestSystemMessage($"Json: {json}"),
                    new ChatRequestUserMessage($"Email : {emailBody}"),
                },
                MaxTokens = MaxProductsTokens,
                Temperature = 0.0f,
                NucleusSamplingFactor = 1.0f,
                FrequencyPenalty = 0,
                PresencePenalty = 0
            };

            Response<ChatCompletions> response = await clientOpenAI!.GetChatCompletionsAsync(chatCompletionsOptions);

            if (response.Value == null || response.Value.Choices.Count == 0)
            {
                Log.Debug("No response from OpenAI");
                return [];
            }

            string? result = response.Value.Choices[0]?.Message?.Content;
            if (string.IsNullOrEmpty(result))
            {
                Log.Debug("No content on response message from OpenAI");
                return [];
            }

            result = ExtractOnlyJSONAndReplaceNullWithEmptyString(result);

            Log.Debug(result);

            JObject parsedResult = JObject.Parse(result);
            List<ProdutoRaw> produtos = parsedResult["produtos"].ToObject<List<ProdutoRaw>>();

            // Assign unique IDs to each product
            for (int i = 0; i < produtos.Count; i++)
            {
                produtos[i].Id = i + 1;
            }

            return produtos;
        }
        catch (FileNotFoundException ex)
        {
            Log.Error($"Configuration file not found in ExtractEntitiesFromEmail: {ex.Message}");
            return [];
        }
        catch (JsonException ex)
        {
            Log.Error($"Error parsing JSON response in ExtractEntitiesFromEmail: {ex.Message}");
            return [];
        }
        catch (Exception ex)
        {
            Log.Error($"Unexpected error in ExtractEntitiesFromEmail: {ex.Message}");
            return [];
        }
    }

    public static string FormatProductNamesForPrompt(List<ProductTypeItem> possibleTypes)
    {
        StringBuilder possibleTypesFormatted = new();
        foreach (ProductTypeItem type in possibleTypes)
        {
            possibleTypesFormatted.AppendLine($"- {type.name}");
        }

        return possibleTypesFormatted.ToString();
    }

    public static async Task<List<ProdutoComTipo>> GetProductsTypes(List<ProdutoRaw> produtosRaw)
    {
        if (clientOpenAI == null)
        {
            SetupAiContext();
        }

        try
        {
            string basePath = Path.Combine("Config", "GetProductsTypes");
            string rulesPath = Path.Combine(basePath, "rules.dat");
            string jsonPath = Path.Combine(basePath, "json.dat");
            string designationsPath = Path.Combine(basePath, "designations.dat");

            string rules = File.ReadAllText(rulesPath);
            string json = File.ReadAllText(jsonPath);
            string designations = File.ReadAllText(designationsPath);

            List<ProductTypeItem> possibleTypes = Models.ProductTypeModel.GetUsedProductTypes("System");
            possibleTypes.RemoveAll(possibleType => possibleType.id == 18);

            string possibleTypesNames = FormatProductNamesForPrompt(possibleTypes);

            rules += possibleTypesNames;
            // TODO: IN THE FUTURE, HAVE A TABLE OF POSSIBLE DESIGNATIONS ASSOCIATED TO EACH TYPE TO DINAMICALLY ADD TO THIS PROMPT
            rules += designations;

            string systemMessage = "És um analisador de produtos. De seguida, vais receber as regras a seguir, a estrutura Json a ser devolvida e os produtos extraidos de um pedido.";

            string produtosJson = JsonConvert.SerializeObject(produtosRaw);

            ChatCompletionsOptions chatCompletionsOptions = new()
            {
                DeploymentName = "gpt-4o-mini",
                Messages =
                {
                    new ChatRequestSystemMessage(systemMessage),
                    new ChatRequestUserMessage($"Regras: {rules}"),
                    new ChatRequestSystemMessage($"Json: {json}"),
                    new ChatRequestUserMessage($"Produtos : {produtosJson}"),
                },
                MaxTokens = MaxProductsTokens,
                Temperature = 0.0f,
                NucleusSamplingFactor = 1.0f,
                FrequencyPenalty = 0,
                PresencePenalty = 0
            };

            Response<ChatCompletions> response = await clientOpenAI.GetChatCompletionsAsync(chatCompletionsOptions);

            if (response.Value == null || response.Value.Choices.Count == 0)
            {
                return [];
            }

            string result = response.Value.Choices[0]?.Message?.Content;
            result = ExtractOnlyJSONAndReplaceNullWithEmptyString(result);

            Log.Debug(result);

            JObject parsedResult = JObject.Parse(result);
            List<ProdutoComTipo> produtos = parsedResult["produtos"].ToObject<List<ProdutoComTipo>>();

            // match the tipo de produto identified and add the corresponding id
            foreach (ProdutoComTipo produto in produtos)
            {
                ProductTypeItem? correspondingProductType = possibleTypes.FirstOrDefault(pt => pt.name.Equals(produto.TipoProduto, StringComparison.OrdinalIgnoreCase));
                if (correspondingProductType == null)
                {
                    Log.Error("Corresponding product type not found!");
                    continue;
                }
                produto.TipoProdutoId = correspondingProductType.id;
            }

            return produtos;
        }
        catch (FileNotFoundException ex)
        {
            Log.Error($"Configuration file not found in ExtractEntitiesFromEmail: {ex.Message}");
            return [];
        }
        catch (JsonException ex)
        {
            Log.Error($"Error parsing JSON response in ExtractEntitiesFromEmail: {ex.Message}");
            return [];
        }
        catch (Exception ex)
        {
            Log.Error($"Unexpected error in ExtractEntitiesFromEmail: {ex.Message}");
            return [];
        }
    }

    public static string GetPromptByProductType(List<int> productTypeIds)
    {
        if (productTypeIds == null || productTypeIds.Count == 0)
        {
            return string.Empty; // Return early for empty input
        }

        StringBuilder fullPromptBuilder = new();

        List<ProductTypePropertiesItem> productTypes = Models.ProductTypeModel.GetAllPropertiesAssociatedWithProductType(productTypeIds, "System");
        List<int> uniqueTypeIds = productTypeIds.Distinct().ToList();

        foreach (int uniqueTypeId in uniqueTypeIds)
        {
            ProductTypePropertiesItem? productType = productTypes.FirstOrDefault(pt => pt.id == uniqueTypeId);
            if (productType == null)
            {
                Log.Error($"Product type with id '{uniqueTypeId}' not found in GetPromptByProductType");
                continue;
            }

            string productTypePrompt = GeneratePromptForProductType(productType);

            fullPromptBuilder.AppendLine(productTypePrompt);
        }

        return fullPromptBuilder.ToString();
    }

    private static string GeneratePromptForProductType(ProductTypePropertiesItem productType)
    {
        StringBuilder promptBuilder = new();

        promptBuilder.AppendLine($"----- {productType.name.ToUpper()} -----");
        promptBuilder.AppendLine($"O produto {productType.name} pode ter apenas as seguintes características:");

        // Map characteristics to labels
        Dictionary<string, string> characteristics = new()
        {
            { "Materiais", productType.materials },
            { "Finalizações", productType.finishings },
            { "Formas", productType.shapes },
            { "Superfícies", productType.surfaces }
        };

        // Append characteristics dynamically
        foreach ((string label, string value) in characteristics)
        {
            AppendCharacteristic(promptBuilder, label, value);
        }

        string basePath = Path.Combine("Config", "Products");
        string product = RemoveDiacritics(productType.name).ToLower(CultureInfo.InvariantCulture).Trim();
        string filePath = Path.Combine(basePath, $"{product}.dat");

        if (!File.Exists(filePath))
        {
            Log.Error($"Warning: File for product type name '{productType.name}' not found at '{Path.GetFullPath(filePath)}'.");
            return promptBuilder.ToString();
        }

        promptBuilder.AppendLine(File.ReadAllText(filePath));

        return promptBuilder.ToString();
    }

    private static void AppendCharacteristic(StringBuilder builder, string label, string value)
    {
        if (string.IsNullOrEmpty(value))
        {
            return;
        }

        builder.AppendLine($"- {label}: {value}");
    }

    public static async Task<List<Produto>> ClassifyProducts(List<ProdutoRaw> produtosRaw)
    {
        if (clientOpenAI == null)
        {
            SetupAiContext();
        }

        try
        {
            string basePath = Path.Combine("Config", "ClassifyProducts");
            string rulesPath = Path.Combine(basePath, "rules.dat");
            string jsonPath = Path.Combine(basePath, "json.dat");

            string rules = File.ReadAllText(rulesPath);
            string json = File.ReadAllText(jsonPath);

            string systemMessage = "És um analisador de produtos. De seguida, vais receber as regras a seguir, a estrutura Json a ser devolvida e os produtos extraidos de um pedido.";

            // now add to the prompt the informations about each product, given the types of products extracted
            string productDetailsPrompt = GetPromptByProductType(produtosRaw.Select(p => p.TipoProdutoId).ToList());
            if (string.IsNullOrEmpty(productDetailsPrompt))
            {
                Log.Error("No product types found in ClassifyProducts");
                return [];
            }

            rules += productDetailsPrompt;

            string produtosJson = JsonConvert.SerializeObject(produtosRaw);

            ChatCompletionsOptions chatCompletionsOptions = new()
            {
                DeploymentName = "gpt-4o-mini",
                Messages =
                {
                    new ChatRequestSystemMessage(systemMessage),
                    new ChatRequestUserMessage($"Regras: {rules}"),
                    new ChatRequestSystemMessage($"Json: {json}"),
                    new ChatRequestUserMessage($"Produtos : {produtosJson}"),
                },
                MaxTokens = MaxProductsTokens,
                Temperature = 0.0f,
                NucleusSamplingFactor = 1.0f,
                FrequencyPenalty = 0,
                PresencePenalty = 0
            };

            Response<ChatCompletions> response = await clientOpenAI.GetChatCompletionsAsync(chatCompletionsOptions);

            if (response.Value == null || response.Value.Choices.Count == 0)
            {
                return new List<Produto>();
            }

            string result = response.Value.Choices[0]?.Message?.Content;
            result = ExtractOnlyJSONAndReplaceNullWithEmptyString(result);

            Log.Debug(result);

            JObject parsedResult = JObject.Parse(result);
            List<Produto> produtos = parsedResult["produtos"].ToObject<List<Produto>>();

            // Assign unique IDs to each product
            for (int i = 0; i < produtos.Count; i++)
            {
                produtos[i].Id = i + 1;
            }

            return produtos;
        }
        catch (FileNotFoundException ex)
        {
            Log.Error($"Configuration file not found in ExtractEntitiesFromEmail: {ex.Message}");
            return [];
        }
        catch (JsonException ex)
        {
            Log.Error($"Error parsing JSON response in ExtractEntitiesFromEmail: {ex.Message}");
            return [];
        }
        catch (Exception ex)
        {
            Log.Error($"Unexpected error in ExtractEntitiesFromEmail: {ex.Message}");
            return [];
        }
    }

    public static string GetProductMaterialName(string material)
    {
        // Include material only if it is not "Ferro" or "Aço"
        if (string.IsNullOrEmpty(material))
        {
            return string.Empty;
        }

        if (material.Equals("Ferro", StringComparison.OrdinalIgnoreCase) ||
             material.Equals("Aço", StringComparison.OrdinalIgnoreCase))
        {
            return string.Empty;
        }

        return material;
    }

    public static string GetFormatedProductForPrompt(Produto produto, bool showDetails)
    {
        string productInfo = $"(id) {produto.Id}: {produto.produtoSolicitado} ({produto.TraducaoNomeProduto})";

        if (!showDetails)
        {
            productInfo += $" / {produto.Quantidade} {produto.UnidadeQuantidade}";
            return productInfo;
        }

        string caracteristics = string.Join(" ", new[]
           {
            produto.CaracteristicasProduto.TipoDeProduto,
            GetProductMaterialName(produto.CaracteristicasProduto.TipoMaterial),
            produto.CaracteristicasProduto.FormaProduto,
            produto.CaracteristicasProduto.FinalizacaoProduto,
            produto.CaracteristicasProduto.SuperficieProduto
        }.Where(c => !string.IsNullOrEmpty(c)));

        string dimensions = string.Join(" | ", new[]
            {
            !string.IsNullOrEmpty(produto.CaracteristicasProduto.Dimensoes.Comprimento) ? $"Comprimento: {produto.CaracteristicasProduto.Dimensoes.Comprimento}" : null,
            !string.IsNullOrEmpty(produto.CaracteristicasProduto.Dimensoes.Largura) ? $"Largura: {produto.CaracteristicasProduto.Dimensoes.Largura}" : null,
            !string.IsNullOrEmpty(produto.CaracteristicasProduto.Dimensoes.Altura) ? $"Altura: {produto.CaracteristicasProduto.Dimensoes.Altura}" : null,
            !string.IsNullOrEmpty(produto.CaracteristicasProduto.Dimensoes.Diametro) ? $"Diâmetro: {produto.CaracteristicasProduto.Dimensoes.Diametro}" : null
        }.Where(d => d != null));

        string formattedCaracteristics = $"{productInfo} ({caracteristics})";
        string formattedDimensions = $"Medidas: {produto.MedidasProduto} ({dimensions}) / {produto.Quantidade} {produto.UnidadeQuantidade}";

        return $"- {formattedCaracteristics} / {formattedDimensions}";
    }

    public static string GetFormatedCatalogProductForPrompt(ProductCatalogDTO product, bool showDetails)
    {
        string productInfo = $"{product.id}: {product.description_full} ";

        // build the measurements of the product, by adding a x as a separator between the values\
        //string dimensions = string.Join(" x ", new[]
        //    {
        //        product.length.ToString(),
        //        product.width.ToString(),
        //        product.height.ToString(),
        //        product.thickness.ToString(),
        //        product.diameter.ToString()
        //    }.Where(d => d != "0"));

        //productInfo += !string.IsNullOrEmpty(dimensions) ? $"{dimensions}" : string.Empty;

        if (!showDetails)
        {
            return productInfo;
        }

        string caracteristics = string.Join(" ", new[]
           {
                product.type?.name,
                GetProductMaterialName(product.material?.name),
                product.shape?.name,
                product.finishing?.name,
                product.surface?.name
            }.Where(c => !string.IsNullOrEmpty(c)));

        productInfo += $" ({caracteristics})";
        return productInfo;
    }

    public static string GetPromptForProductMatchValidation(string rules, string json, List<Produto> intendedProducts, List<ProductCatalogDTO> comparisonProducts)
    {
        StringBuilder openAiPrompt = new(rules);

        openAiPrompt.AppendLine("Produto solicitado / Medida solicitada (se existentes, adicionar-se-ão detalhes como Comprimento, Largura, Altura e Diametro) / Quantidade solicitada e unidade");

        foreach (Produto produto in intendedProducts)
        {
            openAiPrompt.AppendLine(GetFormatedProductForPrompt(produto, true));
        }

        openAiPrompt.AppendLine("\n\n\nEstes são os produtos disponíveis na nossa base de dados, existentes no catálogo:");
        openAiPrompt.AppendLine("Id do produto / Produto e medida");

        foreach (ProductCatalogDTO catalogProduct in comparisonProducts)
        {
            openAiPrompt.AppendLine(GetFormatedCatalogProductForPrompt(catalogProduct, true));
        }

        openAiPrompt.AppendLine("\n\nPreencha o JSON abaixo com as correspondências feitas.");
        openAiPrompt.Append(json);

        return openAiPrompt.ToString();
    }

    public static async Task<List<ProdutoComparado>> ValidateProductMatches(List<Produto> produtos, List<ProductCatalogDTO> allMostSimilarProductCatalogList)
    {
        if (clientOpenAI == null)
        {
            SetupAiContext();
        }

        try
        {
            string basePath = Path.Combine("Config", "ValidateProductMatches");
            string rulesPath = Path.Combine(basePath, "rules.dat");
            string jsonPath = Path.Combine(basePath, "json.dat");
            string finalRemarksPath = Path.Combine("Config", "PromptRevalidateFinalObservation.dat");

            // Read the files
            string rules = File.ReadAllText(rulesPath);
            string json = File.ReadAllText(jsonPath);
            string finalRemarks = File.ReadAllText(finalRemarksPath);

            string openAiPrompt = GetPromptForProductMatchValidation(rules, json, produtos, allMostSimilarProductCatalogList);
            openAiPrompt += finalRemarks;

            ChatCompletionsOptions validationOptions = new()
            {
                DeploymentName = "gpt-4o-mini",
                Messages = { new ChatRequestUserMessage(openAiPrompt) },
                // This one does not need as much tokens as the others, because this call is made in chunks
                MaxTokens = 4000,
                Temperature = 0.0f,
                NucleusSamplingFactor = 1.0f,
                FrequencyPenalty = 0,
                PresencePenalty = 0
            };

            Response<ChatCompletions> validationResponse = await clientOpenAI.GetChatCompletionsAsync(validationOptions);

            if (validationResponse.Value == null || validationResponse.Value.Choices.Count <= 0)
            {
                Log.Debug("Error retrieving OpenAI Product matching response");
                return [];
            }

            string validationResult = validationResponse.Value.Choices[0]?.Message?.Content;
            validationResult = ExtractOnlyJSONAndReplaceNullWithEmptyString(validationResult);

            Log.Debug(validationResult);

            JObject matchResult = JObject.Parse(validationResult);
            List<ProdutoComparado> matchedProductsOpenAI = matchResult["produtos"].ToObject<List<ProdutoComparado>>();

            return matchedProductsOpenAI;
        }
        catch (FileNotFoundException ex)
        {
            Log.Error($"Configuration file not found: {ex.Message}");
            return [];
        }
        catch (JsonException ex)
        {
            Log.Error($"Error parsing JSON response for product matching: {ex.Message}");
            return [];
        }
        catch (Exception ex)
        {
            Log.Error($"Unexpected error in ValidateProductMatches: {ex.Message}");
            Log.Error(ex.ToString());
            return [];
        }
    }

    public static async Task<Cliente?> IdentifyClientFromEmail(FilteredEmail filteredEmail)
    {
        if (clientOpenAI == null)
        {
            SetupAiContext();
        }

        try
        {
            string basePath = Path.Combine("Config", "ExtractClientInfo");
            string rulesPath = Path.Combine(basePath, "rules.dat");
            string jsonPath = Path.Combine(basePath, "json.dat");
            string finalRemarksPath = Path.Combine("Config", "PromptRevalidateFinalObservation.dat");

            // Read the files
            string rules = File.ReadAllText(rulesPath);
            string json = File.ReadAllText(jsonPath);
            string finalRemarks = File.ReadAllText(finalRemarksPath);

            string openAiPrompt = rules;

            openAiPrompt += "\n\n\n## EMAIL ##\n\n";
            openAiPrompt += "Remetente: " + filteredEmail.email.from + "\n\n";
            openAiPrompt += "Email: " + filteredEmail.email.body + "\n\n\n";

            openAiPrompt += finalRemarks;

            openAiPrompt += json;

            ChatCompletionsOptions validationOptions = new()
            {
                DeploymentName = "gpt-4o-mini",
                Messages = { new ChatRequestUserMessage(openAiPrompt) },
                MaxTokens = 3000,
                Temperature = 0.0f,
                NucleusSamplingFactor = 1.0f,
                FrequencyPenalty = 0,
                PresencePenalty = 0
            };

            Response<ChatCompletions> openAiResponse = await clientOpenAI!.GetChatCompletionsAsync(validationOptions);

            if (openAiResponse.Value == null || openAiResponse.Value.Choices.Count <= 0)
            {
                Log.Debug("Error retrieving OpenAI Product matching response");
                return null;
            }

            string? resultContent = openAiResponse.Value.Choices[0]?.Message?.Content;
            if (string.IsNullOrEmpty(resultContent))
            {
                Log.Debug(" retrieving OpenAI Product matching response");
                return null;
            }

            resultContent = ExtractOnlyJSONAndReplaceNullWithEmptyString(resultContent);

            JObject result = JObject.Parse(resultContent);
            Cliente cliente = result.ToObject<Cliente>();

            return cliente;
        }
        catch (FileNotFoundException ex)
        {
            Log.Error($"Configuration file not found: {ex.Message}");
            return null;
        }
        catch (JsonException ex)
        {
            Log.Error($"Error parsing JSON response for product matching: {ex.Message}");
            return null;
        }
        catch (Exception ex)
        {
            Log.Error($"Unexpected error in ValidateProductMatches: {ex.Message}");
            return null;
        }
    }
    public static string FormatClientsInfo(Cliente client, List<MFPrimaveraClientItem> closesClients)
    {
        StringBuilder clientsInfo = new();

        // Format main client information
        clientsInfo.AppendLine("## CLIENTE ##");

        if (!string.IsNullOrEmpty(client.NomeCliente)) { clientsInfo.AppendLine($"Nome Cliente: {client.NomeCliente}"); }
        if (!string.IsNullOrEmpty(client.NomeEmpresa)) { clientsInfo.AppendLine($"Nome Empresa: {client.NomeEmpresa}"); }
        if (!string.IsNullOrEmpty(client.Email)) { clientsInfo.AppendLine($"Email: {client.Email}"); }

        clientsInfo.AppendLine("\n## CLIENTES MAIS PRÓXIMOS ##");

        // Format closest client information
        foreach (MFPrimaveraClientItem closeClient in closesClients)
        {
            if (!string.IsNullOrEmpty(closeClient.Cliente)) { clientsInfo.AppendLine($"Cliente: {closeClient.Cliente}"); }
            if (!string.IsNullOrEmpty(closeClient.Nome)) { clientsInfo.AppendLine($"Nome: {closeClient.Nome}"); }
            if (!string.IsNullOrEmpty(closeClient.Email)) { clientsInfo.AppendLine($"Email: {closeClient.Email}"); }

            clientsInfo.AppendLine();
        }

        return clientsInfo.ToString();
    }

    public static async Task<OpenAiPrimaveraClient?> MatchClientWithClosest(Cliente client, List<MFPrimaveraClientItem> closestClients)
    {
        if (clientOpenAI == null)
        {
            SetupAiContext();
        }

        try
        {
            string basePath = Path.Combine("Config", "MatchClient");
            string rulesPath = Path.Combine(basePath, "rules.dat");
            string jsonPath = Path.Combine(basePath, "json.dat");
            string finalRemarksPath = Path.Combine("Config", "PromptRevalidateFinalObservation.dat");

            // Read the files
            string rules = File.ReadAllText(rulesPath);
            string json = File.ReadAllText(jsonPath);
            string finalRemarks = File.ReadAllText(finalRemarksPath);

            string openAiPrompt = rules;

            openAiPrompt += FormatClientsInfo(client, closestClients);
            openAiPrompt += finalRemarks;
            openAiPrompt += json;

            ChatCompletionsOptions validationOptions = new()
            {
                DeploymentName = "gpt-4o-mini",
                Messages = { new ChatRequestUserMessage(openAiPrompt) },
                MaxTokens = 3000,
                Temperature = 0.0f,
                NucleusSamplingFactor = 1.0f,
                FrequencyPenalty = 0,
                PresencePenalty = 0
            };

            Response<ChatCompletions> openAiResponse = await clientOpenAI!.GetChatCompletionsAsync(validationOptions);

            if (openAiResponse.Value == null || openAiResponse.Value.Choices.Count <= 0)
            {
                Log.Debug("Error retrieving OpenAI Product matching response");
                return null;
            }

            string? resultContent = openAiResponse.Value.Choices[0]?.Message?.Content;
            if (string.IsNullOrEmpty(resultContent))
            {
                Log.Debug("Failed retrieving OpenAI Product matching response");
                return null;
            }

            resultContent = ExtractOnlyJSONAndReplaceNullWithEmptyString(resultContent);

            JObject result = JObject.Parse(resultContent);
            OpenAiPrimaveraClient cliente = result.ToObject<OpenAiPrimaveraClient>();

            if (cliente.Confianca < 90)
            {
                Log.Debug("Not enough confidence on the client matching");
                return null;
            }

            return cliente;

        }
        catch (FileNotFoundException ex)
        {
            Log.Error($"Configuration file not found: {ex.Message}");
            return null;
        }
        catch (JsonException ex)
        {
            Log.Error($"Error parsing JSON response for product matching: {ex.Message}");
            return null;
        }
        catch (Exception ex)
        {
            Log.Error($"Unexpected error in ValidateProductMatches: {ex.Message}");
            return null;
        }
    }

    public static async Task<bool> ProcessReleasePrompt(MimeMessage message, string result, string json, string rulesFileName, string systemMessage, string account)
    {
        return await ProcessReleasePrompt(message, result, json, rulesFileName, systemMessage, default(MailKit.UniqueId), null, account);
    }

    public static async Task<bool> ProcessReleasePrompt(MimeMessage message, string result, string json, string rulesFileName, string systemMessage, UniqueId emailUId, string emailBox, string account)
    {
        string emailBody = message.TextBody;

        string basePath = "Config/";
        string rcFileName = rulesFileName.Replace(".dat", "_rc.dat");
        string rcFilePath = Path.Combine(basePath, rcFileName);

        if (!File.Exists(rcFilePath))
        {
            return false;
        }

        if (clientOpenAI == null)
        {
            SetupAiContext();
        }

        string rules = File.ReadAllText(rcFilePath);

        emailBody = EmailHelper.RemoveAllHtmlTags(emailBody);

        if (emailBody.Length > 7500)
        {
            emailBody = emailBody.Substring(0, 7500);
        }

        try
        {
            var chatCompletionsOptions = new ChatCompletionsOptions
            {
                DeploymentName = "gpt-35-turbo",
                Messages =
                    {
                         new ChatRequestSystemMessage(systemMessage),
                         new ChatRequestSystemMessage(rules),
                         new ChatRequestSystemMessage(json),
                         new ChatRequestUserMessage(emailBody)
                    },
                MaxTokens = 1000,
                Temperature = 0.0f,
                NucleusSamplingFactor = 1.0f,
                FrequencyPenalty = 0,
                PresencePenalty = 0
            };

            var response = await clientOpenAI.GetChatCompletionsAsync(chatCompletionsOptions);

            if (response.Value != null && response.Value.Choices.Count > 0)
            {
                string result_release = response.Value.Choices[0]?.Message?.Content;

                result_release = ExtractOnlyJSONAndReplaceNullWithEmptyString(result_release);

                // Parse category previous result
                JObject parsedResult = JObject.Parse(result);
                string category = parsedResult["categoria"].ToString();

                // Parse category release result
                JObject parsedRelease = JObject.Parse(result_release);
                string categoryRelease = parsedResult["categoria"].ToString();

                if (category != categoryRelease)
                {
                    // Paths for the email and attachments to be saved to
                    string emailIdFolder = Guid.NewGuid().ToString();
                    string uniqueEmailFolder = Path.Combine("DifferentEmails", DateTime.Now.ToString("yyyy-MM-dd"), Path.GetFileNameWithoutExtension(rcFilePath));
                    Directory.CreateDirectory(uniqueEmailFolder);
                    string emlLink = Path.Combine(uniqueEmailFolder, emailIdFolder, $"{emailUId}.eml");

                    var results = new
                    {
                        SenderEmail = message.From.ToString(),
                        EmailSubject = message.Subject,
                        EmailDate = message.Date,
                        ResultPrd = parsedResult,
                        ResultCandidata = parsedRelease,
                        EmlLink = emlLink,
                    };

                    // Paths for the JSON with the comparisons to be saved to
                    string fileName = $"ResultsComparison_{DateTime.Now:yyyyMMdd}.json";
                    string dailyFolderPath = Path.Combine("DifferentResults", Path.GetFileNameWithoutExtension(rcFilePath), DateTime.Now.ToString("yyyy-MM-dd"));
                    Directory.CreateDirectory(dailyFolderPath);
                    string jsonFilePath = Path.Combine(dailyFolderPath, fileName);

                    if (File.Exists(jsonFilePath))
                    {
                        string existingContent = File.ReadAllText(jsonFilePath);
                        JArray existingResults = JArray.Parse(existingContent);

                        existingResults.Add(JObject.FromObject(results));

                        File.WriteAllText(jsonFilePath, JsonConvert.SerializeObject(existingResults, Formatting.Indented));
                    }
                    else
                    {
                        JArray newResults = new JArray { JObject.FromObject(results) };
                        File.WriteAllText(jsonFilePath, JsonConvert.SerializeObject(newResults, Formatting.Indented));
                    }

                    if (emailUId.IsValid && !string.IsNullOrEmpty(emailBox))
                    {
                        string folderToSave = Path.Combine(uniqueEmailFolder, emailIdFolder);
                        await EmailHelper.DownloadEmailAndAttachmentsAsync(emailUId, emailBox, folderToSave, account);
                    }
                }
            }
        }
        catch (Exception ex)
        {
            Log.Error($"Error retrieving response from the OpenAI API: {ex.Message}");
        }

        return true;
    }

    private static string ExtractOnlyJSONAndReplaceNullWithEmptyString(string response)
    {
        // In case OpenAI adds text before or after the json, look for the {} of the json and delete everything outside
        Regex regex = new(@"\{[\s\S]*\}");
        Match match = regex.Match(response);
        if (!match.Success)
        {
            throw new System.Exception("No valid JSON found in the response");
        }

        response = match.Value;
        return Util.ReplaceJsonNullValuesToEmptyString(response);
    }

    public static string RemoveDiacritics(string text)
    {
        var normalizedString = text.Normalize(NormalizationForm.FormD);
        var stringBuilder = new StringBuilder();

        foreach (char c in normalizedString)
        {
            if (CharUnicodeInfo.GetUnicodeCategory(c) != UnicodeCategory.NonSpacingMark)
                stringBuilder.Append(c);
        }

        return stringBuilder.ToString().Normalize(NormalizationForm.FormC);
    }
}
