// // Copyright (c) 2024 Engibots. All rights reserved.

using System.Net;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using engimatrix.Config;

namespace engimatrix.Filters
{
    /// <summary>
    /// ActionFilterAttribute to validate referrer url
    /// </summary>
    /// <seealso cref="Microsoft.AspNetCore.Mvc.Filters.ActionFilterAttribute" />
    [AttributeUsage(AttributeTargets.Method)]
    public sealed class ValidateReferrerAttribute : ActionFilterAttribute
    {
        private IConfiguration _configuration;

        /// <summary>
        /// Initializes a new instance of the <see cref="ValidateReferrerAttribute"/> class.
        /// </summary>
        public ValidateReferrerAttribute()
        { }

        /// <summary>
        /// Called when /[action executing].
        /// </summary>
        /// <param name="context">The action context.</param>
        public override void OnActionExecuting(ActionExecutingContext context)
        {
            _configuration = (IConfiguration)context.HttpContext.RequestServices.GetService(typeof(IConfiguration));
            base.OnActionExecuting(context);
            if (!IsValidRequest(context.HttpContext.Request))
            {
                context.Result = new ContentResult
                {
                    Content = "Invalid referer header"
                };
                context.HttpContext.Response.StatusCode = (int)HttpStatusCode.ExpectationFailed;
            }
        }

        /// <summary>
        /// Determines whether /[is valid request] [the specified request].
        /// </summary>
        /// <param name="request">The request.</param>
        /// <returns>
        /// <c>true</c> if [is valid request] [the specified request]; otherwise, <c>false</c>.
        /// </returns>
        private bool IsValidRequest(HttpRequest request)
        {
            if (!ConfigManager.isProduction)
            {
                return true;
            }

            string referrerURL = "";
            if (request.Headers.ContainsKey("Referer"))
            {
                referrerURL = request.Headers["Referer"];
            }

            if (string.IsNullOrWhiteSpace(referrerURL))
                return false;

            foreach (string adrr in ConfigManager.corsAllowedAddress)
            {
                if (referrerURL.Contains(adrr))
                {
                    return true;
                }
            }

            return false;
        }
    }
}
