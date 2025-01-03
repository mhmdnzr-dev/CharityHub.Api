using System.Security.Claims;
using System.Text;

using CharityHub.Core.Application.Services.Donations;
using CharityHub.Core.Contract.Donations.Interfaces.Repositories;
using CharityHub.Core.Contract.Donations.Interfaces.Services;
using CharityHub.Core.Domain.Entities.Identity;
using CharityHub.Infra.Identity.Interfaces;
using CharityHub.Infra.Identity.Services;
using CharityHub.Infra.Sql.Data.DbContexts;
using CharityHub.Infra.Sql.Repositories.Donations;

using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;

using Serilog;
using Serilog.Sinks.MSSqlServer;

namespace CharityHub.Endpoints.DependencyInjection;

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
        var configuration = services.BuildServiceProvider().GetService<IConfiguration>();

        // Register Identity services
        services.AddIdentity<ApplicationUser, ApplicationRole>()
            .AddEntityFrameworkStores<CharityHubCommandDbContext>()
            .AddDefaultTokenProviders();


        // Register policies for access control
        services.AddAuthorization(options =>
        {
            options.AddPolicy("CanViewDonations", policy =>
                policy.RequireRole("Admin", "Manager"));
        });

        // Add authentication and authorization
        services.AddAuthentication(options =>
        {
            options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
            options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
        })
        .AddJwtBearer(options =>
        {
            options.TokenValidationParameters = new TokenValidationParameters
            {
                ValidateIssuer = true,
                ValidateAudience = true,
                ValidateLifetime = true,
                ValidateIssuerSigningKey = true,
                ValidIssuer = configuration["Jwt:Issuer"],
                ValidAudience = configuration["Jwt:Audience"],
                IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(configuration["Jwt:Key"]))
            };
            options.Events = new JwtBearerEvents
            {
                OnTokenValidated = context =>
                {
                    var userClaims = context.Principal.Claims;
                    // Example: Extract roles or custom claims
                    var roles = userClaims.Where(c => c.Type == ClaimTypes.Role).Select(c => c.Value);
                    var customClaim = userClaims.FirstOrDefault(c => c.Type == "CustomClaimType")?.Value;

                    // Perform additional validation or processing if necessary
                    return Task.CompletedTask;
                }
            };
        });
    }

    public static void AddCustomServices(this IServiceCollection services)
    {
        services.AddTransient<IDonationApplicationService, DonationApplicationService>();
        services.AddTransient<IDonationCommandRepository, DonationCommandRepository>();
        services.AddTransient<IDonationQueryRepository, DonationQueryRepository>();


        services.AddScoped<IIdentityService, IdentityService>();
        services.AddScoped<ITokenService, TokenService>();
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
