FROM mcr.microsoft.com/dotnet/sdk:9.0 AS build
ARG BUILD_CONFIGURATION=Release
WORKDIR /src

# Copy solution and project files
COPY CharityHub.sln ./
COPY src/ ./src/
COPY test/ ./test/

# Restore dependencies
RUN dotnet restore CharityHub.sln

WORKDIR /src/src/Presentation/CharityHub.Endpoints
RUN dotnet publish CharityHub.Endpoints.csproj -c $BUILD_CONFIGURATION -o /app/publish --no-restore

FROM mcr.microsoft.com/dotnet/aspnet:9.0 AS runtime
WORKDIR /app
EXPOSE 80
COPY --from=build /app/publish .
ENTRYPOINT ["dotnet", "CharityHub.Endpoints.dll"]
