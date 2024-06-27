#See https://aka.ms/customizecontainer to learn how to customize your debug container and how Visual Studio uses this Dockerfile to build your images for faster debugging.

FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS base
USER app
WORKDIR /app
EXPOSE 8080
EXPOSE 8081

ENV ENV_EXAMPLE_VALUE=foobaa

FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
ARG BUILD_CONFIGURATION=Release
WORKDIR /src
COPY ["Sample_Aspire_Custom_Container.ContainerService/Sample_Aspire_Custom_Container.ContainerService.csproj", "Sample_Aspire_Custom_Container.ContainerService/"]
RUN dotnet restore "./Sample_Aspire_Custom_Container.ContainerService/Sample_Aspire_Custom_Container.ContainerService.csproj"
COPY . .
WORKDIR "/src/Sample_Aspire_Custom_Container.ContainerService"
RUN dotnet build "./Sample_Aspire_Custom_Container.ContainerService.csproj" -c $BUILD_CONFIGURATION -o /app/build

FROM build AS publish
ARG BUILD_CONFIGURATION=Release
RUN dotnet publish "./Sample_Aspire_Custom_Container.ContainerService.csproj" -c $BUILD_CONFIGURATION -o /app/publish /p:UseAppHost=false

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "Sample_Aspire_Custom_Container.ContainerService.dll"]