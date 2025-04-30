// // Copyright (c) 2024 Engibots. All rights reserved.

using engimatrix.Config;
using Microsoft.IdentityModel.Tokens;
using MimeKit;

namespace engimatrix.Utils
{
    public static class EmailCategorizer
    {
        public static (string, int) GetFolderNameAndCategoryConstantFromCategory(string? category, string? confianca)
        {
            if (string.IsNullOrEmpty(category))
            {
                category = "Erro";
            }
            if (string.IsNullOrEmpty(confianca))
            {
                confianca = "0";
            }

            int thresholdConfiancaAI = ConfigManager.theresholdConfiancaAI;

            int categoryToSave = 0;
            string folderToSave = "";

            if (Int32.Parse(confianca) > thresholdConfiancaAI)
            {
                switch (category)
                {
                    case "Cotacoes e Orcamentos" or "Cotações & Orçamentos":
                        categoryToSave = CategoryConstants.CategoryCode.COTACOES_ORCAMENTOS;
                        folderToSave = ConfigManager.QuotationsFolder;
                        break;

                    case "Encomendas":
                        categoryToSave = CategoryConstants.CategoryCode.ENCOMENDAS;
                        folderToSave = ConfigManager.OrdersFolder;
                        break;

                    case "Comprovativos de Pagamento":
                        categoryToSave = CategoryConstants.CategoryCode.COMPROVATIVOS_PAGAMENTO;
                        folderToSave = ConfigManager.ReceiptsFolder;
                        break;

                    case "Certificados de Qualidade":
                        categoryToSave = CategoryConstants.CategoryCode.CERTIFICADOS_QUALIDADE;
                        folderToSave = ConfigManager.CertificatesFolder;
                        break;

                    case "Duplicados":
                        categoryToSave = CategoryConstants.CategoryCode.DUPLICADOS;
                        folderToSave = ConfigManager.DuplicatesFolder;
                        break;

                    default:
                        categoryToSave = CategoryConstants.CategoryCode.OUTROS;
                        folderToSave = ConfigManager.ValidateFolder;
                        break;
                }
            }
            else if (category.Equals("Outros", StringComparison.Ordinal))
            {
                categoryToSave = CategoryConstants.CategoryCode.OUTROS;
                folderToSave = ConfigManager.ValidateFolder;
            }
            else
            {
                categoryToSave = CategoryConstants.CategoryCode.ERRO;
                folderToSave = ConfigManager.ValidateFolder;
            }

            return (folderToSave, categoryToSave);
        }

        public static string GetFolderByCategoryId(int categoryId)
        {
            Dictionary<int, string> categoryFolderMapping = new()
            {
                { CategoryConstants.CategoryCode.ENCOMENDAS, ConfigManager.OrdersFolder },
                { CategoryConstants.CategoryCode.COTACOES_ORCAMENTOS, ConfigManager.QuotationsFolder },
                { CategoryConstants.CategoryCode.COMPROVATIVOS_PAGAMENTO, ConfigManager.ReceiptsFolder },
                { CategoryConstants.CategoryCode.CERTIFICADOS_QUALIDADE, ConfigManager.CertificatesFolder },
                { CategoryConstants.CategoryCode.SPAM, ConfigManager.SpamFolder },
                { CategoryConstants.CategoryCode.DUPLICADOS, ConfigManager.DuplicatesFolder },
                { CategoryConstants.CategoryCode.OUTROS, ConfigManager.OthersFolder },
                { CategoryConstants.CategoryCode.ERRO, ConfigManager.ValidateFolder }
            };

            if (categoryFolderMapping.TryGetValue(categoryId, out string? folder))
            {
                return folder;
            }

            throw new ArgumentException($"GetFolderByCategoryId: Invalid category id given: {categoryId}");
        }

        public static string GetFolderByCategoryId(string categoryId)
        {
            int category = Int32.Parse(categoryId);
            return GetFolderByCategoryId(category);
        }
    }
}
