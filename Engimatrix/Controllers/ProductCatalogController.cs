// // Copyright (c) 2024 Engibots. All rights reserved.

using engimatrix.Config;
using engimatrix.Filters;
using engimatrix.ResponseMessages;
using engimatrix.Views;
using Microsoft.AspNetCore.Mvc;
using engimatrix.Models;
using engimatrix.Utils;
using static engimatrix.Config.RatingConstants;
using engimatrix.Exceptions;
using engimatrix.ModelObjs;

namespace Engimatrix.Controllers;

[ApiController]
[Route("api/products/catalogs")]
public class ProductCatalogController : ControllerBase
{
    [HttpGet]
    [Route("")]
    [RequestLimit]
    [ValidateReferrer]
    public ActionResult<ProductCatalogListResponse> GetProductCatalogs([FromQuery] string? family_id, [FromQuery] bool? has_stock)
    {
        string language = this.Request.Headers["client-lang"];
        if (string.IsNullOrEmpty(language))
        {
            language = ConfigManager.defaultLanguage;
        }
        string token = this.Request.Headers["Authorization"];

        string executer_user = "Anonymous";
        if (!String.IsNullOrEmpty(token))
        {
            executer_user = UserModel.GetUserByToken(token);
        }

        try
        {
            List<ProductCatalogDTO> products = ProductCatalogModel.GetProductCatalogsDTO(executer_user, family_id, has_stock, true);
            return new ProductCatalogListResponse(products, ResponseSuccessMessage.Success, language);
        }
        catch (Exception e)
        {
            Log.Error("GetProductCatalogs endpoint - Error - " + e);
            return new ProductCatalogListResponse(ResponseErrorMessage.InternalError, language);
        }
    }

    [HttpGet]
    [Route("no-auth/search")]
    [RequestLimit]
    [ValidateReferrer]
    public ActionResult<BaseResponse> SearchProductCatalogsNoAuth([FromQuery] string? query, [FromQuery] bool? has_stock)
    {
        string language = this.Request.Headers["client-lang"];
        if (string.IsNullOrEmpty(language))
        {
            language = ConfigManager.defaultLanguage;
        }
        string token = this.Request.Headers["Authorization"];
        string executer_user = "Anonymous";
        bool isAuthenticated = false;

        try
        {
            if (!string.IsNullOrEmpty(token))
            {
                executer_user = UserModel.GetUserByToken(token);
                isAuthenticated = true;
            }

            if (isAuthenticated)
            {
                List<ProductCatalogDTO> products = ProductCatalogModel.SearchProductCatalogs(query, has_stock, executer_user);
                return new ProductCatalogListResponse(products, ResponseSuccessMessage.Success, language);
            }
            else
            {
                List<ProductCatalogDTONoAuth> products = ProductCatalogModel.SearchProductCatalogsNoAuth(query, has_stock, executer_user);
                return new ProductCatalogListResponseNoAuth(products, ResponseSuccessMessage.Success, language);
            }
        }
        catch (Exception e)
        {
            Log.Error("SearchProductCatalogs endpoint - Error - " + e);
            return new ProductCatalogListResponseNoAuth(ResponseErrorMessage.InternalError, language);
        }
    }

    [HttpGet]
    [Route("no-auth")]
    [RequestLimit]
    [ValidateReferrer]
    public ActionResult<ProductCatalogListResponseNoAuth> GetProductCatalogsNoAuth([FromQuery] string? family_id, [FromQuery] bool? has_stock)
    {
        string language = this.Request.Headers["client-lang"];
        if (string.IsNullOrEmpty(language))
        {
            language = ConfigManager.defaultLanguage;
        }
        string token = this.Request.Headers["Authorization"];

        string executer_user = string.Empty;
        if (!string.IsNullOrEmpty(token))
        {
            executer_user = UserModel.GetUserByToken(token);
        }

        try
        {
            List<ProductCatalogDTONoAuth> products = ProductCatalogModel.GetProductCatalogsDTONoAuth(family_id, has_stock, true, executer_user);
            return new ProductCatalogListResponseNoAuth(products, ResponseSuccessMessage.Success, language);
        }
        catch (Exception e)
        {
            Log.Error("GetProductCatalogsNoAuth endpoint - Error - " + e);
            return new ProductCatalogListResponseNoAuth(ResponseErrorMessage.InternalError, language);
        }
    }

    [HttpGet]
    [Route("{productCatalogId}")]
    [RequestLimit]
    [ValidateReferrer]
    [Authorize]
    public ActionResult<ProductCatalogItemResponse> GetProductCatalogById(string productCatalogId)
    {
        string language = this.Request.Headers["client-lang"];
        if (string.IsNullOrEmpty(language))
        {
            language = ConfigManager.defaultLanguage;
        }
        string token = this.Request.Headers["Authorization"];
        string executer_user = UserModel.GetUserByToken(token);

        try
        {
            return new ProductCatalogItemResponse(ProductCatalogModel.GetProductCatalogsByIdDTO(productCatalogId, executer_user), ResponseSuccessMessage.Success, language);
        }
        catch (InputNotValidException e)
        {
            Log.Error("GetProductCatalogById endpoint - Error - " + e);
            return new ProductCatalogItemResponse(ResponseErrorMessage.InvalidArgs, language);
        }
        catch (Exception e)
        {
            Log.Error("GetProductCatalogById endpoint - Error - " + e);
            return new ProductCatalogItemResponse(ResponseErrorMessage.InternalError, language);
        }
    }

    [HttpPost]
    [Route("")]
    [RequestLimit]
    [ValidateReferrer]
    [Authorize]
    public ActionResult<ProductCatalogItemResponse> CreateProductCatalog([FromBody] ProductCatalogCreateRequest productCatalogCreateRequest)
    {
        string language = this.Request.Headers["client-lang"];
        if (string.IsNullOrEmpty(language))
        {
            language = ConfigManager.defaultLanguage;
        }
        string token = this.Request.Headers["Authorization"];
        string executer_user = UserModel.GetUserByToken(token);

        if (!productCatalogCreateRequest.IsValid())
        {
            return new ProductCatalogItemResponse(ResponseErrorMessage.InvalidArgs, language);
        }

        try
        {
            /*
            ProductCatalogItem productCatalog = new(
                productCatalogCreateRequest.product_code,
                productCatalogCreateRequest.description,
                productCatalogCreateRequest.unit,
                productCatalogCreateRequest.stock_current,
                productCatalogCreateRequest.currency,
                productCatalogCreateRequest.price_pvp,
                productCatalogCreateRequest.price_avg,
                productCatalogCreateRequest.price_last,
                productCatalogCreateRequest.date_last_entry,
                productCatalogCreateRequest.date_last_exit,
                productCatalogCreateRequest.family_id,
                productCatalogCreateRequest.price_ref_market
            );

            ProductCatalogModel.CreateProductCatalog(productCatalog, executer_user);
            */
            return new ProductCatalogItemResponse(ResponseSuccessMessage.Success, language);
        }
        catch (InputNotValidException e)
        {
            Log.Error("CreateProductCatalog endpoint - Error - " + e);
            return new ProductCatalogItemResponse(ResponseErrorMessage.InvalidArgs, language);
        }
        catch (Exception e)
        {
            Log.Error("CreateProductCatalog endpoint - Error - " + e);
            return new ProductCatalogItemResponse(ResponseErrorMessage.InternalError, language);
        }
    }

    [HttpPut]
    [Route("{id}")]
    [RequestLimit]
    [ValidateReferrer]
    [Authorize]
    public ActionResult<ProductCatalogItemResponse> UpdateProductCatalog(int id, [FromBody] ProductCatalogUpdateRequest productCatalogReq)
    {
        string language = this.Request.Headers["client-lang"];
        if (string.IsNullOrEmpty(language))
        {
            language = ConfigManager.defaultLanguage;
        }
        string token = this.Request.Headers["Authorization"];
        string executer_user = UserModel.GetUserByToken(token);

        if (!productCatalogReq.IsValid())
        {
            return new ProductCatalogItemResponse(ResponseErrorMessage.InvalidArgs, language);
        }

        try
        {
            /*
            ProductCatalogItem productCatalog = new(
                id,
                productCatalogReq.product_code,
                productCatalogReq.description,
                productCatalogReq.unit,
                productCatalogReq.stock_current,
                productCatalogReq.currency,
                productCatalogReq.price_pvp,
                productCatalogReq.price_avg,
                productCatalogReq.price_last,
                productCatalogReq.date_last_entry,
                productCatalogReq.date_last_exit,
                productCatalogReq.family_id,
                productCatalogReq.price_ref_market
            );

            ProductCatalogModel.UpdateProductCatalog(productCatalog, executer_user);
            */
            return new ProductCatalogItemResponse(ResponseSuccessMessage.Success, language);
        }
        catch (InputNotValidException e)
        {
            Log.Error("UpdateProductCatalog endpoint - Error - " + e);
            return new ProductCatalogItemResponse(ResponseErrorMessage.InvalidArgs, language);
        }
        catch (Exception e)
        {
            Log.Error("UpdateProductCatalog endpoint - Error - " + e);
            return new ProductCatalogItemResponse(ResponseErrorMessage.InternalError, language);
        }
    }

    [HttpDelete]
    [Route("{productCatalogId}")]
    [RequestLimit]
    [ValidateReferrer]
    [Authorize]
    public ActionResult<ProductCatalogItemResponse> DeleteProductCatalog(int productCatalogId)
    {
        string language = this.Request.Headers["client-lang"];
        if (string.IsNullOrEmpty(language))
        {
            language = ConfigManager.defaultLanguage;
        }
        string token = this.Request.Headers["Authorization"];
        string executer_user = UserModel.GetUserByToken(token);

        try
        {
            ProductCatalogModel.DeleteProductCatalog(productCatalogId, executer_user);

            return new ProductCatalogItemResponse(ResponseSuccessMessage.Success, language);
        }
        catch (InputNotValidException e)
        {
            Log.Error("DeleteProductCatalog endpoint - Error - " + e);
            return new ProductCatalogItemResponse(ResponseErrorMessage.InvalidArgs, language);
        }
        catch (Exception e)
        {
            Log.Error("DeleteProductCatalog endpoint - Error - " + e);
            return new ProductCatalogItemResponse(ResponseErrorMessage.InternalError, language);
        }
    }

    [HttpPatch]
    [Route("pricing-strategy")]
    [RequestLimit]
    [AuthorizeAdmin]
    public ActionResult<GenericResponse> PatchProductPricingStrategy(PatchPricingStrategyRequest pricingReq)
    {
        string language = this.Request.Headers["client-lang"];
        if (string.IsNullOrEmpty(language))
        {
            language = ConfigManager.defaultLanguage;
        }
        string token = this.Request.Headers["Authorization"];
        string executer_user = UserModel.GetUserByToken(token);

        if (!pricingReq.IsValid())
        {
            return new GenericResponse(ResponseErrorMessage.InvalidArgs, language);
        }

        try
        {
            // Update products pricing strategies
            ProductCatalogModel.PatchProductPricingStrategy(pricingReq, executer_user);

            return new GenericResponse(ResponseSuccessMessage.Success, language);
        }
        catch (InputNotValidException e)
        {
            Log.Error("ChangePricingOptions endpoint - Error - " + e);
            return new GenericResponse(ResponseErrorMessage.InvalidArgs, language);
        }
        catch (Exception e)
        {
            Log.Error("ChangePricingOptions endpoint - Error - " + e);
            return new GenericResponse(ResponseErrorMessage.InternalError, language);
        }
    }
}
