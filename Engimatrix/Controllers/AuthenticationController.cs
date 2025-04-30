// // Copyright (c) 2024 Engibots. All rights reserved.

using Microsoft.AspNetCore.Mvc;
using engimatrix.Config;
using engimatrix.Exceptions;
using engimatrix.Filters;
using engimatrix.ModelObjs;
using engimatrix.Models;
using engimatrix.ResponseMessages;
using engimatrix.Utils;
using engimatrix.Views;

namespace engimatrix.Controllers
{
    /// <summary>
    /// Controller class that handles all authentication and registration requests.
    /// </summary>
    [ApiController]
    [Route("api/auth")]
    public class AuthenticationController : ControllerBase
    {
        /// <summary>
        /// Entry point for registration requests.
        /// </summary>
        /// <param name="input">Request structure - email, name and password.</param>
        /// <returns>Response object with code and message in the language provided in the HTTP request header.</returns>
        [HttpPost]
        [Route("signUp")]
        [RequestLimit]
        [ValidateReferrer]
        public ActionResult<GenericResponse> SignUp(SignUpRequest input)
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
                if (!AuthenticationModel.RegisterUser(input, language))
                {
                    Log.Error("signUp endpoint - A problem occurred registing user - " + input.name + " " + input.email);
                    return new GenericResponse(ResponseErrorMessage.UnableToRegist, language);
                }

                return new GenericResponse(ResponseSuccessMessage.Success, language);
            }
            catch (UserExistsException e)
            {
                Log.Warning("signUp endpoint - User already exists - " + input.name + " " + input.email);
                return new GenericResponse(ResponseErrorMessage.UserExists, language);
            }
            catch (UserPendingException e)
            {
                Log.Warning("signUp endpoint - User already registred - " + input.name + " " + input.email);
                return new GenericResponse(ResponseErrorMessage.UserRegistred, language);
            }
            catch (Exception e)
            {
                Log.Error("signUp endpoint - Error - " + input.name + " " + input.email + " " + e.ToString());
                return new GenericResponse(ResponseErrorMessage.InternalError, language);
            }
        }

        [HttpPost]
        [Route("forgotPassword")]
        [RequestLimit]
        [ValidateReferrer]
        public ActionResult<GenericResponse> ForgotPassowrd(ForgotPassRequest input)
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
                AuthenticationModel.ForgotPass(input, language);
                return new GenericResponse(ResponseSuccessMessage.Success, language);
            }
            catch (Exception e)
            {
                Log.Error("ForgotPassowrd endpoint - Error - " + input.email + " " + e.ToString());
                return new GenericResponse(ResponseErrorMessage.InternalError, language);
            }
        }

        [HttpPost]
        [Route("resetPassword")]
        [RequestLimit]
        [ValidateReferrer]
        public ActionResult<GenericResponse> ResetPassword(ResetPassRequest input)
        {
            string language = this.Request.Headers["client-lang"];
            if (string.IsNullOrEmpty(language))
                language = ConfigManager.defaultLanguage;

            if (!input.Validate())
                return new GenericResponse(ResponseErrorMessage.InvalidArgs, language);

            try
            {
                string token = this.Request.Headers["Authorization"];

                AuthenticationModel.ResetPass(input, true, token == null ? input.email : UserModel.GetUserByToken(token));
                return new GenericResponse(ResponseSuccessMessage.Success, language);
            }
            catch (InvalidArgsResetPassException e)
            {
                Log.Warning("ResetPassowrd endpoint - Invalid or expired token - " + input.email);
                return new GenericResponse(ResponseErrorMessage.InvalidArgsResetPass, language);
            }
            catch (EqualsRecentHistoryPassException e)
            {
                Log.Warning("ResetPassowrd endpoint - New pass equals to recent one - " + input.email);
                return new GenericResponse(ResponseErrorMessage.EqualsRecentHistoryPass, language);
            }
            catch (Exception e)
            {
                Log.Error("ResetPassowrd endpoint - Error - " + input.email + " " + e.ToString());
                return new GenericResponse(ResponseErrorMessage.InternalError, language);
            }
        }

        [HttpPost]
        [Route("signIn")]
        [RequestLimit]
        [ValidateReferrer]
        public ActionResult<SignInResponse> SignIn(SignInRequest input)
        {
            //Create new Guid to serve as 2 factor authentication code with 6 characters
            string auth2f_code = AuthenticationModel.generateNewAuth2fCode();
            bool useAuth2f = ConfigManager.Use2FactorAuth();

            string language = this.Request.Headers["client-lang"];
            if (string.IsNullOrEmpty(language))
            {
                language = ConfigManager.defaultLanguage;
            }

            if (!input.Validate())
            {
                return new SignInResponse(null, null, null, null, ResponseErrorMessage.InvalidArgs, language, useAuth2f);
            }
            try
            {
                UserSimple loginUserResp = AuthenticationModel.LoginUser(input, language);
                if (loginUserResp.expiredPass)
                {
                    return new SignInResponse(null, null, null, null, ResponseErrorMessage.PasswordExpired, language, useAuth2f);
                }

                if (!loginUserResp.IsValid())
                {
                    Log.Error("signIn endpoint - A problem occurred login user - " + input.email);
                    return new SignInResponse(null, null, null, null, ResponseErrorMessage.UnableToLogin, language, useAuth2f);
                }

            
                if (useAuth2f == true)
                {
                    try
                    {
                        if (!AuthenticationModel.AddAuth2FCode(auth2f_code, input.email, language))
                        {
                            Log.Error("Add Auth2F_Code - A problem occurred registing auth2f_code - " + input.email);
                            return new SignInResponse(null, null, null, null, ResponseErrorMessage.InternalError, language, useAuth2f);
                        }

                        return new SignInResponse(loginUserResp.user, input.email, loginUserResp.token, loginUserResp.role, loginUserResp.departments, ResponseSuccessMessage.Success, language, useAuth2f);
                    }
                    catch (Exception e)
                    {
                        Log.Error("Add Auth2F_Code endpoint - Error - " + input.email + " " + e.ToString());
                        return new SignInResponse(null, null, null, null, ResponseErrorMessage.InternalError, language, useAuth2f);
                    }
                }

                return new SignInResponse(loginUserResp.user, input.email, loginUserResp.token, loginUserResp.role, loginUserResp.departments, ResponseSuccessMessage.Success, language, useAuth2f);
            }
            catch (InvalidLoginException e)
            {
                Log.Warning("signIn endpoint - Wrong login - " + input.email);
                return new SignInResponse(null, null, null, null, ResponseErrorMessage.InvalidLogin, language, useAuth2f);
            }
            catch (UserNotFoundException e)
            {
                Log.Warning("signIn endpoint - User not found - " + input.email);
                return new SignInResponse(null, null, null, null, ResponseErrorMessage.InvalidLogin, language, useAuth2f);
            }
            catch (PasswordExpiredException e)
            {
                Log.Warning("signIn endpoint - Password Expired - " + input.email);
                return new SignInResponse(null, null, null, null, ResponseErrorMessage.PasswordExpired, language, useAuth2f);
            }
            catch (Exception e)
            {
                Log.Error("signIn endpoint - Error - " + input.email + " " + e.ToString());
                return new SignInResponse(null, null, null, null, ResponseErrorMessage.InternalError, language, useAuth2f);
            }
        }

        [HttpPost]
        [Route("sendNewAuth2fCode")]
        [RequestLimit]
        [ValidateReferrer]
        public ActionResult<GenericResponse> SendNewAuth2FCode(SendNewAuth2FCodeRequest input)
        {
            //Create new Guid to serve as 2 factor authentication code with 6 characters
            string auth2f_code = AuthenticationModel.generateNewAuth2fCode();

            string language = this.Request.Headers["client-lang"];

            if (string.IsNullOrEmpty(language))
            {
                language = ConfigManager.defaultLanguage;
            }

            try
            {
                // Adds the auth2fCode to DB
                if (!AuthenticationModel.AddAuth2FCode(auth2f_code, input.email, language))
                {
                    Log.Error("Add Auth2F_Code - A problem occurred registing auth2f_code - " + input.email);
                    return new GenericResponse(ResponseErrorMessage.InternalError, language);
                }

                return new GenericResponse(ResponseSuccessMessage.Success, language);
            }
            catch (Exception e)
            {
                Log.Error("Add Auth2F_Code endpoint - Error - " + input.email + " " + e.ToString());
                return new GenericResponse(ResponseErrorMessage.InternalError, language);
            }
        }

        [HttpPost]
        [Route("validateAuth2fcode")]
        [RequestLimit]
        [ValidateReferrer]
        public ActionResult<ValidateAuth2FCodeResponse> ValidateAuth2FCode(ValidateAuth2FCodeRequest input)
        {
            string language = this.Request.Headers["client-lang"];
            if (string.IsNullOrEmpty(language))
            {
                language = ConfigManager.defaultLanguage;
            }

            try
            {
                // Validates auth2fcode
                if (!AuthenticationModel.ValidateAuth2FCode(input.auth2f_code, input.email))
                {
                    Log.Error("Validate Auth2F_Code - A problem occurred getting auth2f_code - " + input.email);
                    return new ValidateAuth2FCodeResponse(input.auth2f_code, ResponseErrorMessage.InternalError, language);
                }

                // If the code is valid, the login is done successfully
                return new ValidateAuth2FCodeResponse(input.auth2f_code, ResponseSuccessMessage.Success, language);
            }
            catch (Exception e)
            {
                Log.Error("Validate Auth2F_Code endpoint - Error - " + input.email + " " + e.ToString());
                return new ValidateAuth2FCodeResponse(input.auth2f_code, ResponseErrorMessage.InternalError, language);
            }
        }

        [HttpPost]
        [Route("signInWithMicrosoft")]
        [RequestLimit]
        [ValidateReferrer]
        public ActionResult<SignInResponse> signInWithMicrosoft(SignInRequest input)
        {
            bool useAuth2f = ConfigManager.Use2FactorAuth();
            string language = this.Request.Headers["client-lang"];
            if (string.IsNullOrEmpty(language))
            {
                language = ConfigManager.defaultLanguage;
            }

            if (!Util.IsValidInputEmail(input.email))
            {
                return new SignInResponse(null, null, null, null, ResponseErrorMessage.InvalidArgs, language, useAuth2f);
            }
            try
            {
                UserSimple loginUserResp = AuthenticationModel.LoginUserWithMicrosoft(input, language);

                if (loginUserResp.expiredPass)
                {
                    return new SignInResponse(null, null, null, null, ResponseErrorMessage.PasswordExpired, language, useAuth2f);
                }

                if (!loginUserResp.IsValid())
                {
                    Log.Error("signInWithMicrosoft endpoint - A problem occurred login user - " + input.email);
                    return new SignInResponse(null, null, null, null, ResponseErrorMessage.UnableToLogin, language, useAuth2f);
                }

                return new SignInResponse(loginUserResp.user, input.email, loginUserResp.token, loginUserResp.role, ResponseSuccessMessage.Success, language, useAuth2f);
            }
            catch (InvalidLoginException e)
            {
                Log.Warning("signInWithMicrosoft endpoint - Wrong login - " + input.email);
                return new SignInResponse(null, null, null, null, ResponseErrorMessage.InvalidLogin, language, useAuth2f);
            }
            catch (UserNotFoundException e)
            {
                Log.Warning("signInWithMicrosoft endpoint - User not found - " + input.email);
                return new SignInResponse(null, null, null, null, ResponseErrorMessage.InvalidLogin, language, useAuth2f);
            }
            catch (Exception e)
            {
                Log.Error("signInWithMicrosoft endpoint - Error - " + input.email + " " + e.ToString());
                return new SignInResponse(null, null, null, null, ResponseErrorMessage.InternalError, language, useAuth2f);
            }
        }

        [HttpPost]
        [Route("refreshAccessToken")]
        [RequestLimit]
        [ValidateReferrer]
        public ActionResult<RefreshTokenResponse> RefreshAcessToken(RefreshTokenRequest input)
        {
            string language = this.Request.Headers["client-lang"];
            if (string.IsNullOrEmpty(language))
            {
                language = ConfigManager.defaultLanguage;
            }
            string user = null;
            int userId = Cryptography.GetUserJwtToken(input.access_token);
            if (userId == 0)
            {
                string msg = "RefreshAcessToken endpoint - FAKE TOKEN!!! " + input.access_token;
                Log.Error(msg);
                // PlatformAlerts.CreatePlatformAlert(msg);
                return new RefreshTokenResponse(null, null, null, null, ResponseErrorMessage.UnableToRefreshToken, language);
            }

            string userEmail = UserModel.GetUserEmailById(userId);

            if (!input.Validate())
            {
                return new RefreshTokenResponse(null, null, null, null, ResponseErrorMessage.InvalidArgs, language);
            }
            string token = this.Request.Headers["Authorization"];
            user = UserModel.GetUserByToken(token);

            try
            {
                UserSimple tokenRenewResp = AuthenticationModel.RefreshAcessToken(userEmail, user);

                if (tokenRenewResp.expiredPass)
                {
                    return new RefreshTokenResponse(tokenRenewResp.user, userEmail, tokenRenewResp.token, tokenRenewResp.role, ResponseErrorMessage.PasswordExpired, language);
                }

                if (!tokenRenewResp.IsValid())
                {
                    Log.Error("RefreshAcessToken endpoint - A problem occurred refreshing token for user - " + userEmail);
                    return new RefreshTokenResponse(null, null, null, null, ResponseErrorMessage.UnableToRefreshToken, language);
                }

                if (ConfigManager.Use2FactorAuth())
                {
                    bool isValidCode = AuthenticationModel.ValidateAuth2FCode(input.a2f_code, userEmail);
                    if (!isValidCode)
                    {
                        Log.Error("RefreshAcessToken endpoint - A problem occurred refreshing token for user using existem A2F code - " + userEmail);
                        return new RefreshTokenResponse(null, null, null, null, ResponseErrorMessage.UnableToRefreshToken, language);
                    }
                }

                return new RefreshTokenResponse(tokenRenewResp.user, userEmail, tokenRenewResp.token, tokenRenewResp.role, tokenRenewResp.departments, ResponseSuccessMessage.Success, language);
            }
            catch (UserNotFoundException e)
            {
                Log.Warning("RefreshAcessToken endpoint - User not found - " + userEmail);
                return new RefreshTokenResponse(null, null, null, null, ResponseErrorMessage.UnableToRefreshToken, language);
            }
            catch (Exception e)
            {
                Log.Error("RefreshAcessToken endpoint - Error - " + userEmail + " " + e.ToString());
                return new RefreshTokenResponse(null, null, null, null, ResponseErrorMessage.InternalError, language);
            }
        }
    }
}
