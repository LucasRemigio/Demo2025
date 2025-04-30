// // Copyright (c) 2024 Engibots. All rights reserved.

namespace engimatrix.ModelObjs
{
    public class ProductConversionDTO : ProductConversionBase
    {
        public ProductUnitItem origin_unit { get; set; }
        public ProductUnitItem end_unit { get; set; }
        public string product_catalog_description { get; set; }
    }

    public class ProductConversionDTOBuilder
    {
        private readonly ProductConversionDTO _productConversionDto = new();

        public ProductConversionDTOBuilder SetId(int id)
        {
            _productConversionDto.id = id;
            return this;
        }

        public ProductConversionDTOBuilder SetProductCode(string productCode)
        {
            _productConversionDto.product_code = productCode;
            return this;
        }

        public ProductConversionDTOBuilder SetOriginUnit(ProductUnitItem originUnit)
        {
            _productConversionDto.origin_unit = originUnit;
            return this;
        }

        public ProductConversionDTOBuilder SetEndUnit(ProductUnitItem endUnit)
        {
            _productConversionDto.end_unit = endUnit;
            return this;
        }

        public ProductConversionDTOBuilder SetRate(decimal rate)
        {
            _productConversionDto.rate = rate;
            return this;
        }

        public ProductConversionDTOBuilder SetProductCatalogDescription(string productCatalogDescription)
        {
            _productConversionDto.product_catalog_description = productCatalogDescription;
            return this;
        }

        public ProductConversionDTO Build()
        {
            return _productConversionDto;
        }
    }
}