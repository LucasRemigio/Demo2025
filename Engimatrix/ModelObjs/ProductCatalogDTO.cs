// // Copyright (c) 2024 Engibots. All rights reserved.

using System.Diagnostics.Metrics;
using Smartsheet.Api.Models;

namespace engimatrix.ModelObjs;
public class ProductCatalogDTO
{
    public int id { get; set; }
    public string product_code { get; set; }
    public string description { get; set; }
    public string description_full { get; set; }
    public ProductTypeItem? type { get; set; }
    public ProductShapeItem? shape { get; set; }
    public ProductMaterialItem? material { get; set; }
    public ProductFinishingItem? finishing { get; set; }
    public ProductSurfaceItem? surface { get; set; }
    public decimal length { get; set; }
    public decimal width { get; set; }
    public decimal height { get; set; }
    public decimal thickness { get; set; }
    public decimal diameter { get; set; }
    public string unit { get; set; }
    public decimal stock_current { get; set; }
    public string currency { get; set; }
    public decimal price_pvp { get; set; }
    public decimal price_avg { get; set; }
    public decimal price_last { get; set; }
    public DateOnly date_last_entry { get; set; }
    public DateOnly date_last_exit { get; set; }
    public ProductFamilyItem family { get; set; }
    public decimal price_ref_market { get; set; }
    public string nominal_dimension { get; set; }
    public PricingStrategyItem pricing_strategy { get; set; }
    public DateTime created_at { get; set; }
    public string created_by { get; set; }
    public DateTime? updated_at { get; set; }
    public string? updated_by { get; set; }
    public bool is_pricing_ready { get; set; }
    public List<ProductConversionDTO> product_conversions { get; set; }

    public ProductCatalogDTO(int id, string productCode, string description, string descriptionFull, ProductTypeItem productType, ProductShapeItem productShape,
        ProductMaterialItem productMaterial, ProductFinishingItem productFinishing, ProductSurfaceItem surface, decimal length, decimal width, decimal height, decimal thickness, decimal diameter,
        string unit, decimal stockCurrent, string currency, decimal pricePvp, decimal priceAvg, decimal priceLast, DateOnly dateLastEntry,
        DateOnly dateLastExit, ProductFamilyItem productFamily, decimal priceRefMarket)
    {
        this.id = id;
        this.product_code = productCode;
        this.description = description;
        this.description_full = descriptionFull;
        this.type = productType;
        this.shape = productShape;
        this.material = productMaterial;
        this.finishing = productFinishing;
        this.length = length;
        this.width = width;
        this.height = height;
        this.thickness = thickness;
        this.diameter = diameter;
        this.unit = unit;
        this.stock_current = stockCurrent;
        this.currency = currency;
        this.price_pvp = pricePvp;
        this.price_avg = priceAvg;
        this.price_last = priceLast;
        this.date_last_entry = dateLastEntry;
        this.date_last_exit = dateLastExit;
        this.family = productFamily;
        this.price_ref_market = priceRefMarket;
        this.surface = surface;
    }

    public ProductCatalogDTO(int id, string description, string productCode, decimal pricePvp)
    {
        this.id = id;
        this.description = description;
        this.product_code = productCode;
        this.price_pvp = pricePvp;
    }

    public ProductCatalogDTO()
    { }

    public override string ToString()
    {
        return $"ProductCatalogDTO:\n" +
            $"id: {id}\n" +
            $"product_code: {product_code}\n" +
            $"description: {description}\n" +
            $"description_full: {description_full}\n" +
            $"unit: {unit}\n" +
            $"stock_current: {stock_current}\n" +
            $"currency: {currency}\n" +
            $"price_pvp: {price_pvp}\n" +
            $"price_avg: {price_avg}\n" +
            $"price_last: {price_last}\n" +
            $"date_last_entry: {date_last_entry}\n" +
            $"date_last_exit: {date_last_exit}\n" +
            $"family_id: {family.id}\n" +
            $"price_ref_market: {price_ref_market}\n";
    }

    public override bool Equals(object obj)
    {
        if (obj is not ProductCatalogDTO other)
        {
            return false;
        }

        return description == other.description &&
                length == other.length &&
                width == other.width &&
                height == other.height &&
                thickness == other.thickness &&
                diameter == other.diameter &&
                type?.id == other.type?.id &&
                shape?.id == other.shape?.id &&
                material?.id == other.material?.id &&
                finishing?.id == other.finishing?.id &&
                surface?.id == other.surface?.id;
    }

    public override int GetHashCode()
    {
        return HashCode.Combine(
            HashCode.Combine(
                description,
                length,
                width,
                height,
                thickness,
                diameter
            ),
            HashCode.Combine(
                type?.id,
                shape?.id,
                material?.id,
                finishing?.id,
                surface?.id
            )
        );
    }
}

public class ProductCatalogDTOBuilder
{
    private ProductCatalogDTO _productCatalogDTO = new();

    public ProductCatalogDTOBuilder SetId(int id)
    {
        _productCatalogDTO.id = id;
        return this;
    }

    public ProductCatalogDTOBuilder SetProductCode(string productCode)
    {
        _productCatalogDTO.product_code = productCode;
        return this;
    }

    public ProductCatalogDTOBuilder SetDescription(string description)
    {
        _productCatalogDTO.description = description;
        return this;
    }

    public ProductCatalogDTOBuilder SetDescriptionFull(string descriptionFull)
    {
        _productCatalogDTO.description_full = descriptionFull;
        return this;
    }

    public ProductCatalogDTOBuilder SetType(ProductTypeItem? type)
    {
        _productCatalogDTO.type = type;
        return this;
    }

    public ProductCatalogDTOBuilder SetShape(ProductShapeItem? shape)
    {
        _productCatalogDTO.shape = shape;
        return this;
    }

    public ProductCatalogDTOBuilder SetMaterial(ProductMaterialItem? material)
    {
        _productCatalogDTO.material = material;
        return this;
    }

    public ProductCatalogDTOBuilder SetFinishing(ProductFinishingItem? finishing)
    {
        _productCatalogDTO.finishing = finishing;
        return this;
    }

    public ProductCatalogDTOBuilder SetSurface(ProductSurfaceItem? surface)
    {
        _productCatalogDTO.surface = surface;
        return this;
    }

    public ProductCatalogDTOBuilder SetLength(decimal length)
    {
        _productCatalogDTO.length = length;
        return this;
    }

    public ProductCatalogDTOBuilder SetWidth(decimal width)
    {
        _productCatalogDTO.width = width;
        return this;
    }

    public ProductCatalogDTOBuilder SetHeight(decimal height)
    {
        _productCatalogDTO.height = height;
        return this;
    }

    public ProductCatalogDTOBuilder SetThickness(decimal thickness)
    {
        _productCatalogDTO.thickness = thickness;
        return this;
    }

    public ProductCatalogDTOBuilder SetDiameter(decimal diameter)
    {
        _productCatalogDTO.diameter = diameter;
        return this;
    }

    public ProductCatalogDTOBuilder SetUnit(string unit)
    {
        _productCatalogDTO.unit = unit;
        return this;
    }

    public ProductCatalogDTOBuilder SetStockCurrent(decimal stockCurrent)
    {
        _productCatalogDTO.stock_current = stockCurrent;
        return this;
    }

    public ProductCatalogDTOBuilder SetCurrency(string currency)
    {
        _productCatalogDTO.currency = currency;
        return this;
    }

    public ProductCatalogDTOBuilder SetPricePvp(decimal pricePvp)
    {
        _productCatalogDTO.price_pvp = pricePvp;
        return this;
    }

    public ProductCatalogDTOBuilder SetPriceAvg(decimal priceAvg)
    {
        _productCatalogDTO.price_avg = priceAvg;
        return this;
    }

    public ProductCatalogDTOBuilder SetPriceLast(decimal priceLast)
    {
        _productCatalogDTO.price_last = priceLast;
        return this;
    }

    public ProductCatalogDTOBuilder SetDateLastEntry(DateOnly dateLastEntry)
    {
        _productCatalogDTO.date_last_entry = dateLastEntry;
        return this;
    }

    public ProductCatalogDTOBuilder SetDateLastExit(DateOnly dateLastExit)
    {
        _productCatalogDTO.date_last_exit = dateLastExit;
        return this;
    }

    public ProductCatalogDTOBuilder SetFamily(ProductFamilyItem family)
    {
        _productCatalogDTO.family = family;
        return this;
    }

    public ProductCatalogDTOBuilder SetPriceRefMarket(decimal priceRefMarket)
    {
        _productCatalogDTO.price_ref_market = priceRefMarket;
        return this;
    }

    public ProductCatalogDTOBuilder SetNominalDimension(string nominalDimension)
    {
        _productCatalogDTO.nominal_dimension = nominalDimension;
        return this;
    }

    public ProductCatalogDTOBuilder SetPricingStrategy(PricingStrategyItem pricingStrategy)
    {
        _productCatalogDTO.pricing_strategy = pricingStrategy;
        return this;
    }

    public ProductCatalogDTOBuilder SetProductConversions(List<ProductConversionDTO> productConversions)
    {
        _productCatalogDTO.product_conversions = productConversions;
        return this;
    }

    public ProductCatalogDTOBuilder SetIsPricingReady(bool isPricingReady)
    {
        _productCatalogDTO.is_pricing_ready = isPricingReady;
        return this;
    }

    public ProductCatalogDTOBuilder SetCreatedAt(DateTime createdAt)
    {
        _productCatalogDTO.created_at = createdAt;
        return this;
    }

    public ProductCatalogDTOBuilder SetCreatedBy(string createdBy)
    {
        _productCatalogDTO.created_by = createdBy;
        return this;
    }

    public ProductCatalogDTOBuilder SetUpdatedAt(DateTime? updatedAt)
    {
        _productCatalogDTO.updated_at = updatedAt;
        return this;
    }

    public ProductCatalogDTOBuilder SetUpdatedBy(string? updatedBy)
    {
        _productCatalogDTO.updated_by = updatedBy;
        return this;
    }

    public ProductCatalogDTO Build()
    {
        return _productCatalogDTO;
    }

    public ProductCatalogDTOBuilder Reset()
    {
        _productCatalogDTO = new ProductCatalogDTO();
        return this; // For chaining
    }
}

public class ProductCatalogDTONoAuth
{
    public int id { get; set; }
    public string product_code { get; set; }
    public string description { get; set; }
    public string description_full { get; set; }
    public ProductTypeItem? type { get; set; }
    public ProductShapeItem? shape { get; set; }
    public ProductMaterialItem? material { get; set; }
    public ProductFinishingItem? finishing { get; set; }
    public ProductSurfaceItem? surface { get; set; }
    public decimal length { get; set; }
    public decimal width { get; set; }
    public decimal height { get; set; }
    public decimal thickness { get; set; }
    public decimal diameter { get; set; }
    public string unit { get; set; }
    public ProductFamilyItem family { get; set; }
    public List<ProductConversionDTO> product_conversions { get; set; }
}