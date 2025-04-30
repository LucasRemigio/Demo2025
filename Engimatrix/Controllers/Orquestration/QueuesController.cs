// // Copyright (c) 2024 Engibots. All rights reserved.

namespace engimatrix.Controllers.Orquestration
{
    using System;
    using Microsoft.AspNetCore.Mvc;
    using engimatrix.Config;
    using engimatrix.Filters;
    using engimatrix.Models;
    using engimatrix.ResponseMessages;
    using engimatrix.Utils;
    using engimatrix.Views;
    using engimatrix.ModelObjs;
    using engimatrix.Views.Orquestration;
    using static engimatrix.Views.Orquestration.QueuesResponse;
    using engimatrix.Models.Orquestration;
    using engimatrix.ModelObjs.Orquestration;
    using static engimatrix.Views.Orquestration.TransactionsResponse;

    [ApiController]
    [Route("api/queues")]
    public class QueuesController : ControllerBase
    {
        [HttpGet]
        [Route("getQueues")]
        [RequestLimit]
        [ValidateReferrer]
        [Authorize]
        public ActionResult<GetQueues> GetQueues()
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
                return new GetQueues(QueuesModel.GetQueues(language, user_operation), new List<TransactionsItem>(), ResponseSuccessMessage.Success, language);
            }
            catch (Exception e)
            {
                Log.Error("GetQueues endpoint - Error - optional args - " + user_operation);
                return new GetQueues(ResponseErrorMessage.QueueError, language);
            }
        }

        [HttpPost]
        [Route("addQueue")]
        [RequestLimit]
        [ValidateReferrer]
        [Authorize]
        public ActionResult<GenericResponse> AddQueue(QueuesRequest.Add input)
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
                if (!QueuesModel.Add(input, executer_user))
                {
                    Log.Error("Add Asset - A problem occurred registing order- ");
                    return new GenericResponse(ResponseErrorMessage.ErrorAddingQueue, language);
                }

                return new GenericResponse(ResponseSuccessMessage.Success, language);
            }
            catch (Exception e)
            {
                Log.Error("Add Asset endpoint - Error - " + e);
                return new GenericResponse(ResponseErrorMessage.ErrorAddingQueue, language);
            }
        }

        [HttpPost]
        [Route("removeQueue")]
        [RequestLimit]
        [ValidateReferrer]
        [Authorize]
        public ActionResult<GenericResponse> removeQueue(string id)
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
                if (!QueuesModel.remove(id, executer_user))
                {
                    Log.Error("Remove Queue - A problem occurred removing Queue- ");
                    return new GenericResponse(ResponseErrorMessage.ErrorRemovingQueue, language);
                }

                return new GenericResponse(ResponseSuccessMessage.Success, language);
            }
            catch (Exception e)
            {
                Log.Error("Remove Queue endpoint - Error - " + e);
                return new GenericResponse(ResponseErrorMessage.ErrorRemovingQueue, language);
            }
        }

        [HttpPost]
        [Route("editQueue")]
        [RequestLimit]
        [ValidateReferrer]
        [Authorize]
        public ActionResult<GenericResponse> EditQueue(QueuesRequest.Edit input)
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
                if (!QueuesModel.edit(input, executer_user))
                {
                    Log.Error("Edit Queue - A problem occurred edit Queue- ");
                    return new GenericResponse(ResponseErrorMessage.ErrorEditingQueue, language);
                }

                return new GenericResponse(ResponseSuccessMessage.Success, language);
            }
            catch (Exception e)
            {
                Log.Error("Edit Queue endpoint - Error - " + e);
                return new GenericResponse(ResponseErrorMessage.ErrorEditingQueue, language);
            }
        }

        [HttpGet]
        [Route("getTransactionByQueueId")]
        [RequestLimit]
        [ValidateReferrer]
        [Authorize]
        public ActionResult<GetTransactions> getTransactionByQueueId(int id)
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
                return new GetTransactions(QueuesModel.getTransactionByQueueId(id.ToString(), language, user_operation), ResponseSuccessMessage.Success, language);
            }
            catch (Exception e)
            {
                Log.Error("GetQueues endpoint - Error - optional args - " + user_operation);
                return new GetTransactions(ResponseErrorMessage.QueueError, language);
            }
        }

        [HttpPost]
        [Route("addTransaction")]
        [RequestLimit]
        [ValidateReferrer]
        [Authorize]
        public ActionResult<AddTransactionResponse> AddTransaction(TransactionsRequest.AddTransaction input)
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
        [Route("removeTransaction")]
        [RequestLimit]
        [ValidateReferrer]
        [Authorize]
        public ActionResult<GenericResponse> RemoveTransaction(string id)
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
                if (!QueuesModel.removeTransaction(id, executer_user))
                {
                    Log.Error("Remove Transaction - A problem occurred removing Transaction");
                    return new GenericResponse(ResponseErrorMessage.ErrorRemovingTransaction, language);
                }

                return new GenericResponse(ResponseSuccessMessage.Success, language);
            }
            catch (Exception e)
            {
                Log.Error("Remove Transaction endpoint - Error - " + e);
                return new GenericResponse(ResponseErrorMessage.ErrorRemovingTransaction, language);
            }
        }

        [HttpPost]
        [Route("editTransaction")]
        [RequestLimit]
        [ValidateReferrer]
        [Authorize]
        public ActionResult<GenericResponse> EditTransaction(TransactionsRequest.EditTransaction input)
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
    }
}
