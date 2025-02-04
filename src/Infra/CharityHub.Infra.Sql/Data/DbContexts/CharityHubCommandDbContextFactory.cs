using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;
using Microsoft.Extensions.Configuration;



namespace CharityHub.Infra.Sql.Data.DbContexts;






public class CharityHubCommandDbContextFactory : IDesignTimeDbContextFactory<CharityHubCommandDbContext>
{
    public CharityHubCommandDbContext CreateDbContext(string[] args)
    {
        // Manually set the correct path for appsettings.json
        var basePath = Path.Combine(Directory.GetCurrentDirectory(), "../../Presentation/CharityHub.Endpoints");
        var environmentName = Environment.GetEnvironmentVariable("DOTNET_ENVIRONMENT") ?? "Development";

        var configuration = new ConfigurationBuilder()
            .SetBasePath(basePath)
            .AddJsonFile("appsettings.json", optional: false)
            .AddJsonFile($"appsettings.{environmentName}.json", optional: true)
            .AddEnvironmentVariables()
            .Build();

        var connectionString = configuration.GetConnectionString("CommandConnectionString");

        if (string.IsNullOrEmpty(connectionString))
        {
            throw new InvalidOperationException("CommandConnectionString is not set in appsettings.json.");
        }

        var optionsBuilder = new DbContextOptionsBuilder<CharityHubCommandDbContext>();
        optionsBuilder.UseSqlServer(connectionString);

        return new CharityHubCommandDbContext(optionsBuilder.Options);
    }
}
