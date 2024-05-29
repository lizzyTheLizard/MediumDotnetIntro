using Swashbuckle.AspNetCore.Swagger;

namespace ASP
{
    public class ExampleMiddleware(RequestDelegate next, ILogger<ExampleMiddleware> logger)
    {
        private readonly RequestDelegate _next = next;
        private readonly ILogger<ExampleMiddleware> _logger = logger;

        public async Task Invoke(HttpContext httpContext, ISwaggerProvider swaggerProvider)
        {
            _logger.LogInformation("Handling request: {path}", httpContext.Request.Path);
            await _next(httpContext);
            _logger.LogInformation("Finished handling request with code {code}", httpContext.Response.StatusCode);
            return;
        }

    }

}
