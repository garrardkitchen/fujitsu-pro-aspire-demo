var builder = DistributedApplication.CreateBuilder(args);

var cache = builder.AddRedis("cache");

#region azd demo

//var secret = builder.AddParameter("MySecret", secret: true);

#endregion

var apiService = builder.AddProject<Projects.Sample_Aspire_Container_Redis_ApiService>("apiservice");

builder.AddProject<Projects.Sample_Aspire_Container_Redis_Web>("webfrontend")
    .WithExternalHttpEndpoints()
    .WithReference(cache)
    .WithReference(apiService);

#region azd demo

//.WithEnvironment("env_MySecret", secret);

#endregion

builder.Build().Run();
