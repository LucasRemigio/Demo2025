// // Copyright (c) 2024 Engibots. All rights reserved.

namespace engimatrix.Controllers
{
    using Microsoft.AspNetCore.Mvc;
    using engimatrix.Config;
    using engimatrix.Filters;
    using engimatrix.ResponseMessages;
    using engimatrix.Views;

    [ApiController]
    [Route("api/config")]
    public class ConfigController : Controller
    
    {
        [HttpGet]
        [Route("ping")]
        [RequestLimit]
        public ActionResult<GenericResponse> Ping()
        {
            return new GenericResponse(ResponseSuccessMessage.Success, ConfigManager.defaultLanguage);
        }

        [HttpGet]
        [Route("testToken")]
        [RequestLimit]
        [Authorize]
        public ActionResult<GenericResponse> TestToken()
        {
            return new GenericResponse(ResponseSuccessMessage.Success, ConfigManager.defaultLanguage);
        }
    }
}
