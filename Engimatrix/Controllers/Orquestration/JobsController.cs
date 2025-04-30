// <copyright file="ReceiptController.cs" company="Brighten">
// Copyright (c) Brighten. All rights reserved.
// </copyright>

namespace engimatrix.Controllers.Orquestration
{
    using Microsoft.AspNetCore.Mvc;
    using System;
    using engimatrix.Filters;
    using engimatrix.Views.Orquestration;
    using engimatrix.ResponseMessages;
    using engimatrix.Utils;
    using engimatrix.Models.Orquestration;
    using engimatrix.Exceptions;
    using engimatrix.ModelObjs.Orquestration;
    using System.Collections.Generic;
    using System.Threading.Tasks;
    using Newtonsoft.Json.Linq;
    using engimatrix.Config;
    using engimatrix.Models;

    [ApiController]
    [Route("api/jobs")]
    public class JobsController : ControllerBase
    {
        [HttpGet]
        [Route("getJobs")]
        [RequestLimit]
        [ValidateReferrer]
        [AuthorizeAdmin]
        public ActionResult<GetJobsResponse> GetJobs()
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
                return new GetJobsResponse(JobsModel.GetJobs(language, user_operation), ResponseSuccessMessage.Success, language);
            }
            catch (Exception e)
            {
                Log.Error("GetJobs endpoint - Error - optional args - " + user_operation);
                return new GetJobsResponse(ResponseErrorMessage.JobsError, language);
            }
        }

        [HttpGet]
        [Route("getJobsByScriptId")]
        [RequestLimit]
        [ValidateReferrer]
        [AuthorizeAdmin]
        public ActionResult<GetJobsResponse> GetJobsByScriptId(string id)
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
                return new GetJobsResponse(JobsModel.GetJobsByScriptId(id, language, user_operation), ResponseSuccessMessage.Success, language);
            }
            catch (Exception e)
            {
                Log.Error("GetJobsByScriptId endpoint - Error - optional args - " + user_operation + " - " + e);
                return new GetJobsResponse(ResponseErrorMessage.JobsError, language);
            }
        }
    }
}
