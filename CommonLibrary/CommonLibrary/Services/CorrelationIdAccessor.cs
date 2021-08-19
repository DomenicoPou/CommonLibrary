using Microsoft.AspNetCore.Http;
using NLog;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CommonLibrary.Services
{
    public interface ICorrelationIdAccessor
    {
        public string GetCorrelationId();
    }

    public class DefaultCorrelationIdAccessor : ICorrelationIdAccessor
    {
        private Logger logger = LogManager.GetCurrentClassLogger();
        private readonly IHttpContextAccessor _httpContextAccessor;

        public DefaultCorrelationIdAccessor(IHttpContextAccessor httpContextAccessor)
        {
            this._httpContextAccessor = httpContextAccessor;
        }

        string ICorrelationIdAccessor.GetCorrelationId()
        {
            try
            {
                var context = this._httpContextAccessor.HttpContext;
                var result = context?.Items["X-CorrelationId"] as string;
                return result;
            }
            catch (Exception exception)
            {
                logger.Error(exception, "Unable to get original correlation id header");
            }

            return string.Empty;
        }
    }
}
