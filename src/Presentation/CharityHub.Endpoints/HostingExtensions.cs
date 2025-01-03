using CharityHub.Core.Application.Services.Donations;
using CharityHub.Core.Contract.Donations.Interfaces.Repositories;
using CharityHub.Core.Contract.Donations.Interfaces.Services;
using CharityHub.Core.Domain.Entities.Users;
using CharityHub.Infra.Sql.Data.DbContexts;
using CharityHub.Infra.Sql.Repositories.Donations;

using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

using Serilog;
using Serilog.Sinks.MSSqlServer;

namespace CharityHub.Endpoints;

public static class HostingExtensions
{
    public static void AddCORSPolicy(this IServiceCollection services, IConfiguration configuration)
    {
        var allowedDomains = configuration.GetSection("AllowedOrigins").Get<string[]>();

        // Add CORS policy
        services.AddCors(options =>
        {
            options.AddPolicy("CorsPolicy", policy =>
            {
                // The allowed origins will be resolved at runtime
                policy.WithOrigins(allowedDomains)
                      .AllowAnyHeader()
                      .AllowAnyMethod();
            });
        });
    }
    public static IHostBuilder UseCustomSerilog(this IHostBuilder builder, IConfiguration configuration)
    {
        builder.UseSerilog((context, services, configurationBuilder) =>
        {
            // Configure MSSQL Sink with AdditionalColumns
            var columnOptions = new ColumnOptions
            {
                AdditionalColumns = new List<SqlColumn>
                {
                    new SqlColumn { ColumnName = "UserName", DataType = System.Data.SqlDbType.NVarChar, DataLength = 100 }
                }
            };

            columnOptions.Store.Remove(StandardColumn.Properties);
            columnOptions.Store.Remove(StandardColumn.MessageTemplate);

            configurationBuilder
                .ReadFrom.Configuration(context.Configuration)
                .ReadFrom.Services(services)
                .WriteTo.Console()
                .WriteTo.MSSqlServer(
                    connectionString: context.Configuration.GetConnectionString("DefaultConnection"),
                    sinkOptions: new MSSqlServerSinkOptions
                    {
                        AutoCreateSqlTable = true,
                        TableName = "Logs"
                    },
                    columnOptions: columnOptions,
                    restrictedToMinimumLevel: Serilog.Events.LogEventLevel.Information
                )
                .WriteTo.Elasticsearch(new Serilog.Sinks.Elasticsearch.ElasticsearchSinkOptions(new Uri(context.Configuration["ElasticSearch:Uri"]))
                {
                    AutoRegisterTemplate = true,
                    IndexFormat = "charityhub-logs-{0:yyyy.MM.dd}"
                });
        });

        return builder;
    }

    public static void AddIdentityAuthorization(this IServiceCollection services)
    {
        // Register Identity services
        services.AddIdentity<User, UserRole>()
            .AddEntityFrameworkStores<CharityHubCommandDbContext>()
            .AddDefaultTokenProviders();


        // Register policies for access control
        services.AddAuthorization(options =>
        {
            options.AddPolicy("CanViewDonations", policy =>
                policy.RequireRole("Admin", "Manager"));
        });
    }

    public static void AddCustomServices(this IServiceCollection services)
    {
        services.AddTransient<IDonationApplicationService, DonationApplicationService>();
        services.AddTransient<IDonationCommandRepository, DonationCommandRepository>();
        services.AddTransient<IDonationQueryRepository, DonationQueryRepository>();
    }

    public static void AddDbContext(this IServiceCollection services)
    {
        var configuration = services.BuildServiceProvider().GetService<IConfiguration>();

        if (configuration == null)
        {
            throw new ArgumentNullException(nameof(configuration));
        }


        // Register the command-side DbContext (for write operations)
        services.AddDbContext<CharityHubCommandDbContext>(options =>
            options.UseSqlServer(configuration.GetConnectionString("CommandConnectionString")));

        // Register the query-side DbContext (for read operations)
        services.AddDbContext<CharityHubQueryDbContext>(options =>
            options.UseSqlServer(configuration.GetConnectionString("QueryConnectionString")));

    }
}
