using Microsoft.AspNetCore.Mvc;

namespace Sample_Aspire_Custom_Container.ContainerService.Controllers;

[ApiController]
[Route("[controller]")]
public class WeatherForecastController : ControllerBase
{
    private static readonly string[] Summaries = new[]
    {
        "Freezing", "Bracing", "Chilly", "Cool", "Mild", "Warm", "Balmy", "Hot", "Sweltering", "Scorching"
    };

    private readonly ILogger<WeatherForecastController> _logger;
    private string sampleEnvVar;

    public WeatherForecastController(ILogger<WeatherForecastController> logger)
    {
        _logger = logger;
        sampleEnvVar = Environment.GetEnvironmentVariable("ENV_EXAMPLE_VALUE") ?? "Not Set";
    }

    [HttpGet(Name = "GetWeatherForecast")]
    public IEnumerable<WeatherForecast> Get()
    {
        return Enumerable.Range(1, 5).Select(index => new WeatherForecast
        {
            Date = DateOnly.FromDateTime(DateTime.Now.AddDays(index)),
            TemperatureC = Random.Shared.Next(-20, 55),
            Summary = $"{Summaries[Random.Shared.Next(Summaries.Length)]} - {sampleEnvVar}"
        })
        .ToArray();
    }
}
