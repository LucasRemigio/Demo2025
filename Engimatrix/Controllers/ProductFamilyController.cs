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
[Route("api/products/families")]
public class ProductFamilyController : ControllerBase
{
    [HttpGet]
    [Route("")]
    [RequestLimit]
    [ValidateReferrer]
    [Authorize]
    public ActionResult<ProductFamilyListResponse> GetProductFamilies()
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
            return new ProductFamilyListResponse(ProductFamilyModel.GetProductFamilies(executer_user), ResponseSuccessMessage.Success, language);
        }
        catch (Exception e)
        {
            Log.Error("GetProductFamilies endpoint - Error - " + e);
            return new ProductFamilyListResponse(ResponseErrorMessage.InternalError, language);
        }
    }

    [HttpGet]
    [Route("{productFamilyId}")]
    [RequestLimit]
    [ValidateReferrer]
    [Authorize]
    public ActionResult<ProductFamilyItemResponse> GetSegmentById(string productFamilyId)
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
            return new ProductFamilyItemResponse(ProductFamilyModel.GetProductFamilyById(productFamilyId, executer_user), ResponseSuccessMessage.Success, language);
        }
        catch (InputNotValidException e)
        {
            Log.Error("GetProductFamilies endpoint - Error - " + e);
            return new ProductFamilyItemResponse(ResponseErrorMessage.InvalidArgs, language);
        }
        catch (Exception e)
        {
            Log.Error("GetProductFamilies endpoint - Error - " + e);
            return new ProductFamilyItemResponse(ResponseErrorMessage.InternalError, language);
        }
    }

    [HttpPost]
    [Route("")]
    [RequestLimit]
    [ValidateReferrer]
    [Authorize]
    public ActionResult<ProductFamilyItemResponse> CreateProductFamily(ProductFamilyCreateRequest request)
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
            return new ProductFamilyItemResponse(ResponseErrorMessage.InvalidArgs, language);
        }

        try
        {
            ProductFamilyItem productFamily = new ProductFamilyItem(request.id, request.name);
            ProductFamilyModel.CreateProductFamily(productFamily, executer_user);
            return new ProductFamilyItemResponse(productFamily, ResponseSuccessMessage.Success, language);
        }
        catch (InputNotValidException e)
        {
            Log.Error("CreateProductFamily endpoint - Error - " + e);
            return new ProductFamilyItemResponse(ResponseErrorMessage.InvalidArgs, language);
        }
        catch (Exception e)
        {
            Log.Error("CreateProductFamily endpoint - Error - " + e);
            return new ProductFamilyItemResponse(ResponseErrorMessage.InternalError, language);
        }
    }

    [HttpPut]
    [Route("{productFamilyId}")]
    [RequestLimit]
    [ValidateReferrer]
    [Authorize]
    public ActionResult<ProductFamilyItemResponse> UpdateProductFamily(string productFamilyId, ProductFamilyUpdateRequest request)
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
            return new ProductFamilyItemResponse(ResponseErrorMessage.InvalidArgs, language);
        }

        try
        {
            ProductFamilyItem productFamily = new ProductFamilyItem(productFamilyId, request.name);
            ProductFamilyModel.UpdateProductFamily(productFamily, executer_user);
            return new ProductFamilyItemResponse(productFamily, ResponseSuccessMessage.Success, language);
        }
        catch (InputNotValidException e)
        {
            Log.Error("UpdateProductFamily endpoint - Error - " + e);
            return new ProductFamilyItemResponse(ResponseErrorMessage.InvalidArgs, language);
        }
        catch (Exception e)
        {
            Log.Error("UpdateProductFamily endpoint - Error - " + e);
            return new ProductFamilyItemResponse(ResponseErrorMessage.InternalError, language);
        }
    }

    [HttpDelete]
    [Route("{productFamilyId}")]
    [RequestLimit]
    [ValidateReferrer]
    [Authorize]
    public ActionResult<ProductFamilyItemResponse> DeleteProductFamily(string productFamilyId)
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
            ProductFamilyModel.DeleteProductFamily(productFamilyId, executer_user);
            return new ProductFamilyItemResponse(ResponseSuccessMessage.Success, language);
        }
        catch (InputNotValidException e)
        {
            Log.Error("DeleteProductFamily endpoint - Error - " + e);
            return new ProductFamilyItemResponse(ResponseErrorMessage.InvalidArgs, language);
        }
        catch (Exception e)
        {
            Log.Error("DeleteProductFamily endpoint - Error - " + e);
            return new ProductFamilyItemResponse(ResponseErrorMessage.InternalError, language);
        }
    }
}
