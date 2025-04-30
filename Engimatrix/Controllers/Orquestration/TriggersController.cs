using engimatrix.Filters;
using engimatrix.Models.Orquestration;
using engimatrix.ResponseMessages;
using engimatrix.Utils;
using Microsoft.AspNetCore.Mvc;
using engimatrix.Config;
using engimatrix.Views.Orquestration;
using GenericResponse = engimatrix.Views.GenericResponse;
using static engimatrix.Views.Orquestration.TriggersResponse;
using engimatrix.Models;

namespace engimatrix.Controllers.Orquestration

{
    [ApiController]
    [Route("api/triggers")]
    public class TriggersController : ControllerBase
    {

        [HttpGet]
        [Route("getTriggers")]
        [RequestLimit]
        [ValidateReferrer]
        //[Authorize]
        public ActionResult<GetTriggers> GetTriggers()
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
                return new GetTriggers(TriggersModel.GetTriggers(executer_user, language), ResponseSuccessMessage.Success, language);
            }
            catch (Exception e)
            {
                Log.Error("GetTriggers endpoint - Error - optional args - " + e);
                return new GetTriggers(ResponseErrorMessage.TriggersError, language);
            }
        }

        [HttpGet]
        [Route("getTriggersByScriptId")]
        [RequestLimit]
        [ValidateReferrer]
        //[Authorize]
        public ActionResult<GetTriggers> getTriggersByScriptId(int id)
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
                return new GetTriggers(TriggersModel.GetTriggersByScriptId(id.ToString(), language, user_operation), ResponseSuccessMessage.Success, language);
            }
            catch (Exception e)
            {
                Log.Error("getTriggersByScriptId endpoint - Error - optional args - " + user_operation);
                return new GetTriggers(ResponseErrorMessage.TriggersError, language);
            }
        }

        [HttpPost]
        [Route("addTrigger")]
        [RequestLimit]
        [ValidateReferrer]
        //[Authorize]
        public ActionResult<GenericResponse> AddTrigger(TriggersRequest.AddTrigger input)
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
                TriggersModel.AddTrigger(input, executer_user);

                return new GenericResponse(ResponseSuccessMessage.Success, language);
            }
            catch (Exception e)
            {
                Log.Error("Add Trigger endpoint - Error - optional args - " + input.name + " - " + e);
                return new Views.GenericResponse(ResponseErrorMessage.ErrorAddingTrigger, language);
            }
        }

        [HttpPost]
        [Route("editTrigger")]
        [RequestLimit]
        [ValidateReferrer]
        //[Authorize]
        public ActionResult<GenericResponse> EditTrigger(TriggersRequest.EditTrigger input)
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
                TriggersModel.EditTrigger(input, executer_user);

                return new GenericResponse(ResponseSuccessMessage.Success, language);
            }
            catch (Exception e)
            {
                Log.Error("EditTrigger endpoint - Error - optional args - " + input.name + " - " + e);
                return new Views.GenericResponse(ResponseErrorMessage.ErrorAddingTrigger, language);
            }
        }

        [HttpPost]
        [Route("removeTrigger")]
        [RequestLimit]
        [ValidateReferrer]
        //[Authorize]
        public ActionResult<Views.GenericResponse> RemoveTrigger(string id)
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
                TriggersModel.RemoveTrigger(id, executer_user);
                return new Views.GenericResponse(ResponseSuccessMessage.Success, language);
            }
            catch (Exception e)
            {
                Log.Error("RemoveTrigger endpoint - Error - optional args - " + id + " - " + e);
                return new Views.GenericResponse(ResponseErrorMessage.ErrorRemovingTrigger, language);
            }
        }
    }
}
