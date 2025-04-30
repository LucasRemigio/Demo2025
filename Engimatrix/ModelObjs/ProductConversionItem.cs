// // Copyright (c) 2024 Engibots. All rights reserved.

namespace engimatrix.ModelObjs
{
    public class ProductConversionItem : ProductConversionBase
    {
        public int origin_unit_id { get; set; }
        public int end_unit_id { get; set; }

        public override string ToString()
        {
            return base.ToString() + $"origin_unit_id: {origin_unit_id}\n, end_unit_id: {end_unit_id}\n";
        }
    }

    public class ProductConversionItemBuilder
    {
        private readonly ProductConversionItem _productConversionItem = new();

        public ProductConversionItemBuilder SetId(int id)
        {
            _productConversionItem.id = id;
            return this;
        }

        public ProductConversionItemBuilder SetProductCode(string productCode)
        {
            _productConversionItem.product_code = productCode;
            return this;
        }

        public ProductConversionItemBuilder SetOriginUnitId(int originUnitId)
        {
            _productConversionItem.origin_unit_id = originUnitId;
            return this;
        }

        public ProductConversionItemBuilder SetEndUnitId(int endUnitId)
        {
            _productConversionItem.end_unit_id = endUnitId;
            return this;
        }

        public ProductConversionItemBuilder SetRate(decimal rate)
        {
            _productConversionItem.rate = rate;
            return this;
        }

        public ProductConversionItem Build()
        {
            return _productConversionItem;
        }
    }
}