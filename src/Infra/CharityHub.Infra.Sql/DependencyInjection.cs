using CharityHub.Core.Contract.Primitives.Repositories;
using CharityHub.Infra.Sql.Data.DbContexts;
using CharityHub.Infra.Sql.Premitives;

using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace CharityHub.Infra.Sql;

public static class DependencyInjection
{
    public static void AddInfra(this IServiceCollection services)
    {
        var configuration = services.BuildServiceProvider().GetService<IConfiguration>();

        if (configuration == null)
        {
            throw new ArgumentNullException(nameof(configuration));
        }

        // Register the query-side DbContext (for read operations) without migrations
        services.AddDbContext<CharityHubQueryDbContext>(options =>
            options.UseSqlServer(configuration.GetConnectionString("QueryConnectionString")));

        // Register the command-side DbContext (for write operations)
        services.AddDbContext<CharityHubCommandDbContext>(options =>
            options.UseSqlServer(configuration.GetConnectionString("CommandConnectionString")));

        services.AddScoped(typeof(ICommandRepository<>), typeof(CommandRepository<>));
        services.AddScoped(typeof(IQueryRepository<>), typeof(QueryRepository<>));
        services.AddScoped<IUnitOfWork, UnitOfWork>();

        // Migrate the command-side DbContext
        using var serviceProvider = services.BuildServiceProvider();
        using var commandContext = serviceProvider.GetRequiredService<CharityHubCommandDbContext>();
        commandContext.Database.Migrate();
    }
}

