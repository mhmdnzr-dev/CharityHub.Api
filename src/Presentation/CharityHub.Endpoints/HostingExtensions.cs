﻿using CharityHub.Core.Application;
using CharityHub.Core.Contract;
using CharityHub.Infra.Identity;
using CharityHub.Infra.Sql;
using CharityHub.Presentation;

using Serilog;
using Serilog.Sinks.MSSqlServer;

namespace CharityHub.Endpoints;

using Infra.FileManager;

using Microsoft.IdentityModel.Tokens;

public static class HostingExtensions
{
    public static void AddCustomServices(this IServiceCollection services, IConfiguration configuration)
    {
        services.AddHttpContextAccessor();
        services.AddDotnetOutputCache();
        services.AddVersion();
        services.AddCORSPolicy(configuration);
        services.AddSwagger();
        services.AddFileManager();
        services.AddSql(configuration);
        services.AddIdentityServices(configuration);
        services.AddContract(configuration);
        services.AddApplication();
    }
}