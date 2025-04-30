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
[Route("api/products/discounts")]
public class ProductDiscountController : ControllerBase
{
    [HttpGet]
    [Route("")]
    [RequestLimit]
    [ValidateReferrer]
    [Authorize]
    public ActionResult<ProductDiscountListResponse> GetProductsDiscounts([FromQuery] string? productFamilyId, [FromQuery] int? segmentId)
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
            // Fetch by specific rating
            List<ProductDiscountDTO> productDiscounts = ProductDiscountModel.GetProductDiscountsDTO(executer_user, productFamilyId, segmentId);
            return new ProductDiscountListResponse(productDiscounts, ResponseSuccessMessage.Success, language);
        }
        catch (Exception e)
        {
            Log.Error("GetProductsDiscounts endpoint - Error - " + e);
            return new ProductDiscountListResponse(ResponseErrorMessage.InternalError, language);
        }
    }

    [HttpGet]
    [Route("{productFamilyId}/{segmentId}")]
    [RequestLimit]
    [ValidateReferrer]
    [Authorize]
    public ActionResult<ProductDiscountItemResponse> GetProductsDiscounts(string productFamilyId, int segmentId)
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
            // Fetch by specific rating
            return new ProductDiscountItemResponse(ProductDiscountModel.GetProductDiscountByProductFamilyIdBySegmentIdDTO(productFamilyId, segmentId, executer_user), ResponseSuccessMessage.Success, language);
        }
        catch (Exception e)
        {
            Log.Error("GetProductsDiscounts endpoint - Error - " + e);
            return new ProductDiscountItemResponse(ResponseErrorMessage.InternalError, language);
        }
    }

    [HttpPost]
    [Route("")]
    [ValidateReferrer]
    [Authorize]
    [RequestLimit]
    public ActionResult<ProductDiscountItemResponse> CreateProductDiscount(ProductDiscountCreateRequest request)
    {
        string language = this.Request.Headers["client-lang"];
        if (string.IsNullOrEmpty(language))
        {
            language = ConfigManager.defaultLanguage;
        }
        string token = this.Request.Headers["Authorization"];
        string executer_user = UserModel.GetUserByToken(token);

        if (!request.IsValid())
        {
            return new ProductDiscountItemResponse(ResponseErrorMessage.InvalidArgs, language);
        }

        try
        {
            ProductDiscountItem productDiscount = new ProductDiscountItem(request.product_family_id, request.segment_id, request.mb_min, request.desc_max);
            ProductDiscountModel.CreateProductDiscount(productDiscount, executer_user);
            return new ProductDiscountItemResponse(ResponseSuccessMessage.Success, language);
        }
        catch (Exception e)
        {
            Log.Error("CreateProductDiscount endpoint - Error - " + e);
            return new ProductDiscountItemResponse(ResponseErrorMessage.InternalError, language);
        }
    }

    [HttpPut]
    [Route("{productFamilyId}/{segmentId}")]
    [ValidateReferrer]
    [Authorize]
    [RequestLimit]
    public ActionResult<ProductDiscountItemResponse> UpdateProductDiscount(string productFamilyId, int segmentId, ProductDiscountUpdateRequest request)
    {
        string language = this.Request.Headers["client-lang"];
        if (string.IsNullOrEmpty(language))
        {
            language = ConfigManager.defaultLanguage;
        }
        string token = this.Request.Headers["Authorization"];
        string executer_user = UserModel.GetUserByToken(token);

        if (!request.IsValid())
        {
            return new ProductDiscountItemResponse(ResponseErrorMessage.InvalidArgs, language);
        }

        try
        {
            ProductDiscountItem productDiscount = new(productFamilyId, segmentId, request.mb_min, request.desc_max);
            ProductDiscountModel.UpdateProductDiscount(productDiscount, executer_user);
            return new ProductDiscountItemResponse(ResponseSuccessMessage.Success, language);
        }
        catch (Exception e)
        {
            Log.Error("UpdateProductDiscount endpoint - Error - " + e);
            return new ProductDiscountItemResponse(ResponseErrorMessage.InternalError, language);
        }
    }

    [HttpDelete]
    [Route("{productFamilyId}/{segmentId}")]
    [ValidateReferrer]
    [Authorize]
    [RequestLimit]
    public ActionResult<ProductDiscountItemResponse> DeleteProductDiscount(string productFamilyId, int segmentId)
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
            ProductDiscountModel.DeleteProductDiscount(productFamilyId, segmentId, executer_user);
            return new ProductDiscountItemResponse(ResponseSuccessMessage.Success, language);
        }
        catch (Exception e)
        {
            Log.Error("DeleteProductDiscount endpoint - Error - " + e);
            return new ProductDiscountItemResponse(ResponseErrorMessage.InternalError, language);
        }
    }
}
