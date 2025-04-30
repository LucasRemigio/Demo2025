// // Copyright (c) 2024 Engibots. All rights reserved.

namespace engimatrix.Controllers
{
    using System;
    using engimatrix.Views;
    using Microsoft.AspNetCore.Mvc;
    using engimatrix.Config;
    using engimatrix.Exceptions;
    using engimatrix.Filters;
    using engimatrix.Models;
    using engimatrix.ResponseMessages;
    using engimatrix.Utils;
    using Microsoft.IdentityModel.Tokens;
    using Engimatrix.Models;

    [ApiController]
    [Route("api/user")]
    public class UserController : ControllerBase
    {
        [HttpGet]
        [Route("getUsers")]
        [RequestLimit]
        [ValidateReferrer]
        [AuthorizeAdmin]
        public ActionResult<GetUsersResponse> GetUsers(string? emailFilter)
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
                return new GetUsersResponse(UserModel.GetUsers(emailFilter, executer_user), ResponseSuccessMessage.Success, language);
            }
            catch (Exception e)
            {
                Log.Error("GetUsers endpoint - Error - optional args - " + emailFilter + " - " + e);
                return new GetUsersResponse(ResponseErrorMessage.InternalError, language);
            }
        }

        [HttpGet]
        [Route("departments")]
        [RequestLimit]
        [ValidateReferrer]
        [AuthorizeAdmin]
        public ActionResult<GetDepartmentsResponse> GetDepartments(string? userEmail)
        {
            string language = this.Request.Headers["client-lang"];
            if (string.IsNullOrEmpty(language))
            {
                language = ConfigManager.defaultLanguage;
            }
            string token = this.Request.Headers["Authorization"];
            string executer_user = UserModel.GetUserByToken(token);
            if (userEmail.IsNullOrEmpty())
            {
                try
                {
                    return new GetDepartmentsResponse(DepartmentModel.GetDepartments(executer_user), ResponseSuccessMessage.Success, language);
                }
                catch (Exception e)
                {
                    Log.Error("GetUsers endpoint - Error - optional args - " + userEmail + " - " + e);
                    return new GetDepartmentsResponse(ResponseErrorMessage.InternalError, language);
                }
            }
            else
            {
                try
                {
                    return new GetDepartmentsResponse(DepartmentModel.GetUserDepartments(userEmail!, executer_user), ResponseSuccessMessage.Success, language);
                }
                catch (Exception e)
                {
                    Log.Error("GetUsers endpoint - Error - optional args - " + userEmail + " - " + e);
                    return new GetDepartmentsResponse(ResponseErrorMessage.InternalError, language);
                }
            }
        }

        [HttpPost]
        [Route("addUser")]
        [RequestLimit]
        [ValidateReferrer]
        [Authorize]
        public ActionResult<GenericResponse> AddUser(AddUserRequest input)
        {
            string language = this.Request.Headers["client-lang"];
            if (string.IsNullOrEmpty(language))
            {
                language = ConfigManager.defaultLanguage;
            }

            if (!input.Validate())
            {
                return new GenericResponse(ResponseErrorMessage.InvalidArgs, language);
            }

            try
            {
                string token = this.Request.Headers["Authorization"];
                string executer_user = UserModel.GetUserByToken(token);
                if (!UserModel.AddUser(input, language, executer_user))
                {
                    Log.Error("AddUser endpoint - A problem occurred registing user - " + input.email);
                    return new GenericResponse(ResponseErrorMessage.UnableToRegist, language);
                }

                return new GenericResponse(ResponseSuccessMessage.Success, language);
            }
            catch (UserExistsException e)
            {
                Log.Warning("AddUser endpoint - User already exists - " + input.email);
                return new GenericResponse(ResponseErrorMessage.UserExists, language);
            }
            catch (Exception e)
            {
                Log.Error("AddUser endpoint - Error - " + input.email + " " + e.ToString());
                return new GenericResponse(ResponseErrorMessage.InternalError, language);
            }
        }

        [HttpPost]
        [Route("removeUser")]
        [RequestLimit]
        [ValidateReferrer]
        [AuthorizeAdmin]
        public ActionResult<GenericResponse> RemoveUser(RemoveUserRequest input)
        {
            string language = this.Request.Headers["client-lang"];
            if (string.IsNullOrEmpty(language))
            {
                language = ConfigManager.defaultLanguage;
            }

            if (!input.Validate())
            {
                return new GenericResponse(ResponseErrorMessage.InvalidArgs, language);
            }

            try
            {
                string token = this.Request.Headers["Authorization"];
                string executer_user = UserModel.GetUserByToken(token);
                if (!UserModel.RemoveUser(input, executer_user))
                {
                    Log.Error("RemoveUser endpoint - A problem occurred removing user - " + input.email);
                    return new GenericResponse(ResponseErrorMessage.UnableToRemoveUser, language);
                }

                return new GenericResponse(ResponseSuccessMessage.Success, language);
            }
            catch (DeleteAdminException e)
            {
                Log.Warning("RemoveUser endpoint - Can't delete user admin - " + input.email);
                return new GenericResponse(ResponseErrorMessage.UnableToRemoveAdmin, language);
            }
            catch (UserNotFoundException e)
            {
                Log.Warning("RemoveUser endpoint - User not found - " + input.email);
                return new GenericResponse(ResponseErrorMessage.UserNotExists, language);
            }
            catch (Exception e)
            {
                Log.Error("RemoveUser endpoint - Error - " + input.email + " " + e.ToString());
                return new GenericResponse(ResponseErrorMessage.InternalError, language);
            }
        }

        [HttpPost]
        [Route("updateUser")]
        [RequestLimit]
        [ValidateReferrer]
        [AuthorizeAdmin]
        public ActionResult<GenericResponse> UpdateUser(UpdateUserRequest input)
        {
            string language = this.Request.Headers["client-lang"];
            if (string.IsNullOrEmpty(language))
            {
                language = ConfigManager.defaultLanguage;
            }

            if (!input.Validate())
            {
                return new GenericResponse(ResponseErrorMessage.InvalidArgs, language);
            }

            try
            {
                string token = this.Request.Headers["Authorization"];
                string executer_user = UserModel.GetUserByToken(token);
                if (!UserModel.UpdateUser(input, executer_user, this.HttpContext.Items["User"].ToString()))
                {
                    Log.Error("UpdateUser endpoint - A problem occurred updating user - " + input.email);
                    return new GenericResponse(ResponseErrorMessage.UnableToUpdateUser, language);
                }

                return new GenericResponse(ResponseSuccessMessage.Success, language);
            }
            catch (EqualsRecentHistoryPassException e)
            {
                Log.Warning("UpdateUser endpoint - New pass equals to recent one - " + input.email);
                return new GenericResponse(ResponseErrorMessage.EqualsRecentHistoryPass, language);
            }
            catch (UpdateAdminException e)
            {
                Log.Warning("UpdateUser endpoint - Can't change user admin settings - " + input.email);
                return new GenericResponse(ResponseErrorMessage.UnableToUpdateAdmin, language);
            }
            catch (UserNotFoundException e)
            {
                Log.Warning("UpdateUser endpoint - User not found - " + input.email);
                return new GenericResponse(ResponseErrorMessage.UserNotExists, language);
            }
            catch (Exception e)
            {
                Log.Error("UpdateUser endpoint - Error - " + input.email + " " + e.ToString());
                return new GenericResponse(ResponseErrorMessage.InternalError, language);
            }
        }

        [HttpPost]
        [Route("sendCredentials")]
        [RequestLimit]
        [ValidateReferrer]
        [AuthorizeAdmin]
        public ActionResult<GenericResponse> SendCredentials(SendCredentialsRequest input)
        {
            string language = this.Request.Headers["client-lang"];
            if (string.IsNullOrEmpty(language))
            {
                language = ConfigManager.defaultLanguage;
            }

            if (!input.Validate())
            {
                return new GenericResponse(ResponseErrorMessage.InvalidArgs, language);
            }

            try
            {
                if (!UserModel.SendUserCredentials(input))
                {
                    Log.Error("SendUserCredentials endpoint - A problem occurred sending credentials to user - " + input.email);
                    return new GenericResponse(ResponseErrorMessage.UnableToUpdateUser, language);
                }

                return new GenericResponse(ResponseSuccessMessage.Success, language);
            }
            catch (UserNotFoundException e)
            {
                Log.Warning("SendUserCredentials endpoint - User not found - " + input.email);
                return new GenericResponse(ResponseErrorMessage.UserNotExists, language);
            }
            catch (Exception e)
            {
                Log.Error("SendUserCredentials endpoint - Error - " + input.email + " " + e.ToString());
                return new GenericResponse(ResponseErrorMessage.InternalError, language);
            }
        }

        [HttpPost]
        [Route("updatePass")]
        [RequestLimit]
        [ValidateReferrer]
        public ActionResult<GenericResponse> UpdatePass(UpdatePasswordRequest input)
        {
            string language = this.Request.Headers["client-lang"];
            if (string.IsNullOrEmpty(language))
            {
                language = ConfigManager.defaultLanguage;
            }

            if (!input.Validate())
            {
                return new GenericResponse(ResponseErrorMessage.InvalidArgs, language);
            }

            try
            {
                string token = this.Request.Headers["Authorization"];
                string executer_user = UserModel.GetUserByToken(token);
                if (!UserModel.UpdatePass(input, executer_user))
                {
                    Log.Error("UpdatePassword endpoint - A problem occurred updating password - " + input.email);
                    return new GenericResponse(ResponseErrorMessage.UnableToUpdatePassword, language);
                }

                return new GenericResponse(ResponseSuccessMessage.Success, language);
            }
            catch (EqualsRecentHistoryPassException e)
            {
                Log.Warning("UpdatePassword endpoint - New pass equals recent one - " + input.email);
                return new GenericResponse(ResponseErrorMessage.EqualsRecentHistoryPass, language);
            }
            catch (UpdatePasswordException e)
            {
                Log.Warning("UpdatePassword endpoint - New pass equals to current one - " + input.email);
                return new GenericResponse(ResponseErrorMessage.EqualsCurrentPass, language);
            }
            catch (PasswordNotEqualToOriginalException e)
            {
                Log.Warning("UpdatePassword endpoint - Old password not equal to password on the Database for user - " + input.email);
                return new GenericResponse(ResponseErrorMessage.PassNotEqualtoDB, language);
            }
            catch (UserNotFoundException e)
            {
                Log.Warning("UpdatePassword endpoint - User not found - " + input.email);
                return new GenericResponse(ResponseErrorMessage.UserNotExists, language);
            }
            catch (Exception e)
            {
                Log.Error("UpdatePassword endpoint - Error - " + input.email + " " + e.ToString());
                return new GenericResponse(ResponseErrorMessage.InternalError, language);
            }
        }
    }
}
