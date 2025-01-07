using CharityHub.Core.Application.Configuration.Models;

using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace CharityHub.Core.Application;

public static class DependencyInjection
{
    public static IServiceCollection AddApplication(this IServiceCollection services)
    {
        var configuration = services.BuildServiceProvider().GetService<IConfiguration>();

        // Bind configuration models to use with IOptions<T>
        services.Configure<LoggingOptions>(configuration.GetSection("Logging"));
        services.Configure<JwtOptions>(configuration.GetSection("Jwt"));
        return services;
    }
}
