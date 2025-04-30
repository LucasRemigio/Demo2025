// // Copyright (c) 2024 Engibots. All rights reserved.

using System.Diagnostics;
using engimatrix.Config;
using engimatrix.Connector;
using engimatrix.Exceptions;
using engimatrix.ModelObjs;
using engimatrix.ModelObjs.Primavera;
using engimatrix.Utils;
using engimatrix.Views;

namespace engimatrix.Models;

public static class PrimaveraProductCatalogModel
{
    public async static Task<List<MFPrimaveraProductItem>> GetPrimaveraProductCatalogs()
    {
        PrimaveraListResponseItem<MFPrimaveraProductItem> primaveraProducts = await Primavera.GetListAsync<MFPrimaveraProductItem>(
            ConfigManager.PrimaveraUrls.MFArtigos,
            9999999,
            0,
            "1=1"
        );

        if (primaveraProducts.IsError)
        {
            throw new PrimaveraApiErrorException($"Error fetching the products from primavera: {primaveraProducts.Message}");
        }

        // Primavera, even though it might throw an error and not return anything, an
        // object on the list is always created, so we must check for 1
        if (primaveraProducts.Data.Count <= 1)
        {
            throw new ResourceEmptyException("Fetched products from primavera came empty");
        }

        return primaveraProducts.Data;
    }

    public static async Task<Dictionary<string, List<MFPrimaveraProductItem>>> GetPrimaveraProductsByProductCodeHashed()
    {
        List<MFPrimaveraProductItem> products = await GetPrimaveraProductCatalogs();

        Dictionary<string, List<MFPrimaveraProductItem>> productsByProductCode = [];
        foreach (MFPrimaveraProductItem product in products)
        {
            if (!productsByProductCode.TryGetValue(product.Artigo, out List<MFPrimaveraProductItem>? currentProducts))
            {
                currentProducts = [];
                productsByProductCode[product.Artigo] = currentProducts;
            }

            currentProducts.Add(product);
        }

        return productsByProductCode;
    }

    public static async Task<List<PrimaveraProductStockItem>> GetAvailableStockForProducts()
    {
        PrimaveraListResponseItem<PrimaveraProductStockItem> stock = await Primavera.GetListAsync<PrimaveraProductStockItem>(
            ConfigManager.PrimaveraUrls.StockDisponivelArtigo,
            9999999,
            0,
            "1=1"
        );

        if (stock.IsError)
        {
            throw new PrimaveraApiErrorException($"Error fetching the stock from primavera: {stock.Message}");
        }

        // Primavera, even though it might throw an error and not return anything, an
        // object on the list is always created, so we must check for 1
        if (stock.Data.Count <= 1)
        {
            throw new ResourceEmptyException("Fetched stock from primavera came empty");
        }

        return stock.Data;
    }

    public static async Task<List<PrimaveraProductStockWarehouseItem>> GetAvailableStockForProductsWarehouse()
    {
        PrimaveraListResponseItem<PrimaveraProductStockWarehouseItem> stock = await Primavera.GetListAsync<PrimaveraProductStockWarehouseItem>(
            ConfigManager.PrimaveraUrls.StockDisponivelArtigoArmazem,
            9999999,
            0,
            "Armazem=\'\'CAB\'\'"
        );

        if (stock.IsError)
        {
            throw new PrimaveraApiErrorException($"Error fetching the stock from primavera: {stock.Message}");
        }

        // Primavera, even though it might throw an error and not return anything, an
        // object on the list is always created, so we must check for 1
        if (stock.Data.Count <= 1)
        {
            throw new ResourceEmptyException("Fetched stock from primavera came empty");
        }

        return stock.Data;
    }

    public static async Task<Dictionary<string, PrimaveraProductStockWarehouseItem>> GetProductStockByProductCodeWarehouseHashed()
    {
        List<PrimaveraProductStockWarehouseItem> stocks = await GetAvailableStockForProductsWarehouse();

        Dictionary<string, PrimaveraProductStockWarehouseItem> stocksByProductCode = [];
        foreach (PrimaveraProductStockWarehouseItem stock in stocks)
        {
            stocksByProductCode[stock.Artigo] = stock;
        }

        return stocksByProductCode;
    }

    public static async Task<Dictionary<string, List<PrimaveraProductStockItem>>> GetProductStockByProductCodeHashed()
    {
        List<PrimaveraProductStockItem> stocks = await GetAvailableStockForProducts();

        Dictionary<string, List<PrimaveraProductStockItem>> stocksByProductCode = [];
        foreach (PrimaveraProductStockItem stock in stocks)
        {
            if (!stocksByProductCode.TryGetValue(stock.Artigo, out List<PrimaveraProductStockItem>? currentStocks))
            {
                currentStocks = [];
                stocksByProductCode[stock.Artigo] = currentStocks;
            }

            currentStocks.Add(stock);
        }

        return stocksByProductCode;
    }

    public static async Task<Dictionary<string, PrimaveraProductStockItem>> GetProductStockByProductCodeByUnitHashed()
    {
        List<PrimaveraProductStockItem> stocks = await GetAvailableStockForProducts();

        Dictionary<string, PrimaveraProductStockItem> stocksByProductCode = [];
        foreach (PrimaveraProductStockItem stock in stocks)
        {
            string key = GetProductStockByProductCodeByUnitHashedKey(stock.Artigo, stock.UnidadeBase);

            stock.StkDisponivel = Math.Round(stock.StkDisponivel, 2);

            stocksByProductCode[key] = stock;
        }

        return stocksByProductCode;
    }

    public static string GetProductStockByProductCodeByUnitHashedKey(string productCode, string unit)
    {
        return $"{productCode}-{unit}";
    }
}
