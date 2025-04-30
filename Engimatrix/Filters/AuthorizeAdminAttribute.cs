// // Copyright (c) 2024 Engibots. All rights reserved.

using System.Net;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using engimatrix.Models;
using engimatrix.Utils;

namespace engimatrix.Filters
{
    [AttributeUsage(AttributeTargets.Class | AttributeTargets.Method)]
    public class AuthorizeAdminAttribute : ActionFilterAttribute
    {
        private IConfiguration _configuration;

        public AuthorizeAdminAttribute()
        { }

        public override void OnActionExecuting(ActionExecutingContext context)
        {
            _configuration = (IConfiguration)context.HttpContext.RequestServices.GetService(typeof(IConfiguration));
            base.OnActionExecuting(context);
            if (!IsTokenValidRequest(context.HttpContext.Request, context.HttpContext))
            {
                context.Result = new ContentResult
                {
                    Content = "Invalid authorization header"
                };
                context.HttpContext.Response.StatusCode = (int)HttpStatusCode.Forbidden;
            }

            if (!UserModel.IsUserAdmin((string)context.HttpContext.Items["User"]))
            {
                context.Result = new ContentResult
                {
                    Content = "Operation allowed for admins only"
                };
                context.HttpContext.Response.StatusCode = (int)HttpStatusCode.Forbidden;
            }
        }

        private bool IsTokenValidRequest(HttpRequest request, HttpContext context)
        {
            string token = "";
            if (request.Headers.ContainsKey("Authorization"))
            {
                token = request.Headers["Authorization"];
            }

            if (string.IsNullOrWhiteSpace(token))
                return false;

            string user = null;
            int userId = Cryptography.GetUserJwtToken(token);

            if (userId == 0)
            {
                return false;
            }

            context.Items["User"] = UserModel.GetUserEmailById(userId);
            return true;
        }
    }
}
