using CharityHub.Core.Application;
using CharityHub.Core.Contract;
using CharityHub.Infra.Identity;
using CharityHub.Infra.Sql;
using CharityHub.Presentation;

using Serilog;
using Serilog.Sinks.MSSqlServer;

namespace CharityHub.Endpoints.DependencyInjection;

public static class HostingExtensions
{

    public static IHostBuilder AddSerilog(this IHostBuilder builder)
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
                      connectionString: context.Configuration.GetSection("Serilog:WriteTo").GetChildren()
                    .First(x => x.GetValue<string>("Name") == "MSSqlServer")
                    .GetSection("Args").GetValue<string>("connectionString"),
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



    public static void AddCustomServices(this IServiceCollection services)
    {
        services.AddVersion();
        services.AddCORSPolicy();
        services.AddControllers();
        services.AddSwagger();
        services.AddInfra();
        services.AddIdentity();
        services.AddApplication();
        services.AddContract();
    }




}
