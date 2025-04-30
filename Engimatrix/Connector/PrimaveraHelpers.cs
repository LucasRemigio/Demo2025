// // Copyright (c) 2024 Engibots. All rights reserved.

using System.Text.Json;
using engimatrix.Config;
using engimatrix.Exceptions;
using engimatrix.ModelObjs.Primavera;
using engimatrix.Utils;

namespace engimatrix.Connector;

public static class PrimaveraHelpers
{
    // Method to validate if the listGuid exists in the configuration
    public static bool IsValidListGuid(string listGuid)
    {
        // Compare the listGuid with the values in the ConfigManager.PrimaveraUrls
        List<string> validGuids =
        [
            ConfigManager.PrimaveraUrls.Moedas,
            ConfigManager.PrimaveraUrls.Paises,
            ConfigManager.PrimaveraUrls.Distritos,
            ConfigManager.PrimaveraUrls.TaxasIVA,
            ConfigManager.PrimaveraUrls.ModoPagamento,
            ConfigManager.PrimaveraUrls.CondPagamento,
            ConfigManager.PrimaveraUrls.Unidades,
            ConfigManager.PrimaveraUrls.UnidadesConversao,
            ConfigManager.PrimaveraUrls.Marcas,
            ConfigManager.PrimaveraUrls.Modelos,
            ConfigManager.PrimaveraUrls.Familias,
            ConfigManager.PrimaveraUrls.SubFamilias,
            ConfigManager.PrimaveraUrls.Dimensoes,
            ConfigManager.PrimaveraUrls.LinhaDimensao,
            ConfigManager.PrimaveraUrls.Artigos,
            ConfigManager.PrimaveraUrls.ArtigosUnidades,
            ConfigManager.PrimaveraUrls.Clientes,
            ConfigManager.PrimaveraUrls.Fornecedores,
            ConfigManager.PrimaveraUrls.MoradasAlternativas,
            ConfigManager.PrimaveraUrls.StockDisponivelArtigo,
            ConfigManager.PrimaveraUrls.StockDisponivelArtigoArmazem,
            ConfigManager.PrimaveraUrls.PrecoVenda,
            ConfigManager.PrimaveraUrls.PrecoCompra,
            ConfigManager.PrimaveraUrls.Pendentes,
            ConfigManager.PrimaveraUrls.EncomendasCabecalhos,
            ConfigManager.PrimaveraUrls.EncomendasLinhas,
            ConfigManager.PrimaveraUrls.MFClientes,
            ConfigManager.PrimaveraUrls.MFArtigos,
            ConfigManager.PrimaveraUrls.MFFaturas
        ];

        return validGuids.Contains(listGuid);
    }
    public static readonly Dictionary<string, string> PrimaveraListNames = new()
    {
        { ConfigManager.PrimaveraUrls.Moedas, "Moedas" },
        { ConfigManager.PrimaveraUrls.Paises, "Paises" },
        { ConfigManager.PrimaveraUrls.Distritos, "Distritos" },
        { ConfigManager.PrimaveraUrls.TaxasIVA, "Taxas IVA" },
        { ConfigManager.PrimaveraUrls.ModoPagamento, "Modo Pagamento" },
        { ConfigManager.PrimaveraUrls.CondPagamento, "Cond Pagamento" },
        { ConfigManager.PrimaveraUrls.Unidades, "Unidades" },
        { ConfigManager.PrimaveraUrls.UnidadesConversao, "Unidades Conversao" },
        { ConfigManager.PrimaveraUrls.Marcas, "Marcas" },
        { ConfigManager.PrimaveraUrls.Modelos, "Modelos" },
        { ConfigManager.PrimaveraUrls.Familias, "Familias" },
        { ConfigManager.PrimaveraUrls.SubFamilias, "SubFamilias" },
        { ConfigManager.PrimaveraUrls.Dimensoes, "Dimensoes" },
        { ConfigManager.PrimaveraUrls.LinhaDimensao, "Linha Dimensao" },
        { ConfigManager.PrimaveraUrls.Artigos, "Artigos" },
        { ConfigManager.PrimaveraUrls.ArtigosUnidades, "Artigos Unidades" },
        { ConfigManager.PrimaveraUrls.Clientes, "Clientes" },
        { ConfigManager.PrimaveraUrls.Fornecedores, "Fornecedores" },
        { ConfigManager.PrimaveraUrls.MoradasAlternativas, "Moradas Alternativas" },
        { ConfigManager.PrimaveraUrls.StockDisponivelArtigo, "Stock Disponivel Artigo" },
        { ConfigManager.PrimaveraUrls.StockDisponivelArtigoArmazem, "Stock Disponivel Artigo Armazem" },
        { ConfigManager.PrimaveraUrls.PrecoVenda, "Preco Venda" },
        { ConfigManager.PrimaveraUrls.PrecoCompra, "Preco Compra" },
        { ConfigManager.PrimaveraUrls.Pendentes, "Pendentes" },
        { ConfigManager.PrimaveraUrls.EncomendasCabecalhos, "Encomendas Cabecalhos" },
        { ConfigManager.PrimaveraUrls.EncomendasLinhas, "Encomendas Linhas" },
        { ConfigManager.PrimaveraUrls.MFClientes, "MF Clientes" },
        { ConfigManager.PrimaveraUrls.MFArtigos, "MF Artigos" },
        { ConfigManager.PrimaveraUrls.MFFaturas, "MF Faturas" }
    };

}
