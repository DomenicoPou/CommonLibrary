using Microsoft.AspNetCore.Http;
using NLog;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CommonLibrary.Middleware
{
    /// <summary>
    /// The correlation Id is used to follow all api calls seperatly by giving them a uuid
    /// </summary>
    public class CorrelationIDMiddleware
    {
        private readonly RequestDelegate _next;

        private static Logger logger = LogManager.GetCurrentClassLogger();

        public CorrelationIDMiddleware(RequestDelegate next)
        {
            _next = next;
        }

        public async Task Invoke(HttpContext httpContext)
        {
            // Obtain the correlation id header and set if needed
            var header = httpContext.Request.Headers["X-CorrelationId"];

            // If the correlation id doesn't exist. Create a new one
            string correlationId;
            if (header.Count > 0)
            {
                correlationId = header[0];
            }
            else
            {
                correlationId = Guid.NewGuid().ToString();
            }


            // Set the correlation id
            httpContext.Items["X-CorrelationId"] = correlationId;
            await this._next(httpContext);
        }
    }
}
