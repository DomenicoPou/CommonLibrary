using Microsoft.AspNetCore.Builder;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CommonLibrary.Middleware
{
    public static class QueryExtension
    {
        public static IApplicationBuilder UseQueryMiddleware(
            this IApplicationBuilder builder)
        {
            return builder.UseMiddleware<QueryMiddleware>();
        }
    }
}
