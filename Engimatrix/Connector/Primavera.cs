// // Copyright (c) 2024 Engibots. All rights reserved.

using System.Diagnostics;
using engimatrix.ModelObjs.Primavera;
using engimatrix.Utils;


namespace engimatrix.Connector;

public static class Primavera
{
    public static async Task<PrimaveraListResponseItem<T>> GetListAsync<T>(
        string listGuid,
        int offset,
        int limit,
        string sqlWhere
    )
    {
        // Validate that the listGuid exists in the configuration
        if (string.IsNullOrEmpty(listGuid) || !PrimaveraHelpers.IsValidListGuid(listGuid))
        {
            throw new ArgumentException($"The listGuid '{listGuid}' is not a valid GUID from the configuration.");
        }

        PrimaveraListParametersItem parameters = new()
        {
            Offset = offset,
            Limit = limit,
            SqlWhere = sqlWhere,
            ListGuid = listGuid
        };

        Stopwatch stopwatch = new();
        stopwatch.Start();
        Log.Debug($"Primavera.GetListAsync<{typeof(T).Name}> - Starting");

        // Use the generic GetListResourceAsync method with the dynamic type T
        PrimaveraListResponseItem<T> result = await PrimaveraGenerics.GetListResourceAsync(
            parameters,
            Activator.CreateInstance<T> // This will create a new instance of T dynamically
        );

        stopwatch.Stop();
        Log.Debug($"Primavera.GetListAsync<{typeof(T).Name}> - Time elapsed: {stopwatch.ElapsedMilliseconds} ms");

        return result;
    }

    public static async Task<PrimaveraListResponseItem<T>> GetListAsync<T>(string listGuid)
    {
        return await GetListAsync<T>(listGuid, 9999, 0, "1=1");
    }

    public static async Task<PrimaveraListResponseItem<T>> GetListAsync<T>(string listGuid, string sqlWhere)
    {
        return await GetListAsync<T>(listGuid, 9999, 0, sqlWhere);
    }

    public static async Task<PrimaveraDocumentCreationResponse> CreateDocumentAsync(PrimaveraDocumentCreationItem document)
    {
        string url = "Vendas/Docs/CreateSalesDocument/";
        PrimaveraDocumentCreationResponse response = await PrimaveraGenerics.PostResourceAsync(
            url,
            () => new PrimaveraDocumentCreationResponse { Results = [] },
            document
        );
        return response;
    }

    /// <summary>
    /// Get a document from Primavera
    /// </summary>
    /// <param name="tipoDoc">Type of the document. ORC, etc...</param>
    /// <param name="serie">Year of the document. 2024, 2025, etc...</param>
    /// <param name="numDoc">Number of the document. 4, 10, 50, etc...</param>
    /// <returns>The Primavera Document</returns>
    public static async Task<PrimaveraDocument> GetDocumentAsync(string tipoDoc, string serie, string numDoc)
    {
        string filial = "000"; // -> Fixo
        string url = $"Vendas/Docs/Edita/{filial}/{tipoDoc}/{serie}/{numDoc}";
        PrimaveraDocument document = await PrimaveraGenerics.GetResourceAsync(url, () => new PrimaveraDocument());
        return document;
    }

    /// <summary>
    /// Anula um documento de venda
    /// </summary>
    /// <param name="tipoDoc"></param>
    /// <param name="serie"></param>
    /// <param name="numDoc"></param>
    /// <param name="motivo"></param>
    /// <param name="emissaoDoc"></param>
    /// <param name="observacoes"></param>
    /// <param name="avisos"></param>
    /// <returns></returns>
    public static async Task<object?> AnulaDocumentAsync(
        string tipoDoc,
        string serie,
        string numDoc,
        string motivo,
        string observacoes,
        string avisos
    ) => await AnulaDocumentoAsync(tipoDoc, serie, numDoc, motivo, observacoes, avisos, true);
    public static async Task<object?> AnulaDocumentoAsync(
        string tipoDoc,
        string serie,
        string numDoc,
        string motivo,
        string observacoes,
        string avisos,
        bool emissaoDoc
    )
    {
        // Filial is fixed as "000"
        string filial = "000";

        // Build the URL, encoding the text parameters to ensure valid URL syntax
        string strEmissaoDoc = emissaoDoc ? "true" : "false";
        string strObservacoes = Uri.EscapeDataString(observacoes);
        string strAvisos = Uri.EscapeDataString(avisos);
        string url = $"Vendas/Docs/AnulaDocumento/{filial}/{tipoDoc}/{serie}/{numDoc}/{motivo}/{strEmissaoDoc}/{strObservacoes}/{strAvisos}";

        // Make the POST request and return the resulting document
        object? document = await PrimaveraGenerics.SendRequestAsync<object>(url, HttpMethod.Post);
        return document;
    }

    public static async Task<PrimaveraProductsItem> GetProductsListAsync()
    {
        string url = "Base/Artigos/LstArtigos";
        PrimaveraProductsItem productsList = await PrimaveraGenerics.GetResourceAsync(url, () => new PrimaveraProductsItem());
        return productsList;
    }

    public static async Task<bool> CheckProductExistenceAsync(string artigo)
    {
        string url = $"Base/Artigos/Existe/{artigo}";
        bool productExists = await PrimaveraGenerics.CheckExistenceAsync(url);
        return productExists;
    }

    public static async Task<PrimaveraClientsItem> GetClientsListAsync()
    {
        string url = "Base/Clientes/LstClientes";
        PrimaveraClientsItem clientsList = await PrimaveraGenerics.GetResourceAsync(url, () => new PrimaveraClientsItem());
        return clientsList;
    }

    public static async Task<bool> CheckClientExistenceAsync(string cliente)
    {
        string url = $"Base/Clientes/Existe/{cliente}";
        bool clientExists = await PrimaveraGenerics.CheckExistenceAsync(url);
        return clientExists;
    }

    public static async Task<PrimaveraSuppliersItem> GetSuppliersListAsync()
    {
        string url = "Base/Fornecedores/LstFornecedores";
        PrimaveraSuppliersItem suppliersList = await PrimaveraGenerics.GetResourceAsync(url, () => new PrimaveraSuppliersItem());
        return suppliersList;
    }

    public static async Task<bool> CheckSupplierExistenceAsync(string fornecedor)
    {
        string url = $"Base/Fornecedores/Existe/{fornecedor}";
        bool supplierExists = await PrimaveraGenerics.CheckExistenceAsync(url);
        return supplierExists;
    }

    public static async Task<PrimaveraPurchasesItem[]> GetPurchaseListAsync()
    {
        string url = "Base/Series/ListaSeries/C/ECF/false";
        PrimaveraPurchasesItem[] purchaseList = await PrimaveraGenerics.GetResourceAsync(url, () => Array.Empty<PrimaveraPurchasesItem>());
        return purchaseList;
    }

    public static async Task<bool> CheckPurchaseExistenceAsync(string documento)
    {
        string url = $"Compras/TabCompras/Existe/{documento}";
        bool purchaseExists = await PrimaveraGenerics.CheckExistenceAsync(url);
        return purchaseExists;
    }

    public static async Task<PrimaveraInternalDocumentsItem> GetInternalDocumentsListAsync()
    {
        string url = "Internos/TabInternos/LstDocInternos";
        PrimaveraInternalDocumentsItem internalDocumentsList = await PrimaveraGenerics.GetResourceAsync(url, () => new PrimaveraInternalDocumentsItem());
        return internalDocumentsList;
    }

    public static async Task<bool> CheckInternalDocumentExistenceAsync(string documento)
    {
        string url = $"Internos/TabInternos/Existe/{documento}";
        bool internalDocumentExists = await PrimaveraGenerics.CheckExistenceAsync(url);
        return internalDocumentExists;
    }

    public static async Task<PrimaveraWarehousesItem> GetWarehousesListAsync()
    {
        string url = "Inventario/Armazens/LstArmazens";
        PrimaveraWarehousesItem warehousesList = await PrimaveraGenerics.GetResourceAsync(url, () => new PrimaveraWarehousesItem());
        return warehousesList;
    }

    public static async Task<bool> CheckWarehouseExistenceAsync(string armazem)
    {
        string url = $"Inventario/Armazens/Existe/{armazem}";
        bool warehouseExists = await PrimaveraGenerics.CheckExistenceAsync(url);
        return warehouseExists;
    }
}
