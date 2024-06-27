using Aspire.Hosting;
using Aspire.Hosting.Azure;

var builder = DistributedApplication.CreateBuilder(args);

// dotnet add package Aspire.Hosting.Azure:
// - AzureBicepResource.KnownParameters

var storage = builder.AddBicepTemplate(
    name: "storage",
    bicepFile: "resources/storage.bicep")
    // commented out value is blank when deployed
    //.WithParameter(AzureBicepResource.KnownParameters.Location)
    .WithParameter("saName", "proaspiredemos")
    .WithParameter(AzureBicepResource.KnownParameters.PrincipalId)
    .WithParameter(AzureBicepResource.KnownParameters.PrincipalType);

var endpoint = storage.GetOutput("endpoint");

var apiService = builder.AddProject<Projects.Sample_Aspire_Creates_Resources_IaC_ApiService>("apiservice")
    .WithEnvironment("ConnectionStrings:blobs", endpoint)
    .WithExternalHttpEndpoints();

builder.AddProject<Projects.Sample_Aspire_Creates_Resources_IaC_Web>("webfrontend")
    .WithExternalHttpEndpoints()
    .WithReference(apiService);

builder.Build().Run();
