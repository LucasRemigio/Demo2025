// // Copyright (c) 2024 Engibots. All rights reserved.

using System.Text.Json;
using engimatrix.Config;
using engimatrix.Connector;
using engimatrix.Exceptions;
using engimatrix.ModelObjs;
using engimatrix.ModelObjs.Primavera;
using engimatrix.Utils;
using Microsoft.AspNetCore.Mvc;
using static engimatrix.Config.PrimaveraEnums;

namespace engimatrix.Models;

public static class PrimaveraDocumentModel
{
    public async static Task<PrimaveraDocumentCreationResponse> CreateDocument(string orderToken, string executeUser)
    {
        OrderItem? order = OrderModel.GetOrderByToken(orderToken, executeUser) ?? throw new NotFoundException("Order not found.");
        MFPrimaveraClientItem client = await PrimaveraClientModel.GetPrimaveraClient(order.client_code) ?? throw new NotFoundException("Client not found.");

        TipoDoc tipoDoc = order.is_draft ? TipoDoc.ORC : TipoDoc.ECL;

        // if the tipoDoc is ORC, check if there is any previous ORC and delete it
        if (tipoDoc == TipoDoc.ORC)
        {
            await CancelLastQuotation(orderToken, executeUser);
        }

        // the serie is the current year we are in
        string serie = DateTime.Now.Year.ToString();

        PrimaveraDocumentCreationItem document = new()
        {
            Filial = "000", // -> Fixo
            TipoEntidade = "C", // -> Fixo
            Tipodoc = tipoDoc.ToString().ToUpperInvariant(),
            Serie = serie,
            DataDoc = DateTime.Now,
            HoraDefinida = true,
            Cambio = 1,
            Entidade = order.client_code,
            CodigoPostalFac = PrimaveraDocumentHelpers.FixClientPostalCode(client.CodPostal),
            NumContribuinteFac = PrimaveraDocumentHelpers.FixClientContribuinte(client.Contribuinte),
        };

        if (order.is_delivery)
        {
            Log.Debug("Order is delivery, setting delivery address.");
            document.Morada = order.address;
            document.Localidade = order.city;
            document.CodigoPostal = order.postal_code;
            document.LocalidadeCodigoPostal = order.city;
            document.Pais = "PT";

            document.MoradaEntrega = order.address;
            document.LocalidadeEntrega = order.city;
            document.CodPostalEntrega = order.postal_code;
            document.CodPostalLocalidadeEntrega = order.city;
        }

        List<PrimaveraDocumentLineCreationItem> lines = GetDocumentLines(orderToken, executeUser);
        document.Linhas = lines;

        PrimaveraDocumentCreationResponse response = await Primavera.CreateDocumentAsync(document);

        if (!string.IsNullOrEmpty(response.ErrorMessage))
        {
            throw new Exception($"Failed to create document, error message is not null. ErrorMessage = {response.ErrorMessage}");
        }
        if (response.StatusCode != 200)
        {
            throw new Exception($"Failed to create document, status code is not 200. StatusCode = {response.StatusCode}");
        }
        if (response.Results == null || response.Results.Count == 0)
        {
            throw new Exception($"Failed to create document, no results returned. ErrorMessage = {response.ErrorMessage}");
        }

        return response;
    }

    private static async Task CancelLastQuotation(string orderToken, string executeUser)
    {
        OrderPrimaveraDocumentItem? lastDocument = OrderPrimaveraDocumentModel.GetLastOrderDoc(orderToken, executeUser);

        if (!IsLastDocumentOrc(lastDocument))
        {
            Log.Debug("Last document is not an orçamento, skipping cancelation.");
            return;
        }

        string motivo = AnulaDocumentoMotivo.ALTERACAO_DIARIO_DOCUMENTO.GetCode();
        string observacoes = "Cancelamento de orcamento para criacao de novo";
        string avisos = "N";

        await Primavera.AnulaDocumentAsync(lastDocument.type, lastDocument.series, lastDocument.number, motivo, observacoes, avisos);
        Log.Debug("Last primavera document canceled successfully");

        // delete the last document from the database
        bool success = OrderPrimaveraDocumentModel.Delete(lastDocument.id, orderToken, executeUser);
        if (!success)
        {
            Log.Error($"Failed to delete last document from database with id {lastDocument.id} and order token {orderToken}.");
        }

        Log.Debug($"Last document deleted from our database with id {lastDocument.id} and order token {orderToken}.");
    }

    private static bool IsLastDocumentOrc(OrderPrimaveraDocumentItem? doc)
    {
        if (doc is null)
        {
            return false;
        }

        if (!doc.type.Equals(TipoDoc.ORC.ToString(), StringComparison.OrdinalIgnoreCase))
        {
            return false;
        }

        return true;
    }

    private static List<PrimaveraDocumentLineCreationItem> GetDocumentLines(string orderToken, string executeUser)
    {
        List<PrimaveraDocumentLineCreationItem> lines = [];

        // Order products
        List<OrderProductItem> products = OrderProductModel.GetOrderProducts(orderToken, executeUser);

        // Our catalog products
        List<string> productIds = [.. products.Select(product => product.product_catalog_id.ToString())];
        List<ProductCatalogItem> catalogs = ProductCatalogModel.GetProductCatalogsByIds(productIds, true, executeUser);
        Dictionary<int, ProductCatalogItem> catalogHash = catalogs.ToDictionary(catalog => catalog.id);

        // Product Units
        List<ProductUnitItem> units = ProductUnitModel.GetProductUnits(executeUser);
        Dictionary<int, ProductUnitItem> unitHash = units.ToDictionary(unit => unit.id);

        foreach (OrderProductItem product in products)
        {
            if (!catalogHash.TryGetValue(product.product_catalog_id, out ProductCatalogItem? catalog))
            {
                throw new NotFoundException($"Product with id {product.product_catalog_id} not found.");
            }

            if (!unitHash.TryGetValue(product.product_unit_id, out ProductUnitItem? unit))
            {
                throw new NotFoundException($"Product unit with id {product.product_unit_id} not found.");
            }

            Log.Debug("Product: " + JsonSerializer.Serialize(product));
            Log.Debug("Unit: " + unit.abbreviation);

            // we have 2 columns. The calculated price which is the product price + margin and the price_discount
            // which is the calculated_price with the client discount applied.
            decimal discount = product.calculated_price - product.price_discount;

            PrimaveraDocumentLineCreationItem line = new()
            {
                Artigo = catalog.product_code,
                Descricao = catalog.description_full,
                Quantidade = product.quantity,
                Unidade = unit.abbreviation.ToUpperInvariant(),
                TaxaIva = 23,
                CodIva = "23",
                PrecUnit = product.calculated_price,
                Desconto1 = discount,
            };

            lines.Add(line);
        }

        return lines;
    }

    public static OrderPrimaveraDocumentItem GetOrderDocumentItemFromResponse(PrimaveraDocumentCreationResponse primaveraDocumentResponse)
    {
        OrderPrimaveraDocumentItem document = new();

        // Results is not empty as, in CreateDocument, we throw an exception if it is null or empty
        // basically this response is so weird that is lists 5 items exatly equals, but the properties change
        // For that, we must cycle all the items and fill in our item with the desired contents
        foreach (PrimaveraDocumentResult doc in primaveraDocumentResponse.Results!)
        {
            // Valor is never null or empty
            JsonElement jsonElement = (JsonElement)doc.Valor!;
            string content = jsonElement.ToString();

            if (string.IsNullOrEmpty(content))
            {
                throw new Exception($"Primavera document info content is null or empty. {content}");
            }

            if (!Enum.TryParse(doc.Nome, out DocumentResultTypes docType))
            {
                throw new Exception($"Failed to parse Primavera document info type. Document info type = {doc.Nome}");
            }

            // Continuing the other comment, this basically switches the response from the list of objects
            // untill all the contents are filled in correctly
            switch (docType)
            {
                case DocumentResultTypes.Documento:
                    document.name = content;
                    break;
                case DocumentResultTypes.TipoDocumento:
                    document.type = content;
                    break;
                case DocumentResultTypes.Serie:
                    document.series = content;
                    break;
                case DocumentResultTypes.NumeroDocumento:
                    document.number = content;
                    break;
                default:
                    // We don't care about the other types
                    break;
            }
        }

        return document;
    }

    public static async Task<PrimaveraDocumentCreationResponse> CreateOrderFromQuotation(string orderToken, string executeUser)
    {
        // get the current order document
        OrderPrimaveraDocumentItem? orderPrimaveraDocumentItem = OrderPrimaveraDocumentModel.GetLastOrderDoc(orderToken, executeUser);

        if (orderPrimaveraDocumentItem is null)
        {
            // create the primavera order document
            throw new ResourceEmptyException("No order document found for order " + orderToken);
        }

        // get the document from primavera
        PrimaveraDocument primaveraDocument = await Primavera.GetDocumentAsync(orderPrimaveraDocumentItem.type, orderPrimaveraDocumentItem.series, orderPrimaveraDocumentItem.number);

        // create the order
        // The order is created the following way: We grab the quotation, and with the IdLinha of each product, we can link the products
        // that will go to the order from the products that were in the quotation.
        PrimaveraDocumentCreationItem orderDocument = CreateOrderDocumentFromQuotation(orderToken, primaveraDocument);
        PrimaveraDocumentCreationResponse response = await Primavera.CreateDocumentAsync(orderDocument);

        if (!string.IsNullOrEmpty(response.ErrorMessage))
        {
            throw new Exception($"Failed to create document, error message is not null. ErrorMessage = {response.ErrorMessage}");
        }
        if (response.StatusCode != 200)
        {
            throw new Exception($"Failed to create document, status code is not 200. StatusCode = {response.StatusCode}");
        }
        if (response.Results == null || response.Results.Count == 0)
        {
            throw new Exception($"Failed to create document, no results returned. ErrorMessage = {response.ErrorMessage}");
        }

        Log.Debug("Results count : " + response.Results.Count);
        return response;
    }

    public static PrimaveraDocumentCreationItem CreateOrderDocumentFromQuotation(string orderToken, PrimaveraDocument quotationDoc)
    {
        // this gets the current order products in a primavera document lines Format
        List<PrimaveraDocumentLineCreationItem> orderProducts = GetDocumentLines(orderToken, "System");

        PrimaveraDocumentCreationItem orderDocument = quotationDoc.ToCreationItem();
        // Change the type from ORC (Quotation) to ECL (Order)
        orderDocument.Tipodoc = TipoDoc.ECL.ToString().ToUpperInvariant();

        // we must check if there was any removed item, added item or changed item
        List<Linha> documentProducts = quotationDoc.Linhas;

        // first we must check if any of the items was removed
        // then we check if there is any new product
        // then, on the products that stayed, see if quantity or price changed
        Dictionary<string, PrimaveraDocumentLineCreationItem> currentProductsByCode = orderProducts.ToDictionary(product => product.Artigo!);
        Dictionary<string, Linha> documentProductsByCode = documentProducts.ToDictionary(product => product.Artigo!);

        // check if any product was removed
        // document products:   10, 22, 44, 20, 30
        // order    products:   22, 44, 20, 30, 76 -> 10 was removed and 76 was added
        List<string> remainingProducts = [.. currentProductsByCode.Keys];
        List<PrimaveraDocumentLineCreationItem> updatedProducts = [.. documentProducts.Select(product => product.ToCreationItem())];
        foreach (string code in documentProductsByCode.Keys)
        {
            if (!currentProductsByCode.TryGetValue(code, out PrimaveraDocumentLineCreationItem? currentProduct))
            {
                // product was removed
                Linha linha = documentProductsByCode[code];
                updatedProducts.RemoveAll(p => p.Artigo == code);
                remainingProducts.Remove(code);
                Log.Debug("Removed product: " + JsonSerializer.Serialize(linha.ToCreationItem()));
                continue;
            }

            // found, so check if anything changed
            Linha docProduct = documentProductsByCode[code];

            if (docProduct.Quantidade != (double)currentProduct.Quantidade! || docProduct.PrecUnit != (double)currentProduct.PrecUnit! || docProduct.Unidade != currentProduct.Unidade)
            {
                // quantity or price changed
                // we must update the product
                updatedProducts.RemoveAll(p => p.Artigo == code);
                docProduct.Quantidade = (double)currentProduct.Quantidade!;
                docProduct.PrecUnit = (double)currentProduct.PrecUnit!;
                docProduct.Unidade = currentProduct.Unidade!;
                Log.Debug("Updated product: " + JsonSerializer.Serialize(docProduct.ToCreationItem()));
                updatedProducts.Add(docProduct.ToCreationItem());
            }

            remainingProducts.Remove(code);
        }

        // check if any product was added
        foreach (string code in remainingProducts)
        {
            PrimaveraDocumentLineCreationItem orderProduct = currentProductsByCode[code];
            updatedProducts.Add(orderProduct);
            Log.Debug("Added product: " + JsonSerializer.Serialize(orderProduct));
        }

        orderDocument.Linhas = updatedProducts;

        return orderDocument;
    }


}

public static class PrimaveraDocumentExtensions
{
    // This keyword is used to extend the PrimaveraDocument class with a new method
    // that converts the PrimaveraDocument object to a PrimaveraDocumentCreationItem object
    // This means that even tho this is not decalred inside the PrimaveraDocument class,
    // we can use it as if it were a method of the class, making this an extension method
    public static PrimaveraDocumentCreationItem ToCreationItem(this PrimaveraDocument doc)
    {
        return new PrimaveraDocumentCreationItem
        {
            Filial = doc.Filial,
            Tipodoc = doc.Tipodoc,
            Serie = doc.Serie,
            TipoEntidade = doc.TipoEntidade,
            Entidade = doc.Entidade,
            DataDoc = doc.DataDoc,
            HoraDefinida = doc.HoraDefinida,
            Cambio = (decimal)doc.Cambio,
            Nome = doc.Nome,
            Morada = doc.Morada,
            Morada2 = doc.Morada2,
            Localidade = doc.Localidade,
            CodigoPostal = doc.CodigoPostal,
            LocalidadeCodigoPostal = doc.LocalidadeCodigoPostal,
            Pais = doc.Pais,
            Distrito = doc.Distrito,
            NumContribuinte = doc.NumContribuinte,
            Referencia = doc.Referencia,
            ModoPag = doc.ModoPag,
            CondPag = doc.CondPag,
            ModoExp = doc.ModoExp,
            Moeda = doc.Moeda,
            TipoOperacao = doc.TipoOperacao,
            Seccao = doc.Seccao,
            Origem = doc.Origem,
            EspacoFiscal = doc.EspacoFiscal.ToString(),
            RegimeIva = doc.RegimeIva,
            RegimeIvaReembolsos = doc.RegimeIvaReembolsos.ToString(),
            LocalOperacao = doc.LocalOperacao,
            TipoEntidadeFac = doc.TipoEntidadeFac,
            EntidadeFac = doc.EntidadeFac,
            NomeFac = doc.NomeFac,
            MoradaFac = doc.MoradaFac,
            Morada2Fac = doc.Morada2Fac,
            LocalidadeFac = doc.LocalidadeFac,
            CodigoPostalFac = doc.CodigoPostalFac,
            LocalidadeCodigoPostalFac = doc.LocalidadeCodigoPostalFac,
            NumContribuinteFac = doc.NumContribuinteFac,
            PaisFac = doc.PaisFac,
            DistritoFac = doc.DistritoFac,
            EntidadeDescarga = doc.EntidadeDescarga,
            LocalDescarga = doc.LocalDescarga,
            TipoEntidadeEntrega = doc.TipoEntidadeEntrega,
            EntidadeEntrega = doc.EntidadeEntrega,
            NomeEntrega = doc.NomeEntrega,
            MoradaEntrega = doc.MoradaEntrega,
            Morada2Entrega = doc.Morada2Entrega,
            LocalidadeEntrega = doc.LocalidadeEntrega,
            CodPostalEntrega = doc.CodPostalEntrega,
            CodPostalLocalidadeEntrega = doc.CodPostalLocalidadeEntrega,
            DistritoEntrega = doc.DistritoEntrega,
            Observacoes = doc.Observacoes,
            Linhas = [.. doc.Linhas.Select(line => line.ToCreationItem())]
        };
    }

    public static PrimaveraDocumentLineCreationItem ToCreationItem(this Linha line)
    {
        return new PrimaveraDocumentLineCreationItem
        {
            Artigo = line.Artigo,
            Quantidade = (decimal)line.Quantidade,
            PrecUnit = (decimal)line.PrecUnit,
            TaxaIva = (decimal)line.TaxaIva,
            CodIva = line.CodIva,
            Desconto1 = (decimal)line.Desconto1,
            Desconto2 = (decimal)line.Desconto2,
            Desconto3 = (decimal)line.Desconto3,
            TipoLinha = line.TipoLinha,
            IDLinhaOriginal = line.IdLinha,
            Descricao = line.Descricao,
            Unidade = line.Unidade,
            CamposUtil = [.. line.CamposUtil.Select(campo => new CampoUtilCreationItem
            {
                Nome = campo.Nome,
                Valor = campo.Valor
            })]
        };
    }
}
