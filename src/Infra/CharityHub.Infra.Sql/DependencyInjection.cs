using CharityHub.Core.Contract.Primitives.Repositories;
using CharityHub.Infra.Sql.Data.DbContexts;

using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace CharityHub.Infra.Sql;

using Primitives;

public static class DependencyInjection
{
    public static void AddSql(this IServiceCollection services)
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

       
    }
}

