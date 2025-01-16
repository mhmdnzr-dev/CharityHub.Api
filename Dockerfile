# Base image for running the application in production or debug mode
FROM mcr.microsoft.com/dotnet/aspnet:9.0 AS base
WORKDIR /app
EXPOSE 8080
EXPOSE 8081

# Build stage for restoring dependencies and compiling all projects
FROM mcr.microsoft.com/dotnet/sdk:9.0 AS build
ARG BUILD_CONFIGURATION=Release
WORKDIR /src

# Copy solution and all project files to leverage layer caching for dependencies
COPY CharityHub.sln .

COPY src/Presentation/CharityHub.Endpoints/appsettings.json src/Presentation/CharityHub.Endpoints/
COPY src/Presentation/CharityHub.Endpoints/appsettings.Development.json src/Presentation/CharityHub.Endpoints/


COPY CharityHub.AspireHost/CharityHub.AspireHost.csproj CharityHub.AspireHost/

COPY src/Presentation/CharityHub.Endpoints/CharityHub.Endpoints.csproj src/Presentation/CharityHub.Endpoints/

COPY src/Presentation/CharityHub.Presentation/CharityHub.Presentation.csproj src/Presentation/CharityHub.Presentation/




COPY src/Core/CharityHub.Core.Application/CharityHub.Core.Application.csproj src/Core/CharityHub.Core.Application/
COPY src/Core/CharityHub.Core.Contract/CharityHub.Core.Contract.csproj src/Core/CharityHub.Core.Contract/
COPY src/Core/CharityHub.Core.Domain/CharityHub.Core.Domain.csproj src/Core/CharityHub.Core.Domain/
COPY src/Core/CharityHub.Core.DomainService/CharityHub.Core.DomainService.csproj src/Core/CharityHub.Core.DomainService/
COPY src/Core/CharityHub.Core.Presistance/CharityHub.Core.Presistance.csproj src/Core/CharityHub.Core.Presistance/
COPY src/Infra/CharityHub.Infra.Identity/CharityHub.Infra.Identity.csproj src/Infra/CharityHub.Infra.Identity/
COPY src/Infra/CharityHub.Infra.Sql/CharityHub.Infra.Sql.csproj src/Infra/CharityHub.Infra.Sql/

COPY src/Utils/CharityHub.Utils.Extensions/CharityHub.Utils.Extensions.csproj src/Utils/CharityHub.Utils.Extensions/
COPY test/CharityHub.Tests/CharityHub.Tests.csproj test/CharityHub.Tests/


# Restore dependencies for all projects
RUN dotnet restore "CharityHub.AspireHost/CharityHub.AspireHost.csproj"

# Copy all source code into the container
COPY . .

# Build the main project and tests
WORKDIR "/src/CharityHub.AspireHost"
RUN dotnet build "CharityHub.AspireHost.csproj" -c $BUILD_CONFIGURATION -o /app/build

WORKDIR "/src/test/CharityHub.Tests"
