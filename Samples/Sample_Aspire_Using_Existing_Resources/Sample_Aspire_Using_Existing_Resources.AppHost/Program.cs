var builder = DistributedApplication.CreateBuilder(args);


// dotnet add package Aspire.Hosting.Azure.Storage
// Connected Services -> Add Resource provisioing Settings
//var blobs = builder.AddAzureStorage("proaspiredemos")
//                   .AddBlobs("blobs");

var apiService = builder.AddProject<Projects.Sample_Aspire_Using_Existing_Resources_ApiService>("apiservice")
    .WithEnvironment("ConnectionStrings__blobs", builder.Configuration["ConnectionStrings:blobs"]);

builder.AddProject<Projects.Sample_Aspire_Using_Existing_Resources_Web>("webfrontend")
    .WithExternalHttpEndpoints()
    .WithReference(apiService);

builder.Build().Run();
