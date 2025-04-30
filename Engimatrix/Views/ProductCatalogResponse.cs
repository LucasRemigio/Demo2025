// // Copyright (c) 2024 Engibots. All rights reserved.

using engimatrix.ModelObjs;
using engimatrix.Models;
using engimatrix.ResponseMessages;

namespace engimatrix.Views
{
    public class ProductCatalogListResponse : BaseResponse
    {
        public List<ProductCatalogDTO> product_catalogs { get; set; }

        public ProductCatalogListResponse(List<ProductCatalogDTO> productCatalogs, int result_code, string language) : base(result_code, language)
        {
            this.product_catalogs = productCatalogs;
        }

        public ProductCatalogListResponse(int result_code, string language) : base(result_code, language)
        {
            this.product_catalogs = [];
        }
}

public class ProductCatalogItemResponse : BaseResponse
{
    public ProductCatalogDTO product_catalog { get; set; }

    public ProductCatalogItemResponse(ProductCatalogDTO productCatalog, int result_code, string language) : base(result_code, language)
    {
        this.product_catalog = productCatalog;
    }

    public ProductCatalogItemResponse(int result_code, string language) : base(result_code, language)
    {
        this.product_catalog = new();
    }
}

public class ProductCatalogListResponseNoAuth : BaseResponse
{
    public List<ProductCatalogDTONoAuth> product_catalogs { get; set; }

    public ProductCatalogListResponseNoAuth(List<ProductCatalogDTONoAuth> productCatalogs, int result_code, string language)
        : base(result_code, language)
    {
        this.product_catalogs = productCatalogs;
    }

    public ProductCatalogListResponseNoAuth(int result_code, string language)
        : base(result_code, language)
    {
        this.product_catalogs = [];
    }
}
}
