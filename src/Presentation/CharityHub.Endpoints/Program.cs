namespace CharityHub.Endpoints;

using Infra.Sql.Data.DbContexts;
using Infra.Sql.Data.SeedData;
using Infra.Sql.Extensions;
using Presentation;
using Presentation.Middleware;

using Microsoft.AspNetCore.Mvc.ApiExplorer;
using Microsoft.Extensions.FileProviders;

using Serilog;

public class Program
{
    public static async Task Main(string[] args)
    {
        var builder = WebApplication.CreateBuilder(args);

        builder.Services.AddHttpClient();
        Log.Logger = new LoggerConfiguration()
            .ReadFrom.Configuration(builder.Configuration)
            .CreateLogger();
        builder.Host.UseSerilog();

        builder.Services.AddMemoryCache();
        builder.Services.AddCustomServices(builder.Configuration);
        builder.Services.AddSeeder<DatabaseSeeder>();

        var uploadDirectory = builder.Configuration["FileSettings:UploadDirectory"] ?? "uploads";
        uploadDirectory = uploadDirectory.TrimStart('/');

        var staticFilesPath = Path.Combine(Directory.GetCurrentDirectory(), uploadDirectory);

        builder.Services.Configure<StaticFileMiddlewareOptions>(options =>
        {
            options.StaticFilePath = staticFilesPath;
            options.RequestPath = $"/{uploadDirectory}";
        });

        var app = builder.Build();

        await ApplyMigrationsAndSeedData(app);

        ConfigureSwagger(app);

        ConfigureMiddleware(app, staticFilesPath, uploadDirectory);

        app.MapControllers();

        await app.RunAsync();
    }

    private static async Task ApplyMigrationsAndSeedData(WebApplication app)
    {
        await app.Services.SeedAsync<CharityHubCommandDbContext>();
    }

    private static void ConfigureSwagger(WebApplication app)
    {
        var isDevMode = app.Environment.IsDevelopment();
        if (isDevMode)
        {
            app.UseSwagger();
            var provider = app.Services.GetRequiredService<IApiVersionDescriptionProvider>();

            app.UseSwaggerUI(options =>
            {
                foreach (var description in provider.ApiVersionDescriptions)
                {
                    options.SwaggerEndpoint($"/swagger/{description.GroupName}/swagger.json",
                        $"API {description.GroupName.ToUpper()}");
                }
            });
        }
    }

    private static void ConfigureMiddleware(WebApplication app, string staticFilesPath, string uploadDirectory)
    {
        app.UseHttpsRedirection();
        app.UseCors("CorsPolicy");
        app.UseOutputCache();
        app.UseRouting();
        app.UseAuthentication();

        if (!Directory.Exists(staticFilesPath))
        {
            Directory.CreateDirectory(staticFilesPath);
        }

        app.UseStaticFiles(new StaticFileOptions
        {
            FileProvider = new PhysicalFileProvider(staticFilesPath),
            RequestPath = $"/{uploadDirectory}",
            ServeUnknownFileTypes = true,
            DefaultContentType = "application/octet-stream"
        });

        app.UseStaticFileResponseMiddleware();
        app.UseAuthorization();
        app.TokenValidationMiddleware();
        app.UsePagedDataResponseMiddleware();
        app.UseBaseResponseMiddleware();
        app.UseExceptionResponseMiddleware();
    }
}