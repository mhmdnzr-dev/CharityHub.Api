# This stage is used when running from VS in fast mode (Default for Debug configuration)
FROM mcr.microsoft.com/dotnet/aspnet:9.0 AS base
USER $APP_UID
WORKDIR /app

# This stage is used to build the service project
FROM mcr.microsoft.com/dotnet/sdk:9.0 AS build
ARG BUILD_CONFIGURATION=Release
WORKDIR /src

# Install required workloads (like aspire)
RUN dotnet workload install aspire

# Copy the solution file and restore dependencies
COPY ["CharityHub.sln", "./"]

# Copy all .csproj files for the entire solution
COPY ["src/Presentation/CharityHub.Endpoints/CharityHub.Endpoints.csproj", "src/Presentation/CharityHub.Endpoints/"]
COPY ["src/Core/CharityHub.Core.Application/CharityHub.Core.Application.csproj", "src/Core/CharityHub.Core.Application/"]
COPY ["src/Core/CharityHub.Core.Contract/CharityHub.Core.Contract.csproj", "src/Core/CharityHub.Core.Contract/"]
COPY ["src/Core/CharityHub.Core.Domain/CharityHub.Core.Domain.csproj", "src/Core/CharityHub.Core.Domain/"]
COPY ["src/Core/CharityHub.Core.DomainService/CharityHub.Core.DomainService.csproj", "src/Core/CharityHub.Core.DomainService/"]

COPY ["src/Infra/CharityHub.Infra.Identity/CharityHub.Infra.Identity.csproj", "src/Infra/CharityHub.Infra.Identity/"]
COPY ["src/Infra/CharityHub.Infra.Sql/CharityHub.Infra.Sql.csproj", "src/Infra/CharityHub.Infra.Sql/"]
COPY ["src/Infra/CharityHub.Infra.FileManager/CharityHub.Infra.FileManager.csproj", "src/Infra/CharityHub.Infra.FileManager/"]
COPY ["src/Presentation/CharityHub.Presentation/CharityHub.Presentation.csproj", "src/Presentation/CharityHub.Presentation/"]
COPY ["src/Utils/CharityHub.Utils.Extensions/CharityHub.Utils.Extensions.csproj", "src/Utils/CharityHub.Utils.Extensions/"]



# Copy the test project file
COPY ["test/CharityHub.Tests/CharityHub.Tests.csproj", "test/CharityHub.Tests/"]

# Run the workload restore to ensure that required workloads are installed
RUN dotnet workload restore

# Restore dependencies for the entire solution
RUN dotnet restore "CharityHub.sln" --use-current-runtime

# Copy the entire source code and build
COPY . .
RUN dotnet build "CharityHub.sln" -c $BUILD_CONFIGURATION -o /app/build 

# This stage is used to publish the service project to be copied to the final stage
FROM build AS publish
ARG BUILD_CONFIGURATION=Release
RUN dotnet publish "src/Presentation/CharityHub.Endpoints/CharityHub.Endpoints.csproj" -c $BUILD_CONFIGURATION -o /app/publish /p:UseAppHost=false

# This stage is used in production or when running from VS in regular mode (Default when not using the Debug configuration)
FROM base AS final
WORKDIR /app

# Copy the published application
COPY --from=publish /app/publish .

# Ensure configuration files are copied
COPY --from=build /src/src/Presentation/CharityHub.Endpoints/appsettings.json /app/
COPY --from=build /src/src/Presentation/CharityHub.Endpoints/appsettings.Development.json /app/
COPY --from=build /src/src/Presentation/CharityHub.Endpoints/appsettings.Production.json /app/

# ✅ Copy `uploads` directly from the local machine
COPY src/Presentation/CharityHub.Endpoints/uploads /app/uploads

# ✅ Ensure the folder exists (fixes "File Not Found" issues)
RUN mkdir -p /app/uploads

# Copy the test folder and project into the final image
COPY --from=build /src/test/CharityHub.Tests /test

ENTRYPOINT ["dotnet", "CharityHub.Endpoints.dll"]
