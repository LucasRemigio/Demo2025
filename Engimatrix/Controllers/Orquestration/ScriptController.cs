using engimatrix.Filters;
using engimatrix.Models.Orquestration;
using engimatrix.Models;
using engimatrix.ResponseMessages;
using engimatrix.Utils;
using Microsoft.AspNetCore.Mvc;
using engimatrix.Config;
using engimatrix.Views.Orquestration;
using static engimatrix.Views.Orquestration.ScriptRequest;

namespace engimatrix.Controllers.Orquestration

{
    [ApiController]
    [Route("api/scripts")]
    public class ScriptController : ControllerBase
    {
        [HttpGet]
        [Route("getScripts")]
        [RequestLimit]
        [ValidateReferrer]
        [Authorize]
        public ActionResult<GetScriptsResponse> GetScripts()
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
                return new GetScriptsResponse(ScriptsModel.GetScripts(executer_user, language), ResponseSuccessMessage.Success, language);
            }
            catch (Exception e)
            {
                Log.Error("GetScripts endpoint - Error - optional args - " + e);
                return new GetScriptsResponse(ResponseErrorMessage.ScriptError, language);
            }
        }

        [HttpPost]
        [Route("startScript")]
        [RequestLimit]
        [ValidateReferrer]
        [Authorize]
        public ActionResult<StartScriptResponse> StartScript(string id)
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
                return new StartScriptResponse(ScriptsModel.StartScript(id, executer_user, language), ResponseSuccessMessage.Success, language);
            }
            catch (Exception e)
            {
                Log.Error("StartScripts endpoint - Error - optional args - " + id + " - " + e);
                return new StartScriptResponse(ResponseErrorMessage.UnableToStartScript, language);
            }
        }

        [HttpPost]
        [Route("editScript")]
        [RequestLimit]
        [ValidateReferrer]
        [AuthorizeAdmin]
        public ActionResult<Views.GenericResponse> EditScript(Edit input)
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
                ScriptsModel.EditScript(input, executer_user);
                return new Views.GenericResponse(ResponseSuccessMessage.Success, language);
            }
            catch (Exception e)
            {
                Log.Error("EditScripts endpoint - Error - optional args - " + input.id + " - " + e);
                return new Views.GenericResponse(ResponseErrorMessage.UnableToEditScript, language);
            }
        }

        [HttpPost]
        [Route("updateScriptStatus")]
        [RequestLimit]
        [ValidateReferrer]
        [Authorize]
        public ActionResult<Views.GenericResponse> UpdateScriptStatus(string id)
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
                ScriptsModel.UpdateScriptStatus(id, executer_user, language);
                return new Views.GenericResponse(ResponseSuccessMessage.Success, language);
            }
            catch (Exception e)
            {
                Log.Error("UpdateScriptsStatus endpoint - Error - optional args - " + id + " - " + e);
                return new Views.GenericResponse(ResponseErrorMessage.UnableToUpdateScriptStatus, language);
            }
        }

        [HttpPost]
        [Route("addScript")]
        [RequestLimit]
        [ValidateReferrer]
        [AuthorizeAdmin]
        public ActionResult<Views.GenericResponse> AddScript(Add input)
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
                ScriptsModel.AddScript(input, executer_user);
                return new Views.GenericResponse(ResponseSuccessMessage.Success, language);
            }
            catch (Exception e)
            {
                Log.Error("Add Script endpoint - Error - optional args - " + input.main_file + " - " + e);
                return new Views.GenericResponse(ResponseErrorMessage.UnableToEditScript, language);
            }
        }

        [HttpPost]
        [Route("removeScript")]
        [RequestLimit]
        [ValidateReferrer]
        [Authorize]
        public ActionResult<Views.GenericResponse> RemoveScript(string id)
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
                string user_role_id = UserModel.GetUserRoleIdByUser(executer_user);

                if (user_role_id != "2")
                {
                    return new Views.GenericResponse(ResponseErrorMessage.OnlyAdmins, language);
                }

                ScriptsModel.RemoveScript(id, executer_user);
                return new Views.GenericResponse(ResponseSuccessMessage.Success, language);
            }
            catch (Exception e)
            {
                Log.Error("RemoveScript endpoint - Error - optional args - " + id + " - " + e);
                return new Views.GenericResponse(ResponseErrorMessage.UnableToRemoveScript, language);
            }
        }
    }
}
