# Demonstrating

Dependent resources are created using IaC (bicep).  Lists blobs in a storage account.

# Prerequisities

- Connected Services -> Azure Resource Provioning Settings

  This is needed so resources can be created in the correct sub/rg. Stored in User Secrets.

  ```powershell
  dotnet add package Aspire.Hosting.Azure --version 8.0.1
  ```

# Notes

- When run you'll see Compiling ARM template...

- Storage Account is created and reference passed to apiservice
