namespace CharityHub.Infra.Sql.Extensions;

using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

using Data.DbContexts;
using Data.SeedData;

public static class SeederExtensions
{
    public static IServiceCollection AddSeeder<TSeeder>(this IServiceCollection services) 
        where TSeeder : class, ISeeder<CharityHubCommandDbContext>
    {
        services.AddScoped<ISeeder<CharityHubCommandDbContext>, TSeeder>();
        return services;
    }

    public static async Task SeedAsync<TContext>(this IServiceProvider serviceProvider) 
        where TContext : DbContext
    {
        using var scope = serviceProvider.CreateScope();
        var context = scope.ServiceProvider.GetRequiredService<TContext>();
        var seeders = scope.ServiceProvider.GetServices<ISeeder<TContext>>();
        var logger = scope.ServiceProvider.GetRequiredService<ILogger<TContext>>();

        logger.LogInformation("Starting database seeding...");

        foreach (var seeder in seeders)
        {
            await seeder.SeedAsync(context);
        }

        logger.LogInformation("Database seeding completed.");
    }
}
