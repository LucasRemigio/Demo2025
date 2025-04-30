using engimatrix.Exceptions;
using engimatrix.Filters;
using engimatrix.Models.Orquestration;
using engimatrix.ResponseMessages;
using engimatrix.Utils;
using engimatrix.Views;
using Microsoft.AspNetCore.Mvc;
using System;
using engimatrix.Config;
using engimatrix.Views.Orquestration;
using static engimatrix.Views.Orquestration.QueuesResponse;
using static engimatrix.Views.Orquestration.AssetsResponse;
using static engimatrix.Views.Orquestration.TransactionsResponse;

using engimatrix.ModelObjs.Orquestration;

namespace engimatrix.Controllers.Orquestration

{
    [ApiController]
    [Route("api/runscripts")]
    public class RunScriptsController : ControllerBase
    {
        private readonly IWebHostEnvironment _env;
        public RunScriptsController(IWebHostEnvironment env)
        {
            _env = env;
        }

        [HttpGet]
        [Route("getQueues")]
        [RequestLimit]
        [ValidateReferrer]
        public ActionResult<GetQueues> GetQueues(string engiToken)
        {
            string language = this.Request.Headers["client-lang"];
            if (string.IsNullOrEmpty(language))
            {
                language = ConfigManager.defaultLanguage;
            }
            string user_operation = "system";

            try
            {
                if (string.IsNullOrEmpty(engiToken) || !engiToken.Equals(ConfigManager.engimatrixInternalApiKey))
                {
                    return new GetQueues(ResponseErrorMessage.InvalidArgs, "pt");
                }

                return new GetQueues(QueuesModel.GetQueues(language, user_operation), new List<TransactionsItem>(), ResponseSuccessMessage.Success, language);
            }
            catch (Exception e)
            {
                Log.Error("GetQueues endpoint - Error - optional args - " + user_operation);
                return new GetQueues(ResponseErrorMessage.QueueError, language);
            }
        }

        [HttpGet]
        [Route("getAssetByName")]
        [RequestLimit]
        [ValidateReferrer]
        public ActionResult<GetAsset> GetAssetByName(string assetName, string engiToken)
        {
            string language = this.Request.Headers["client-lang"];
            if (string.IsNullOrEmpty(language))
            {
                language = ConfigManager.defaultLanguage;
            }
            string user_operation = "system";

            if (string.IsNullOrEmpty(assetName))
            {
                Log.Error("GetAssetByName endpoint - Error - optional args - " + user_operation);
                return new GetAsset(ResponseErrorMessage.InvalidArgs, language);
            }

            try
            {
                if (string.IsNullOrEmpty(engiToken) || !engiToken.Equals(ConfigManager.engimatrixInternalApiKey))
                {
                    return new GetAsset(ResponseErrorMessage.InvalidArgs, language);
                }

                AssetsItem assetItem = AssetsModel.GetAssetByName(assetName, user_operation);
                return new GetAsset(assetItem, ResponseSuccessMessage.Success, language);
            }
            catch (Exception e)
            {
                Log.Error("GetTextAssetByName endpoint - Error - optional args - " + user_operation);
                return new GetAsset(ResponseErrorMessage.InternalError, language);
            }
        }

        [HttpGet]
        [Route("getTransactionByQueueName")]
        [RequestLimit]
        [ValidateReferrer]
        public ActionResult<GetTransactions> getTransactionByQueueName(string queue_name, string engiToken)
        {
            string language = this.Request.Headers["client-lang"];
            if (string.IsNullOrEmpty(language))
            {
                language = ConfigManager.defaultLanguage;
            }
            string user_operation = "system";

            try
            {
                if (string.IsNullOrEmpty(engiToken) || !engiToken.Equals(ConfigManager.engimatrixInternalApiKey))
                {
                    return new GetTransactions(ResponseErrorMessage.QueueError, language);
                }

                return new GetTransactions(QueuesModel.getTransactionByQueueName(queue_name, language, user_operation), ResponseSuccessMessage.Success, language);
            }
            catch (Exception e)
            {
                Log.Error("getTransactionByQueueName endpoint - Error - optional args - " + user_operation);
                return new GetTransactions(ResponseErrorMessage.QueueError, language);
            }
        }

        [HttpPost]
        [Route("addTransaction")]
        [RequestLimit]
        [ValidateReferrer]
        public ActionResult<AddTransactionResponse> AddTransaction(TransactionsRequest.AddTransaction input, string engiToken)
        {
            string language = this.Request.Headers["client-lang"];
            if (string.IsNullOrEmpty(language))
            {
                language = ConfigManager.defaultLanguage;
            }

            string executer_user = "system";

            try
            {
                (bool success, int transaction_id) = QueuesModel.AddTransaction(input, executer_user);

                if (!success)
                {
                    Log.Error("Add Transaction - A problem occurred adding the Transaction");
                    return new AddTransactionResponse(ResponseErrorMessage.ErrorAddingTransaction, language);
                }

                return new AddTransactionResponse(ResponseSuccessMessage.Success, language, transaction_id.ToString());
            }
            catch (Exception e)
            {
                Log.Error("Add Transaction endpoint - Error - " + e);
                return new AddTransactionResponse(ResponseErrorMessage.ErrorAddingTransaction, language);
            }
        }

        [HttpPost]
        [Route("editTransaction")]
        [RequestLimit]
        [ValidateReferrer]
        public ActionResult<GenericResponse> EditTransaction(TransactionsRequest.EditTransaction input, string engiToken)
        {
            string language = this.Request.Headers["client-lang"];
            if (string.IsNullOrEmpty(language))
            {
                language = ConfigManager.defaultLanguage;
            }

            string executer_user = "system";

            try
            {
                if (string.IsNullOrEmpty(engiToken) || !engiToken.Equals(ConfigManager.engimatrixInternalApiKey))
                {
                    return new GenericResponse(ResponseErrorMessage.InvalidArgs, language);
                }

                if (!QueuesModel.editTransaction(input, executer_user, language))
                {
                    Log.Error("Edit Transaction - A problem occurred editing the Transaction- ");
                    return new GenericResponse(ResponseErrorMessage.ErrorEditingTransaction, language);
                }

                return new GenericResponse(ResponseSuccessMessage.Success, language);
            }
            catch (Exception e)
            {
                Log.Error("Edit Transaction endpoint - Error - " + e);
                return new GenericResponse(ResponseErrorMessage.ErrorEditingTransaction, language);
            }
        }

        [HttpPost]
        [Route("insertJobDetails")]
        [RequestLimit]
        [ValidateReferrer]
        public ActionResult<GenericResponse> InsertJobDetailsUsingQueueName(JobsRequest.Add input, string engiToken)
        {
            string language = this.Request.Headers["client-lang"];
            if (string.IsNullOrEmpty(language))
            {
                language = ConfigManager.defaultLanguage;
            }

            string executer_user = "system";

            try
            {
                if (string.IsNullOrEmpty(engiToken) || !engiToken.Equals(ConfigManager.engimatrixInternalApiKey))
                {
                    return new GenericResponse(ResponseErrorMessage.InvalidArgs, language);
                }

                bool success = JobsModel.InsertJobDetailsUsingQueueName(input.queue_name, executer_user, input.status_name, input.date_time, input.job_details, language);

                if (!success)
                {
                    Log.Error("Insert Job Details - A problem occurred trying to insert job details - ");
                    return new GenericResponse(ResponseErrorMessage.JobsError, language);
                }

                return new GenericResponse(ResponseSuccessMessage.Success, language);
            }
            catch (Exception e)
            {
                Log.Error("Insert Job Details endpoint - Error - " + e);
                return new GenericResponse(ResponseErrorMessage.JobsError, language);
            }
        }

        [HttpPost]
        [Route("removeTransactionsWithStatusNew")]
        [RequestLimit]
        [ValidateReferrer]
        public ActionResult<GenericResponse> RemoveTransactionWithStatusNew(string engiToken)
        {
            string language = this.Request.Headers["client-lang"];
            if (string.IsNullOrEmpty(language))
            {
                language = ConfigManager.defaultLanguage;
            }

            string executer_user = "system";

            try
            {
                if (string.IsNullOrEmpty(engiToken) || !engiToken.Equals(ConfigManager.engimatrixInternalApiKey))
                {
                    return new GenericResponse(ResponseErrorMessage.InvalidArgs, language);
                }

                if (!QueuesModel.removeTransactionsWithStatusNew(executer_user))
                {
                    Log.Error("RemoveTransactionWithStatusNew endpoint - A problem occurred removing transactions with status New.");
                    return new GenericResponse(ResponseErrorMessage.ErrorRemovingTransaction, language);
                }

                return new GenericResponse(ResponseSuccessMessage.Success, language);
            }
            catch (Exception e)
            {
                Log.Error("RemoveTransactionWithStatus endpoint - Error - " + e);
                return new GenericResponse(ResponseErrorMessage.ErrorRemovingTransaction, language);
            }
        }

    }
}
