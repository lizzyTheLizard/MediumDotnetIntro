using System.Diagnostics.Metrics;

using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Http.Features;

using NUglify.JavaScript.Syntax;

using Swashbuckle.AspNetCore.Swagger;

namespace ASP
{
    public class MetricsTaggingMiddleware(RequestDelegate next, ApiTagMetric apiTagMetric)
    {
        public async Task Invoke(HttpContext httpContext, ISwaggerProvider swaggerProvider)
        {
            apiTagMetric.TagRequest(httpContext.Request);
            await next(httpContext);
            return;
        }
    }
}
