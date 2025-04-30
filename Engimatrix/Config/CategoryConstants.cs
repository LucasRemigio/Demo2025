using System.Collections.Generic;
using Engimatrix.ModelObjs;

namespace engimatrix.Config;

public static class CategoryConstants
{
    public static class CategoryCode
    {
        public static readonly int ENCOMENDAS = 1;
        public static readonly int COTACOES_ORCAMENTOS = 2;
        public static readonly int COMPROVATIVOS_PAGAMENTO = 3;
        public static readonly int OUTROS = 4;
        public static readonly int ERRO = 5;
        public static readonly int DUPLICADOS = 6;
        public static readonly int CERTIFICADOS_QUALIDADE = 7;
        public static readonly int SPAM = 8;
    }

    public static class CategoryTitle
    {
        public static readonly string ENCOMENDAS = "Encomendas";
        public static readonly string COTACOES_ORCAMENTOS = "Cotações & Orçamentos";
        public static readonly string COMPROVATIVOS_PAGAMENTO = "Comprovativos de Pagamento";
        public static readonly string OUTROS = "Outros";
        public static readonly string ERRO = "Erro";
        public static readonly string DUPLICADOS = "Duplicados";
        public static readonly string CERTIFICADOS_QUALIDADE = "Certificados de Qualidade";
        public static readonly string SPAM = "Spam";
    }

    private static readonly Dictionary<int, string> CategoryNamesForEmail = new()
    {
        { CategoryConstants.CategoryCode.ENCOMENDAS, "Encomenda" },
        { CategoryConstants.CategoryCode.COTACOES_ORCAMENTOS, "Cotação" },
        { CategoryConstants.CategoryCode.COMPROVATIVOS_PAGAMENTO, "Comprovativo" },
        { CategoryConstants.CategoryCode.OUTROS, CategoryTitle.OUTROS },
        { CategoryConstants.CategoryCode.ERRO, CategoryTitle.ERRO },
        { CategoryConstants.CategoryCode.DUPLICADOS, CategoryTitle.DUPLICADOS },
        { CategoryConstants.CategoryCode.CERTIFICADOS_QUALIDADE, CategoryTitle.CERTIFICADOS_QUALIDADE },
        { CategoryConstants.CategoryCode.SPAM, CategoryTitle.SPAM }
    };

    private static readonly Dictionary<int, string> CategoryNames = new()
    {
        { CategoryConstants.CategoryCode.ENCOMENDAS, CategoryTitle.ENCOMENDAS },
        { CategoryConstants.CategoryCode.COTACOES_ORCAMENTOS, CategoryTitle.COTACOES_ORCAMENTOS},
        { CategoryConstants.CategoryCode.COMPROVATIVOS_PAGAMENTO, CategoryTitle.COMPROVATIVOS_PAGAMENTO },
        { CategoryConstants.CategoryCode.OUTROS, CategoryTitle.OUTROS },
        { CategoryConstants.CategoryCode.ERRO, CategoryTitle.ERRO },
        { CategoryConstants.CategoryCode.DUPLICADOS, CategoryTitle.DUPLICADOS },
        { CategoryConstants.CategoryCode.CERTIFICADOS_QUALIDADE, CategoryTitle.CERTIFICADOS_QUALIDADE },
        { CategoryConstants.CategoryCode.SPAM, CategoryTitle.SPAM }
    };

    public static string GetCategoryNameForEmailByCode(int categoryId)
    {
        return CategoryNamesForEmail.TryGetValue(categoryId, out string? categoryName) ? categoryName : String.Empty;
    }

    public static string GetCategoryNameByCode(int categoryId)
    {
        return CategoryNames.TryGetValue(categoryId, out string
            ? categoryName) ? categoryName : String.Empty;
    }
}
