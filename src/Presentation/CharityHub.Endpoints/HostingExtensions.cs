using CharityHub.Core.Application;
using CharityHub.Core.Contract;
using CharityHub.Infra.Identity;
using CharityHub.Infra.Sql;
using CharityHub.Presentation;

using Serilog;
using Serilog.Sinks.MSSqlServer;

namespace CharityHub.Endpoints;

using System.Reflection;

using Infra.FileManager;

public static class HostingExtensions
{
    public static void AddCustomServices(this IServiceCollection services,IConfiguration configuration)
    {
        services.AddHttpContextAccessor(); // Correct way to register IHttpContextAccessor

        services.AddDotnetOutputCache();
        services.AddVersion();
        services.AddCORSPolicy();
        services.AddControllers();
        services.AddSwagger();

        services.AddFileManager();
        services.AddSql();
        services.AddIdentity();

        services.AddContract(configuration); // Pass IConfiguration here

        services.AddApplication();
    }

}