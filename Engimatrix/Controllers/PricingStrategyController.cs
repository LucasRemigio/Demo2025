// // Copyright (c) 2024 Engibots. All rights reserved.

namespace engimatrix.Controllers
{
    using Microsoft.AspNetCore.Mvc;
    using engimatrix.Config;
    using engimatrix.Filters;
    using engimatrix.ResponseMessages;
    using engimatrix.Views;
    using engimatrix.Models;
    using engimatrix.Exceptions;
    using engimatrix.Utils;
    using engimatrix.ModelObjs;

    [ApiController]
    [Route("api/pricing-strategies")]
    public class PricingStrategyController : Controller
    {
        [HttpGet]
        [Route("")]
        [RequestLimit]
        [AuthorizeAdmin]
        public ActionResult<PricingStrategyResponse> GetCurrentPricingStrategy()
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
                List<PricingStrategyItem> pricingStrategies = PricingStrategyModel.GetPricingStrategies(executer_user);
                return new PricingStrategyResponse(pricingStrategies, ResponseSuccessMessage.Success, language);
            }
            catch (InputNotValidException e)
            {
                Log.Error("GetCurrentPricingStrategy endpoint - Error - " + e);
                return new PricingStrategyResponse(ResponseErrorMessage.InvalidArgs, language);
            }
            catch (Exception e)
            {
                Log.Error("GetCurrentPricingStrategy endpoint - Error - " + e);
                return new PricingStrategyResponse(ResponseErrorMessage.InternalError, language);
            }
        }
    }
}
