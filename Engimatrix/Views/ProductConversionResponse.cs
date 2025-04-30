// // Copyright (c) 2024 Engibots. All rights reserved.

using engimatrix.ModelObjs;
using engimatrix.Models;
using engimatrix.ResponseMessages;

namespace engimatrix.Views
{
    public class ProductConversionListResponse : BaseResponse
    {
        public List<ProductConversionItem>? product_conversions { get; set; }

        public ProductConversionListResponse(List<ProductConversionItem> product_conversions, int result_code, string language) : base(result_code, language)
        {
            this.product_conversions = product_conversions;
        }

        public ProductConversionListResponse(int result_code, string language) : base(result_code, language)
        {
        }
    }

    public class ProductConversionDtoListResponse : BaseResponse
    {
        public List<ProductConversionDTO>? product_conversions { get; set; }

        public ProductConversionDtoListResponse(List<ProductConversionDTO> conversions, int result_code, string language) : base(result_code, language)
        {
            this.product_conversions = conversions;
        }

        public ProductConversionDtoListResponse(int result_code, string language) : base(result_code, language)
        {
        }
    }

    public class ProductConversionItemResponse : BaseResponse
    {
        public ProductConversionItem? product_conversion { get; set; }

        public ProductConversionItemResponse(ProductConversionItem product_conversion, int result_code, string language) : base(result_code, language)
        {
            this.product_conversion = product_conversion;
        }

        public ProductConversionItemResponse(int result_code, string language) : base(result_code, language)
        {
        }
    }
}
