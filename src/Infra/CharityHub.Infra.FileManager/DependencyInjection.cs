namespace CharityHub.Infra.FileManager;

using Microsoft.Extensions.DependencyInjection;

public static class DependencyInjection
{
    public static IServiceCollection AddFileManager(this IServiceCollection services)
    {
        services.AddScoped<IFileManagerService, FileManagerService>();

        return services;
    }
}