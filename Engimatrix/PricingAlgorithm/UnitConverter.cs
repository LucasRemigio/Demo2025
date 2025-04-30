// // Copyright (c) 2024 Engibots. All rights reserved.

using engimatrix.ModelObjs;
using Engimatrix.ModelObjs;
using engimatrix.Models;
using engimatrix.Utils;
using System.Text.RegularExpressions;
using System.Text;
using engimatrix.ModelObjs.Primavera;
using engimatrix.Config;
using System.Globalization;
using System.Linq.Expressions;
using engimatrix.Connector;
using Engimatrix.Controllers;
using engimatrix.Exceptions;

namespace engimatrix.PricingAlgorithm;

public static class UnitConverter
{
    public static decimal? CalculateConversionRate(string requestedUnit, ProductCatalogItem productCatalog)
    {
        ProductUnitItem originSizeUnit = GetValidUnit(requestedUnit);
        string originUnit = originSizeUnit.abbreviation.ToUpper(CultureInfo.InvariantCulture);

        // If the units are the same, the conversion rate is 1
        if (originUnit.Equals(productCatalog.unit, StringComparison.OrdinalIgnoreCase))
        {
            return 1;
        }

        // get the conversion from our table
        ProductConversionItem? productConversion = ProductConversionModel.GetProductConversionByAbbreviations(productCatalog.product_code, originUnit, productCatalog.unit, "System");
        if (productConversion != null)
        {
            Log.Info($"Conversion rate found in database: {productConversion.rate}");
            return productConversion.rate;
        }

        Log.Debug("Conversion rate not found in database. Calculating the product weight manually...");
        // if conversion rate not found, try calculating manually
        decimal productUnitWeight = GetProductUnitWeight(productCatalog);
        decimal? conversionRate = null;

        // if the client wants 1 unit, and the database is in meters, we can convert directly through the length
        if (requestedUnit.Equals(ProductUnitConstants.Unit.UN.ToString(), StringComparison.OrdinalIgnoreCase) &&
            productCatalog.unit.Equals(ProductUnitConstants.Unit.MT.ToString(), StringComparison.OrdinalIgnoreCase))
        {
            conversionRate = productCatalog.length / 1000;
            return conversionRate;
        }

        // if the client wants in meters, and the database is in units, we can divide by length to get the units
        if (requestedUnit.Equals(ProductUnitConstants.Unit.MT.ToString(), StringComparison.OrdinalIgnoreCase) &&
            productCatalog.unit.Equals(ProductUnitConstants.Unit.UN.ToString(), StringComparison.OrdinalIgnoreCase))
        {
            conversionRate = 1 / productCatalog.length * 1000;
            return conversionRate;
        }

        // if the client wants in meters, and the database is in kg, we can divide by weight to get the units

        return conversionRate;
    }

    public static decimal GetProductUnitWeight(ProductCatalogItem product)
    {
        // product weight formula: weight = length * width * thickness * density
        // This works if the product is square, or a rectangle
        decimal density = 7.8m;
        decimal productUnitWeight = product.length * product.width * product.height * density;

        // if it is a TUBO or VARAO, we need to calculate using the cilinder formula, and then remove the inner diameter
        List<int> tuboFamilies = [ProductFamilyConstants.ProductFamilyCode.TUBO_CANALIZACAO];
        if (product.family_id.Equals(ProductFamilyConstants.ProductFamilyCode.TUBO_CANALIZACAO.ToString(), StringComparison.OrdinalIgnoreCase))
        {
            // TODO
        }

        // If the product is a VIGA, we need to calculate the weight using the formula for a beam

        return productUnitWeight;
    }

    public static ProductUnitItem GetValidUnit(string unit)
    {
        List<ProductUnitItem> possibleSizes = ProductUnitModel.GetProductUnits("System");
        ProductUnitItem defaultProductUnit = possibleSizes.Where(s => s.abbreviation.Equals("UN", StringComparison.OrdinalIgnoreCase)).First();

        if (string.IsNullOrEmpty(unit))
        {
            Log.Error("Unit is empty, converting to Unit");
            return defaultProductUnit;
        }

        string originUnit = unit.ToUpper(CultureInfo.InvariantCulture);

        ProductUnitItem? unitAvailable = possibleSizes.Where(s => s.abbreviation.Equals(originUnit, StringComparison.OrdinalIgnoreCase)).FirstOrDefault();
        if (unitAvailable != null)
        {
            Log.Debug($"Unit {originUnit} is a valid unit");
            return unitAvailable;
        }

        Log.Error($"Unit {originUnit} is not a valid unit");

        ProductUnitConstants.Unit finalUnit = originUnit switch
        {
            string u when u.StartsWith("UN", StringComparison.OrdinalIgnoreCase) => ProductUnitConstants.Unit.UN,
            string u when u.StartsWith("K", StringComparison.OrdinalIgnoreCase) => ProductUnitConstants.Unit.KG,
            string u when u.StartsWith("Q", StringComparison.OrdinalIgnoreCase) => ProductUnitConstants.Unit.KG,
            string u when u.Contains('2', StringComparison.OrdinalIgnoreCase) => ProductUnitConstants.Unit.M2,
            string u when u.Contains('L', StringComparison.OrdinalIgnoreCase) => ProductUnitConstants.Unit.ML,
            string u when u.StartsWith("M", StringComparison.OrdinalIgnoreCase) => ProductUnitConstants.Unit.MT,
            string u when u.StartsWith("R", StringComparison.OrdinalIgnoreCase) => ProductUnitConstants.Unit.RL,
            _ => ProductUnitConstants.Unit.UN,
        };

        Log.Debug($"Converted unit {originUnit} to {finalUnit}");
        ProductUnitItem finalProductUnit = possibleSizes.Where(s => s.abbreviation.Equals(finalUnit.ToString(), StringComparison.OrdinalIgnoreCase)).First();
        return finalProductUnit;
    }

    public static decimal GetProductConversionToMeters(string productCode, int originUnitId)
    {
        int endUnitId = (int)ProductUnitConstants.Unit.MT;
        if (originUnitId == endUnitId)
        {
            return 1;
        }

        // get the product with the product code
        List<ProductCatalogItem> productCatalogs = ProductCatalogModel.GetProductCatalogsByCode(productCode, "System");
        if (productCatalogs.Count == 0)
        {
            Log.Error("No product found with code");
            return 0;
        }

        ProductCatalogItem productCatalog = productCatalogs[0];

        if (originUnitId == (int)ProductUnitConstants.Unit.UN)
        {
            return productCatalog.length / 1000;
        }

        // get the available conversions in the database
        List<ProductConversionItem> productConversions = ProductConversionModel.GetProductConversionsByCode(productCode, "System");
        if (productConversions.Count == 0)
        {
            Log.Error("No conversions found for product");
            return 0;
        }

        // check if any of the conversion is for meters
        ProductConversionItem? productConversion = productConversions.Where(c => c.origin_unit_id == originUnitId && c.end_unit_id == endUnitId).FirstOrDefault();
        if (productConversion != null)
        {
            // if it is, we have incredible luck :)
            return (decimal)productConversion.rate;
        }

        // if not (most likely)
        // Check if there is a conversion for units
        productConversion = productConversions.Where(c => c.origin_unit_id == originUnitId && c.end_unit_id == (int)ProductUnitConstants.Unit.UN).FirstOrDefault();

        if (productConversion != null)
        {
            // if it is, we have incredible luck :)
            // logic...
            return productCatalog.length / 1000 * (decimal)productConversion.rate;
        }

        return 0;
    }
}
