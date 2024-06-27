using Azure.Storage.Blobs;

var builder = WebApplication.CreateBuilder(args);

// Add service defaults & Aspire components.
builder.AddServiceDefaults();

// Add services to the container.
builder.Services.AddProblemDetails();

// dotnet add package Aspire.Azure.Storage.Blobs
builder.AddAzureBlobClient("blobs");

builder.Services.AddSingleton<ExampleService>();

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

app.MapGet("/blobs", (ExampleService service) =>
{
    return service.GetBlobs();
});

app.MapDefaultEndpoints();

app.Run();

record WeatherForecast(DateOnly Date, int TemperatureC, string? Summary)
{
    public int TemperatureF => 32 + (int)(TemperatureC / 0.5556);
}


public class ExampleService(BlobServiceClient blobServiceClient)
{
    public List<string?> GetBlobs()
    {
        var queueClient = blobServiceClient.GetBlobContainerClient("demo1");
        queueClient.CreateIfNotExists();

        var blobs = queueClient.GetBlobs();

        if (blobs == null)
        {
            return null;
        }

        return blobs?.Select(x => x.Name).ToList()!;
    }
}