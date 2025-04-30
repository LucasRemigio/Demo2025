// // Copyright (c) 2024 Engibots. All rights reserved.

using System.Net;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.Extensions.Caching.Memory;
using engimatrix.Config;

namespace engimatrix.Filters
{
    /// <summary>
    /// Action filter to restrict limit on no. of requests per IP address.
    /// </summary>
    /// <seealso cref="Microsoft.AspNetCore.Mvc.Filters.ActionFilterAttribute" />
    [AttributeUsage(AttributeTargets.Method)]
    public class RequestLimitAttribute : ActionFilterAttribute
    {
        public RequestLimitAttribute()
        { }

        private static MemoryCache Cache
        {
            get;
        } = new MemoryCache(new MemoryCacheOptions());

        public override void OnActionExecuting(ActionExecutingContext context)
        {
            var ipAddress = context.HttpContext.Request.HttpContext.Connection.RemoteIpAddress;
            var memoryCacheKey = ipAddress.ToString();
            Cache.TryGetValue(memoryCacheKey, out int prevReqCount);

            if (prevReqCount >= ConfigManager.DnosNumberRequests())
            {
                context.Result = new ContentResult
                {
                    Content = "Request limit is exceeded. Try again in few seconds.",
                };
                context.HttpContext.Response.StatusCode = (int)HttpStatusCode.TooManyRequests;
            }
            else
            {
                var cacheEntryOptions = new MemoryCacheEntryOptions().SetAbsoluteExpiration(TimeSpan.FromSeconds(ConfigManager.DnosNumberRequestsDiffSeconds()));
                Cache.Set(memoryCacheKey, (prevReqCount + 1), cacheEntryOptions);
            }
        }
    }
}
