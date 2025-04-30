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
    [Route("api/logs")]
    public class LogsController : ControllerBase
    {
        [HttpGet]
        [Route("getLogs")]
        [RequestLimit]
        [ValidateReferrer]
        [AuthorizeAdmin]
        public ActionResult<GetLogsResponse> GetLogs()
        {
            string language = this.Request.Headers["client-lang"];
            if (string.IsNullOrEmpty(language))
            {
                language = ConfigManager.defaultLanguage;
            }

            string token = this.Request.Headers["Authorization"];
            string user_operation = UserModel.GetUserByToken(token);
            try
            {
                return new GetLogsResponse(LogsModel.GetLogs(user_operation), ResponseSuccessMessage.Success, language);
            }
            catch (Exception e)
            {
                Log.Error("GetReceipt endpoint - Error - optional args - " + user_operation);
                return new GetLogsResponse(ResponseErrorMessage.InternalError, language);
            }
        }
    }
}
