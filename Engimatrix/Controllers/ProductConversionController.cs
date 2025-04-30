// // Copyright (c) 2024 Engibots. All rights reserved.

using engimatrix.Config;
using engimatrix.Filters;
using engimatrix.ResponseMessages;
using engimatrix.Views;
using Microsoft.AspNetCore.Mvc;
using engimatrix.Models;
using engimatrix.Utils;
using engimatrix.Exceptions;
using engimatrix.ModelObjs;

namespace Engimatrix.Controllers
{
    [ApiController]
    [Route("api/products/conversions")]
    public class ProductConversionController : ControllerBase
    {
        [HttpGet]
        [Route("")]
        [RequestLimit]
        [ValidateReferrer]
        [Authorize]
        public ActionResult<ProductConversionListResponse> GetProductConversions()
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
                List<ProductConversionItem> productConversions = ProductConversionModel.GetProductConversions(executer_user);
                return new ProductConversionListResponse(productConversions, ResponseSuccessMessage.Success, language);
            }
            catch (DatabaseException e)
            {
                Log.Error("GetProductConversions endpoint - Error in database - " + e);
                return new ProductConversionListResponse(ResponseErrorMessage.InternalError, language);
            }
            catch (ResourceEmptyException e)
            {
                Log.Error("GetProductConversions endpoint - Error in resource - " + e);
                return new ProductConversionListResponse(ResponseErrorMessage.ResourceEmpty, language);
            }
            catch (Exception e)
            {
                Log.Error("GetProductConversions endpoint - Error - " + e);
                return new ProductConversionListResponse(ResponseErrorMessage.DatabaseQueryError, language);
            }
        }


        [HttpGet]
        [Route("dto")]
        [RequestLimit]
        [ValidateReferrer]
        public ActionResult<ProductConversionDtoListResponse> GetProductConversionsDto()
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
                List<ProductConversionDTO> productConversions = ProductConversionModel.GetProductConversionsDTO(executer_user);
                return new ProductConversionDtoListResponse(productConversions, ResponseSuccessMessage.Success, language);
            }
            catch (DatabaseException e)
            {
                Log.Error("GetProductConversions endpoint - Error in database - " + e);
                return new ProductConversionDtoListResponse(ResponseErrorMessage.InternalError, language);
            }
            catch (ResourceEmptyException e)
            {
                Log.Error("GetProductConversions endpoint - Error in resource - " + e);
                return new ProductConversionDtoListResponse(ResponseErrorMessage.ResourceEmpty, language);
            }
            catch (Exception e)
            {
                Log.Error("GetProductConversions endpoint - Error - " + e);
                return new ProductConversionDtoListResponse(ResponseErrorMessage.DatabaseQueryError, language);
            }
        }

        [HttpGet]
        [Route("{id}")]
        [RequestLimit]
        [ValidateReferrer]
        [Authorize]
        public ActionResult<ProductConversionItemResponse> GetProductConversionById(int id)
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
                ProductConversionItem? productConversion = ProductConversionModel.GetProductConversionById(id, executer_user);
                if (productConversion == null)
                {
                    return new ProductConversionItemResponse(ResponseErrorMessage.NotFound, language);
                }
                return new ProductConversionItemResponse(productConversion, ResponseSuccessMessage.Success, language);
            }
            catch (DatabaseException e)
            {
                Log.Error("GetProductConversionsById endpoint - Error in database - " + e);
                return new ProductConversionItemResponse(ResponseErrorMessage.DatabaseQueryError, language);
            }
            catch (Exception e)
            {
                Log.Error("GetProductConversionById endpoint - Error - " + e);
                return new ProductConversionItemResponse(ResponseErrorMessage.InternalError, language);
            }
        }

        [HttpPost]
        [Route("sync-primavera")]
        [RequestLimit]
        [ValidateReferrer]
        public async Task<ActionResult<SyncPrimaveraStatsResponse>> SyncPrimavera(string key)
        {
            string language = this.Request.Headers["client-lang"];
            if (string.IsNullOrEmpty(language))
            {
                language = ConfigManager.defaultLanguage;
            }
            string token = this.Request.Headers["Authorization"];
            string executer_user = "System";
            if (!string.IsNullOrEmpty(token))
            {
                executer_user = UserModel.GetUserByToken(token);
            }

            try
            {
                if (string.IsNullOrEmpty(key) || !key.Equals(ConfigManager.engimatrixInternalApiKey, StringComparison.Ordinal))
                {
                    return new SyncPrimaveraStatsResponse(ResponseErrorMessage.InvalidArgs, language);
                }

                SyncPrimaveraStats stats = await ProductConversionPrimaveraModel.SyncPrimavera(executer_user);
                return new SyncPrimaveraStatsResponse(stats, ResponseSuccessMessage.Success, language);
            }
            catch (DatabaseException e)
            {
                Log.Error("SyncPrimavera endpoint - Error in database - " + e);
                return new SyncPrimaveraStatsResponse(ResponseErrorMessage.DatabaseQueryError, language);
            }
            catch (Exception e)
            {
                Log.Error("SyncPrimavera endpoint - Error - " + e);
                return new SyncPrimaveraStatsResponse(ResponseErrorMessage.InternalError, language);
            }
        }
    }
}
