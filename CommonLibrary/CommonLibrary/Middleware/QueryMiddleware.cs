using CommonLibrary.Handlers;
using CommonLibrary.Models;
using CommonLibrary.Services;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Http.Extensions;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using Newtonsoft.Json.Schema;
using NLog;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;

namespace CommonLibrary.Middleware
{
    /// <summary>
    /// Query middleware allows us to store all api calls in our database
    /// </summary>
    public class QueryMiddleware
    {
        private readonly RequestDelegate _next;

        private static Logger logger = LogManager.GetCurrentClassLogger();


        public QueryMiddleware(RequestDelegate next)
        {
            _next = next;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="httpContext"></param>
        /// <param name="databaseContext"></param>
        /// <param name="correlationIdAccessor"></param>
        /// <param name="resourceIdAccessor"></param>
        /// <param name="testingAccessor"></param>
        /// <returns></returns>
        public async Task Invoke(
            HttpContext httpContext,
            ICorrelationIdAccessor correlationIdAccessor)
        {
            // Generate the query
            if (FilterUnwanted(httpContext))
            {
                string phase = "Request";
                ApiCall newQuery = await GenerateQuery(httpContext, correlationIdAccessor);

                try
                {
                    // Add it to the database
                }
                catch (Exception ex)
                {
                    string message;
                    if (ex is JsonReaderException)
                    {
                        httpContext.Response.StatusCode = StatusCodes.Status400BadRequest;
                        message = $"{phase} has an incorrect Json Format: {ex.Message}";
                    }
                    else
                    {
                        httpContext.Response.StatusCode = StatusCodes.Status500InternalServerError;
                        message = ex.Message;
                    }

                    // Write to http context the uuid
                    byte[] data = Encoding.UTF8.GetBytes(message);
                    httpContext.Response.ContentType = "text/plain";
                    await httpContext.Response.Body.WriteAsync(data, 0, data.Length);
                }

                // Obtain the response and save the query
                try
                {

                    phase = "Response";
                    string response = await ObtainQueryResponse(httpContext);
                    // Add it to the db
                }
                catch (Exception ex)
                {
                    logger.Error($"Error when handling response: {ex} \r\n {httpContext.Request.GetDisplayUrl()} \r\n {ex.StackTrace}");
                }
            } else
            {
                // If its unwanted just use it and leave
                await this._next(httpContext);
            }
            GC.Collect();
        }

        private bool FilterUnwanted(HttpContext httpContext)
        {
            if (httpContext.Request.GetDisplayUrl().ToLower().Contains("swagger"))
            {
                return false;
            }
            return true;
        }


        private async Task<string> ObtainQueryResponse(HttpContext httpContext)
        {
            string response = "";

            //replace the context response with our buffer
            var buffer = new MemoryStream();
            var stream = httpContext.Response.Body;
            httpContext.Response.Body = buffer;
            httpContext.Response.Body.Seek(0, SeekOrigin.Begin);

            await this._next(httpContext);

            //reset the buffer and read out the contents
            buffer.Seek(0, SeekOrigin.Begin);
            using (var bufferReader = new StreamReader(buffer))
            {

                string body = "";
                if (bufferReader.BaseStream.Length < 5000000)
                {
                    body = await bufferReader.ReadToEndAsync();

                    //reset to start of stream
                    buffer.Seek(0, SeekOrigin.Begin);

                    //copy our content to the original stream and put it back
                    await buffer.CopyToAsync(stream);
                    httpContext.Response.Body = stream;
                    response = JsonResponseFilterHandler.Filter(JsonValidate(body, httpContext.Response.ContentType));
                } else
                {
                    response = "{ \"message\":\"Response too long to store\"}";
                }
            }
            buffer.Dispose();
            stream.Dispose();
            return response;
        }


        private async Task<ApiCall> GenerateQuery(HttpContext httpContext, ICorrelationIdAccessor correlationIdAccessor)
        {
            // Obtain the clients Id
            int? clientId = null;
            List<Claim> clientIdClaim = httpContext.User.Claims
                .Where(c => c.Type == "Id").ToList();

            if (clientIdClaim.Count > 0)
            {
                clientId = Convert.ToInt32(clientIdClaim[0].Value);
            }

            // Obtain company ID
            int? companyId = null;
            List<Claim> companyIdClaim = httpContext.User.Claims
                .Where(c => c.Type == "CompanyId").ToList();

            if (companyIdClaim.Count > 0)
            {
                companyId = Convert.ToInt32(companyIdClaim[0].Value);
            } else
            {
                //companyId = await GetCompanyIdFromAPIKey(httpContext, databaseContext);
            }

            //  Enable seeking
            httpContext.Request.EnableBuffering();
            //  Read the stream as text
            string bodyAsText = await new System.IO.StreamReader(httpContext.Request.Body).ReadToEndAsync();
            if (bodyAsText == "" && httpContext.Request.Query.Count > 0) bodyAsText = QueryToJson(httpContext.Request.Query);

            //  Set the position of the stream to 0 to enable rereading
            httpContext.Request.Body.Position = 0;

            string ip = httpContext.Connection.RemoteIpAddress.ToString();
            ApiCall call = new ApiCall()
            {
                IpAddress = ip,
                CorrelationId = correlationIdAccessor.GetCorrelationId(),
                Url = httpContext.Request.GetDisplayUrl(),
                Request = JsonValidate(bodyAsText, httpContext.Request.ContentType),
                ClientId = clientId,
                CompanyId = companyId,
                Type = httpContext.Request.Method,
                Status = "Awaiting",
                CreatedDate = DateTime.Now
            };
            return call;
        }

        private async Task<int?> GetCompanyIdFromAPIKey(HttpContext context)
        {
            /*// obtains the headerName (X-ApiKey) from the headers and obtains the merchant info from it
            IHeaderDictionary headers = context.Request.Headers;
            if (!headers.TryGetValue(CustomHeaderConst.ApiKey, out var apiKeyHeaderValue) ||
                !headers.TryGetValue(CustomHeaderConst.ApiSecret, out var apiSecretHeaderValue))
            {
                return null;
            }

           // Check if api key is correct
           ApiKey apikey = await databaseContext.ApiKey.FirstOrDefaultAsync(u => u.ApiKey1 == (string)apiKeyHeaderValue || u.TestApiKey == (string)apiKeyHeaderValue);
           if (apikey == null)
           {
               return null;
           }

           // Get the company and check to see if they are either live or testing
           Company company = await databaseContext.Company.FirstOrDefaultAsync(u => u.Id == Convert.ToInt32(apikey.CompanyId));
           if (company.StatusId != AccountStatusConst.Live && company.StatusId != AccountStatusConst.Testing)
           {
               return null;
           }
           return company.Id;
           */
            return null;
        }

        private string QueryToJson(IQueryCollection query)
        {
            string json = "{";
            foreach (string key in query.Keys)
            {
                json += $"\"{key}\":\"{query[key]}\",";
            }
            json = json.Substring(0, json.Length - 1) + "}";
            return json;
        }

        public static string JsonValidate(string body, string contentType)
        {
            try
            {
                if (contentType != null && contentType.ToLower().Contains("multipart/form-data"))
                body = JsonConvert.SerializeObject(new MultiFormObject { multiformData = true });


                object? bodyJObject = JsonConvert.DeserializeObject(body);
                if (body == "")
                {
                    return "{}";
                }
                else
                {
                    if (body.ToLower().Contains("password") || body.ToLower().Contains("accesstoken")) return "{\"ContainsPassword\":\"True\"}";
                    return body;
                }
            } catch (Exception ex)
            {
                logger.Warn($"Body '{body}' isn't a json. With content type {contentType}: {ex}");
                return JsonConvert.SerializeObject(new ErroredJsonResponse(body));
            }
        }
    }


    public class MultiFormObject
    {
        public bool multiformData { get; set; }
    }


    public class ErroredJsonResponse
    {
        public string ErroredJson { get; set; }

        public ErroredJsonResponse(string body)
        {
            this.ErroredJson = body;
        }
    }
}