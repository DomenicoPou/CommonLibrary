using Microsoft.AspNetCore.Builder;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CommonLibrary.Middleware
{
    public static class CorrelationIDExtension
    {
        public static IApplicationBuilder UseCorrelationIDMiddleware(
            this IApplicationBuilder builder)
        {
            return builder.UseMiddleware<CorrelationIDMiddleware>();
        }
    }
}
