using Microsoft.AspNetCore.Mvc;

namespace ASP;

[ApiController]
[Route("[controller]")]
public class WeatherForecastController(ILogger<WeatherForecastController> logger) : ControllerBase
{
    private readonly ILogger<WeatherForecastController> _logger = logger;
    private static readonly List<WeatherForecast> Forecasts = [new WeatherForecast { Date = DateOnly.FromDateTime(DateTime.Now), TemperatureC = 25, Summary = "Hot" }];

    [HttpGet]
    public ActionResult<IEnumerable<WeatherForecast>> GetWeatherForecasts() => Ok(Forecasts);

    [HttpPost]
    [ProducesResponseType(StatusCodes.Status201Created)]
    public ActionResult<WeatherForecast> AddWeatherForcast([FromBody] WeatherForecast forecast) {
        Forecasts.Add(forecast);
        _logger.LogInformation("Added new weather forecast: {0}", forecast);
        _logger.LogInformation("Total forecasts: {0}", Forecasts.Count);
        return CreatedAtAction(nameof(GetWeatherForecasts), forecast);
    }

    [HttpGet("{date}")]
    public ActionResult<WeatherForecast> GetWeatherForecast(DateOnly date) {
        var forecast = Forecasts.FirstOrDefault(f => f.Date == date);
        if (forecast == null) {
            _logger.LogWarning("Forecast with date {0} not found", date);
            return NotFound();
        }
        return Ok(forecast);
    }
}
