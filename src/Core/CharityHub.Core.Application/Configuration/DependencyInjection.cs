using CharityHub.Core.Application.Configuration.Models;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace CharityHub.Core.Application.Configuration;

public static class DependencyInjection
{
    public static IServiceCollection AddApplicationConfiguration(this IServiceCollection services, IConfiguration configuration)
    {
        // Bind configuration models to use with IOptions<T>
        services.Configure<LoggingOptions>(configuration.GetSection("Logging"));

        return services;
    }
}
