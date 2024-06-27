using System.Net.Http;
using System.Threading;

var builder = WebApplication.CreateBuilder(args);

// Add service defaults & Aspire components.
builder.AddServiceDefaults();

// Add services to the container.
builder.Services.AddProblemDetails();
//
builder.Services.AddHttpClient<WeatherApiClient>(client =>
{
    string url = Environment.GetEnvironmentVariable("services__ContainerService__http__0")!;
    client.BaseAddress = new Uri(url);
});

var app = builder.Build();
app.UseExceptionHandler();

app.MapGet("/weatherforecast", async (WeatherApiClient service) =>
{
    return await service.GetAsync();
});

app.MapDefaultEndpoints();
app.Run();

public record WeatherForecast(DateOnly Date, int TemperatureC, string? Summary)
{
    public int TemperatureF => 32 + (int)(TemperatureC / 0.5556);
}

public class WeatherApiClient(HttpClient httpClient)
{
    public async Task<WeatherForecast[]> GetAsync(int maxItems = 10, CancellationToken cancellationToken = default)
    {
        List<WeatherForecast>? forecasts = null;

        await foreach (var forecast in httpClient.GetFromJsonAsAsyncEnumerable<WeatherForecast>("/weatherforecast", cancellationToken))
        {
            if (forecasts?.Count >= maxItems)
            {
                break;
            }
            if (forecast is not null)
            {
                forecasts ??= [];
                forecasts.Add(forecast);
            }
        }

        return forecasts?.ToArray() ?? [];
    }
}