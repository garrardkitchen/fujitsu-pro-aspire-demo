# Demonstrating

This shows .NET Aspire running a custom container.

# Prerequisities

- Docker for Desktop or Podman

- Build image:

```powershell
cd Samples\Sample_Aspire_Custom_Container
docker build -f .\Sample_Aspire_Custom_Container.ContainerService\Dockerfile -t garrardkitchen/proaspiredemo1-customcontainer:0.0.1 .
```

# Notes

- Open api service endpoint

- Change `ENV_EXAMPLE_VALUE` envvar and restart demo

