using System.Diagnostics.Metrics;

namespace ASP
{
    public class ApiTagMetric
    {
        private readonly ILogger<MetricsTaggingMiddleware> _logger;
        private readonly Counter<int> _apiCounter;

        public ApiTagMetric(ILogger<MetricsTaggingMiddleware> logger, IMeterFactory meterFactory)
        {
            var meter = meterFactory.Create("Example");
            _apiCounter = meter.CreateCounter<int>("example.asp.apicounter");
            _logger = logger;
        }

        public void TagRequest(HttpRequest httpRequest)
        {
            var api = getApi(httpRequest);
            var tag = new KeyValuePair<string, object?>("api", api);
            _logger.LogInformation("API request {api}", api);
            _apiCounter.Add(1, tag);
        }

        private string? getApi(HttpRequest request)
        {
            if (request.Path.StartsWithSegments("/Example"))
            {
                return "Example";
            }
            if (request.Path.StartsWithSegments("/Other"))
            {
                return "Other";
            }
            else
            {
                return null;
            }
        }


    }
}
