namespace CharityHub.Infra.FileManager;

using Interfaces;

using Microsoft.Extensions.DependencyInjection;

using Services;

public static class DependencyInjection
{
    public static IServiceCollection AddFileManager(this IServiceCollection services)
    {
        services.AddScoped<IFileManagerService, FileManagerService>();

        return services;
    }
}