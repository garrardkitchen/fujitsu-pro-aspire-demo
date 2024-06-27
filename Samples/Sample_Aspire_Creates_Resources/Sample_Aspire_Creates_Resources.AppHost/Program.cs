using Aspire.Hosting;
using Azure.Provisioning.KeyVaults;

var builder = DistributedApplication.CreateBuilder(args);

var secrets = builder.AddAzureKeyVault("pro-aspire-demos");

var apiService = builder.AddProject<Projects.Sample_Aspire_Creates_Resources_ApiService>("apiservice").WithReference(secrets);

builder.AddProject<Projects.Sample_Aspire_Creates_Resources_Web>("webfrontend")
    .WithExternalHttpEndpoints()
    .WithReference(apiService);

builder.Build().Run();
