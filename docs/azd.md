CD to a sample solution:

```powershellz
cd Samples\Sample_Aspire_Container_Redis
```

**Step 1** - azd init

**Step 2** - azd up

RG: pro-aspire-demos


```
PS C:\Users\garrard\source\demo\Samples\Sample_Aspire_Container_Redis> azd up
? Select an Azure Subscription to use:  3. DevelopmentSub (*********-****-****-****-************)
? Select an Azure location to use: 33. (Europe) UK South (uksouth)

Packaging services (azd package)


Provisioning Azure resources (azd provision)
Provisioning Azure resources can take some time.

Subscription: DevelopmentSub (********-****-****-****-************)
Location: UK South

  You can view detailed progress in the Azure Portal:
  https://portal.azure.com/#view/HubsExtension/DeploymentDetailsBlade/~/overview/id/%2Fsubscriptions%2F*********-****-****-****-************%2Fproviders%2FMicrosoft.Resources%2Fdeployments%2Fpro-aspire-demos-1719320421

  (✓) Done: Resource group: rg-pro-aspire-demos
  (✓) Done: Container Registry: acrjbhxe7qk7vnnc
  (✓) Done: Log Analytics workspace: law-jbhxe7qk7vnnc
  (✓) Done: Container Apps Environment: cae-jbhxe7qk7vnnc

Deploying services (azd deploy)

  (✓) Done: Deploying service apiservice
  - Endpoint: https://apiservice.internal.ashysea-9f080ece.uksouth.azurecontainerapps.io/

  (✓) Done: Deploying service cache
  - Endpoint: https://cache.internal.ashysea-9f080ece.uksouth.azurecontainerapps.io/

  (✓) Done: Deploying service webfrontend
  - Endpoint: https://webfrontend.ashysea-9f080ece.uksouth.azurecontainerapps.io/

  Aspire Dashboard: https://aspire-dashboard.ext.ashysea-9f080ece.uksouth.azurecontainerapps.io

SUCCESS: Your up workflow to provision and deploy to Azure completed in 4 minutes 15 seconds.

```

**Step 3** - add a secret programmatically


```csharp

var secret = builder.AddParameter("MySecret", secret: true);

...

builder.AddProject<Projects.Sample_Aspire_Container_Redis_Web>("webfrontend")
    .WithExternalHttpEndpoints()
    .WithReference(cache)
    .WithReference(apiService)
    .WithEnvironment("env_MySecret", secret);
```

**Step 4** - confirm with iac first

  
```powershell        
azd config set alpha.infraSynth on
azd infra synth
```

**Step 5** - azd up

_Either `azd up` or `azd provision`._

- Delete folders

```powershell
azd config set alpha.infraSynth off
```

You'll now be prompted for the value of `MySecret`.


**Step 6** - azd down

- tear down resources