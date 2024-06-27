using Azure.Security.KeyVault.Secrets;
using Microsoft.Extensions.Azure;

var builder = WebApplication.CreateBuilder(args);

// Add service defaults & Aspire components.
builder.AddServiceDefaults();

// Add services to the container.
builder.Services.AddProblemDetails();

#region Code for Demo

// Add secrets to configuration
builder.Configuration.AddAzureKeyVaultSecrets("pro-aspire-demos");

// Add client to service collection for DI
builder.AddAzureKeyVaultClient("pro-aspire-demos");

builder.Services.AddSingleton<SecretsConfigurationService>();
builder.Services.AddSingleton<SecretsClientService>();

#endregion

var app = builder.Build();

// Configure the HTTP request pipeline.
app.UseExceptionHandler();

var summaries = new[]
{
    "Freezing", "Bracing", "Chilly", "Cool", "Mild", "Warm", "Balmy", "Hot", "Sweltering", "Scorching"
};

app.MapGet("/weatherforecast", () =>
{
    var forecast = Enumerable.Range(1, 5).Select(index =>
        new WeatherForecast
        (
            DateOnly.FromDateTime(DateTime.Now.AddDays(index)),
            Random.Shared.Next(-20, 55),
            summaries[Random.Shared.Next(summaries.Length)]
        ))
        .ToArray();
    return forecast;
});

#region Code for Demo

app.MapGet("/secret-conf", (SecretsConfigurationService service) =>
{
    var secretValue = service.TryGetSecret("demo1");
    return secretValue ?? "Does not exist";
});

app.MapGet("/secret-client", async (SecretsClientService service) =>
{
    var secretValue = await service.TryGetSecret("demo1");
    return secretValue ?? "Does not exist";
});

#endregion

app.MapDefaultEndpoints();

app.Run();

record WeatherForecast(DateOnly Date, int TemperatureC, string? Summary)
{
    public int TemperatureF => 32 + (int)(TemperatureC / 0.5556);
}

#region Code for Demo

public class SecretsConfigurationService(IConfiguration configuration)
{    
    public string? TryGetSecret(string key)
    {
        var secretValue = configuration[key] ?? null;
        return secretValue;
    }
}

public class SecretsClientService(SecretClient secretClient)
{
    public async Task<string?> TryGetSecret(string key)
    {
        var secretValue = await secretClient.GetSecretAsync(key);

        return secretValue?.Value?.Value!;
    }
}

#endregion