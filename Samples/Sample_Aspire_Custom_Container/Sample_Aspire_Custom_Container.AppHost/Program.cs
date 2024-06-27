using Aspire.Hosting;

var builder = DistributedApplication.CreateBuilder(args);

var containerService = builder
    .AddContainer("ContainerService", "garrardkitchen/proaspiredemo1-customcontainer", "0.0.1")
    .WithHttpEndpoint(targetPort: 8080, name: "http")
    //.WithHttpsEndpoint(targetPort: 8081, name: "https")
    .WithEnvironment("ENV_EXAMPLE_VALUE", "Garrard")    
    .WithExternalHttpEndpoints();

// http://localhost:?/weatherforecast

var endpoint = containerService.GetEndpoint("http");

//var apiService = builder
//    .AddProject<Projects.Sample_Aspire_Custom_Container_ApiService>("apiservice")
//    .WithReference(endpoint);

builder.AddProject<Projects.Sample_Aspire_Custom_Container_Web>("webfrontend")
    .WithExternalHttpEndpoints()
    .WithReference(endpoint);

builder.Build().Run();
