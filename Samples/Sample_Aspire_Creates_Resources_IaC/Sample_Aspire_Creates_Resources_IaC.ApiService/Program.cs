using Azure.Storage.Blobs;
using Microsoft.Extensions.Azure;

var builder = WebApplication.CreateBuilder(args);

// Add service defaults & Aspire components.
builder.AddServiceDefaults();

// Add services to the container.
builder.Services.AddProblemDetails();

#region Code for Demo

// dotnet add package Aspire.Azure.Storage.Blobs
// Add Azure Blob Storage client.
builder.AddAzureBlobClient("blobs");

#endregion

builder.Services.AddSingleton<ExampleService>();

var app = builder.Build();

// Configure the HTTP request pipeline.
app.UseExceptionHandler();

#region Code for Demo

app.MapGet("/blobs", (ExampleService service) =>
{
    return service.GetBlobs();
    
});

#endregion

app.MapDefaultEndpoints();

app.Run();

#region Code for Demo

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

        return blobs?.Select(x=>x.Name).ToList()!;
    }
}

#endregion