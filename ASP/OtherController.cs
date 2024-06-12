using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;

namespace ASP;

[ApiController]
[Route("[controller]")]
public class OtherController(ILogger<OtherController> logger, IOptions<ExampleOptions> options) : ControllerBase
{
    private readonly ILogger<OtherController> _logger = logger;
    private readonly ExampleOptions _options = options.Value;

    [HttpGet]
    [Route("Options")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    public ActionResult<ExampleOptions> GetOptions()
    {
        _logger.LogDebug(301, "Returning all options: {options}", _options);
        return Ok(_options);
    }

    [HttpGet]
    [Route("Log")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    public ActionResult<ExampleOptions> Log()
    {
        //Using scopes you can add additional information to the log message
        using (_logger.BeginScope(new List<KeyValuePair<string, object>> { new("TransactionId", "value") }))
        {
            //Events can be used to associates a set of log messages with a specific event
            _logger.LogInformation(302, "This is a test log message with scope2");
        }
        return Ok();
    }
}
