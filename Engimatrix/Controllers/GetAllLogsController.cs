// // Copyright (c) 2024 Engibots. All rights reserved.

namespace engimatrix.Controllers
{
    using System;
    using Microsoft.AspNetCore.Mvc;
    using engimatrix.Config;
    using engimatrix.Filters;
    using engimatrix.Models;
    using engimatrix.ResponseMessages;
    using engimatrix.Utils;
    using engimatrix.Views;

    [ApiController]
    [Route("api/GetAllLogs")]
    public class GetAllLogsController : ControllerBase
    {
        [HttpGet]
        [Route("getAllLogs")]
        [RequestLimit]
        [ValidateReferrer]
        [AuthorizeAdmin]
        public ActionResult<GetAllLogsResponse> GetAllLogs(string emailFilter)
        {
            string language = this.Request.Headers["client-lang"];
            if (string.IsNullOrEmpty(language))
            {
                language = ConfigManager.defaultLanguage;
            }
            try
            {
                return new GetAllLogsResponse(GetAllLogsModel.GetAllLogs(emailFilter), ResponseSuccessMessage.Success, language);
            }
            catch (Exception e)
            {
                Log.Error("GetClient endpoint - Error - optional args - " + emailFilter);
                return new GetAllLogsResponse(ResponseErrorMessage.InternalError, language);
            }
        }
    }
}
