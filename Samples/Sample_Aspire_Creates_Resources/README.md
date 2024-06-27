# Demonstrating

Dependent resources are created by .NET Aspire.  Shows KV being used as configuration provider and as secret client.

# Prerequisities

- Connected Services -> Azure Resource Provioning Settings

  This is needed so resources can be created in the correct sub/rg. Stored in User Secrets.

  ```powershell
  dotnet add package Azure.Provisioning.KeyVault --version 0.2.s
  dotnet add package Aspire.Hosting.Azure.KeyVault --version 8.0.1

  # add roleAssignment of User security principal (or sami if on azure VM) to Key Vault with role (Key Vault Secrets User)
  az role assignment create --assignee <service-principal-id> --role "Key Vault Secrets User" --scope /subscriptions/<subscription-id>/resourceGroups/<resource-group-name>
  ```

# Notes

- When run you'll see Compiling templates...

- KV is created and reference passed to apiservice

- Shows KV being used as both Configuration provider AND Secrets client

- Show secret-conf endpoint

  Change secret and reference refresh endpoint. Value is old value

- Show secret-client endpoint

  Change secret and reference endpoint. Value is now showing the change to the secret
