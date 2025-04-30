// // Copyright (c) 2024 Engibots. All rights reserved.

namespace engimatrix.ModelObjs
{
    public class ProductCatalogItem
    {
        public int id { get; set; }
        public string product_code { get; set; }
        public string description { get; set; }
        public int type_id { get; set; }
        public int material_id { get; set; }
        public int shape_id { get; set; }
        public int finishing_id { get; set; }
        public int surface_id { get; set; }
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
        public string family_id { get; set; }
        public decimal price_ref_market { get; set; }
        public string description_full { get; set; }
        public string nominal_dimension { get; set; }
        public int pricing_strategy_id { get; set; }
        public DateTime created_at { get; set; }
        public string created_by { get; set; }
        public DateTime? updated_at { get; set; }
        public string updated_by { get; set; }
        public bool is_pricing_ready { get; set; }

        public ProductCatalogItem()
        { }

        public override string ToString()
        {
            return $"ProductCatalogItem:\n" +
                $"id: {id}\n" +
                $"product_code: {product_code}\n" +
                $"description: {description}\n" +
                $"unit: {unit}\n" +
                $"stock_current: {stock_current}\n" +
                $"currency: {currency}\n" +
                $"price_pvp: {price_pvp}\n" +
                $"price_avg: {price_avg}\n" +
                $"price_last: {price_last}\n" +
                $"date_last_entry: {date_last_entry}\n" +
                $"date_last_exit: {date_last_exit}\n" +
                $"family_id: {family_id}\n" +
                $"price_ref_market: {price_ref_market}\n";
        }
    }

    public class ProductCatalogItemBuilder
    {
        private readonly ProductCatalogItem _productCatalogItem = new();

        public ProductCatalogItemBuilder SetId(int id)
        {
            _productCatalogItem.id = id;
            return this;
        }

        public ProductCatalogItemBuilder SetProductCode(string productCode)
        {
            _productCatalogItem.product_code = productCode;
            return this;
        }

        public ProductCatalogItemBuilder SetDescription(string description)
        {
            _productCatalogItem.description = description;
            return this;
        }

        public ProductCatalogItemBuilder SetTypeId(int typeId)
        {
            _productCatalogItem.type_id = typeId;
            return this;
        }

        public ProductCatalogItemBuilder SetMaterialId(int materialId)
        {
            _productCatalogItem.material_id = materialId;
            return this;
        }

        public ProductCatalogItemBuilder SetShapeId(int shapeId)
        {
            _productCatalogItem.shape_id = shapeId;
            return this;
        }

        public ProductCatalogItemBuilder SetFinishingId(int finishingId)
        {
            _productCatalogItem.finishing_id = finishingId;
            return this;
        }

        public ProductCatalogItemBuilder SetSurfaceId(int surfaceId)
        {
            _productCatalogItem.surface_id = surfaceId;
            return this;
        }

        public ProductCatalogItemBuilder SetLength(decimal length)
        {
            _productCatalogItem.length = length;
            return this;
        }

        public ProductCatalogItemBuilder SetWidth(decimal width)
        {
            _productCatalogItem.width = width;
            return this;
        }

        public ProductCatalogItemBuilder SetHeight(decimal height)
        {
            _productCatalogItem.height = height;
            return this;
        }

        public ProductCatalogItemBuilder SetThickness(decimal thickness)
        {
            _productCatalogItem.thickness = thickness;
            return this;
        }

        public ProductCatalogItemBuilder SetDiameter(decimal diameter)
        {
            _productCatalogItem.diameter = diameter;
            return this;
        }

        public ProductCatalogItemBuilder SetUnit(string unit)
        {
            _productCatalogItem.unit = unit;
            return this;
        }

        public ProductCatalogItemBuilder SetStockCurrent(decimal stockCurrent)
        {
            _productCatalogItem.stock_current = stockCurrent;
            return this;
        }

        public ProductCatalogItemBuilder SetCurrency(string currency)
        {
            _productCatalogItem.currency = currency;
            return this;
        }

        public ProductCatalogItemBuilder SetPricePvp(decimal pricePvp)
        {
            _productCatalogItem.price_pvp = pricePvp;
            return this;
        }

        public ProductCatalogItemBuilder SetPriceAvg(decimal priceAvg)
        {
            _productCatalogItem.price_avg = priceAvg;
            return this;
        }

        public ProductCatalogItemBuilder SetPriceLast(decimal priceLast)
        {
            _productCatalogItem.price_last = priceLast;
            return this;
        }

        public ProductCatalogItemBuilder SetDateLastEntry(DateOnly dateLastEntry)
        {
            _productCatalogItem.date_last_entry = dateLastEntry;
            return this;
        }

        public ProductCatalogItemBuilder SetDateLastExit(DateOnly dateLastExit)
        {
            _productCatalogItem.date_last_exit = dateLastExit;
            return this;
        }

        public ProductCatalogItemBuilder SetFamilyId(string familyId)
        {
            _productCatalogItem.family_id = familyId;
            return this;
        }

        public ProductCatalogItemBuilder SetPriceRefMarket(decimal priceRefMarket)
        {
            _productCatalogItem.price_ref_market = priceRefMarket;
            return this;
        }

        public ProductCatalogItemBuilder SetNominalDimension(string nominalDimension)
        {
            _productCatalogItem.nominal_dimension = nominalDimension;
            return this;
        }

        public ProductCatalogItemBuilder SetPricingStrategyId(int pricingStrategyId)
        {
            _productCatalogItem.pricing_strategy_id = pricingStrategyId;
            return this;
        }

        public ProductCatalogItemBuilder SetDescriptionFull(string descriptionFull)
        {
            _productCatalogItem.description_full = descriptionFull;
            return this;
        }

        public ProductCatalogItemBuilder SetIsPricingReady(bool isPricingReady)
        {
            _productCatalogItem.is_pricing_ready = isPricingReady;
            return this;
        }

        public ProductCatalogItemBuilder SetCreatedAt(DateTime createdAt)
        {
            _productCatalogItem.created_at = createdAt;
            return this;
        }

        public ProductCatalogItemBuilder SetCreatedBy(string createdBy)
        {
            _productCatalogItem.created_by = createdBy;
            return this;
        }

        public ProductCatalogItemBuilder SetUpdatedAt(DateTime? updatedAt)
        {
            _productCatalogItem.updated_at = updatedAt;
            return this;
        }

        public ProductCatalogItemBuilder SetUpdatedBy(string updatedBy)
        {
            _productCatalogItem.updated_by = updatedBy;
            return this;
        }

        public ProductCatalogItem Build()
        {
            return _productCatalogItem;
        }
    }
}
