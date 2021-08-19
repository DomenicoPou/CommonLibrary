using Microsoft.AspNetCore.Http;
using NLog;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CommonLibrary.Services;

namespace CommonLibrary.Middleware
{
    /// <summary>
    /// The exception handling middlware helps us debug api exceptions 
    /// without shutting the whole api down
    /// </summary>
    public class ExceptionHandlingMiddleware
    {
        private readonly RequestDelegate _next;

        private static Logger logger = LogManager.GetCurrentClassLogger();

        public ExceptionHandlingMiddleware(RequestDelegate next)
        {
            _next = next;
        }

        /// <summary>
        /// Use the correlation id, and the resource its accessing to help us understand the issue
        /// </summary>
        /// <param name="httpContext"></param>
        /// <param name="correlationIdAccessor"></param>
        /// <param name="resourceIdAccessor"></param>
        /// <returns></returns>
        public async Task Invoke(HttpContext httpContext, 
            ICorrelationIdAccessor correlationIdAccessor)
        {
            // Set the loggin prefix
            string correlationId = correlationIdAccessor.GetCorrelationId();
            string correlationPrefix = $"{correlationId} |";

            logger.Debug($"{correlationPrefix} User IP {GetIP(httpContext)} | Request: {httpContext.Request.Method} {httpContext.Request.Path}");
            try
            {
                await _next(httpContext);
                logger.Debug($"{correlationPrefix} Response | {httpContext.Response.StatusCode}");
            } catch (Exception apiException)
            {
                // Log the error
                logger.Error($"{correlationPrefix} Threw an exception '500' | {apiException.Message} \n Stacktrace : {apiException.StackTrace}");
                try
                {
                    // Write to http context the uuid
                    httpContext.Response.StatusCode = StatusCodes.Status500InternalServerError;
                    var jsonString = $"{{\"error\":\"External Error UUID: {correlationId}\", \"message\":\"The developers will be notified.\"}}";
                    byte[] data = Encoding.UTF8.GetBytes(jsonString);
                    httpContext.Response.ContentType = "text/plain";
                    await httpContext.Response.Body.WriteAsync(data, 0, data.Length);

                } catch (Exception handlerException)
                {
                    // Hope this never happens. However if it does, fix the error strait away.
                    logger.Error($"{correlationPrefix} Exception handler had an error | {handlerException.Message} \n Stacktrace : {handlerException.StackTrace}");
                }
            }
        }

        public static string GetIP(HttpContext httpContext)
        {
            return httpContext.Connection.RemoteIpAddress.ToString();
        }
    }
}
