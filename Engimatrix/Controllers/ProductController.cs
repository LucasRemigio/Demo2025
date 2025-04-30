// // Copyright (c) 2024 Engibots. All rights reserved.

using engimatrix.Config;
using engimatrix.Filters;
using Engimatrix.Models;
using engimatrix.ResponseMessages;
using engimatrix.Views;
using Microsoft.AspNetCore.Mvc;
using engimatrix.Models;
using engimatrix.Utils;
using engimatrix.Connector;
using Engimatrix.ModelObjs;
using Engimatrix.Views;
using engimatrix.Exceptions;

namespace Engimatrix.Controllers
{
    [ApiController]
    [Route("api/products")]
    public class ProductController : ControllerBase
    {
        [HttpGet]
        [Route("{emailToken}")]
        [RequestLimit]
        [ValidateReferrer]
        [Authorize]
        public ActionResult<ProductResponse> GetEmailProducts(string emailToken)
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
                ProductModel.GetEmailProducts(emailToken, executer_user);

                return new ProductResponse(ResponseSuccessMessage.Success, language);
            }
            catch (Exception e)
            {
                Log.Error("Filtered endpoint - Error - " + e);
                return new ProductResponse(ResponseErrorMessage.InternalError, language);
            }
        }

        [HttpPut]
        [Route("{emailToken}")]
        [RequestLimit]
        [ValidateReferrer]
        [Authorize]
        public ActionResult<GenericResponse> UpdateEmailProductsList(UpdateProductRequest products, string emailToken)
        {
            string language = this.Request.Headers["client-lang"];
            if (string.IsNullOrEmpty(language))
            {
                language = ConfigManager.defaultLanguage;
            }

            if (!products.Validate())
            {
                return new GenericResponse(ResponseErrorMessage.InvalidArgs, language);
            }

            string token = this.Request.Headers["Authorization"];
            string executer_user = UserModel.GetUserByToken(token);
            try
            {
                ProductModel.UpdateEmailProducts(emailToken, products.products, executer_user);

                return new GenericResponse(ResponseSuccessMessage.Success, language);
            }
            catch (Exception e)
            {
                Log.Error("ReplyToEmailEndpoint - Error - " + e);
                return new GenericResponse(ResponseErrorMessage.InternalError, language);
            }
        }
    }
}
