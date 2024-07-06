# Setup: Create Resource Group for the demos

```powershell
$rg="proaspiredemo-RG"

# get your principal id
$id=(az ad signed-in-user show --query id --output tsv)

# create resource group
az group create --location uksouth --resource-group $rg
```

# To create Storage Account and RBAC to your security principal (inner-loop)

```powershell
# get your principal id
$rg="pro-landl-demo1"
$id=(az ad signed-in-user show --query id --output tsv)

# create resource group
az group create --location uksouth --resource-group $rg

# create SA and roleAssignment
az deployment group create --resource-group $rg  --template-file ignore-infra/main.bicep --parameters principalId=$id principalType=User
```
👆 replace `create` with `what-if` if you want to see what will be created

---

# Add wire up App to Azure resources (outer-loop)

Todo:
- Add redirect URL to App Reg
- Replace AzureId:ClientSecret (if using Azure as IDP)

  ```csharp
  var secret = builder.AddParameter("ClientSecret", secret: true);

  builder.AddProject<Projects.Demo1_WebApp>("webapp").WithExternalHttpEndpoints()
    .WithReference(api)
    .WithEnvironment("AzureAd__ClientSecret", secret);
  ```
  ⚠️ _how does this look in ADO?_
- Assign MI to SA with Storage Table Data Contributor role

# Note

Combine with extand bicep

_not including here in these demos_

```
# rename ignore-infra to infra
# enable alpha feature
azd config set alpha.resourceGroupDeployments on
```

# Generate Bicep from .NET Aspire project model

Click [here](https://learn.microsoft.com/en-us/dotnet/aspire/deployment/azure/aca-deployment-azd-in-depth?tabs=windows#generate-bicep-from-net-aspire-project-model) for more info.

```
azd config set alpha.infraSynth on
azd infra synth
```

# CD (GitHub)

_this does not create a workflow for you_

**Step 1** - Added workflow from template

**Step 2** - Added restore task:

```yaml
- name: Restore        
  run: |
    dotnet workload restore
    shell: pwsh
```

**Step 3** - Create secrets and variables for GH actions

```
mkdir .github/workloads
azd pipeline config --provider github
```

**AZD_INITIAL_ENVIRONMENT_CONFIG** will contain external parameter secret values (e.g. AzureAD__ClientSecret)

---

# bicep resources

AppHost appsettings:

```json
  "Azure": {
    "SubscriptionId": "********redacted********",
    "AllowResourceGroupCreation": true,
    "ResourceGroup": "my-resource-group-RG",
    "Location": "uksouth",
    "CredentialSource": "AzureCli"
  }
```

👆 SubscriptionId should be set in `User Secrets`.

Program.cs:

```csharp 
var storage = builder.AddBicepTemplate(
    name: "storage",
    bicepFile: "../resources/storage.bicep")
    //.WithParameter(AzureBicepResource.KnownParameters.Location)
    .WithParameter("saName", "demo1gpkdevsa4")
    .WithParameter(AzureBicepResource.KnownParameters.PrincipalId)
    .WithParameter(AzureBicepResource.KnownParameters.PrincipalType);
```

👆 .WithParameter(AzureBicepResource.KnownParameters.Location) fails when publishing - value is empty. This is why I've commented it here


---

# NodeJS

```powershell
winget install OpenJS.NodeJS.LTS
```

AppHost:

⚠️ Do not add as project to sln via template.  Treat separately


```powershell
dotnet add package Aspire.Hosting.NodeJs --version 8.0.1
```

```csharp
var nodeApi = builder.AddNpmApp("node-api", "../Demo1.NodeApi", "watch")
    .WithReference(api)
    //.WithReference(cache)
    .WithHttpEndpoint(env: "PORT")
    .WithExternalHttpEndpoints()
    .PublishAsDockerFile();
  
if (builder.Environment.IsDevelopment() && builder.Configuration["DOTNET_LAUNCH_PROFILE"] == "https")
{
    // Disable TLS certificate validation in development, see https://github.com/dotnet/aspire/issues/3324 for more details.
    nodeApi.WithEnvironment("NODE_TLS_REJECT_UNAUTHORIZED", "0");
}
```
