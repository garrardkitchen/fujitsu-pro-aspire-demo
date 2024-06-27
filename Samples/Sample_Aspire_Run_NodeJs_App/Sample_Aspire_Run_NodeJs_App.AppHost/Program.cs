using Google.Protobuf.WellKnownTypes;
using Microsoft.Extensions.Hosting;

var builder = DistributedApplication.CreateBuilder(args);

var apiService = builder.AddProject<Projects.Sample_Aspire_Run_NodeJs_App_ApiService>("apiservice");

builder.AddProject<Projects.Sample_Aspire_Run_NodeJs_App_Web>("webfrontend")
    .WithExternalHttpEndpoints()
    .WithReference(apiService);

// dotnet add package Aspire.Hosting.NodeJs
var nodeApi = builder.AddNpmApp("node-api", "../Sample_Aspire_Run_NodeJs_App.NodeApi", "watch")
    .WithReference(apiService)
    .WithHttpEndpoint(env: "PORT")
    .WithExternalHttpEndpoints()
    .PublishAsDockerFile();

if (builder.Environment.IsDevelopment() && builder.Configuration["DOTNET_LAUNCH_PROFILE"] == "https")
{
    // Disable TLS certificate validation in development, see https://github.com/dotnet/aspire/issues/3324 for more details.
    nodeApi.WithEnvironment("NODE_TLS_REJECT_UNAUTHORIZED", "0");
}

builder.Build().Run();
