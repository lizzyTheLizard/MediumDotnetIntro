using System;
using System.Net;
using System.Runtime.Intrinsics.X86;

using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Security;

[ApiController]
[Route("[controller]")]
public class WeatherForecastController : ControllerBase
{
    private static readonly string[] Summaries = [
        "Freezing", "Bracing", "Chilly", "Cool", "Mild", "Warm", "Balmy", "Hot", "Sweltering", "Scorching"
    ];

    private readonly ILogger<WeatherForecastController> _logger;
    private readonly IAuthorizationService _authorizationService;

    public WeatherForecastController(ILogger<WeatherForecastController> logger, IAuthorizationService authorizationService)
    {
        _logger = logger;
        _authorizationService = authorizationService;   
    }

    [HttpGet]
    public async Task<IEnumerable<WeatherForecast>> Get()
    {
        //Access user information
        _logger.LogInformation("Get called from {}", User.Identity?.Name);
        var result = new List<WeatherForecast>();
        foreach(var index in Enumerable.Range(1, 5))
        {
            var f = new WeatherForecast
            {
                Date = DateOnly.FromDateTime(DateTime.Now.AddDays(index)),
                TemperatureC = Random.Shared.Next(-20, 55),
                Summary = Summaries[Random.Shared.Next(Summaries.Length)]
            };
            //Use imperative authorization
            var allowed = await _authorizationService.AuthorizeAsync(User, f, new OnlyAdminSeesColdRequirement());
            if(allowed.Succeeded) result.Add(f);
        }
        return result;
    }

    [HttpGet]
    [Route("public")]
    [AllowAnonymous]
    public IEnumerable<WeatherForecast> GetPublic()
    {
        //Only first 2 are public?
        return Enumerable.Range(1, 2).Select(index => new WeatherForecast
        {
            Date = DateOnly.FromDateTime(DateTime.Now.AddDays(index)),
            TemperatureC = Random.Shared.Next(-20, 55),
            Summary = Summaries[Random.Shared.Next(Summaries.Length)]
        })
        .ToArray();
    }

    [FromLocalhost]
    [HttpGet]
    [Route("local")]
    [AllowAnonymous]
    [FromLocalhost]
    public IEnumerable<WeatherForecast> GetLocal()
    {
        return Enumerable.Range(1, 5).Select(index => new WeatherForecast
        {
            Date = DateOnly.FromDateTime(DateTime.Now.AddDays(index)),
            TemperatureC = Random.Shared.Next(-20, 55),
            Summary = Summaries[Random.Shared.Next(Summaries.Length)]
        })
        .ToArray();
    }


}
