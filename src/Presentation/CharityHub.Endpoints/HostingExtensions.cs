using Serilog;
using Serilog.Sinks.MSSqlServer;

namespace CharityHub.Endpoints;

public static class HostingExtensions
{
    ILogger
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
                AdditionalColumns =
                [
                    new SqlColumn { ColumnName = "UserName", DataType = System.Data.SqlDbType.NVarChar, DataLength = 100 }
                ]
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
}
