// // Copyright (c) 2024 Engibots. All rights reserved.

using System.Diagnostics;
using engimatrix.Exceptions;
using engimatrix.ModelObjs;
using engimatrix.ModelObjs.Primavera;
using engimatrix.PricingAlgorithm;
using engimatrix.Utils;
using engimatrix.Views;

namespace engimatrix.Models;

public static class ProductConversionPrimaveraModel
{

    public async static Task<SyncPrimaveraStats> SyncPrimavera(string execute_user)
    {
        Log.Debug("Syncing the conversions....");
        Stopwatch stopwatch = new();
        stopwatch.Start();
        int numSyncs = 0;

        /*
        * GET OUR CURRENT CONVERSIONS 
        */
        List<ProductConversionDTO> productConversions = ProductConversionModel.GetProductConversionsDTO(execute_user);
        Dictionary<string, List<ProductConversionDTO>> productConversionsHash = ProductConversionModel.HashProductConversionByCode(productConversions);

        /*
        * GET PRIMAVERA CONVERSIONS
        */
        List<PrimaveraUnitConversionItem> primaveraConversions = await UnitConverter.GetPrimaveraConversions();

        /*
        * GET PRODUCT CATALOGS FOR EXISTENCE CHECK
        */
        Dictionary<string, string> productCodesDic = ProductCatalogModel.GetProductCodesDictionary(execute_user);

        /*
        * SYNC
        */
        foreach (PrimaveraUnitConversionItem primaveraConversion in primaveraConversions)
        {
            // Check if we have any conversion rate with given Artigo
            if (!productConversionsHash.TryGetValue(primaveraConversion.Artigo, out List<ProductConversionDTO>? productConversionsList))
            {
                // we do not have any conversion rate with given Artigo
                if (!productCodesDic.ContainsKey(primaveraConversion.Artigo))
                {
                    // product does not exist, so we do not need to create the conversion
                    continue;
                }
                // product exists, so we need to create the conversion
                bool success = CreatePrimaveraConversionRate(primaveraConversion, execute_user);
                if (success)
                {
                    numSyncs++;
                }
                continue;
            }

            // we do have it, compare to see if anything changed
            ProductConversionDTO? productConversion = productConversionsList?.Find(pc => CompareProductConversionWithPrimavera(primaveraConversion, pc));

            if (productConversion == null)
            {
                // we do not have the product conversion, so create it
                // check if product exists 
                if (!productCodesDic.ContainsKey(primaveraConversion.Artigo))
                {
                    // product does not exist, so we do not need to create the conversion
                    continue;
                }
                // product exists, so we need to create the conversion
                bool success = CreatePrimaveraConversionRate(primaveraConversion, execute_user);
                if (success)
                {
                    numSyncs++;
                }
                continue;
            }

            // check if rate changed
            decimal primaveraRate = Convert.ToDecimal(primaveraConversion.FactorConversao);
            if (Math.Abs(productConversion.rate - primaveraRate) < 0.0001m)
            {
                continue;
            }

            Log.Debug($"Rate changed for product: {primaveraConversion.Artigo} from {productConversion.rate} to {primaveraConversion.FactorConversao}");
            ProductConversionModel.PatchRate(productConversion.id, primaveraConversion.FactorConversao, execute_user);
            numSyncs++;
            continue;
        }

        stopwatch.Stop();
        Log.Debug($"Syncing the conversions done! Elapsed time: {stopwatch.ElapsedMilliseconds}ms");
        SyncPrimaveraStats stats = new(stopwatch.ElapsedMilliseconds, numSyncs);
        return stats;
    }

    public static bool CompareProductConversionWithPrimavera(PrimaveraUnitConversionItem primaveraConversion, ProductConversionDTO productConversion)
    {
        if (!primaveraConversion.Artigo.Equals(productConversion.product_code, StringComparison.OrdinalIgnoreCase))
        {
            return false;
        }

        if (!primaveraConversion.UnidadeOrigem.Equals(productConversion.origin_unit.abbreviation, StringComparison.OrdinalIgnoreCase))
        {
            return false;
        }

        if (!primaveraConversion.UnidadeDestino.Equals(productConversion.end_unit.abbreviation, StringComparison.OrdinalIgnoreCase))
        {
            return false;
        }

        return true;
    }


    public static bool CreatePrimaveraConversionRate(
        PrimaveraUnitConversionItem primaveraConversion,
        string execute_user
    )
    {
        Dictionary<string, ProductUnitItem> unitsHashed = ProductUnitModel.GetHashedProductUnitsByAbbreviation(execute_user);

        // get the abbreviation id
        if (!unitsHashed.TryGetValue(primaveraConversion.UnidadeOrigem, out ProductUnitItem? originUnit))
        {
            Log.Error("Unit not found for origin unit abbreviation: " + primaveraConversion.UnidadeOrigem);
            return false;
        }

        if (!unitsHashed.TryGetValue(primaveraConversion.UnidadeDestino, out ProductUnitItem? endUnit))
        {
            Log.Error("Unit not found for end unit abbreviation: " + primaveraConversion.UnidadeDestino);
            return false;
        }

        decimal rate = Convert.ToDecimal(primaveraConversion.FactorConversao);

        ProductConversionItem productToCreate = new ProductConversionItemBuilder()
            .SetProductCode(primaveraConversion.Artigo)
            .SetOriginUnitId(originUnit.id)
            .SetEndUnitId(endUnit.id)
            .SetRate(rate)
            .Build();

        ProductConversionModel.CreateProductConversion(productToCreate, execute_user);

        return true;
    }

}
