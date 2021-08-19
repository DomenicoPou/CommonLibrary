using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Http.Extensions;
using NLog;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CommonLibrary.Middleware
{
    /// <summary>
    /// The bot filter middleware stops webcrawlers from using up our resources.
    /// </summary>
    public class BotFilterMiddleware
    {
        private readonly RequestDelegate _next;
        private Logger logger = LogManager.GetCurrentClassLogger();
        public BotFilterMiddleware(RequestDelegate next)
        {
            _next = next;
        }

        /// <summary>
        /// Simply get the list of known api urls known from webcrawlers. Then send them a 418
        /// </summary>
        /// <param name="httpContext"></param>
        /// <returns></returns>
        public async Task Invoke(HttpContext httpContext)
        {
            try
            {
                // Obtain the correlation id header and set if needed
                string fullUrl = httpContext.Request.GetDisplayUrl().ToLower();
                string[] knownBots = new string[] { "cungcap", "phunguyentea", ".env", "robots.txt", ".php" };
                bool isKnownBot = false;
                foreach (string botStr in knownBots)
                {
                    if (fullUrl.Contains(botStr.ToLower()))
                    {
                        isKnownBot = true;
                        break;
                    }
                }

                // If its a known bot or the request isn't even apart of our api. Send im a teapot.
                if (isKnownBot)
                {
                    httpContext.Response.StatusCode = StatusCodes.Status418ImATeapot;
                    var jsonString = $"{{\"message\":\"Bots only desereve teapots\"}}";
                    byte[] data = Encoding.UTF8.GetBytes(jsonString);
                    httpContext.Response.ContentType = "text/plain";
                    await httpContext.Response.Body.WriteAsync(data, 0, data.Length);
                    return;
                }
            } catch (Exception ex)
            {
                // If an error happened log it and continue
                logger.Error($"Error when handling bot filtering: {ex} \r\n {ex.StackTrace}");
            }
            await this._next(httpContext);
        }
    }
}
